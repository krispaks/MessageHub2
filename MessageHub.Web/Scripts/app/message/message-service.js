﻿define(['angular'], function(angular) {
	'use strict';

	return angular.module('messageModule.Services', [])
    .factory('messageService', ['$resource', function ($resource) {
    	return {
    		GetMessages: function(searchCriteria) {
    			if (!searchCriteria) {
    				searchCriteria = {
    					Title: '',
    					Tag: '',
    					Category: 0,
    					SubCategory: 0
    				}
    			}

    			//NOTE KPACA 10/20: in $resource all you have to do is pass in a javascript object {}. 
    			//the Web API Model Binder (FromURI) will bind it to the Object parameter in the service.)
    			//return $resource('/api/MessageApi', {
    			//	Title: searchCriteria.Title,
    			//	Tag: searchCriteria.Tag,
    			//	Category: searchCriteria.Category,
    			//	SubCategory: searchCriteria.SubCategory
    			//}).query();

			    return $resource('/api/MessageApi').query();
    		},
    		GetPagedMessageList: function (searchCriteria) {
    			return $resource('/api/MessageApi', searchCriteria).get();
			},
		    GetThings: function(page) {
    		    return $resource('/api/MessageApi', { page: page }).query();
    		},
    		GetMessage: function(id) {
    			return $resource('/api/MessageApi', { id: id }).get();
    		},
    		SaveMessage: function(message) {
    			return $resource('/api/MessageApi').save(message);
    		}
    	};
    }]);
});
