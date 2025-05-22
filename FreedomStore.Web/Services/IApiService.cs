using FreedomStore.Web.Models;

namespace FreedomStore.Web.Services
{
    public interface IApiService
    {
        Task<ApiResponse> ExecuteRequestAsync(ApiRequest request);
    }
}
