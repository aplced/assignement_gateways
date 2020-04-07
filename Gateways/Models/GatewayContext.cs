using Microsoft.EntityFrameworkCore;

namespace Gateways.Models
{
    public class GatewayContext : DbContext
    {
        public GatewayContext(DbContextOptions<GatewayContext> options)
            : base(options)
        {
        }

        public DbSet<Gateway> TodoItems { get; set; }
    }
}