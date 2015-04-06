'use strict';

requirejs.config({
	urlArgs: 'bust=' + (new Date()).getTime(),
	baseUrl: '../Scripts/lib',
	paths: {
		angular: 'angular',
		angularRoute: 'angular-route',
		angularResource: 'angular-resource',
		messageModule: '../app/message'
	},
	shim: {
		'angular': { 'exports': 'angular' },
		'angularRoute': ['angular'],
		'angularResource': ['angular']
	}
});