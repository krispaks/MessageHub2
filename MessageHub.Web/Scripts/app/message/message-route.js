define(['angular', 'messageModule/message'], function (angular, message) {
	'use strict';

	return message.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
		$locationProvider.html5Mode(true);
		$routeProvider.when('/Message', {
			templateUrl: '/templates/Message/_List.html',
			controller: 'MessageListCtrl'
		});
		$routeProvider.when('/Message/Create', {
			templateUrl: '/templates/Message/_Create.html',
			controller: 'MessageCreateCtrl'
		});
		$routeProvider.when('/Message/Detail/:id', {
			templateUrl: '/templates/Message/_Detail.html',
			controller: 'MessageDetailCtrl'
		});
		$routeProvider.when('/Category', {
			controller: function () {
				window.location.replace('/Category/Index');
			},
		});
		//$routeProvider.otherwise({
		//	 redirectTo: '/Message'
		//});
	}]);
});