using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VotingApp.Models
{
    public class Vote
    {

        public Vote()
        {
            CreateDate = DateTime.UtcNow;
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<VoteOptions> Options { get; set; }


    }
}
