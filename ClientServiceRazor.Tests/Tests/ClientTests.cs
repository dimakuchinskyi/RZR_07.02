using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ClientServiceRazor.Features.Data;
using ClientServiceRazor.Features.Clients.Models;
using ClientServiceRazor.Features.Users.Models;

namespace ClientServiceRazor.Tests
{
    public class ClientTests
    {
        private ApplicationDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public void CreateClient_SavesToDatabase()
        {
            using var context = GetDbContext(Guid.NewGuid().ToString());

            var client = new Client
            {
                Id = Guid.NewGuid(),
                Surname = "Ivanov",
                FirstName = "Ivan",
                Patronymic = "Ivanovich",
                Email = "ivan@example.com",
                BirthDate = new DateTime(1990,1,1),
                CreatedAt = DateTimeOffset.UtcNow
            };

            context.Clients.Add(client);
            context.SaveChanges();

            var saved = context.Clients.First();

            Assert.Equal(client.Surname, saved.Surname);
            Assert.Equal(client.FirstName, saved.FirstName);
            Assert.Equal(client.Patronymic, saved.Patronymic);
            Assert.Equal(client.Email, saved.Email);
            Assert.Equal(client.BirthDate, saved.BirthDate);
        }

        [Fact]
        public void GetAllClients_ReturnsAll()
        {
            using var context = GetDbContext(Guid.NewGuid().ToString());

            context.Clients.AddRange(
                new Client { Id = Guid.NewGuid(), Surname = "S1", FirstName = "F1", CreatedAt = DateTimeOffset.UtcNow },
                new Client { Id = Guid.NewGuid(), Surname = "S2", FirstName = "F2", CreatedAt = DateTimeOffset.UtcNow }
            );
            context.SaveChanges();

            var list = context.Clients.ToList();
            Assert.Equal(2, list.Count);
            Assert.Contains(list, c => c.Surname == "S1");
            Assert.Contains(list, c => c.Surname == "S2");
        }

        [Fact]
        public void DeleteClient_RemovesFromDatabase()
        {
            using var context = GetDbContext(Guid.NewGuid().ToString());
            var client = new Client { Id = Guid.NewGuid(), Surname = "ToDel", CreatedAt = DateTimeOffset.UtcNow };
            context.Clients.Add(client);
            context.SaveChanges();

            context.Clients.Remove(client);
            context.SaveChanges();

            Assert.Empty(context.Clients.ToList());
        }

        [Fact]
        public void UpdateClient_ChangesPersisted()
        {
            using var context = GetDbContext(Guid.NewGuid().ToString());
            var client = new Client { Id = Guid.NewGuid(), Surname = "Old", Email = "old@example.com", CreatedAt = DateTimeOffset.UtcNow };
            context.Clients.Add(client);
            context.SaveChanges();

            client.Surname = "New";
            client.Email = "new@example.com";
            context.Clients.Update(client);
            context.SaveChanges();

            var saved = context.Clients.First();
            Assert.Equal("New", saved.Surname);
            Assert.Equal("new@example.com", saved.Email);
        }

        [Fact]
        public void Client_WithPhones_SavesAndIncludesPhones()
        {
            using var context = GetDbContext(Guid.NewGuid().ToString());
            var client = new Client { Id = Guid.NewGuid(), Surname = "HasPhones", CreatedAt = DateTimeOffset.UtcNow };
            client.Phones.Add(new Phone { Id = Guid.NewGuid(), Number = "+380501234567", CreatedAt = DateTimeOffset.UtcNow });
            client.Phones.Add(new Phone { Id = Guid.NewGuid(), Number = "+380671234567", CreatedAt = DateTimeOffset.UtcNow });

            context.Clients.Add(client);
            context.SaveChanges();

            var saved = context.Clients.Include(c => c.Phones).First(c => c.Id == client.Id);

            Assert.Equal(2, saved.Phones.Count);
            Assert.Contains(saved.Phones, p => p.Number == "+380501234567");
            Assert.Contains(saved.Phones, p => p.Number == "+380671234567");
            foreach (var phone in saved.Phones)
            {
                Assert.Equal(client.Id, phone.ClientId);
            }
        }

