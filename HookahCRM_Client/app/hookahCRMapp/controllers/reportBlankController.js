'use strict';

define(['app'], function (app) {

    var injectParams = ['$scope', '$location', 'authService', '$rootScope', 'toastr', 'tobaccoService', 'storageService'];

    var ReportBlankController = function ($scope, $location, authService, $rootScope, toastr, tobaccoService, storageService) {
        var vm = this;

        vm.tobaccoList = {};
        vm.currentReportBlank = {};
        vm.showSelectStorageModal = false;
        vm.showSelectStorageModalTemp = false;
        vm.currentBranch = [];
        vm.isCloseRepot = false;

        function init() {
            storageService.getReportBlank(vm.currentBranch().StorageId, vm.isCloseRepot).then(function (data) {
                console.log(data);

                if ((vm.isCloseRepot || data.IsClosed) || (data[0] == null && data[1] == null)) {
                    tobaccoService.getTobaccoList().then(function (value) {
                        vm.tobaccoList = value;
                        vm.showSelectStorageModal = vm.showSelectStorageModalTemp;

                        //vm.currentReportBlank[0] = {};
                        //angular.forEach(value, function (data) {
                        //    vm.currentReportBlank[0][data.Id] = {};
                        //    angular.forEach(data.TobaccoList, function (dataTobacco) {
                        //        console.log(dataTobacco);
                        //        vm.currentReportBlank[0][data.Id][dataTobacco.Id] = 1;
                        //    });
                        //    //vm.currentReportBlank[0][key.TobaccoStyle.Category.Id][key.TobaccoStyle.Id] = key.Weight;
                        //});

                        vm.currentReportBlank[0] = {};
                        angular.forEach(value, function (data) {
                            vm.currentReportBlank[0][data.Id] = {};
                            angular.forEach(data.TobaccoList, function (dataTobacco) {
                                vm.currentReportBlank[0][data.Id][dataTobacco.Id] = 0;
                            });
                        });

                        angular.forEach(data[0].StorageTobaccoList, function (key, val) {
                            vm.currentReportBlank[0][key.TobaccoStyle.Category.Id][key.TobaccoStyle.Id] = key.Weight;
                        });

                        console.log(vm.currentReportBlank);
                    });
                }
                else {
                    vm.showSelectStorageModal = false;
                }
            });
        };

        vm.insertReportStorageBlank = function (isClose) {
            storageService.putReportBlank(vm.currentReportBlank, vm.currentBranch().StorageId, vm.isCloseRepot).then(function (value) {
                if (!vm.isCloseRepot)
                    toastr.info('Бланк отчётности сохранён! Удачного рабочего дня!', 'Информация');
                else
                    toastr.info('Бланк отчётности сохранён! Рабочий день закончен!', 'Информация');
            });
        };

        $rootScope.$on('onSelectBranch', function (event, data) {
            vm.showSelectStorageModalTemp = data.showSelectStorageModal;
            vm.currentBranch = data.selectedBranch;
            vm.isCloseRepot = data.isCloseRepot;
            init();
        });
    };

    ReportBlankController.$inject = injectParams;

    app.controller('ReportBlankController', ReportBlankController);

});