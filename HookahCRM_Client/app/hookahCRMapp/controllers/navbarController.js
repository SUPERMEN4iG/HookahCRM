'use strict';

define(['app'], function (app) {

    var injectParams = ['$scope', '$location', 'authService', 'branchService', '$rootScope', 'toastr'];

    var NavbarController = function ($scope, $location, authService, branchService, $rootScope, toastr) {
        var vm = this;

        //$rootScope.pageName = '';
        vm.appTitle = 'HookahCRM';
        vm.loginLogoutText = '';
        vm.isCollapsed = false;
        vm.currentUser = {};

        vm.selectedBranch = null;
        vm.branchActiveList = [];
        vm.showSelectBranchModal = false;
        vm.currentBranch = {};
        //vm.showSelectStorageModal = false;

        authService.getCurrentUser().then(function (response) {
            vm.currentUser = response.data;
        });

        vm.loginOrOut = function () {
            setLoginLogoutState();
            var isAuthenticated = authService.isAuthorize;
            if (isAuthenticated) { //logout 
                console.log("LOGOUT");
                authService.logout();
                $location.path('/');
            }
            redirectToLogin();
        };

        function redirectToLogin() {
            var path = '/login' + $location.$$path;
            $location.replace();
            $location.path(path);
        }

        $scope.$on('loginStatusChanged', function (loggedIn) {
            setLoginLogoutState(loggedIn);
        });

        $scope.$on('redirectToLogin', function () {
            redirectToLogin();
        });

        function setLoginLogoutState() {
            vm.loginLogoutText = (authService.isAuthorize) ? 'Logout' : 'Login';
            vm.showSelectBranchModal = authService.isAuthorize;

            if (authService.isAuthorize) {
                branchService.getActiveBranchList().then(function (data) {
                    vm.branchActiveList = data;
                });
            }
            else {
                vm.branchActiveList = [];
            }
        }

        vm.selectBranch = function () {
            if (vm.selectedBranch == null)
                return

            console.info(vm.selectedBranch);

            branchService.setCurrentBranch(vm.selectedBranch);
            toastr.info('Выбрано заведение ' + vm.selectedBranch, 'Информация');

            console.info(vm.currentBranch);

            $rootScope.$broadcast('onSelectBranch', {
                showSelectStorageModal: true,
                selectedBranch: vm.currentBranch,
                isCloseRepot: false
            });
        };

        //vm.currentBranch = function () { return branchService.getCurrentBranch()[0]; };

    	//$rootScope.currentBranch = vm.currentBranch;

        $rootScope.$on('branch:updated', function (event, data) {;
        	vm.currentBranch = data;
        });

        setLoginLogoutState();

        vm.notFoundClick = function (val) {
            toastr.info(val, 'Информация');
        };

    };

    NavbarController.$inject = injectParams;

    //Loaded normally since the script is loaded upfront 
    //Dynamically loaded controller use app.register.controller
    app.controller('NavbarController', NavbarController);

});