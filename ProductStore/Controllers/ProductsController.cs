using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ProductStore.Models;

namespace ProductStore.Controllers
{
    public class ProductsController : ApiController
    {
        //Add a field that holds an IProductRepository instance.
        //** NOTE: This line of code is a problem...need to replace it with dependency injection ** !!!!!
        /*
         * Notice that the controller class depends on ProductRepository, and we are letting the controller 
         * create the ProductRepository instance. However, it's a bad idea to hard code the dependency in this way, 
         * for several reasons:
         * 
         * -- If you want to replace ProductRepository with a different implementation, you also need to modify 
         * the controller class.
         * -- If the ProductRepository has dependencies, you must configure these inside the controller. For a large project with multiple controllers, your configuration code becomes scattered across your project.
         * It is hard to unit test, because the controller is hard-coded to query the database. 
         * For a unit test, you should use a mock or stub repository, which is not possible with the currect design.
         * 
         * 
         */
        static readonly IProductRepository repository = new ProductRepository();

        //Get the list of all products
        public IEnumerable<Product> GetAllProducts()
        {
            return repository.GetAll();
        }

        // Get a product by Id
        public Product GetProduct(int id)
        {
            Product item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }

        // Get the list of products for a category
        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return repository.GetAll().Where(
                product => string.Equals(product.Category, category, StringComparison.OrdinalIgnoreCase));
        }

        // Create a new product
        public HttpResponseMessage PostProduct(Product item)
        {
            item = repository.Add(item);
            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);

            string uri = Url.Link("DefaultApi", new {id = item.Id});
            response.Headers.Location = new Uri(uri);
            return response;
        }

        // Update a product
        public void PutProduct(int id, Product product)
        {
            product.Id = id;
            if (!repository.Update(product))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        // Delete a product
        public void DeleteProduct(int id)
        {
            Product item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            repository.Remove(id);
        }
    }
}
