'use strict';

define(['app'], function (app) {

    var injectParams = ['$location', '$routeParams', 'authService', '$rootScope', 'toastr', '$route'];

    var MainController = function ($location, $routeParams, authService, $rootScope, toastr, $route) {
        var vm = this,
            path = '/';

        $rootScope.pageName = 'Main';
        vm.controllerName = 'Главная';
    };

    MainController.$inject = injectParams;

    app.register.controller('MainController', MainController);

});