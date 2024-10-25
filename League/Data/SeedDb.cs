using League.Data.Entities;
using League.Helpers;
using League.Migrations;
using Microsoft.AspNetCore.Identity;

namespace League.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }


        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("FederationEmployee");
            await _userHelper.CheckRoleAsync("ClubRepresentant");
            await _userHelper.CheckRoleAsync("Guest");


            var user = await _userHelper.GetUserByEmailAsync("rafa.testes@sapo.pt");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Rafael",
                    LastName = "Ferreira",
                    UserName = "rafa.testes@sapo.pt",
                    Email = "rafa.testes@sapo.pt",
                    PhoneNumber= "123456789",
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if(result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if(!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }


            if (!_context.Clubs.Any())
            {
                AddClub("FC Barcelona", new DateTime(1899, 11, 29), "Camp Nou" , 0,0,0,0,0,0,0);
                AddClub("Real Madrid", new DateTime(1902, 3, 6), "Santiago Bernabeu", 0, 0, 0, 0, 0, 0, 0);
                AddClub("Paris Saint-Germain", new DateTime(1970, 8, 12), "Parc des Princes", 0, 0, 0, 0, 0, 0, 0);
                AddClub("Manchester City", new DateTime(1880, 4, 23), "Etihad Stadium", 0, 0, 0, 0, 0, 0, 0);
                await _context.SaveChangesAsync();
            }


            if (!_context.Players.Any())
            {
                var barcelona = _context.Clubs.FirstOrDefault(c => c.Name == "FC Barcelona");
                var madrid = _context.Clubs.FirstOrDefault(c => c.Name == "Real Madrid");

                AddPlayer("Lionel", "Messi", new DateTime(1987, 6, 24), "Striker", barcelona.Id);
                AddPlayer("Cristiano", "Ronaldo", new DateTime(1985, 2, 5), "Striker", madrid.Id);
                AddPlayer("Neymar", "Jr", new DateTime(1992, 2, 5), "Striker", barcelona.Id);
                AddPlayer("Kylian", "Mbappé", new DateTime(1998, 12, 20), "Striker", madrid.Id);

                await _context.SaveChangesAsync();
            }


            if (!_context.Staffs.Any())
            {
                var barcelona = _context.Clubs.FirstOrDefault(c => c.Name == "FC Barcelona");
                var madrid = _context.Clubs.FirstOrDefault(c => c.Name == "Real Madrid");

                AddStaff("Ronald", "Koeman", new DateTime(1963, 3, 21), "Head Coach", barcelona.Id);
                AddStaff("Zinedine", "Zidane", new DateTime(1972, 6, 23), "Head Coach", madrid.Id);
                AddStaff("Jordi", "Roura", new DateTime(1967, 3, 6), "Assistant Coach", barcelona.Id);
                AddStaff("David", "Bettoni", new DateTime(1980, 5, 8), "Assistant Coach", madrid.Id);

                await _context.SaveChangesAsync();
            }

            if (!_context.Games.Any())
            {
                var barcelona = _context.Clubs.FirstOrDefault(c => c.Name == "FC Barcelona");
                var madrid = _context.Clubs.FirstOrDefault(c => c.Name == "Real Madrid");

                AddGame(new DateTime(2024, 11, 30), barcelona.Id, "FC Barcelona", madrid.Id, "Real Madrid", Game.GameStatus.Scheduled);
                AddGame(new DateTime(2024, 12, 25), madrid.Id, "Real Madrid", barcelona.Id, "FC Barcelona", Game.GameStatus.Scheduled);

                await _context.SaveChangesAsync();
            }
            
        }


        private void AddPlayer(string firstName, string lastName,DateTime birthDate, string position, int clubId)
        {
            _context.Players.Add(new Player
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                Position = position,
                ClubId = clubId
            });
        }

        private void AddStaff(string firstName, string lastName, DateTime birthDate, string functionPerformed, int clubId)
        {
            _context.Staffs.Add(new Staff
            {
                FirstName = firstName,
                LastName = lastName,
                BirthDate = birthDate,
                FunctionPerformed = functionPerformed,
                ClubId = clubId
            });
        }

        private void AddClub(string name, DateTime foundingDate, string stadium, int gamesPlayed, int gamesWon, int gamesDrawn, int gamesLost, int goalsScored, int goalsConceded, int points)
        {
            _context.Clubs.Add(new Club
            {
                Name = name,
                FoundingDate = foundingDate,
                Stadium = stadium,
                GamesPlayed = gamesPlayed,
                GamesWon = gamesWon,
                GamesDrawn = gamesDrawn,
                GamesLost = gamesLost,
                GoalsScored = goalsScored,
                GoalsConceded = goalsConceded,
                Points = points
            });
        }

        private void AddGame(DateTime gameDate, int visitedClubId, string visitedClubName, int visitorClubId, string visitorClubName, Game.GameStatus status)
        {
            _context.Games.Add(new Game
            {
                GameDate = gameDate,
                VisitedClubId = visitedClubId,
                VisitedClubName = visitedClubName,
                VisitorClubId = visitorClubId,
                VisitorClubName = visitorClubName,
                Status = status
            });
        }
    }
}
