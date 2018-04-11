(function () {

    'use strict';

    /*
    * @ngdoc Directive
    * @name Notely.Directives.NotelyNotePreview
    * 
    * @description
    * 
    */

    function NotePreviewDirective() {

        function link(scope, el, attr, ctrl) {
            if (!scope.editLabelKey) {
                scope.editLabelKey = "general_edit";
            }
        }

        var directive = {
            restrict: 'E',
            replace: true,
            templateUrl: '/App_Plugins/Notely/views/directives/notely-note-preview.html',
            scope: {
                icon: "=?",
                name: "=",
                assignTo: "=?",
                state: "=?",
                priority: "=?",
                isNote: "=?",
                canAssign: "=?",
                allowOpen: "=?",
                allowEdit: "=?",
                allowRemove: "=?",
                onOpen: "&?",
                onRemove: "&?",
                onEdit: "&?"
            },
            link: link
        };

        return directive;

    }

    angular.module('notely.directives').directive('notelyNotePreview', NotePreviewDirective);

})();