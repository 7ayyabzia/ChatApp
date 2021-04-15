using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChatApp.Models
{
    public class Message
    {
        public int MessageID { get; set; }
        public string MessageText { get; set; }

        public string RecipientType { get; set; } //Individual Message || Group Message
        public string RecipientID { get; set; } //ApplicationUserID if Recipient == "Individual"  || GroupID if Recipient == "Group"

        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }

        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}