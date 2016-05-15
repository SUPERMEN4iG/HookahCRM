'use strict';

define(['app'], function (app) {

    var injectParams = ['$http'];

    var loadingDirective = function ($http) {
        return {
            restrict: 'A',
            link: function (scope, elm, attrs) {
                scope.isLoading = function () {
                    return $http.pendingRequests.length > 0;
                };

                //var firstLoad = true;

                scope.$watch(scope.isLoading, function (v) {
                    if (v) {
                        elm.fadeIn('slow');
                    } else {
                        elm.fadeOut('slow');
                    }
                });
            }
        }
    };

    loadingDirective.$inject = injectParams;

    app.directive('loading', loadingDirective);

});
