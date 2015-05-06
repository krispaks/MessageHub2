using MessageHub.Lib.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageHub.Lib.DTO;

namespace MessageHub.Lib.Service
{
	public interface IMessageService
	{
		int SaveMessage(Message message);
		IEnumerable<Message> GetMessageList();
		Message GetMessage(int id);
		MessageDetailDTO GetMessageDetail(int id);
	}
}
