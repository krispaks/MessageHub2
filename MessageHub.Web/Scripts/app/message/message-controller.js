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
			$scope.pageInfo.Rows = 5;
			$scope.pageInfo.TotalPages = 0;
			$scope.pageInfo.TotalRecords = 0;


			//dropdownlist
			$scope.categoryddlist = [{ 'id': 1, 'name': 'Category1' }, { 'id': 2, 'name': 'Category2' }];
			$scope.subCategoryddlist = [{ 'id': 1, 'name': 'SubCategory1', 'parentid': 1 }, { 'id': 2, 'name': 'SubCategory2', 'parentid': 1 }, { 'id': 3, 'name': 'SubCategory3', 'parentid': 2 }];
            
			getResultsPage($scope.pageInfo.Page);

			$scope.setPage = function (pageNo) {
			    $scope.pageInfo.Page = pageNo;
			};

			$scope.pageChanged = function () {
			    console.log('Page changed to: ' + $scope.pageInfo.Page);
			    getResultsPage($scope.pageInfo.Page);
			};

			function getResultsPage(pageNumber) {

			    var searchPaging = {};
			    searchPaging.Page = $scope.pageInfo.Page;
			    searchPaging.Rows = $scope.pageInfo.Rows;
			    console.log("page=" + searchPaging.Page + ", rows=" + searchPaging.Rows);
			    messageService.GetPagedMessageList(searchPaging).$promise.then(
					function (data) {
					    $scope.messages = data.data;

					    $scope.pageInfo.Page = data.pageInfo.page;
					    $scope.pageInfo.Rows = data.pageInfo.rows;
					    $scope.pageInfo.TotalPages = data.pageInfo.totalPages;
					    $scope.pageInfo.TotalRecords = data.pageInfo.totalRecords;

					},
					function (reason) {
					    $log.error('Errot at MessageListCtrl GetPagedMessageList: ' + reason.data.Message + '- Detail: ' + reason.data.MessageDetail);
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
                // gets the info for the message and attaches it to the scope's message variable
			    messageService.GetMessage($routeParams.id).$promise.then(function (data) {
			        //console.log(JSON.stringify(data));

                    // when it receives the response, attachs the data to the scope
			        $scope.message = {
			            "id": data.message["id"],
			            "title": data.message["title"],
			            "content": data.message["content"],
			            "createdBy": data.message["createdBy"],
			            "createdDate": data.message["createdDate"],
			            "newComment": {
			                "id": 0,
			                "messageId": data.message["id"],
			                "value": null,
			                "createdBy": null,
			                "createdDate": null
			            }
			        };
			    });

			    // gets the list of comments and attaches it to the scope's comments variable
			    commentService.GetComments($routeParams.id).$promise.then(function (data) {
			        //console.log(JSON.stringify(data));
			        $scope.comments = data;
			    });
                
                // disarm them jsons (actually not used)
			    //function disarm() {
			    //    console.log("disarm!");

			    //    //console.log("comments = " + JSON.stringify($scope.message["commentList"]));
			    //    //console.log("length = " + $scope.message["commentList"].length);
			    //    for (var i = 0; i < $scope.message["commentList"].length; i++) {
			    //        // lets do some changes here...
			    //        /*if (i == 2) {
                //            //$scope.releases[i].name = "newname";
                //            var jsontext = '{"a": 15, "b": 17}';
                //            $scope.tests[i].content = JSON.parse(jsontext);
                //        }*/
			    //        console.log("LEVEL " + i + " - title: " + $scope.message["commentList"][i]["value"]);
			    //    }
			    //}

			    $scope.SaveComment = function (comment) {
			        // the comment is saved onto the db
			        commentService.SaveComment(comment).$promise.then(
						function (data) {
						    // empty the comment box
						    $("#newCommentBox").val('');

						    // when the promise is fulfilled, the comment list gotta be updated
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