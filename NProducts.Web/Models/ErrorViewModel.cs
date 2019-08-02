using System;

namespace NProducts.Web.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string ErrorPath { get; set; }
        public string ErrorTimeStamp { get; set; }
    }
}