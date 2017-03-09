window.GeekWear.app.directive('loadingIndicator', ['$rootScope', function ($rootScope) {
    return {
        restrict: 'E',
        template: '<div class="ajax-loading" ng-show="isLoading.value"></div>',
        link: function (scope, element, attrs) {
            scope.isLoading = $rootScope.AjaxInProgress;
        }
    };
}]);
