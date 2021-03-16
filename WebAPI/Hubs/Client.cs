using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Hubs
{
    public class Client
    {
        public Client(string connectionId, string userId, string fullName)
        {
            ConnectionId = connectionId;
            EntranceTime = DateTime.Now;
            UserId = userId;
            FullName = fullName;
        }
        public string ConnectionId { get; }
        public DateTime EntranceTime { get; }
        public DateTime LatestPingTime { get; set; }
        public string UserId { get;  }
        public string FullName { get; }
    }

    public class ClientComparer : IEqualityComparer<Client>
    {
        public bool Equals(Client x, Client y)
        {
            return x.UserId == y.UserId;
        }

        public int GetHashCode([DisallowNull] Client obj) => obj.UserId.GetHashCode();
    }
}
