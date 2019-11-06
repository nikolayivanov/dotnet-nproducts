# IO.Swagger.Api.ProductsApi

All URIs are relative to *https://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**Delete**](ProductsApi.md#delete) | **DELETE** /api/Products/{id} | 
[**Get**](ProductsApi.md#get) | **GET** /api/Products | 
[**Get_0**](ProductsApi.md#get_0) | **GET** /api/Products/{id} | 
[**Post**](ProductsApi.md#post) | **POST** /api/Products | 
[**Put**](ProductsApi.md#put) | **PUT** /api/Products/{id} | 


<a name="delete"></a>
# **Delete**
> void Delete (int? id)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class DeleteExample
    {
        public void main()
        {
            var apiInstance = new ProductsApi();
            var id = 56;  // int? | 

            try
            {
                apiInstance.Delete(id);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ProductsApi.Delete: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **int?**|  | 

### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="get"></a>
# **Get**
> PagedCollectionResponseProductsDTO Get (string productName = null, string supplierName = null, int? categoryId = null, int? page = null, int? pageSize = null, string orderByFieldName = null, string orderByDirection = null)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetExample
    {
        public void main()
        {
            var apiInstance = new ProductsApi();
            var productName = productName_example;  // string |  (optional) 
            var supplierName = supplierName_example;  // string |  (optional) 
            var categoryId = 56;  // int? |  (optional) 
            var page = 56;  // int? |  (optional) 
            var pageSize = 56;  // int? |  (optional) 
            var orderByFieldName = orderByFieldName_example;  // string |  (optional) 
            var orderByDirection = orderByDirection_example;  // string |  (optional) 

            try
            {
                PagedCollectionResponseProductsDTO result = apiInstance.Get(productName, supplierName, categoryId, page, pageSize, orderByFieldName, orderByDirection);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ProductsApi.Get: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **productName** | **string**|  | [optional] 
 **supplierName** | **string**|  | [optional] 
 **categoryId** | **int?**|  | [optional] 
 **page** | **int?**|  | [optional] 
 **pageSize** | **int?**|  | [optional] 
 **orderByFieldName** | **string**|  | [optional] 
 **orderByDirection** | **string**|  | [optional] 

### Return type

[**PagedCollectionResponseProductsDTO**](PagedCollectionResponseProductsDTO.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="get_0"></a>
# **Get_0**
> ProductsDTO Get_0 (int? id)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class Get_0Example
    {
        public void main()
        {
            var apiInstance = new ProductsApi();
            var id = 56;  // int? | 

            try
            {
                ProductsDTO result = apiInstance.Get_0(id);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ProductsApi.Get_0: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **int?**|  | 

### Return type

[**ProductsDTO**](ProductsDTO.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="post"></a>
# **Post**
> int? Post (ProductsDTO product = null)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PostExample
    {
        public void main()
        {
            var apiInstance = new ProductsApi();
            var product = new ProductsDTO(); // ProductsDTO |  (optional) 

            try
            {
                int? result = apiInstance.Post(product);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ProductsApi.Post: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **product** | [**ProductsDTO**](ProductsDTO.md)|  | [optional] 

### Return type

**int?**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="put"></a>
# **Put**
> void Put (int? id, ProductsDTO product = null)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class PutExample
    {
        public void main()
        {
            var apiInstance = new ProductsApi();
            var id = 56;  // int? | 
            var product = new ProductsDTO(); // ProductsDTO |  (optional) 

            try
            {
                apiInstance.Put(id, product);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ProductsApi.Put: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **id** | **int?**|  | 
 **product** | [**ProductsDTO**](ProductsDTO.md)|  | [optional] 

### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/_*+json
 - **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

