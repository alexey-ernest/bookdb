(function (window, angular) {
    'use strict';

    var module = angular.module('app.authors', [
        'ui.router',
        'services'
    ]);

    // Routes
    module.config([
        '$stateProvider', function ($stateProvider) {
            $stateProvider
                .state('app.authors', {
                    url: '/authors',
                    templateUrl: 'authors.html',
                    controller: 'AuthorsCtrl',
                    data: {
                        pageTitle: 'Authors - BookDb'
                    }
                })
                .state('app.authordetails', {
                    url: '/authors/{id:int}',
                    templateUrl: 'authors.details.html',
                    controller: 'AuthorDetailsCtrl',
                    data: {
                        pageTitle: 'Author Details - BookDb'
                    }
                })
                .state('app.authorcreate', {
                    url: '/authors/new',
                    templateUrl: 'authors.create.html',
                    controller: 'AuthorCreateCtrl',
                    data: {
                        pageTitle: 'New Author - BookDb'
                    }
                });
        }
    ]);

    // Controllers
    module.controller('AuthorsCtrl', [
        '$scope', '$state', 'authorService',
        function ($scope, $state, authorService) {

            // PROPERTIES
            $scope.items = [];
            $scope.isLoading = false;

            function filterItems() {

                $scope.isLoading = true;
                return authorService.query($scope.filter)
                    .then(function (data) {
                        $scope.items = data;
                        $scope.isLoading = false;
                    }, function () {
                        $scope.isLoading = false;
                        throw new Error();
                    });
            };

            function load() {
                $scope.isLoading = true;
                filterItems();
            }

            // INIT
            load();
        }
    ]);

    module.controller('AuthorDetailsCtrl', [
        '$scope', '$state', 'authorService', '$stateParams',
        function ($scope, $state, authorService, $stateParams) {

            $scope.item = null;
            $scope.isLoading = false;

            $scope.update = function (form, item) {
                if (form.$invalid) {
                    return;
                }

                item.$isLoading = true;
                item.$update().then(function () {
                    item.$isLoading = false;
                }, function (reason) {
                    item.$isLoading = false;
                    throw new Error(reason);
                });

            };

            $scope.delete = function (item) {
                item.$delete().then(function () {
                    $state.go('^.authors');
                }, function (reason) {
                    throw new Error(reason);
                });
            }

            function load(id) {
                $scope.isLoading = true;
                authorService.get(id).then(function (data) {
                    $scope.item = data;
                    $scope.isLoading = false;
                }, function () {
                    $scope.isLoading = false;
                });
            }

            load($stateParams.id);
        }
    ]);

    module.controller('AuthorCreateCtrl', [
        '$scope', '$state', 'authorService',
        function ($scope, $state, authorService) {

            $scope.item = authorService.create({});
            $scope.isLoading = false;

            $scope.create = function (form, item) {
                if (form.$invalid) {
                    return;
                }

                item.$isLoading = true;
                item.$save().then(function () {
                    item.$isLoading = false;
                    $state.go('^.authors');
                }, function (reason) {
                    item.$isLoading = false;
                    throw new Error(reason);
                });

            };
        }
    ]);

})(window, window.angular);
