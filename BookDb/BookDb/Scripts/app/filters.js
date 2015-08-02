(function (window, angular) {
    'use strict';

    angular.module('filters', [])
        .filter('apiDate', [
                'moment', function(moment) {

                    return function(text, format) {
                        var date = moment(text);
                        return date.format(format);
                    };

                }
            ]
        );

})(window, window.angular);