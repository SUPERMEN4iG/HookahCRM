'use strict';

define(['app'], function (app) {

    var injectParams = ['$parse'];

    var modalDirective = function ($parse) {
        return {
            restrict: 'E',
            replace: true,
            transclude: true,
            require: 'ngModel',
            scope: {
                closable: '=',
                scrolling: '=',
                onShow: '&',
                onHide: '&',
            },
            template: '<div class="ui modal" ng-transclude></div>',
            link: function (scope, element, attrs, ngModel) {
                element.modal({
                    onHide: function () {
                        ngModel.$setViewValue(false);
                        scope.onHide();
                    },
                    closable: scope.closable,
                    onShow: scope.onShow
                });

                element.addClass(scope.scrolling ? 'scrolling' : '');
                
                scope.$watch(function () {
                    return ngModel.$modelValue;
                }, function (modelValue) {
                    element.modal(modelValue ? 'show' : 'hide');
                });
                scope.$on('$destroy', function () {
                    element.modal('hide');
                    element.remove();
                });
            }
        }
    };

    modalDirective.$inject = injectParams;

    app.directive('modal', modalDirective);

});
