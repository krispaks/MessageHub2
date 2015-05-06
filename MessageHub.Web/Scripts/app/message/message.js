define(['angular'
		, 'angularResource'
		, '../messageModule/message-controller'
		, '../messageModule/message-service'
		, '../commentModule/comment-service'
		, 'angularRoute']
		, function (angular, angularResource, messageController, messageService, commentService, angularRoute) {
		'use strict';

		return angular.module('messageModule'
							, ['ngRoute'
							, 'ngResource'
							, 'messageModule.Controllers'
							, 'messageModule.Services'
							, 'commentModule.Services']);
});