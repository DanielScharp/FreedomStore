using FreedomStore.Domain.Login;
using FreedomStore.Web.Models;
using FreedomStore.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace FreedomStore.Web.Controllers
{
    public class LoginController :Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApiService _apiService;

        public LoginController(IHttpContextAccessor httpContextAccessor, IApiService apiService)
        {
            _httpContextAccessor = httpContextAccessor;
            _apiService = apiService;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Access(Login login)
        {

            try
            {

                //Atribue o IP da origem, Verificar se vem do Cliente 
                login.IpOrigem = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                var request = new ApiRequest
                {
                    Route = "/Api/Login/access",
                    Method = HttpMethod.Post,
                    Body = new Login
                    {
                        Nickname = login.Nickname,
                        Password = login.Password,
                        IpOrigem = login.IpOrigem,
                    }
                };

                var result = await _apiService.ExecuteRequestAsync(request);

                if(result.Success)
                {
                    var loginResult = JsonConvert.DeserializeObject<LoginResult>(result.Data.ToString());

                    if(loginResult.Id > 0)
                    {

                        var userClaims = new List<Claim>()
                        {
                            new Claim("Id", loginResult.Id.ToString()),
                            new Claim("Nickname", loginResult.Nickname),
                            new Claim("TokenApi", loginResult.Token)
                        };

                        var myIdentity = new ClaimsIdentity(userClaims, "Admin");
                        var userPrincipal = new ClaimsPrincipal(new[] { myIdentity });

                        //Cria o cookie
                        _ = _httpContextAccessor.HttpContext.SignInAsync(userPrincipal);

                        return Json(1);

                    } else
                    {
                        return Json("Cliente não localizado com os dados fornecidos!");
                    }
                }

                //Mostra a mensagem de Erro e o Status para mostrar na view
                var errorMessage = (result.Data == null ? result.Message : JsonConvert.DeserializeObject<Dictionary<string, string>>(result.Data.ToString())["error"]);
                return Json(errorMessage);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult Logoff()
        {
            try
            {
                if(_httpContextAccessor.HttpContext != null)
                    _httpContextAccessor.HttpContext.SignOutAsync();

                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
