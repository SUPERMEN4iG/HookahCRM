'use strict';

define(['app'], function (app) {

    var injectParams = ['$location', '$routeParams', 'authService', 'tobaccoService', '$rootScope', 'toastr'];

    var TobaccoController = function ($location, $routeParams, authService, tobaccoService, $rootScope, toastr) {
        var vm = this;

        vm.tobacco = [];
        vm.tobaccoList = [];
        vm.controllerName = "Продукция";
        $rootScope.pageName = 'Tobacco';

        vm.selectedTobacco;

        vm.currentEditTobacco;

        vm.showModal = false;

        function init() {
            getTobaccos();
            vm.getTobaccosByCategory();
        };

        function getTobaccos() {
            tobaccoService.getTobaccoList()
                .then(function (data) {
                    vm.tobacco = data;
                });
        };

        vm.getTobaccosByCategory = function() {
            tobaccoService.getTobaccoListByCategory(vm.selectedTobacco)
                .then(function (data) {
                    vm.tobaccoList = data;
                });
        };

        vm.insertTobaccoStyle = function () {
            tobaccoService.insertTobaccoStyle(vm.currentEditTobacco)
                .then(function (data) {
                    console.log(data);
                    toastr.info('Продукт изменён!', 'Информация');
                    vm.getTobaccosByCategory();
                });
        };

        vm.update = function () {
            console.log("Update category");
            vm.getTobaccosByCategory();
        };

        vm.delete = function (id) {
            console.log("DELETE");
            tobaccoService.deleteTobaccoStyle(id)
                .then(function (response) {
                    console.log(response);
                    vm.getTobaccosByCategory();
                    toastr.info('Продукт удалён!', 'Информация');
                });
        };

        vm.closeModal = function () {
            vm.showModal = false;
            vm.update();
        };

        vm.openModal = function (obj, isEdit) {
            vm.showModal = true;

            if (isEdit) {
                vm.currentEditTobacco = obj;
            }
            else {
                vm.currentEditTobacco = {};
            }
        };

        init();
    };

    TobaccoController.$inject = injectParams;

    app.register.controller('TobaccoController', TobaccoController);

});