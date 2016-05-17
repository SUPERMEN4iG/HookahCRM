'use strict';

define(['app'], function (app) {

	var injectParams = ['$location', '$routeParams', 'authService', 'usersService', '$rootScope', 'branchService', 'toastr'];

	var UsersController = function ($location, $routeParams, authService, usersService, $rootScope, branchService, toastr) {
        var vm = this;

        vm.users = [];
        vm.controllerName = "Сотрудники";
        $rootScope.pageName = 'Users';
        vm.currentBranch = $rootScope.currentBranch();

        vm.showUserCreateModal = false;
        vm.currentEditUser;
        vm.activeBranches = [];

        vm.openUserCreateModal = function (obj, isEdit) {
            vm.showUserCreateModal = true;

            if (isEdit) {
                vm.currentEditUser = obj;
            }
            else {
                vm.currentEditUser = {};
            }
        };

        vm.putUser = function () {
            console.log(vm.currentEditUser);
            usersService.putUser(vm.currentEditUser)
                .then(function (data) {
                    toastr.success('Пользователь изменён!', 'Готово');
                    init();
                });
        };

        function init() {
            getUsers();
            getActiveBranches();
        };

        function getUsers() {
            usersService.getUsers()
                .then(function (data) {
                    vm.users = data;
                });
        };

        function getActiveBranches() {
            branchService.getActiveBranchList()
                .then(function (data) {
                    vm.activeBranches = data;
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