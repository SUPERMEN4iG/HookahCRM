'use strict';

define(['app'], function (app) {

    var injectParams = ['$http', '$q', 'baseApiUrl'];

    var additionService = function ($http, $q, baseApiUrl) {
        var serviceBase = baseApiUrl + 'addition/',
            service = {};

        service.getActiveAdditions = function () {
            return $http.get(serviceBase + 'list/').then(
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

    additionService.$inject = injectParams;

    app.factory('additionService', additionService);

});