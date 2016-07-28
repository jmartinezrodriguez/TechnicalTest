
var currentPage = ko.observable(1);
self.Data_Products = ko.observableArray([]);
self.Data_Products_Temp = ko.observableArray([]);


var columnas = [{ field: 'Name', displayName: 'Name', width: '20%' },
                { field: 'ProductNumber', displayName: 'ProductNumber', width: '15%' },
                { field: 'Color', displayName: 'Color', width: '14%' },
                { field: "StandardCost", displayName: 'StandardCost', width: '14%' },
                { field: 'Size', displayName: 'Size', width: '10%' },
                { field: 'Weight', displayName: 'Weight', width: '10%' },
                { field: 'SellStartDate', displayName: 'SellStartDate', width: '15%' }];

self.gridOptionsProducts = {
    data: self.Data_Products,
    columnDefs: columnas,
    canSelectRows: false,
    enablePaging: true, // enables the paging feature
    pagingOptions: {
        pageSizes: ko.observableArray([14, 30, 40]), // page Sizes
        pageSize: ko.observable(14), // Size of Paging data
        totalServerItems: ko.observable(0), // how many items are on the server (for paging)
        currentPage: currentPage, // what page they are currently on
    }, filterOptions: {
        filterText: ko.observable(""),
        useExternalFilter: false
    },
    enableRowReordering: true,
};

self.setPagingData = function (data, page, pageSize) {
    var pagedData = data.slice((page - 1) * pageSize, page * pageSize);
    self.Data_Products(pagedData);
    self.gridOptionsProducts.pagingOptions.totalServerItems(data.length);
};

self.getPagedDataAsync = function (pageSize, page, searchText) {
    var data = self.Data_Products_Temp();
    if (searchText) {
        var ft = searchText.toLowerCase();
        self.setPagingData(data, page, pageSize);

    } else {
        self.setPagingData(data, page, pageSize);
    }
};

self.gridOptionsProducts.filterOptions.filterText.subscribe(function (data) {
    self.getPagedDataAsync(self.gridOptionsProducts.pagingOptions.pageSize(), self.gridOptionsProducts.pagingOptions.currentPage(), self.gridOptionsProducts.filterOptions.filterText());
});

self.gridOptionsProducts.pagingOptions.pageSizes.subscribe(function (data) {
    self.getPagedDataAsync(self.gridOptionsProducts.pagingOptions.pageSize(), self.gridOptionsProducts.pagingOptions.currentPage(), self.gridOptionsProducts.filterOptions.filterText());
});
self.gridOptionsProducts.pagingOptions.pageSize.subscribe(function (data) {
    self.getPagedDataAsync(self.gridOptionsProducts.pagingOptions.pageSize(), self.gridOptionsProducts.pagingOptions.currentPage(), self.gridOptionsProducts.filterOptions.filterText());
});
self.gridOptionsProducts.pagingOptions.totalServerItems.subscribe(function (data) {
    self.getPagedDataAsync(self.gridOptionsProducts.pagingOptions.pageSize(), self.gridOptionsProducts.pagingOptions.currentPage(), self.gridOptionsProducts.filterOptions.filterText());
});
self.gridOptionsProducts.pagingOptions.currentPage.subscribe(function (data) {
    self.getPagedDataAsync(self.gridOptionsProducts.pagingOptions.pageSize(), self.gridOptionsProducts.pagingOptions.currentPage(), self.gridOptionsProducts.filterOptions.filterText());
});

self.koGridClearData = function () {
    self.Data_Products([]);
    self.Data_Products_Temp([]);
    self.gridOptionsProducts.pagingOptions.totalServerItems(0);
};

