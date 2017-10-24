using aspNetCore2.Interfaces;
using aspNetCore2.Models;
using System;
using System.Linq;

namespace aspNetCore2.Providers
{
    public class SessionService : ISessionService
    {
        public Guid AddSession(string user)
        {
            KillOldSessions();
            SessionModel sm = new SessionModel()
            {
                TimeStamp = DateTime.Now.Ticks,
                Username = user
            };
            using(var db = new AppDbContext())
            {
                db.Sessions.Add(sm);
                db.SaveChanges();
            }
            return sm.Id;
        }

        public string GetName(string id)
        {
            KillOldSessions();
            Guid gid;
            try
            {
                gid = new Guid(id);
            }
            catch (Exception) 
            {
                return null;
            }
            using (var db = new AppDbContext())
            {
                var current = db.Sessions.FirstOrDefault(s => s.Id == gid);
                return current?.Username;
            }
        }

        public bool IsValid(string id)
        {
            KillOldSessions();
            Guid gid;
            try {
                gid = new Guid(id);
            } catch (Exception) {
                return false;
            }
            using (var db = new AppDbContext())
            {
                return db.Sessions.Any(s => s.Id == gid);
            }
        }

        private void KillOldSessions()
        {
            using (var db = new AppDbContext())
            {
                double timeout = double.Parse(Program.Configuration["sessionTimeout"]);
                var old = db.Sessions.Where(s => s.TimeStamp > DateTime.Now.AddSeconds(timeout).Ticks);
                if (old != null)
                {
                    db.Sessions.RemoveRange(old);
                    db.SaveChanges();
                }
            }
        }
    }
}
