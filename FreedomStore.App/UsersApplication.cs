using FreedomStore.Database.Repository;
using FreedomStore.Domain.Login;
using FreedomStore.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomStore.App
{
    public class UsersApplication
    {
        private readonly UsersRepository _usersRepository;
        public UsersApplication(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        //Retorna o usuário por login e senha
        public async Task<User> GetUserAsync(Login login)
        {
            try
            {
                return await _usersRepository.GetUserAsync(login);
            }
            catch
            {
                throw;
            }
        }

        public async Task<User> GetUserForEmailAsync(string apelido, string email)
        {
            try
            {
                return await _usersRepository.GetUserForEmailAsync(apelido, email);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> AlterPasswordAsync(User user)
        {
            try
            {
                return await _usersRepository.AlterPasswordAsync(user);
            }
            catch
            {
                throw;
            }
        }
    }
}
