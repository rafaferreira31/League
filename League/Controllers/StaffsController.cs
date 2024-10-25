using League.Data.Repositories;
using League.Helpers;
using League.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace League.Controllers
{
    public class StaffsController : Controller
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;

        public StaffsController(
            IStaffRepository staffRepository,
            IClubRepository clubRepository,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper,
            IUserHelper userHelper)
        {
            _staffRepository = staffRepository;
            _clubRepository = clubRepository;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _userHelper = userHelper;
        }

        // GET: Staffs
        [Authorize]
        public IActionResult Index()
        {
            return View(_staffRepository.GetAll());
        }

        // GET: Staffs/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _staffRepository.GetByIdAsync(id.Value);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        [Authorize]
        public async Task<IActionResult> DetailsManage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _staffRepository.GetByIdAsync(id.Value);
            if (staff == null)
            {
                return NotFound();
            }
            
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if(user.ClubId == null || user.ClubId != staff.ClubId)
            {
                return RedirectToAction("Error403", "Errors");
            }

            return View(staff);
        }

        // GET: Staffs/Create
        [Authorize(Roles = "ClubRepresentant")]
        public IActionResult Create()
        {
            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Name");

            ViewBag.Functions = new SelectList(new List<string>
            {
                "Head Coach",
                "Assistant Coach",
                "Physical Trainer",
                "Doctor",
                "Physiotherapist",
                "Analyst",
            });

            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ClubRepresentant")]
        public async Task<IActionResult> Create(StaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "staffs");
                }

                var staff = _converterHelper.ToStaff(model, imageId, true);

                await _staffRepository.CreateAsync(staff);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Name");

            ViewBag.Functions = new SelectList(new List<string>
            {
                "Head Coach",
                "Assistant Coach",
                "Physical Trainer",
                "Doctor",
                "Physiotherapist",
                "Analyst",
            });

            return View(model);
        }

        // GET: Staffs/Edit/5
        [Authorize(Roles = "ClubRepresentant")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _staffRepository.GetByIdAsync(id.Value);
            if (staff == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToStaffViewModel(staff);

            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Name", model.ClubId);

            ViewBag.Functions = new SelectList(new List<string>
            {
                "Head Coach",
                "Assistant Coach",
                "Physical Trainer",
                "Doctor",
                "Physiotherapist",
                "Analyst",
            });

            return View(model);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ClubRepresentant")]
        public async Task<IActionResult> Edit(StaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "staffs");
                    }

                    var staff = _converterHelper.ToStaff(model, imageId, false);

                    await _staffRepository.UpdateAsync(staff);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _staffRepository.ExistAsync(model.Id))
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

            ViewBag.Functions = new SelectList(new List<string>
            {
                "Head Coach",
                "Assistant Coach",
                "Physical Trainer",
                "Doctor",
                "Physiotherapist",
                "Analyst",
            });

            return View(model);
        }

        // GET: Staffs/Delete/5
        [Authorize(Roles = "ClubRepresentant")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _staffRepository.GetByIdAsync(id.Value);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ClubRepresentant")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            await _staffRepository.DeleteAsync(staff);
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "ClubRepresentant")]
        public async Task<IActionResult> Manage()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            if(user == null || user.ClubId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var staff = await _staffRepository.GetStaffByClubAsync(user.ClubId.Value);

            return View("Index", staff);
        }
    }
}
