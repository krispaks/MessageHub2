define(['angular'], function(angular) {
	'use strict';

	return angular.module('messageModule.Services', [])
    .factory('messageService', ['$resource', function ($resource) {
        return {

    	    GetMessages: function (searchCriteria) {
    	        console.log("mess");
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

    		GetThings: function (page) {
    		    return $resource('/api/MessageApi', { page: page }).get();
    		},

    		SaveCategory: function (category) {
    		    console.log("ms: category");
    		    return $resource('/api/CategoryApi').save(category);
    		},

    		GetMessage: function(id) {
    			return $resource('/api/MessageApi', { id: id }).get();
    		},

    		GetFile: function (fileId, download) {
    		    return $resource('/api/MessageApi', { fileId: fileId, download: download }).get();
    		},

    		SaveMessage: function (message, encoded64, filename) {

    		    var deferred = $.Deferred();

    		    var result;
    		    var data = new FormData();

    		    data.append("newMessage", JSON.stringify(message));

    		    if (encoded64 != null) {
    		        var blob = new Blob([encoded64], { type: 'text/plain' });
    		        data.append("UploadedFile", blob);
    		        data.append("FileName", filename);
    		    }

    		    var ajaxRequest = $.ajax({
    		        type: "POST",
    		        url: "/api/MessageApi/Post",
    		        contentType: false,
    		        processData: false,
    		        data: data,
    		        success: function (response) {
    		            deferred.resolve();
    		        }
    		    });

    		    //return $resource('/api/MessageApi').save(message);
    		    return deferred.promise();
    		}
    	};
    }]);
});
