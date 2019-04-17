using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VotingApp.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        public string IpAdress { get; set; }

        [ForeignKey("VoteOption")]
        public int VoteOptionId { get; set; }
        public virtual VoteOptions VoteOption { get; set; }
    }
}
