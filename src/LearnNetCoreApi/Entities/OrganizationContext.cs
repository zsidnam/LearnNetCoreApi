using Microsoft.EntityFrameworkCore;

namespace LearnNetCoreApi.Entities
{
    public class OrganizationContext : DbContext
    {
        public OrganizationContext(DbContextOptions<OrganizationContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<OrgList> OrgLists { get; set; }
        public DbSet<OrgListItem> OrgListItems { get; set; }
    }
}
