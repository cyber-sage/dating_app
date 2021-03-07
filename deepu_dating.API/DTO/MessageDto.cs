using System;
using deepu_dating.API.Models;

namespace deepu_dating.API.DTO
{
    public class MessageDto
    {
        public int Id { get; set; }

        public int SenderId { get; set; }

        public string SenderUserName { get; set; }

       public string SenderPhotoUrl {get; set;}


        public int RecipientId { get; set; }

        public string RecipientUserName { get; set; }

        public string RecipientPhotoUrl { get; set; }

        public string content { get; set; }

        public DateTime? DateRead { get; set; }
        
        public DateTime DateSend { get; set; } 

       
    }
}