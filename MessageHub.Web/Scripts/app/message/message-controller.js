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
		.controller('MessageCreateCtrl', ['$scope', '$location','$log', 'messageService', function ($scope, $location,$log, messageService) {
			$scope.SaveMessage = function(message) {
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
				    commentService.SaveComment(comment).$promise
                        .then(function(data) {
						    //need to call get message or load the comments
                            //$scope.message = messageService.GetMessage($routeParams.id);
                            $timeout(function () {
                                $scope.$apply(function () {
                                    $scope.message = messageService.GetMessage($routeParams.id)
                                });
                            }, 500);    // dirty fix (very, very dirty)
						}, function (reason) {
							$log.error('Errot at MessageDetailCtrl SaveComment: ' + reason.data.Message + '- Detail: ' + reason.data.MessageDetail);
						}
					);
				};
		}]);
});