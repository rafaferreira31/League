using League.Data.Repositories;
using League.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace League.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IGameRepository _gameRepository;

        public StatisticsController(IClubRepository clubRepository, IGameRepository gameRepository)
        {
            _clubRepository = clubRepository;
            _gameRepository = gameRepository;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var clubs = _clubRepository.GetAll();

            var clubStats = new List<ClubStatisticsViewModel>();

            foreach (var club in clubs)
            {
                var stats = new ClubStatisticsViewModel
                {
                    ClubId = club.Id,
                    ClubName = club.Name,
                    GamesPlayed = await _gameRepository.GetGamesPlayedByClub(club.Id),
                    GamesWon = await _gameRepository.GetGamesWonByClub(club.Id),
                    GamesDrawn = await _gameRepository.GetGamesDrawnByClub(club.Id),
                    GamesLost = await _gameRepository.GetGamesLostByClub(club.Id),
                    GoalsScored = await _gameRepository.GetGoalsScoredByClub(club.Id),
                    GoalsConceded = await _gameRepository.GetGoalsConcededByClub(club.Id),
                    Points = await _gameRepository.CalculatePointsByClub(club.Id)
                };

                clubStats.Add(stats);
            }

            var sortedClubStats = clubStats.OrderByDescending(c => c.Points).ThenByDescending(c => c.GamesPlayed).ToList();

            return View(sortedClubStats);
        }
    }
}
