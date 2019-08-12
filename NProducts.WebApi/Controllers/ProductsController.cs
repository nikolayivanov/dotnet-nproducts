using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NProducts.Data.Interfaces;
using NProducts.Data.Models;
using NProducts.WebApi.DTO;
using NProducts.WebApi.Models;

namespace NProducts.WebApi.Controllers
{
    /// <summary>
    /// RESTful service for products in Northwind database.
    /// </summary>    
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private ILogger<ProductsController> logger;        
        private IUnitOfWork unitofwork;

        public ProductsController(ILogger<ProductsController> logger, IUnitOfWork unitofwork)
        {
            this.logger = logger;            
            this.unitofwork = unitofwork;
        }

        /// <summary>
        /// GET api/products
        /// Gets this all products.
        /// </summary>
        /// <returns>List of all products in the database</returns>
        [HttpGet]
        public async Task<ActionResult<PagedCollectionResponse<ProductsDTO>>> Get([FromQuery]ProductsFilterModel filter)
        {
            Func<Products, bool> wherecond = (p) =>
            {
                return string.IsNullOrEmpty(filter.ProductName) || p.ProductName.StartsWith(filter.ProductName);
            };

            var result = await unitofwork.Products.GetAsync(filter.Page, filter.PageSize, filter.OrderByFieldName, filter.OrderByDirection, wherecond);            
            return Ok(result.ConvertToProductsDTOList());
        }

        /// <summary>
        /// GET api/products/5
        /// Gets a products with the specified identifier.
        /// </summary>
        /// <param name="id">The product identifier.</param>
        /// <returns>Products object.</returns>
        [HttpGet("{id}")]
        public ActionResult<ProductsDTO> Get(int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("id.incorrect", $"id {id} has incorrect value.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = unitofwork.Products.Get(id);            
            return Ok(result.ConvertToProductsDTO());
        }

        /// <summary>
        /// POST api/products
        /// Creates the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns>Returns Id of new products.</returns>
        [HttpPost]
        public ActionResult<int> Post([FromBody] ProductsDTO product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var p = product.ConvertToProducts();
            unitofwork.Products.Create(p);
            unitofwork.Save();
            this.logger.LogInformation("New product was created ProductId=[{p.ProductId}] ProductName=[{product.ProductName}]", p.ProductId, product.ProductName);
            this.logger.LogInformation(new EventId(123, "Product was created successfully."),
                "New product was created ProductId=[{p.ProductId}] ProductName=[{product.ProductName}]", p.ProductId, product.ProductName);
            return Ok(p.ProductId);
        }

        /// <summary>
        /// PUT api/products/5
        /// Updates Products with specified id the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="product">The product.</param>
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ProductsDTO product)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("id.incorrect", $"id {id} has incorrect value.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pinitial = unitofwork.Products.Get(product.ProductId);
            var p = product.ConvertToProducts(pinitial);

            unitofwork.Products.Update(p);
            unitofwork.Save();

            return Ok();
        }

        /// <summary>
        /// DELETE api/products/5
        /// Deletes the product with specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("id.incorrect", $"id {id} has incorrect value.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            unitofwork.Products.Delete(id);
            unitofwork.Save();

            return Ok();
        }
    }
}
