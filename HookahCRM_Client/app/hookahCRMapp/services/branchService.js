'use strict';

define(['app'], function (app) {

    var injectParams = ['$http', '$rootScope', 'baseApiUrl', '$filter'];

    var branchFactory = function ($http, $rootScope, baseApiUrl, $filter) {
        var service = {},
            serviceBase = baseApiUrl + 'branch/',
            currentBranch = {},
            list = [];

        service.getActiveBranchList = function () {
            return $http.get(serviceBase + 'ActiveBranchList/').then(
                function (response) {
                    list.push(response.data);
                    return response.data;
                });
        };

        service.getActiveBranchByName = function (name) {
            if (currentBranch == null)
                return;

            return $filter('filter')(list, { Name: name })[0];
        };

        service.getCurrentBranch = function () {
            return currentBranch;
        };

        service.setCurrentBranch = function (newBranch) {
            if (newBranch == null)
                return;

            var obj = service.getActiveBranchByName(newBranch);
            currentBranch = obj;
        };

        return service;
    };

    branchFactory.$inject = injectParams;

    app.factory('branchService', branchFactory);

});
