using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NProducts.DAL;
using NProducts.DAL.Interfaces;
using NProducts.Data.Context;
using NProducts.Data.Models;

namespace NProducts.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private ILogger<CategoriesController> logger;        
        private IUnitOfWork unitofwork;

        public CategoriesController(IUnitOfWork unitofwork, ILogger<CategoriesController> logger)
        {            
            this.unitofwork = unitofwork;
            this.logger = logger;            
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await unitofwork.Categories.GetAllAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await unitofwork.Categories.GetAsync(id.Value);
            if (categories == null)
            {
                return NotFound();
            }

            return View(categories);

        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,Description,Picture")] Categories categories)
        {
            if (ModelState.IsValid)
            {
                this.unitofwork.Categories.Create(categories);
                await this.unitofwork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(categories);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await this.unitofwork.Categories.GetAsync(id.Value);
            if (categories == null)
            {
                return NotFound();
            }
            return View(categories);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,Description,Picture")] Categories categories)
        {
            if (id != categories.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this.unitofwork.Categories.Update(categories);
                    await this.unitofwork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriesExists(categories.CategoryId))
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
            return View(categories);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await this.unitofwork.Categories.GetAsync(id.Value);
            if (categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {            
            this.unitofwork.Categories.Delete(id);
            await this.unitofwork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriesExists(int id)
        {
            return this.unitofwork.Categories.Get(id) != null;
        }
    }
}