        // Нові тести для Address, FinanceAccount, ClientFinanceAccount, User

        [Fact]
        public void Address_Saves_WithCorrectFields()
        {
            using var context = GetDbContext(Guid.NewGuid().ToString());
            var client = new Client { Id = Guid.NewGuid(), Surname = "AClient", CreatedAt = DateTimeOffset.UtcNow };
            var address = new Address
            {
                Id = Guid.NewGuid(),
                Country = "Ukraine",
                Region = "Kyiv",
                Area = null,
                City = "Kyiv",
                Street = "Khreshchatyk",
                Building = "1",
                Apartment = null,
                Entrance = null,
                Room = null,
                CreatedAt = DateTimeOffset.UtcNow
            };
            client.Address = address;
            context.Clients.Add(client);
            context.SaveChanges();

            var saved = context.Clients.Include(c => c.Address).First(c => c.Id == client.Id);
            Assert.NotNull(saved.Address);
            Assert.Equal("Ukraine", saved.Address.Country);
            Assert.Equal("Kyiv", saved.Address.Region);
            Assert.Null(saved.Address.Area);
            Assert.Equal("Khreshchatyk", saved.Address.Street);
            Assert.Equal("1", saved.Address.Building);
        }

        [Fact]
        public void FinanceAccount_ManyToMany_WithClient_Works()
        {
            using var context = GetDbContext(Guid.NewGuid().ToString());
            var client1 = new Client { Id = Guid.NewGuid(), Surname = "C1", CreatedAt = DateTimeOffset.UtcNow };
            var client2 = new Client { Id = Guid.NewGuid(), Surname = "C2", CreatedAt = DateTimeOffset.UtcNow };

            var acc = new FinanceAccount { Id = Guid.NewGuid(), Balance = 100m, CreatedAt = DateTimeOffset.UtcNow };

            client1.ClientFinanceAccounts.Add(new ClientFinanceAccount { Client = client1, FinanceAccount = acc, CreatedAt = DateTimeOffset.UtcNow });
            client2.ClientFinanceAccounts.Add(new ClientFinanceAccount { Client = client2, FinanceAccount = acc, CreatedAt = DateTimeOffset.UtcNow });

            context.Clients.AddRange(client1, client2);
            context.FinanceAccounts.Add(acc);
            context.SaveChanges();

            var savedAcc = context.FinanceAccounts.Include(f => f.ClientFinanceAccounts).ThenInclude(cf => cf.Client).First(f => f.Id == acc.Id);
            Assert.Equal(2, savedAcc.ClientFinanceAccounts.Count);
            Assert.Contains(savedAcc.ClientFinanceAccounts, cf => cf.Client.Id == client1.Id);
            Assert.Contains(savedAcc.ClientFinanceAccounts, cf => cf.Client.Id == client2.Id);
        }

        [Fact]
        public void User_Entity_Saves_WithRoleAndStatus()
        {
            using var context = GetDbContext(Guid.NewGuid().ToString());
            var role = new Role { Id = Guid.NewGuid(), Name = "Admin" };
            var status = new Status { Id = Guid.NewGuid(), Name = "Active" };
            var user = new User
            {
                Id = Guid.NewGuid(),
                Login = "user1",
                Password = "passhash",
                Email = "user1@example.com",
                Role = role,
                Status = status,
                CreatedAt = DateTimeOffset.UtcNow
            };

            context.Roles.Add(role);
            context.Statuses.Add(status);
            context.Users.Add(user);
            context.SaveChanges();

            var saved = context.Users.Include(u => u.Role).Include(u => u.Status).First(u => u.Id == user.Id);
            Assert.Equal("user1", saved.Login);
            Assert.Equal("passhash", saved.Password);
            Assert.Equal("user1@example.com", saved.Email);
            Assert.Equal("Admin", saved.Role.Name);
            Assert.Equal("Active", saved.Status.Name);
        }
    }
}
