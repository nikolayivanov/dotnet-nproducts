using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NProducts.DAL;
using NProducts.Data.Common;
using NProducts.Data.Interfaces;
using NProducts.Web.Controllers;
using NProducts.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NProducts.Tests.Extentions;
using NProducts.Data.Models;

namespace NProducts.Tests.Web
{
    [TestClass]
    public class CategoriesControllerTests
    {
        private IConfiguration configuration;        
        private ILogger<CategoriesController> logger;

        [TestInitialize]
        public void TestInitialize()
        {
            this.configuration = TestHelper.GetIConfiguration();            
            this.logger = CreateLogger();

            var unnitofwork = GetNorthwindUnitOfWork();
            unnitofwork.InitialDatabaseForUnitTests();
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        [TestMethod]
        public async Task Index_ReturnsAViewRersult_WhenCorrectRequest()
        {
            // Arrange
            var categoriesController = this.CreateCategoriesController();

            // Act
            var result = await categoriesController.Index();

            // Assert            
            var viewResult = MyAssert.IsType<ViewResult>(result);
            var model = MyAssert.IsType<IEnumerable<CategoriesDTO>>(viewResult.ViewData.Model);
        }

        [TestMethod]
        public async Task Details_ReturnsViewResult_WhenPassExistingId()
        {
            // Arrange
            var categoriesController = this.CreateCategoriesController();
            int? id = this.CreateTestCategory("MyTestCategory"); ;

            // Act
            var result = await categoriesController.Details(id);

            // Assert
            var viewResult = MyAssert.IsType<ViewResult>(result);
            var model = MyAssert.IsType<CategoriesDTO>(viewResult.ViewData.Model);
            Assert.AreEqual("MyTestCategory", model.CategoryName);

            this.DeleteTestCategory(id.Value);
        }

        [TestMethod]
        public async Task Details_ReturnsNotFoundResult_WhenPassUnknownOrNullId()
        {
            // Arrange
            var categoriesController = this.CreateCategoriesController();
            int? id = (new Random()).Next();

            // Act
            var result = await categoriesController.Details(id);

            // Assert
            var viewResult = MyAssert.IsType<NotFoundResult>(result);
        }

        [TestMethod]
        public void Create_ReturnsViewResult_Any()
        {
            // Arrange
            var categoriesController = this.CreateCategoriesController();

            // Act
            var result = categoriesController.Create();

            // Assert
            var viewResult = MyAssert.IsType<ViewResult>(result);            
        }

        [TestMethod]
        public async Task Create_RedirectToActionIndex_WhenPassCorrectCategory()
        {
            // Arrange
            var categoriesController = this.CreateCategoriesController();
            CategoriesDTO categories = new CategoriesDTO()
            {
                CategoryName = "MyTestCategory1",
                Description = "MyTestCategoryDescription1"
            };

            // Act
            var result = await categoriesController.Create(categories);

            // Assert
            var redirectResult = MyAssert.IsType<RedirectToActionResult>(result);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [TestMethod]
        public async Task Create_ReturnsTheSameCreateViewResult_WhenModelStateIsNotValid()
        {
            // Arrange
            var categoriesController = this.CreateCategoriesController();
            CategoriesDTO categories = new CategoriesDTO();

            // Act
            TestHelper.TryValidateModel(categoriesController, categories);
            var result = await categoriesController.Create(categories);

            // Assert
            var viewResult = MyAssert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public async Task Edit_ReturnsNotFoundResult_WhenPassUnknownOrNullId()
        {
            // Arrange
            var categoriesController = this.CreateCategoriesController();
            int? id = this.CreateTestCategory("TestCatForEdit1");

            // Act
            var result = await categoriesController.Edit(id);

            // Assert
            var viewResult = MyAssert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public async Task Edit_ReturnsTheSameCreateViewResult_WhenModelStateIsNotValid()
        {
            // Arrange
            var categoriesController = this.CreateCategoriesController();
            int id = 0;
            CategoriesDTO categories = new CategoriesDTO();

            // Act
            TestHelper.TryValidateModel(categoriesController, categories);
            var result = await categoriesController.Edit(categories);

            // Assert
            var viewResult = MyAssert.IsType<ViewResult>(result);
            Assert.AreEqual("Edit", viewResult.ViewName);
        }        

        [TestMethod]
        public async Task Delete_ReturnsNotFoundResult_WhenUnknownId()
        {
            // Arrange
            var categoriesController = this.CreateCategoriesController();
            int? id = (new Random()).Next();

            // Act
            var result = await categoriesController.Delete(id);

            // Assert
            var viewResult = MyAssert.IsType<NotFoundResult>(result);
        }

        [TestMethod]
        public async Task Delete_ReturnsViewResult_WhenCorrectId()
        {
            // Arrange
            var categoriesController = this.CreateCategoriesController();
            int? id = this.CreateTestCategory("DeleteTestCat1");

            // Act
            var result = await categoriesController.Delete(id);

            // Assert
            var viewResult = MyAssert.IsType<ViewResult>(result);
            var model = MyAssert.IsType<CategoriesDTO>(viewResult.ViewData.Model);
            Assert.AreEqual("DeleteTestCat1", model.CategoryName);

            // Clean
            this.DeleteTestCategory(id.Value);
        }

        [TestMethod]
        public async Task DeleteConfirmed_RedirectToActionResult_WhenCorrectId()
        {
            // Arrange
            var categoriesController = this.CreateCategoriesController();
            int id = this.CreateTestCategory("DeleteTestCat1");

            // Act
            var result = await categoriesController.DeleteConfirmed(id);

            // Assert
            var redirectResult = MyAssert.IsType<RedirectToActionResult>(result);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }



        #region Helper Methods

        private ILogger<CategoriesController> CreateLogger()
        {
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            return factory.CreateLogger<CategoriesController>();
        }

        private NorthwindUnitOfWork GetNorthwindUnitOfWork()
        {
            NProductsOptions options = new NProductsOptions() { PageSize = 20 };
            IOptionsSnapshot<NProductsOptions> ioptions = Options.Create(options) as IOptionsSnapshot<NProductsOptions>;
            return new NorthwindUnitOfWork(this.configuration, ioptions);
        }

        private CategoriesController CreateCategoriesController()
        {
            return new CategoriesController(
                GetNorthwindUnitOfWork(),
                this.logger);
        }

        private int CreateTestCategory(string catname)
        {
            var unitofwork = GetNorthwindUnitOfWork();
            var cat = new Categories()
            {
                CategoryName = catname
            };

            unitofwork.Categories.Create(cat);
            unitofwork.Save();
            return cat.CategoryId;
        }

        private void DeleteTestCategory(int value)
        {
            var unitofwork = GetNorthwindUnitOfWork();
            unitofwork.Categories.Delete(value);
            unitofwork.Save();
        }

        #endregion
    }
}
