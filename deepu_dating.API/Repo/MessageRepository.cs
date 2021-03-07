using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using deepu_dating.API.DTO;
using deepu_dating.API.Helper;
using deepu_dating.API.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace deepu_dating.API.Repo
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public MessageRepository(DataContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;

        }
        public void AddMessage(Message message)
        {
            context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await context.Messages.FindAsync(id);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageForUser(MessageParams messageParams)
        {
            var messages = await context.Messages
            .Include(c => c.Sender).ThenInclude(c=> c.Photos).
            Include(c => c.Recipient).ThenInclude(c=> c.Photos)
            .OrderByDescending(c => c.DateRead).ToListAsync();

            messages = messageParams.Container switch
            {
                "Inbox" => messages.Where(c => c.RecipientUserName == messageParams.UserName).ToList(),
               "Outbox" => messages.Where(c => c.SenderUserName == messageParams.UserName).ToList(),
                _ => messages.Where(c => c.RecipientUserName == messageParams.UserName && c.DateRead == null).ToList()
            };
            return mapper.Map<IEnumerable<MessageDto>>(messages);

        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
        {
           var message = await context.Messages
           .Include(c=>c.Sender).ThenInclude(c=>c.Photos)
           .Include(c=>c.Recipient).ThenInclude(c=>c.Photos).Where(c => c.RecipientUserName==currentUsername && c.SenderUserName==recipientUsername 
            || c.SenderUserName==currentUsername && c.RecipientUserName==recipientUsername )
           .OrderBy(c => c.DateSend).ToListAsync();
            
            // messsage = messsage.Where(c => c.RecipientUserName==currentUsername && c.DateRead==null 
            // || c.SenderUserName==currentUsername && c.RecipientUserName==recipientUsername ).ToList();
           var messageUnread = message.Where(c=>c.DateRead==null && c.RecipientUserName==currentUsername).ToList();


           if(messageUnread.Any()){
               foreach(var item in message){
                   item.DateRead = DateTime.Now;
               }
           }

           await context.SaveChangesAsync();

           return mapper.Map<IEnumerable<MessageDto>>(message);
            
        
        
        }


        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}