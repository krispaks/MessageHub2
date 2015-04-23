define(['angular', 'categoryModule/category'], function (angular, category) {
	'use strict';

	return category.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
		$locationProvider.html5Mode(true);
		$routeProvider.when('/Category', {
			templateUrl: '/templates/Category/_List.html',
			controller: 'CategoryListCtrl'
		});
		$routeProvider.when('/Category/Create', {
			templateUrl: '/templates/Category/_Create.html',
			controller: 'CategoryCreateCtrl'
		});
		$routeProvider.when('/Category/Detail/:id', {
			templateUrl: '/templates/Category/_Detail.html',
			controller: 'CategoryDetailCtrl'
		});
		$routeProvider.otherwise({ redirectTo: '/Category' });
	}]);
});