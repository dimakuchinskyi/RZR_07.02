using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace ClientServiceRazor.Features.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            var connectionString = config.GetConnectionString("DefaultConnection") ?? "Host=localhost;Port=5432;Database=client_service;Username=postgres;Password=postgres";

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
