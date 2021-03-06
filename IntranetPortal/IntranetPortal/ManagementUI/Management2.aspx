﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Management2.aspx.vb" Inherits="IntranetPortal.ManagementUI2" MasterPageFile="~/Content.Master" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1">
<%--    <link rel="stylesheet" href="http://cdn3.devexpress.com/jslib/15.1.6/css/dx.common.css" />
    <link rel="stylesheet" href="http://cdn3.devexpress.com/jslib/15.1.6/css/dx.light.css" />--%>
    <style>
        .nofoucs:focus {
            border: none !important;
        }
    </style>

</asp:Content>
<asp:Content ContentPlaceHolderID="MainContentPH" runat="server">
    
<%--    <script src="/bower_components/globalize/lib/globalize.js"></script>--%>
    <%--<script src="http://cdn3.devexpress.com/jslib/15.1.6/js/dx.chartjs.js"></script>
    <script src="http://cdn3.devexpress.com/jslib/15.1.6/js/dx.webappjs.js"></script>--%>
    <div class="container-fluid">
        <%--Head--%>
        <div style="padding-top: 30px">
            <div style="font-size: 48px; color: #234b60">

                <div class="row">
                    <div class="col-md-4 ">
                        <div class="border_right" style="padding-right: 0px; font-weight: 300;">Management Summary</div>
                    </div>
                    <div class="col-md-2">
                        <table>
                            <tr>
                                <td>
                                    <span style="font-size: 30px" id="teams_link">Queens Team</span>
                                </td>
                                <td>
                                    <%--<i class="fa fa-caret-down" style="color: #2e2f31; font-size: 18px;" ></i>--%>
                                    <div id="dropDownMenu" class="nofoucs" style="margin-left: 10px; background: white; border: none; box-shadow: none" />
                                </td>
                            </tr>
                        </table>
                        <%--<i class="fa fa-caret-down" style="color: #2e2f31; font-size: 18px;" ></i>--%>
                    </div>
                    <div class="col-md-6">
                        <div>
                            <div style="display: inline-block">
                                <span class="magement_font">Total agents</span>
                                <span class="magement_font magement_number" id="total_agent_count">23</span>
                            </div>
                            <div style="display: inline-block; margin-left: 20px">
                                <span class="magement_font">total deals within last 30 days</span>
                                <span class="magement_font magement_number">241</span>
                            </div>
                            <div style="display: inline-block">
                                <span class="magement_number management_score">
                                    <span class="management_molecular ">87</span> <span class="management_denominator">/100</span>
                                </span>
                            </div>
                            <div style="display: inline-block" class="management_text">
                                <span>Effeciency Score</span><br />
                                <span>This Month</span>
                            </div>
                            <i class="fa fa-search-plus management_search_icon" style="margin-left: 10px;"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div style="margin-top: 40px;">
            <%--body Left--%>
            <%--<div class="row" style="display:none">
                <div class="col-md-7">
                    <div role="tabpanel" class="mag_tab_panel">

                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs" role="tablist">
                            <li role="presentation" class="active mag_tab"><a class="mag_tab_text" href="#phone_callsTab" aria-controls="phone_callsTab" role="tab" data-toggle="tab"><i class="fa fa-phone"></i>
                                <br />
                                Phone Calls</a></li>
                            <li role="presentation" class="mag_tab"><a class="mag_tab_text" href="#geomap" aria-controls="profile" role="tab" data-toggle="tab">
                                <i class="fa fa-map-marker"></i>
                                <br />
                                Geo map</a>

                            </li>
                            <li role="presentation" class="mag_tab"><a class="mag_tab_text" href="#profile" aria-controls="profile" role="tab" data-toggle="tab">
                                <i class="fa fa-sign-in"></i>
                                <br />
                                Door Knocks</a>

                            </li>
                            <li role="presentation" class="mag_tab"><a class="mag_tab_text" href="#deals_tab" aria-controls="deals_tab" role="tab" data-toggle="tab"><i class="fa fa-check-circle"></i>
                                <br />
                                Deals</a></li>
                            <li role="presentation" class="mag_tab"><a class="mag_tab_text" href="#messages" aria-controls="messages" role="tab" data-toggle="tab"><i class="fa fa-crosshairs"></i>
                                <br />
                                HR Analytics</a></li>

                        </ul>

                        <!-- Tab panes -->
                        <div class="tab-content" style="padding: 30px;">
                            <div role="tabpanel" class="tab-pane active" id="phone_callsTab">
                                <div class="mag_tab_input_group">

                                    <div class="row">
                                        <div class="col-md-3">
                                            <select class="form-control">
                                                <option>Last Month</option>
                                            </select>
                                        </div>
                                        <div class="col-md-3">
                                            <select class="form-control" id="selAgents">
                                                <option>All Agents</option>
                                                <option>Agents 1</option>
                                            </select>
                                        </div>
                                        <div class="col-md-2">
                                            <input type="button" value="Display" class="rand-button bg_color_blue rand-button-padding" onclick="LoadGrid()" />
                                        </div>
                                           <div class="col-md-2">
                                            <input type="button" value="Chart" class="rand-button bg_color_blue rand-button-padding" onclick="LoadPhoneBarChart()" />
                                           </div>
                                    </div>
                                </div>
                                <div style="margin: 30px 0; font-size: 24px; color: #234b60; display: none" id="divPhoneSummary">
                                    <span style="font-weight: 900" id="CallTotalCount">52,013 </span>Phone Calls<br />
                                    <span style="font-size: 14px; color: #77787b">January 1, 2015 - January 31, 2015</span>
                                </div>
                                <div style="font-size: 14px;">
                                    <div id="chartContainer" style="height: 450px; max-width: 1000px; width: 100%; margin: 0 auto; display: none"></div>
                                    <div id="gridContainer" style="height: 450px; max-width: 1000px; width: 100%; margin: 0 auto; display: none"></div>
                                </div>
                            </div>
                            <div role="tabpanel" class="tab-pane" id="profile">...</div>
                            <div role="tabpanel" class="tab-pane" id="geomap">
                                <iframe src="/Map/ZipMap.aspx" style="width: 100%; min-height: 600px"></iframe>
                            </div>
                            
                            <div role="tabpanel" class="tab-pane" id="deals_tab">deals_tab</div>
                            <div role="tabpanel" class="tab-pane" id="messages">...</div>

                        </div>

                    </div>
                </div>
                <div class="col-md-5">
                    <div style="padding-left: 10px">
                        <div>
                            <i class="fa fa-pie-chart report_head_button report_head_button_padding tooltip-examples"></i><span class="font_black">Status Of Leads</span>
                        </div>
                        <div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div id="container" class="containers" style="height: 250px; width: 100%;"></div>
                                    <div class="chart_text">
                                        In Process Leads: <span id="InProcessCount" class="font_black">0</span>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div id="current_leads" class="containers" style="height: 250px; width: 100%;"></div>
                                    <div class="chart_text">
                                        Current Leads: <span id="spanTotalLeads" class="font_black">0</span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div style="margin: 30px 0;">
                            <i class="fa fa-bar-chart report_head_button report_head_button_padding tooltip-examples"></i><span class="font_black">Monthly Leads Process</span>
                        </div>
                        <div>
                            <div id="leads_process_chart" class="containers" style="height: 250px; width: 100%;"></div>
                        </div>
                        <div style="margin: 30px 0;">
                            <i class="fa fa-line-chart report_head_button report_head_button_padding tooltip-examples"></i><span class="font_black">Compare Offices</span>
                        </div>
                        <div>
                            <div id="compare_offices_chart" class="containers" style="height: 200px; width: 100%;"></div>
                        </div>
                    </div>
                </div>
            </div>--%>
            <%-- New layout --%>
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-4">
                        <div class="mag_grid">
                            <div class="mag_grid_title">
                                <i class="fa fa-users management_denominator color_gray "></i><span>Agent Activity</span>
                            </div>
                            <div class="mag_grid_edit">
                                <select class="form-control">
                                    <option>In Process Leads</option>
                                </select>
                            </div>
                            <div class="mag_grid_chart">
                                <div id="current_leads" class="containers" style="height: 250px; width: 100%;"></div>
                                <div class="chart_text">
                                    Phone Call
                                </div>
                            </div>
                            <div>
                                <div class="row">
                                    <div class="col-md-8">&nbsp;</div>
                                    <div class="col-md-4">
                                        <button class="rand-button bg_color_blue rand-button-padding" type="button">All Charts <i class="fa fa-angle-right btn_arrow"></i></button>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="mag_grid">
                            <div class="mag_grid_title">
                                <i class="fa fa-pie-chart management_denominator color_gray "></i><span>Status Of Leads</span>
                            </div>
                            <div class="mag_grid_edit">
                                <select class="form-control">
                                    <option>Phone Call</option>
                                </select>
                            </div>
                            <div class="mag_grid_chart">
                                <div id="container" class="containers" style="height: 250px; width: 100%;"></div>
                                <div class="chart_text">
                                    &nbsp;
                                </div>
                            </div>
                            <div>
                                <div class="row">
                                    <div class="col-md-8">&nbsp;</div>
                                    <div class="col-md-4">
                                        <button class="rand-button bg_color_blue rand-button-padding" type="button">All Charts <i class="fa fa-angle-right btn_arrow"></i></button>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="mag_grid">
                            <div class="mag_grid_title">
                                <i class="fa fa-map-marker management_denominator color_gray "></i><span>Geo Leads</span>
                            </div>

                            <div class="mag_grid_chart">
                                <iframe src="/Map/ZipMap.aspx" style="width: 100%; min-height: 299px"></iframe>
                                <div class="chart_text">
                                    &nbsp;
                                </div>
                            </div>
                            <div>
                                <div class="row">
                                    <div class="col-md-7">&nbsp;</div>
                                    <div class="col-md-5">
                                        <button class="rand-button bg_color_blue rand-button-padding" type="button">Show Big Map <i class="fa fa-angle-right btn_arrow"></i></button>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <div class="row" style="margin-top: 30px">
                    <div class="col-md-4">
                        <div class="mag_grid">
                            <div class="mag_grid_title">
                                <i class="fa  fa-bar-char management_denominator color_gray "></i><span>Monthly Intake</span>
                            </div>
                            <div class="mag_grid_edit">
                                <select class="form-control">
                                    <option>Sample Chart</option>
                                </select>
                            </div>
                            <div class="mag_grid_chart">
                                <div id="monthly_intake" class="containers" style="height: 250px; width: 100%;"></div>
                                <div class="chart_text">
                                    Phone Call
                                </div>
                            </div>
                            <div>
                                <div class="row">
                                    <div class="col-md-8">&nbsp;</div>
                                    <div class="col-md-4">
                                        <button class="rand-button bg_color_blue rand-button-padding" type="button">All Charts <i class="fa fa-angle-right btn_arrow"></i></button>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="mag_grid">
                            <div class="mag_grid_title">
                                <i class="fa fa-line-chart management_denominator color_gray "></i><span>Compare Offices</span>
                            </div>
                            <div class="mag_grid_edit">
                                <select class="form-control">
                                    <option>Sample Chart</option>
                                </select>
                            </div>
                            <div class="mag_grid_chart">
                                <div id="leads_process_chart" class="containers" style="height: 250px; width: 100%;"></div>
                                <div class="chart_text">
                                    &nbsp;
                                </div>
                            </div>
                            <div>
                                <div class="row">
                                    <div class="col-md-8">&nbsp;</div>
                                    <div class="col-md-4">
                                        <button class="rand-button bg_color_blue rand-button-padding" type="button">All Charts <i class="fa fa-angle-right btn_arrow"></i></button>
                                    </div>

                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="mag_grid">
                            <div class="mag_grid_title">
                                <i class="fa fa-line-chart management_denominator color_gray "></i><span>Phone Logs 
                                    <button class="rand-button bg_color_blue rand-button-padding" type="button" onclick="ShowGrid()">Show</button>
                                </span>
                            </div>
                            <div class="mag_grid_edit">
                                <select class="form-control" id="selAgents">
                                    <option>Sample Chart</option>
                                </select>
                            </div>
                            <div class="mag_grid_chart ">
                                <div id="chartContainer" style="max-width: 1000px; width: 100%; margin: 0 auto; display: none"></div>
                            </div>
                            <div>
                                <div class="row">
                                    <div class="col-md-8">&nbsp;</div>
                                    <div class="col-md-4">
                                        <button class="rand-button bg_color_blue rand-button-padding" type="button">All Charts <i class="fa fa-angle-right btn_arrow"></i></button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="gridPopup">
        <div id="gridContainer" style="height: 450px; max-width: 1000px; width: 100%; margin: 0 auto; display: none"></div>
    </div>

    <script type="text/javascript">
        function BindAgents() {
            if (currentTeamInfo != null) {
                $("#selAgents").html("");
                $.each(currentTeamInfo.Users, function (key, value) {
                    $('#selAgents')
                    .append($("<option></option>")
                    .attr("value", value)
                    .text(value));
                });

                $("#selAgents").prepend("<option value='' selected='selected'>All</option>");
            }
        }

        function LoadPhoneBarChart() {
            $("#gridContainer").hide();
            $("#chartContainer").show();
            var localCallLogs = null;
            var customStore = new DevExpress.data.CustomStore({
                load: function (loadOptions) {
                    var d = $.Deferred();
                    $.getJSON('/wcfdataservices/portalReportservice.svc/UserReports').done(function (data) {
                        d.resolve(data, { totalCount: data.length });
                    });
                    return d.promise();
                }
            });

            var gridDataSourceConfiguration = { store: customStore };

            var logDataSource = new DevExpress.data.DataSource({
                store: customStore,
                paginate: false
            });

            $("#chartContainer").dxChart({
                dataSource: customStore,
                commonSeriesSettings: {
                    argumentField: "EmployeeName",
                    type: "stackedbar"
                },
                series: [
                     { valueField: "Inbound", name: "Inbound" },
                     { valueField: "Outbound", name: "Outbound" },
                     { valueField: "Internal", name: "Internal" },
                     { valueField: "Count", name: "Total", type: 'spline', color: 'blue' }
                ],
                argumentAxis: {
                    argumentType: 'string'
                },
                tooltip: {
                    enabled: true
                },
                legend: {
                    verticalAlignment: 'bottom',
                    horizontalAlignment: 'center'
                },
                onPointClick: function (info) {
                    var clickedPoint = info.target;
                    clickedPoint.isSelected() ? clickedPoint.clearSelection() : clickedPoint.select();
                },
                onPointSelectionChanged: function (info) {
                    var selectedPoint = info.target;
                    ShowEmployeeReport(selectedPoint.originalArgument);
                    $("#gridPopup").dxPopup({
                        showTitle: true,
                        title: 'Phone Logs - ' + selectedPoint.originalArgument,
                        showCloseButton: true
                    });
                    $("#gridPopup").dxPopup("instance").show();
                }
            });
        }

        function ShowGrid() {
            LoadGrid();
            $("#gridPopup").dxPopup({
                showTitle: true,
                title: 'All Phone Logs',
                showCloseButton: true
            });

            $("#gridPopup").dxPopup("instance").show();
        }

        function LoadGrid() {
            $("#gridContainer").show();
            //$("#chartContainer").hide();
            debugger;
            var agent = $("#selAgents").val();
            if (agent != undefined && agent != "") {
                ShowEmployeeReport(agent);
                return;
            }

            var localCallLogs = null;
            var customStore = new DevExpress.data.CustomStore({
                load: function (loadOptions) {
                    var d = $.Deferred();
                    $.getJSON('/wcfdataservices/portalReportservice.svc/UserReports').done(function (data) {
                        DevExpress.data.query(data).sum("Count").done(function (result) {
                            //$("#divPhoneSummary").show();
                            //$("#CallTotalCount").html(result);
                        });
                        d.resolve(data, { totalCount: data.length });
                    });
                    return d.promise();
                }
            });

            var gridDataSourceConfiguration = { store: customStore };

            var logDataSource = new DevExpress.data.DataSource({
                store: customStore,
                paginate: false
            });

            $(function () {
                var datagrid = $("#gridContainer").dxDataGrid({
                    dataSource: logDataSource,
                    showColumnLines: false,
                    showRowLines: true,
                    rowAlternationEnabled: true,
                    paging: { enabled: false },
                    columns: [{
                        dataField: "EmployeeName",
                        caption: "Name",
                        cellTemplate: function (container, options) {
                            if (options.value != null) {
                                var fieldHTML = '<a href="javascript:ShowEmployeeReport(\'' + options.value + '\')">' + options.value + "</a>"
                                container.html(fieldHTML);
                            }
                        }
                    }, "Inbound", {
                        dataField: "Outbound",
                        caption: "Outbound"
                    }, {
                        dataField: "Count",
                        caption: "Phone Calls"
                    }, {
                        dataField: "Duration",
                        caption: "Total Time"
                    }],
                    summary: {
                        totalItems: [{
                            column: 'Count',
                            summaryType: 'sum'
                        }]
                    }
                });
            });
        }

        function ShowEmployeeReport(empName) {
            $("#gridContainer").show();
            //$("#chartContainer").hide();
            var empCallLogs = null;
            var empCallLogsDs = new DevExpress.data.DataSource("/wcfdataservices/portalReportservice.svc/CallLog/" + empName);
            empCallLogsDs.load().done(function (result) {
                empCallLogs = result;

                $("#gridContainer").dxDataGrid({
                    dataSource: empCallLogs,
                    showColumnLines: false,
                    showRowLines: true,
                    rowAlternationEnabled: true,
                    paging: { enabled: false },
                    columns: [{
                        dataField: "DateTime",
                        caption: "Date",
                        dataType: "date",
                        format: "shortDate",
                        calculateGroupValue: function (rowData) {
                            var callDate = new Date(rowData.DateTime);
                            return callDate.toLocaleDateString();
                        }
                    }, {
                        dataField: "DateTime",
                        caption: "Time",
                        dataType: "date",
                        format: "shortTime",
                    },
                    "CallingNumber", "TimeZone", "Direction", "Duration", "DialedNumber", "Talk", "Answer"
                    ],
                    groupPanel: {
                        visible: true
                    },
                    allowColumnReordering: true,
                    grouping: {
                        autoExpandAll: true,
                    },
                    searchPanel: {
                        visible: true
                    },
                    summary: {
                        totalItems: [{
                            column: 'CallingNumber',
                            summaryType: 'count'
                        }, {
                            column: 'Duration',
                            summaryType: 'sum'
                        }]
                    }
                });
            });
        }
    </script>


    <script>

        var currentTeamInfo = null;
        function loadCharts(office) {

            //phone logs
            BindAgents();
            LoadPhoneBarChart();



            $("#monthly_intake").dxChart({
                legend: { visible: false },
                rotated: true,
                dataSource: [
                    { day: "Monday", oranges: 3 },
                    { day: "Tuesday", oranges: 2 },
                    { day: "Wednesday", oranges: 3 },
                    { day: "Thursday", oranges: 4 },
                    { day: "Friday", oranges: 6 },
                    { day: "Saturday", oranges: 11 },
                    { day: "Sunday", oranges: 4 }],

                series: {
                    argumentField: "day",
                    valueField: "oranges",

                    type: "bar",
                    color: '#ffa500'
                }
            });
            var dataSource = [
                  { language: "English", percent: 55.5 },
                  { language: "Chinese", percent: 4.0 },
                  { language: "Spanish", percent: 4.3 },
                  { language: "Japanese", percent: 4.9 },
                  { language: "Portuguese", percent: 2.3 },
                  { language: "German", percent: 5.6 },
                  { language: "French", percent: 3.8 },
                  { language: "Russian", percent: 6.3 },
                  { language: "Italian", percent: 1.6 },
                  { language: "Polish", percent: 1.8 }
            ];

            $("#team_hours_chart").dxPieChart({

                dataSource: dataSource,
                legend: {
                    horizontalAlignment: "center",
                    verticalAlignment: "bottom",

                    visible: false
                },

                series: [{
                    smallValuesGrouping: {
                        mode: "topN",
                        topCount: 3
                    },
                    type: "doughnut",
                    argumentField: "language",
                    valueField: "percent",
                    label: {
                        visible: true,
                        format: "fixedPoint",

                        connector: {
                            visible: true,
                            width: 1
                        }
                    }
                }]
            });

            $.getJSON('/WCFDataServices/PortalReportService.svc/LoadTeamInfo/' + office).done(function (data) {
                currentTeamInfo = data;

                //bind phone log agents name
                BindAgents();

                var total_agent_count = data.TeamAgentCount
                $("#total_agent_count").html(total_agent_count)
            });
            var dataSource = new DevExpress.data.DataSource({
                load: function (loadOptions) {
                    var d = $.Deferred();
                    $.getJSON('/WCFDataServices/PortalReportService.svc/LeadsInProcessReport/' + office).done(function (data) {
                        d.resolve(data);
                        var inporcessCount = data.reduce(function (a, b) {
                            return { Count: a.Count + b.Count };
                        });
                        $("#InProcessCount").html(inporcessCount.Count)
                    });
                    return d.promise();
                }
            });
            var leadstatusData = null;
            var dataSource2 = new DevExpress.data.DataSource("/wcfdataservices/portalReportservice.svc/LeadsStatusReport/" + office);


            dataSource2.load().done(function (result) {
                leadstatusData = result;
                DevExpress.data.query(leadstatusData).sum("Count").done(function (result) {
                    $("#spanTotalLeads").html(result);
                });
            });

            var option =
                {
                    dataSource: dataSource,
                    tooltip: {
                        enabled: true,

                        percentPrecision: 2,
                        customizeText: function () {

                            return this.argumentText + " - " + this.percentText;
                        }
                    },
                    legend: { visible: false },
                    series: [{
                        type: "doughnut",
                        argumentField: "Status",
                        valueField: "Count",
                        label: {
                            visible: true,

                            connector: {
                                visible: true
                            }
                        },

                    }],
                    palette: ['#a5bcd7', '#e97c82', '#da5859', '#f09777', '#fbc986', '#a5d7d0', '#a5bcd7']

                }
            $("#container").dxPieChart(option);
            option.dataSource = dataSource2;
            $("#current_leads").dxPieChart(option);



            var leadsProcess = [
                { state: "May", young: 6.7, middle: 28.6, older: 5.1 },
                { state: "Jun", young: 9.6, middle: 43.4, older: 9 },
                { state: "Jul", young: 13.5, middle: 49, older: 5.8 },
                { state: "Aug", young: 30, middle: 90.3, older: 14.5 }
            ];

            $("#leads_process_chart").dxChart({
                dataSource: leadsProcess,
                commonSeriesSettings: {
                    argumentField: "state",
                    type: "stackedBar"
                },
                series: [
                    { valueField: "young", name: "Closed" },
                    { valueField: "middle", name: "In Process" },
                    { valueField: "older", name: "Appointments" }
                ],
                legend: {
                    verticalAlignment: "bottom",
                    horizontalAlignment: "center",
                    itemTextPosition: 'top'
                },
                valueAxis: {
                    title: {
                        text: "millions"
                    },
                    position: "right"
                },

                tooltip: {
                    enabled: true,
                    location: "edge",
                    customizeText: function () {
                        return this.seriesName + " years: " + this.valueText;
                    }
                },
                palette: ['#a5bcd7', '#e97c82', '#da5859', '#f09777', '#fbc986', '#a5d7d0', '#a5bcd7']
            });
            var compateOfficeData = [
                { year: "November,2014", Queens: 190, Brooklyn: 180, Bronx: 100, Manhattan: 150 },
                { year: "December,2014", Queens: 263, Brooklyn: 280, Bronx: 230, Manhattan: 150 },
                { year: "January", Queens: 220, Brooklyn: 380, Bronx: 190, Manhattan: 150 },

            ];



            var chartOptions = {
                dataSource: compateOfficeData,
                commonSeriesSettings: {
                    type: "spline",
                    argumentField: "year"
                },
                commonAxisSettings: {
                    grid: {
                        visible: true
                    }
                },
                margin: {
                    bottom: 20
                },
                series: [
                    { valueField: "Queens", name: "Queens Office" },
                    { valueField: "Brooklyn", name: "Queens Office" },
                    { valueField: "Bronx", name: "Bronx Office" },
                    { valueField: "Manhattan", name: "Manhattan Office" }
                ],
                tooltip: {
                    enabled: true
                },
                legend: {
                    verticalAlignment: "bottom",
                    horizontalAlignment: "center",
                    itemTextPosition: 'top'
                },
                //title: "Architecture Share Over Time (Count)",
                commonPaneSettings: {
                    border: {
                        visible: true
                    }
                }
            };
            var chart = $("#compare_offices_chart").dxChart(chartOptions).dxChart("instance");
        }
        loadCharts("Queens");
    </script>
    <script>
        dropDownMenuData = <%= AllTameJson()%>
        //dropDownMenuData = [
        //    "Queens Team",
        //    "Bronx Team",
        //    "Rockaway Team"

        //];
        menuItemClicked = function (e) {

            DevExpress.ui.notify({ message: e.itemData + " Data Loaded", type: "success", displayTime: 2000 });
            $('#teams_link').html(e.itemData);
            //officeDropDown.option("buttonText", e.itemData );
            loadCharts(e.itemData.replace("Office", '').trim());
        };
        var officeDropDown = $("#dropDownMenu").dxDropDownMenu({
            dataSource: dropDownMenuData,
            itemClickAction: menuItemClicked,
            buttonIcon: 'arrowdown',
        }).dxDropDownMenu("instance");

    </script>
</asp:Content>

