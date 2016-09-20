/*
 * @ngdoc Service
 * @name noteService
 * 
 */
angular.module('notely.services').factory('noteService', [

    function () {

        return {

            // Format the description which adds the assignee if assignd to
            formatDescription: function (note) {
                return note.description + (note.type.canAssign && note.assignedTo ? ' <strong>[Assigned to: ' + note.assignedTo.name + ']</strong>' : '');
            },

            // Add note
            getAddOverlay: function () {
                return {
                    view: "/App_Plugins/Notely/backoffice/notely/dialogs/notely.notes.add.html",
                    title: "Add note",
                    show: true,
                    property: {},
                    hideSubmitButton: false
                };
            },

            // Edit note
            getEditOverlay: function () {
                return {
                    view: "/App_Plugins/Notely/backoffice/notely/dialogs/notely.notes.edit.html",
                    title: "Edit note",
                    note: {},
                    show: true,
                    hideSubmitButton: false
                };
            },

            // Delete note
            getDeleteDialog: function () {
                return {
                    template: '/App_Plugins/Notely/backoffice/notely/dialogs/notely.notes.delete.html'
                };
            },

            // View comments
            getViewCommentsDialog: function () {
                return {
                    template: '/App_Plugins/Notely/backoffice/notely/dialogs/notely.notes.comments.html'
                };
            }

        };

    }
]);