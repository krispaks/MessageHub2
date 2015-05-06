define(['angular'], function(angular) {
	'use strict';

	return angular.module('commentModule.Services', [])
	.factory('commentService', ['$resource', function($resource) {
		return {
			GetComments: function(searchCriteria) {
				if (!searchCriteria) {
					searchCriteria = {
						MessageId: 0
					}
				}

				return $resource('/api/CommentApi', {
					MessageId: searchCriteria.MessageId
				}).query();
			},
			SaveComment: function(comment) {
				return $resource('api/CommentApi').save(comment);
			}
		};
	}]);
});