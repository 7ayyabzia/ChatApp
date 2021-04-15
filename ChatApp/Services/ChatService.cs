using ChatApp.Models;
using ChatApp.SignalR;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.Services
{
    public class ChatService
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        CurrentConnections _connections = new CurrentConnections();

        public void CreateGroup(Group group)
        {
            _context.Groups.Add(group);
            _context.SaveChanges();
        }

        public IEnumerable<ApplicationUser> GetUsers(string user)
        {
            var users = _context.Users.Where(s=>s.Id != user).ToList();
            return users;
        }

        public IEnumerable<Message> GetUserMessage(string recipientId, string userId)
        {
            var myMessages = _context.Messages
                .Where(s => s.RecipientType == "Individual" && s.RecipientID == recipientId && s.UserID == userId).ToList();
            var friendMessages = _context.Messages
                .Where(s => s.RecipientType == "Individual" && s.RecipientID == userId && s.UserID == recipientId).ToList();

            var allMessages = myMessages.Concat(friendMessages).OrderBy(s=>s.TimeStamp);
            return allMessages;
        }

        public IEnumerable<Message> GetGroupMessage(int id)
        {
            var messages = _context.Messages.Where(s => s.RecipientType == "Group" && s.RecipientID == id.ToString()).ToList().OrderBy(s => s.TimeStamp);
            return messages;
        }

        public void SendMessage(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();

            message.User = _context.Users.Find(message.UserID);

            if (message.RecipientType == "Individual") BroadcastMessageToIndividual(message);
            if (message.RecipientType == "Group") BroadcastMessageToGroup(message);
        }
        
        private void BroadcastMessageToIndividual(Message message)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            var connections = _connections.GetConnections(message.RecipientID);

            var _message = JsonConvert.SerializeObject(message, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            hub.Clients.Clients(connections).receiveNotification(_message);
            hub.Clients.Clients(connections).receiveMessage(_message);
        }
        private void BroadcastMessageToGroup(Message message)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            var userIDs = _context.GroupUsers.Where(s => s.GroupID.ToString() == message.RecipientID && s.UserID != message.UserID).Select(s => s.UserID).ToList();
            var connections = new List<string>();
            foreach (var id in userIDs)
            {
                connections.AddRange(_connections.GetConnections(id));
            }

            var _message = JsonConvert.SerializeObject(message, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            hub.Clients.Clients(connections).receiveNotification(_message);
            hub.Clients.Clients(connections).receiveMessage(_message);
        }
    }
}