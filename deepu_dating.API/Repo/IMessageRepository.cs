using System.Collections.Generic;
using System.Threading.Tasks;
using deepu_dating.API.DTO;
using deepu_dating.API.Helper;
using deepu_dating.API.Models;

namespace deepu_dating.API.Repo
{
    public interface IMessageRepository
    {
         void AddMessage(Message message);

         void DeleteMessage(Message message);
         

         Task<Message> GetMessage(int id);

         Task<IEnumerable<MessageDto>> GetMessageForUser(MessageParams messageParams);

         Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername);

         Task<bool> SaveAllAsync();

         
         
    }
}