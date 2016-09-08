/*
     * @ngdoc model
     * @name DataType
     * @function
     * 
     * @description
     * Represents the DataType object
     * 
     */

var DataType = function () {
    var self = this;

    self.guid = '';
    self.name = '';
    self.prevalues = null;
    self.propertyTypeAlias = '';
    self.view = '';
};

angular.module('notely.models').constant('DataType', DataType);


/*
     * @ngdoc model
     * @name Property
     * @function
     * 
     * @description
     * Represents the Property object
     * 
     */

var Property = function () {
    var self = this;

    self.config = {};
    self.view = '';
};

angular.module('notely.models').constant('Property', Property);


/*
 * @ngdoc model
 * @name ContentProperty
 * @function
 * Used to link comments to the right content node and property type
 * 
 * @description
 * Represents the js version of the Notely's PropertyViewModel
 * 
 */

var ContentProperty = function () {
    var self = this;

    self.contentId = -1;
    self.propertyDataId = -1;
    self.propertyTypeAlias = '';
};

angular.module('notely.models').constant('ContentProperty', ContentProperty);


/*
 * @ngdoc model
 * @name CommentType
 * @function
 * 
 * @description
 * Represents the js version of the Notely's CommentType object
 * 
 */

var CommentType = function () {
    var self = this;

    self.id = -1;
    self.title = '';
    self.icon = '';
    self.canAssign = false;
};

angular.module('notely.models').constant('CommentType', CommentType);


/*
 * @ngdoc model
 * @name CommentState
 * @function
 * 
 * @description
 * Represents the js version of the Notely's CommentState object
 * 
 */

var CommentState = function () {
    var self = this;

    self.id = -1;
    self.title = '';
    self.color = '';
};

angular.module('notely.models').constant('CommentState', CommentState);


/*
 * @ngdoc model
 * @name Comment
 * @function
 * 
 * @description
 * Represents the js version of the Notely's CommentViewModel
 * 
 */

var Comment = function () {
    var self = this;

    self.id = -1;
    self.title = '';
    self.description = '';
    self.type = null;
    self.assignedTo = null;
    self.state = null;
    self.contentProperty = null;
    self.closed = false;
};

angular.module('notely.models').constant('Comment', Comment);



/*
 * @ngdoc model
 * @name User
 * @function
 * 
 * @description
 * Represents the js version of the Notely's UserViewModel
 * 
 */

var User = function () {
    var self = this;

    self.id = -1;
    self.name = '';
};

angular.module('notely.models').constant('User', User);


/*
 * @ngdoc model
 * @name BackOfficeNode
 * @function
 * 
 * @description
 * Represents the js version of the Notely's BackOfficeNode
 * 
 */

var BackOfficeNode = function () {
    var self = this;

    self.contentId = -1;
    self.contentName = '';
    self.showDetails = false;
    self.properties = [];
};

angular.module('notely.models').constant('BackOfficeNode', BackOfficeNode);


/*
 * @ngdoc model
 * @name BackOfficeProperty
 * @function
 * 
 * @description
 * Represents the js version of the Notely's BackOfficeProperty
 * 
 */

var BackOfficeProperty = function () {
    var self = this;

    self.id = -1;
    self.alias = '';
    self.name = '';
    self.limit = 1;
    self.comments = [];
};

angular.module('notely.models').constant('BackOfficeProperty', BackOfficeProperty);


/*
* @ngdoc service
* @name modelsBuilder
* 
* @description 
* Convert json result into angular model
* 
*/
angular.module('notely.models').factory('modelsBuilder', [

    function () {

        // Private convert function
        function convertItem(jsonResult, Constructor) {
            var model = new Constructor();
            angular.extend(model, jsonResult);
            return model;
        }

        // Convert json result to the provided model
        function convert(jsonResult, Constructor) {
            if (angular.isArray(jsonResult)) {
                // Array: So we need to convert each element and push it into a new array to send back
                var models = [];
                angular.forEach(jsonResult, function (item) {
                    models.push(convertItem(item, Constructor));
                });
                return models;
            } else {
                return convertItem(jsonResult, Constructor);
            }
        }

        // Make functions public
        return {
            convert: convert
        };

    }

]);

/*
 * @ngdoc service
 * @name DataTypeBuilder
 * 
 * @decription
 * Modelsbuilder for the DataType model
 * 
 */
