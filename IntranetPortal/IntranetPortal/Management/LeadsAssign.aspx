<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LeadsAssign.aspx.vb" Inherits="IntranetPortal.LeadsAssign" MasterPageFile="~/Content.Master" %>

<asp:Content ID="header" runat="server" ContentPlaceHolderID="head">
    <script>
        angular
            .module("PortalApp")
            .controller('LeadsAssignController', ['$scope', '$http', function ($scope, $http) {
                var sp = $scope;
                sp.loadPortalStatus = loadPortalStatus;
                sp.importToPortal = importToPortal;
                sp.checkBatchStatus = checkBatchStatus;
                sp.filterByStatus = filterByStatus;
                sp.executeQuery = executeQuery;


                function loadPortalStatus() {
                    var url = '/api/dataservice/leadsportalstatus';
                    loadData(url, sp.bbles).then(function (response) {
                        sp.portalLeads = response.data;
                    });
                }

                function importToPortal() {
                    var url = '/api/dataservice/assignleads';
                    var batch = {
                        "BBLEs": sp.bbles,
                        "Tags": sp.tag,
                        "Employee": sp.Employee,
                        "Assigned": "test"
                    };

                    loadData(url, batch).then(function (response) {
                        debugger;
                        sp.batchId = response.data;
                        sp.newId = sp.batchId;
                    });
                }

                function checkBatchStatus() {
                    var url = '/api/dataservice/assignleads/' + sp.newId;
                    loadData(url, null, 'GET').then(function (response) {
                        sp.batchResult = response.data;
                    });
                }

                function filterByStatus(status) {
                    sp.filtedStatus = status;
                }

                function executeQuery() {
                    var url = '/api/dataservice/remotequery/';
                    loadData(url, JSON.stringify(sp.query)).then(function (response) {
                        sp.bbles = response.data;
                    });
                }

                function loadData(url, data, method) {
                    return $http({
                        method: method ? method : 'POST',
                        url: url,
                        data: data,
                        headers: {
                            'Content-Type': "application/json",
                            'apiKey': "6F43717752E839FD9034B6C426770488A7BA624E9E6D018E26D52451C7A4BCFE56338E92D621F826AC8B8228DDFEC4D7628AAC4917A06F3AE6CD56C978A691CA"
                        }
                    })
                }
            }]);


    </script>
</asp:Content>
<asp:Content ID="contentMaster" runat="server" ContentPlaceHolderID="MainContentPH">
    <div class="container" style="margin: 10px" ng-controller="LeadsAssignController">
        <ul class="nav nav-tabs">
            <li class="active">
                <a data-toggle="tab" href="#importLeads">Import Leads</a>
            </li>
            <li>
                <a data-toggle="tab" href="#checkStatus">Check Status</a>
            </li>
        </ul>
        <div class="tab-content">
            <div id="importLeads" class="tab-pane fade in active">
                <div class="form-group">
                    <label for="txtQuery">Remote Query</label>
                    <textarea ng-model="query" class="form-control" id="txtQuery" placeholder="Input Query" aria-describedby="queryHelp">
                    </textarea>
                    <small id="queryHelp" class="form-text text-muted">Input remote query here.</small>
                </div>
                <button type="button" class="btn btn-primary" ng-click="executeQuery()">Execute the Query</button>
                <div class="form-group">
                    <label for="txtBBLEs">BBLEs</label>
                    <textarea ng-model="bbles" ng-list="&#10;" ng-trim="false"
                        class="form-control" id="txtBBLEs" placeholder="Input BBLEs" aria-describedby="bblesHelp">
                    </textarea>
                    <small id="bblesHelp" class="form-text text-muted">Input your BBLEs here.</small>
                </div>
                <button type="button" class="btn btn-primary" ng-click="loadPortalStatus()">Load Status In Portal</button>
                <div ng-if="portalLeads">
                    <div class="form-group">
                        <label for="tblPortalStatus">Portal Status</label>
                        <table class="table" id="tblPortalStatus">
                            <thead>
                                <tr>
                                    <td></td>
                                    <td>BBLE</td>
                                    <td>Address</td>
                                    <td>Employee</td>
                                    <td>Status</td>
                                    <td>Team</td>
                                    <td>AssignDate</td>
                                    <td>LastUpdate</td>
                                </tr>
                            </thead>
                            <tbody>
                                <tr ng-repeat="m in portalLeads">
                                    <td>
                                        <input type="checkbox" class="form-control" /></td>
                                    <td>{{m.BBLE}}</td>
                                    <td>{{m.PropertyAddress}}</td>
                                    <td>{{m.EmployeeName}}</td>
                                    <td>{{m.StatusName}}</td>
                                    <td>{{m.Team}}</td>
                                    <td>{{m.AssignDate| date:'MM/dd/yyyy'}}</td>
                                    <td>{{m.LastUpdate| date:'MM/dd/yyyy'}}</td>
                                </tr>
                            </tbody>
                        </table>

                    </div>
                    <div class="form-group">
                        <label for="txtTag">Tag</label>
                        <input type="text" class="form-control" id="txtTag" ng-model="tag" placeholder="Input Batch tags" aria-describedby="tagsHelp" />
                        <small id="tagsHelp" class="form-text text-muted">Input batch tag here.</small>
                    </div>
                    <div class="form-group">
                        <label for="txtEmployee">Employee</label>
                        <input type="text" ng-model="employee" class="form-control" id="txtEmployee" placeholder="Input Employee" aria-describedby="employeeHelp" />
                        <small id="employeeHelp" class="form-text text-muted">Input your BBLEs here.</small>
                    </div>
                    <button type="button" class="btn btn-primary" ng-click="importToPortal()">Import to Portal</button>
                    <div class="alert alert-success" role="alert" ng-if="batchId">
                        <strong>Well done!</strong> You successfully import the batch to portal. The batch id is {{batchId}}.
                    </div>
                </div>
            </div>
            <div id="checkStatus" class="tab-pane fade">
                <div class="form-group">
                    <label for="txtBatchId">Batch Id</label>
                    <input class="form-control" id="txtBatchId" ng-model="newId" placeholder="Input batch id" aria-describedby="batchIdHelp" />
                    <small id="batchIdHelp" class="form-text text-muted">Input your batch id here.</small>
                </div>
                <button type="button" class="btn btn-primary" ng-click="checkBatchStatus()">Check Status</button>
                <div class="container" ng-if="batchResult">
                    <h2>Result</h2>
                    <a href="#" ng-click="filterByStatus(0)">Active <span class="badge">{{batchResult.Active}}</span></a><br>
                    <a href="#" ng-click="filterByStatus(1)">InProcess <span class="badge">{{batchResult.InProcess}}</span></a><br>
                    <a href="#" ng-click="filterByStatus(2)">Completed <span class="badge">{{batchResult.Completed}}</span></a>
                    <div>
                        <table class="table">
                            <thead>
                                <tr>
                                    <td>BBLE</td>
                                    <td>Employee</td>
                                    <td>Create Date</td>
                                    <td>Finish Date</td>
                                    <td>Status</td>
                                </tr>
                            </thead>
                            <tbody>
                                <tr ng-repeat="m in batchResult.Detail | filter: {Status: filtedStatus}">
                                    <td>{{m.BBLE}}</td>
                                    <td>{{m.EmployeeName}}</td>
                                    <td>{{m.CreateDate| date:'MM/dd/yyyy'}}</td>@
                                    <td>{{m.FinishDate| date:'MM/dd/yyyy'}}</td>
                                    <td>{{m.Status}}</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
