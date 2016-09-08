/*
 * @ngdoc Controller
 * @name Notely.PropertyEditors.DataTypePickerController
 * 
 * @description
 * Contains the logic to display all available datatypes ( excepts the ones of notely )
 * 
 */
angular.module('notely').controller('Notely.PropertyEditors.DataTypePickerController', [

    '$scope',
    'notelyResources',
    'dataTypeBuilder',

    function ($scope, notelyResources, dataTypeBuilder) {

        $scope.loaded = false;

        // Load all datatypes
        var dataTypePromise = notelyResources.getDataTypes();
        dataTypePromise.then(function (data) {
            $scope.model.dataTypes = dataTypeBuilder.convert(data);
            $scope.loaded = true;
        });
        
    }

]);


/*
 * @ngdoc Controller
 * @name Notely.PropertyEditors.MainController
 * 
 * @description
 * Contains the logic of the wrapped property editor
 * 
 */
angular.module('notely').controller('Notely.PropertyEditors.MainController', [

    '$scope',
    'umbPropEditorHelper',
    'notelyResources',
    'dataTypeBuilder',
    'propertyBuilder',
    'commentsBuilder',
    'contentPropertyBuilder',
    '$routeParams',
    'notificationsService',
    'dialogService',

    function ($scope, umbPropEditorHelper, notelyResources, dataTypeBuilder, propertyBuilder,
        commentsBuilder, contentPropertyBuilder, $routeParams, notificationsService, dialogService) {

        $scope.loaded = false;

        $scope.showComments = false;
        $scope.hasProperty = false;
        $scope.showDefault = false;

        // Init function
        $scope.init = function () {
            // Check if there is a data type selected
            if ($scope.model.config.dataType != null) {

                var dataTypePromise = notelyResources.getDataType($scope.model.config.dataType.guid);
                dataTypePromise.then(function (data) {
                    var _dataType = dataTypeBuilder.convert(data);

                    // Create a property object
                    $scope.property = propertyBuilder.createEmpty();
                    $scope.property.config = _dataType.prevalues;
                    $scope.property.view = umbPropEditorHelper.getViewPath(data.view);

                    // We also need to append the config to the model.config scope
                    // because only then the property editor will load in the configuration prevalues
                    angular.extend($scope.model.config, $scope.property.config);
                });

            }

            // Show default message
            $scope.showDefault = ($routeParams.section == "content" && $scope.model.id <= 0);

            // Check if existing content page
            if ($routeParams.section == "content" && $routeParams.id > 0 && $scope.model.id > 0) {
                $scope.hasProperty = true;
                $scope.showDefault = false;

                // Get the comments
                $scope.load();
            }

            $scope.loaded = true;
        };

        // Load comments
        $scope.load = function () {
            $scope.comments = [];

            var _contentProperty = contentPropertyBuilder.createEmpty();
            _contentProperty.contentId = $routeParams.id;
            _contentProperty.propertyDataId = $scope.model.id;
            _contentProperty.propertyTypeAlias = $scope.model.alias;

            if(_contentProperty.contentId > 0 && _contentProperty.propertyDataId > 0)
            {
                var commentsPromise = notelyResources.getComments(_contentProperty);
                commentsPromise.then(function (data) {
                    $scope.comments = commentsBuilder.convert(data);
                });
            }
        };

        // Expand comments list
        $scope.expand = function () {
            $scope.showComments = !$scope.showComments;
        };

        // Render the comment description field
        $scope.renderDescription = function (comment) {
            return comment.description + (comment.type.canAssign && comment.assignedTo ? ' <strong>[Assigned to: ' + comment.assignedTo.name + ']</strong>' : '');
        };

        // Add a new comment
        $scope.addComment = function () {

            // Extra check to see if comments limit is not reached
            if ($scope.comments.length < $scope.model.config.limit) {

                // Create new contentProperty object
                var _cp = contentPropertyBuilder.createEmpty();
                _cp.contentId = $routeParams.id;
                _cp.propertyDataId = $scope.model.id;
                _cp.propertyTypeAlias = $scope.model.alias;

                $scope.overlay = {
                    view: "/App_Plugins/Notely/views/dialogs/notely.comments.add.html",
                    title: "Add comment",
                    show: true,
                    property: _cp,
                    hideSubmitButton: false,
                    close: function (oldModel) {
                        $scope.overlay.show = false;
                        $scope.overlay = null;
                    },
                    submit: function (model) {
                        // Add comment
                        notelyResources.addComment(model.comment).then(function () {
                            // Get the comments
                            $scope.load();
                        });

                        $scope.overlay.show = false;
                        $scope.overlay = null;

                        // Show notification
                        notificationsService.success("Comment added", "Comment is successfully added to the property editor.");
                    }
                };

            }

        };

        // Edit comment
        $scope.editComment = function (comment) {
            // Assign the propertydataid ( model.id ) to the comment object
            comment.contentProperty.propertyDataId = $scope.model.id;

            $scope.overlay = {
                view: "/App_Plugins/Notely/views/dialogs/notely.comments.edit.html",
                title: "Edit comment",
                comment: angular.copy(comment),
                show: true,
                hideSubmitButton: false,
                close: function (oldModel) {
                    $scope.overlay.show = false;
                    $scope.overlay = null;
                },
                submit: function (model) {
                    // Update comment
                    notelyResources.updateComment(model.comment).then(function () {
                        // Get the comments
                        $scope.load();
                    });

                    $scope.overlay.show = false;
                    $scope.overlay = null;

                    // Show notification
                    notificationsService.success("Comment saved", "Comment is successfully saved.");
                }
            };
        };

        // Delete a comment
        $scope.deleteComment = function (commentId) {
            dialogService.open({
                template: '/App_Plugins/Notely/views/dialogs/notely.comments.delete.html',
                dialogData: commentId,
                callback: function (data) {
                    notelyResources.deleteComment(data).then(function () {
                        // Get the comments
                        $scope.load();

                        // Show notification
                        notificationsService.success("Comment removed", "Comment is successfully deleted.");
                    });
                }
            });
        };

        // Set task as done
        $scope.taskComplete = function (commentId) {
            notelyResources.taskComplete(commentId).then(function () {
                // Get the comments
                $scope.load();

                // Show notification
                notificationsService.success("Task completed", "Todo comment is set completed.");
            });
        };

    }

]);

