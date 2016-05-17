'use strict';

define(['app'], function (app) {

    var injectParams = ['$http', '$q', 'baseApiUrl'];

    var expendableService = function ($http, $q, baseApiUrl) {
        var serviceBase = baseApiUrl + 'Expendable/',
            service = {};

        service.getActiveExpendable = function () {
            return $http.get(serviceBase + 'ActiveExpendableList/').then(
                function (response) {
                    return response.data;
                });
        };

        //factory.getUsers = function () {
        //    return $http.get(serviceBase + 'list/').then(
        //        function (response) {
        //            console.log(response.data);
        //            return response.data;
        //        });
        //};

        return service;
    };

    expendableService.$inject = injectParams;

    app.factory('expendableService', expendableService);

});