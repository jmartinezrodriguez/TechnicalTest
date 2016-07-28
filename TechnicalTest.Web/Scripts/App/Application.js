
var App = {};
App.Application = {};
App.Application.Product = {};

App._ProductViewModel_init = function () {
    if (document.getElementById("Div_Product_View")) {
        App.Application.Product.ProductViewMode.GetView();
    }
};
