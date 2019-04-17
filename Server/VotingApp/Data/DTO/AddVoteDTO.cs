using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingApp.Data.DTO
{

    public class VoteOptions
    {
        public string Name { get; set; }
    }


    public class AddVoteDTO
    {
        public string Name { get; set; }
        public ICollection<VoteOptions> VoteOptions { get; set; }
    }
}
