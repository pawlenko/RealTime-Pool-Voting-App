using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VotingApp.Models
{
    public class VoteOptions
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Vote")]
        public int VoteId { get; set; }
        public virtual Vote Vote { get; set; }


        public virtual ICollection<Answer> Answers { get; set; }
    }
}
