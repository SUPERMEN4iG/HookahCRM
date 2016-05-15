'use strict';

define(['app'], function (app) {

	var injectParams = ['$location', '$routeParams', 'authService', 'usersService', '$rootScope', 'branchService'];

	var UsersController = function ($location, $routeParams, authService, usersService, $rootScope, branchService) {
        var vm = this;

        vm.users = [];
        vm.controllerName = "Сотрудники";
        $rootScope.pageName = 'Users';
        vm.currentBranch = $rootScope.currentBranch();

        function init() {
            getUsers();
        };

        function getUsers() {
            usersService.getUsers()
                .then(function (data) {
                    vm.users = data;
                });
        };

        if (vm.currentBranch !== undefined)
        	init();

        $rootScope.$on('branch:updated', function (event, data) {
        	vm.currentBranch = data[0];
        	init();
        });

        //$rootScope.$on('onSelectBranch', function (event, data) {
        //    init();
		//});
    };

    UsersController.$inject = injectParams;

    app.register.controller('UsersController', UsersController);

});