using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingApp.Data
{
    public class DatabaseInitializer
    {

        public static void SeedData( ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
