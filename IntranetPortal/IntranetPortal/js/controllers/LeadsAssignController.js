angular
    .module("PortalApp")
    .controller('LeadsAssignController', ['$scope', '$http', function ($scope, $http) {
        var sp = $scope;
        sp.loadPortalStatus = loadPortalStatus;
        

        function loadPortalStatus()
        {
            var url = '/api/dataservice/leadsportalstatus';
            postData(url, sp.bbles).then(function (response) {
                sp.portalLeads = response.data;
            });
        }

        function postData(url, data) {
            return $http({
                method: 'POST',
                url: localUrl,
                data: data,
                headers: {
                    'Content-Type': "application/json",
                    'apiKey': "6F43717752E839FD9034B6C426770488A7BA624E9E6D018E26D52451C7A4BCFE56338E92D621F826AC8B8228DDFEC4D7628AAC4917A06F3AE6CD56C978A691CA"
                }
            })
        };
}]);