/*
 * @ngdoc Controller
 * @name Notely.PropertyEditors.AddController
 */
angular.module('notely').controller('Notely.PropertyEditors.AddController', [

    '$scope',
    'notelyResources',
    '$routeParams',
    'commentsBuilder',
    'commentTypesBuilder',
    'commentStatesBuilder',
    'usersBuilder',

    function ($scope, notelyResources, $routeParams, commentsBuilder, commentTypesBuilder, commentStatesBuilder, usersBuilder) {

        // Reset comment window
        $scope.model.comment = commentsBuilder.createEmpty();
        $scope.model.comment.contentProperty.contentId = $scope.model.property.contentId;
        $scope.model.comment.contentProperty.propertyDataId = $scope.model.property.propertyDataId;
        $scope.model.comment.contentProperty.propertyTypeAlias = $scope.model.property.propertyTypeAlias;

        // Init controller
        $scope.init = function () {
            // Get comment types
            var commentTypesPromise = notelyResources.getCommentTypes();
            commentTypesPromise.then(function (data) {
                $scope.commentTypes = commentTypesBuilder.convert(data);
                $scope.model.comment.type = $scope.commentTypes[0];
            });

            // Get comment states
            var commentStatesPromise = notelyResources.getCommentStates();
            commentStatesPromise.then(function (data) {
                $scope.commentStates = commentStatesBuilder.convert(data);
                $scope.model.comment.state = $scope.commentStates[0];
            });

            // Get active users to display in select
            var usersPromise = notelyResources.getUsers();
            usersPromise.then(function (data) {
                $scope.users = usersBuilder.convert(data);
            });
        };

        // Comment type changed
        $scope.commentTypeChanged = function () {
            if (!$scope.model.comment.type.canAssign)
                $scope.resetAssigndTo();

            // Reset state
            $scope.model.comment.state = $scope.commentStates[0];
        };

        // Reset select
        $scope.resetAssigndTo = function () {
            $scope.model.comment.assignedTo = null;
        };

    }

]);

