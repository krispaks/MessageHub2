'use strict';

requirejs.config({
	//urlArgs: 'bust=' + (new Date()).getTime(),
	baseUrl: '../Scripts/lib',
	paths: {
		angular: 'angular',
		angularRoute: 'angular-route',
		angularResource: 'angular-resource',
		messageModule: '../app/message',
		categoryModule: '../app/category',
		commentModule: '../app/comment',
		pubsub: 'pubsub'
	},
	shim: {
		'angular': { 'exports': 'angular' },
		'angularRoute': ['angular'],
		'angularResource': ['angular']
	}
});