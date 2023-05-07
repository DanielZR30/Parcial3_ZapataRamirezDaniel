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


        [Authorize(Roles = "Admin")]
        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
              return _context.Vehicles != null ? 
                          View(await _context.Vehicles.ToListAsync()) :
                          Problem("Entity set 'DatabaseContext.Vehicles'  is null.");
        }

        // GET: Vehicles/Details/5
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Details(VehicleViewModel vehicleViewModel)
        {
            return View(vehicleViewModel);
        }

        // GET: Vehicles/Create
        [Authorize(Roles = "Client")]
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
            try
            {
                if (serviceViewModel.NumbrePlate.Length != 6)
                {
                    throw new Exception("la placa debe tener 6 caracteres.");
                }
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
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                if(serviceViewModel.ServiceId == Guid.Empty)
                {
                    ModelState.AddModelError(string.Empty, "Seleccione un servicio");
                }   
                serviceViewModel = new()
                {
                    Services = await _DDLHelper.GetDDLServicesAsync(),
                };
                return View(serviceViewModel);
            }

        }

        [Authorize(Roles = "Client")]
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
            VehicleDetail vehicleDetail = await _context.VehicleDetails.Include(vd => vd.Vehicle).ThenInclude(v => v.Service).OrderByDescending(v=>v.DeliveryDate).Where(v=> v.Vehicle.NumbrePlate == NumbrePlate).FirstOrDefaultAsync();
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

        // GET: Vehicles/Delete/5
  
        private bool VehicleExists(Guid id)
        {
          return (_context.Vehicles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
