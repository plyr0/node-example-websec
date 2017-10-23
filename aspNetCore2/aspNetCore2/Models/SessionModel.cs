using System;

namespace aspNetCore2.Models
{
    public class SessionModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public long TimeStamp { get; set; } 
    }
}
