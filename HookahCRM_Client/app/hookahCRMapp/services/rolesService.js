'use strict';

define(['app'], function (app) {

    var injectParams = ['$http', '$rootScope', 'baseApiUrl', '$filter', '$q'];

    var rolesService = function ($http, $rootScope, baseApiUrl, $filter, $q) {
        var service = {},
            serviceBase = baseApiUrl + 'roles/',
            listRoles = [];

        service.getActiveRoles = function (branchId, isClosed) {
            var deferred = $q.defer();

            if (listRoles.length == 0) {
                $http.get(serviceBase + 'ActiveRolesList/').then(
                        function (response) {
                            deferred.resolve(response.data);
                            listRoles.push(response.data);
                        },
                        function (response) {
                            deferred.reject(response.data);
                        });
            }
            else {
                angular.forEach(listRoles, function (val) {
                    deferred.resolve(val);
                });
            }

            return deferred.promise;
        };

        service.clear = function () {
            listRoles.splice(0, listRoles.length);
        };

        service.getActiveBranchByName = function (name) {
            if (currentBranch == null)
                return;

            return $filter('filter')(list, { Name: name })[0];
        };

        return service;
    };

    rolesService.$inject = injectParams;

    app.factory('rolesService', rolesService);

});
