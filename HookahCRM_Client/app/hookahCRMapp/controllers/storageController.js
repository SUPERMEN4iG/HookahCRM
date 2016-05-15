'use strict';

define(['app'], function (app) {

    var injectParams = ['$location', '$routeParams', 'authService', '$rootScope', 'toastr', '$route', 'storageService', 'tobaccoService'];

    var StorageController = function ($location, $routeParams, authService, $rootScope, toastr, $route, storageService, tobaccoService) {
        var vm = this,
            path = '/';

        vm.tobaccoList = [];

        $rootScope.pageName = 'Storage';
        vm.controllerName = 'Склад';

        vm.closeStorage = function () {
            $rootScope.$broadcast('onSelectBranch', {
                showSelectStorageModal: true,
                selectedBranch: $rootScope.currentBranch,
                isCloseRepot: true
            });
        };

        function init() {
            vm.tobaccoList = tobaccoService.getTobaccoList();
        };

        init();
    };

    StorageController.$inject = injectParams;

    app.register.controller('StorageController', StorageController);

});