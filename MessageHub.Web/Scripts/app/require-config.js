'use strict';

requirejs.config({
	//urlArgs: 'bust=' + (new Date()).getTime(),
	baseUrl: '../Scripts/lib',
	paths: {
		angular: 'angular',
		angularRoute: 'angular-route',
		angularResource: 'angular-resource',
		messageModule: '../app/message',
		commentModule: '../app/comment',
		categoryModule: '../app/category',
		directiveResource: '../app/directives',
		pubsub: 'pubsub',
		uiBootstrap: 'ui-bootstrap',
		bootstrapTagsinput: 'bootstrap-tagsinput',
		moment: '../moment'
	},
	shim: {
		'angular': { 'exports': 'angular' },
		'angularRoute': ['angular'],
		'angularResource': ['angular'],
		'uiBootstrap': ['angular'],
		'bootstrapTagsinput': ['angular']
	}
});