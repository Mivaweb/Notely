(function() {

    'use strict';

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
        'notesBuilder',
        'contentPropertyBuilder',
        '$routeParams',
        'notificationsService',
        'dialogService',
        'noteService',

        function ($scope, umbPropEditorHelper, notelyResources, dataTypeBuilder, propertyBuilder,
            notesBuilder, contentPropertyBuilder, $routeParams, notificationsService, dialogService, noteService) {

            var vm = this;
            vm.loaded = false;
            vm.showNotes = false;
            vm.hasProperty = false;
            vm.showDefault = false;

            // Init function
            function init() {
                // Check if there is a data type selected
                if ($scope.model.config.dataType != null) {

                    var dataTypePromise = notelyResources.getDataType($scope.model.config.dataType.guid);
                    dataTypePromise.then(function (data) {
                        var _dataType = dataTypeBuilder.convert(data);

                        // Create a property object
                        vm.property = propertyBuilder.createEmpty();
                        vm.property.config = _dataType.prevalues;
                        vm.property.view = umbPropEditorHelper.getViewPath(data.view);

                        // We also need to append the config to the model.config scope
                        // because only then the property editor will load in the configuration prevalues
                        angular.extend($scope.model.config, vm.property.config);
                    });

                }

                // Show default message
                vm.showDefault = ($routeParams.section == "content" && $scope.model.id <= 0);

                // Check if existing content page
                if ($routeParams.section == "content" && $routeParams.id > 0 && $scope.model.id > 0) {
                    vm.hasProperty = true;
                    vm.showDefault = false;

                    // Get the notes
                    load();
                }

                vm.loaded = true;
            };

            // Load notes
            function load() {
                vm.notes = [];

                var _contentProperty = contentPropertyBuilder.createEmpty();
                _contentProperty.contentId = $routeParams.id;
                _contentProperty.propertyDataId = $scope.model.id;
                _contentProperty.propertyTypeAlias = $scope.model.alias;

                if(_contentProperty.contentId > 0 && _contentProperty.propertyDataId > 0)
                {
                    var notesPromise = notelyResources.getNotes(_contentProperty);
                    notesPromise.then(function (data) {
                        vm.notes = notesBuilder.convert(data);
                    });
                }
            };

            // Expand note list
            function expand() {
                vm.showNotes = !vm.showNotes;
            };

            // Render the note description field
            function renderDescription(note) {
                //return noteService.formatDescription(note);
                return note.type.canAssign && note.assignedTo ? note.assignedTo.name : '';
            };

            // Add a new note
            function addNote() {

                // Extra check to see if notes limit is not reached
                if (vm.notes.length < $scope.model.config.limit) {

                    // Create new contentProperty object
                    var _cp = contentPropertyBuilder.createEmpty();
                    _cp.contentId = $routeParams.id;
                    _cp.propertyDataId = $scope.model.id;
                    _cp.propertyTypeAlias = $scope.model.alias;

                    // Get overlay object of the service
                    var _overlay = noteService.getAddOverlay();
                    _overlay.property = _cp;
                    _overlay.close = function (oldModel) {
                        vm.overlay.show = false;
                        vm.overlay = null;
                    };
                    _overlay.submit = function (model) {
                        notelyResources.addNote(model.note).then(function () {
                            load();
                        });

                        vm.overlay.show = false;
                        vm.overlay = null;

                        // Show notification
                        notificationsService.success("Note added", "Note is successfully added to the property editor.");
                    };

                    vm.overlay = _overlay;
                }
            };

            // Edit note
            function editNote(note) {
                // Assign the propertydataid ( model.id ) to the note object
                note.contentProperty.propertyDataId = $scope.model.id;

                var _overlay = noteService.getEditOverlay();
                _overlay.note = angular.copy(note);
                _overlay.close = function (oldModel) {
                    vm.overlay.show = false;
                    vm.overlay = null;
                };
                _overlay.submit = function (model) {
                    notelyResources.updateNote(model.note).then(function () {
                        load();
                    });

                    vm.overlay.show = false;
                    vm.overlay = null;

                    // Show notification
                    notificationsService.success("Note saved", "Note is successfully saved.");
                };

                vm.overlay = _overlay;
            };

            // Delete a note
            function deleteNote(noteId) {
                var _dialog = noteService.getDeleteDialog();
                _dialog.dialogData = noteId;
                _dialog.callback = function (data) {
                    notelyResources.deleteNote(data).then(function () {
                        load();

                        // Show notification
                        notificationsService.success("Note removed", "Note is successfully deleted.");
                    });
                };
                dialogService.open(_dialog);
            };

            // View comments from a note
            function viewComments(noteId) {
                var _data = {
                    note: noteId
                };

                var _dialog = noteService.getViewCommentsDialog();
                _dialog.dialogData = _data;
                dialogService.open(_dialog);
            };

            vm.init = init;
            vm.load = load;
            vm.expand = expand;
            vm.renderDescription = renderDescription;
            vm.addNote = addNote;
            vm.editNote = editNote;
            vm.deleteNote = deleteNote;
            vm.viewComments = viewComments;
        }

    ]);

    /*
     * @ngdoc Controller
     * @name Notely.Notes.AddController
     */
    angular.module('notely').controller('Notely.Notes.AddController', [

        '$scope',
        'notelyResources',
        '$routeParams',
        'notesBuilder',
        'noteTypesBuilder',
        'noteStatesBuilder',
        'usersBuilder',

        function ($scope, notelyResources, $routeParams, notesBuilder, noteTypesBuilder, noteStatesBuilder, usersBuilder) {

            var vm = this;
            vm.model = $scope.model;

            // Reset note window
            vm.model.note = notesBuilder.createEmpty();
            vm.model.note.contentProperty.contentId = vm.model.property.contentId;
            vm.model.note.contentProperty.propertyDataId = vm.model.property.propertyDataId;
            vm.model.note.contentProperty.propertyTypeAlias = vm.model.property.propertyTypeAlias;

            // Init controller
            function init() {
                // Get note types
                var noteTypesPromise = notelyResources.getNoteTypes();
                noteTypesPromise.then(function (data) {
                    vm.noteTypes = noteTypesBuilder.convert(data);
                    vm.model.note.type = vm.noteTypes[0];
                });

                // Get note states
                var noteStatesPromise = notelyResources.getNoteStates();
                noteStatesPromise.then(function (data) {
                    vm.noteStates = noteStatesBuilder.convert(data);
                    vm.model.note.state = vm.noteStates[0];
                });

                // Get active users to display in select
                var usersPromise = notelyResources.getUsers();
                usersPromise.then(function (data) {
                    vm.users = usersBuilder.convert(data);
                });
            };

            // Note type changed
            function noteTypeChanged() {
                if (!vm.model.note.type.canAssign)
                    vm.resetAssigndTo();

                // Reset state
                vm.model.note.state = vm.noteStates[0];
            };

            // Reset select
            function resetAssignedTo() {
                vm.model.note.assignedTo = null;
            };

            // Over usserPicker overlay dialog
            function openUserDialog() {
                var overlay = {
                    view: 'userpicker',
                    multiPicker: false,
                    show: true,
                    close: function (oldModel) {
                        vm.overlay.show = false;
                        vm.overlay = null;
                    },
                    submit: function (model) {
                        // Get first selected user
                        vm.model.note.assignedTo = usersBuilder.convert(model.selection[0]);
                        vm.overlay.show = false;
                        vm.overlay = null;
                    }
                };
                vm.overlay = overlay;
            }

            vm.init = init;
            vm.noteTypeChanged = noteTypeChanged;
            vm.resetAssignedTo = resetAssignedTo;
            vm.openUserDialog = openUserDialog;
        }

    ]);

    /*
     * @ngdoc Controller
     * @name Notely.Notes.EditController
     * 
     * @description
     */
    angular.module('notely').controller('Notely.Notes.EditController', [

        '$scope',
        'notelyResources',
        'notesBuilder',
        'noteTypesBuilder',
        'noteStatesBuilder',
        'usersBuilder',
        '$routeParams',

        function ($scope, notelyResources, notesBuilder, noteTypesBuilder, noteStatesBuilder, usersBuilder, $routeParams) {

            // Init controller
            $scope.init = function () {

                // Note model is already in scope when calling the overlay
                // So we don't need to call the api again to catch the note data
                // We also made a copy so that changes are not visible in the list untill we hit save!

                // Get note types
                var noteTypesPromise = notelyResources.getNoteTypes();
                noteTypesPromise.then(function (data) {
                    $scope.noteTypes = noteTypesBuilder.convert(data);
                });

                // Get note states
                var noteStatesPromise = notelyResources.getNoteStates();
                noteStatesPromise.then(function (data) {
                    $scope.noteStates = noteStatesBuilder.convert(data);
                });

                // Get active users to display in select
                var usersPromise = notelyResources.getUsers();
                usersPromise.then(function (data) {
                    $scope.users = usersBuilder.convert(data);
                });
            };

            // Note type changed
            $scope.noteTypeChanged = function () {
                if (!$scope.model.note.type.canAssign)
                    $scope.resetAssigndTo();

                // Reset state
                $scope.model.note.state = $scope.noteStates[0];
            };

            // Reset select
            $scope.resetAssigndTo = function () {
                $scope.model.note.assignedTo = null;
            };

        }

    ]);

    /*
     * @ngdoc Controller
     * @name Notely.Notes.DeleteController
     */
    angular.module('notely').controller('Notely.Notes.DeleteController', [

        '$scope',
        'notelyResources',
        'notesBuilder',

        function ($scope, notelyResources, notesBuilder) {

            // Init model object
            $scope.model = {};

            // Init controller
            $scope.init = function (noteId) {
                // Get note by id
                notelyResources.getNote(noteId).then(function (data) {
                    $scope.model.note = notesBuilder.convert(data);
                });
            };

            // Delete note
            $scope.deleteNote = function (noteId) {
                $scope.submit(noteId);
            };

        }

    ]);

    /*
     * @ngdoc Controller
     * @name Notely.Notes.CommentsController
     */
    angular.module('notely').controller('Notely.Notes.CommentsController', [

        '$scope',
        'notelyResources',
        'notesBuilder',
        'noteCommentsBuilder',
        '$filter',
        'userService',

        function ($scope, notelyResources, notesBuilder, noteCommentsBuilder, $filter, userService) {

            // Init model object
            $scope.model = {};

            $scope.selectComment = -1;
        
            $scope.currentUser = {
                id: -1,
                name: ''
            };

            // Init controller
            $scope.init = function (settings) {
                $scope.selectComment = settings.comment;

                // Get note by id
                notelyResources.getNote(settings.note).then(function (data) {
                    $scope.model.note = notesBuilder.convert(data);

                    // Get comments
                    notelyResources.getComments(settings.note).then(function (data) {
                        $scope.model.note.comments = noteCommentsBuilder.convert(data);
                    });
                });

                // Get current logged in user
                userService.getCurrentUser().then(function (user) {
                    $scope.currentUser.id = user.id;
                    $scope.currentUser.name = user.name;
                });
            };

            // Render comment description
            $scope.renderDescription = function (comment) {
                if (comment.logType === 'Info')
                    return '<strong>' + comment.user.name + '</strong> ' + comment.logComment;
                else
                    return $filter('setbold')(comment.logComment) + ' by ' + '<strong>' + comment.user.name + '</strong>';
            };

            // Add comment
            $scope.addComment = function () {
                if ($scope.newcomment && $scope.newcomment.length > 0) {
                    var noteComment = noteCommentsBuilder.createEmpty();
                    noteComment.logType = 'Info';
                    noteComment.logComment = $scope.newcomment;
                    noteComment.noteId = $scope.model.note.id;
                    noteComment.user = $scope.currentUser;

                    notelyResources.addComment(noteComment).then(function (data) {
                        $scope.newcomment = '';

                        // Reload comments
                        notelyResources.getComments(noteComment.noteId).then(function (data) {
                            $scope.model.note.comments = noteCommentsBuilder.convert(data);
                        });
                    });
                } else {
                    alert('Please enter a comment!');
                }
            }

            // Delete comment
            $scope.deleteComment = function (noteId, commentId) {
                notelyResources.deleteComment(commentId).then(function (data) {
                    // Reload comments
                    notelyResources.getComments(noteId).then(function (data) {
                        $scope.model.note.comments = noteCommentsBuilder.convert(data);
                    });
                });
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
     * @name Notely.Backoffice.NotesController
     * 
     */
    angular.module('notely').controller('Notely.Backoffice.NotesController', [

        '$scope',
        'notelyResources',
        'backOfficeNodesBuilder',
        'notesBuilder',
        'noteTypesBuilder',
        'noteStatesBuilder',
        'contentPropertyBuilder',
        'userService',
        '$routeParams',
        'dialogService',
        'notificationsService',
        'noteService',

        function ($scope, notelyResources, backOfficeNodesBuilder, notesBuilder, noteTypesBuilder, noteStatesBuilder,
            contentPropertyBuilder, userService, $routeParams, dialogService, notificationsService, noteService) {

            $scope.loaded = false;
            $scope.expanded = false;
            $scope.treeNode = 0; // 0 = all notes / 1 = my tasks
            $scope.options = {
                type: {},
                state: {},
                priority: "",
                hiding: false
            };
            $scope.noteTypes = [];
            $scope.noteStates = [];
            $scope.backOfficeDetails = [];
            $scope.visibleTabs = [];
            $scope.filteredNotes = [];

            // Init function
            $scope.init = function () {

                // Get current tree node
                $scope.treeNode = $routeParams.id;

                // Setup tabs
                $scope.visibleTabs.push({
                    id: 1,
                    label: 'Notes Listing'
                });

                // Get all the options
                $scope.loadOptions();

                // Get all the notes
                $scope.load();

            };

            // Load the filter options
            $scope.loadOptions = function () {

                // Get note types
                var noteTypesPromise = notelyResources.getNoteTypes();
                noteTypesPromise.then(function (data) {
                    $scope.noteTypes = noteTypesBuilder.convert(data);
                });

                // Get note states
                var noteStatesPromise = notelyResources.getNoteStates();
                noteStatesPromise.then(function (data) {
                    $scope.noteStates = noteStatesBuilder.convert(data);
                });

            };

            // Load the notes from the API
            $scope.load = function () {

                // Reset
                $scope.backOfficeDetails = [];

                // Check id of the route parameters:
                // Case 1: My tasks => so we need to get the current logged in user and get his tasks
                // Case 0: All notes
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
                $scope.loaded = false;

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

            // Render the note description field
            $scope.renderDescription = function (note) {
                return noteService.formatDescription(note);
            };

            // Changed filter option type
            $scope.changedType = function () {
                if ($scope.options.type.id > 0 && !$scope.options.type.canAssign)
                    $scope.options.state = {};
            };

            // Add a new note
            $scope.addNote = function (contentId, property) {

                // Extra check to see if notes limit is not reached
                if (property.notes.length < property.limit) {

                    // Create new contentProperty object
                    var _cp = contentPropertyBuilder.createEmpty();
                    _cp.contentId = contentId;
                    _cp.propertyDataId = property.id;
                    _cp.propertyTypeAlias = property.alias;

                    // Get overlay object of the service
                    var _overlay = noteService.getAddOverlay();
                    _overlay.property = _cp;
                    _overlay.close = function (oldModel) {
                        $scope.overlay.show = false;
                        $scope.overlay = null;
                    };
                    _overlay.submit = function (model) {
                        notelyResources.addNote(model.note).then(function () {
                            $scope.load();
                            $scope.expanded = false;
                        });

                        $scope.overlay.show = false;
                        $scope.overlay = null;

                        // Show notification
                        notificationsService.success("Note added", "Note is successfully added.");
                    };

                    $scope.overlay = _overlay;

                }

            };

            // Edit note
            $scope.editNote = function (note) {
                var _overlay = noteService.getEditOverlay();
                _overlay.note = angular.copy(note);
                _overlay.close = function (oldModel) {
                    $scope.overlay.show = false;
                    $scope.overlay = null;
                };
                _overlay.submit = function (model) {
                    notelyResources.updateNote(model.note).then(function () {
                        $scope.load();
                        $scope.expanded = false;
                    });

                    $scope.overlay.show = false;
                    $scope.overlay = null;

                    // Show notification
                    notificationsService.success("Note saved", "Note is successfully saved.");
                };

                $scope.overlay = _overlay;
            };

            // Delete a note
            $scope.deleteNote = function (noteId) {
                var _dialog = noteService.getDeleteDialog();
                _dialog.dialogData = noteId;
                _dialog.callback = function (data) {
                    notelyResources.deleteNote(data).then(function () {
                        $scope.load();

                        // Show notification
                        notificationsService.success("Note removed", "Note is successfully deleted.");
                    });
                };
                dialogService.open(_dialog);
            };

            // View comments from a note
            $scope.viewComments = function (noteId) {
                var _data = {
                    note: noteId
                };

                var _dialog = noteService.getViewCommentsDialog();
                _dialog.dialogData = _data;
                dialogService.open(_dialog);
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
        'noteTypesBuilder',
        'noteStatesBuilder',
        'notificationsService',
        'dialogService',

        function ($scope, notelyResources, noteTypesBuilder, noteStatesBuilder, notificationsService, dialogService) {

            $scope.loaded = false;
            $scope.visibleTabs = [];
            $scope.noteTypes = [];
            $scope.noteStates = [];

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
                var noteTypePromise = notelyResources.getNoteTypes();
                noteTypePromise.then(function (data) {
                    $scope.noteTypes = noteTypesBuilder.convert(data);
                });

                var noteStatePromise = notelyResources.getNoteStates();
                noteStatePromise.then(function (data) {
                    $scope.noteStates = noteStatesBuilder.convert(data);
                });
            };

            // Add note type
            $scope.addType = function () {

                $scope.overlay = {
                    view: "/App_Plugins/Notely/backoffice/notely/dialogs/notely.types.add.html",
                    title: "Add type",
                    show: true,
                    hideSubmitButton: false,
                    close: function (oldModel) {
                        $scope.overlay.show = false;
                        $scope.overlay = null;
                    },
                    submit: function (model) {
                        // Add note
                        notelyResources.addNoteType(model.type).then(function () {
                            $scope.load();
                        });

                        $scope.overlay.show = false;
                        $scope.overlay = null;

                        // Show notification
                        notificationsService.success("Type added", "Type is successfully added.");
                    }
                };

            };

            // Edit note type
            $scope.editType = function (noteType) {

                $scope.overlay = {
                    view: "/App_Plugins/Notely/backoffice/notely/dialogs/notely.types.edit.html",
                    title: "Edit type",
                    type: angular.copy(noteType),
                    show: true,
                    hideSubmitButton: false,
                    close: function (oldModel) {
                        $scope.overlay.show = false;
                        $scope.overlay = null;
                    },
                    submit: function (model) {
                        notelyResources.updateNoteType(model.type).then(function () {
                            $scope.load();
                        });

                        $scope.overlay.show = false;
                        $scope.overlay = null;

                        // Show notification
                        notificationsService.success("Type saved", "Type is successfully saved.");
                    }
                };

            };

            // Delete note type
            $scope.deleteType = function (noteTypeId) {
                dialogService.open({
                    template: '/App_Plugins/Notely/backoffice/notely/dialogs/notely.types.delete.html',
                    dialogData: noteTypeId,
                    callback: function (data) {
                        notelyResources.deleteNoteType(data).then(function () {
                            $scope.load();

                            // Show notification
                            notificationsService.success("Type removed", "Type is successfully deleted.");
                        });
                    }
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
                    label: 'Cleanup Notes'
                });

                $scope.loaded = true;

            };

            // Cleanup notes
            $scope.cleanup = function () {
                var cleanupPromise = notelyResources.cleanupNotes();
                cleanupPromise.then(function (data) {
                    notificationsService.success("Cleanup done", data + " notes were removed.");
                });
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
        'noteCommentsBuilder',
        '$filter',
        'noteService',
        'dialogService',

        function ($scope, notelyResources, noteCommentsBuilder, $filter, noteService, dialogService) {

            $scope.loaded = false;

            $scope.visibleTabs = [];
            $scope.comments = [];
            $scope.filteredComments = [];

            $scope.options = {
                type: ""
            };

            $scope.pagination = {
                current: 0,
                limit: 100,
                total: 1
            };

            // Init function
            $scope.init = function () {

                // Setup tabs
                $scope.visibleTabs.push({
                    id: 1,
                    label: 'Comments Listing'
                });

                // Load comments
                $scope.load();
            };

            // Load
            $scope.load = function () {
                // Get all comments
                notelyResources.getAllComments($scope.options.type).then(function (data) {
                    $scope.comments = noteCommentsBuilder.convert(data);

                    $scope.pagination.total = Math.ceil($scope.comments.length / $scope.pagination.limit);
                    $scope.pagination.current = 1;

                    $scope.loaded = true;
                });
            };

            // Watch logType changes and then reload the table
            $scope.$watch('options.type', function (term) {
                $scope.loaded = false;
                $scope.pagination.current = 0;
                $scope.load();
            });

            // Render comment description
            $scope.renderDescription = function (comment) {
                return $filter('setbold')(comment.logComment);
            };

            // Reset
            $scope.reset = function () {
                $scope.options.type = "";
            };
        
            // Reload
            $scope.reload = function () {
                $scope.loaded = false;

                $scope.reset();
                $scope.load();
            };

            // View comments from a note
            $scope.viewComments = function (noteId, commentId) {
                var _data = {
                    note: noteId,
                    comment: commentId
                };

                var _dialog = noteService.getViewCommentsDialog();
                _dialog.dialogData = _data;
                dialogService.open(_dialog);
            };

            // Pagination: previous
            $scope.prev = function (page) {
                $scope.pagination.current = page;
            };

            // Pagination: next
            $scope.next = function (page) {
                $scope.pagination.current = page;
            };

            // Pagination: goto page
            $scope.goto = function (page) {
                $scope.pagination.current = page;
            };
        }

    ]);

    /*
     * @ngdoc Controller
     * @name Notely.Types.AddController
     * 
     */
    angular.module('notely').controller('Notely.Types.AddController', [

        '$scope',
        'dialogService',
        'noteTypesBuilder',

        function ($scope, dialogService, noteTypesBuilder) {

            $scope.model.type = {};

            // Init
            $scope.init = function () {

                $scope.model.type = noteTypesBuilder.createEmpty();
                $scope.model.type.icon = "icon-info";

            };

            // Open icon picker dialog
            $scope.openIconPicker = function () {
                dialogService.iconPicker({
                    callback: function (data) {
                        $scope.model.type.icon = data;
                    }
                });
            };

        }

    ]);

    /*
     * @ngdoc Controller
     * @name Notely.Types.EditController
     * 
     */
    angular.module('notely').controller('Notely.Types.EditController', [

        '$scope',
        'dialogService',
        'noteTypesBuilder',

        function ($scope, dialogService, noteTypesBuilder) {

            // Init
            $scope.init = function () {

            };

            // Open icon picker dialog
            $scope.openIconPicker = function () {
                dialogService.iconPicker({
                    callback: function (data) {
                        $scope.model.type.icon = data;
                    }
                });
            };

        }

    ]);

    /*
     * @ngdoc Controller
     * @name Notely.Types.DeleteController
     */
    angular.module('notely').controller('Notely.Types.DeleteController', [

        '$scope',
        'notelyResources',
        'noteTypesBuilder',

        function ($scope, notelyResources, noteTypesBuilder) {

            // Init model object
            $scope.model = {};

            // Init controller
            $scope.init = function (noteTypeId) {
                // Get note by id
                notelyResources.getNoteType(noteTypeId).then(function (data) {
                    $scope.model.type = noteTypesBuilder.convert(data);
                });
            };

            // Delete note
            $scope.deleteNoteType = function (noteTypeId) {
                $scope.submit(noteTypeId);
            };

        }

    ]);

})();