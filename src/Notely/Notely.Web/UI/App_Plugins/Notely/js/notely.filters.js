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