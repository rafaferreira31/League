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
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;

        public PlayersController(
            IPlayerRepository playerRepository,
            IClubRepository clubRepository,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper,
            IUserHelper userHelper
            )
        {
            _playerRepository = playerRepository;
            _clubRepository = clubRepository;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
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
        public async Task<IActionResult> Create()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            if (User.IsInRole("ClubRepresentant") && user.ClubId.HasValue)
            {
                // Exibe apenas o clube do representante logado
                ViewBag.Clubs = new SelectList(
                    _clubRepository.GetAll().Where(c => c.Id == user.ClubId.Value),
                    "Id",
                    "Name");
            }
            else
            {
                // Exibe todos os clubes para outros tipos de usuários
                ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Name");
            }

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

                Guid imageId = Guid.Empty;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "players");
                }

                var player = _converterHelper.ToPlayer(model, imageId, true);

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
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            if (User.IsInRole("ClubRepresentant") && user.ClubId.HasValue)
            {
                // Exibe apenas o clube do representante logado
                ViewBag.Clubs = new SelectList(
                    _clubRepository.GetAll().Where(c => c.Id == user.ClubId.Value),
                    "Id",
                    "Name",
                    model.ClubId);
            }
            else
            {
                // Exibe todos os clubes para outros tipos de usuários
                ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Name", model.ClubId);
            }

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
                var duplicatedPlayer = await _clubRepository.GetByNameAsync(model.FullName);
                if (duplicatedPlayer != null && duplicatedPlayer.Id != model.Id)
                {
                    ModelState.AddModelError(string.Empty, "There is a player with the same name.");
                    return View(model);
                }
                try
                {
                    Guid imageId = model.ImageId;
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "players");
                    }

                    var player = _converterHelper.ToPlayer(model, imageId, false);

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


        public async Task<IActionResult> Manage()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            if (user.ClubId == null || user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var players = await _playerRepository.GetPlayersByClubAsync(user.ClubId.Value);

            return View("Index", players);
        }
    }
}
