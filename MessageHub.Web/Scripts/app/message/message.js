define(['angular'
		, 'angularResource'
		, '../messageModule/message-controller'
		, '../messageModule/message-service'
        , '../commentModule/comment-controller'
		, '../commentModule/comment-service'
        , '../directiveResource/directives'
		, 'angularRoute']
		, function (angular, angularResource, messageController, messageService, commentController, commentService, directives, angularRoute) {
		'use strict';

		return angular.module('messageModule'
							, ['ngRoute'
							, 'ngResource'
							, 'messageModule.Controllers'
							, 'messageModule.Services'
                            , 'commentModule.Controllers'
                            , 'docsSimpleDirective'
							, 'commentModule.Services']);
});