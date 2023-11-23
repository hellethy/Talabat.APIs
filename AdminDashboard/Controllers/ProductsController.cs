using AdminDashboard.Helpers;
using AdminDashboard.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace AdminDashboard.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var Products = await _unitOfWork.Repository<Product>().GetAllAsync();
            var MappedProduct = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductsViewModel>>(Products);
            return View(MappedProduct);
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(ProductsViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.Image != null)
                    model.PictureUrl = ImageSettings.UploadFile(model.Image, "products");
                else
                    model.PictureUrl = "images\\Products\\default-image.jpg";
                var MappedProduct = _mapper.Map<ProductsViewModel, Product>(model);
                await _unitOfWork.Repository<Product>().AddAsync(MappedProduct);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            var MappedProduct = _mapper.Map<Product, ProductsViewModel>(product);
            return View(MappedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, ProductsViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    if (model.PictureUrl != null)
                    {
                        ImageSettings.DeleteFile(model.PictureUrl, "Products");
                        model.PictureUrl = ImageSettings.UploadFile(model.Image, "products");
                    }
                    else
                    {
                        model.PictureUrl = ImageSettings.UploadFile(model.Image, "products");
                    }
                }

                var mappedProduct = _mapper.Map<ProductsViewModel, Product>(model);
                _unitOfWork.Repository<Product>().Update(mappedProduct);
                var result = await _unitOfWork.Complete();
                if (result > 0)
                    return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            var MappedProduct = _mapper.Map<Product, ProductsViewModel>(product);
            return View(MappedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, ProductsViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            try
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(model.Id);
                ImageSettings.DeleteFile(product.PictureUrl, "Products");
                _unitOfWork.Repository<Product>().Delete(product);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            catch (System.Exception)
            {

                return View(model);
            }
        }
    }
}
