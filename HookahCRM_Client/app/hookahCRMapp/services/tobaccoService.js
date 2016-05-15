﻿'use strict';

define(['app'], function (app) {

    var injectParams = ['$http', '$q', 'baseApiUrl'];

    var tobaccoService = function ($http, $q, baseApiUrl) {
        var factory = {},
            serviceBase = baseApiUrl + 'tobacco/';

        factory.getTobaccoList = function () {
            return $http.get(serviceBase + 'TobaccoCategory/').then(
                function (response) {
                    console.log(response.data);
                    return response.data;
                });
        };

        factory.getTobaccoListByCategory = function (idList) {
            return $http.get(serviceBase + 'TobaccoStyle/', { params: { take: 10, skip: 0, idList: idList } }).then(
                function (response) {
                    console.log(response.data);
                    return response.data;
                });
        };

        factory.insertTobaccoStyle = function (obj) {
            console.log(obj);
            $http.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
            return $http({
                method: 'PUT',
                url: serviceBase + 'TobaccoStyle/',
                data: JSON.stringify(obj)
                }).then(
                function (response) {
                    return response;
                });
        };

        factory.deleteTobaccoStyle = function (id) {
            console.log(id);
            $http.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
            return $http.delete(serviceBase + 'TobaccoStyle/' + id).then(
                function (response) {
                    return response;
                });
        };

        return factory;
    };

    tobaccoService.$inject = injectParams;

    app.factory('tobaccoService', tobaccoService);

});