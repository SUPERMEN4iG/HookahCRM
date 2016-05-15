'use strict';

define(['app'], function (app) {

    var injectParams = ['$location', '$routeParams', 'authService', '$rootScope', 'toastr', '$route'];

    var LoginController = function ($location, $routeParams, authService, $rootScope, toastr, $route) {
        var vm = this,
            path = '/';

        $rootScope.pageName = 'Login';

        console.log($route);

        vm.username = null;
        vm.password = null;
        vm.message = {
            status: "error",
            code: ""
        };
        //authService.ClearCredentials();

        vm.login = function () {
            console.log("LOGIN");
            authService.Login(vm.username, vm.password, function (response) {
                //$routeParams.redirect will have the route
                //they were trying to go to initially
                if (response.success) {
                    authService.SetCredentials(vm.username, vm.password);
                    vm.message.status = "success";
                    vm.message.code = "Redirecting...";
                    toastr.success('Выход выполнен успешно!');
                } else {
                    vm.message.status = "error";
                    vm.message.code = response.message;
                    return;
                }
                console.log($routeParams);
                if (response.success && $routeParams && $routeParams.redirect) {
                    path = path + $routeParams.redirect;
                }

                $location.path(path);
            });
        };
    };

    LoginController.$inject = injectParams;

    app.register.controller('LoginController', LoginController);

});