//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;

//namespace Danplanner.Data
//{
//    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
//    {
//        public AppDbContext CreateDbContext(string[] args)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
//            optionsBuilder.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
//            ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnection")));

//            return new AppDbContext(optionsBuilder.Options);
//        }
//    }
//}

