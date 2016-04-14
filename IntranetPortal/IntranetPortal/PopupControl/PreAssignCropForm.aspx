﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Content.Master" CodeBehind="PreAssignCropForm.aspx.vb" Inherits="IntranetPortal.PerAssignCropForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
      
        .wizard-content {
            min-height: 400px;
        }

        .online {
            width: 100% !important;
        }

        /*Theming options - change and everything updates*/
        /*don't use more decimals, as it makes browser round errors more likely, make heights unmatching
        -also watch using decimals at all at low wizardSize font sizes!*/
        .wizardbar {
            font-size: 18px;
            line-height: 1;
            display: inline-block;
            margin: 50px 0;
        }
        /*base item styles*/
        .wizardbar-item {
            display: inline-block;
            padding: 0.5em 0.8em;
            padding-left: 1.8em;
            text-decoration: none;
            transition: all .15s;
            /*default styles*/
            background-color: #76a9dd;
            color: rgba(255, 255, 255, 0.8);
            text-align: center;
            text-shadow: 1px 1px rgba(0, 0, 0, 0.2);
            position: relative;
            margin-right: 2px;
        }
            /*arrow styles*/
            .wizardbar-item:before,
            .wizardbar-item:after {
                content: "";
                height: 0;
                width: 0;
                border-width: 1em 0 1em 1em;
                border-style: solid;
                transition: all .15s;
                position: absolute;
                left: 100%;
                top: 0;
            }
            /*arrow overlapping left side of item*/
            .wizardbar-item:before {
                border-color: transparent transparent transparent white;
                left: 0;
            }
            /*arrow pointing out from right side of items*/
            .wizardbar-item:after {
                border-color: transparent transparent transparent #76a9dd;
                z-index: 1;
            }
        /*current item styles*/
        .current.wizardbar-item {
            background-color: #205081;
            color: white;
            cursor: default;
        }

            .current.wizardbar-item:after {
                border-color: transparent transparent transparent #205081;
            }
        /*hover styles*/
        .wizardbar-item:not(.current):hover {
            background-color: #3983ce;
        }

            .wizardbar-item:not(.current):hover:after {
                border-color: transparent transparent transparent #3983ce;
            }
        /*remove arrows from beginning and end*/
        .wizardbar-item:first-of-type:before,
        .wizardbar-item:last-of-type:after {
            border-color: transparent !important;
        }
        /*no inset arrow for first item*/
        .wizardbar-item:first-of-type {
            border-radius: 0.25em 0 0 0.25em;
            padding-left: 1.3em;
        }
        /*no protruding arrow for last item*/
        .wizardbar-item:last-of-type {
            border-radius: 0 0.25em 0.25em 0;
            padding-right: 1.3em;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPH" runat="server">
    <input type="hidden" id="preSignId" value='<%= Request.QueryString("preSignId")%>' />
    <input type="hidden" id="BBLE" value='<%= Request.QueryString("BBLE")%>' />
    <div ng-controller="perAssignCtrl" class="container">
        <div class="row">
            <div class="col-md-12" ng-hide="!preSignList">
                <div style="padding: 20px">
                    <h2 ng-if="role==null">Pre Deal Request List</h2>
                    <h2 ng-if="role=='finance'">Check Requests List</h2>
                    <div dx-data-grid="preSignRecordsGridOpt">
                        <div data-options="dxTemplate: {name: 'detail'}">
                            <div class="internal-grid-container">
                                <div>Checks :</div>
                                <div class="internal-grid" dx-data-grid="{
				                    dataSource: data.Checks,
				                    columnAutoWidth: true,
                                    columns: ['PaybleTo', {
                                        dataField: 'Amount',
                                        format: 'currency', dataType: 'number', precision: 2
                                        
                                    }, {
                                        dataField: 'Date',
                                        caption:'Check Date',
                                        dataType: 'date',
                                        format: 'shortDate'
                                    },{
                                        dataField: 'Description'  
                                    }]                
			                    }">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row" ng-if="preAssign.BBLE||model=='View'" id="preDealForm">
            <div style="max-width: 700px">
                <div id="wizard" <%=IIf(String.IsNullOrEmpty(Request.QueryString("popup")), "style='padding:20px';max-width:600px", "") %>>
                    <%--<div class="wizardbar">
                <a class="wizardbar-item {{step==$index+1?'current':'' }}" href="#" ng-repeat="s in steps|filter:{show:true}">{{s.title}} {{$index +1}}
                </a>
            </div>--%>
                    <div ng-show="step==1" class="wizard-content">
                        <section>
                            <div>
                                <h4 class="ss_form_title ">Pre Deal</h4>
                                <ul class="ss_form_box clearfix">
                                    <li class="ss_form_item online">
                                        <label class="ss_form_input_title">Property Address</label>
                                        <input class="ss_form_input" <%--ng-model="perAssignCtrl.Property_Address"--%> ng-model="preAssign.Title" disabled>
                                    </li>
                                    <%-- <li class="ss_form_item ">
                                <label class="ss_form_input_title "># of Parties</label>
                                <input class="ss_form_input " ng-model="perAssignCtrl.Numberof_Parties">
                            </li>--%>
                                    <%-- <li class="ss_form_item ">
                                <label class="ss_form_input_title ">Name Of parties</label>
                                <input class="ss_form_input " ng-model="perAssignCtrl.Name_Of_parties">
                            </li>--%>
                                    <li class="ss_form_item ">
                                        <label class="ss_form_input_title " ng-class="{ss_warning:!preAssign.ExpectedDate}">Expected Date of Signing</label>
                                        <input class="ss_form_input" ng-model="preAssign.ExpectedDate" ss-date required  />
                                    </li>
                                    <li class="ss_form_item">
                                        <label class="ss_form_input_title">Need do search</label>
                                        <pt-radio name="PreAssign_Needdosearch0" model="preAssign.NeedSearch" ></pt-radio>
                                    </li>
                                    <li class="ss_form_item">
                                        <label class="ss_form_input_title">Check request</label>
                                        <pt-radio name="PreAssign_Checkrequest0" model="preAssign.NeedCheck" ></pt-radio>
                                    </li>
                                    <li class="ss_form_item">
                                        <label class="ss_form_input_title">Manager </label>
                                        <input class="ss_form_input" value="<%=Page.User.Identity.Name %>" disabled />
                                    </li>
                                    <%-- <li class="ss_form_item ">
                                <label class="ss_form_input_title "># of checks</label>
                                <input class="ss_form_input " ng-model="perAssignCtrl.Number_of_checks">
                            </li>--%>
                                    <%--<li class="ss_form_item ">
                                <label class="ss_form_input_title ">Total Check Amount</label>
                                <input class="ss_form_input " money-mask ng-model="perAssignCtrl.Total_Check_Amount">
                            </li>--%>
                                    <div ng-show="preAssign.NeedCheck">
                                        <%--<li class="ss_form_item ">
                                <label class="ss_form_input_title ">Check Issued by</label>
                                <input class="ss_form_input" ng-model="preAssign.CheckIssuedBy" ng-show="CheckTotalAmount()<=100000" />
                                <input class="ss_form_input" ng-show="CheckTotalAmount()>10000" value="MyIdealProperty" disabled />
                               
                            </li>--%>
                                        <li class="ss_form_item">
                                            <label class="ss_form_input_title " ng-class="{ss_warning:CheckTotalAmount() > preAssign.DealAmount}">Total Amount paid for the deal</label>
                                            <input class="ss_form_input" ng-model="preAssign.DealAmount" money-mask />
                                        </li>
                                        <li class="ss_form_item">
                                            <label class="ss_form_input_title">Type of Check request</label>
                                            <select class="ss_form_input" ng-model="preAssign.CheckRequestData.Type" >

                                                <option>Short Sale</option>
                                                <option>Straight Sale</option>
                                                <option>Other</option>
                                            </select>
                                        </li>
                                    </div>
                                    <%--  <li class="ss_form_item ">
                                <label class="ss_form_input_title ">Name On Check</label>
                                <input class="ss_form_input " ng-model="perAssignCtrl.Name_On_Check">
                            </li>--%>
                                </ul>
                                <div ng-if="!preAssign.NeedSearch" class="alert alert-warning" role="alert">
                                    <strong>Warning!</strong> If you do not need a search please make sure you have all informations or you did a search already.
                                </div>
                            </div>
                            <div class="ss_form">
                                <h4 class="ss_form_title " ng-class="{ss_warning:preAssign.Parties.length<1 }">Parties <%--({{preAssign.Parties.length}})--%> <%--<i class="fa fa-plus-circle icon_btn" title="Add" ng-click="ensurePush('preAssign.Parties')">--%></i></h4>
                                <ul class="ss_form_box clearfix">
                                    <%--<li class="ss_form_item" ng-repeat="p in preAssign.Parties">
                                <label class="ss_form_input_title ">Party {{$index+1}} <i class="fa fa-times icon_btn" ng-click="arrayRemove(preAssign.Parties, $index)"></i></label>
                                <input class="ss_form_input " type="text" ng-model="p.Name" />
                            </li>--%>
                                    <li style="list-style: none">
                                        <div id="gridParties" dx-data-grid="partiesGridOptions"></div>
                                    </li>
                                </ul>
                            </div>

                            <div class="ss_form" ng-show="preAssign.NeedCheck">
                                <h4 class="ss_form_title " ng-class="{ss_warning:preAssign.CheckRequestData.Checks.length<1}">Checks <%--({{preAssign.CheckRequestData.Checks.length}})--%> <%--<i class="fa fa-plus-circle icon_btn" title="Add" ng-click="ensurePush('preAssign.CheckRequestData.Checks')"></i>--%></h4>
                                <ul class="ss_form_box clearfix">
                                    <%-- <li class="ss_form_item" ng-repeat="p in preAssign.CheckRequestData.Checks">
                                <label class="ss_form_input_title ">Check {{$index+1}} <i class="fa fa-times icon_btn" ng-click="arrayRemove(preAssign.CheckRequestData.Checks, $index)"></i></label>
                                <input class="ss_form_input " type="text" ng-model="p.Name" />
                            </li>--%>
                                    <li style="list-style: none">
                                        <div id="gridChecks" dx-data-grid="checkGridOptions"></div>
                                    </li>
                                </ul>
                            </div>
                        </section>
                    </div>
                    <%-- <div ng-show="step==2" class="wizard-content">
            </div>--%>

                    <div class="modal-footer">
                        <%--<button type="button" class="btn btn-default" ng-show="step>1" ng-click="PrevStep()">< Prev</button>--%>
                        <button type="button" class="btn btn-default" ng-click="RequestPreSign()" <%--ng-show="step==MaxStep"--%> ng-show="model!='View'">{{preAssign.Id ?'Update':'Submit'}} </button>
                        <%--<button type="button" class="btn btn-default" ng-show="step<MaxStep" ng-click="NextStep()">Next ></button>--%>
                        
                    </div>
                    <div class="alert alert-success" role="alert" ng-if="model=='View'">Your request submit succeeded on {{preAssign.CreateDate|date:'MM/dd/yyyy HH:mm'}} </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript" src="/js/PortalHttpFactory.js"></script>
    <script>

        var portalApp = angular.module('PortalApp');

        portalApp.controller('perAssignCtrl', function ($scope, ptCom, $firebaseObject, $http) {


            $scope.preAssign = { Parties: [], CheckRequestData: { Checks: [] } };
            var _BBLE = PortalUtility.QueryUrl().BBLE;
            var _model = PortalUtility.QueryUrl().model;
            var _role = PortalUtility.QueryUrl().role;
            $scope.role = _role;
            $scope.model = _model
            $scope.role = _role
            $scope.gridEdit = {
                mode: "batch",
                editEnabled: true,
                insertEnabled: true,
                removeEnabled: true
            }
            if (_model == 'List') {
                var checksListApi = $scope.role == 'finance' ? '/api/PreSign/CheckRequests' : '/api/presign/records'
                $http.get(checksListApi).success(function (data) {
                    $scope.preSignList = _.map(data, function (p) { p.ChecksTotal = _.sum(p.Checks, 'Amount'); return p; });
                });
            }
            if (_model == 'View') {
                setTimeout(function () {
                    $("#preDealForm input").prop("disabled", true);
                    $("#preDealForm select").prop("disabled", true);
                },1000)
                
                $scope.gridEdit = {}
                var _Id = PortalUtility.QueryUrl().Id;

                var preSignRespose = $.ajax({
                    url: '/api/PreSign/' + _Id,
                    async: false
                })
               
                var message = PortalHttp.BuildAjaxErrorMessage(preSignRespose);
                if (message) {
                    AngularRoot.alert(message)
                } else {
                    $scope.preAssign = JSON.parse(preSignRespose.responseText)
                    _BBLE = $scope.preAssign.BBLE
                   
                }

            }
            $scope.preAssign.BBLE = _BBLE
            $scope.preAssign.CheckRequestData.BBLE = $scope.preAssign.BBLE;
            $scope.preAssign.NeedCheck = true;
            $scope.steps = [
              { title: "Pre Sign", show: true },
              { title: "Parties", show: $scope.preAssign.NeedCheck },
              { title: "Checks", show: $scope.preAssign.NeedCheck },
              { title: "Finish", show: true },
            ];
            $scope.steps[1].show
            $scope.arrayRemove = ptCom.arrayRemove;
            $scope.step = 1
            $scope.MaxStep = $scope.steps.length;
            $scope.NextStep = function () {
                $scope.step++;
            }
            $scope.PrevStep = function () {
                $scope.step--;
            }

            $scope.initByBBLE = function (BBLE) {
                $http.get('/api/Leads/LeadsInfo/' + BBLE).success(function (data) {
                    $scope.preAssign.Title = data.PropertyAddress
                });
                if ($scope.model != 'View')
                {
                    $scope.preAssign.CheckRequestData.Type = "Short Sale";
                    $scope.preAssign.DealAmount = 0;
                    $scope.preAssign.NeedSearch = true;
                }
              
            }

            if (_BBLE) {
                $scope.initByBBLE(_BBLE);
            }
            $scope.init = function (preSignId) {

                $http.get('/api/PreSign/' + preSignId).success(function (data) {
                    $scope.preAssign = data;
                    $scope.preAssign.Parties = $scope.preAssign.Parties || [];
                });

            }
            $scope.Save = function () {
                var selfData = $scope.preAssign
                if ($scope.preAssign.Id) {

                    $http.put('/api/PreSign/' + $scope.preAssign.Id, JSON.stringify($scope.preAssign)).success(function () {
                        AngularRoot.alert("Save success!");

                    });
                } else {

                    if (!selfData.ExpectedDate) {
                        AngularRoot.alert("Please fill expected date !");
                        return;
                    }
                    if ((!$scope.preAssign.Parties) || $scope.preAssign.Parties.length < 1) {
                        AngularRoot.alert("Please fill at least one Party !");
                        return;
                    }

                    if ($scope.preAssign.NeedCheck && $scope.preAssign.CheckRequestData.Checks.length < 1) {
                        AngularRoot.alert("If need request check please fill at least one check!");
                        return;
                    }

                    if ($scope.CheckTotalAmount() > $scope.preAssign.DealAmount) {
                        AngularRoot.alert("The check's total amount must less than the deal amount, Please correct! ");
                        return;
                    }
                    if (!$scope.preAssign.NeedCheck) {
                        $scope.preAssign.Parties = null;
                        $scope.preAssign.CheckRequestData = null
                    }
                    $http.post('/api/PreSign', JSON.stringify($scope.preAssign)).success(function (data) {
                        AngularRoot.alert("Submit success !");
                        $scope.preAssign = data;
                        window.location.href = '/popupControl/preAssignCropForm.aspx?model=View&Id=' + data.Id
                    });
                }

            }

            var _preSignId = $("#preSignId").val();
            if (_preSignId) {
                $scope.init(_preSignId)
            }

            //var ref = new Firebase("https://sdatabasetest.firebaseio.com/qqq");
            //var syncObject = $firebaseObject(ref);
            //syncObject.$bindTo($scope, "Test");

            $scope.onSelectedChanged = function (e) {
                var request = e.selectedRowsData[0];
                PortalUtility.OpenWindow('/PopupControl/PreAssignCropForm.aspx?model=View&Id=' + request.Id, 'Pre Sign ' + request.BBLE, 800, 900);
            }
            $scope.preSignRecordsGridOpt = {
                onSelectionChanged: $scope.onSelectedChanged,
                selection: { mode: 'single' },
                bindingOptions:
                {
                    dataSource: 'preSignList'
                },
                headerFilter: {
                    visible: true
                },
                searchPanel: {
                    visible: true,
                    width: 250
                },
                paging: {
                    pageSize: 10
                },
                columnAutoWidth: true,
                columns: [{ dataField: 'Title', caption: 'Address' }, { dataField: 'CreateBy', caption: 'Request By' },
                    { dataField: 'CreateDate', caption: 'Request Date', dataType: 'date' },
                    { dataField: 'ExpectedDate', caption: 'Expected Date Of Sign', dataType: 'date' },
                    { dataField: 'DealAmount', format: 'currency', dataType: 'number', precision: 2 },
                    { dataField: 'NeedSearch', caption: 'Search Request' }, ],
                wordWrapEnabled: true
            }
            if (_role == 'finance') {
                $scope.preSignRecordsGridOpt.masterDetail = {
                    enabled: true,
                    template: "detail"
                }
                $scope.preSignRecordsGridOpt.selection  = null;
                $scope.preSignRecordsGridOpt.columns = [
                    { dataField: 'PropertyAddress', caption: 'Address' }, { dataField: 'RequestBy', caption: 'Request By' },
                    { dataField: 'Type', caption: 'Request Type' },
                    { dataField: 'RequestDate', caption: 'Request Date', dataType: 'date' },
                    { dataField: 'CheckAmount', format: 'currency', dataType: 'number', precision: 2 },
                ]
            }

            $scope.partiesGridOptions = {
                bindingOptions:
                    {
                        dataSource: 'preAssign.Parties'
                    },
                //dataSource: $scope.preAssign.CheckRequestData.Checks,
                paging: {
                    pageSize: 10
                },
                editing: $scope.gridEdit,
                pager: {
                    showPageSizeSelector: true,
                    allowedPageSizes: [5, 10, 20],
                    showInfo: true
                },
                columns: [{ dataField: "Name", validationRules: [{ type: "required" }] }],
                summary: {
                    totalItems: [{
                        column: "Name",
                        summaryType: "count"
                    }]
                }
            }

            $scope.CheckTotalAmount = function () {
                return _.sum($scope.preAssign.CheckRequestData.Checks, 'Amount');
            }
            $scope.checkGridOptions = {
                bindingOptions:
                    {
                        dataSource: 'preAssign.CheckRequestData.Checks'
                    },
                //dataSource: $scope.preAssign.CheckRequestData.Checks,
                paging: {
                    pageSize: 10
                },

                editing: $scope.gridEdit,
                pager: {
                    showPageSizeSelector: true,
                    allowedPageSizes: [5, 10, 20],
                    showInfo: true
                },
                wordWrapEnabled: true,
                columns: [{ dataField: "PaybleTo", validationRules: [{ type: "required" }] },
                    { dataField: 'Amount', dataType: 'number', format: 'currency', precision: 2, validationRules: [{ type: "required" }] },
                    { dataField: 'Date', dataType: 'date', validationRules: [{ type: "required" }] },
                    { dataField: 'Description', validationRules: [{ type: "required" }] },
                ],
                summary: {
                    totalItems: [{
                        column: "Name",
                        summaryType: "count"
                    },
                    {
                        column: "Amount",
                        summaryType: "sum",
                        valueFormat: "currency",
                        precision: 2
                    }]
                }
            };
            $scope.ensurePush = function (modelName, data) { ptCom.ensurePush($scope, modelName, data); }

            $scope.RequestPreSign = function () {
                $scope.Save();
                if (window.parent && window.parent.preAssignPopopClient) {
                    var popup = window.parent.preAssignPopopClient
                    popup.Hide();


                }
            }
        });
    </script>

</asp:Content>
