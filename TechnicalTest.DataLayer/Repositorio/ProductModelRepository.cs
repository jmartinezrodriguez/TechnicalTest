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
    public class ProductModelRepository : Repository<ProductModel>
    {
        private ObjectContext objContext;

        public ProductModelRepository()
            : base(new TechnicalTestEntities())
        {
            objContext = new TechnicalTestEntities();
        }

        public List<ProductModel> GetProductsModel()
        {
            return this.GetAll().ToList();
        }
    }
}
