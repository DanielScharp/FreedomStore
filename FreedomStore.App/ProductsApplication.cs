using FreedomStore.Database.Repository;
using FreedomStore.Domain.Login;
using FreedomStore.Domain.Product;
using FreedomStore.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomStore.App
{
    public class ProductsApplication
    {
        private readonly ProductsRepository _productsRepository;
        public ProductsApplication(ProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        //Retorna o usuário por login e senha
        public async Task<List<Product>> ListProductsAsync()
        {
            try
            {
                return await _productsRepository.ListProductsAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
