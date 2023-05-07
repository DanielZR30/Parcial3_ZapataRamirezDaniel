using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Carwash.DAL;
using Carwash.DAL.Entities;
using Carwash.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Carwash.Controllers
{
    public class VehicleDetailsController : Controller
    {
        private readonly DatabaseContext _context;

        public VehicleDetailsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: VehicleDetails
        public async Task<IActionResult> Index()
        {
            return _context.VehicleDetails != null ?
                        View(await _context.VehicleDetails.Include(vd => vd.Vehicle).ThenInclude(v => v.Service).OrderBy(vd => vd.CreatedDate).ToListAsync()) :
                        Problem("Entity set 'DatabaseContext.VehicleDetails'  is null.");
        }

        // GET: VehicleDetails/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.VehicleDetails == null)
            {
                return NotFound();
            }

            var vehicleDetail = await _context.VehicleDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleDetail == null)
            {
                return NotFound();
            }

            return View(vehicleDetail);
        }

        // GET: VehicleDetails/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: VehicleDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            VehicleDetail vehicleDetail = new VehicleDetail();
            if (ModelState.IsValid)
            {
                vehicleDetail.Id = Guid.NewGuid();
                vehicleDetail.CreatedDate = DateTime.Now;
                vehicleDetail.Vehicle = vehicle;
                _context.Add(vehicleDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleDetail);
        }



        // GET: Vehicles/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid? id, Guid? vehicleId)
        {
            if (id == null || _context.VehicleDetails == null)
            {
                return NotFound();
            }

            var vehicleDetail = await _context.VehicleDetails.FindAsync(id);
            vehicleDetail.Vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicleDetail == null)
            {
                return NotFound();
            }

            VehicleDetailViewModel vehicleDetailVM = new VehicleDetailViewModel()
            {
                Id = vehicleDetail.Id,
                VehicleId = vehicleDetail.Vehicle.Id,
                CreatedDate = vehicleDetail.CreatedDate,
                DeliveryDate = vehicleDetail.DeliveryDate,
                Vehicle= vehicleDetail.Vehicle,
            };
            return View(vehicleDetailVM);
        }

       

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id, VehicleDetailViewModel vehicleDetailVM)
        {
                try
                {
                    VehicleDetail vehicleDetail = new()
                    {
                        Id = id,
                        Vehicle= await _context.Vehicles.FindAsync(vehicleDetailVM.VehicleId),
                        CreatedDate = vehicleDetailVM.CreatedDate,
                        DeliveryDate = vehicleDetailVM.DeliveryDate,
                        
                    };

                    _context.Update(vehicleDetail);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            return View(vehicleDetailVM);
        }

        private bool VehicleDetailExists(Guid id)
        {
            return (_context.VehicleDetails?.Any(e => e.Id == id)).GetValueOrDefault();

        }
    }
}
