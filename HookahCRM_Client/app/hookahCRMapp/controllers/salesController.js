'use strict';

define(['app'], function (app) {

    var injectParams = ['$location', '$routeParams', 'authService', 'salesService', '$interval', '$rootScope'];

    var SalesController = function ($location, $routeParams, authService, salesService, $interval, $rootScope) {
        var vm = this,
            salesType = ($routeParams.salesType) ? $routeParams.salesType : '',
            advancedParam = ($routeParams.advancedParam) ? $routeParams.advancedParam : '';

        //vm.users = [];
        vm.controllerName = "Продажи";
        $rootScope.pageName = 'Sales';

        vm.labels = [];
        vm.series = ['Работник A', 'Работник B'];
        vm.data = [];

        //function init() {
        //    getUsers();
        //};

        //function getUsers() {
        //    usersService.getUsers()
        //        .then(function (data) {
        //            vm.users = data;
        //        });
        //};

        function init() {
            switch (salesType) {
                case 'year':
                    console.log('year');

                    vm.labels = ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"];

                    for (var i = 0; i < vm.series.length; i++) {
                        vm.data.push(new Array());
                        for (var y = 0; y < vm.labels.length; y++) {
                            vm.data[i].push(getRandomArbitary(0, 100));
                        }
                    }

                    break;

                case 'month':
                    console.log('month');
                    var daysArray = getDaysArray(2016, parseInt(advancedParam));

                    vm.labels = daysArray;

                    for (var i = 0; i < vm.series.length; i++) {
                        vm.data.push(new Array());
                        for (var y = 0; y < daysArray.length; y++) {
                            vm.data[i].push(getRandomArbitary(0, 100));
                        }
                    }

                    break;

                case 'now':
                    break;
                default:
                    console.log('default');
                    salesType = 'year';
                    advancedParam = new Date().getYear();
                    init();
                    break;
            }
        };

        function daysInMonth(month, year) {
            return new Date(year, month, 0).getDate();
        }

        function getDaysArray(year, month) {
            var numDaysInMonth = [], daysInWeek, daysIndex, index, i, l, daysArray;

            for (var i = 0; i <= 12; i++) {
                numDaysInMonth[i] = daysInMonth(i + 1, year);
            }

            daysInWeek = ['Воскресеньте', 'Понедельник', 'Вторник', 'Средя', 'Четверг', 'Пятница', 'Суббота'];
            daysIndex = { 'Sun': 0, 'Mon': 1, 'Tue': 2, 'Wed': 3, 'Thu': 4, 'Fri': 5, 'Sat': 6 };
            index = daysIndex[(new Date(year, month - 1, 1)).toString().split(' ')[0]];
            daysArray = [];

            for (i = 0, l = numDaysInMonth[month - 1]; i < l; i++) {
                daysArray.push((i + 1) + '. ' + daysInWeek[index++]);
                if (index == 7) index = 0;
            }

            return daysArray;
        }

        init();

        //$interval(function () {
        //    vm.data = [
        //      [getRandomArbitary(0, 100), getRandomArbitary(0, 100), getRandomArbitary(0, 100), getRandomArbitary(0, 100), getRandomArbitary(0, 100), getRandomArbitary(0, 100), getRandomArbitary(0, 100)],
        //      [getRandomArbitary(0, 100), getRandomArbitary(0, 100), getRandomArbitary(0, 100), getRandomArbitary(0, 100), getRandomArbitary(0, 100), getRandomArbitary(0, 100), getRandomArbitary(0, 100)]
        //    ];
        //}, 3000);

        function getRandomArbitary(min, max) {
            return Math.random() * (max - min) + min;
        };
    };

    SalesController.$inject = injectParams;

    app.config(['ChartJsProvider', function (ChartJsProvider) {
        // Configure all charts
        ChartJsProvider.setOptions({
            colours: ['#FF5252', '#FF8A80'],
            responsive: false
        });
        // Configure all line charts
        ChartJsProvider.setOptions('Line', {
            datasetFill: false
        });
    }]);
    app.register.controller('SalesController', SalesController);

});