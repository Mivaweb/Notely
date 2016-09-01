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
            getDataTypes: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getdatatypes"),
                    'Unable to retreive the datatypes!'
                );
            },
            getDataType: function (id) {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getdatatype", { params: { guid: id } }),
                    'Unable to retreive the datatype object!'
                );
            },
            getUsers: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getusers"),
                    "Unable to retreive the users!"
                );
            },
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
            getComment: function (id) {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getcomment", { params: { id: id } }),
                    "Unable to retreive the comment!"
                );
            },
            addComment: function (comment) {
                return umbRequestHelper.resourcePromise(
                    $http.post("backoffice/notely/notelyapi/addcomment", comment),
                    "Unable to add the comment!"
                );
            },
            updateComment: function (comment) {
                return umbRequestHelper.resourcePromise(
                    $http.put("backoffice/notely/notelyapi/updatecomment", comment),
                    "Unable to update the comment!"
                );
            },
            deleteComment: function (id) {
                return umbRequestHelper.resourcePromise(
                    $http.delete("backoffice/notely/notelyapi/deletecomment", { params: { id: id } }),
                    "Unable to delete the comment!"
                );
            },
            taskComplete: function (id) {
                return umbRequestHelper.resourcePromise(
                    $http.post("backoffice/notely/notelyapi/taskcomplete", id),
                    "Unable to set task as completed!"
                );
            }
        };
    }
);