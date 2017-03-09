window.GeekWear.app.controller('HomeController', [
    '$scope',
    '$q',
    '$rootScope',
    '$timeout',
    'TextConverters',
    'ProjectService', function ($scope,
                          $q,
                          $rootScope,
                          $timeout,
                          TextConverters,
                          ProjectService) {
        $scope.outputText = '';
        var newProject = {
            id: 0,
            transform: 'none',
            shirtColor: 'white',
            textInput: '',
            userId: ''
        };

        $scope.project = angular.copy(newProject);

        if (geekWearUtils && geekWearUtils.getUrlParam) {
            var projectId = geekWearUtils.getUrlParam('projectId') || localStorage.getItem('projectId');

            if (projectId) {
                ProjectService.get(projectId).then(function (data) {
                    if (data && data.transform) {
                        $scope.project = data;

                        if ($scope.project.transform && $scope.transforms[$scope.project.transform]) {
                            $scope.selectedTransform = $scope.transforms[$scope.project.transform];
                            $scope.transformText();
                        }
                        else {
                            $scope.selectedTransform = $scope.transforms.notransform;
                        }

                        $scope.selectedSize = $scope.sizes[0];
                        $scope.sizes.forEach(function (size) {
                            if ($scope.project.size == size.value) {
                                $scope.selectedSize = size
                            }
                        });

                        localStorage.setItem('projectId', data.userId ? '' : projectId);
                        if (!geekWearUtils.getUrlParam('projectId')) {
                            history.replaceState(null, '', '?projectId=' + projectId);
                        }
                    }
                }, function (data) {
                    console.log(data);
                });
            }
        }

        $scope.transforms = TextConverters;

        $scope.sizes = [
            {
                label: "XS",
                value: 0
            },
            {
                label: "S",
                value: 1
            },
            {
                label: "M",
                value: 2
            },
            {
                label: "L",
                value: 3
            },
            {
                label: "XL",
                value: 4
            },
            {
                label: "XXL",
                value: 5
            },
            {
                label: "XXXL",
                value: 6
            }
        ];

        $scope.selectedTransform = $scope.transforms.notransform;
        $scope.selectedSize = $scope.sizes[0];

        $scope.setTransform = function (transformType) {
            $scope.selectedTransform = $scope.transforms[transformType];
            $scope.project.transform = transformType;
            $scope.transformText();
        };

        $scope.setSize = function (size) {
            $scope.selectedSize = size;
            $scope.project.size = size.value;
        };

        $scope.transformText = function () {
            $scope.outputText = $scope.selectedTransform.transformer($scope.project.textInput);
            if ($scope.project.id) {
                $scope.qrLink = window.location.origin + '/?projectId=' + $scope.project.id;
            } else {
                $scope.qrLink = '';
            }
        };
        $scope.new = function () {
            ProjectService.save($scope.project).then(function (data) {
                var project = angular.copy(newProject);
                project.userId = $scope.project.userId;
                $scope.project = project;
                $scope.selectedTransform = $scope.transforms.notransform;
                $scope.selectedSize = $scope.sizes[0];
                $scope.transformText();
                history.replaceState(null, '', '/');
            });
        }

        $scope.save = function () {
            ProjectService.save($scope.project).then(function (data) {
                window.location = "/cart";
            });
        }
    }]);
