using System;

namespace NProducts.WebApi.Models
{
    public abstract class FilterModel : ICloneable
    {
        public FilterModel()
        {
            this.Page = 1;
            this.PageSize = 10;
        }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public string OrderByFieldName { get; set; }

        public string OrderByDirection { get; set; }

        public abstract object Clone();
    }
}