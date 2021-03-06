﻿(function() {

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

            var vm = this;
            vm.model = $scope.model;

            // Init controller
            function init() {

                // Note model is already in scope when calling the overlay
                // So we don't need to call the api again to catch the note data
                // We also made a copy so that changes are not visible in the list untill we hit save!

                // Get note types
                var noteTypesPromise = notelyResources.getNoteTypes();
                noteTypesPromise.then(function (data) {
                    vm.noteTypes = noteTypesBuilder.convert(data);
                });

                // Get note states
                var noteStatesPromise = notelyResources.getNoteStates();
                noteStatesPromise.then(function (data) {
                    vm.noteStates = noteStatesBuilder.convert(data);
                });
            };

            // Note type changed
            function noteTypeChanged() {
                if (!vm.model.note.type.canAssign)
                    vm.resetAssignedTo();

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
     * @name Notely.Notes.DeleteController
     */
    angular.module('notely').controller('Notely.Notes.DeleteController', [

        '$scope',
        'notelyResources',
        'notesBuilder',

        function ($scope, notelyResources, notesBuilder) {

            var vm = this;

            // Init model object
            vm.model = {};

            // Init controller
            function init(noteId) {
                // Get note by id
                notelyResources.getNote(noteId).then(function (data) {
                    vm.model.note = notesBuilder.convert(data);
                });
            };

            // Delete note
            function deleteNote(noteId) {
                $scope.submit(noteId);
            };

            // Close dialog
            function close() {
                $scope.close();
            }

            vm.init = init;
            vm.deleteNote = deleteNote;
            vm.close = close;

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

            var vm = this;

            // Init model object
            vm.model = {};
            vm.selectComment = -1;       
            vm.currentUser = {
                id: -1,
                name: ''
            };

            // Init controller
            function init(settings) {
                vm.selectComment = settings.comment;

                // Get note by id
                notelyResources.getNote(settings.note).then(function (data) {
                    vm.model.note = notesBuilder.convert(data);

                    // Get comments
                    notelyResources.getComments(settings.note).then(function (data) {
                        vm.model.note.comments = noteCommentsBuilder.convert(data);
                    });
                });

                // Get current logged in user
                userService.getCurrentUser().then(function (user) {
                    vm.currentUser.id = user.id;
                    vm.currentUser.name = user.name;
                });
            };

            // Render comment description
            function renderDescription(comment) {
                if (comment.logType === 'Info')
                    return '<strong>' + comment.user.name + '</strong> ' + comment.logComment;
                else
                    return $filter('setbold')(comment.logComment) + ' by ' + '<strong>' + comment.user.name + '</strong>';
            };

            // Add comment
            function addComment() {
                if (vm.newcomment && vm.newcomment.length > 0) {
                    var noteComment = noteCommentsBuilder.createEmpty();
                    noteComment.logType = 'Info';
                    noteComment.logComment = vm.newcomment;
                    noteComment.noteId = vm.model.note.id;
                    noteComment.user = vm.currentUser;

                    notelyResources.addComment(noteComment).then(function (data) {
                        vm.newcomment = '';

                        // Reload comments
                        notelyResources.getComments(noteComment.noteId).then(function (data) {
                            vm.model.note.comments = noteCommentsBuilder.convert(data);
                        });
                    });
                } else {
                    alert('Please enter a comment!');
                }
            }

            // Delete comment
            function deleteComment(noteId, commentId) {
                notelyResources.deleteComment(commentId).then(function (data) {
                    // Reload comments
                    notelyResources.getComments(noteId).then(function (data) {
                        vm.model.note.comments = noteCommentsBuilder.convert(data);
                    });
                });
            };

            vm.init = init;
            vm.renderDescription = renderDescription;
            vm.addComment = addComment;
            vm.deleteComment = deleteComment;

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

            var vm = this;
            vm.loaded = false;
            vm.expanded = false;
            vm.treeNode = 0; // 0 = all notes / 1 = my tasks
            vm.options = {
                type: {},
                state: {},
                priority: "",
                hiding: false
            };
            vm.noteTypes = [];
            vm.noteStates = [];
            vm.backOfficeDetails = [];
            vm.visibleTabs = [];

            // Init function
            function init() {

                // Get current tree node
                vm.treeNode = $routeParams.id;

                // Setup tabs
                vm.visibleTabs.push({
                    id: 1,
                    label: 'Notes Listing'
                });

                // Get all the options
                vm.loadOptions();

                // Get all the notes
                vm.load();

            };

            // Load the filter options
            function loadOptions() {

                // Get note types
                var noteTypesPromise = notelyResources.getNoteTypes();
                noteTypesPromise.then(function (data) {
                    vm.noteTypes = noteTypesBuilder.convert(data);
                });

                // Get note states
                var noteStatesPromise = notelyResources.getNoteStates();
                noteStatesPromise.then(function (data) {
                    vm.noteStates = noteStatesBuilder.convert(data);
                });

            };

            // Load the notes from the API
            function load() {

                // Reset
                vm.backOfficeDetails = [];

                // Check id of the route parameters:
                // Case 1: My tasks => so we need to get the current logged in user and get his tasks
                // Case 0: All notes
                var _userServicePromise = userService.getCurrentUser();
                var _user = -1;

                if (vm.treeNode == 1)
                {
                    _userServicePromise.then(function (user) {
                        _user = user.id;

                        loadDetails(_user);
                    });
                } else {
                    loadDetails(_user);
                }
            };

            // Reset
            function reset() {
                vm.options = {
                    type: {},
                    state: {},
                    hiding: false
                };
            };

            // Reload
            function reload() {
                vm.loaded = false;

                vm.loadOptions();
                vm.load();
                vm.reset();
                vm.expanded = false;
            };

            // Expand the content node details
            function expandContent(index) {
                vm.backOfficeDetails[index].showDetails = !vm.backOfficeDetails[index].showDetails;

                checkToggleState();
            };

            // Toggle content nodes
            function toggleContentNodes() {
                vm.expanded = !vm.expanded;

                angular.forEach(vm.backOfficeDetails, function (content) { content.showDetails = vm.expanded; });
            }

            // Render the note description field
            function renderDescription(note) {
                //return noteService.formatDescription(note);
                return note.type.canAssign && note.assignedTo ? note.assignedTo.name : '';
            };

            // Changed filter option type
            function changedType() {
                if (vm.options.type.id > 0 && !vm.options.type.canAssign)
                    vm.options.state = {};
            };

            // Add a new note
            function addNote(contentId, property) {

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
                        vm.overlay.show = false;
                        vm.overlay = null;
                    };
                    _overlay.submit = function (model) {
                        notelyResources.addNote(model.note).then(function () {
                            vm.load();
                            vm.expanded = false;
                        });

                        vm.overlay.show = false;
                        vm.overlay = null;

                        // Show notification
                        notificationsService.success("Note added", "Note is successfully added.");
                    };

                    vm.overlay = _overlay;

                }

            };

            // Edit note
            function editNote(note) {
                var _overlay = noteService.getEditOverlay();
                _overlay.note = angular.copy(note);
                _overlay.close = function (oldModel) {
                    vm.overlay.show = false;
                    vm.overlay = null;
                };
                _overlay.submit = function (model) {
                    notelyResources.updateNote(model.note).then(function () {
                        vm.load();
                        vm.expanded = false;
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
                        vm.load();

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
            vm.loadOptions = loadOptions;
            vm.load = load;
            vm.reset = reset;
            vm.reload = reload;
            vm.expandContent = expandContent;
            vm.toggleContentNodes = toggleContentNodes;
            vm.renderDescription = renderDescription;
            vm.changedType = changedType;
            vm.addNote = addNote;
            vm.editNote = editNote;
            vm.deleteNote = deleteNote;
            vm.viewComments = viewComments;

            function loadDetails(_user) {
                notelyResources.getUniqueContentNodes(_user).then(function (data) {

                    angular.forEach(data, function (content) {

                        var contentPromise = notelyResources.getContentNodeDetails(content, _user);
                        contentPromise.then(function (details) {
                            vm.backOfficeDetails.push(backOfficeNodesBuilder.convert(details));

                            vm.loaded = true;
                        });

                    });

                });
            }

            function checkToggleState() {
                var result = vm.backOfficeDetails.filter(function (detail) { return detail.showDetails == !vm.expanded; });
            
                if(result.length == vm.backOfficeDetails.length)
                    vm.expanded = !vm.expanded;
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

            var vm = this;
            vm.loaded = false;
            vm.visibleTabs = [];
            vm.noteTypes = [];
            vm.noteStates = [];

            // Init function
            function init() {

                // Setup tabs
                vm.visibleTabs.push({
                    id: 1,
                    label: 'Notely Settings'
                });

                vm.load();
                vm.loaded = true;

            };

            // Load data
            function load() {
                var noteTypePromise = notelyResources.getNoteTypes();
                noteTypePromise.then(function (data) {
                    vm.noteTypes = noteTypesBuilder.convert(data);
                });

                var noteStatePromise = notelyResources.getNoteStates();
                noteStatePromise.then(function (data) {
                    vm.noteStates = noteStatesBuilder.convert(data);
                });
            };

            // Add note type
            function addType() {

                vm.overlay = {
                    view: "/App_Plugins/Notely/backoffice/notely/dialogs/notely.types.add.html",
                    title: "Add type",
                    show: true,
                    hideSubmitButton: false,
                    close: function (oldModel) {
                        vm.overlay.show = false;
                        vm.overlay = null;
                    },
                    submit: function (model) {
                        // Add note
                        notelyResources.addNoteType(model.type).then(function () {
                            vm.load();
                        });

                        vm.overlay.show = false;
                        vm.overlay = null;

                        // Show notification
                        notificationsService.success("Type added", "Type is successfully added.");
                    }
                };

            };

            // Edit note type
            function editType(noteType) {

                vm.overlay = {
                    view: "/App_Plugins/Notely/backoffice/notely/dialogs/notely.types.edit.html",
                    title: "Edit type",
                    type: angular.copy(noteType),
                    show: true,
                    hideSubmitButton: false,
                    close: function (oldModel) {
                        vm.overlay.show = false;
                        vm.overlay = null;
                    },
                    submit: function (model) {
                        notelyResources.updateNoteType(model.type).then(function () {
                            vm.load();
                        });

                        vm.overlay.show = false;
                        vm.overlay = null;

                        // Show notification
                        notificationsService.success("Type saved", "Type is successfully saved.");
                    }
                };

            };

            // Delete note type
            function deleteType(noteTypeId) {
                dialogService.open({
                    template: '/App_Plugins/Notely/backoffice/notely/dialogs/notely.types.delete.html',
                    dialogData: noteTypeId,
                    callback: function (data) {
                        notelyResources.deleteNoteType(data).then(function () {
                            vm.load();

                            // Show notification
                            notificationsService.success("Type removed", "Type is successfully deleted.");
                        });
                    }
                });
            };

            vm.init = init;
            vm.load = load;
            vm.addType = addType;
            vm.editType = editType;
            vm.deleteType = deleteType;

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

            var vm = this;
            vm.loaded = false;

            vm.visibleTabs = [];

            // Init function
            function init() {

                // Setup tabs
                vm.visibleTabs.push({
                    id: 1,
                    label: 'Cleanup Notes'
                });

                vm.loaded = true;

            };

            // Cleanup notes
            function cleanup() {
                var cleanupPromise = notelyResources.cleanupNotes();
                cleanupPromise.then(function (data) {
                    notificationsService.success("Cleanup done", data + " notes were removed.");
                });
            };

            vm.init = init;
            vm.cleanup = cleanup;

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

            var vm = this;
            vm.loaded = false;

            vm.visibleTabs = [];
            vm.comments = [];
            vm.filteredComments = [];

            vm.options = {
                type: ""
            };

            vm.pagination = {
                current: 0,
                limit: 100,
                total: 1
            };

            // Init function
            function init() {

                // Setup tabs
                vm.visibleTabs.push({
                    id: 1,
                    label: 'Comments Listing'
                });

                // Load comments
                vm.load();
            };

            // Load
            function load() {
                // Get all comments
                notelyResources.getAllComments(vm.options.type).then(function (data) {
                    vm.comments = noteCommentsBuilder.convert(data);
                    vm.filteredComments = vm.comments;
                    vm.pagination.total = Math.ceil(vm.comments.length / vm.pagination.limit);
                    vm.pagination.current = 1;

                    vm.loaded = true;
                });
            };

            // Watch logType changes and then reload the table
            $scope.$watch('vm.options.type', function (term) {
                vm.pagination.current = 0;
                vm.load();
            });

            // Render comment description
            function renderDescription(comment) {
                return $filter('setbold')(comment.logComment);
            };

            // Reset
            function reset() {
                vm.options.type = "";
            };
        
            // Reload
            function reload() {
                vm.loaded = false;

                vm.reset();
                vm.load();
            };

            // View comments from a note
            function viewComments(noteId, commentId) {
                var _data = {
                    note: noteId,
                    comment: commentId
                };

                var _dialog = noteService.getViewCommentsDialog();
                _dialog.dialogData = _data;
                dialogService.open(_dialog);
            };

            // Pagination: previous
            function prev(page) {
                vm.pagination.current = page;
            };

            // Pagination: next
            function next(page) {
                vm.pagination.current = page;
            };

            // Pagination: goto page
            function goto(page) {
                vm.pagination.current = page;
            };

            vm.init = init;
            vm.load = load;
            vm.renderDescription = renderDescription;
            vm.reset = reset;
            vm.reload = reload;
            vm.viewComments = viewComments;
            vm.prev = prev;
            vm.next = next;
            vm.goto = goto;
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

            var vm = this;
            vm.model = $scope.model;
            vm.model.type = {};

            // Init
            function init() {

                vm.model.type = noteTypesBuilder.createEmpty();
                vm.model.type.icon = "icon-info";

            };

            // Open icon picker dialog
            function openIconPicker() {
                dialogService.iconPicker({
                    callback: function (data) {
                        vm.model.type.icon = data;
                    }
                });
            };

            vm.init = init;
            vm.openIconPicker = openIconPicker;

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

            var vm = this;
            vm.model = $scope.model;

            // Init
            function init() {

            };

            // Open icon picker dialog
            function openIconPicker() {
                dialogService.iconPicker({
                    callback: function (data) {
                        vm.model.type.icon = data;
                    }
                });
            };

            vm.init = init;
            vm.openIconPicker = openIconPicker;

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

            var vm = this;

            // Init model object
            vm.model = {};

            // Init controller
            function init(noteTypeId) {
                // Get note by id
                notelyResources.getNoteType(noteTypeId).then(function (data) {
                    vm.model.type = noteTypesBuilder.convert(data);
                });
            };

            // Delete note
            function deleteNoteType(noteTypeId) {
                $scope.submit(noteTypeId);
            };

            // Close
            function close() {
                $scope.close();
            }

            vm.init = init;
            vm.deleteNoteType = deleteNoteType;
            vm.close = close;

        }

    ]);

})();