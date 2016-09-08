// Bootstrap the Commentor angular module
(function () {
    angular.module('notely', [
        'umbraco.filters',
        'umbraco.directives',
        'umbraco.services',
        'notely.filters',
        'notely.directives',
        'notely.resources',
        'notely.services',
        'chart.js'
    ]);

    angular.module('notely.models', []);
    angular.module('notely.filters', []);
    angular.module('notely.directives', []);
    angular.module('notely.resources', ['notely.models']);
    angular.module('notely.services', ['notely.models']);

    angular.module('umbraco.packages').requires.push('notely');

}());