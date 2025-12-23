using FreedomStore.Domain.Product;
using FreedomStore.Web.Models;
using FreedomStore.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FreedomStore.Web.Controllers
{
    public class ProductsController :Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApiService _apiService;

        public ProductsController(IHttpContextAccessor httpContextAccessor, IApiService apiService)
        {
            _httpContextAccessor = httpContextAccessor;
            _apiService = apiService;

        }

        public async Task<IActionResult> List()
        {
            var request = new ApiRequest
            {
                Route = "/Api/Products/list",
                Method = HttpMethod.Get,
            };

            var result = await _apiService.ExecuteRequestAsync(request);
            List<Product> listProducts = new List<Product>();
            if(result.Success)
            {
                listProducts = JsonConvert.DeserializeObject<List<Product>>(result.Data.ToString());
            }
            return View("_ProductsList", listProducts);
        }

        public async Task<IActionResult> CategoryList()
        {
            var request = new ApiRequest
            {
                Route = "/Api/Products/category-list",
                Method = HttpMethod.Get,
            };

            var result = await _apiService.ExecuteRequestAsync(request);

            List<ProductCategory> listCategorys = new List<ProductCategory>();

            if(result.Success)
            {
                listCategorys = JsonConvert.DeserializeObject<List<ProductCategory>>(result.Data.ToString());
            }

            return View("_CategoryList", listCategorys);
        }
    }
}
