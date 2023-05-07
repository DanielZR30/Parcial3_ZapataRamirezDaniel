using Carwash.DAL.Entities;
using Carwash.Enums;
using Carwash.Helpers;
using Carwash.Services;
using System.Net.Sockets;

namespace Carwash.DAL
{
    public class SeederDb
    {
        private readonly DatabaseContext _context;
        private readonly IUserHelper _userHelper;

        public SeederDb(DatabaseContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeederAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await PopulateServicesAsync();
            await PopulateUserAsync("Admin", "Role", "admin_role@yopmail.com", "3002323232", "Street Fighter 1", "102030", UserType.Admin);
            await PopulateUserAsync("User", "Role", "user_role@yopmail.com", "40056566756", "Street Fighter 2", "405060", UserType.User);
            await PopulateRoleAsync();
            await _context.SaveChangesAsync();
        }

        private async Task PopulateServicesAsync()
        {
            if (!_context.Services.Any())
            {
                _context.Services.Add(new Service { Name = "Lavada Simple", Price = 25000.00m });
                _context.Services.Add(new Service { Name = "Lavada + Polishada", Price = 50000.00m });
                _context.Services.Add(new Service { Name = "Lavada + Aspirada de Cojineria", Price = 30000.00m });
                _context.Services.Add(new Service { Name = "Lavada Full", Price = 65000.00m });
                _context.Services.Add(new Service { Name = "Lavada en seco del Motor", Price = 80000.00m });
                _context.Services.Add(new Service { Name = "Lavada Chasis", Price = 90000.00m });
            }
        }

        private async Task PopulateRoleAsync()
        {
            if (!_context.Roles.Any())
            {
                await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
                await _userHelper.CheckRoleAsync(UserType.User.ToString());
            }
        }

        private async Task PopulateUserAsync(string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            string document,
            UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    CreatedDate = DateTime.Now,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    UserType = userType,
                };
                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, user.UserType.ToString());

            }
        }
    }
}
