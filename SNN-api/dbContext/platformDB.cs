using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace snn
{
    public class platformDB : DbContext
    {
        public IConfiguration myConfig;
        public platformDB(DbContextOptions<platformDB> options = null)
            : base(options)
        {
        }

        public DbSet<snn_Insight> snn_Insight { get; set; }
        public DbSet<ref_InsightType> ref_InsightType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (myConfig != null) {
                    optionsBuilder.UseMySql(myConfig.GetConnectionString("PlatformID"), ServerVersion.AutoDetect(myConfig.GetConnectionString("newsroom")));
                }
            }
        }
        
    }
}
