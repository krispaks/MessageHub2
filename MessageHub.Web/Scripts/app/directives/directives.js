define(['angular'], function(angular) {
    'use strict';

    return angular.module('docsSimpleDirective', [])
    .directive('onScrollShow', function () {

        // function to be returned by the directive
        function link(scope, element, attrs) {
            element.scroll(function () {
                if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
                    // access the general $scope through parent
                    scope.$parent.$apply(function () {
                        // modify the number of elements to be shown
                        scope.$parent.numShown += 3;
                    });
                }                
            });
        }

        return {
            link: link
        };
    });
});