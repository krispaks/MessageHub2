define(['angular'
		, 'angularResource'
		, '../categoryModule/category-controller'
		, '../categoryModule/category-service'
		, 'angularRoute']
		, function (angular, angularResource, categoryController, categoryService, angularRoute) {
			'use strict';
			console.log("CATEGORY.JS");

			return angular.module('categoryModule'
								, ['ngRoute'
								, 'ngResource'
								, 'categoryModule.Controllers'
								, 'categoryModule.Services']);
		});