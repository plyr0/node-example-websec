using System;

namespace aspNetCore2.Models
{
    public class TwitModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
    }
}
