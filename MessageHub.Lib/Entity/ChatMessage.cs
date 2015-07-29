using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Lib.Entity
{
    public class ChatMessage : BaseChatEntity
	{
		public ChatMessage() 
		{			
		}

        [Required]
        public string From { get; set; }
        [Required]
        public string To { get; set; }
        [Required]
        public string Content { get; set; }

        public string Time { get; set; }
	}
}
