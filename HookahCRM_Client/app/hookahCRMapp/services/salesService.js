'use strict';

define(['app'], function (app) {

    var injectParams = ['$http', '$q', 'baseApiUrl'];

    var salesService = function ($http, $q, baseApiUrl) {
        var serviceBase =  baseApiUrl + 'sales/',
            factory = {};

        //factory.getUsers = function () {
        //    return $http.get(serviceBase + 'list/').then(
        //        function (response) {
        //            console.log(response.data);
        //            return response.data;
        //        });
        //};

        return factory;
    };

    salesService.$inject = injectParams;

    app.factory('salesService', salesService);

});