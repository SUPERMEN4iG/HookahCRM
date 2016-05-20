'use strict';

define(['app'], function (app) {

    var injectParams = ['$location', '$routeParams', 'authService', '$rootScope', 'toastr', '$route', 'storageService', 'tobaccoService', 'branchService'];

    var StorageController = function ($location, $routeParams, authService, $rootScope, toastr, $route, storageService, tobaccoService, branchService) {
        var vm = this,
            path = '/storage';

        vm.tobaccoList = [];

        $rootScope.pageName = 'Storage';
        vm.controllerName = 'Склад';
        vm.currentStorage;
        vm.currentBranch = {};

        vm.currentStorageGetStateString = function () {
        	var str;

        	if (vm.currentStorage != null) {
        		console.log(vm.currentStorage.StorageHookah);
        		angular.forEach(vm.currentStorage.StorageHookah, function (val) {
        			if (val.IsClosed)
        				str = "Закрыт";
        			else
        				str = "Открыт";
        		});
        	} else {
        		str = "Не известно";
        	}

        	return str;
        };

        vm.closeStorage = function () {
            $rootScope.$broadcast('onSelectBranch', {
                showSelectStorageModal: true,
                selectedBranch: vm.currentBranch,
                isCloseRepot: true
            });
        };

        function init() {
        	vm.currentBranch = branchService.getCurrentBranch();
        	vm.tobaccoList = tobaccoService.getTobaccoList();
        	storageService.getCurrentStorage(vm.currentBranch.Id).then(function (data) {
        		vm.currentStorage = data;
        	});
        };

        function preInit() {
            vm.currentBranch = branchService.getCurrentBranch();

            if (vm.currentBranch.Id !== undefined)
            {
                init();
            }
        };

        preInit();
        //init();

        $rootScope.$on('branch:updated', function (event, data) {
        	vm.currentBranch = data;
        	init();
        });

        $rootScope.$on('storage:updated', function (event, data) {
        	init();
        });
    };

    StorageController.$inject = injectParams;

    app.register.controller('StorageController', StorageController);

});