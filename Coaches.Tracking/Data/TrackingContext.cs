using Coaches.CommonModels;
using Microsoft.EntityFrameworkCore;



namespace Coaches.Tracking.Data
{
    public class TrackingContext :DbContext
    {
        public TrackingContext(DbContextOptions<TrackingContext> options)
            : base(options)
        {

        }

        public DbSet<TrackingLogEvent> TrackingLogEvent { get; set; }
    }
}
