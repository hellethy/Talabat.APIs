using AdminDashboard.Models;
using AutoMapper;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace AdminDashboard.Helpers
{
    public class MapsProfile:Profile
    {
        public MapsProfile()
        {
            CreateMap<ProductsViewModel, Product>().ReverseMap();
        }
    }
}
