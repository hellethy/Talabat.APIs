using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace AdminDashboard.Controllers
{
    public class TypesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TypesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var Types = await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return View(Types);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductType type)
        {
            try
            {
                await _unitOfWork.Repository<ProductType>().AddAsync(type);
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception)
            {

                ModelState.AddModelError("Name", "Please Enter a new Type");
                return View(nameof(Index), await _unitOfWork.Repository<ProductType>().GetAllAsync());
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var Type = await _unitOfWork.Repository<ProductType>().GetByIdAsync(id);
            _unitOfWork.Repository<ProductType>().Delete(Type);
            await _unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var Type = await _unitOfWork.Repository<ProductType>().GetByIdAsync(id);
            return View(Type);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductType type)
        {
            try
            {
                _unitOfWork.Repository<ProductType>().Update(type);
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));

            }
            catch (System.Exception)
            {
                ModelState.AddModelError("Name", "Please Enter a new type");
                return View(type);
            }
        }
    }
}
