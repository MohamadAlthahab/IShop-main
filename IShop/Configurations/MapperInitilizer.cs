using AutoMapper;
using IShop.Data;
using IShop.Models;
using System.IO;

namespace IShop.Configurations
{
    public class MapperInitilizer : Profile
    {
        public MapperInitilizer() 
        {
            CreateMap<Category , CategoryDTO>().ReverseMap();
            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, CreateProductDTO>().ReverseMap();
            CreateMap<Product, UpdateProductDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<CompanyDetails, CompanyDetailsDTO>().ReverseMap();
            CreateMap<CompanyDetails, CreateCompanyDetailsDTO>().ReverseMap();
            CreateMap<Cart, CartDTO>().ReverseMap();
            CreateMap<Cart, CreateCartDTO>().ReverseMap();
            CreateMap<ConfirmCode, ConfirmCodeDTO>().ReverseMap();
            CreateMap<Street, StreetDTO>().ReverseMap();
            CreateMap<Stream, SelectStreetDTO>().ReverseMap();


        }
    }
}
