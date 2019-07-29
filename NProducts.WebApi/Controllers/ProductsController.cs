using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NProducts.DAL;
using NProducts.Data.Models;
using NProducts.WebApi.DTO;
using NProducts.WebApi.Models;

namespace NProducts.WebApi.Controllers
{
    /// <summary>
    /// RESTful service for products in Northwind database.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private ILogger<ProductsController> logger;
        private IConfiguration configuration;

        public ProductsController(ILogger<ProductsController> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        // GET api/products
        /// <summary>
        /// Gets this all products.
        /// </summary>
        /// <returns>List of all products in the database</returns>
        [HttpGet]
        public async Task<ActionResult<PagedCollectionResponse<ProductsDTO>>> Get([FromQuery]ProductsFilterModel filter)
        {
            var mapper = GetProductsToProductsDTOMapper();
            using (var unitofwork = new NorthwindUnitOfWork(this.configuration))
            {
                Func<Products, bool> wherecond = (p) =>
                {
                    return string.IsNullOrEmpty(filter.ProductName) || p.ProductName.StartsWith(filter.ProductName);
                };

                var result = await unitofwork.Products.GetAsync(filter.Page, filter.PageSize, filter.OrderByFieldName, filter.OrderByDirection, wherecond);
                var resultdto = mapper.Map<IEnumerable<Products>, IEnumerable<ProductsDTO>>(result);
                return Ok(resultdto);
            }
        }

        // GET api/products/5
        /// <summary>
        /// Gets a products with the specified identifier.
        /// </summary>
        /// <param name="id">The product identifier.</param>
        /// <returns>Products object.</returns>
        [HttpGet("{id}")]
        public ActionResult<ProductsDTO> Get(int id)
        {
            var mapper = GetProductsToProductsDTOMapper();
            using (var unitofwork = new NorthwindUnitOfWork(this.configuration))
            {
                var result = unitofwork.Products.Get(id);
                var dto = mapper.Map<Products, ProductsDTO>(result);
                return Ok(dto);
            }
        }

        // POST api/products
        [HttpPost]
        public ActionResult<int> Post([FromBody] ProductsDTO product)
        {
            IMapper mapper = GetProductsDTOtoProductsMapper();
            var p = mapper.Map<ProductsDTO, Products>(product);
            using (var unitofwork = new NorthwindUnitOfWork(this.configuration))
            {
                unitofwork.Products.Create(p);
                unitofwork.Save();
                return Ok(p.ProductId);
            }
        }

        // PUT api/products/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] ProductsDTO product)
        {
            IMapper mapper = GetProductsDTOtoProductsMapper();
            var p = mapper.Map<ProductsDTO, Products>(product);
            using (var unitofwork = new NorthwindUnitOfWork(this.configuration))
            {
                unitofwork.Products.Update(p);
                unitofwork.Save();
            }
        }

        // DELETE api/products/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var unitofwork = new NorthwindUnitOfWork(this.configuration))
            {
                unitofwork.Products.Delete(id);
                unitofwork.Save();
            }
        }

        private static IMapper GetProductsDTOtoProductsMapper()
        {
            // Настройка AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductsDTO, Products>();
            });

            var mapper = config.CreateMapper();
            return mapper;
        }

        private static IMapper GetProductsToProductsDTOMapper()
        {
            // Настройка AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Products, ProductsDTO>();
            });

            var mapper = config.CreateMapper();
            return mapper;
        }
    }
}
