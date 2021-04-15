using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApp.SignalR
{
    public class CurrentConnections
    {
        private readonly static ConnectionMapping<string> _connections =
           new ConnectionMapping<string>();

        public void AddConnection(string key, string connection)
        {
            _connections.Add(key, connection);
        }

        public void RemoveConnection(string key, string connection)
        {
            _connections.Remove(key, connection);
        }

        public string[] GetConnections(string key)
        {
            var conns = _connections.GetConnections(key);
            var connections = new string[conns.Count()];
            int i = 0;
            foreach (var item in conns)
            {
                connections[i] = item;
                i++;
            }
            return connections;
        }
    }
}