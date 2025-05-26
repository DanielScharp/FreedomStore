using FreedomStore.Web.Models;
using FreedomStore.Web.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> IndexAsync()
        {
            var request = new ApiRequest
            {
                Route = "/Api/Products/list",
                Method = HttpMethod.Get,
            };

            var result = await _apiService.ExecuteRequestAsync(request);
            return View();
        }
    }
}
