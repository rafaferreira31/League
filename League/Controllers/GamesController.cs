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
        public IActionResult Index()
        {
            return View(_gameRepository.GetAll().OrderBy(g => g.GameDate));
        }

        // GET: Games/Details/5
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

            return View(game);
        }

        // GET: Games/Create
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
        public async Task<IActionResult> Create(Game game)
        {
            if (ModelState.IsValid)
            {
                var visitedName = await _clubRepository.GetClubNameById(game.VisitedClubId);
                var visitorName = await _clubRepository.GetClubNameById(game.VisitorClubId);

                game.VisitedClubName = visitedName;
                game.VisitorClubName = visitorName;

                await _gameRepository.CreateAsync(game);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Name");

            return View(game);
        }

        // GET: Games/Edit/5
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

            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Name");

            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Game game)
        {
            if (ModelState.IsValid)
            {
                try
                {
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

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            await _gameRepository.DeleteAsync(game);

            return RedirectToAction(nameof(Index));
        }
    }
}
