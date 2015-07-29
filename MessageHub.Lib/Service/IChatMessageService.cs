using MessageHub.Lib.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageHub.Lib.DTO;

namespace MessageHub.Lib.Service
{
	public interface IChatMessageService
	{
        int SaveChatMessage(ChatMessage chatMessage);
        IEnumerable<Entity.ChatMessage> GetChatMessageList(string from, string to);
	}
}
