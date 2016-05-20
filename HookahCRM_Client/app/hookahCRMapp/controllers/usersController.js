'use strict';

define(['app'], function (app) {

	var injectParams = ['$location', '$routeParams', 'authService', 'usersService', '$rootScope', 'branchService', 'toastr', 'rolesService'];

	var UsersController = function ($location, $routeParams, authService, usersService, $rootScope, branchService, toastr, rolesService) {
        var vm = this;

        vm.users = [];
        vm.controllerName = "Сотрудники";
        $rootScope.pageName = 'Users';
        vm.currentBranch = {};

        vm.showUserCreateModal = false;
        vm.currentEditUser;
        vm.activeBranches = [];
        vm.activeRoles = [];

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
            getActiveRoles();
        };

        function getUsers() {
            usersService.getUsers()
                .then(function (data) {
                    vm.users = data;
                },
                function (data) {
                    toastr.error(data.data.ExceptionMessage, 'Ошибка');
                });
        };

        function getActiveBranches() {
            branchService.getActiveBranchList()
                .then(function (data) {
                    vm.activeBranches = data;
                });
        };

        function getActiveRoles() {
            rolesService.getActiveRoles()
                .then(function (data) {
                    vm.activeRoles = data;
                });
        };

        if (vm.currentBranch !== undefined)
        	init();

        $rootScope.$on('branch:updated', function (event, data) {
        	vm.currentBranch = data;
        	init();
        });

        //$rootScope.$on('onSelectBranch', function (event, data) {
        //    init();
		//});
    };

    UsersController.$inject = injectParams;

    app.register.controller('UsersController', UsersController);

});