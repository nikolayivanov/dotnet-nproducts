/* 
 * My NProducts API
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: v1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using RestSharp;
using NUnit.Framework;

using IO.Swagger.Client;
using IO.Swagger.Api;
using IO.Swagger.Model;

namespace IO.Swagger.Test
{
    /// <summary>
    ///  Class for testing ProductsApi
    /// </summary>
    /// <remarks>
    /// This file is automatically generated by Swagger Codegen.
    /// Please update the test case below to test the API endpoint.
    /// </remarks>
    [TestFixture]
    public class ProductsApiTests
    {
        private ProductsApi instance;

        /// <summary>
        /// Setup before each unit test
        /// </summary>
        [SetUp]
        public void Init()
        {
            instance = new ProductsApi();
        }

        /// <summary>
        /// Clean up after each unit test
        /// </summary>
        [TearDown]
        public void Cleanup()
        {

        }

        /// <summary>
        /// Test an instance of ProductsApi
        /// </summary>
        [Test]
        public void InstanceTest()
        {
            // TODO uncomment below to test 'IsInstanceOfType' ProductsApi
            //Assert.IsInstanceOfType(typeof(ProductsApi), instance, "instance is a ProductsApi");
        }

        
        /// <summary>
        /// Test Delete
        /// </summary>
        [Test]
        public void DeleteTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //int? id = null;
            //instance.Delete(id);
            
        }
        
        /// <summary>
        /// Test Get
        /// </summary>
        [Test]
        public void GetTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //string productName = null;
            //string supplierName = null;
            //int? categoryId = null;
            //int? page = null;
            //int? pageSize = null;
            //string orderByFieldName = null;
            //string orderByDirection = null;
            //var response = instance.Get(productName, supplierName, categoryId, page, pageSize, orderByFieldName, orderByDirection);
            //Assert.IsInstanceOf<PagedCollectionResponseProductsDTO> (response, "response is PagedCollectionResponseProductsDTO");
        }
        
        /// <summary>
        /// Test Get_0
        /// </summary>
        [Test]
        public void Get_0Test()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //int? id = null;
            //var response = instance.Get_0(id);
            //Assert.IsInstanceOf<ProductsDTO> (response, "response is ProductsDTO");
        }
        
        /// <summary>
        /// Test Post
        /// </summary>
        [Test]
        public void PostTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //ProductsDTO product = null;
            //var response = instance.Post(product);
            //Assert.IsInstanceOf<int?> (response, "response is int?");
        }
        
        /// <summary>
        /// Test Put
        /// </summary>
        [Test]
        public void PutTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //int? id = null;
            //ProductsDTO product = null;
            //instance.Put(id, product);
            
        }
        
    }

}