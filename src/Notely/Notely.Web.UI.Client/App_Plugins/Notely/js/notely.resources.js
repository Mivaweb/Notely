angular.module('notely.resources').factory('Notely.notelyResources',
    function ($q, $http, umbRequestHelper) {
        return {
            getDataTypes: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notely/getdatatypes"),
                    'Unable to retreive the datatypes!'
                );
            },
            getDataType: function (id) {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notely/getdatatype", { params: { guid: id } }),
                    'Unable to retreive the datatype object!'
                );
            },
            getUsers: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notely/getusers"),
                    "Unable to retreive the users!"
                );
            },
            getComments: function (property) {

                var config = {
                    params: property,
                    headers: { 'Accept': 'application/json' }
                };

                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notely/getcomments", config),
                    "Unable to retreive the comments!"
                );
            },
            getComment: function (id) {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notely/getcomment", { params: { id: id } }),
                    "Unable to retreive the comment!"
                );
            },
            addComment: function (comment) {
                return umbRequestHelper.resourcePromise(
                    $http.post("backoffice/notely/notely/addcomment", comment),
                    "Unable to add the comment!"
                );
            },
            updateComment: function (comment) {
                return umbRequestHelper.resourcePromise(
                    $http.put("backoffice/notely/notely/updatecomment", comment),
                    "Unable to update the comment!"
                );
            },
            deleteComment: function (id) {
                return umbRequestHelper.resourcePromise(
                    $http.delete("backoffice/notely/notely/deletecomment", { params: { id: id } }),
                    "Unable to delete the comment!"
                );
            },
            taskComplete: function (id) {
                return umbRequestHelper.resourcePromise(
                    $http.post("backoffice/notely/notely/taskcomplete", id),
                    "Unable to set task as completed!"
                );
            }
        };
    });