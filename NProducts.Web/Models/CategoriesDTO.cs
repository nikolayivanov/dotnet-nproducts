using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NProducts.Web.Models
{
    public partial class CategoriesDTO
    {
        public int CategoryId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
    }
}
