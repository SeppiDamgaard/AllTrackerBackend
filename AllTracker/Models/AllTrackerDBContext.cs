using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AllTracker.Models
{
    public class AllTrackerDBContext : IdentityDbContext
    {
        public IConfiguration Configuration;
        public IHttpContextAccessor Context;
        public AllTrackerDBContext(DbContextOptions<AllTrackerDBContext> ctxOpts, IConfiguration config, IHttpContextAccessor context) : base(ctxOpts)
        {
            Configuration = config;
            Context = context;
        }
        public DbSet<User> User { get; set; }
        public DbSet<ConsumptionPost> ConsumptionPost { get; set; }
        public DbSet<ConsumptionRegistration> ConsumptionRegistration { get; set; }
    }
}
