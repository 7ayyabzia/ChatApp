using ChatApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ChatApp.SignalR
{
    public class ChatHub : Hub
    {
        CurrentConnections _connections = new CurrentConnections();

        public void SendMessage(Message message)
        {
            Clients.Caller.SendMessage(message);
        }
        public override Task OnConnected()
        {
            var userId = Context.User.Identity.GetUserId();
            _connections.AddConnection(userId, Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var userId = Context.User.Identity.GetUserId();
            _connections.RemoveConnection(userId, Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

    }
}