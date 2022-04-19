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
            //post category
            CreateMap<CategoryDto, Category>();
            CreateMap<CustomerDto, Customer>();
        }
    }
}
