using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalTest.DataLayer.Repositorio;
using TechnicalTest.CrossLayer;

namespace TechnicalTest.BusinessLayer
{
    public class ProductBL
    {

        ProductRepository rpProduct;
        ProductCategoryRepository rpProductCategory;
        ProductModelRepository rpProductModel;

        public ResultBase<Product> GetAllProducts()
        {
            try
            {
                rpProduct = new ProductRepository();
                ResultBase<Product> result = new ResultBase<Product>();
                result.DataList = rpProduct.GetAllProducts();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ResultBase<Product> GetProductById(string id)
        {
            try
            {
                rpProduct = new ProductRepository();
                ResultBase<Product> result = new ResultBase<Product>();
                result.DataSingle = rpProduct.GetProductById(id);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ResultBase<Product> SaveProduct(Product product)
        {
            try
            {
                rpProduct = new ProductRepository();
                ResultBase<Product> result = new ResultBase<Product>();
                if (product != null)
                {
                    product.rowguid = Guid.NewGuid();
                    product.ModifiedDate = DateTime.Now;
                    result.Success = rpProduct.SaveProduct(product);
                }

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ResultBase<Product> UpdateProduct(Product product)
        {
            try
            {
                rpProduct = new ProductRepository();
                ResultBase<Product> result = new ResultBase<Product>();
                if (product != null)
                {
                    product.ModifiedDate = DateTime.Now;
                    rpProduct.UpdateProduct(product);
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                }

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ResultBase<ProductCategory> GetProductsCategory()
        {
            try
            {
                rpProductCategory = new ProductCategoryRepository();
                ResultBase<ProductCategory> result = new ResultBase<ProductCategory>();
                result.DataList = rpProductCategory.GetProductsCategory();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ResultBase<ProductModel> GetProductsModel()
        {
            try
            {
                rpProductModel = new ProductModelRepository();
                ResultBase<ProductModel> result = new ResultBase<ProductModel>();
                result.DataList = rpProductModel.GetProductsModel();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
