using System;
using System.Collections.Generic;

namespace NProducts.WebApi.DTO
{
    public partial class ProductsDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }

        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }
        public string SupplierName { get; set; }
        
    }
}
