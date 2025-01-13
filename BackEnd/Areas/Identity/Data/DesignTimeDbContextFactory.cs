using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AccountApplication.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
    {
        public AuthDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<AuthDbContext>();
            var connectionString = configuration.GetConnectionString("AuthDbContextConnection");
            builder.UseSqlServer(connectionString);

            return new AuthDbContext(builder.Options);
        }
    }
}