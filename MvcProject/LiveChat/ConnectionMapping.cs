using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BO;
using Entities.Param;
using Entities.Filter;
using Microsoft.AspNet.SignalR;
using Utilities;

namespace MvcProject.LiveChat
{
    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<string>> _connections =
            new Dictionary<T, HashSet<string>>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }
        public Dictionary<string, HashSet<string>> DictConnection
        {
            get
            {
                var dictConnection = new Dictionary<string, HashSet<string>>();
                foreach (var conn in _connections)
                {
                    var strKey = conn.Key.JsonSerialize();
                    var hashValue = conn.Value;
                    dictConnection.Add(strKey, hashValue);
                }
                return dictConnection;
            }
        }

        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }
        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }
        public IEnumerable<string> GetConnectionsByUnsignName(string UnsignName)
        {
            foreach (var conn in _connections)
            {
                if (conn.Key.ChangeType<LiveChatUser>().UnsignName == UnsignName)
                {
                    return conn.Value;
                }
            }

            return Enumerable.Empty<string>();
        }
        public LiveChatUser GetUserByUnsignName(string UnsignName)
        {
            var user = new LiveChatUser();
            foreach (var conn in _connections)
            {
                if (conn.Key.ChangeType<LiveChatUser>().UnsignName == UnsignName)
                {
                    return conn.Key.ChangeType<LiveChatUser>();
                }
            }

            return user;
        }

        public void Remove(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }
        public void RemoveByConnection(string connectionId)
        {
            foreach (var conn in _connections)
            {
                if (conn.Value.Contains(connectionId))
                {
                    conn.Value.Remove(connectionId);
                    if (conn.Value.Count == 0)
                    {
                        _connections.Remove(conn.Key);
                        break;
                    }
                }
            }
        }
    }
    [Serializable]
    public class LiveChatUser
    {
        public string Name { get; set; }
        public string UnsignName { get { return Name.ToKoDauAndGach(); } }
        public string SupportName { get; set; }
        public bool IsSupport { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string PhoneNumber { get; set; }
        public LiveChatUser()
        {

        }
        public LiveChatUser(string Name ="", bool IsSupport = false, string Email = "")
        {
            var guest = Utilities.Helper.CookieStore.GetCookie<LiveChatUser>(Constants.LiveChatGuest);
            if (!string.IsNullOrEmpty(Name))
            {
                var bo = new AspNetUsersBo();
                var param = new AspNetUsersParam();
                var AspNetUsersFilter = new AspNetUsersFilter() { LockoutEnabled = false , UserName = Name };
                param.AspNetUsersFilter = AspNetUsersFilter;
                bo.Search(param);
                this.Avatar = "/api/thumb?w=500&h=500&path=/Image/Admin/default-avatar.jpg";
                if (param.AspNetUsersEntitys != null && param.AspNetUsersEntitys.Any())
                {
                    var user = param.AspNetUsersEntitys.FirstOrDefault();
                    this.Name = Name;
                    this.SupportName = user.Name;
                    this.Avatar = !string.IsNullOrEmpty(user.Avatar) ? user.Avatar.ChangeThumbSize(500, 500) : this.Avatar;
                }
                else
                {
                    this.Name = Name;
                }
                
                this.IsSupport = true;
            }
            else if(guest != null)
            {
                this.Name = guest.Name;
                this.IsSupport = false;
                this.Email = guest.Email;
                this.PhoneNumber = guest?.PhoneNumber;
                this.Avatar = "/api/thumb?w=500&h=500&path=/Image/Admin/default-avatar.jpg";
            }
            else
            {
                this.Email = Email;
                this.Name = Name;
                this.Avatar = "/api/thumb?w=500&h=500&path=/Image/Admin/default-avatar.jpg";
            }
        }
        public override bool Equals(object obj)
        {
            var other = obj as LiveChatUser;
            if (other == null)
                return false;

            return other.Name == Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}