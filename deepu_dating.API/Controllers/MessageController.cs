using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using deepu_dating.API.DTO;
using deepu_dating.API.Helper;
using deepu_dating.API.Models;
using deepu_dating.API.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace deepu_dating.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMessageRepository messageRepository;
        private readonly IMapper mapper;
        public MessageController(IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this.messageRepository = messageRepository;
            this.userRepository = userRepository;

        }
        [Authorize]
        [HttpPost("CreateMessages")]
        public async Task<ActionResult<MessageDto>> CreateMessage([FromBody]createMessage CreateMessage)
        {

            var SenderUserName = User.FindFirst(ClaimTypes.Name)?.Value;

            var Recipient = await userRepository.GetUserByUsernameNondtoAsync(CreateMessage.RecipientUserName);
            var Sender = await userRepository.GetUserByUsernameNondtoAsync(SenderUserName);
            if (Recipient == null) return NotFound();

            if (Recipient.UserName == SenderUserName) return BadRequest("You cannot Message Yourself");

            var message = new Message
            {
                SenderId = Sender.Id,
                SenderUserName = Sender.UserName,
                Sender = Sender,
                RecipientId = Recipient.Id,
                RecipientUserName = Recipient.UserName,
                Recipient = Recipient,
                content = CreateMessage.content

            };

            messageRepository.AddMessage(message);

            if (await messageRepository.SaveAllAsync()) return Ok(mapper.Map<MessageDto>(message));

            return BadRequest("Problem Adding the Message");
          }
          
          [Authorize]
          [HttpGet("getMessageForUser/{container}")]

          public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageForUser(string container){
                var messageParams = new MessageParams();
                messageParams.Container = container;
                messageParams.UserName = User.FindFirst(ClaimTypes.Name)?.Value;
            var returnMessage = await messageRepository.GetMessageForUser(messageParams);
                return Ok(returnMessage);

          }
          [Authorize]
          [HttpGet("getMessageThread/{recipientUsername}")]
          public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string recipientUsername){

              var currentUsername = User.FindFirst(ClaimTypes.Name)?.Value;
              var returnData = await messageRepository.GetMessageThread(currentUsername,recipientUsername);
              
              return Ok(returnData);
          }


    }
}