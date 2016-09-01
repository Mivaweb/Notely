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
    self.type = 0;
    self.assignedTo = null;
    self.state = false;
    self.nodeId = -1;
    self.contentProperty = new ContentProperty();
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

var User = new function () {
    var self = this;

    self.id = -1;
    self.name = '';
};

angular.module('notely.models').constant('User', User);

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