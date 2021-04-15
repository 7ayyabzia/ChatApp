using ChatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.ViewModels
{
    public class ChatContactViewModel
    {
        public string UserID { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<Group> Groups { get; set; }
    }
}