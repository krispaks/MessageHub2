﻿define(['angular'], function(angular) {
	'use strict';

	return angular.module('messageModule.Controllers', ['messageModule.Services', 'commentModule.Services', 'categoryModule.Services', 'ui.bootstrap'])
		.controller('MessageListCtrl', ['$scope', '$location', '$log', 'messageService', 'categoryService', function ($scope, $location, $log, messageService, categoryService) {

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

            // gets the messages for the page
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
			    /*>>*/searchPaging.Title = $scope.searchCriteria.Title;
			    /*>>*/searchPaging.SubCategory = $scope.searchSubcategory;
			    /*>>*/searchPaging.Tag = $scope.searchCriteria.Tag;
			    searchPaging.Page = $scope.pageInfo.Page;
			    searchPaging.Rows = $scope.pageInfo.Rows;

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

            // array for the dropdown for categories and subcategories
			$scope.categoryList = [];
			$scope.subcategoryList = [];

		    // gets the list of categories and populates the categories and subcategories scope vars
			categoryService.GetCateories().$promise.then(
					function (data) {
					    $.each(data, function (i, item) {
					        item.parentId == 0 ? $scope.categoryList.push(item) : $scope.subcategoryList.push(item);
					    });
					},
					function (reason) {
					    $log.error('Errot at MessageListCtrl GetPagedMessageList: ' + reason.data.Message + '- Detail: ' + reason.data.MessageDetail);
					});

		    // save a new category
		    /*categoryService.SaveCategory().$promise.then(
					function (data) {
					    console.log("CATEGORY: " + JSON.stringify(data));
					},
					function (reason) {
					    $log.error('Errot at MessageListCtrl SaveCategory: ' + reason.data.Message + '- Detail: ' + reason.data.MessageDetail);
					});*/
	        
			// search functionality
			$scope.SearchMessages = function () {
			    $scope.pageInfo.Page = 1;
				var searchPaging = {};
				searchPaging.Title = $scope.searchCriteria.Title;
				searchPaging.SubCategory = $scope.searchSubcategory;
				searchPaging.Tag = $scope.searchCriteria.Tag;
				searchPaging.Page = $scope.pageInfo.Page;
				searchPaging.Rows = $scope.pageInfo.Rows;

				console.log("   title: " + searchPaging.Title);
				console.log("category: " + searchPaging.SubCategory);

				messageService.GetPagedMessageList(searchPaging).$promise.then(
					function (data) {
					    console.log("response: "+JSON.stringify(data));
					    $scope.messages = data.data;

					    $scope.pageInfo.Page = data.pageInfo.page;
					    $scope.pageInfo.Rows = data.pageInfo.rows;
					    $scope.pageInfo.TotalPages = data.pageInfo.totalPages;
					    $scope.pageInfo.TotalRecords = data.pageInfo.totalRecords;
					},
					function (reason) {
						$log.error('Errot at MessageListCtrl GetPagedMessageList: ' + reason.data.Message + '- Detail: ' + reason.data.MessageDetail);
				});
			};
		}])

        .filter('secondDropdown', function () {
            return function (searchSubcategory, searchCategory) {
                var filtered = [];
                if (searchCategory === null) {
                    return filtered;
                }
                angular.forEach(searchSubcategory, function (item) {
                    if (item.parentId == searchCategory) {
                        filtered.push(item);
                    }
                });
                return filtered;
            };
        })

		.controller('MessageCreateCtrl', ['$scope', '$location', '$log', 'messageService', 'categoryService', function ($scope, $location, $log, messageService, categoryService) {

		    // array for the dropdown for categories and subcategories
		    $scope.categoryList = [];
		    $scope.subcategoryList = [];

		    // gets the list of categories and populates the categories and subcategories scope vars
		    categoryService.GetCateories().$promise.then(
					function (data) {
					    $.each(data, function (i, item) {
					        item.parentId == 0 ? $scope.categoryList.push(item) : $scope.subcategoryList.push(item);
					    });
					},
					function (reason) {
					    $log.error('Errot at MessageListCtrl GetPagedMessageList: ' + reason.data.Message + '- Detail: ' + reason.data.MessageDetail);
					});

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