using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingApp.Models;

namespace VotingApp.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {


            base.OnModelCreating(builder);
        }


        public DbSet<Vote> Votes { get; set; }
        public DbSet<VoteOptions> VoteOptions { get; set; }
        public DbSet<Answer> VoteOpionAnswer { get; set; }

    }

}
