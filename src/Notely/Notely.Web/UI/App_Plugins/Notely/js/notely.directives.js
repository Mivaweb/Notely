/*
 * @ngdoc Directive
 * @name notelyProperty
 * 
 */
angular.module('notely.directives').directive('notelyProperty',

    function () {

        var link = function (scope, element, attrs, controllers) {

            scope.model = {};

            // Because we catch info from the backoffice async
            // We need to wait untill this info is captured
            // After that we can copy the config to the scope.model.config
            scope.$watch('config', function (config) {
                scope.model.config = angular.copy(scope.config);
            });

            // Copy value
            scope.model.value = scope.value;

            // Copy alias
            scope.model.alias = scope.alias;

        };

        return {
            restrict: "E",
            replace: true,
            link: link,
            template: '<div class="notely-property" ng-include="propertyEditorView"></div>',
            scope: {
                propertyEditorView: '=view',
                config: '=',
                value: '=',
                alias: '='
            }
        };
    }

);