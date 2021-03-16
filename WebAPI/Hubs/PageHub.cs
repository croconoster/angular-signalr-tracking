using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Intrastructure;

namespace WebAPI.Hubs
{
    public class PageHub : Hub
    {
        private readonly IClientList _list;
        private readonly ICurrentUser _currentUser;

        public PageHub(IClientList list, ICurrentUser currentUser)
        {
            _list = list;
            _currentUser = currentUser;
        }
        public override Task OnConnectedAsync()
        {

            _list.CreateUser(Context.ConnectionId, _currentUser.UserId, _currentUser.FullName);
            Heartbeat();
            Clients.All.SendAsync("broadcastUsers", _list.GetClients());
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception ex)
        {
            _list.RemoveUser(Context.ConnectionId);
            Clients.All.SendAsync("broadcastUsers", _list.GetClients());
            return base.OnDisconnectedAsync(ex);
        }

        /// <summary>
        /// Initiate the heartbeat callback.
        /// </summary>
        private void Heartbeat()
        {
            var heartbeat = Context.Features.Get<IConnectionHeartbeatFeature>();

            heartbeat.OnHeartbeat(state => {
                (HttpContext context, string connectionId) = ((HttpContext, string))state;
                IClientList clientList = (IClientList)context.RequestServices.GetService(typeof(IClientList));
                clientList.LatestPing(connectionId);
            }, (Context.GetHttpContext(), Context.ConnectionId));
        }

        public async Task GetUser(string data)
        {
            Console.WriteLine(data);
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastUsers", _list.GetClients());
        }
    }
}
