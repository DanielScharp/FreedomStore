using FreedomStore.Domain.Login;

namespace FreedomStore.Web.Models
{
    public class ClientSession
    {
        public static LoginResult Logado
        {
            get
            {
                return GetCookie();
            }
        }

        private static LoginResult GetCookie()
        {
            var clientLogin = new LoginResult();

            try
            {
                if(HttpContextAccessorHelper.Current != null && HttpContextAccessorHelper.Current.User.Identity.IsAuthenticated)
                {
                    clientLogin.Id = Convert.ToInt32(HttpContextAccessorHelper.Current.User.FindFirst("Id").Value);
                    clientLogin.Nickname = HttpContextAccessorHelper.Current.User.FindFirst("Nickname").Value;
                    clientLogin.Token = HttpContextAccessorHelper.Current.User.FindFirst("TokenApi").Value;

                }
            }
            catch
            {
                //Não faz nada, só vai retornar um objeto vazio
            }

            return clientLogin;

        }
    }
}
