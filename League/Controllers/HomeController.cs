using League.Data;
using League.Data.Repositories;
using League.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace League.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPlayerRepository _playerRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IClubRepository _clubRepository;

        public HomeController(ILogger<HomeController> logger, IPlayerRepository playerRepository, IGameRepository gameRepository, IClubRepository clubRepository)
        {
            _logger = logger;
            _playerRepository = playerRepository;
            _gameRepository = gameRepository;
            _clubRepository = clubRepository;
        }

        public async Task<IActionResult> Index()
        {
            var nextGame = await _gameRepository.GetNextGameAsync();

            var visitedClubEmblem = await _clubRepository.GetClubEmblemById(nextGame.VisitedClubId);
            var visitorClubEmblem = await _clubRepository.GetClubEmblemById(nextGame.VisitorClubId);

            ViewBag.VisitedClubEmblem = visitedClubEmblem;
            ViewBag.VisitorClubEmblem = visitorClubEmblem;

            var clubs = _clubRepository.GetAll();
            var clubsStats = new List<ClubStatisticsViewModel>();

            var players = _playerRepository.GetAll();
            var randomPlayers = players
                .OrderBy(p => Guid.NewGuid())
                .Take(5)
                .ToList();

            foreach (var club in clubs)
            {
                clubsStats.Add(new ClubStatisticsViewModel
                {
                    ClubId = club.Id,
                    ClubName = club.Name,
                });
            }

            var top5Clubs = clubsStats
                    .OrderByDescending(c => c.Points)
                    .ThenByDescending(c => c.GoalsScored)
                    .Take(5)
                    .ToList();

            var model = new HomePageViewModel
            {
                RandomPlayers = randomPlayers,
                NextGame = nextGame,
                ClubStatistics = top5Clubs
            };

            return View(model);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult UseTerms()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
