using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace NProducts.Tests.Extentions
{
    public static class MyAssert
    {
        public static T IsType<T>(Object obj)
        {
            Assert.IsInstanceOfType(obj, typeof(T));
            return (T)obj;
        }
    }
}
