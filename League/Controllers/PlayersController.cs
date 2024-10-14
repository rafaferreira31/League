using League.Data.Entities;
using League.Data.Repositories;
using League.Helpers;
using League.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace League.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public PlayersController(
            IPlayerRepository playerRepository,
            IClubRepository clubRepository,
            IImageHelper imageHelper,
            IConverterHelper converterHelper
            )
        {
            _playerRepository = playerRepository;
            _clubRepository = clubRepository;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        // GET: Players
        public IActionResult Index()
        {
            return View(_playerRepository.GetAll());
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _playerRepository.GetByIdAsync(id.Value);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Name");

            ViewBag.Positions = new SelectList(new List<string>
            {
                "Goalkeeper",
                "Left-Back",
                "Right-Back",
                "Central-Back",
                "Sweeper",
                "Defensive Midfielder",
                "Central Midfielder",
                "Left-Winger",
                "Right-Winger",
                "Central-Forward",
                "Striker"
            });

            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if(model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "players");
                }

                var player = _converterHelper.ToPlayer(model, path, true);

                await _playerRepository.CreateAsync(player);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Name");

            ViewBag.Positions = new SelectList(new List<string>
            {
                "Goalkeeper",
                "Left-Back",
                "Right-Back",
                "Central-Back",
                "Sweeper",
                "Defensive Midfielder",
                "Central Midfielder",
                "Left-Winger",
                "Right-Winger",
                "Central-Forward",
                "Striker"
            });

            return View(model);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _playerRepository.GetByIdAsync(id.Value);
            if (player == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToPlayerViewModel(player);

            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Name", model.ClubId);

            ViewBag.Positions = new SelectList(new List<string>
            {
                "Goalkeeper",
                "Left-Back",
                "Right-Back",
                "Central-Back",
                "Sweeper",
                "Defensive Midfielder",
                "Central Midfielder",
                "Left-Winger",
                "Right-Winger",
                "Central-Forward",
                "Striker"
            });

            return View(model);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PlayerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = string.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "players");
                    }

                    var player = _converterHelper.ToPlayer(model, path, false);

                    await _playerRepository.UpdateAsync(player);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _playerRepository.ExistAsync(model.Id))
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

            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Name", model.ClubId);

            ViewBag.Positions = new SelectList(new List<string>
            {
                "Goalkeeper",
                "Left-Back",
                "Right-Back",
                "Central-Back",
                "Sweeper",
                "Defensive Midfielder",
                "Central Midfielder",
                "Left-Winger",
                "Right-Winger",
                "Central-Forward",
                "Striker"
            });

            return View(model);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _playerRepository.GetByIdAsync(id.Value);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _playerRepository.GetByIdAsync(id);
            await _playerRepository.DeleteAsync(player);
            return RedirectToAction(nameof(Index));
        }
    }
}
