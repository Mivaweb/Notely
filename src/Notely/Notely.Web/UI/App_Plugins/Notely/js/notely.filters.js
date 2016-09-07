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