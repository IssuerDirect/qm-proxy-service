using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace snn
{
    public class peopleDB : DbContext
    {
        public IConfiguration myConfig;
        public peopleDB(DbContextOptions<peopleDB> options = null)
            : base(options)
        {
        }
        public DbSet<pi_User> pi_User { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (myConfig != null)
                {
                    optionsBuilder.UseMySql(myConfig.GetConnectionString("peopleDB"), ServerVersion.AutoDetect(myConfig.GetConnectionString("peopleDB")));
                }
            }
        }
    }
}
