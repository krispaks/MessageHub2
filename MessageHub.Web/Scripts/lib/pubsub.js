define([function() {
	'use strict';

	var pubsub = {
		topics: {},
		subscribe: function(topic, listener) {
			if (!this.topics[topic]) this.topics[topic] = [];

			this.topics[topic] = listener;
		},
		publish: function(topic, data) {
			if (!this.topics[topic] || this.topics[topic].length < 1) return;

			this.topics[topic].forEach(function(listener) {
				listener(data || {});
			});
		}
	};

	return pubsub;
}]);
