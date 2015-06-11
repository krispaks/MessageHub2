define(['angular'], function (angular) {
	'use strict';

	return angular.module('categoryModule.Controllers', ['categoryModule.Services'])
		.controller('CategoryListCtrl', ['$scope', '$location', '$log', 'categoryService', function ($scope, $location, $log, categoryService) {

		    console.log("CATEGORY-CONTROLLER.JS");

			//page data
		    //$scope.categories = categoryService.GetCategories();

		    // metod de prueba
		    function onlyTesting() {
		        console.log("only testing");
		    }

		    onlyTesting();

		    // parent dropdown
		    $scope.categoryList = [{
		        id: 1,
		        name: 'Category 11'
		    }, {
		        id: 2,
		        name: 'Category 22'
		    }];

		    // child dropdown (depends on the selection on the father)
		    $scope.subcategoryList = [{
		        parent: 1,
		        id: 9,
		        name: 'Category 1 - A'
		    }, {
		        parent: 1,
		        id: 10,
		        name: 'Category 1 - B'
		    }, {
		        parent: 1,
		        id: 11,
		        name: 'Category 1 - C'
		    }, {
		        parent: 2,
		        id: 12,
		        name: 'Category 2 - A'
		    }, {
		        parent: 2,
		        id: 13,
		        name: 'Category 2 - B'
		    }];

		}])

		.controller('CategoryCreateCtrl', ['$scope', '$location', '$log', 'categoryService', function ($scope, $location, $log, categoryService) {
			$scope.SaveCategory = function (category) {
				categoryService.SaveCategory(category).$promise.then(
					function () {
						$location.url('/Category');
					},
					function (reason) {
						$log.error('Errot at CategoryCreateCtrl SaveCategory: ' + reason.data.Message + '- Detail: ' + reason.data.MessageDetail);
						$location.url('/Error');
					});
			};
		}])

		.controller('CategoryDetailCtrl', ['$scope', '$routeParams', '$log', 'categoryService', function ($scope, $routeParams, $log, categoryService) {
			categoryService.GetCategory($routeParams.id).$promise.then(
				function (data) {
					$scope.message = data;
				},
				function (reason) {
					$log.error('Errot at CategoryDetailCtrl GetCategory: ' + reason.data.Message + '- Detail: ' + reason.data.MessageDetail);
					$location.url('/Error');
				});
		}]);
});