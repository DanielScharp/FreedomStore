using FreedomStore.Domain.Login;
using FreedomStore.Domain.User;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomStore.Database.Repository
{
    public class UsersRepository
    {
        private readonly string _connMySql;

        public UsersRepository(string connMySql)
        {
            _connMySql = connMySql;
        }

        //Retornar usuário por apelido e password || READ_WRITE
        public async Task<User> GetUserAsync(Login login)
        {

            using var connection = new MySqlConnection(_connMySql);

            try
            {
                await connection.OpenAsync();

                var query = new StringBuilder();
                query.Append(" SELECT * ");
                query.Append(" FROM freedom_store.users ");
                query.AppendFormat(" WHERE Nickname = '{0}' AND Password = MD5('{1}'); ", login.Nickname, login.Password);

                using MySqlCommand command = new(query.ToString(), connection);

                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                if(reader.Read())
                {
                    var cliente = new User();

                    cliente.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    cliente.Name = reader.GetString(reader.GetOrdinal("Name"));
                    cliente.Nickname = reader.GetString(reader.GetOrdinal("Nickname"));

                    return cliente;
                }

                return new User();
            }
            catch
            {
                throw;
            }
            finally
            {
                if(connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
        //Retornar usuário por apelido e password || READ_WRITE
        public async Task<User> GetUserForEmailAsync(string apelido, string email)
        {

            using var connection = new MySqlConnection(_connMySql);

            try
            {
                await connection.OpenAsync();

                var query = new StringBuilder();
                query.Append(" SELECT * ");
                query.Append(" FROM freedom_store.users ");
                query.AppendFormat(" WHERE Nickname = '{0}' AND Email = '{1}'; ", apelido, email);

                using MySqlCommand command = new(query.ToString(), connection);

                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                if(reader.Read())
                {
                    var cliente = new User();

                    cliente.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    cliente.Name = reader.GetString(reader.GetOrdinal("Name"));
                    cliente.Email = reader.GetString(reader.GetOrdinal("Email"));

                    return cliente;
                }

                return new User();
            }
            catch
            {
                throw;
            }
            finally
            {
                if(connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        public async Task<bool> AlterPasswordAsync(User user)
        {

            using var connection = new MySqlConnection(_connMySql);

            try
            {
                await connection.OpenAsync();

                var query = new StringBuilder();
                query.Append(" UPDATE freedom_store.users set ");
                query.AppendFormat(" Password = md5('{0}') ", user.Password);
                query.AppendFormat(" where ( Id = '{0}' ) ", user.Id);

                using MySqlCommand command = new(query.ToString(), connection);

                var result = await command.ExecuteNonQueryAsync();

                return result > 0;
            }
            catch
            {
                throw;
            }
            finally
            {
                if(connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
    }
}
