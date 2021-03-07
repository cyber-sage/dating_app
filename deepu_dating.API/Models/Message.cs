using System;

namespace deepu_dating.API.Models
{
    public class Message
    {
        public int Id { get; set; }

        public int SenderId { get; set; }

        public string SenderUserName { get; set; }

        public userData Sender { get; set; }


        public int RecipientId { get; set; }

        public string RecipientUserName { get; set; }

        public userData Recipient { get; set; }

        public string content { get; set; }

        public DateTime? DateRead { get; set; }
        
        public DateTime DateSend { get; set; } = DateTime.Now;

        public bool SenderDeleted { get; set; } 

        public bool RecipientDeleted { get; set; }
    }
}