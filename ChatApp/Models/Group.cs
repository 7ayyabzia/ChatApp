using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Models
{
    public class Group
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        
        public IEnumerable<GroupUser> GroupUsers { get; set; }
    }
}