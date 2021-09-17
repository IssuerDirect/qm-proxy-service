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
        public DbSet<snn_alerts> snn_alerts { get; set; }
        public DbSet<snn_Insight> snn_Insight { get; set; }
        public DbSet<ref_InsightType> ref_InsightType { get; set; }
        public DbSet<snn_users> snn_users { get; set; }
        public DbSet<ref_Status> ref_Status { get; set; }
        public DbSet<snn_alertsCategrories> snn_alertsCategrories { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (myConfig != null) {
                    optionsBuilder.UseMySql(myConfig.GetConnectionString("PlatformID"), ServerVersion.AutoDetect(myConfig.GetConnectionString("PlatformID")));
                }
            }
        }
        
    }
}
