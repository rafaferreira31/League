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
using League.Helpers;
using League.Models;

namespace League.Controllers
{
    public class StaffsController : Controller
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IClubRepository _clubRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public StaffsController(
            IStaffRepository staffRepository,
            IClubRepository clubRepository,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _staffRepository = staffRepository;
            _clubRepository = clubRepository;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        // GET: Staffs
        public IActionResult Index()
        {
            return View(_staffRepository.GetAll());
        }

        // GET: Staffs/Details/5
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

        // GET: Staffs/Create
        public IActionResult Create()
        {
            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "Staffs");
                }

                var staff = _converterHelper.ToStaff(model, path, true);

                await _staffRepository.CreateAsync(staff);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Name");
            return View(model);
        }

        // GET: Staffs/Edit/5
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
            return View(model);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = string.Empty;

                    if(model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "Staffs");
                    }

                    var staff = _converterHelper.ToStaff(model, path, false);

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

            return View(model);
        }

        // GET: Staffs/Delete/5
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staff = await _staffRepository.GetByIdAsync(id);
            await _staffRepository.DeleteAsync(staff);
            return RedirectToAction(nameof(Index));
        }
    }
}
