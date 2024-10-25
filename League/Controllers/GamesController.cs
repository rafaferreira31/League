using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using League.Data;
using League.Data.Entities;
using League.Data.Repositories;
using League.Models;
using Microsoft.AspNetCore.Authorization;

namespace League.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameRepository _gameRepository;
        private readonly IClubRepository _clubRepository;

        public GamesController(IGameRepository gameRepository, IClubRepository clubRepository)
        {
            _gameRepository = gameRepository;
            _clubRepository = clubRepository;
        }

        // GET: Games
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var games = _gameRepository.GetAll().OrderByDescending(g => g.GameDate).ToList();

            var model = new List<GameViewModel>();

            foreach (var game in games)
            {
                var visitedClubEmblem = await _clubRepository.GetClubEmblemById(game.VisitedClubId);
                var visitorClubEmblem = await _clubRepository.GetClubEmblemById(game.VisitorClubId);

                model.Add(new GameViewModel
                {
                    Id = game.Id,
                    GameDate = game.GameDate,
                    VisitedClubName = game.VisitedClubName,
                    VisitorClubName = game.VisitorClubName,
                    VisitedGoals = game.VisitedGoals,
                    VisitorGoals = game.VisitorGoals,
                    VisitedClubEmblem = visitedClubEmblem,
                    VisitorClubEmblem = visitorClubEmblem,
                    Status = game.Status
                });

                await _gameRepository.UpdateGameStatusAsync(game);
            }

            return View(model);
        }


        // GET: Games/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _gameRepository.GetByIdAsync(id.Value);
            if (game == null)
            {
                return NotFound();
            }

            var visitedClubEmblem = await _clubRepository.GetClubEmblemById(game.VisitedClubId);
            var visitorClubEmblem = await _clubRepository.GetClubEmblemById(game.VisitorClubId);

            ViewBag.VisitedClubEmblem = visitedClubEmblem;
            ViewBag.VisitorClubEmblem = visitorClubEmblem;

            return View(game);
        }

        // GET: Games/Create
        [Authorize(Roles ="FederationEmployee")]
        public IActionResult Create()
        {
            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="FederationEmployee")]
        public async Task<IActionResult> Create(Game game)
        {
            if (ModelState.IsValid)
            {
                var visitedName = await _clubRepository.GetClubNameById(game.VisitedClubId);
                var visitorName = await _clubRepository.GetClubNameById(game.VisitorClubId);

                game.VisitedClubName = visitedName;
                game.VisitorClubName = visitorName;

                await _gameRepository.UpdateGameStatusAsync(game);

                await _gameRepository.CreateAsync(game);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Name");

            return View(game);
        }

        // GET: Games/Edit/5
        [Authorize(Roles = "FederationEmployee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _gameRepository.GetByIdAsync(id.Value);
            if (game == null)
            {
                return NotFound();
            }

            if(game.Status == Game.GameStatus.Closed)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Name");

            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "FederationEmployee")]

        public async Task<IActionResult> Edit(int id, Game game)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _gameRepository.UpdateGameStatusAsync(game);

                    var visitedName = await _clubRepository.GetClubNameById(game.VisitedClubId);
                    var visitorName = await _clubRepository.GetClubNameById(game.VisitorClubId);

                    game.VisitedClubName = visitedName;
                    game.VisitorClubName = visitorName;

                    await _gameRepository.UpdateAsync(game);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _gameRepository.ExistAsync(game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Name");

            return View(game);
        }

        // GET: Games/Delete/5
        [Authorize(Roles = "FederationEmployee")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _gameRepository.GetByIdAsync(id.Value);
            if (game == null)
            {
                return NotFound();
            }

            var visitedClubEmblem = await _clubRepository.GetClubEmblemById(game.VisitedClubId);
            var visitorClubEmblem = await _clubRepository.GetClubEmblemById(game.VisitorClubId);

            ViewBag.VisitedClubEmblem = visitedClubEmblem;
            ViewBag.VisitorClubEmblem = visitorClubEmblem;

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "FederationEmployee")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            await _gameRepository.DeleteAsync(game);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Dashboard()
        {
            var games = await _gameRepository.GetGamesToCloseAsync();
            
            var model = new List<GameViewModel>();

            foreach(var game in games)
            {
                var visitedClubEmblem = await _clubRepository.GetClubEmblemById(game.VisitedClubId);
                var visitorClubEmblem = await _clubRepository.GetClubEmblemById(game.VisitorClubId);

                model.Add(new GameViewModel
                {
                    Id = game.Id,
                    GameDate = game.GameDate,
                    VisitedClubName = game.VisitedClubName,
                    VisitorClubName = game.VisitorClubName,
                    VisitedGoals = game.VisitedGoals,
                    VisitorGoals =  game.VisitorGoals,
                    VisitedClubEmblem = visitedClubEmblem,
                    VisitorClubEmblem = visitorClubEmblem,
                    Status = game.Status
                });

                await _gameRepository.UpdateGameStatusAsync(game);
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GameClosing(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var game = await _gameRepository.GetByIdAsync(id.Value);
            if (game == null)
            {
                return NotFound();
            }

            var visitedClubEmblem = await _clubRepository.GetClubEmblemById(game.VisitedClubId);
            var visitorClubEmblem = await _clubRepository.GetClubEmblemById(game.VisitorClubId);

            ViewBag.VisitedClubEmblem = visitedClubEmblem;
            ViewBag.VisitorClubEmblem = visitorClubEmblem;

            return View(game);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CloseGame(int id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            if(game == null)
            {
                return NotFound();
            }

            game.Status = Game.GameStatus.Closed;


            await _gameRepository.UpdateAsync(game);

            ViewBag.Message = "Game closed successfully";

            return RedirectToAction(nameof(Dashboard));
        }

    }
}
