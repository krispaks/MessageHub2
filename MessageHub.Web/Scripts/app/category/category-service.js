define(['angular'], function (angular) {
	'use strict';

	return angular.module('categoryModule.Services', [])
    .factory('categoryService', ['$resource', function ($resource) {
    	return {
    		GetCateories: function () {
    			
    			//NOTE KPACA 10/20: in $resource all you have to do is pass in a javascript object {}. 
    			//the Web API Model Binder (FromURI) will bind it to the Object parameter in the service.)
    			return $resource('/api/CategoryApi').query();
    		},
    		GetCategory: function (id) {
    			return $resource('/api/CategoryApi', { id: id }).get();
    		},
    		SaveCategory: function (category) {
    			return $resource('/api/CategoryApi').save(category);
    		}
    	};
    }]);
});
