using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using NProducts.Data.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace NProducts.Web.Middleware
{
    public class ImageCachingMiddleware
    {
        // key - request path, value - image name
        private static ConcurrentDictionary<string, string> CachedImagesDict = new ConcurrentDictionary<string, string>();

        private readonly RequestDelegate _next;

        public ImageCachingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(
            HttpContext httpContext,
            ILogger<ImageCachingMiddleware> logger,
            IOptionsSnapshot<ImageCachingOptions> imagecacheingoptions)
        {
            logger.LogDebug("ImageCachingMiddleware.Invoke - before _next() call.");

            // if we see GetPicture request
            if (httpContext.Request != null && httpContext.Request.Path.HasValue &&
                httpContext.Request.Path.Value.Contains("GetPicture"))
            {
                var options = imagecacheingoptions.Value;

                // Copy a pointer to the original response body stream
                var originalBodyStream = httpContext.Response.Body;

                // Create a new memory stream... Finally we will copy it to originalBodyStream
                using (var responseBody = new MemoryStream())
                {
                    //...and use that for the temporary response body
                    httpContext.Response.Body = responseBody;

                    // check that image exists in cache directory
                    // If next request accessing to the same image, get it from cache directory
                    var filenamefromcache = string.Empty;
                    if (CachedImagesDict.TryGetValue(httpContext.Request.Path, out filenamefromcache))
                    {
                        // write image from cache and headers into response
                        await this.WriteImageFromCacheToResponse(options, httpContext, filenamefromcache);

                        //We need to reset the reader for the response so that the client can read it.
                        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
                    }
                    else
                    {
                        await _next(httpContext);

                        // Read the response from the server
                        var responseImageByteArray = await ReadResponseBody(httpContext.Response);

                        // Check Content-Type every response and if returned any valid image format
                        if (string.Compare(httpContext.Response.ContentType, "image/jpeg") == 0)
                        {
                            // save byte array into a file and put to concurrent dictionary
                            await this.SaveToCache(logger, responseImageByteArray, options, httpContext);                            
                        }
                    }

                    // Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
            else
            {
                await _next(httpContext);
            }

            logger.LogDebug("ImageCachingMiddleware.Invoke - after _next() call.");
        }

        private async Task WriteImageFromCacheToResponse(ImageCachingOptions options, HttpContext httpContext, string filenamefromcache)
        {
            var fullfilename = Path.Combine(options.CacheDirectoryPath, filenamefromcache);
            if (File.Exists(fullfilename))
            {
                var image = File.ReadAllBytes(fullfilename);
                await httpContext.Response.Body.WriteAsync(image);
            }

            httpContext.Response.ContentType = "image/jpeg";
            var cd = new ContentDisposition { FileName = filenamefromcache, DispositionType = "attachment" };
            httpContext.Response.Headers[HeaderNames.ContentDisposition] = cd.ToString();
        }

        private static string GetFileNameFromHeader(HttpContext httpContext)
        {
            var header_contentDisposition = httpContext.Response.Headers[HeaderNames.ContentDisposition];
            if (string.IsNullOrEmpty(header_contentDisposition))
            {
                return string.Empty;
            }

            var filename = new ContentDisposition(header_contentDisposition).FileName;
            return filename;
        }

        private async Task SaveToCache(ILogger<ImageCachingMiddleware> logger, byte[] responseImageByteArray, ImageCachingOptions options, HttpContext httpContext)
        {
            string filename = GetFileNameFromHeader(httpContext);

            if (!string.IsNullOrEmpty(filename))
            {
                if (!Directory.Exists(options.CacheDirectoryPath))
                {
                    Directory.CreateDirectory(options.CacheDirectoryPath);
                }

                // check Max count of cached images
                var files = Directory.GetFiles(options.CacheDirectoryPath);
                if (files == null || files.Length < options.MaxCountImagesToCache)
                {
                    var filescount = files != null ? files.Length : 0;
                    logger.LogDebug($"ImageCachingMiddleware - [{filescount}] images in cache.");

                    var fullfilename = Path.Combine(options.CacheDirectoryPath, filename);

                    if (File.Exists(fullfilename))
                    {
                        File.Delete(fullfilename);
                    }

                    // Keep image on the disk (as file)                    
                    await File.WriteAllBytesAsync(fullfilename, responseImageByteArray);
                    CachedImagesDict.TryAdd(httpContext.Request.Path, filename);
                    logger.LogDebug($"ImageCachingMiddleware - cached image [{filename}].");
                }                
            }
        }

        private async Task<byte[]> ReadResponseBody(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(response.ContentLength)];

            //...Then we copy the entire response stream into the new buffer.
            await response.Body.ReadAsync(buffer, 0, buffer.Length);

            // We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            // return byte array
            return buffer;
        }
    }

    public static class ImageCachingMiddlewareExtensions
    {
        public static IApplicationBuilder UseImageCaching(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageCachingMiddleware>();
        }
    }
}
