/*
 * @ngdoc Resource
 * @name notelyResources
 * 
 * @description 
 * Define API calls
 * 
 */
angular.module('notely.resources').factory('notelyResources',
    function ($q, $http, umbRequestHelper) {
        return {

            // Get a list of data types without the 'Notely' types
            getDataTypes: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getdatatypes"),
                    'Unable to retreive the datatypes!'
                );
            },

            // Get the data type object
            getDataType: function (id) {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getdatatype", { params: { guid: id } }),
                    'Unable to retreive the datatype object!'
                );
            },

            // Get the active users ( max 50 )
            getUsers: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getusers"),
                    "Unable to retreive the users!"
                );
            },

            // Get the comments of a given content node and property
            getComments: function (property) {

                var config = {
                    params: property,
                    headers: { 'Accept': 'application/json' }
                };

                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getcomments", config),
                    "Unable to retreive the comments!"
                );
            },

            // Get all comments
            getAllComments: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getallcomments"),
                    "Unable to retreive the comments!"
                );
            },

            // Get the comment object
            getComment: function (id) {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getcomment", { params: { id: id } }),
                    "Unable to retreive the comment!"
                );
            },

            // Add a new comment
            addComment: function (comment) {
                return umbRequestHelper.resourcePromise(
                    $http.post("backoffice/notely/notelyapi/addcomment", comment),
                    "Unable to add the comment!"
                );
            },

            // Update an existing comment
            updateComment: function (comment) {
                return umbRequestHelper.resourcePromise(
                    $http.put("backoffice/notely/notelyapi/updatecomment", comment),
                    "Unable to update the comment!"
                );
            },

            // Delete comment
            deleteComment: function (id) {
                return umbRequestHelper.resourcePromise(
                    $http.delete("backoffice/notely/notelyapi/deletecomment", { params: { id: id } }),
                    "Unable to delete the comment!"
                );
            },

            // Set task completed
            taskComplete: function (id) {
                return umbRequestHelper.resourcePromise(
                    $http.post("backoffice/notely/notelyapi/taskcomplete", id),
                    "Unable to set task as completed!"
                );
            }
        };
    }
);