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

            // Get the note types
            getNoteTypes: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getnotetypes"),
                    'Unable to retreive the note types!'
                );
            },

            // Get the note states
            getNoteStates: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getnotestates"),
                    'Unable to retreive the note states!'
                );
            },

            // Get the notes of a given content node and property
            getNotes: function (property) {

                var config = {
                    params: property,
                    headers: { 'Accept': 'application/json' }
                };

                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getnotes", config),
                    "Unable to retreive the notes!"
                );
            },

            // Get all notes
            getAllNotes: function () {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getallnotes"),
                    "Unable to retreive the notes!"
                );
            },

            // Get my tasks
            getMyTasks: function (user) {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getmytasks", { params: { userId: user.id } }),
                    "Unable to retreive the tasks of the loggedin user!"
                );
            },

            // Get the note object
            getNote: function (id) {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getnote", { params: { id: id } }),
                    "Unable to retreive the note!"
                );
            },

            // Add a new note
            addNote: function (note) {
                return umbRequestHelper.resourcePromise(
                    $http.post("backoffice/notely/notelyapi/addnote", note),
                    "Unable to add the note!"
                );
            },

            // Update an existing note
            updateNote: function (note) {
                return umbRequestHelper.resourcePromise(
                    $http.put("backoffice/notely/notelyapi/updatenote", note),
                    "Unable to update the note!"
                );
            },

            // Delete a note
            deleteNote: function (id) {
                return umbRequestHelper.resourcePromise(
                    $http.delete("backoffice/notely/notelyapi/deletenote", { params: { id: id } }),
                    "Unable to delete the note!"
                );
            },

            // Cleanup notes
            cleanupNotes: function () {
                return umbRequestHelper.resourcePromise(
                    $http.delete("backoffice/notely/notelyapi/cleanupnotes"),
                    "Unable to cleanup the notes!"
                );
            },

            // Get unique content node id's with notes
            getUniqueContentNodes: function (userId, type, state) {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getuniquecontentnodes", { params: { userId: userId, type: type, state: state } }),
                    "Unable to get the unique content nodes!"
                );
            },

            // Get details of a content node: backoffice
            getContentNodeDetails: function (contentId, userId) {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getbackofficenodedetails", { params: { contentId: contentId, userId: userId } }),
                    "Unable to get the unique content nodes!"
                );
            },

            // Get the note type object
            getNoteType: function (id) {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getnotetype", { params: { id: id } }),
                    "Unable to retreive the note type!"
                );
            },

            // Add a new note type
            addNoteType: function(noteType) {
                return umbRequestHelper.resourcePromise(
                    $http.post("backoffice/notely/notelyapi/addnotetype", noteType),
                    "Unable to add the note type!"
                );
            },

            // Update an existing note type
            updateNoteType: function (noteType) {
                return umbRequestHelper.resourcePromise(
                    $http.put("backoffice/notely/notelyapi/updatenotetype", noteType),
                    "Unable to update the note type!"
                );
            },

            // Delete note type
            deleteNoteType: function (id) {
                return umbRequestHelper.resourcePromise(
                    $http.delete("backoffice/notely/notelyapi/deletenotetype", { params: { id: id } }),
                    "Unable to delete the note type!"
                );
            },

            // Get all comments
            getAllComments: function (logType) {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getallnotecomments", { params: { logType: logType } }),
                    "Unable to get all the comments!"
                );
            },

            // Get comments of note
            getComments: function (noteId) {
                return umbRequestHelper.resourcePromise(
                    $http.get("backoffice/notely/notelyapi/getnotecomments", { params: { noteId: noteId } }),
                    "Unable to get the comments of the note!"
                );
            },

            // Add comment to note
            addComment: function (noteComment) {
                return umbRequestHelper.resourcePromise(
                    $http.post("backoffice/notely/notelyapi/addnotecomment", noteComment),
                    "Unable to add the comment to the note!"
                );
            },

            // Delete comment from note
            deleteComment: function (commentId) {
                return umbRequestHelper.resourcePromise(
                    $http.delete("backoffice/notely/notelyapi/deletenotecomment", { params: { commentId: commentId } }),
                    "Unable to delete the note comment!"
                );
            }
        };
    }
);