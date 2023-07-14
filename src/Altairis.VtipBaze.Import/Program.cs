﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Altairis.VtipBaze.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Altairis.VtipBaze.Import
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddDbContext<VtipBazeContext>(options => options.UseSqlServer("Data Source=.\\SQLEXPRESS; Initial Catalog=VtipBaze; Integrated Security=true; Trust Server Certificate=true"));
            services.AddIdentityCore<IdentityUser>().AddEntityFrameworkStores<VtipBazeContext>();
            var serviceProvider = services.BuildServiceProvider();

            await using (var dc = serviceProvider.GetRequiredService<VtipBazeContext>())
            {
                await dc.Database.EnsureCreatedAsync();

                // seed data (all jokes were kindly provided by ChatGPT)
                if (!dc.Tags.Any())
                {
                    dc.Tags.Add(new Tag()
                    {
                        TagName = "Puns",
                        Jokes = new List<Joke>()
                        {
                            new Joke() { Text = "I'm reading a book on the history of glue. I just can't seem to put it down.", Approved = true },
                            new Joke() { Text = "Why did the bicycle fall over? Because it was two-tired.", Approved = true },
                            new Joke() { Text = "I told my wife she was drawing her eyebrows too high. She looked surprised.", Approved = true },
                            new Joke() { Text = "Why don't scientists trust atoms? Because they make up everything.", Approved = true },
                            new Joke() { Text = "I used to play piano by ear, but now I use my hands.", Approved = true }
                        }
                    });
                    dc.Tags.Add(new Tag()
                    {
                        TagName = "KnockKnock",
                        Jokes = new List<Joke>()
                        {
                            new Joke() { Text = "Knock, knock.\r\nWho's there?\r\nBoo.\r\nBoo who?\r\nDon't cry. It's just a joke.", Approved = true },
                            new Joke() { Text = "Knock, knock.\r\nWho's there?\r\nNobel.\r\nNobel who?\r\nNo bell, that's why I knocked.", Approved = true },
                            new Joke() { Text = "Knock, knock.\r\nWho's there?\r\nOrange.\r\nOrange who?\r\nOrange you going to answer the door?", Approved = true },
                            new Joke() { Text = "Knock, knock.\r\nWho's there?\r\nTank.\r\nTank who?\r\nYou're welcome.", Approved = true },
                            new Joke() { Text = "Knock, knock.\r\nWho's there?\r\nAlpaca.\r\nAlpaca who?\r\nAlpaca the suitcase, you load up the car!", Approved = true },
                            new Joke() { Text = "Knock, knock.\r\nWho's there?\r\nLettuce.\r\nLettuce who?\r\nLettuce in, it's cold out here!", Approved = true },
                            new Joke() { Text = "Knock, knock.\r\nWho's there?\r\nInterrupting cow.\r\nInterrupting cow wh-\r\nMOOOOOOOOO!", Approved = true },
                            new Joke() { Text = "Knock, knock.\r\nWho's there?\r\nHarry.\r\nHarry who?\r\nHarry up and answer the door!", Approved = true }
                        }
                    });
                    dc.Tags.Add(new Tag()
                    {
                        TagName = "Animal",
                        Jokes = new List<Joke>()
                        {
                            new Joke() { Text = "Why did the chicken cross the playground? To get to the other slide.", Approved = true },
                            new Joke() { Text = "What do you call a bear with no teeth? A gummy bear.", Approved = true },
                            new Joke() { Text = "Why did the frog call his insurance company? He had a jump in his car.", Approved = true },
                            new Joke() { Text = "What do you call a sleeping bull? A bulldozer.", Approved = true },
                            new Joke() { Text = "What do you call a group of cows playing instruments? A moo-sical band.", Approved = true },
                            new Joke() { Text = "Why did the elephant wear green sneakers? Because the red ones were in the wash.", Approved = true },
                            new Joke() { Text = "What did the grape say when the elephant stepped on it? Nothing, it just let out a little wine.", Approved = true },
                            new Joke() { Text = "Why did the cat wear a fancy dress? She was feline fine.", Approved = true }
                        }
                    });
                    dc.Tags.Add(new Tag()
                    {
                        TagName = "Political",
                        Jokes = new List<Joke>()
                        {
                            new Joke() { Text = "Why did the politician decide to retire? He wanted to spend more time raising campaign funds.", Approved = true },
                            new Joke() { Text = "Why did the politician refuse to take a polygraph test? He didn't want to be hooked up to a machine that was smarter than he was.", Approved = true },
                            new Joke() { Text = "Why did the politician wear two jackets when he painted the house? The instructions on the can said: \"Put on two coats for best results.\"", Approved = true },
                            new Joke() { Text = "Why did the politician go to the doctor? He needed a prescription for his addiction to lobbyists' money.", Approved = true },
                        }
                    });
                    await dc.SaveChangesAsync();

                    Console.WriteLine("Data seed completed.");
                }
                else
                {
                    Console.WriteLine("The database already exists, skipped.");
                }

                // migrate Users table from ASP.NET membership
                if (dc.Database.SqlQueryRaw<int>("SELECT COUNT(*) AS Value FROM information_schema.Tables WHERE TABLE_NAME = 'Users'").First() != 0)
                {
                    Console.WriteLine("Migrating old users...");

                    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                    
                    var oldUsers = dc.Database.SqlQueryRaw<string>(
                        @"SELECT (
                            SELECT Users.UserId, Users.UserName, Memberships.Email
                            FROM Users
                            JOIN Memberships ON Memberships.UserId = Users.UserId
                            FOR JSON PATH
                          ) AS Value")
                        .First();
                    foreach (var oldUser in JsonSerializer.Deserialize<OldUser[]>(oldUsers))
                    {
                        var entity = new IdentityUser()
                        {
                            Id = oldUser.UserId.ToString(),
                            UserName = oldUser.UserName,
                            Email = oldUser.Email,
                            SecurityStamp = Guid.NewGuid().ToString()
                        };
                        await userManager.CreateAsync(entity);
                    }

                    // set password to the admin user
                    var adminUser = await userManager.FindByNameAsync("admin");
                    await userManager.AddPasswordAsync(adminUser, "Admin123+");
                    
                    // delete old user tables
                    await dc.Database.ExecuteSqlRawAsync("DROP TABLE Memberships");
                    await dc.Database.ExecuteSqlRawAsync("DROP TABLE UsersInRoles");
                    await dc.Database.ExecuteSqlRawAsync("DROP TABLE Profiles");
                    await dc.Database.ExecuteSqlRawAsync("DROP TABLE Roles");
                    await dc.Database.ExecuteSqlRawAsync("DROP TABLE Users");
                    await dc.Database.ExecuteSqlRawAsync("DROP TABLE Applications");

                    Console.WriteLine("Password for admin user reset.");
                }
                else
                {
                    Console.WriteLine("Users already migrated, skipped.");
                }
            }
        }
    }

    public class OldUser
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

}
