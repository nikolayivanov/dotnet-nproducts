using System;
using System.Collections.Generic;
using System.Text;

namespace NProducts.Data.Common
{
    public class ImageCachingOptions
    {
        public string CacheDirectoryPath { get; set; }
        public int MaxCountImagesToCache { get; set; }
        public int CacheExpirationTimeInMinutes { get; set; }
    }
}
