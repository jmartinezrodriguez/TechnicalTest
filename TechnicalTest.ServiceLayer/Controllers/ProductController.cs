using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using TechnicalTest.BusinessLayer;
using TechnicalTest.CrossLayer;


namespace TechnicalTest.ServiceLayer.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProductController : ApiController
    {
        ProductBL blProduct;

        // GET: api/Product
        public IEnumerable<Product> Get()
        {
            blProduct = new ProductBL();
            return blProduct.GetAllProducts().DataList;
        }

        // GET: api/Product/5
        public Product Get(string id)
        {
            blProduct = new ProductBL();
            return blProduct.GetProductById(id).DataSingle;
        }

        // POST: api/Product
        [HttpPost]
        public HttpResponseMessage Post([FromBody] Product product)
        {
            blProduct = new ProductBL();
            return Request.CreateResponse(HttpStatusCode.OK, blProduct.SaveProduct(product).Success);
        }


        // POST: api/Product
        [HttpPost]
        [Route("api/Product/Update")]
        public HttpResponseMessage Update([FromBody] Product product)
        {
            blProduct = new ProductBL();
            return Request.CreateResponse(HttpStatusCode.OK, blProduct.UpdateProduct(product).Success);
        }

    }
}
