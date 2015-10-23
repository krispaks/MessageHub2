using MessageHub.Lib.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageHub.Lib.DTO;
using System.IO;
using Raven.Json.Linq;

namespace MessageHub.Lib.Service
{
	public interface IMessageService
	{
		int SaveMessage(Message message);
		IEnumerable<Message> GetMessageList();
		PagedResultDTO<Message> GetPagedMessageList(MessageSearchCriteriaDTO searchCriteria);
		Message GetMessage(int id);
		MessageDetailDTO GetMessageDetail(int id);
        void StoreFiles(Stream uploadStream, string fileName, RavenJObject metadata);
    }
}
