
/*PROGRAM NAME: [ProductViewModel.js] 
REQUIREMENT: TechnicalTest
AUTHOR: Jonathan Martinez
DATE: 26-07-2016*/

App.Application.Product.ProductViewModel = {};
App.Application.Product.ProductViewModel._init_ = function () {

    var self = this;
    self.ProductID = ko.observable();
    self.Name = ko.observable();
    self.ProductNumber = ko.observable();
    self.Color = ko.observable();
    self.StandardCost = ko.observable();;
    self.ListPrice = ko.observable();
    self.Size = ko.observable();
    self.Weight = ko.observable();
    self.ProductCategorys = ko.observableArray([]);
    self.ProductCategoryID = ko.observable([]);
    self.ProductModels = ko.observableArray([]);
    self.ProductModelID = ko.observable([]);
    self.SellStartDate = ko.observable();
    self.SellEndDate = ko.observable();
    self.DiscontinuedDate = ko.observable();




    self.GetAllProducts = function () {
        $("#KoGridProducts").mask("loading..");
        App.Comun.AjaxRequest({
            "url": 'http://localhost:3566/api/product',
            "type": "GET"
        }, function (data) {
            //KOGRID
            self.Data_Products(data);
            self.Data_Products_Temp(data);
            self.gridOptionsProducts.pagingOptions.totalServerItems(self.Data_Products().length);
            self.gridOptionsProducts.pagingOptions.currentPage(1);
            //KOGRID
            $("#KoGridProducts").unmask("loading...");
        });
    };

    self.SearchProductById = function () {
        App.Comun.AjaxRequest({
            "url": 'http://localhost:3566/api/product/',
            "type": "GET",
            "parameters": { id: self.ProductNumber() }
        }, function (data) {
            self.ProductID(data.ProductID);
            self.Name(data.Name);
            self.Color(data.Color);
            self.StandardCost(data.StandardCost);
            self.ListPrice(data.ListPrice);
            self.Size(data.Size);
            self.Weight(data.Weight);
            self.ProductCategoryID(data.ProductCategoryID);
            self.ProductModelID(data.ProductModelID);
            $("#start_date").val(data.SellStartDate);
            $("#end_date").val(data.SellEndDate);
            $("#dis_date").val(data.DiscontinuedDate);
        });
    };

    self.SaveProduct = function () {
        if ($("#form_product").valid()) {
            $("#form_new_product").mask("loading..");
            var product = {
                "Name": self.Name(),
                "ProductNumber": self.ProductNumber(),
                "Color": self.Color(),
                "StandardCost": self.StandardCost(),
                "ListPrice": self.ListPrice(),
                "Size": self.Size(),
                "Weight": self.Weight(),
                "ProductCategoryID": self.ProductCategoryID(),
                "ProductModelID": self.ProductModelID(),
                "SellStartDate": self.SellStartDate(),
                "SellEndDate": self.SellEndDate(),
                "DiscontinuedDate": self.DiscontinuedDate()
            };

            App.Comun.AjaxRequest({
                "url": 'http://localhost:3566/api/product',
                "parameters": product
            }, function (data) {
                if (data == true) {
                    $("#form_new_product").unmask("loading..");
                    App.Comun.Alert('Product has been saved successfully', "success");
                    App.Comun.ClearForm('form_product');
                } else {
                    $("#form_new_product").unmask("loading..");
                    App.Comun.Alert('This product already exists', "danger");
                    App.Comun.Alert('You must review the product name and product number.');
                }
            });
        }
    };

    self.UpdateProduct = function () {
        $("#form_new_product").mask("loading..");
        var product = {
            "ProductID": self.ProductID(),
            "Name": self.Name(),
            "ProductNumber": self.ProductNumber(),
            "Color": self.Color(),
            "StandardCost": self.StandardCost(),
            "ListPrice": self.ListPrice(),
            "Size": self.Size(),
            "Weight": self.Weight(),
            "ProductCategoryID": self.ProductCategoryID(),
            "ProductModelID": self.ProductModelID(),
            "SellStartDate": $("#start_date").val(),
            "SellEndDate": $("#end_date").val(),
            "DiscontinuedDate": $("#dis_date").val()
        };

        App.Comun.AjaxRequest({
            "url": 'http://localhost:3566/api/product/update',
            "parameters": product
        }, function (data) {
            if (data == true) {
                $("#form_new_product").unmask("loading..");
                self.DisableFields();
                $('#search_parameter').attr('readonly', false);
                App.Comun.Alert('Product has been update successfully', "success");
            } else {
                $("#form_new_product").unmask("loading..");
                App.Comun.Alert('Product not  update', "danger");
            }
        });
    };

    self.CreateNewProduct = function () {
        $("#form_new_product").mask("loading..");
        $("#KoGridProducts").hide();
        App.Comun.ClearForm('form_product');
        $("#form_new_product").show();
        $("#message_search").hide();
        $("#message_search").hide();
        $("#btn_save").show();
        $("#btn_search").hide();
        $("#btn_edit").hide();
        $("#btn_update").hide();
        self.EnableFields();
        $("#form_new_product").unmask("loading..");

    };

    self.GetProductById = function () {
        $("#form_new_product").mask("loading..");
        $("#KoGridProducts").hide();
        $("#form_new_product").show();
        $("#btn_search").show();
        $("#btn_edit").show();
        $("#btn_update").show();
        $("#btn_save").hide();
        $("#message_search").show();
        self.DisableFields();
        $('#search_parameter').attr('readonly', false);
        $("#form_new_product").unmask("loading..");
    };

    self.EditProduct = function () {
        self.EnableFields();
    };

    $('#start_date').Zebra_DatePicker({
        show_icon: false,
        onSelect: function () {
            self.SellStartDate($('#start_date').val());
        }
    });

    $('#end_date').Zebra_DatePicker({
        show_icon: false,
        onSelect: function () {
            self.SellEndDate($('#end_date').val());
        }
    });

    $('#dis_date').Zebra_DatePicker({
        show_icon: false,
        onSelect: function () {
            self.DiscontinuedDate($('#dis_date').val());
        }
    });

    self.GetAllProductsCategory = function () {
        App.Comun.AjaxRequest({
            "url": '/Product/GetProductCategory',
            "type": "GET"
        }, function (data) {
            self.ProductCategorys(data.DataList);
        });
    };

    self.GetAllProductsModel = function () {
        App.Comun.AjaxRequest({
            "url": '/Product/GetProductsModel',
            "type": "GET"
        }, function (data) {
            self.ProductModels(data.DataList);
        });
    };

    self.EnableFields = function () {
        $('#form_product input').attr('readonly', false);
        $("#DropDownlist_Product_Category").prop('disabled', false);
        $("#DropDownlist_Product_Model").prop('disabled', false);
        $("#start_date").prop('disabled', false);
        $("#end_date").prop('disabled', false);
        $("#dis_date").prop('disabled', false);
    }

    self.DisableFields = function () {
        $('#form_product input').attr('readonly', true);
        $("#DropDownlist_Product_Category").prop('disabled', true);
        $("#DropDownlist_Product_Model").prop('disabled', true);
        $("#start_date").prop('disabled', true);
        $("#end_date").prop('disabled', true);
        $("#dis_date").prop('disabled', true);
    }

    App.Application.Product.ProductViewModel.GetView = function () {
        self.GetAllProducts();
        self.GetAllProductsCategory();
        self.GetAllProductsModel();
        ko.applyBindings(self, document.getElementById("scope_product"));
    };


    $("#form_product").validate({
        rules: {
            product_name: {
                required: true
            },
            search_parameter: {
                required: true
            },
            color: {
                required: true,
            },
            standard_cost: {
                required: true,
                number: true
            },
            list_price: {
                required: true,
                number: true
            },
            start_date: {
                required: true
            }
        }
    });

}();