/*
 * @ngdoc Controller
 * @name Notely.PropertyEditors.EditController
 * 
 * @description
 */
angular.module('notely').controller('Notely.PropertyEditors.EditController', [

    '$scope',
    'notelyResources',
    'commentsBuilder',
    'commentTypesBuilder',
    'commentStatesBuilder',
    'usersBuilder',
    '$routeParams',

    function ($scope, notelyResources, commentsBuilder, commentTypesBuilder, commentStatesBuilder, usersBuilder, $routeParams) {

        // Init controller
        $scope.init = function () {

            // Comment model is already in scope when calling the overlay
            // So we don't need to call the api again to catch the comment data
            // We also made a copy so that changes are not visible in the list untill we hit save!

            // Get comment types
            var commentTypesPromise = notelyResources.getCommentTypes();
            commentTypesPromise.then(function (data) {
                $scope.commentTypes = commentTypesBuilder.convert(data);
            });

            // Get comment states
            var commentStatesPromise = notelyResources.getCommentStates();
            commentStatesPromise.then(function (data) {
                $scope.commentStates = commentStatesBuilder.convert(data);
            });

            // Get active users to display in select
            var usersPromise = notelyResources.getUsers();
            usersPromise.then(function (data) {
                $scope.users = usersBuilder.convert(data);
            });
        };

        // Comment type changed
        $scope.commentTypeChanged = function () {
            if (!$scope.model.comment.type.canAssign)
                $scope.resetAssigndTo();

            // Reset state
            $scope.model.comment.state = $scope.commentStates[0];
        };

        // Reset select
        $scope.resetAssigndTo = function () {
            $scope.model.comment.assignedTo = null;
        };

    }

]);

/*
 * @ngdoc Controller
 * @name Notely.PropertyEditors.DeleteController
 */
angular.module('notely').controller('Notely.PropertyEditors.DeleteController', [

    '$scope',
    'notelyResources',
    'commentsBuilder',

    function ($scope, notelyResources, commentsBuilder) {

        // Init model object
        $scope.model = {};

        // Init controller
        $scope.init = function (commentId) {
            // Get comment by id
            notelyResources.getComment(commentId).then(function (data) {
                $scope.model.comment = commentsBuilder.convert(data);
            });
        };

        // Delete comment
        $scope.deleteComment = function (commentId) {
            $scope.submit(commentId);
        };

    }

]);

/*
 * @ngdoc Controller
 * @name Notely.Backoffice.DashboardController
 * 
 */
angular.module('notely').controller('Notely.Backoffice.DashboardController', [

    '$scope',
    'notelyResources',
    '$routeParams',
    'dialogService',
    'notificationsService',

    function ($scope, notelyResources, $routeParams, dialogService, notificationsService) {

        $scope.loaded = false;

        // Init function
        $scope.init = function () {

            $scope.loaded = true;

        };

    }

]);

/*
 * @ngdoc Controller
 * @name Notely.Backoffice.CommentsController
 * 
 */
