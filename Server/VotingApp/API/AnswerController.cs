using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotingApp.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace VotingApp.API
{
    [Produces("application/json")]
    [Route("api/Vote/{voteID:int}/Option/{optionID:int}/Answer")]
    public class AnswerController : Controller
    {
        ApplicationDbContext _context;
        IHttpContextAccessor _accessor;
        IHubContext<Hubs.Vote> _hubContext;


        public AnswerController(ApplicationDbContext context, IHttpContextAccessor accessor, IHubContext<Hubs.Vote> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _accessor = accessor;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromRoute] int voteID, [FromRoute] int optionID)
        {

            var voteExist = _context.Votes.Include(x=>x.Options).FirstOrDefault(x => x.Id == voteID);

            if (voteExist == null)
                return BadRequest(new { result = "This vote not exist" });

            var voteOption = voteExist.Options.FirstOrDefault(x => x.Id == optionID);

            if(voteOption == null)
                return BadRequest( new { result = "Option in that vote not exist" });

            var userIP = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();

            var answerExist = _context.VoteOptions.Where(x => x.Vote == voteExist).Include(x => x.Answers).SelectMany(x => x.Answers).Where(x => x.IpAdress == userIP);

            if(answerExist != null)
            {
                _context.VoteOpionAnswer.RemoveRange(answerExist);
                await _context.SaveChangesAsync();
            }


            var newAnswer = new Models.Answer();

            newAnswer.IpAdress = userIP;
            newAnswer.VoteOption = voteOption;
            newAnswer.VoteOptionId = voteOption.Id;

            await _context.VoteOpionAnswer.AddAsync(newAnswer);
            await _context.SaveChangesAsync();

            var option = new
            {
                Id = voteExist.Id,
                CreateDate = voteExist.CreateDate,
                Name = voteExist.Name,
                Options = voteExist.Options.Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    Answers = _context.VoteOpionAnswer.Where(x => x.VoteOption == c).Count(),
                    Answered =  _context.VoteOpionAnswer.Any(x => x.VoteOption == c && x.IpAdress == userIP)
                })
            };


            await _hubContext.Clients.Group(voteID.ToString()).SendAsync("sendToGroup", option);

           return Ok();

        }


    }
}
