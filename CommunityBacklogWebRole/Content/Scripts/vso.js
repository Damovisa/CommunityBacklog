angular.module('vsoModule', [])
    .controller('VsoController', VsoController);

function VsoController($scope, $http) {
    $scope.showWorkItems = false;

    function getAccounts() {
        $http({
                method: 'Get',
                url: '/vso/accounts'
            })
            .success(function(data) { $scope.accounts = data; })
            .error(function() { $scope.errorMessage = 'Unexpected error retrieving accounts'; });
    }

    function getProjects(account) {
        $http({
                method: 'Get',
                url: '/vso/projects/' + account
            })
            .success(function(data) { $scope.projects = data; })
            .error(function() { $scope.errorMessage = 'Unexpected error retrieving projects'; });
    }

    function getWorkItemTypes(account, project) {
        /*
        $http({
                method: 'Get',
                url: '/vso/workitemtypes/' + account + '/' + project
            })
            .success(function(data) { $scope.witypes = data; })
            .error(function() { $scope.errorMessage = 'Unexpected error retrieving work item types'; });
        */
        $scope.witypes = [{ name: 'Product Backlog Item' }, { name: 'User Story' }, { name: 'Requirement' }];
    }

    // change projects dropdown when an account has been chosen
    $scope.$watch('model.account', function(newVal) {
        if (newVal) {
            getProjects(newVal.accountName);
        }
    });
    // change work item type dropdown when an account has been chosen
    $scope.$watch('model.project', function (newVal) {
        if (newVal) {
            getWorkItemTypes($scope.model.account.accountName, newVal.name);
        }
    });


    getAccounts();
}

