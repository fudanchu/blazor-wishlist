using Wishlist.Server.Models;
using Wishlist.Shared.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wishlist.Shared.Models.User;

namespace Wishlist.Server.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //seed the database!
            string userName = "JoeB", userEmail = "joe_blow_tester@gmail.com", userId = "400d5b1c-b761-4e2b-b0ef-d9909eefc433";
            string userName2 = "JaneD", userEmail2 = "jane_doe_tester@gmail.com", userId2 = "aad29937-f673-426b-b338-fc5b75926c37";
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Email = userEmail,
                    NormalizedEmail = userEmail.ToUpper(),
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    GroupId = 1,
                    FirstName = "Joe",
                    LastName = "Blow",
                    NickName = "Jman",
                    UserName = userName,
                    NormalizedUserName = userName.ToUpper(),
                    Id = userId,
                    PasswordHash = "AQAAAAEAACcQAAAAELRLao2z9QK1uTSF9MkS3XFL7kGTJ5peolLcnUgufG9kG8KuRtvypjjKxRL6nPvxUw==",
                    SecurityStamp = "4IBP2DQCF5ZMZZAQ2POAGGYHQMDNGT4M",
                    ConcurrencyStamp = "caf86e69-0a26-4f93-b7f5-b8557cf3c49c",
                    ListNote = "This is my example list note.  :)"
                },
                new ApplicationUser
                {
                    Email = userEmail2,
                    NormalizedEmail = userEmail2.ToUpper(),
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    GroupId = 2,
                    FirstName = "Jane",
                    LastName = "Doe",
                    NickName = "",
                    UserName = userName2,
                    NormalizedUserName = userName2.ToUpper(),
                    Id = userId2,
                    PasswordHash = "AQAAAAEAACcQAAAAEApbnFa0lPJ/4EP2Uo60kyLy8ZBcFAezRYiU9soysXiGH9YjCxGcVxB1ugbGxU5/1w==",
                    SecurityStamp = "VJNRLNY67OH36WP3OWRTKNXU2TJK3OBN",
                    ConcurrencyStamp = "642aa26e-a0dd-47de-b145-348e8f6845e7",
                    ListNote = ""
                }
            );
            modelBuilder.Entity<Gift>().HasData(
                new Gift
                {
                    Id = 1,
                    Cost = 39999,
                    Description = "Shiny new wheels!",
                    Name = "New Car",
                    UserAskingId = userId,
                    WebLink = "https://www.tesla.com",
                    Rank = 0
                },
                new Gift
                {
                    Id = 2,
                    Cost = 9,
                    Description = "So warm....",
                    Name = "Socks",
                    UserAskingId = userId,
                    Rank = 1
                },
                new Gift
                {
                    Id = 3,
                    Description = "Your best idea!",
                    Name = "Mystery gift",
                    UserAskingId = userId,
                },
                new Gift
                {
                    Id = 4,
                    Description = "Something for our cats",
                    Name = "Cat toys",
                    UserAskingId = userId2
                },
                new Gift
                {
                    Id = 5,
                    Description = "The best moves you know!",
                    Name = "Dance moves",
                    UserAskingId = userId2
                }
            );

            modelBuilder.Entity<Gift>()
                .HasOne(u => u.UserAsking);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        public DbSet<Gift> Gifts { get; set; }

        public DbSet<ApplicationUser> People { get; set; }
    }
}
