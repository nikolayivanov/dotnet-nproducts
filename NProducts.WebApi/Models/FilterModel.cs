using System;
using System.ComponentModel.DataAnnotations;

namespace NProducts.WebApi.Models
{
    public abstract class FilterModel : ICloneable
    {
        public FilterModel()
        {
            this.Page = 1;
            this.PageSize = 10;
        }

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be between 1 and int Max.")]
        public int Page { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page size must be between 1 and int Max.")]
        public int PageSize { get; set; }

        public string OrderByFieldName { get; set; }

        public string OrderByDirection { get; set; }

        public abstract object Clone();
    }
}