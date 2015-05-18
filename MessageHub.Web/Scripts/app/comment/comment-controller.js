define(['angular'], function(angular) {
    'use strict';

    return angular.module('commentModule.Controllers', ['messageModule.Services', 'commentModule.Services'])
		.controller('CommentListCtrl', ['$scope', '$location', '$log', 'commentService', function ($scope, $location, $log, commentService) {

		    // variables
		    $scope.numShown = 0;

		    // scrolling function
		    $(".list-group-item").scroll(function () {
		        if ($(this).scrollTop() == $(this).innerHeight() - $(this).height()) {

		            console.log("scrooooll. numshown=" + $scope.numShown);
		            $scope.$apply(function () {
		                $scope.numShown += 3;
		            });		            
		        }
		    });

		    document.getElementById('auto_stretch_div').setAttribute("style", "height: 350px; overflow: auto;");

		    $(".list-group-item").scroll(function () {
		        if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
		            console.log("div scroll. numshown=" + $scope.numShown);
		            $scope.$apply(function () {
		                $scope.numShown += 3;
		            });
		        }
		    });

		}]);
});