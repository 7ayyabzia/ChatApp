using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ChatApp.Models
{
    public class GroupUser
    {
        public int GroupUserID { get; set; }
        public string Color { get; set; }

        public int GroupID { get; set; }
        public virtual Group Group { get; set; }
        
        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

}