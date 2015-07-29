using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageHub.Web.Models
{
    public class ChatMessageModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }
    }
}