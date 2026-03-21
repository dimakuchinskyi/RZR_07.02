using System;
using Microsoft.EntityFrameworkCore;
using ClientServiceRazor.Features.Clients.Models;
using ClientServiceRazor.Features.Users.Models;

namespace ClientServiceRazor.Features.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<Client> Clients { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<FinanceAccount> FinanceAccounts { get; set; }
        public DbSet<ClientFinanceAccount> ClientFinanceAccounts { get; set; }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Phones)
                .WithOne(p => p.Client)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Client>()
                .HasOne(c => c.Address)
                .WithOne(a => a.Client)
                .HasForeignKey<Address>(a => a.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<ClientFinanceAccount>()
                .HasKey(ca => new { ca.ClientId, ca.FinanceAccountId });

            modelBuilder.Entity<ClientFinanceAccount>()
                .HasOne(ca => ca.Client)
                .WithMany(c => c.ClientFinanceAccounts)
                .HasForeignKey(ca => ca.ClientId);

            modelBuilder.Entity<ClientFinanceAccount>()
                .HasOne(ca => ca.FinanceAccount)
                .WithMany(fa => fa.ClientFinanceAccounts)
                .HasForeignKey(ca => ca.FinanceAccountId);
            
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);
            
            modelBuilder.Entity<Status>()
                .HasMany(s => s.Users)
                .WithOne(u => u.Status)
                .HasForeignKey(u => u.StatusId);
            
            modelBuilder.Entity<Address>(b =>
            {
                b.Property(a => a.Country)
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                b.Property(a => a.Region)
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                b.Property(a => a.Area)
                    .HasColumnType("varchar(100)")
                    .IsRequired(false);

                b.Property(a => a.City)
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                b.Property(a => a.Street)
                    .HasColumnType("varchar(150)")
                    .IsRequired();

                b.Property(a => a.Building)
                    .HasColumnType("varchar(20)")
                    .IsRequired();

                b.Property(a => a.Apartment)
                    .HasColumnType("varchar(20)")
                    .IsRequired(false);

                b.Property(a => a.Entrance)
                    .HasColumnType("varchar(10)")
                    .IsRequired(false);

                b.Property(a => a.Room)
                    .HasColumnType("varchar(20)")
                    .IsRequired(false);
                
                b.Property(a => a.CreatedAt)
                    .HasColumnType("timestamp with time zone");

                b.Property(a => a.UpdatedAt)
                    .HasColumnType("timestamp with time zone");
            });
            
            modelBuilder.Entity<User>(b =>
            {
                b.Property(u => u.Login)
                    .HasColumnType("varchar(50)")
                    .IsRequired();

                b.Property(u => u.Password)
                    .HasColumnType("varchar(255)")
                    .IsRequired();

                b.Property(u => u.Email)
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                b.Property(u => u.CreatedAt)
                    .HasColumnType("timestamp with time zone");

                b.Property(u => u.UpdatedAt)
                    .HasColumnType("timestamp with time zone");

                b.HasIndex(u => u.Login).IsUnique();
            });

            modelBuilder.Entity<Role>(b =>
            {
                b.Property(r => r.Name)
                    .HasColumnType("varchar(50)")
                    .IsRequired();
            });

            modelBuilder.Entity<Status>(b =>
            {
                b.Property(s => s.Name)
                    .HasColumnType("varchar(50)")
                    .IsRequired();
            });
            
            modelBuilder.Entity<Client>(b =>
            {
                b.Property(c => c.Surname)
                    .HasColumnType("varchar(100)")
                    .IsRequired(false);

                b.Property(c => c.FirstName)
                    .HasColumnType("varchar(100)")
                    .IsRequired(false);

                b.Property(c => c.Patronymic)
                    .HasColumnType("varchar(100)")
                    .IsRequired(false);

                b.Property(c => c.Email)
                    .HasColumnType("varchar(320)")
                    .IsRequired(false);

                b.Property(c => c.CreatedAt).HasColumnType("timestamp with time zone");
                b.Property(c => c.UpdatedAt).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<Phone>(b =>
            {
                b.Property(p => p.Number)
                    .HasColumnType("varchar(20)")
                    .IsRequired(false);

                b.Property(p => p.CreatedAt).HasColumnType("timestamp with time zone");
                b.Property(p => p.UpdatedAt).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<FinanceAccount>(b =>
            {
                b.Property(f => f.CreatedAt).HasColumnType("timestamp with time zone");
                b.Property(f => f.UpdatedAt).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<ClientFinanceAccount>(b =>
            {
                b.Property(cf => cf.CreatedAt).HasColumnType("timestamp with time zone");
            });
        }
    }
}
