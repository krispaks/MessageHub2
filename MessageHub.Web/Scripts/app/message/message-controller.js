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
		        console.log("message = "+message);
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

				    /*var res;

				    //console.log('res definied');

				    $scope.$watch(res, function () {
				        console.log('hey, res has changed!');
				        //$scope.message = messageService.GetMessage($routeParams.id);
				        $timeout(function () {
				            $scope.message = messageService.GetMessage($routeParams.id);
				            $scope.$apply();
				        }, 0);
				    });

				    //console.log('res about to change');

				    res = commentService.SaveComment(comment);*/

				    var res = commentService.SaveComment(comment);

				    console.log('1');

				    $scope.$watch($scope.message, function () {
				        console.log('hey, $scope.message has changed!');
				        $timeout(function () {
				            $scope.$apply();
				        }, 0);
				    });

				    console.log('2');

				    $scope.message = messageService.GetMessage($routeParams.id);

				    console.log('3');

				    //console.log('res just changed');


                    // promise for when the comment is saved
				    /*var p1 = new Promise(
                        function (resolve, reject) {                            
                            // comment saving
                            var res;

                            $scope.$watch(res, function () {
                                console.log('hey, res has changed!');
                            });

                            res = commentService.SaveComment(comment);

                            // fulfill the promise
                            resolve(res);
                            // update the scope with the new message
                            //$scope.message = messageService.GetMessage($routeParams.id);
                        });

                    // when the promise is fulfilled:
                    p1.then(
                        function (val) {*/
                            // timeout for async refresh of the scope
                            /*$timeout(function () {
                                $scope.message = messageService.GetMessage($routeParams.id);
                                $scope.$apply();
                            }, 0);*/
                            /*$scope.message = messageService.GetMessage($routeParams.id);
                            $scope.$apply();
                    });*/
				};
		}]);
});