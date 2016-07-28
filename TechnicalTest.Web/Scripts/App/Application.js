
var App = {};
App.Application = {};
App.Application.Product = {};

App._ProductViewModel_init = function () {
    if (document.getElementById("scope_product")) {
        App.Application.Product.ProductViewMode.GetView();
    }
};
