using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NProducts.WebApi.DTO
{
    public partial class ProductsDTO : IValidatableObject
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is required field.")]
        [StringLength(40, ErrorMessage = "Product Name length can't be more than 40.")]
        public string ProductName { get; set; }

        public string QuantityPerUnit { get; set; }

        [Range(0, 999.99, ErrorMessage = "Unit Price cannot be more than 999.00")]
        public decimal? UnitPrice { get; set; }

        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }
        public string SupplierCompanyName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var valresult = new List<ValidationResult>();
            if (SupplierId.HasValue && SupplierId.Value == 0)
            {
                valresult.Add(new ValidationResult("SupplierId is zero."));
            }

            return valresult;
        }
    }
}
