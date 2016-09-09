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
                    hideSubmitButton: false,
                    close: undefined,
                    submit: undefined
                };
            },

            // Edit note
            getEditOverlay: function () {
                return {
                    view: "/App_Plugins/Notely/backoffice/notely/dialogs/notely.notes.edit.html",
                    title: "Edit note",
                    comment: {},
                    show: true,
                    hideSubmitButton: false,
                    close: undefined,
                    submit: undefined
                };
            },

            // Delete note
            getDeleteDialog: function () {
                return {
                    template: '/App_Plugins/Notely/backoffice/notely/dialogs/notely.notes.delete.html',
                    dialogData: undefined,
                    callback: undefined
                };
            }

        };

    }
]);