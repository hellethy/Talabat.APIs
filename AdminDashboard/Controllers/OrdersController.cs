using AdminDashboard.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Orders;

namespace AdminDashboard.Controllers
{
	public class OrdersController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public OrdersController(IUnitOfWork unitOfWork )
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<IActionResult> Index()
        {
			var spec = new OrderWithItemsAndDeliveryMethodSpecifications();
			var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return View(orders);
		}


        [HttpPost]
        public async Task<IActionResult> GetUserOrder(string BuyerEmail)
        {
            var spec = new OrderWithItemsAndDeliveryMethodSpecifications(BuyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return View(nameof(Index), orders);
        }

    }
}
