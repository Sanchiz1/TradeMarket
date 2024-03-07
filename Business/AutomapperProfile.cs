using AutoMapper;
using Business.Models;
using Data.Entities;
using System.Linq;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Receipt, ReceiptModel>()
                .ForMember(rm => rm.ReceiptDetailsIds, r => r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            CreateMap<Product, ProductModel>()
                .ForMember(rm => rm.Id, r => r.MapFrom(x => x.Id))
                .ForMember(rm => rm.Price, r => r.MapFrom(x => x.Price))
                .ForMember(rm => rm.ProductName, r => r.MapFrom(x => x.ProductName))
                .ForMember(rm => rm.ProductCategoryId, r => r.MapFrom(x => x.ProductCategoryId))
                .ForMember(rm => rm.ProductCategoryId, r => r.MapFrom(x => x.Category.Id))
                .ForMember(rm => rm.CategoryName, r => r.MapFrom(x => x.Category.CategoryName))
                .ForMember(rm => rm.ReceiptDetailIds, r => r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            CreateMap<ReceiptDetail, ReceiptDetailModel>()
                .ReverseMap();

            CreateMap<Customer, CustomerModel>()
                .ForMember(rm => rm.Id, r => r.MapFrom(x => x.Person.Id))
                .ForMember(rm => rm.Name, r => r.MapFrom(x => x.Person.Name))
                .ForMember(rm => rm.Surname, r => r.MapFrom(x => x.Person.Surname))
                .ForMember(rm => rm.BirthDate, r => r.MapFrom(x => x.Person.BirthDate))
                .ForMember(rm => rm.ReceiptsIds, r => r.MapFrom(x => x.Receipts.Select(rd => rd.Id)))
                .ReverseMap();

            CreateMap<ProductCategory, ProductCategoryModel>()
                .ForMember(rm => rm.ProductIds, r => r.MapFrom(x => x.Products.Select(rd => rd.Id)))
                .ReverseMap();
        }
    }
}