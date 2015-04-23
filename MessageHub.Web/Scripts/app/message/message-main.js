'use strict';

//NOTE: Kpaca 10/15/2014: window.name = 'NG_DEFER_BOOTSTRAP!' is not present there is possibility it stuff wont work coz they are not yet loaded (i dont really know why)

window.name = 'NG_DEFER_BOOTSTRAP!';



requirejs(['../Scripts/app/require-config'], function (config) {
	requirejs(['angular'
		, 'messageModule/message'
		, 'messageModule/message-route']
		, function (angular, message, messageRoute) {

			angular.element().ready(function () {

			//NOTE: Kpaca 10/15/2014: NG_DEFER_BOOTSTRAP will pause the process of bootrapping angular to the application, it can only be resumed when angular.resumeBootstrap is called.
			angular.resumeBootstrap([message['name']]);
		});
	});
});