using FreedomStore.Domain.Login;
using FreedomStore.Domain.Product;
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
    public class ProductsRepository
    {
        private readonly string _connMySql;

        public ProductsRepository(string connMySql)
        {
            _connMySql = connMySql;
        }

        //Retornar usuário por apelido e password || READ_WRITE
        public async Task<List<Product>> ListProductsAsync()
        {

            using var connection = new MySqlConnection(_connMySql);

            try
            {
                await connection.OpenAsync();

                var query = new StringBuilder();
                query.Append(" SELECT t1.*, t2.Name as 'CategoryName' ");
                query.Append(" FROM products t1 ");
                query.Append(" LEFT JOIN products_category t2 ON t1.CategoryId = t2.Id ");
                query.Append(" WHERE t1.IsActive = 1 ");

                using MySqlCommand command = new(query.ToString(), connection);

                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                List<Product> products = new List<Product>();

                while(reader.Read())
                {
                    var product = new Product();

                    product.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                    product.Name = reader[reader.GetOrdinal("Name")].ToString();
                    product.Description = reader[reader.GetOrdinal("Description")].ToString();
                    product.ImageUrl = reader[reader.GetOrdinal("ImageUrl")].ToString();
                    product.Price = reader[reader.GetOrdinal("Price")] != DBNull.Value ? reader.GetDecimal("Price") : 0;
                    product.StockQuantity = reader[reader.GetOrdinal("StockQuantity")] != DBNull.Value ? reader.GetInt32("StockQuantity") : 0;

                    product.Category.Id = reader[reader.GetOrdinal("CategoryId")] != DBNull.Value ? reader.GetInt32("CategoryId") : 0;
                    product.Category.Name = reader[reader.GetOrdinal("CategoryName")].ToString();

                    products.Add(product);
                }

                return products;
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
