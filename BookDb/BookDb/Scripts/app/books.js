﻿(function(window, angular) {
    "use strict";

    var module = angular.module("app.books", [
        "ui.router",
        "services",
        "ui.select",
        "ngSanitize"
    ]);

    // Routes
    module.config([
        "$stateProvider", function($stateProvider) {
            $stateProvider
                .state("app.books", {
                    url: "/?s",
                    templateUrl: "books.html",
                    controller: "BooksCtrl",
                    data: {
                        pageTitle: "Books - BookDb"
                    }
                })
                .state("app.bookdetails", {
                    url: "/{id:int}",
                    templateUrl: "books.details.html",
                    controller: "BookDetailsCtrl",
                    data: {
                        pageTitle: "Book Details - BookDb"
                    }
                })
                .state("app.bookcreate", {
                    url: "/new",
                    templateUrl: "books.create.html",
                    controller: "BookCreateCtrl",
                    data: {
                        pageTitle: "New Book - BookDb"
                    }
                });
        }
    ]);

    // Controllers
    module.controller("BooksCtrl", [
        "$scope", "$state", "bookService", "$stateParams",
        function($scope, $state, bookService, $stateParams) {

            // PROPERTIES
            $scope.items = [];

            $scope.isLoading = false;

            function filterItems() {

                $scope.isLoading = true;

                var options = {};
                if ($stateParams.s === "p") {
                    options.byPublishedDate = true;
                }

                return bookService.query(options)
                    .then(function(data) {
                        $scope.items = data;
                        $scope.isLoading = false;
                    }, function() {
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

    module.controller("BookDetailsCtrl", [
        "$scope", "$state", "bookService", "$stateParams", "moment", "authorService", "imageService",
        function($scope, $state, bookService, $stateParams, moment, authorService, imageService) {

            $scope.item = null;
            $scope.authors = [];
            $scope.isLoading = false;

            function parseDate(dateStr) {
                var date = moment(dateStr);;
                return date.toDate();
            }

            function printDate(date) {
                return date ? date.toISOString() : null;
            }

            $scope.uploadImage = function (file) {
                if (!file.length) {
                    return;
                }

                $scope.isLoading = true;
                imageService.upload(file)
                    .success(function(data, status, headers) {
                        $scope.item.image = headers("Location");
                        $scope.isLoading = false;
                    })
                    .error(function() {
                        $scope.isLoading = false;
                    });
            };

            $scope.update = function(form, item) {
                if (form.$invalid) {
                    return;
                }

                item.published = printDate(item.publishedDate);

                item.$isLoading = true;
                item.$update().then(function() {
                    item.publishedDate = parseDate(item.published);
                    item.$isLoading = false;
                }, function(reason) {
                    item.$isLoading = false;
                    throw new Error(reason);
                });

            };

            $scope.delete = function(item) {
                item.$delete().then(function() {
                    $state.go("^.books");
                }, function(reason) {
                    throw new Error(reason);
                });
            };

            function load(id) {
                $scope.isLoading = true;
                return bookService.get(id).then(function(data) {
                    $scope.item = data;
                    data.publishedDate = parseDate(data.published);
                    $scope.isLoading = false;
                }, function() {
                    $scope.isLoading = false;
                });
            }

            function loadAuthors() {
                return authorService.query($scope.filter)
                    .then(function(data) {
                        $scope.authors.length = 0;
                        angular.extend($scope.authors, data);
                        $scope.isLoading = false;
                    }, function() {
                        $scope.isLoading = false;
                    });
            }

            loadAuthors().then(
                function() {
                    load($stateParams.id);
                });
        }
    ]);

    module.controller("BookCreateCtrl", [
        "$scope", "$state", "bookService", "authorService", "imageService",
        function($scope, $state, bookService, authorService, imageService) {

            $scope.item = bookService.create({});
            $scope.authors = [];
            $scope.isLoading = false;

            $scope.uploadImage = function (file) {
                if (!file.length) {
                    return;
                }

                $scope.isLoading = true;
                imageService.upload(file)
                    .success(function (data, status, headers) {
                        $scope.item.image = headers("Location");
                        $scope.isLoading = false;
                    })
                    .error(function () {
                        $scope.isLoading = false;
                    });
            };

            $scope.create = function(form, item) {
                if (form.$invalid) {
                    return;
                }

                item.$isLoading = true;
                item.$save().then(function() {
                    item.$isLoading = false;
                    $state.go("^.books");
                }, function(reason) {
                    item.$isLoading = false;
                    throw new Error(reason);
                });

            };

            function loadAuthors() {
                return authorService.query($scope.filter)
                    .then(function(data) {
                        $scope.authors.length = 0;
                        angular.extend($scope.authors, data);
                        $scope.isLoading = false;
                    }, function() {
                        $scope.isLoading = false;
                    });
            }

            loadAuthors();
        }
    ]);

})(window, window.angular);