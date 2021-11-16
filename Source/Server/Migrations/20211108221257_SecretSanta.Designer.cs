﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Wishlist.Server.Data;

namespace Wishlist.Server.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20211108221257_SecretSanta")]
    partial class SecretSanta
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = "1e23d1a2-a932-4223-b322-47770dc9bb95",
                            ConcurrencyStamp = "d15e8ae1-d846-4eed-86ee-391fe1ddd72a",
                            Name = "Basic",
                            NormalizedName = "BASIC"
                        },
                        new
                        {
                            Id = "a36a567b-f28a-49c8-8f86-3181271bd93c",
                            ConcurrencyStamp = "bfec01f0-c4c9-4daa-ac3f-6d5e6360643a",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Wishlist.Shared.Models.Gift", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal?>("Cost")
                        .HasColumnType("Money");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Rank")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TimeAdded")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("TimeBought")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserAskingId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserBuyingId")
                        .HasColumnType("text");

                    b.Property<string>("UserSuggestingId")
                        .HasColumnType("text");

                    b.Property<string>("WebLink")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserAskingId");

                    b.HasIndex("UserBuyingId");

                    b.ToTable("Gifts");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Cost = 39999m,
                            Description = "Shiny new wheels!",
                            Name = "New Car",
                            Rank = 0,
                            TimeAdded = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeBought = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserAskingId = "400d5b1c-b761-4e2b-b0ef-d9909eefc433",
                            WebLink = "https://www.tesla.com"
                        },
                        new
                        {
                            Id = 2L,
                            Cost = 9m,
                            Description = "So warm....",
                            Name = "Socks",
                            Rank = 1,
                            TimeAdded = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeBought = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserAskingId = "400d5b1c-b761-4e2b-b0ef-d9909eefc433"
                        },
                        new
                        {
                            Id = 3L,
                            Description = "Your best idea!",
                            Name = "Mystery gift",
                            Rank = 0,
                            TimeAdded = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeBought = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserAskingId = "400d5b1c-b761-4e2b-b0ef-d9909eefc433"
                        },
                        new
                        {
                            Id = 4L,
                            Description = "Something for our cats",
                            Name = "Cat toys",
                            Rank = 0,
                            TimeAdded = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeBought = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserAskingId = "aad29937-f673-426b-b338-fc5b75926c37"
                        },
                        new
                        {
                            Id = 5L,
                            Description = "The best moves you know!",
                            Name = "Dance moves",
                            Rank = 0,
                            TimeAdded = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TimeBought = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            UserAskingId = "aad29937-f673-426b-b338-fc5b75926c37"
                        });
                });

            modelBuilder.Entity("Wishlist.Shared.Models.User.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<int>("GroupId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("LastListUpdate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("ListNote")
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NickName")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<Guid>("PasswordResetCode")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("PasswordResetTimestamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("PictureData")
                        .HasColumnType("text");

                    b.Property<string>("PictureType")
                        .HasColumnType("text");

                    b.Property<string>("SantaForUserName")
                        .HasColumnType("text");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");

                    b.HasData(
                        new
                        {
                            Id = "400d5b1c-b761-4e2b-b0ef-d9909eefc433",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "caf86e69-0a26-4f93-b7f5-b8557cf3c49c",
                            Email = "joe_blow_tester@gmail.com",
                            EmailConfirmed = false,
                            FirstName = "Joe",
                            GroupId = 1,
                            LastListUpdate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Blow",
                            ListNote = "This is my example list note.  :)",
                            LockoutEnabled = true,
                            NickName = "Jman",
                            NormalizedEmail = "JOE_BLOW_TESTER@GMAIL.COM",
                            NormalizedUserName = "JOEB",
                            PasswordHash = "AQAAAAEAACcQAAAAELRLao2z9QK1uTSF9MkS3XFL7kGTJ5peolLcnUgufG9kG8KuRtvypjjKxRL6nPvxUw==",
                            PasswordResetCode = new Guid("00000000-0000-0000-0000-000000000000"),
                            PasswordResetTimestamp = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "4IBP2DQCF5ZMZZAQ2POAGGYHQMDNGT4M",
                            TwoFactorEnabled = false,
                            UserName = "JoeB"
                        },
                        new
                        {
                            Id = "aad29937-f673-426b-b338-fc5b75926c37",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "642aa26e-a0dd-47de-b145-348e8f6845e7",
                            Email = "jane_doe_tester@gmail.com",
                            EmailConfirmed = false,
                            FirstName = "Jane",
                            GroupId = 2,
                            LastListUpdate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Doe",
                            ListNote = "",
                            LockoutEnabled = true,
                            NickName = "",
                            NormalizedEmail = "JANE_DOE_TESTER@GMAIL.COM",
                            NormalizedUserName = "JANED",
                            PasswordHash = "AQAAAAEAACcQAAAAEApbnFa0lPJ/4EP2Uo60kyLy8ZBcFAezRYiU9soysXiGH9YjCxGcVxB1ugbGxU5/1w==",
                            PasswordResetCode = new Guid("00000000-0000-0000-0000-000000000000"),
                            PasswordResetTimestamp = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "VJNRLNY67OH36WP3OWRTKNXU2TJK3OBN",
                            TwoFactorEnabled = false,
                            UserName = "JaneD"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Wishlist.Shared.Models.User.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Wishlist.Shared.Models.User.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wishlist.Shared.Models.User.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Wishlist.Shared.Models.User.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Wishlist.Shared.Models.Gift", b =>
                {
                    b.HasOne("Wishlist.Shared.Models.User.ApplicationUser", "UserAsking")
                        .WithMany()
                        .HasForeignKey("UserAskingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wishlist.Shared.Models.User.ApplicationUser", "UserBuying")
                        .WithMany()
                        .HasForeignKey("UserBuyingId");

                    b.Navigation("UserAsking");

                    b.Navigation("UserBuying");
                });
#pragma warning restore 612, 618
        }
    }
}
