using Microsoft.Data.Extensions;
using TechnicalTest.CrossLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalTest.DataLayer.Repositorio
{
    public class ProductRepository : Repository<Product>
    {

        private ObjectContext objContext;

        public ProductRepository()
            : base(new TechnicalTestEntities())
        {
            objContext = (TechnicalTestEntities)this.UnitOfWork;
        }

        public List<Product> GetAllProducts()
        {
            return this.GetAll().ToList();
        }

        public Product GetProductById(string id)
        {
            return this.GetFilteredElements(x => x.ProductNumber.Equals(id)).SingleOrDefault();
        }

        public bool SaveProduct(Product product)
        {
            var productToSave = this.GetFilteredElements(x => x.ProductNumber.Equals(product.ProductNumber) || x.Name.Equals(product.Name)).SingleOrDefault();
            bool transaction = false;

            if (productToSave == null)
            {
                this.Add(product);
                this.UnitOfWork.Commit();
                transaction = true;
            }
            return transaction;
        }

        public void UpdateProduct(Product product)
        {
            Product prod = new Product();
            TechnicalTestEntities context = new TechnicalTestEntities();
            prod = context.Products.SingleOrDefault(x => x.ProductID == product.ProductID);
            prod.Name = product.Name;
            prod.Color = product.Color;
            prod.StandardCost = product.StandardCost;
            prod.ListPrice = product.ListPrice;
            prod.Size = product.Size;
            prod.Weight = product.Weight;
            prod.ProductCategoryID = product.ProductCategoryID;
            prod.ProductModelID = product.ProductModelID;
            prod.SellStartDate = product.SellStartDate;
            prod.SellEndDate = product.SellEndDate;
            prod.DiscontinuedDate = product.DiscontinuedDate;
            context.SaveChanges();
        }
    }
}
