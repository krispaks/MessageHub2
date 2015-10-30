define(['angular'], function(angular) {
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

		    // connection with the hub for allowing real-time notifications
		    var notifications = $.connection.notificationHub;
		    $.connection.hub.start().done(function () {
		        console.log("connection with the hub up and running");
		    });

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

		        // check for the tags
		        if (message.Tags == null)
		            message.Tags = "null";

		        console.log("message = " + message);

		        // file upload
		        var fileId = document.getElementById('fileupload');
		        var files = fileId.files;
		        var encoded64 = "";
		        if ($('#fileupload')[0].files.length == 1) {
		            console.log("Fichero '" + files[0].name + "' listo para subida");

		            var file = files[0];
		            var reader = new FileReader();
		            reader.onloadend = function () {
		                var encoded64 = reader.result;
		                console.log("B64 = " + encoded64);

		                messageService.SaveMessage(message, encoded64, files[0].name).then(
		                function () {
		                    // updates the notification list for all users
		                    notifications.server.updateWithNewNotification();

		                    $location.url('/Message');
		                    $scope.$apply();
		                },
		                function (reason) {
		                    $log.error('Errot at MessageCreateCtrl SaveMessage: ' + reason.data.Message + '- Detail: ' + reason.data.MessageDetail);
		                    $location.url('/Error');
		                });
		            }

		            if (file) {
		                reader.readAsDataURL(file);
		            } else {
		                console.log("caca");
		            }

		        } else {
		            console.log("No files are gonna be uploaded");

		            messageService.SaveMessage(message, null, null).then(
                    function () {
                        // updates the notification list for all users
                        notifications.server.updateWithNewNotification();

                        $location.url('/Message');
                        $scope.$apply();
                    },
                    function (reason) {
                        $log.error('Errot at MessageCreateCtrl SaveMessage: ' + reason.data.Message + '- Detail: ' + reason.data.MessageDetail);
                        $location.url('/Error');
                    });
		        }
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
                        "tags": data.message["tags"],
			            "newComment": {
			                "id": 0,
			                "messageId": data.message["id"],
			                "value": null,
			                "createdBy": null,
			                "createdDate": null
			            }
			        };

			        if ($scope.message.tags == "null")
			            $scope.message.tags = null;

			        if ($scope.message.tags != null)
			            $scope.message.tags = $scope.message.tags.split(',');
			    });

			    // checks if there's a file uploaded for the specified message
			    messageService.GetFile("" + $routeParams.id, false).$promise.then(
						function (data) {
                            // fills the content of the button with the name of the file
						    document.getElementById('filedownload').value = data['value'];
						    document.getElementById('filedownload').style.display = 'block';
						    document.getElementById('filedownload-text').innerHTML = "Attached (click to download)";
						    //document.getElementById('filedownload-text').style.color = "#ff0000";
						},
						function (reason) {
						    document.getElementById('filedownload-text').innerHTML = "No files attached";
						    console.log("  error: " + reason);
						}
					);

			    // gets the list of comments and attaches it to the scope's comments variable
			    commentService.GetComments($routeParams.id).$promise.then(function (data) {
			        $scope.comments = data;
			    });

			    // connection with the hub for allowing real-time notifications
			    var notifications = $.connection.notificationHub;
			    $.connection.hub.start().done(function () {
			        console.log("connection with the hub up and running");
			    });

			    $scope.SaveComment = function (comment) {
			        comment.createdBy = $('#username').val();
			        // the comment is saved onto the db
			        commentService.SaveComment(comment).$promise.then(
						function (data) {
						    // updates the notification list for all users
						    notifications.server.updateWithNewNotification();

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

			    // metodo GET que recibe los registros de la BBDD
			    $scope.GetFile = function () {

			        messageService.GetFile("" + $routeParams.id, true).$promise.then(
						function (data) {
                            // decode the file encoded @ base64
						    var encoded64 = objToString(data);
						    console.log("B64 = " + encoded64);
						    var blob = dataURLtoBlob(encoded64);

                            // generate the download for the file
						    var a = document.createElement("a");
						    document.body.appendChild(a);
						    var url = window.URL.createObjectURL(blob);
						    a.href = url;
						    a.download = document.getElementById('filedownload').value;
						    a.click();
						    window.URL.revokeObjectURL(url);
						},
						function (reason) {
						    console.log("  error: "+reason);
						}
					);
			    }

                // convert an object to a string
			    function objToString(obj) {
			        var str = '';
			        for (var p in obj) {
			            if (obj.hasOwnProperty(p)) {
			                if(!isNaN(p))
			                    str += obj[p];
			            }
			        }
			        return str;
			    }

                // convert an encoded data url to a blob
			    function dataURLtoBlob(dataurl) {
			        var arr = dataurl.split(','), mime = arr[0].match(/:(.*?);/)[1],
                        bstr = atob(arr[1]), n = bstr.length, u8arr = new Uint8Array(n);
			        while (n--) {
			            u8arr[n] = bstr.charCodeAt(n);
			        }
			        return new Blob([u8arr], { type: mime });
			    }
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