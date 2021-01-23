using Coaches.MainApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Coaches.MainApp.Data
{
    public class CoachesContext : DbContext
    {
        public CoachesContext(DbContextOptions<CoachesContext> options)
            : base(options)
        {

        }

        public DbSet<Coach> Coach { get; set; }

    }
}