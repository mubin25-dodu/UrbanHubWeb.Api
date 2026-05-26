using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using UrbanHub.Data;
using UrbanHub.Entities;
using UrbanHub.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace UrbanHub.web.Controllers
{
    [Authorize]
    public class ParkingSpaceController : Controller
    {
        private readonly UrbanHubDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ParkingSpaceController(
            UrbanHubDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET
        public async Task<IActionResult> Create(int? id)
        {
            if (id != null)
            {
                ViewBag.ParkingId = id;
            }

            return View("ParkingSpace");
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParkingSpaceCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View("ParkingSpace", vm);
            }

           
            var userIdClaim = User?.FindFirst("UserID")?.Value;
            if (!int.TryParse(userIdClaim, out var currentUserId) || currentUserId <= 0)
            {
                return BadRequest("Invalid user identity.");
            }

            string imagePath = await SaveImage(vm.ImageFile);

            var point = new Point(vm.Longitude, vm.Latitude)
            {
                SRID = 4326
            };

            var parking = new ParkingSpace
            {
                Address = vm.Address,
                Location = point,
                RentPerHour = vm.RentPerHour,
                VehicleType = vm.VehicleType,
                Available = vm.Available,
                IsAvailable = vm.IsAvailable,
                Description = vm.Description,
                Image = imagePath,
                Date = DateTime.Now,
                // use the safely parsed owner id
                OwnerId = currentUserId
            };

            var roleUser = await _context.Users.FindAsync(currentUserId);
            if (roleUser == null)
            {
                return NotFound();
            }

            roleUser.Role = "Owner";

            _context.ParkingSpaces.Add(parking);

            await _context.SaveChangesAsync();
            ViewBag.ParkingId = parking.ID;
            TempData["Error"] = false;
            TempData["Message"] = "Parking Space Added Successfully";

            return RedirectToAction("Create");
        }


        [HttpGet]
        public async Task<IActionResult> MyParking()
        {
            // temporary static owner
            int ownerId = 1;

            var parking = _context.ParkingSpaces
                .Where(x => x.OwnerId == ownerId)
                .OrderByDescending(x => x.ID)
                .FirstOrDefault();

            if (parking == null)
            {
                TempData["error"] =
                    "You did not add any parking space yet.";

                return RedirectToAction("Create");
            }

            return RedirectToAction(
                "Edit",
                new { id = parking.ID }
            );
        }

        // SAVE IMAGE
        private async Task<string> SaveImage(IFormFile file)
        {
            string folder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "parking"
            );

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string fileName =
                Guid.NewGuid().ToString() +
                Path.GetExtension(file.FileName);

            string filePath =
                Path.Combine(folder, fileName);

            using (var stream =
                   new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/parking/" + fileName;
        }

        // EDIT PAGE LOAD
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var parking = await _context.ParkingSpaces
                .FirstOrDefaultAsync(x => x.ID == id);

            if (parking == null)
            {
                return NotFound();
            }

            var vm = new ParkingSpaceEditVM
            {
                ID = parking.ID,
                Address = parking.Address,
                Latitude = parking.Location!.Y,
                Longitude = parking.Location.X,
                RentPerHour = parking.RentPerHour,
                VehicleType = parking.VehicleType,
                Available = parking.Available,
                IsAvailable = parking.IsAvailable,
                Description = parking.Description,
                ExistingImage = parking.Image
            };

            return View(vm);
        }

        // UPDATE PARKING
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ParkingSpaceEditVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var parking = await _context.ParkingSpaces
                .FirstOrDefaultAsync(x => x.ID == vm.ID);

            if (parking == null)
            {
                return NotFound();
            }

            // UPDATE IMAGE
            if (vm.ImageFile != null)
            {
                string imagePath = await SaveImage(vm.ImageFile);

                parking.Image = imagePath;
            }

            var point = new Point(vm.Longitude, vm.Latitude)
            {
                SRID = 4326
            };

            parking.Address = vm.Address;
            parking.Location = point;
            parking.RentPerHour = vm.RentPerHour;
            parking.VehicleType = vm.VehicleType;
            parking.Available = vm.Available;
            parking.IsAvailable = vm.IsAvailable;
            parking.Description = vm.Description;

            await _context.SaveChangesAsync();

            TempData["success"] = "Parking Updated Successfully";

            return RedirectToAction("Create",new { id = parking.ID });
        }
    }
}