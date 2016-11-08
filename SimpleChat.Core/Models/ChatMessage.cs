using System;

namespace SimpleChat.Core.Models
{
    public class ChatMessage
    {
        public ChatMessage()
        {
            Sent = DateTime.Now;
        }

        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime Sent { get; set; }

        public User User { get; set; }
    }
}