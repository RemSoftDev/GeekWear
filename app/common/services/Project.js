window.GeekWear.app.factory('ProjectService',
    ['$http',
     '$q', function (
            $http,
            $q) {
         return {
             save: function (project) {
                 var deferred = $q.defer();
                 $http({
                     url: 'api/Projects/',
                     method: "POST",
                     data: project
                 }).success(function (data) {
                     deferred.resolve(data);
                 }).error(function (data) {
                     deferred.reject(data);
                 });
                 return deferred.promise;
             },
             remove: function (project/*Id*/) {
                 project.transform = 'deleteme';
                 var deferred = $q.defer();
                 $http({
                     url: 'api/Projects/',
                     method: 'POST',
                     /*method: "DELETE",
                     params: {
                         id: projectId
                     }*/
                     data: project
                 }).success(function (data) {
                     deferred.resolve(data);
                 }).error(function (data) {
                     deferred.reject(data);
                 });
                 return deferred.promise;
             },
             get: function (projectId) {
                 var deferred = $q.defer();
                 $http({
                     url: 'api/Projects/',
                     method: "GET",
                     params: {
                         id: projectId
                     }
                 }).success(function (data) {
                     deferred.resolve(data);
                 }).error(function (data) {
                     deferred.reject(data);
                 });
                 return deferred.promise;
             },
             duplicate: function (projectId) {
                 var deferred = $q.defer();
                 $http({
                     url: 'api/Projects/',
                     method: "PUT",
                     params: {
                         id: projectId
                     }
                 }).success(function (data) {
                     deferred.resolve(data);
                 }).error(function (data) {
                     deferred.reject(data);
                 });
                 return deferred.promise;
             },
             getForUser: function () {
                 var deferred = $q.defer();
                 $http({
                     url: 'api/Projects/',
                     method: "GET"
                 }).success(function (data) {
                     deferred.resolve(data);
                 }).error(function (data) {
                     deferred.reject(data);
                 });
                 return deferred.promise;
             }
         }
     }]);
