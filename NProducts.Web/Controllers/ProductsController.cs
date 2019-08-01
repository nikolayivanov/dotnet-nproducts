using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NProducts.Data.Common;
using NProducts.Data.Interfaces;
using NProducts.Data.Models;
using NProducts.Web.Models;

namespace NProducts.Web.Controllers
{
    public class ProductsController : Controller
    {        
        private ILogger<ProductsController> logger;
        private NProductsOptions nproductsoptions;
        private IUnitOfWork unitofwork;

        public ProductsController(IUnitOfWork unitofwork, ILogger<ProductsController> logger, IOptionsSnapshot<NProductsOptions> nproductsoptions)
        {
            this.unitofwork = unitofwork;
            this.logger = logger;
            this.nproductsoptions = nproductsoptions.Value;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var items = await this.unitofwork.Products.GetAsync(1, this.nproductsoptions.PageSize, string.Empty, string.Empty, (p) => true);
            return View(items.ConvertToProductsDTO());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await this.unitofwork.Products.GetAsync(id.Value);
            if (products == null)
            {
                return NotFound();
            }

            return View(products.ConvertToProductsDTO());
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryIdMy"] = new SelectList(this.unitofwork.Categories.GetAll(), "CategoryId", "CategoryName");
            ViewData["SupplierId"] = new SelectList(this.unitofwork.Suppliers.GetAll(), "SupplierId", "CompanyName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductsDTO products)
        {
            if (ModelState.IsValid)
            {
                var p = products.ConvertToProducts();
                this.unitofwork.Products.Create(p);
                await this.unitofwork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryIdMy"] = new SelectList(this.unitofwork.Categories.GetAll(), "CategoryId", "CategoryName", products.CategoryId);
            ViewData["SupplierId"] = new SelectList(this.unitofwork.Suppliers.GetAll(), "SupplierId", "CompanyName", products.SupplierId);
            return View(products);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await this.unitofwork.Products.GetAsync(id.Value);
            if (products == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(this.unitofwork.Categories.GetAll(), "CategoryId", "CategoryName", products.CategoryId);
            ViewData["SupplierId"] = new SelectList(this.unitofwork.Suppliers.GetAll(), "SupplierId", "CompanyName", products.SupplierId);            
            return View(products.ConvertToProductsDTO());
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] ProductsDTO products)
        {
            if (id != products.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var p = products.ConvertToProducts();
                    this.unitofwork.Products.Update(p);
                    await this.unitofwork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(this.unitofwork.Categories.GetAll(), "CategoryId", "CategoryName", products.CategoryId);
            ViewData["SupplierId"] = new SelectList(this.unitofwork.Suppliers.GetAll(), "SupplierId", "CompanyName", products.SupplierId);
            return View(products);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await this.unitofwork.Products.GetAsync(id.Value);
            if (products == null)
            {
                return NotFound();
            }

            return View(products.ConvertToProductsDTO());
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {            
            this.unitofwork.Products.Delete(id);
            await this.unitofwork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return this.unitofwork.Products.Get(id) != null;
        }        
    }
}
