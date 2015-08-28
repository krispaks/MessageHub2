using MessageHub.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MessageHub.Web.Controllers
{
    public class UserInfoApiController : ApiController
    {

        // controller for retrieving the info for the user stored in the db (personal info stored during registration)

        public string[] GetUserRealName(string email)
        {
            // query on the db to check the users info
            ApplicationDbContext db = new ApplicationDbContext();
            var currentUser = db.Users.FirstOrDefault(x => x.UserName == email);

            // return the array with the users info
            string[] userInfoArray = new string[] { email, currentUser.Firstname, currentUser.Lastname };
            return userInfoArray;
        }

    }
}