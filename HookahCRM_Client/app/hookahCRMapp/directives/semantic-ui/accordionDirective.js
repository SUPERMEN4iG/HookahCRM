'use strict';

define(['app'], function (app) {

    var injectParams = [];

    'use strict';

    app.controller('AccordionController', ['$scope', function($scope){
        $scope.accordions = [];

        this.add_accordion = function(scope) {
            $scope.accordions.push(scope);
        
            var _this = this;
            scope.$on('$destroy', function (event) {
                _this.remove_accordion(scope);
            });
        
            return $scope.accordions;
        }

        this.closeAll = function(scope) {
            var i = 0;
            var isCloseAll = false;

            var index = $scope.accordions.indexOf(scope);

            for (i in $scope.accordions){
                if ($scope.accordions[i].close)
                    isCloseAll = true;
            }

            if (isCloseAll == true){
                for (i in $scope.accordions){
                    if (i !== index) {
                        $scope.accordions[i].active = false;
                    }
                }

                return true;
            }

            return false;

        }

        this.remove_accordion = function(scope) {
            var index = $scope.accordions.indexOf(scope);
            if ( index !== -1 ) {
                $scope.accordions.splice(index, 1);
            }
        }

        this.is_close_all = function() {
            var i = 0;
        
            for (i in $scope.accordions){
                if ($scope.accordions[i].close == 'true')
                    return true;
            }
            return false;
        }
    }]);

    app.directive('accordion', function () {
        return {
            restrict: 'E',
            replace: true,
            transclude: true,
            controller: 'AccordionController',
            scope: {
                'close': '@'
            },
            template: "<div class=\"ui accordion field\" ng-transclude></div>",
            link: function(scope, element, attrs, AccordionController) {

                if(typeof attrs.styled  !== 'undefined') {
                    element.addClass('styled');
                }

                AccordionController.add_accordion(scope);
            }
        }
    });

    app.directive('accordionGroup', function() {
        return {
            restrict: 'E',
            replace: true,
            transclude: true,
            scope : {
                title:  '@',
                open: '@',
                isField: '='
            },
            require:'^accordion',
            template: "<div class=\"ui field\">\
                       <div class=\"title\" ng-class=\"{ active: active }\" ng-click=\"click_on_accordion_tab()\"> \
                         <i class=\"dropdown icon\"></i> \
                         {{ title }} \
                       </div> \
                       <div class=\"content field\"  ng-class=\"{ active: active }\" ng-transclude> \
                       </div> \
                       </div>",

            link: function(scope, element, attrs, AccordionController) {

                // set up active
                scope.active = attrs.open === 'true';
            
                // Add the accordion to the controller
                AccordionController.add_accordion(scope);

                // Click handler
                scope.click_on_accordion_tab = function(){
                
                    // class all first of all
                    AccordionController.closeAll(scope);
                
                    // Swap the active state
                    scope.active = !scope.active;

                    // Add animation to the accordion group content
                    element.children().last().slideToggle();
                };
            }
        };
    });
});