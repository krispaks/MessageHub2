define(['angular'], function(angular) {
	'use strict';

	return angular.module('messageModule.Controllers', ['messageModule.Services', 'commentModule.Services', 'ui.bootstrap'])
		.controller('MessageListCtrl', ['$scope', '$location', '$log', 'messageService', function ($scope, $location, $log, messageService) {

			$scope.searchCriteria = {};
			$scope.searchCriteria.Title = '';
			$scope.searchCriteria.Category = 0;
			$scope.searchCriteria.SubCategory = 0;
			$scope.searchCriteria.Tag = '';
			$scope.pageInfo = {};
			$scope.pageInfo.Page = 1;
			$scope.pageInfo.Rows = 10;
			$scope.pageInfo.TotalPages = 0;
			$scope.pageInfo.TotalRecords = 0;


			//dropdownlist
			$scope.categoryddlist = [{ 'id': 1, 'name': 'Category1' }, { 'id': 2, 'name': 'Category2' }];
			$scope.subCategoryddlist = [{ 'id': 1, 'name': 'SubCategory1', 'parentid': 1 }, { 'id': 2, 'name': 'SubCategory2', 'parentid': 1 }, { 'id': 3, 'name': 'SubCategory3', 'parentid': 2 }];

		    //page data & pagination
		    $scope.totalItems = 15;
		    $scope.itemsPerPage = 5;
		    $scope.currentPage = 1;
		    $scope.maxSize = 5;

		    getResultsPage($scope.currentPage);

		    $scope.setPage = function (pageNo) {
		        $scope.currentPage = pageNo;
		    };

		    $scope.pageChanged = function () {
		        console.log('Page changed to: ' + $scope.currentPage);
		        getResultsPage($scope.currentPage);
		    };

		    function getResultsPage(pageNumber) {
		        messageService.GetThings(pageNumber).$promise.then(function (data) {
		            $scope.messages = data;
		        });
		    }
	        
			//functions
			$scope.SearchMessages = function () {
				var searchPaging = {};
				searchPaging.Title = $scope.searchCriteria.Title;
				searchPaging.SubCategory = $scope.searchCriteria.SubCategory;
				searchPaging.Tag = $scope.searchCriteria.Tag;
				searchPaging.Page = $scope.pageInfo.Page;
				searchPaging.Rows = $scope.pageInfo.Rows;

				messageService.GetPagedMessageList(searchPaging).$promise.then(
					function (data) {
						alert('test');
						$scope.messages = data.data;
					},
					function (reason) {
						$log.error('Errot at MessageListCtrl GetPagedMessageList: ' + reason.data.Message + '- Detail: ' + reason.data.MessageDetail);
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
		        console.log("message = " + message);
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
			    //messageService.GetMessage($routeParams.id).$promise.then(function (data) {
			    //    console.log(">>> MESSAGE <<<");
			    //    console.log(JSON.stringify(data));

                //    // when it receives the response, attachs the data to the scope

			    //    $scope.message = {
			    //        "id": data.message["id"],
			    //        "title": data.message["title"],
			    //        "content": data.message["content"],
			    //        "createdBy": data.message["createdBy"],
			    //        "createdDate": data.message["createdDate"],
			    //        "newComment": {
			    //            "id": 0,
			    //            "messageId": data.message["id"],
			    //            "value": null,
			    //            "createdBy": null,
			    //            "createdDate": null
			    //        }
			    //    };

                //    // now there's a different scope for the content of the message and the comments
			    //    //$scope.comments = data["commentList"];

			    //    //console.log("JSON 2 = " + JSON.stringify($scope.message));
			    //});

			    commentService.GetComments($routeParams.id).$promise.then(function (data) {
			        console.log(">>> COMMENTS <<<");
			        console.log(JSON.stringify(data));
			        $scope.comments = data;
			    });
                
                // disarm them jsons (actually not used)
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

			    $scope.SaveComment = function (comment) {
			        // the comment is saved onto the db
			        commentService.SaveComment(comment).$promise.then(
						function (data) {
						    // empty the comment box
						    $("#newCommentBox").val('');

						    // when the promise is fulfilled, the comment list gotta be updated
						    /*messageService.GetMessage($routeParams.id).$promise.then(function (data) {
						        // the comment list is updated with the new comments
						        $scope.comments = data["commentList"];
						    });*/
						    //$scope.$apply();

						    commentService.GetComments($routeParams.id).$promise.then(function (data) {
						        $scope.comments = data;
						    });
						},
						function (reason) {
						    $log.error('Errot at MessageDetailCtrl SaveComment: ' + reason.data.Message + '- Detail: ' + reason.data.MessageDetail);
						}
					);
			    };
			}])

            .controller('PaginationDemoCtrl', function ($scope, $log) {
                $scope.totalItems = 64;
                $scope.currentPage = 4;

                $scope.setPage = function (pageNo) {
                    $scope.currentPage = pageNo;
                };

                $scope.pageChanged = function () {
                    $log.log('Page changed to: ' + $scope.currentPage);
                };

                $scope.maxSize = 5;
                $scope.bigTotalItems = 175;
                $scope.bigCurrentPage = 1;
            })
});