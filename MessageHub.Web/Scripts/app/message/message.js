define(['angular'
		, 'angularResource'
		, '../messageModule/message-controller'
		, '../messageModule/message-service'
		, 'angularRoute']
		, function (angular, angularResource, messageController, messageService, angularRoute) {
		'use strict';

		return angular.module('messageModule'
							, ['ngRoute'
							, 'ngResource'
							, 'messageModule.Controllers'
							, 'messageModule.Services']);
});