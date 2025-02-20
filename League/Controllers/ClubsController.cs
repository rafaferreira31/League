﻿using League.Data;
using League.Data.Repositories;
using League.Helpers;
using League.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace League.Controllers
{
    public class ClubsController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        private readonly IGameRepository _gameRepository;

        public ClubsController(
            IClubRepository clubRepository,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper,
            IUserHelper userHelper,
            DataContext context,
            IGameRepository gameRepository
            )
        {
            _clubRepository = clubRepository;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
            _context = context;
            _gameRepository = gameRepository;
        }

        // GET: Clubs
        [Authorize]
        public IActionResult Index()
        {
            return View(_clubRepository.GetAll().OrderBy(c => c.Name));
        }

        // GET: Clubs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _context.Clubs
                .Include(c => c.Players)
                .Include(c => c.Staffs)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        // GET: Clubs/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clubs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ClubViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "clubs");
                }

                var club = _converterHelper.ToClub(model, imageId, true);

                var duplicatedClub = await _clubRepository.GetByNameAsync(club.Name);
                if (duplicatedClub != null)
                {
                    ModelState.AddModelError(string.Empty, "There is a club with the same name.");
                    return View(model);
                }

                club.GamesPlayed = 0;
                club.GamesWon = 0;
                club.GamesDrawn = 0;
                club.GamesLost = 0;
                club.GoalsScored = 0;
                club.GoalsConceded = 0;
                club.Points = 0;

                await _clubRepository.CreateAsync(club);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Clubs/Edit/5
        [Authorize(Roles = "ClubRepresentant")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _clubRepository.GetByIdAsync(id.Value);

            if (club == null)
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (user.ClubId != club.Id)
            {
                return RedirectToAction("Error403", "Errors");
            }

            var model = _converterHelper.ToClubViewModel(club);
            return View(model);
        }

        // POST: Clubs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ClubRepresentant")]
        public async Task<IActionResult> Edit(ClubViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (user.ClubId != model.Id)
            {
                return RedirectToAction("Error403", "Errors");
            }

            if (ModelState.IsValid)
            {
                var duplicatedClub = await _clubRepository.GetByNameAsync(model.Name);
                if (duplicatedClub != null && duplicatedClub.Id != model.Id)
                {
                    ModelState.AddModelError(string.Empty, "There is a club with the same name.");
                    return View(model);
                }
                try
                {
                    Guid imageId = model.ImageId;
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "clubs");
                    }

                    var club = _converterHelper.ToClub(model, imageId, false);

                    await _clubRepository.UpdateAsync(club);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _clubRepository.ExistAsync(model.Id))
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
            return View(model);
        }

        // GET: Clubs/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _clubRepository.GetByIdAsync(id.Value);
            if (club == null)
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (user.ClubId != club.Id)
            {
                return RedirectToAction("Error403", "Errors");
            }

            return View(club);
        }

        // POST: Clubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);

            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (user.ClubId != club.Id)
            {
                return RedirectToAction("Error403", "Errors");
            }

            await _clubRepository.DeleteAsync(club);

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles ="ClubRepresentant")]
        public async Task<IActionResult> Manage()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            if (user.ClubId == null || user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Details", new { id = user.ClubId });
        }


        public async Task<IActionResult> Statistics()
        {
            var clubs = _clubRepository.GetAll().ToList();
            var model = new List<ClubStatisticsViewModel>();

            foreach (var club in clubs)
            {
                model.Add(new ClubStatisticsViewModel
                {
                    ClubId = club.Id,
                    ClubName = club.Name,
                    GamesPlayed = await _gameRepository.GetGamesPlayedAsync(club.Id),
                    GamesWon = await _gameRepository.GetGamesWonAsync(club.Id),
                    GamesDrawn = await _gameRepository.GetGamesDrawnAsync(club.Id),
                    GamesLost = await _gameRepository.GetGamesLostAsync(club.Id),
                    GoalsScored = await _gameRepository.GetGoalsScoredAsync(club.Id),
                    GoalsConceded = await _gameRepository.GetGoalsConcededAsync(club.Id),
                    Points = await _gameRepository.GetPointsAsync(club.Id)
                });
            }

            model = model.OrderByDescending(c => c.Points)
                .ThenByDescending(c => c.GoalsScored)
                .Select((club, index) =>
                {
                    club.Rank = index + 1;
                    return club;
                })
                .ToList();

            return View(model);
        }
    }
}
