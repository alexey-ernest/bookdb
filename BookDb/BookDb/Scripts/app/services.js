(function (window, angular) {
    'use strict';

    var module = angular.module('services', ['ngResource']);

    module.factory('$exceptionHandler', [
        function () {
            return function (exception) {

                var message = exception.message;
                if (!message || message === '[object Object]') {
                    message = 'Unknown Error';
                }

                alert(message);
            };
        }
    ]);

    module.factory('bookService', [
        '$resource', '$q', function ($resource, $q) {

            var url = '/api/books';
            var resource = $resource(url + '/:id',
            { id: '@id' },
            {
                update: { method: 'PUT' }
            });


            return {
                create: function (options) {
                    return new resource(options);
                },
                get: function (id) {
                    var deferred = $q.defer();

                    resource.get({ id: id }, function (data) {
                        deferred.resolve(data);
                    }, function () {
                        deferred.reject();
                    });

                    return deferred.promise;
                },
                query: function(filter) {

                    filter = filter || {};
                    var params = {};

                    if (filter.byPublishedDate) {
                        params.id = 'published';
                    }

                    var deferred = $q.defer();
                    resource.query(params, function (data) {
                        deferred.resolve(data);
                    }, function () {
                        deferred.reject();
                    });

                    return deferred.promise;
                }
            };
        }
    ]);

    module.factory('authorService', [
        '$resource', '$q', function ($resource, $q) {

            var url = '/api/authors';
            var resource = $resource(url + '/:id',
            { id: '@id' },
            {
                update: { method: 'PUT' }
            });


            return {
                create: function (options) {
                    return new resource(options);
                },
                get: function (id) {
                    var deferred = $q.defer();

                    resource.get({ id: id }, function (data) {
                        deferred.resolve(data);
                    }, function () {
                        deferred.reject();
                    });

                    return deferred.promise;
                },
                query: function () {
                    var deferred = $q.defer();
                    resource.query({}, function (data) {
                        deferred.resolve(data);
                    }, function () {
                        deferred.reject();
                    });

                    return deferred.promise;
                }
            };
        }
    ]);

})(window, window.angular);
