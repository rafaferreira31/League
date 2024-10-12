using League.Data.Entities;
using League.Helpers;
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
            await _userHelper.CheckRoleAsync("FederationEmpolyee");
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
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if(!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }


            if (!_context.Clubs.Any())
            {
                AddClub("FC Barcelona", new DateTime(1899, 11, 29), "Camp Nou");
                AddClub("Real Madrid", new DateTime(1902, 3, 6), "Santiago Bernabeu");
                AddClub("Paris Saint-Germain", new DateTime(1970, 8, 12), "Parc des Princes");
                AddClub("Manchester City", new DateTime(1880, 4, 23), "Etihad Stadium");
                await _context.SaveChangesAsync();
            }


            if (!_context.Players.Any())
            {
                var barcelona = _context.Clubs.FirstOrDefault(c => c.Name == "FC Barcelona");
                var madrid = _context.Clubs.FirstOrDefault(c => c.Name == "Real Madrid");

                AddPlayer("Lionel", "Messi", new DateTime(1987, 6, 24), "Forward", barcelona.Id);
                AddPlayer("Cristiano", "Ronaldo", new DateTime(1985, 2, 5), "Forward", madrid.Id);
                AddPlayer("Neymar", "Jr", new DateTime(1992, 2, 5), "Forward", barcelona.Id);
                AddPlayer("Kylian", "Mbappé", new DateTime(1998, 12, 20), "Forward", madrid.Id);

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


        private void AddClub(string name, DateTime foundingDate, string stadium)
        {
            _context.Clubs.Add(new Club
            {
                Name = name,
                FoundingDate = foundingDate,
                Stadium = stadium
            });
        }



    }
}
