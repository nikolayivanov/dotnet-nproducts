using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NProducts.WebApi.Controllers;
using NProducts.WebApi.DTO;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NProducts.Tests.WebApi
{
    [TestClass]
    public class ProductsControllerTests
    {
        private MockRepository mockRepository;

        private Mock<ILogger<ProductsController>> mockLogger;
        private Mock<IConfiguration> mockConfiguration;
        private IConfiguration configuration;

        [TestInitialize]
        public void TestInitialize()
        {
            this.configuration = TestHelper.GetIConfiguration();
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockLogger = this.mockRepository.Create<ILogger<ProductsController>>();
            this.mockConfiguration = this.mockRepository.Create<IConfiguration>();            
        }

        [TestCleanup]
        public void TestCleanup()
        {
            //this.mockRepository.VerifyAll();
        }

        private ProductsController CreateProductsController()
        {
            return new ProductsController(
                this.mockLogger.Object,
                this.configuration);
        }

        private ProductsDTO CreateTestProductsDto()
        {
            return new ProductsDTO()
            {
                ProductName = "My first product",
                QuantityPerUnit = "1 in big container",
                UnitPrice = 12
            };
        }

        [TestMethod]
        public async Task Get_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var productsController = this.CreateProductsController();

            // Act
            var result = await productsController.Get();
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var ok = result.Result as OkObjectResult;
            var list = ok.Value as IEnumerable<ProductsDTO>;

            // Assert
            Assert.IsTrue(list != null && list.Any());
        }

        [TestMethod]
        public async Task Get_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var productsController = this.CreateProductsController();
            var all = await productsController.Get();

            Assert.IsInstanceOfType(all.Result, typeof(OkObjectResult));
            var ok = all.Result as OkObjectResult;
            var list = ok.Value as IEnumerable<ProductsDTO>;
            int id = list.First().ProductId;

            // Act
            var result = productsController.Get(id);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var p = (result.Result as OkObjectResult).Value as ProductsDTO;

            // Assert
            Assert.IsNotNull(p);
            Assert.AreEqual(id, p.ProductId);
        }

        [TestMethod]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var productsController = this.CreateProductsController();            
            ProductsDTO product = CreateTestProductsDto();

            // Act
            var okresult = productsController.Post(product);
            var productid = (int)(okresult.Result as OkObjectResult).Value;

            // Assert that product is created
            ProductsDTO p = GetDtoById(productsController, productid);

            // Assert
            Assert.IsNotNull(p);
        }

        [TestMethod]
        public void Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var productsController = this.CreateProductsController();
            ProductsDTO product = CreateTestProductsDto();
            var okresult = productsController.Post(product);
            var productid = (int)(okresult.Result as OkObjectResult).Value;

            ProductsDTO p = GetDtoById(productsController, productid);
            p.ProductName += " updated";
            var updatedname = p.ProductName;

            // Act
            productsController.Put(productid, p);

            ProductsDTO pupdated = GetDtoById(productsController, productid);

            // Assert
            Assert.AreEqual(updatedname, pupdated.ProductName);
        }

        private static ProductsDTO GetDtoById(ProductsController productsController, int productid)
        {
            var result = productsController.Get(productid);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var p = (result.Result as OkObjectResult).Value as ProductsDTO;
            return p;
        }

        [TestMethod]
        public void Delete_StateUnderTest_ExpectedBehavior()
        {
            ProductsDTO product = CreateTestProductsDto();
            
            // Arrange
            var productsController = this.CreateProductsController();
            var okresult = productsController.Post(product);
            var productid = (int)(okresult.Result as OkObjectResult).Value;            

            // Act
            productsController.Delete(productid);
            ProductsDTO p = GetDtoById(productsController, productid);

            // Assert
            Assert.IsNull(p);
        }
    }
}
