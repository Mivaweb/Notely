angular.module('notely').controller('Notely.PropertyEditors.DataTypePickerController', [

    '$scope',
    'Notely.notelyResources',

    function ($scope, notelyResources) {
        $scope.model.dataTypes = [];

        notelyResources.getDataTypes().then(function (data) { $scope.model.dataTypes = data });
    }

]);
