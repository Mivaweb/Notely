/*
 * @ngdoc filter
 * @name filterType
 * 
 * @description
 * Filter comments based on the comment type
 * 
 */
angular.module('notely.filters').filter('filterType',

    function () {
        return function (comments, type) {
            var filtered = [];

            if (type && type.id > 0) {
                angular.forEach(comments, function (comment) {

                    if (comment.type.id == type.id) {
                        filtered.push(comment);
                    }

                });

            } else {
                return comments;
            }

            return filtered;
        };
    }

);

/*
 * @ngdoc filter
 * @name filterState
 * 
 * @description
 * Filter comments based on the comment state
 * 
 */
angular.module('notely.filters').filter('filterState',

    function () {
        return function (comments, state) {
            var filtered = [];

            if (state && state.id > 0) {
                angular.forEach(comments, function (comment) {

                    if (comment.state.id == state.id) {
                        filtered.push(comment);
                    }

                });

            } else {
                return comments;
            }

            return filtered;
        };
    }

);

/*
 * @ngdoc filter
 * @name orderByContentId
 * 
 * @description
 * Sort comments based on the content id
 * 
 */
angular.module('notely.filters').filter('orderByContentId',

    function () {
        return function (details) {
            var sorted = details;
            sorted.sort(function (a, b) {
                return a.contentId > b.contentId ? 1 : -1;
            });
            return sorted;
        };
    }

);