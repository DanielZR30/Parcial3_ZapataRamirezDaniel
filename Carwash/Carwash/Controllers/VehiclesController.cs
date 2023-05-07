using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Carwash.DAL;
using Carwash.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Carwash.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Carwash.Models;
using Carwash.Enums;

namespace Carwash.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IDropDownListHelper _DDLHelper;
        private readonly IUserHelper _userHelper;
        public VehiclesController(DatabaseContext context, IDropDownListHelper dropDownListHelper)
        {
            _context = context;
            _DDLHelper = dropDownListHelper;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
              return _context.Vehicles != null ? 
                          View(await _context.Vehicles.ToListAsync()) :
                          Problem("Entity set 'DatabaseContext.Vehicles'  is null.");
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(VehicleViewModel vehicleViewModel)
        {
            return View(vehicleViewModel);
        }

        // GET: Vehicles/Create
        public async Task<IActionResult> Create()
        {

            ServiceViewModel serviceViewModel = new()
            {
                Services = await _DDLHelper.GetDDLServicesAsync(),
            };
            return View(serviceViewModel);
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceViewModel serviceViewModel)
        {
            Vehicle vehicle;
            VehicleDetail vehicleDetail;
            vehicle = new Vehicle()
            {
                Id = Guid.NewGuid(),
                Service = await _context.Services.FindAsync(serviceViewModel.ServiceId),
                Owner = serviceViewModel.Owner,
                NumbrePlate = serviceViewModel.NumbrePlate
            };

            vehicleDetail = new VehicleDetail()
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                DeliveryDate = null,
                Vehicle = vehicle
            };

            _context.Add(vehicle);
            _context.VehicleDetails.Add(vehicleDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Search()
        {
            return View();
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(string NumbrePlate)
        {
            VehicleDetail vehicleDetail = await _context.VehicleDetails.Include(vd => vd.Vehicle).ThenInclude(v => v.Service).Where(v=> v.Vehicle.NumbrePlate == NumbrePlate).FirstOrDefaultAsync();
            if (vehicleDetail != null)
            {
                Vehicle vehicle = vehicleDetail.Vehicle;
                Service service = vehicle.Service;
                VehicleViewModel vehicleViewModel = new VehicleViewModel()
                {

                    Id = vehicle.Id,
                    NumbrePlate = NumbrePlate,
                    Owner = vehicle.Owner,
                    CreatedDate = vehicleDetail.CreatedDate,
                    ServiceName = service.Name,
                    Price = service.Price,
                    DeliveryDate = vehicleDetail.DeliveryDate
                };
                return View("Details", vehicleViewModel);
            }
            ModelState.AddModelError("","No se encuentra el vehiculo");
            return View();
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Vehicles == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Owner,NumbrePlate,Id")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
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
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Vehicles == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Vehicles == null)
            {
                return Problem("Entity set 'DatabaseContext.Vehicles'  is null.");
            }
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(Guid id)
        {
          return (_context.Vehicles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
