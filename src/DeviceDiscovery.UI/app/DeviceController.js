var App;
(function (App) {
    var Controllers;
    (function (Controllers) {
        var DeviceController = (function () {
            function DeviceController($http) {
                var _this = this;
                this.$http = $http;
                this.$http({
                    method: "GET",
                    url: "api/device/list"
                }).success(function (data) {
                    _this.devices = data;
                });
            }
            DeviceController.inject = ["$http"];
            return DeviceController;
        })();
        Controllers.DeviceController = DeviceController;
        angular.module("App").controller("DeviceController", DeviceController);
    })(Controllers = App.Controllers || (App.Controllers = {}));
})(App || (App = {}));
