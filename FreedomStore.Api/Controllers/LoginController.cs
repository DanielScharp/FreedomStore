using ChamadaApi.Api;
using ChamadaApi.Api.Services;
using ChurchStore.Database;
using FreedomStore.Api.Email;
using FreedomStore.Api.Services;
using FreedomStore.App;
using FreedomStore.Domain.Login;
using FreedomStore.Domain.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FreedomStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController :ControllerBase
    {
        private readonly UsersApplication _usersApplication;
        private readonly EmailSender _emailSender;

        public LoginController(UsersApplication usersApplication, EmailSender emailsender)
        {
            _usersApplication = usersApplication;
            _emailSender = emailsender;
        }

        [HttpPost]
        [Route("in")]
        public async Task<IActionResult> AuthenticateAsync(Login login)
        {
            try
            {
                //Desencripta a senha
                string keyStoreDecoded = Encoding.GetEncoding("iso-8859-1").GetString(Convert.FromBase64String(login.Password));
                var password = AESEncrytDecry.DecryptStringAES(keyStoreDecoded);

                //Validações
                var validNickname = login.Nickname?.SafeSql().Replace("\"", "");
                var pwdValidado = password.SafeSql().Replace("\"", "");

                //Verifica Apelido
                if(string.IsNullOrEmpty(validNickname) || validNickname.Length < 5)
                {
                    return BadRequest(ResultMessage.Erro("O parâmetro [Apelido] é obrigatório, e deve conter pelo menos 5 caracteres."));
                }

                //Verifica Senha
                if(string.IsNullOrEmpty(pwdValidado) || pwdValidado.Length < 4)
                {
                    return BadRequest(ResultMessage.Erro("O parâmetro [Password] é obrigatório, e deve conter pelo menos 5 caracteres."));
                }


                //Atribui a senha descriptada ao modelo
                login.Password = password;

                var user = await _usersApplication.GetAsync(login);

                if(user.Id == 0)
                {
                    return NotFound(ResultMessage.Erro("Usuário não localizado com os dados informados!"));
                }

                var token = TokenService.GenerateToken(user);

                var loginResult = new LoginResult
                {
                    Id = user.Id,
                    Nickname = user.Nickname,
                    IpOrigem = login.IpOrigem,
                    UserType = user.UserType,
                    Token = token

                };

                return Ok(ResultMessage.Sucesso(0, loginResult));

            }
            catch(Exception ex)
            {
                // _logger.LogError(ex, "Erro ao tentar processar os dados!"); -----------------------------------------------------------Registar Log

                return new StatusCodeResult(500);

            }
        }


        /// <summary>
        /// [Aberta] Verifica a existência do cliente e envia um e-mail para recuperação de acesso. Recebe um Schema LoginResetPwd
        /// </summary>
        [Route("send-password")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(Login login)
        {
            try
            {

                //Verifica E-mail
                if(string.IsNullOrEmpty(login.Email) || login.Email.Length < 10 || login.Email.IndexOf("@") < 0)
                {
                    return BadRequest(ResultMessage.Erro("O parâmetro [E-mail] é obrigatório, e deve conter pelo menos 10 caracteres."));
                }


                //Retorna o cliente pelo CPF e Email
                var usuario = await _usersApplication.ReturnUserForEmail(login.Nickname, login.Email);

                if(usuario.Id == 0)
                    return NotFound(ResultMessage.Erro("Cliente não localizado com os dados informados!"));


                //Gera uma senha aleatória para o cliente
                usuario.Password = DBValidate.GetRandomPassword(8);

                //Altera a senha
                bool senhaAlterada = await _usersApplication.AlterPasswordAsync(usuario);

                if(!senhaAlterada)
                {
                    return BadRequest(ResultMessage.Erro("Não foi possível realizar a alteração desta senha."));
                }

                //Envia o e-mail se alterar a senha
                if(!string.IsNullOrEmpty(usuario.Password))
                {
                    try
                    {
                        await _emailSender.EnviarEmailRecuperaSenha(usuario);

                        return Ok(ResultMessage.Sucesso(0, "Uma nova senha foi enviada para o e-mail informado!"));
                    }
                    catch(Exception ex)
                    {
                        //_logger.LogError(ex, "Erro ao tentar enviar o e-mail!"); -----------------------------------------------------------Registar Log
                        return BadRequest(ResultMessage.Erro("Ocorreu um erro ao tentar enviar o e-mail! " + ex.InnerException));
                    }
                }

                return BadRequest(ResultMessage.Erro("Erro desconhecido!"));

            }
            catch(Exception ex)
            {
                //_logger.LogError(ex, "Erro ao tentar processar os dados!"); ----------------------------------------------------------- Registar Log
                return new StatusCodeResult(500);
            }
        }
    }
}
