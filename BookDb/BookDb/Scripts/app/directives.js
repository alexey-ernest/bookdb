(function (window, angular) {
    'use strict';

    angular.module('directives', [])
        .directive('loader', [
            function () {

                return {
                    restrict: 'EA',
                    scope: {
                        trigger: '=loaderIf'
                    },
                    template: '<div class="wrapper-loading" ng-show="trigger">Loading...</div></div>'
                };
            }
        ])
        .directive('onConfirmClick', function () {
            return {
                restrict: 'A',
                scope: {
                    message: '@confirmMessage',
                    onConfirm: '&confirmClick'
                },
                link: function (scope, element) {
                    element.bind('click', function () {
                        var message = scope.message || "Are you sure?";
                        if (window.confirm(message)) {
                            var action = scope.onConfirm;
                            if (action != null)
                                scope.$apply(action());
                        }
                    });
                }
            };
        })
        .directive('validateIsbn', function () {

            // from https://www.safaribooksonline.com/library/view/regular-expressions-cookbook/9781449327453/ch04s13.html

            var regexp = /^(?:ISBN(?:-1[03])?:? )?(?=[0-9X]{10}$|(?=(?:[0-9]+[- ]){3})[- 0-9X]{13}$|97[89][0-9]{10}$|(?=(?:[0-9]+[- ]){4})[- 0-9]{17}$)(?:97[89][- ]?)?[0-9]{1,5}[- ]?[0-9]+[- ]?[0-9]+[- ]?[0-9X]$/;

            return {
                restrict: 'A',
                require: 'ngModel',
                link: function (scope, elm, attrs, ctrl) {
                    ctrl.$parsers.unshift(function (viewValue) {
                        if (regexp.test(viewValue)) {

                            // Remove non ISBN digits, then split into an array
                            var chars = viewValue.replace(/[- ]|^ISBN(?:-1[03])?:?/g, "").split("");

                            // Remove the final ISBN digit from `chars`, and assign it to `last`
                            var last = parseInt(chars.pop(), 10);
                            var sum = 0;
                            var check, i;

                            if (chars.length === 9) {
                                // Compute the ISBN-10 check digit
                                chars.reverse();
                                for (i = 0; i < chars.length; i++) {
                                    sum += (i + 2) * parseInt(chars[i], 10);
                                }
                                check = 11 - (sum % 11);
                                if (check === 10) {
                                    check = "X";
                                } else if (check === 11) {
                                    check = "0";
                                }
                            } else {
                                // Compute the ISBN-13 check digit
                                for (i = 0; i < chars.length; i++) {
                                    sum += (i % 2 * 2 + 1) * parseInt(chars[i], 10);
                                }
                                check = 10 - (sum % 10);
                                if (check === 10) {
                                    check = "0";
                                }
                            }

                            if (check === last) {
                                ctrl.$setValidity('isbn', true);
                                return viewValue;
                            } else {
                                ctrl.$setValidity('isbn', false);
                                return undefined;
                            }
                        } else {
                            // it is invalid, return undefined (no model update)
                            ctrl.$setValidity('isbn', false);
                            return undefined;
                        }
                    });
                }
            };
        })
    ;
})(window, window.angular);