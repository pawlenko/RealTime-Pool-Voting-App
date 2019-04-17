using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VotingApp.Data;
using VotingApp.Data.DTO;
using VotingApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace VotingApp.API
{
    [Produces("application/json")]
    [Route("api/Vote")]
    public class VoteController : Controller
    {

        ApplicationDbContext _context;
        IHubContext<Hubs.Vote> _hubContext;
        IHttpContextAccessor _accessor;

        public VoteController(ApplicationDbContext context, IHubContext<Hubs.Vote> hubContext, IHttpContextAccessor accessor)
        {
            _context = context;
            _hubContext = hubContext;
            _accessor = accessor;
        }





        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var temp = _context.Votes.Select(x => new
            {
                Id = x.Id,
                CreateDate = x.CreateDate,
                Name = x.Name
            });

            return Ok(new  { result = temp });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get (int id)
        {
            var userIP = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();

            var temp = _context.Votes.Where(x => x.Id == id).Select(x => new
            {
                Id = x.Id,
                CreateDate = x.CreateDate,
                Name = x.Name,
                Options = x.Options.Select(c => new
                {
                    Id = c.Id,
                    Name = c.Name,
                    Answers = c.Answers.Count,
                   Answered = _context.VoteOpionAnswer.Any(d => d.VoteOption == c && d.IpAdress == userIP)
                }),


            }).FirstOrDefault();


            return Ok(new { result = temp });
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddVoteDTO state)
        {

            if (string.IsNullOrEmpty(state.Name))
                return BadRequest(new { result = "Please type vote name" });

            if(state.VoteOptions == null || state.VoteOptions.Count ==0)
                return BadRequest( new { result = "Please type minimum one vote option" });


            var temp = new Vote();

            temp.Name = state.Name;

            await _context.Votes.AddAsync(temp);


            foreach (var item in state.VoteOptions)
            {
                var newVoteOption = new Models.VoteOptions();
                newVoteOption.Name = item.Name;
                newVoteOption.Vote = temp;
                newVoteOption.VoteId = temp.Id;

                await _context.VoteOptions.AddAsync(newVoteOption);
            }


            await _context.SaveChangesAsync();

            return Ok(new
            {
                Id = temp.Id,
                CreateDate = temp.CreateDate,
                Name = temp.Name
            });

        }

      
    }
}
