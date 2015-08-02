(function (window, angular) {
    'use strict';

    var module = angular.module('app.books', [
        'ui.router',
        'services'
    ]);

    // Routes
    module.config([
        '$stateProvider', function ($stateProvider) {
            $stateProvider
                .state('app.books', {
                    url: '/',
                    templateUrl: 'books.html',
                    controller: 'BooksCtrl',
                    data: {
                        pageTitle: 'Books at BookDb'
                    }
                })
                .state('app.bookdetails', {
                    url: '/{id:int}',
                    templateUrl: 'books.details.html',
                    controller: 'BookDetailsCtrl',
                    data: {
                        pageTitle: 'Book details at BookDb'
                    }
                })
                .state('app.bookcreate', {
                    url: '/new',
                    templateUrl: 'books.create.html',
                    controller: 'BookCreateCtrl',
                    data: {
                        pageTitle: 'New Book at BookDb'
                    }
                });
        }
    ]);

    // Controllers
    module.controller('BooksCtrl', [
        '$scope', '$state', 'bookService',
        function ($scope, $state, bookService) {

            // PROPERTIES
            $scope.items = [];
            $scope.filter = {
                skip: 0,
                take: 20
            };

            $scope.isLoading = false;
            $scope.isAllLoaded = false;

            function filterItems() {

                $scope.isLoading = true;
                return bookService.query($scope.filter)
                    .then(function (data) {
                        if (data.length < $scope.filter.take) {
                            $scope.isAllLoaded = true;
                        } else {
                            $scope.isAllLoaded = false;
                        }

                        if (!$scope.filter.skip) {
                            $scope.items = [];
                        }
                        $scope.items = $scope.items.concat(data);

                        $scope.isLoading = false;
                    }, function () {
                        $scope.isLoading = false;
                        throw new Error();
                    });
            };

            $scope.nextPage = function () {
                if ($scope.isAllLoaded || $scope.isLoading) {
                    return;
                }

                $scope.filter.skip += $scope.filter.take;
                filterItems();
            };

            function load() {
                $scope.isLoading = true;
                filterItems();
            }

            // INIT
            load();
        }
    ]);

    module.controller('BookDetailsCtrl', [
        '$scope', '$state', 'bookService', '$stateParams', 'moment',
        function ($scope, $state, bookService, $stateParams, moment) {

            $scope.item = null;
            $scope.isLoading = false;

            function parseDate(dateStr) {
                var date = moment(dateStr);;
                return date.toDate();
            }

            function printDate(date) {
                return date.toISOString();
            }

            $scope.update = function (form, item) {
                if (form.$invalid) {
                    return;
                }

                item.published = printDate(item.publishedDate);

                item.$isLoading = true;
                item.$update().then(function () {
                    item.publishedDate = parseDate(item.published);
                    item.$isLoading = false;
                }, function (reason) {
                    item.$isLoading = false;
                    throw new Error(reason);
                });

            };

            $scope.delete = function (item) {
                item.$delete().then(function () {
                    $state.go('^.books');
                }, function (reason) {
                    throw new Error(reason);
                });
            }

            function load(id) {
                $scope.isLoading = true;
                bookService.get(id).then(function (data) {
                    $scope.item = data;
                    data.publishedDate = parseDate(data.published);
                    $scope.isLoading = false;
                }, function () {
                    $scope.isLoading = false;
                });
            }

            

            load($stateParams.id);
        }
    ]);

    module.controller('BookCreateCtrl', [
        '$scope', '$state', 'bookService',
        function ($scope, $state, bookService) {

            $scope.item = bookService.create({});
            $scope.isLoading = false;

            $scope.create = function (form, item) {
                if (form.$invalid) {
                    return;
                }

                item.$isLoading = true;
                item.$save().then(function () {
                    item.$isLoading = false;
                    $state.go('^.books');
                }, function (reason) {
                    item.$isLoading = false;
                    throw new Error(reason);
                });

            };
        }
    ]);

})(window, window.angular);
