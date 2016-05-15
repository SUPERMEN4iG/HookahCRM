'use strict';

define(['app'], function (app) {

    var injectParams = ['$location', '$routeParams', 'authService', 'usersService', '$rootScope'];

    var UsersController = function ($location, $routeParams, authService, usersService, $rootScope) {
        var vm = this;

        vm.users = [];
        vm.controllerName = "Сотрудники";
        $rootScope.pageName = 'Users';

        function init() {
            getUsers();
        };

        function getUsers() {
            usersService.getUsers()
                .then(function (data) {
                    vm.users = data;
                });
        };

        $rootScope.$on('onSelectBranch', function (event, data) {
            init();
        });
    };

    UsersController.$inject = injectParams;

    app.register.controller('UsersController', UsersController);

});