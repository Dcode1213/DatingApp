using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MessagesController : BaseApiController
    {
        private readonly IUserRepository userRepository;
        private readonly IMessageRepository messageRepository;
        private readonly IMapper mapper;

        public MessagesController(IUserRepository userRepository,IMessageRepository messageRepository,IMapper mapper)
        {
            this.userRepository=userRepository;
            this.messageRepository=messageRepository;
            this.mapper=mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();
            if (username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("You Can Not Send Messages To Yourself");

            var sender = await userRepository.getUserByUserNameAsync(username);
            var recipeint = await userRepository.getUserByUserNameAsync(createMessageDto.RecipientUsername);

            if (recipeint == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipeint,
                SenderUserName = sender.UserName,
                RecipientUserName = recipeint.UserName,
                Content = createMessageDto.Content
            };

            messageRepository.AddMessage(message);

            if(await messageRepository.SaveAllAsync()) return Ok(mapper.Map<MessageDto>(message));  
            return BadRequest("Failed To Send Message"); 
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessageForUser([FromQuery]MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();

            var messages = await messageRepository.GetMessageForUser(messageParams);
            Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize,
                messages.TotalCount, messages.TotalPages));

            return messages;
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMesssageThread(string username)
        {
            var curruntUsername = User.GetUsername();
            return Ok(await messageRepository.GetMessageThread(curruntUsername, username));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();
            var message = await messageRepository.GetMessage(id);
            if (message.SenderUserName != username && message.RecipientUserName != username) return Unauthorized();
            
            if(message.SenderUserName == username) message.SenderDeleted = true;
            if (message.RecipientUserName == username) message.RecipientDeleted = true;

            if(message.SenderDeleted && message.RecipientDeleted)
            {
                messageRepository.DeleteMessage(message);
            }
            if (await messageRepository.SaveAllAsync()) return Ok();
            return BadRequest("Problem deleting the message");
        }
    }
}
