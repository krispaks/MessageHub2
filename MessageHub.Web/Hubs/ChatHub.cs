using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.Identity;
using System.Web.Security;

using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using MessageHub.Web.Models;
using System.Threading.Tasks;

namespace MessageHub.Web.Hubs
{

    public static class UserHandler
    {
        //public static HashSet<string> ConnectionName = new HashSet<string>();
        public static HashSet<string[]> ConnectionIds = new HashSet<string[]>();
    }

    public class ChatHub : Hub
    {

        public override Task OnConnected()
        {
            int element = -1;
            // checks if the user is already registered
            for (int i = 0; i < UserHandler.ConnectionIds.Count; i++)
            {
                if (UserHandler.ConnectionIds.ElementAt(i)[0].Equals(Context.User.Identity.Name))
                    element = i;
            }
            // if its not, adds a new row for the user
            if(element == -1)
            {
                UserHandler.ConnectionIds.Add(new string[2] { Context.User.Identity.Name, Context.ConnectionId });

            // if it was, replaces the connection id for the user
            }
            else
            {
                UserHandler.ConnectionIds.ElementAt(element)[1] = Context.ConnectionId;
            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            int element = -1;
            // checks wheres the user in the list
            for (int i = 0; i < UserHandler.ConnectionIds.Count; i++)
            {
                if (UserHandler.ConnectionIds.ElementAt(i)[0].Equals(Context.User.Identity.Name))
                    element = i;
            }

            // changes the status of the connection id to NULL
            if(element != -1)
                UserHandler.ConnectionIds.ElementAt(element)[1] = "NULL";

            GetUsers();

            return base.OnDisconnected(stopCalled);
        }

        // appends the name of each of the users on the client
        public void GetUsers() {
            // resets the list of connected users
            Clients.All.resetUsers();

            // returns the complete list of users
            for (int i = 0; i < UserHandler.ConnectionIds.Count; i++ )
            {
                // returns the info for the user if the status of the connection is not NULL
                if(!UserHandler.ConnectionIds.ElementAt(i)[1].Equals("NULL"))
                    Clients.All.userConnects(UserHandler.ConnectionIds.ElementAt(i)[0], UserHandler.ConnectionIds.ElementAt(i)[1]);
            }
        }

        // creates a private group for two users
        public void CreateGroup(string currentUserId, string toConnectTo)
        {
            string toConnectUserId = "";
            for(int i=0; i<UserHandler.ConnectionIds.Count; i++)
                if(UserHandler.ConnectionIds.ElementAt(i)[0].Equals(toConnectTo))
                    toConnectUserId = UserHandler.ConnectionIds.ElementAt(i)[1];

            string strGroupName = GetUniqueGroupName(currentUserId, toConnectTo);
            if (!string.IsNullOrEmpty(toConnectUserId))
            {
                Groups.Add(Context.ConnectionId, strGroupName);
                Groups.Add(toConnectUserId, strGroupName);
            }
        }

        // generates a unique name for the group between both users
        private string GetUniqueGroupName(string currentUserId, string toConnectTo)
        {
            int hash = String.Concat(String.Concat(toConnectTo, currentUserId).OrderBy(c => c)).GetHashCode();
            return ""+hash;
        }

        // server hub method "send"
        public void Send(string message, string strFrom, string strTo)
        {
            // send the message to both users using the group
            string groupName = GetUniqueGroupName(strFrom, strTo);

            if (Clients != null)
            {
                Clients.Group(groupName).addMessage(message, strFrom, strTo);
            }
        }
    }
}