using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NProducts.Web.Models
{
    public partial class ProductsDTO
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "ProductName is required field.")]
        [Remote(action: "VerifyProductName", controller: "Products")]
        public string ProductName { get; set; }

        public int? SupplierId { get; set; }
        [Display(Name = "Supplier")]
        public string SupplierCompanyName { get; set; }
        public int? CategoryId { get; set; }
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
    }
}
