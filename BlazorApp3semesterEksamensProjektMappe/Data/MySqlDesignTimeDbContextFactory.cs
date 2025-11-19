using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BlazorApp3semesterEksamensProjektMappe.Data
{

    
    public class MySqlDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MySqlDbContext>
    {
        public MySqlDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MySqlDbContext>();

            // Her bruger vi MySQL connection string 
            optionsBuilder.UseMySql(
                "database string",
                new MySqlServerVersion(new Version(8, 0, 35))
            );

            return new MySqlDbContext(optionsBuilder.Options);
        }
    }
}