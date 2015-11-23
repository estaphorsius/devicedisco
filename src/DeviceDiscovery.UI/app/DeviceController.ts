module App.Controllers {
    export class DeviceController {
        static inject: string[] = ["$http"];
        public devices: any;

        constructor(private $http: angular.IHttpService) {
            this.$http({
                method: "GET",
                url: "api/device/list"
            }).success((data: any) => {
                this.devices = data;
            });
        }
    }

    angular.module("App").controller("DeviceController", DeviceController);
}