angular.module('notely.models').factory('dataTypeBuilder', [

    'modelsBuilder',
    'DataType',

    function (modelsBuilder, DataType) {

        var Constructor = DataType;

        return {
            createEmpty: function () {
                return new Constructor();
            },
            convert: function (jsonResult) {
                return modelsBuilder.convert(jsonResult, Constructor);
            }
        };

    }

]);

/*
 * @ngdoc service
 * @name propertyBuilder
 * 
 * @decription
 * Modelsbuilder for the Property model
 * 
 */
angular.module('notely.models').factory('propertyBuilder', [

    'modelsBuilder',
    'Property',

    function (modelsBuilder, Property) {

        var Constructor = Property;

        return {
            createEmpty: function () {
                return new Constructor();
            },
            convert: function (jsonResult) {
                return modelsBuilder.convert(jsonResult, Constructor);
            }
        };

    }

]);

/*
 * @ngdoc service
 * @name contentPropertyBuilder
 * 
 * @decription
 * Modelsbuilder for the ContentProperty model
 * 
 */
angular.module('notely.models').factory('contentPropertyBuilder', [

    'modelsBuilder',
    'ContentProperty',

    function (modelsBuilder, ContentProperty) {

        var Constructor = ContentProperty;

        return {
            createEmpty: function () {
                return new Constructor();
            },
            convert: function (jsonResult) {
                return modelsBuilder.convert(jsonResult, Constructor);
            }
        };

    }

]);


/*
 * @ngdoc service
 * @name commentTypeBuilder
 * 
 * @decription
 * Modelsbuilder for the CommentType model
 * 
 */
angular.module('notely.models').factory('commentTypesBuilder', [

    'modelsBuilder',
    'CommentType',

    function (modelsBuilder, CommentType) {

        var Constructor = CommentType;

        return {
            createEmpty: function () {
                return new Constructor();
            },
            convert: function (jsonResult) {
                return modelsBuilder.convert(jsonResult, Constructor);
            }
        };

    }

]);


/*
 * @ngdoc service
 * @name commentStateBuilder
 * 
 * @decription
 * Modelsbuilder for the CommentState model
 * 
 */
angular.module('notely.models').factory('commentStatesBuilder', [

    'modelsBuilder',
    'CommentState',

    function (modelsBuilder, CommentState) {

        var Constructor = CommentState;

        return {
            createEmpty: function () {
                return new Constructor();
            },
            convert: function (jsonResult) {
                return modelsBuilder.convert(jsonResult, Constructor);
            }
        };

    }

]);


/*
 * @ngdoc service
 * @name commentsBuilder
 * 
 * @decription
 * Modelsbuilder for the Comment model
 * 
 */
angular.module('notely.models').factory('commentsBuilder', [

    'modelsBuilder',
    'Comment',
    'contentPropertyBuilder',
    'commentTypesBuilder',
    'commentStatesBuilder',

    function (modelsBuilder, Comment, contentPropertyBuilder, commentTypesBuilder, commentStatesBuilder) {

        var Constructor = Comment;

        return {
            createEmpty: function () {
                var _c = new Constructor();
                _c.contentProperty = contentPropertyBuilder.createEmpty();
                _c.type = commentTypesBuilder.createEmpty();
                _c.state = commentStatesBuilder.createEmpty();
                return _c;
            },
            convert: function (jsonResult) {
                return modelsBuilder.convert(jsonResult, Constructor);
            }
        };

    }

]);

/*
 * @ngdoc service
 * @name usersBuilder
 * 
 * @decription
 * Modelsbuilder for the User model
 * 
 */
angular.module('notely.models').factory('usersBuilder', [

    'modelsBuilder',
    'User',

    function (modelsBuilder, User) {

        var Constructor = User;

        return {
            createEmpty: function () {
                return new Constructor();
            },
            convert: function (jsonResult) {
                return modelsBuilder.convert(jsonResult, Constructor);
            }
        };

    }

]);


/*
 * @ngdoc service
 * @name backOfficeNodesBuilder
 * 
 * @decription
 * Modelsbuilder for the User model
 * 
 */
angular.module('notely.models').factory('backOfficeNodesBuilder', [

    'modelsBuilder',
    'BackOfficeNode',

    function (modelsBuilder, BackOfficeNode) {

        var Constructor = BackOfficeNode;

        return {
            createEmpty: function () {
                return new Constructor();
            },
            convert: function (jsonResult) {
                return modelsBuilder.convert(jsonResult, Constructor);
            }
        };

    }

]);