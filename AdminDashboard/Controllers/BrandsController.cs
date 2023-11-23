using AdminDashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace AdminDashboard.Controllers
{
    public class BrandsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BrandsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var Brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return View(Brands);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductBrand brand)
        {
            try
            {
                await _unitOfWork.Repository<ProductBrand>().AddAsync(brand);
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception)
            {

                ModelState.AddModelError("Name", "Please Enter a new Brand");
                return View(nameof(Index), await _unitOfWork.Repository<ProductBrand>().GetAllAsync());
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var Brand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(id);
            _unitOfWork.Repository<ProductBrand>().Delete(Brand);
            await _unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var Brand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(id);
            return View(Brand);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductBrand brand)
        {
            try
            {
                _unitOfWork.Repository<ProductBrand>().Update(brand);
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));

            }
            catch (System.Exception)
            {

                ModelState.AddModelError("Name", "Please Enter a new Brand");
                return View(brand);
            }

        }
    }
}
