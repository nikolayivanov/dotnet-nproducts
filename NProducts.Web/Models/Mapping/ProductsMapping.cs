using AutoMapper;
using NProducts.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NProducts.Web.Models
{
    public static class ProductsMapping
    {
        public static Products ConvertToProducts(this ProductsDTO item)
        {
            // Настройка AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductsDTO, Products>();
            });

            var mapper = config.CreateMapper();
            return mapper.Map<ProductsDTO, Products>(item);
        }

        public static ProductsDTO ConvertToProductsDTO(this Products item)
        {
            // Настройка AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Products, ProductsDTO>()
                .ForMember(dto => dto.SupplierCompanyName, conf => conf.MapFrom(ol => ol.Supplier.CompanyName))
                .ForMember(dto => dto.CategoryName, conf => conf.MapFrom(ol => ol.Category.CategoryName));
            });

            var mapper = config.CreateMapper();
            return mapper.Map<Products, ProductsDTO>(item);
        }

        public static IEnumerable<ProductsDTO> ConvertToProductsDTO(this IEnumerable<Products> items)
        {
            // Настройка AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Products, ProductsDTO>()
                .ForMember(dto => dto.SupplierCompanyName, conf => conf.MapFrom(ol => ol.Supplier.CompanyName))
                .ForMember(dto => dto.CategoryName, conf => conf.MapFrom(ol => ol.Category.CategoryName));
            });

            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<Products>, IEnumerable<ProductsDTO>>(items);
        }
    }
}
