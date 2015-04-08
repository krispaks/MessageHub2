define(['angular'], function (angular) {
	'use strict';

	return angular.module('categoryModule.Controllers', ['categoryModule.Services'])
		.controller('CategoryListCtrl', ['$scope', '$location', '$log', 'categoryService', function ($scope, $location, $log, categoryService) {

			//page data
			//$scope.categories = categoryService.GetCategories();
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