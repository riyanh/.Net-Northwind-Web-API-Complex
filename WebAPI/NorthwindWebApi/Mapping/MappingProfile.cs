using AutoMapper;
using Northwind.Entities.Models;
using Northwind.Entities.DataTransferObject;

namespace NorthwindWebApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //get category
            CreateMap<Category, CategoryDto>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<Product, ProductDto>();
            CreateMap<OrderDetail, OrdersDetailDto>();
            //post category
            CreateMap<CategoryDto, Category>();
            CreateMap<CategoryUpdateDto, Category>().ReverseMap();
            CreateMap<CustomerDto, Customer>().ReverseMap();
            CreateMap<CustomerUpdateDto, Customer>().ReverseMap();
            CreateMap<CartDto, OrderDetail>().ReverseMap();
            CreateMap<OrdersDetailDto, OrderDetail>();
            CreateMap<OrderDto, Order>().ReverseMap();
        }
    }
}
