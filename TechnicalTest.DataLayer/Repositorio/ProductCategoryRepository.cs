using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Extensions;
using TechnicalTest.CrossLayer;

namespace TechnicalTest.DataLayer.Repositorio
{
    public class ProductCategoryRepository : Repository<ProductCategory>
    {
        private ObjectContext objContext;

        public ProductCategoryRepository()
            : base(new TechnicalTestEntities())
        {
            objContext = new TechnicalTestEntities();
        }

        public List<ProductCategory> GetProductsCategory()
        {
            return this.GetAll().ToList();
        }

    }
}
