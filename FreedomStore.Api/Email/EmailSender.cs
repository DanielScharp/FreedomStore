using FreedomStore.Api.Services;
using FreedomStore.Domain.User;

namespace FreedomStore.Api.Email
{
    public class EmailSender
    {
        private readonly EmailService _emailservice;

        public EmailSender(EmailService emailservice)
        {
            _emailservice = emailservice;
        }
        //Envia email com dados de acesso solicitados pelo usuário
        public async Task EnviarEmailRecuperaSenha(User user)
        {

            try
            {

                //Monta o email
                var strMsg = "<font face=tahoma, arial size=2>";
                strMsg += " Prezado " + user.Name + ", <br/><br/>";
                strMsg += " Este e-mail tem como finalidade a notificação de sua nova senha de acesso ao site. <br/><br/>";
                strMsg += " Apelido/Login: " + user.Nickname + "<br/>";
                strMsg += " Senha: " + user.Password + "<br/><br/>";
                strMsg += " <BR/><BR/><BR/>Atenciosamente, <br/> Freedom Store <BR/>";

                strMsg += " <font color=red>Não é preciso responder esta mensagem, pois trata-se de um email automático de nosso sistema.</font>";
                strMsg += " </font>";

                //Enviar a senha para o e-mail do cliente
                var emailRequest = new EmailRequest
                {
                    MailTo = user.Email,
                    Subject = "Novos dados de acesso",
                    Body = strMsg
                };


                //Envia o Email
                await _emailservice.EnviarEmail(emailRequest);
            }
            catch
            {
                throw;
            }

        }
    }
}
