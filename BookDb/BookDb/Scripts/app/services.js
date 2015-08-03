(function(window, angular) {
    "use strict";

    var module = angular.module("services", ["ngResource", "ngFileUpload"]);

    module.factory("$exceptionHandler", [
        function() {
            return function(exception) {

                var message = exception.message;
                if (!message || message === "[object Object]") {
                    message = "Unknown Error";
                }

                alert(message);
            };
        }
    ]);

    module.factory("bookService", [
        "$resource", "$q", function($resource, $q) {

            var resource = $resource("/api/books/:id",
            { id: "@id" },
            {
                update: { method: "PUT" }
            });


            return {
                create: function(options) {
                    return new resource(options);
                },
                get: function(id) {
                    var deferred = $q.defer();

                    resource.get({ id: id }, function(data) {
                        deferred.resolve(data);
                    }, function() {
                        deferred.reject();
                    });

                    return deferred.promise;
                },
                query: function(filter) {

                    filter = filter || {};
                    var params = {};

                    if (filter.byPublishedDate) {
                        params.id = "published";
                    }

                    var deferred = $q.defer();
                    resource.query(params, function(data) {
                        deferred.resolve(data);
                    }, function() {
                        deferred.reject();
                    });

                    return deferred.promise;
                }
            };
        }
    ]);

    module.factory("imageService", [
        "$resource", "$q", "Upload", function ($resource, $q, Upload) {

            var url = "/api/images";
            var service = {};

            service.upload = function(file) {
                var promise = Upload.upload({
                    url: url,
                    method: "POST",
                    file: file
                });

                return promise;
            };

            return service;
        }
    ]);

    module.factory("authorService", [
        "$resource", "$q", function($resource, $q) {

            var resource = $resource("/api/authors/:id",
            { id: "@id" },
            {
                update: { method: "PUT" }
            });


            return {
                create: function(options) {
                    return new resource(options);
                },
                get: function(id) {
                    var deferred = $q.defer();

                    resource.get({ id: id }, function(data) {
                        deferred.resolve(data);
                    }, function() {
                        deferred.reject();
                    });

                    return deferred.promise;
                },
                query: function() {
                    var deferred = $q.defer();
                    resource.query({}, function(data) {
                        deferred.resolve(data);
                    }, function() {
                        deferred.reject();
                    });

                    return deferred.promise;
                }
            };
        }
    ]);

})(window, window.angular);