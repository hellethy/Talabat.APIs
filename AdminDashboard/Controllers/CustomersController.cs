using AdminDashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace AdminDashboard.Controllers
{
	public class CustomersController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public CustomersController(UserManager<AppUser> userManager , RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}
		public async Task<IActionResult> Index()
		{
			var Users = await _userManager.Users.Select(U => new UserViewModel
			{
				Id = U.Id,
				UserName = U.UserName,
				Email = U.Email,
				DisplayName = U.DisplayName,
				PhoneNumber = U.PhoneNumber,
				Roles = _userManager.GetRolesAsync(U).Result
			}).ToListAsync();
			return View(Users);
		}

        public async Task<IActionResult> Edit(string id)
        {
            var Customer = await _userManager.FindByIdAsync(id);
            var AllRoles = await _roleManager.Roles.ToListAsync();

            var ViewModel = new UserRolesViewModel()
            {
                UserId = Customer.Id,
                UserName = Customer.DisplayName,
                Roles = AllRoles.Select(R => new RoleViewModel
                {
                    Id = R.Id,
                    Name = R.Name,
                    IsSelected = _userManager.IsInRoleAsync(Customer, R.Name).Result
                }).ToList()
            };
            
            return View(ViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserRolesViewModel model)
        {
            var User = await _userManager.FindByIdAsync(model.UserId);
            var UserRoles = await _userManager.GetRolesAsync(User);
            foreach (var Role in model.Roles)
            {
                // Remove Role From User
                if (UserRoles.Any(R => R == Role.Name) && !Role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(User, Role.Name);

                // Add Role To User
                if (!UserRoles.Any(R => R == Role.Name) && Role.IsSelected)
                    await _userManager.AddToRoleAsync(User, Role.Name);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
