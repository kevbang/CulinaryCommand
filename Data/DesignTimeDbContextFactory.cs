using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CulinaryCommand.Data
{
    // Provides a context instance for design-time commands (dotnet ef ...)
    // Avoids ServerVersion.AutoDetect trying to reach a private RDS during migrations add.
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var conn = Environment.GetEnvironmentVariable("ConnectionStrings__Default")
                      ?? "Server=127.0.0.1;Port=3306;Database=culinary_local;Uid=root;Pwd=localpass;SslMode=None;";

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql(conn, new MySqlServerVersion(new Version(8, 0, 36)))
                .Options;

            return new AppDbContext(options);
        }
    }
}
