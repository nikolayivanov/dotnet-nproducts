using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NProducts.DAL;
using NProducts.Data.Interfaces;
using NProducts.Data.Models;
using NProducts.Web.Models;

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
            var items = await unitofwork.Categories.GetAllAsync();
            return View(items.ConvertToCategoriesDTO());
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

            return View(categories.ConvertToCategoriesDTO());

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
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,Description,Picture")] CategoriesDTO categories)
        {
            if (ModelState.IsValid)
            {
                var c = categories.ConvertToCategories();
                this.unitofwork.Categories.Create(c);
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
            return View(categories.ConvertToCategoriesDTO());
        }

        // GET: Categories/GetPicture/5
        public IActionResult GetPicture(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = this.unitofwork.Categories.Get(id.Value);
            if (categories == null)
            {
                return NotFound();
            }

            return File(categories.Picture, "image/jpeg", categories.CategoryName + ".jpeg");
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("CategoryId,CategoryName,Description,Picture")] CategoriesDTO categories, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                if (categories.CategoryId < 0)
                {
                    return NotFound();
                }

                try
                {
                    byte[] imageData = null;

                    if (uploadedFile != null)
                    {
                        if (string.Compare(uploadedFile.ContentType, "image/jpeg", true) != 0)
                        {
                            ModelState.AddModelError("OnlyJpeg", "Invalid Picture format. Only jpeg is allowed.");
                            return View(categories);
                        }

                        this.logger.LogDebug("Category picture change: $uploadedFile.FileName", uploadedFile.FileName);
                        
                        // считываем переданный файл в массив байтов
                        using (var binaryReader = new BinaryReader(uploadedFile.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)uploadedFile.Length);
                        }
                        
                        categories.Picture = imageData;
                    }

                    this.unitofwork.Categories.Update(categories.ConvertToCategories());
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

            return View("Edit", categories);
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

            return View(categories.ConvertToCategoriesDTO());
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
