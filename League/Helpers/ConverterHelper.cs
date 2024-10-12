using League.Data.Entities;
using League.Models;

namespace League.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Club ToClub(ClubViewModel model, string path, bool isNew)
        {
            return new Club
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                ImageId = path,
                FoundingDate = model.FoundingDate,
                Stadium = model.Stadium,
            };
        }

        public Club ToClubViewModel(Club club)
        {
            return new ClubViewModel
            {
                Id = club.Id,
                Name = club.Name,
                ImageId = club.ImageId,
                FoundingDate = club.FoundingDate,
                Stadium = club.Stadium,
            };
        }

        public Player ToPlayer(PlayerViewModel model, string path, bool isNew)
        {
            return new Player
            {
                Id = isNew ? 0 : model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                Position = model.Position,
                ImageUrl = path,
                ClubId = model.ClubId,
            };
        }

        public Player ToPlayerViewModel(Player player)
        {
            return new PlayerViewModel
            {
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                BirthDate = player.BirthDate,
                Position = player.Position,
                ImageUrl = player.ImageUrl,
                ClubId = player.ClubId,
            };
        }

        public Staff ToStaff(StaffViewModel model, string path, bool isNew)
        {
            return new Staff
            {
                Id = isNew ? 0 : model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                FunctionPerformed = model.FunctionPerformed,
                ImageUrl = path,
                ClubId = model.ClubId,
            };
        }

        public Staff ToStaffViewModel(Staff staff)
        {
            return new StaffViewModel
            {
                Id = staff.Id,
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                BirthDate = staff.BirthDate,
                FunctionPerformed = staff.FunctionPerformed,
                ImageUrl = staff.ImageUrl,
                ClubId = staff.ClubId,
            };
        }
    }

}
