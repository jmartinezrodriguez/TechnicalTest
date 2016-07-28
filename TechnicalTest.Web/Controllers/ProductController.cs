using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechnicalTest.BusinessLayer;
using TechnicalTest.CrossLayer;


namespace TechnicalTest.Web.Controllers
{
    public class ProductController : Controller
    {

        ProductBL blProduct;


        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetProductCategory()
        {
            blProduct = new ProductBL();
            return Json(blProduct.GetProductsCategory(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetProductsModel()
        {
            blProduct = new ProductBL();
            return Json(blProduct.GetProductsModel(), JsonRequestBehavior.AllowGet);
        }
    }
}