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

			    $scope.numShown = 0;
			    messageService.GetMessage($routeParams.id).$promise.then(function (data) {
			        //console.log(JSON.stringify(data));

                    // when it receives the response, attachs the data to the scope

			        $scope.message = {
			            "id": data["id"],
			            "title": data["title"],
			            "content": data["content"],
			            "createdBy": data["createdBy"],
			            "createdDate": data["createdDate"]
			        };

                    // now there's a different scope for the content of the message and the comments
			        $scope.comments = data["commentList"];

			        //console.log("JSON 2 = " + JSON.stringify($scope.message));
			    });
                
			    function disarm() {
			        console.log("disarm!");

			        //console.log("comments = " + JSON.stringify($scope.message["commentList"]));
			        //console.log("length = " + $scope.message["commentList"].length);
			        for (var i = 0; i < $scope.message["commentList"].length; i++) {
			            // lets do some changes here...
			            /*if (i == 2) {
                            //$scope.releases[i].name = "newname";
                            var jsontext = '{"a": 15, "b": 17}';
                            $scope.tests[i].content = JSON.parse(jsontext);
                        }*/
			            console.log("LEVEL " + i + " - title: " + $scope.message["commentList"][i]["value"]);
			        }
			    }                

			    // ----- START TEST ---------------------------------------------------------------------------------------------------------------------------
			    $scope.tests = [
                    { name: "Development", active: false, content: {a: 0} },
                    { name: "Production", active: false, content: {a: 3, b: 4} },
                    { name: "Staging", active: false, content: { a: 1, b: 2} }
			    ];

			    $scope.pru = 
                    { name: "Development", active: false, content: { a: 0 } }
			    ;

			    console.dir($scope.tests[0]);
			    console.dir($scope.pru["name"]);

                // disarm them jsons
			    for (var i = 0; i < $scope.tests.length; i++) {
			        // lets do some changes here...
			        if (i == 2) {
			            //$scope.releases[i].name = "newname";
			            var jsontext = '{"a": 15, "b": 17}';
			            $scope.tests[i].content = JSON.parse(jsontext);
			        }

                    // and print the content
			        /*console.log("level 1 name: " + $scope.tests[i].name);
			        console.log($scope.tests[i].content);*/
			    }

			    //console.log($scope.tests);
			    // ----- END TEST -----------------------------------------------------------------------------------------------------------------------------

			    $scope.SaveComment = function (comment) {
			        // the comment is saved onto the db
			        commentService.SaveComment(comment).$promise.then(
						function (data) {
						    // when the promise is fulfilled, the message content gotta be updated
						    $scope.message = messageService.GetMessage($routeParams.id);
						    //$scope.$apply();
						},
						function (reason) {
						    $log.error('Errot at MessageDetailCtrl SaveComment: ' + reason.data.Message + '- Detail: ' + reason.data.MessageDetail);
						}
					);
			    };
			}]);
});