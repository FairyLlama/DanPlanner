using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BlazorApp3semesterEksamensProjektMappe.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {

        // This method is called by the EF Core tools to create a DbContext instance at design time
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=CampingDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

            return new AppDbContext(optionsBuilder.Options);
        }

        
    }
}
