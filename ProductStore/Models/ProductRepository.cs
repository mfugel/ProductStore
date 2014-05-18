using System;
using System.Collections.Generic;

namespace ProductStore.Models
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> products = new List<Product>();
        private int _nextId = 1;

        public ProductRepository()
        {
            Add(new Product {Name = "Tomato soup", Category = "Groceries", Price = 1.39M});
            Add(new Product {Name = "Yo-yo", Category = "Toys", Price = 3.75M});
            Add(new Product {Name = "Hammer", Category = "Hardware", Price = 16.99M});
        }


        // ===== Interface methods ================
        public IEnumerable<Product> GetAll()
        {
            return products;
        }

        public Product Get(int id)
        {
            return products.Find(product => product.Id == id);
        }

        public Product Add(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            item.Id = _nextId;
            products.Add(item);
            return item;
        }

        public void Remove(int id)
        {
            products.RemoveAll(product => product.Id == id);
        }

        public bool Update(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            int index = products.FindIndex(product => product.Id == item.Id);
            if (index == -1)
            {
                return false;
            }
            products.RemoveAt(index);
            products.Add(item);
            return (true);
        }
    }
}