angular.module('notely').controller('Notely.Backoffice.CommentsController', [

    '$scope',
    'notelyResources',
    'backOfficeNodesBuilder',
    'commentsBuilder',
    'commentTypesBuilder',
    'commentStatesBuilder',
    'contentPropertyBuilder',
    'userService',
    '$routeParams',
    'dialogService',
    'notificationsService',

    function ($scope, notelyResources, backOfficeNodesBuilder, commentsBuilder, commentTypesBuilder, commentStatesBuilder,
        contentPropertyBuilder, userService, $routeParams, dialogService, notificationsService) {

        $scope.loaded = false;
        $scope.expanded = false;
        $scope.treeNode = 0; // 0 = all comments / 1 = my comments
        $scope.options = {
            type: {},
            state: {},
            hiding: false
        };
        $scope.commentTypes = [];
        $scope.commentStates = [];
        $scope.backOfficeDetails = [];
        $scope.visibleTabs = [];
        $scope.filteredComments = [];

        // Init function
        $scope.init = function () {

            // Get current tree node
            $scope.treeNode = $routeParams.id;

            // Setup tabs
            $scope.visibleTabs.push({
                id: 1,
                label: 'Comments Listing'
            });

            // Get all the options
            $scope.loadOptions();

            // Get all the comments
            $scope.load();

        };

        // Load the filter options
        $scope.loadOptions = function () {

            // Get comment types
            var commentTypesPromise = notelyResources.getCommentTypes();
            commentTypesPromise.then(function (data) {
                $scope.commentTypes = commentTypesBuilder.convert(data);
            });

            // Get comment states
            var commentStatesPromise = notelyResources.getCommentStates();
            commentStatesPromise.then(function (data) {
                $scope.commentStates = commentStatesBuilder.convert(data);
            });

        };

        // Load the comments from the API
        $scope.load = function () {

            // Reset
            $scope.backOfficeDetails = [];

            // Check id of the route parameters:
            // Case 1: My comments => so we need to get the current logged in user and get his comments
            // Case 0: All comments
            var _userServicePromise = userService.getCurrentUser();
            var _user = -1;

            if ($scope.treeNode == 1)
            {
                _userServicePromise.then(function (user) {
                    _user = user.id;

                    loadDetails(_user);
                });
            } else {
                loadDetails(_user);
            }

            $scope.loaded = true;

        };

        // Reset
        $scope.reset = function () {
            $scope.options = {
                type: {},
                state: {},
                hiding: false
            };
        };

        // Reload
        $scope.reload = function () {
            $scope.loadOptions();
            $scope.load();
            $scope.reset();
            $scope.expanded = false;
        };

        // Expand the content node details
        $scope.expandContent = function (index) {
            $scope.backOfficeDetails[index].showDetails = !$scope.backOfficeDetails[index].showDetails;

            checkToggleState();
        };

        // Toggle content nodes
        $scope.toggleContentNodes = function () {
            $scope.expanded = !$scope.expanded;

            angular.forEach($scope.backOfficeDetails, function (content) { content.showDetails = $scope.expanded; });
        }

        // Render the comment description field
        $scope.renderDescription = function (comment) {
            return comment.description + (comment.type.canAssign && comment.assignedTo ? ' <strong>[Assigned to: ' + comment.assignedTo.name + ']</strong>' : '');
        };

        // Changed filter option type
        $scope.changedType = function () {
            if ($scope.options.type.id > 0 && !$scope.options.type.canAssign)
                $scope.options.state = {};
        };

        // Add a new comment
        $scope.addComment = function (contentId, property) {

            // Extra check to see if comments limit is not reached
            if (property.comments.length < property.limit) {

                // Create new contentProperty object
                var _cp = contentPropertyBuilder.createEmpty();
                _cp.contentId = contentId;
                _cp.propertyDataId = property.id;
                _cp.propertyTypeAlias = property.alias;

                $scope.overlay = {
                    view: "/App_Plugins/Notely/views/dialogs/notely.comments.add.html",
                    title: "Add comment",
                    show: true,
                    property: _cp,
                    hideSubmitButton: false,
                    close: function (oldModel) {
                        $scope.overlay.show = false;
                        $scope.overlay = null;
                    },
                    submit: function (model) {
                        // Add comment
                        notelyResources.addComment(model.comment).then(function () {
                            // Get the comments
                            $scope.load();
                            $scope.expanded = false;
                        });

                        $scope.overlay.show = false;
                        $scope.overlay = null;

                        // Show notification
                        notificationsService.success("Comment added", "Comment is successfully added to the property editor.");
                    }
                };
            }

        };

        // Edit comment
        $scope.editComment = function (comment) {
            $scope.overlay = {
                view: "/App_Plugins/Notely/views/dialogs/notely.comments.edit.html",
                title: "Edit comment",
                comment: angular.copy(comment),
                show: true,
                hideSubmitButton: false,
                close: function (oldModel) {
                    $scope.overlay.show = false;
                    $scope.overlay = null;
                },
                submit: function (model) {
                    // Update comment
                    notelyResources.updateComment(model.comment).then(function () {
                        // Reload
                        $scope.load();
                        $scope.expanded = false;
                    });

                    $scope.overlay.show = false;
                    $scope.overlay = null;

                    // Show notification
                    notificationsService.success("Comment saved", "Comment is successfully saved.");
                }
            };
        };

        // Delete a comment
        $scope.deleteComment = function (commentId) {
            dialogService.open({
                template: '/App_Plugins/Notely/views/dialogs/notely.comments.delete.html',
                dialogData: commentId,
                callback: function (data) {
                    notelyResources.deleteComment(data).then(function () {
                        // Get the comments
                        $scope.load();

                        // Show notification
                        notificationsService.success("Comment removed", "Comment is successfully deleted.");
                    });
                }
            });
        };

        function loadDetails(_user) {
            notelyResources.getUniqueContentNodes(_user).then(function (data) {

                angular.forEach(data, function (content) {

                    var contentPromise = notelyResources.getContentNodeDetails(content, _user);
                    contentPromise.then(function (details) {
                        $scope.backOfficeDetails.push(backOfficeNodesBuilder.convert(details));
                    });

                });

            });
        }

        function checkToggleState() {
            var result = $scope.backOfficeDetails.filter(function (detail) { return detail.showDetails == !$scope.expanded; });
            
            if(result.length == $scope.backOfficeDetails.length)
                $scope.expanded = !$scope.expanded;
        }

    }

]);

