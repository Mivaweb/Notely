/*
 * @ngdoc Service
 * @name commentService
 * 
 */
angular.module('notely.services').factory('commentService', [

    function () {

        return {

            // Format the description which adds the assignee if assignd to
            formatDescription: function (comment) {
                return comment.description + (comment.type.canAssign && comment.assignedTo ? ' <strong>[Assigned to: ' + comment.assignedTo.name + ']</strong>' : '');
            },

            // Add comment
            getAddOverlay: function () {
                return {
                    view: "/App_Plugins/Notely/backoffice/notely/dialogs/notely.comments.add.html",
                    title: "Add note",
                    show: true,
                    property: {},
                    hideSubmitButton: false,
                    close: undefined,
                    submit: undefined
                };
            },

            // Edit comment
            getEditOverlay: function () {
                return {
                    view: "/App_Plugins/Notely/backoffice/notely/dialogs/notely.comments.edit.html",
                    title: "Edit note",
                    comment: {},
                    show: true,
                    hideSubmitButton: false,
                    close: undefined,
                    submit: undefined
                };
            },

            // Edit comment
            getDeleteDialog: function () {
                return {
                    template: '/App_Plugins/Notely/backoffice/notely/dialogs/notely.comments.delete.html',
                    dialogData: undefined,
                    callback: undefined
                };
            }

        };

    }
]);