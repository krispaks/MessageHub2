define(['angular'], function(angular) {
    'use strict';

    return angular.module('commentModule.Controllers', ['messageModule.Services', 'commentModule.Services'])
		.controller('CommentListCtrl', ['$scope', '$location', '$log', 'commentService', function ($scope, $location, $log, commentService) {

		    // variables
		    $scope.numShown = 0;

		    // scrolling function
		    $(window).scroll(function () {
		        if ($(window).scrollTop() == $(document).height() - $(window).height()) {

		            console.log("scrooooll. numshown=" + $scope.numShown);
		            $scope.$apply(function () {
		                $scope.numShown += 3;
		            });		            
		        }
		    });

		}]);
});