using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NProducts.WebApi.Models
{
    public class ProductsFilterModel: FilterModel
    {
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
        public int CategoryId { get; set; }
        public override object Clone()
        {
            var jsonString = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject(jsonString, this.GetType());
        }
    }
}
