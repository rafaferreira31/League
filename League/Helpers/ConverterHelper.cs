using League.Data.Entities;
using League.Models;

namespace League.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Club ToClub(ClubViewModel model, Guid imageId, bool isNew)
        {
            return new Club
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                ImageId = imageId,
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

        public Player ToPlayer(PlayerViewModel model, Guid imageId, bool isNew)
        {
            return new Player
            {
                Id = isNew ? 0 : model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                Position = model.Position,
                ImageId = imageId,
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
                ImageId = player.ImageId,
                ClubId = player.ClubId,
            };
        }

        public Staff ToStaff(StaffViewModel model, Guid imageId, bool isNew)
        {
            return new Staff
            {
                Id = isNew ? 0 : model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                FunctionPerformed = model.FunctionPerformed,
                ImageId = imageId,
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
                ImageId = staff.ImageId,
                ClubId = staff.ClubId,
            };
        }
    }
}