/*
 * @ngdoc Controller
 * @name Notely.Backoffice.SettingsController
 * 
 */
angular.module('notely').controller('Notely.Backoffice.SettingsController', [

    '$scope',
    'notelyResources',
    'commentTypesBuilder',
    'commentStatesBuilder',
    'notificationsService',

    function ($scope, notelyResources, commentTypesBuilder, commentStatesBuilder, notificationsService) {

        $scope.loaded = false;

        $scope.visibleTabs = [];

        $scope.commentTypes = [];

        // Init function
        $scope.init = function () {

            // Setup tabs
            $scope.visibleTabs.push({
                id: 1,
                label: 'Notely Settings'
            });

            $scope.load();

            $scope.loaded = true;

        };

        // Load data
        $scope.load = function () {
            var commentTypePromise = notelyResources.getCommentTypes();
            commentTypePromise.then(function (data) {
                $scope.commentTypes = commentTypesBuilder.convert(data);
            });

            var commentStatePromise = notelyResources.getCommentStates();
            commentStatePromise.then(function (data) {
                $scope.commentStates = commentStatesBuilder.convert(data);
            });
        };

    }

]);

/*
 * @ngdoc Controller
 * @name Notely.Backoffice.CleanupController
 * 
 */
angular.module('notely').controller('Notely.Backoffice.CleanupController', [

    '$scope',
    'notelyResources',
    'notificationsService',

    function ($scope, notelyResources, notificationsService) {

        $scope.loaded = false;

        $scope.visibleTabs = [];

        // Init function
        $scope.init = function () {

            // Setup tabs
            $scope.visibleTabs.push({
                id: 1,
                label: 'Cleanup Comments'
            });

            $scope.loaded = true;

        };

        // Cleanup comments
        $scope.cleanup = function () {
            var cleanupPromise = notelyResources.cleanupComments();
            cleanupPromise.then(function (data) {
                notificationsService.success("Cleanup done", data + " unnecessary comments were removed.");
            });
        };

    }

]);