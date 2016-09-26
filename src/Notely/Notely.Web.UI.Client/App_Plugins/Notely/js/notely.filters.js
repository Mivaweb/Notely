/*
 * @ngdoc filter
 * @name filterType
 * 
 * @description
 * Filter notes based on the note type
 * 
 */
angular.module('notely.filters').filter('filterType',

    function () {
        return function (notes, type) {
            var filtered = [];

            if (type && type.id > 0) {
                angular.forEach(notes, function (note) {

                    if (note.type.id == type.id) {
                        filtered.push(note);
                    }

                });

            } else {
                return notes;
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
 * Filter notes based on the note state
 * 
 */
angular.module('notely.filters').filter('filterState',

    function () {
        return function (notes, state) {
            var filtered = [];

            if (state && state.id > 0) {
                angular.forEach(notes, function (note) {

                    if (note.state.id == state.id) {
                        filtered.push(note);
                    }

                });

            } else {
                return notes;
            }

            return filtered;
        };
    }

);

/*
 * @ngdoc filter
 * @name filterPriority
 * 
 * @description
 * Filter notes based on the note priority
 * 
 */
angular.module('notely.filters').filter('filterPriority',

    function () {
        return function (notes, priority) {
            var filtered = [];

            if (priority) {
                angular.forEach(notes, function (note) {

                    if (note.priority == priority) {
                        filtered.push(note);
                    }

                });

            } else {
                return notes;
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
 * Sort notes based on the content id
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


/*
 * @ngdoc filter
 * @name setbold
 * 
 * @description
 * Replace % by strong to set text bold
 * 
 */
angular.module('notely.filters').filter('setbold',

    function () {
        return function (text) {
            return text.replace(/%1%/g, '<strong>').replace(/%2%/g, '</strong>');
        };
    }

);

/*
 * @ngdoc filter
 * @name filterLogType
 * 
 * @description
 * Filter note comments based on the log type
 * 
 */
angular.module('notely.filters').filter('filterLogType',

    function () {
        return function (comments, type) {
            var filtered = [];

            if (type) {
                angular.forEach(comments, function (comment) {

                    if (comment.logType == type) {
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
 * @name pagination
 * 
 * @description
 * Filter results with pagination
 * 
 */
angular.module('notely.filters').filter('pagination',

    function () {
        return function (elements, pagination) {
            var filtered = [];

            if (pagination) {

                var begin, end, index;

                begin = (pagination.current - 1) * pagination.limit;
                end = begin + pagination.limit;
                
                angular.forEach(elements, function (element) {
                    index = elements.indexOf(element);

                    if (begin <= index && index < end)
                        filtered.push(element);
                });

            } else {
                return elements;
            }

            return filtered;
        };
    }

);