window.GeekWear.app.service('LoadingInterceptor',
    ['$q',
     '$rootScope',
     '$http',
    function ($q, $rootScope, $http) {
        'use strict';
        $rootScope.AjaxInProgress = { value: false };
        return {
            request: function (config) {
                $rootScope.AjaxInProgress.value = !!$http.pendingRequests.length;
                return config;
            },
            requestError: function (rejection) {
                $rootScope.AjaxInProgress.value = !!$http.pendingRequests.length;
                return $q.reject(rejection);
            },
            response: function (response) {
                $rootScope.AjaxInProgress.value = !!$http.pendingRequests.length;
                return response;
            },
            responseError: function (rejection) {
                $rootScope.AjaxInProgress.value = !!$http.pendingRequests.length;
                return $q.reject(rejection);
            }
        };
    }]);
