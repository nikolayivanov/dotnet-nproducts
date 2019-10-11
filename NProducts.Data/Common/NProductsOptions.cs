using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NProducts.Data.Common
{
    public class NProductsOptions
    {
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Need to log Action parametrs.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [log action parameters]; otherwise, <c>false</c>.
        /// </value>
        public bool LogActionParameters { get; set; }
    }
}
