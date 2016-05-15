'use strict';

define(['app'], function (app) {

    var injectParams = ['$http', '$rootScope', 'baseApiUrl', '$filter'];

    var storageService = function ($http, $rootScope, baseApiUrl, $filter) {
        var service = {},
            serviceBase = baseApiUrl + 'storage/';

        service.getReportBlank = function (storageId, isClosed) {
            return $http.get(serviceBase + 'ReportBlank/', { params: { storageId: storageId, isClosed: isClosed } }).then(
                function (response) {
                    return response.data;
                });
        };

        service.putReportBlank = function (obj, storageId, isClose) {
            //$http.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
            console.log(obj);
            return $http({
                method: 'PUT',
                url: serviceBase + 'ReportBlank/',
                data: { 'storageId': storageId, 'model': obj, 'isClose': isClose }
            }).then(
                function (response) {
                    return response;
                });
        };

        service.getActiveBranchByName = function (name) {
            if (currentBranch == null)
                return;

            return $filter('filter')(list, { Name: name })[0];
        };

        return service;
    };

    storageService.$inject = injectParams;

    app.factory('storageService', storageService);

});
