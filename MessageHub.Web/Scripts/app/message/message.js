define(['angular'
		, 'angularResource'
		, '../messageModule/message-controller'
		, '../messageModule/message-service'
        , '../commentModule/comment-controller'
		, '../commentModule/comment-service'
        , '../directiveResource/directives'
		, '../categoryModule/category-service'
        , 'uiBootstrap'
		, 'angularRoute']
		, function (angular
            , angularResource
            , messageController
            , messageService
            , commentController
            , commentService
            , directives
            , categoryService
            , uiBootstrap
            , angularRoute) {
		'use strict';

		return angular.module('messageModule'
							, ['ngRoute'
							, 'ngResource'
							, 'messageModule.Controllers'
							, 'messageModule.Services'
                            , 'commentModule.Controllers'
                            , 'commentModule.Services'
                            , 'docsSimpleDirective'
							, 'categoryModule.Services']);
});