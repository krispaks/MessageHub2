define(['angular'], function(angular) {
	'use strict';

	return angular.module('messageModule.Controllers', ['messageModule.Services', 'commentModule.Services'])
		.controller('MessageListCtrl', ['$scope', '$location', '$log', 'messageService', function ($scope, $location, $log, messageService) {

			//dropdownlist
			$scope.categoryddlist = [{ 'id': 1, 'name': 'Category1' }, { 'id': 2, 'name': 'Category2' }];
			$scope.subCategoryddlist = [{ 'id': 1, 'name': 'SubCategory1', 'parentid': 1 }, { 'id': 2, 'name': 'SubCategory2', 'parentid': 1 }, { 'id': 3, 'name': 'SubCategory3', 'parentid': 2 }];

			//page data
			$scope.messages = messageService.GetMessages(null);
	        
			//functions
			$scope.SearchMessages = function(searchCriteria) {
				messageService.GetMessages(searchCriteria).$promise.then(
					function(data) {
						$scope.messages = data;
					},
					function (reason) {
						$log.error('Errot at MessageListCtrl GetMessages: ' + reason.data.Message + '- Detail: ' + reason.data.MessageDetail);
						$location.url('/Error');
					});
			};
		}])
		.controller('MessageCreateCtrl', ['$scope', '$location', '$log', 'messageService', function ($scope, $location, $log, messageService) {

		    //dropdownlist
		    $scope.categoryddlist = [
                { 'id': 1, 'name': 'Category1' },
                { 'id': 2, 'name': 'Category2' }
		    ];
		    $scope.myCategory = $scope.categoryddlist[0];

		    $scope.subCategoryddlist = [{ 'id': 1, 'name': 'SubCategory1', 'parentid': 1 }, { 'id': 2, 'name': 'SubCategory2', 'parentid': 1 }, { 'id': 3, 'name': 'SubCategory3', 'parentid': 2 }];
		    $scope.mySubCategory = $scope.subCategoryddlist[0];

		    $scope.SaveMessage = function (message) {
				messageService.SaveMessage(message).$promise.then(
					function () {
						$location.url('/Message');
					},
					function (reason) {
						$log.error('Errot at MessageCreateCtrl SaveMessage: ' + reason.data.Message + '- Detail: ' + reason.data.MessageDetail);
						$location.url('/Error');
				});
			};
		}])
		.controller('MessageDetailCtrl'
			, ['$scope'
            , '$timeout'
			, '$location'
			, '$routeParams'
			, '$log'
			, 'messageService'
			, 'commentService'
			, function ($scope, $timeout, $location, $routeParams, $log, messageService, commentService) {

				$scope.message = messageService.GetMessage($routeParams.id);
            
				$scope.SaveComment = function (comment) {
				    /*var res = commentService.SaveComment(comment).$promise
                        .then(function (data) {
                            console.log("res 1 = "+res);
						    //need to call get message or load the comments
                            //$scope.message = messageService.GetMessage($routeParams.id);
                            $timeout(function () {
                                console.log("res 2 = " + res);
                                $scope.$apply(function () {
                                    console.log("res 3 = " + res);
                                    $scope.message = messageService.GetMessage($routeParams.id)
                                });
                            }, 500);    // dirty fix (very, very dirty)
						}, function (reason) {
							$log.error('Errot at MessageDetailCtrl SaveComment: ' + reason.data.Message + '- Detail: ' + reason.data.MessageDetail);
						}
					);*/

				    var p1 = new Promise(
                        function (resolve, reject) {
                            console.log("promise started");
                            
                            // comment saving
                            var res;
                            console.log("res (pre) = " + res);
                            res = commentService.SaveComment(comment);
                            console.log("res (mid) = " + res);

                            // timeout for async resolution of promise
                            $timeout(
                                function () {
                                    // fulfill the promise
                                    resolve(res);
                                    console.log("res (post) = " + res);
                                }
                            , 500);
                        });

                    // We define what to do when the promise is fulfilled
                    p1.then(
                    // Just log the message and a value
                        function (val) {
                            console.log("promise fulfilled");
                            $scope.message = messageService.GetMessage($routeParams.id);
                    });
				};
		}]);
});