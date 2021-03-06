﻿'use strict';

define(['hookahCRMapp/services/routeResolver'], function () {

    var app = angular.module('hookahCRMapp', [
        'ngRoute',
        'ngAnimate',
        'ngCookies',
        'routeResolverServices',
        'ui.bootstrap',
        //'angularify.semantic.modal',
        'angularify.semantic.dropdown',
        'chart.js',
        'toastr'
    ]);

    app.constant('baseApiUrl', 'http://localhost:3470/api/');
    app.constant('DebugConfig', {
        isDebug: true,
        version: '1.4'
    });

    app.provider('DevelopConstants', function () {
        // default values
        var values = {
            isDebug: true,
            version: '1.4'
        };
        return {
            $get: function () {
                return values;
            }
        };
    });

    app.config(['$routeProvider', 'routeResolverProvider', '$controllerProvider',
                '$compileProvider', '$filterProvider', '$provide', '$httpProvider',

        function ($routeProvider, routeResolverProvider, $controllerProvider,
                  $compileProvider, $filterProvider, $provide, $httpProvider) {

            //Change default views and controllers directory using the following:
            //routeResolverProvider.routeConfig.setBaseDirectories('/app/views', '/app/controllers');

            app.register =
            {
                controller: $controllerProvider.register,
                directive: $compileProvider.directive,
                filter: $filterProvider.register,
                factory: $provide.factory,
                service: $provide.service
            };

            //Define routes - controllers will be loaded dynamically
            var route = routeResolverProvider.route;

            $routeProvider
                .when('/users', route.resolve('Users', '', 'vm'))
                .when('/login/:redirect*?', route.resolve('Login', '', 'vm'))
                .when('/tobacco', route.resolve('Tobacco', '', 'vm'))
                .when('/storage', route.resolve('Storage', '', 'vm'))
                .when('/main', route.resolve('Main', '', 'vm'))
                .when('/sales/:salesType*?/:advancedParam*?', route.resolve('Sales', '', 'vm', true))
                .otherwise({ redirectTo: '/main' });

            //$routeProvider
            //    //route.resolve() now accepts the convention to use (name of controller & view) as well as the 
            //    //path where the controller or view lives in the controllers or views folder if it's in a sub folder. 
            //    //For example, the controllers for customers live in controllers/customers and the views are in views/customers.
            //    //The controllers for orders live in controllers/orders and the views are in views/orders
            //    //The second parameter allows for putting related controllers/views into subfolders to better organize large projects
            //    //Thanks to Ton Yeung for the idea and contribution
            //    .when('/customers', route.resolve('Customers', 'customers/', 'vm'))
            //    .when('/customerorders/:customerId', route.resolve('CustomerOrders', 'customers/', 'vm'))
            //    .when('/customeredit/:customerId', route.resolve('CustomerEdit', 'customers/', 'vm', true))
            //    .when('/orders', route.resolve('Orders', 'orders/', 'vm'))
            //    .when('/about', route.resolve('About', '', 'vm'))
            //    .when('/login/:redirect*?', route.resolve('Login', '', 'vm'))
            //    .otherwise({ redirectTo: '/customers' });

        }]);

    //app.run(['$rootScope', '$location', 'authService',
    //    function ($rootScope, $location, authService) {

    //        //Client-side security. Server-side framework MUST add it's 
    //        //own security as well since client-based security is easily hacked
    //        $rootScope.$on("$routeChangeStart", function (event, next, current) {
    //            if (next && next.$$route && next.$$route.secure) {
    //                if (!authService.user.isAuthenticated) {
    //                    $rootScope.$evalAsync(function () {
    //                        authService.redirectToLogin();
    //                    });
    //                }
    //            }
    //        });

    //    }]);

    app.run(['$rootScope', '$location', '$cookieStore', '$http', 'DevelopConstants',
        function ($rootScope, $location, $cookieStore, $http, DevelopConstants) {

            $rootScope.developConstants = DevelopConstants;

            $rootScope.globals = $cookieStore.get('globals') || {};

            if ($rootScope.globals.currentUser) {
                $http.defaults.headers.common['Authorization'] = 'Basic ' + $rootScope.globals.currentUser.authdata; // jshint ignore:line
            }

            $rootScope.debugTest = function (message) { console.log(message); };

            $rootScope.$on('$locationChangeStart', function (event, next, current) {
                // redirect to login page if not logged in
                console.log($rootScope.globals.currentUser);
                //if ($rootScope.globals.currentUser !== undefined && $rootScope.globals.currentUser.currentUser !== undefined)
                //{
                //    $http.defaults.headers.common['Authorization'] = 'Basic ' + $rootScope.globals.currentUser.authdata;
                //    console.log($http.defaults.headers);
                //}
                if ($location.path() !== '/login' && !$rootScope.globals.currentUser) {
                    $location.path('/login');
                }
                else {
                }
            });

            //delete $httpProvider.defaults.headers.common['X-Requested-With'];
            //$httpProvider.defaults.headers.common['Access-Control-Allow-Headers'] = '*';

            //Client-side security. Server-side framework MUST add it's 
            //own security as well since client-based security is easily hacke

        }]);

    return app;

});