using aspNetCore2.Interfaces;
using aspNetCore2.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace aspNetCore2.Providers
{
    public class SessionService : ISessionService
    {
        public Guid AddSession(string user)
        {
            SessionModel sm = new SessionModel()
            {
                TimeStamp = DateTime.Now,
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

        public Guid? Login(LoginViewModel model)
        {
            KillOldSessions();
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Password))
            {
                return null;
            }
            string hash = string.Empty;
            using (var algorithm = SHA256.Create())
            {
                byte[] input = Encoding.UTF8.GetBytes(model.Password);
                byte[] output = algorithm.ComputeHash(input);
                hash = Convert.ToBase64String(output);
                System.Diagnostics.Debug.WriteLine(hash);
            }
            if (model.Name == "root" && hash == "jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=")
            {
                return AddSession("root");
            } else
            {
                return null;
            }
        }

        private void KillOldSessions()
        {
            using (var db = new AppDbContext())
            {
                double timeout = double.Parse(Program.Configuration["sessionTimeout"]);
                var old = db.Sessions.Where(s => DateTime.Now > s.TimeStamp.AddSeconds(timeout));
                System.Diagnostics.Debug.WriteLine("SESSIONS old " + old.ToList().Count);
                if (old != null)
                {
                    db.Sessions.RemoveRange(old);
                    db.SaveChanges();
                }
            }
        }
    }
}
