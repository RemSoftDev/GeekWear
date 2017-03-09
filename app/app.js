window.GeekWear = (function () {
    var app = angular.module('GeekWear', [
        'ngSanitize',
        'ngAnimate',
        'ngMessages',
        'ui.bootstrap',
        'ja.qr'
    ]);

    return {
        app: app
    };
})();
/*
window.GeekWear.app.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.interceptors.push(function ($http) {
        var user_token = $cookieStore.get('user_token');

        return {
            'request': function (config) {
                if (user_token) {
                    config.headers['Authorization'] = user_token;
                }
                return config;
            },
            'responseError': function (responseError) {
                if (responseError.status == 401) {
                    window.location = web_login_url;
                }
                return responseError;
            },

            'response': function (response) {
                return response;
            }
        };
    });
}]);
//GeekWear.app
*/
