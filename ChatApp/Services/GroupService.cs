using ChatApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChatApp.Services
{
    public class GroupService
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        
        public Group GetGroup(int id)
        {
            var group = _context.Groups.SingleOrDefault(s => s.GroupID == id);
            group.GroupUsers = _context.GroupUsers.Where(s => s.GroupID == group.GroupID).ToList();
            return group;
        }

        public IEnumerable<Group> GetGroups(string userId)
        {
            var groups = _context.GroupUsers.Where(s=>s.UserID == userId).Select(s=>s.Group).ToList();
            foreach (var group in groups)
            {
                group.GroupUsers = _context.GroupUsers.Where(s => s.GroupID == group.GroupID).ToList();
            }
            return groups;
        }

        public void AddGroup(Group group, string userId)
        {
            if(group.GroupName == "")
            {
                throw new Exception("Group Name can not be empty");
            }
            if(group.GroupUsers.Count() == 0)
            {
                throw new Exception("Add User in group to continue");
            }
            if (group.GroupUsers.GroupBy(s => s.UserID).Where(s => s.Count() > 1).Count() > 0)
            {
                throw new Exception("Duplicate Contacts are not allowed");
            }
            _context.Groups.Add(group);
            _context.SaveChanges();

            var _groupUsers = new List<GroupUser> { new GroupUser { UserID = userId } };
            _groupUsers.AddRange(group.GroupUsers);

            _groupUsers.ForEach(s => s.GroupID = group.GroupID);
            _context.GroupUsers.AddRange(_groupUsers);
            _context.SaveChanges();
        }
    }
}