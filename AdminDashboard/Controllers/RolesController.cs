using AdminDashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AdminDashboard.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var Roles =  await _roleManager.Roles.ToListAsync();
            return View(Roles);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var RoleExists = await _roleManager.RoleExistsAsync(model.Name);
                if (!RoleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
                    return RedirectToAction(nameof(Index));
                }
                else 
                {
                    ModelState.AddModelError("Name", "Roles Is Exists");
                    return View("Index", await _roleManager.Roles.ToListAsync());
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(role);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var Mappedrole = new RoleViewModel()
            {
                Name = role.Name
            };
            return View(Mappedrole);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var RoleExists = await _roleManager.RoleExistsAsync(model.Name);
                if (!RoleExists)
                {
                    var role = await _roleManager.FindByIdAsync(model.Id);
                    role.Name = model.Name;
                    await _roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Name", "Roles Is Exists");
                    return View("Index", await _roleManager.Roles.ToListAsync());
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
