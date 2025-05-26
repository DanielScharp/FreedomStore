using ChamadaApi.Api.Services;
using ChamadaApi.Api;
using FreedomStore.Api.Email;
using FreedomStore.Api.Services;
using FreedomStore.App;
using FreedomStore.Domain.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using FreedomStore.Domain.Product;

namespace FreedomStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController :ControllerBase
    {
        private readonly ProductsApplication _productsApplication;

        public ProductsController(ProductsApplication productsApplication)
        {
            _productsApplication = productsApplication;
        }


        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> AuthenticateAsync()
        {
            try
            {
                List<Product> listProducts = await _productsApplication.ListProductsAsync();
                return Ok(ResultMessage.Sucesso(1, listProducts));
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

    }
}
