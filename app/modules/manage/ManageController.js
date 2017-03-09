window.GeekWear.app.controller('ManageController', [
    '$scope',
    '$q',
    '$rootScope',
    '$timeout',
    'ProjectService', function ($scope,
                          $q,
                          $rootScope,
                          $timeout,
                          ProjectService) {
        $scope.projects = [];

        $timeout(load);

        $scope.edit = function (projectId) {
            window.location = '/?projectId=' + projectId;
        };

        $scope.duplicate = function (project) {
            var newProject = angular.copy(project);
            newProject.id = 0;
            ProjectService.save(newProject).then(function (data) {
                if (data && data.id) {
                    load();
                }
            });
            //server does not allow PUT
            //ProjectService.duplicate(projectId).then(function (data) {
            //    if (data.ok) {
            //        load();
            //    }
            //});
        };

        $scope.delete = function (project) {
            if (confirm("Are you sure you want to delete this project?")) {
                ProjectService.remove(project).then(function (data) {
                    if (data && data.id) {
                        load();
                    }
                });
            }
        };

        function load() {
            ProjectService.getForUser().then(function (data) {
                $scope.projects = data;
            })
        }
    }]);
