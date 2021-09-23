using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace snn
{
    public class companyHubDB : DbContext
    {
        public IConfiguration myConfig;
        public companyHubDB(DbContextOptions<companyHubDB> options = null)
            : base(options)
        {
        }
        public DbSet<cc_SnnInsight> cc_SnnInsight { get; set; }
        public DbSet<ref_InsightType> ref_InsightType { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (myConfig != null)
                {
                    optionsBuilder.UseMySql(myConfig.GetConnectionString("companyHubDB"), ServerVersion.AutoDetect(myConfig.GetConnectionString("companyHubDB")));
                }
            }
        }
    }
}
