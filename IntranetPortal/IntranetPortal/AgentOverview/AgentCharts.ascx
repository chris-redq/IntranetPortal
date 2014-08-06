﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AgentCharts.ascx.vb" Inherits="IntranetPortal.AgentCharts" %>
<%--use jquery 2.1.1 can't show the tooltips--%>
<%--<script src="/Scripts/jquery-2.1.1.min.js"></script>--%>
<script src="/Scripts/globalize/globalize.js"></script>
<script src="/Scripts/dx.chartjs.js"></script>
<div id="container" style="min-width: 900px; max-width: 1200px; height: 400px;"></div>
<div id="pieChart" style="min-width: 900px; max-width: 1200px; height: 400px;"></div>
<script type="text/javascript">


    //var dataSource = [
    //    {day: "Monday", oranges: 3},
    //    {day: "Tuesday", oranges: 2},
    //    {day: "Wednesday", oranges: 3},
    //    {day: "Thursday", oranges: 4},
    //    {day: "Friday", oranges: 6},
    //    {day: "Saturday", oranges: 11},
    //    {day: "Sunday", oranges: 4} 
    //];
    function change_chart_type(e) {
        //alert($(e).text());
        var type = $(e).text();
        var functions = {
            Line: show_line_chart,
            Bar: show_bar_chart,
            Pie: show_pie_chart,
        }
        //alert(functions[type])
        functions[type]();
    };
    function clear_chart(show_pie)
    {
        if (show_pie)
        {
            $('#pieChart').show();

            $('#container').hide();
        }
        else {
            $('#pieChart').hide();

            $('#container').show();
        }
       
    }
    function show_line_chart() {
        clear_chart()
        var dataSource = [
            { year: 1950, europe: 546, americas: 332, africa: 227 },
            { year: 1960, europe: 605, americas: 417, africa: 283 },
            { year: 1970, europe: 656, americas: 513, africa: 361 },
            { year: 1980, europe: 694, americas: 614, africa: 471 },
            { year: 1990, europe: 721, americas: 721, africa: 623 },
            { year: 2000, europe: 730, americas: 836, africa: 797 },
            { year: 2010, europe: 728, americas: 935, africa: 982 },
            { year: 2020, europe: 721, americas: 1027, africa: 1189 },
            { year: 2030, europe: 704, americas: 1110, africa: 1416 },
            { year: 2040, europe: 680, americas: 1178, africa: 1665 },
            { year: 2050, europe: 650, americas: 1231, africa: 1937 }
        ];

        $("#container").dxChart({
            dataSource: dataSource,
            commonSeriesSettings: {
                argumentField: "year"
            },
            series: [
                { valueField: "europe", name: "Europe" },
                { valueField: "americas", name: "Americas" },
                { valueField: "africa", name: "Africa" }
            ],
            argumentAxis: {
                grid: {
                    visible: true
                }
            },
            tooltip: {
                enabled: true
            },
            title: "Historic, Current and Future Population Trends",
            legend: {
                verticalAlignment: "bottom",
                horizontalAlignment: "center"
            },
            commonPaneSettings: {
                border: {
                    visible: true,
                    right: false
                }
            }
        });
    }
    function show_bar_chart(ds) {
        var dataFormSever = ds!=null?ds:$.parseJSON('<%=ChartSource()%>');
        clear_chart()
        
        var charts = $("#container").dxChart({
            dataSource: dataFormSever,

            series: {
                argumentField: "Name",
                valueField: "Count",
                name: "name",
                type: "bar",
                color: '#ffa500'
            },
            title: "In the last 6 months",
            legend: {
                verticalAlignment: "bottom",
                horizontalAlignment: "center"
            },
            tooltip: {
                enabled: true,
                customizeText: function () {

                    return this.valueText;
                }
            }
        ,
            pointClick: function (point) {
                this.select();
            }
        });     
    }

    function show_pie_chart() {
        clear_chart(true);
       
        var dataSource = [
          { country: "USA", medals: 110 },
          { country: "China", medals: 100 },
          { country: "Russia", medals: 72 },
          { country: "Britain", medals: 47 },
          { country: "Australia", medals: 46 },
          { country: "Germany", medals: 41 },
          { country: "France", medals: 40 },
          { country: "South Korea", medals: 31 }
        ];

        $("#pieChart").dxPieChart({
            dataSource: dataSource,
            title: "Olympic Medals in 2008",
            legend: {
                orientation: "horizontal",
                itemTextPosition: "right",
                horizontalAlignment: "right",
                verticalAlignment: "bottom",
                rowCount: 2
            },
            series: [{
                argumentField: "country",
                valueField: "medals",
                label: {
                    visible: true,
                    font: {
                        size: 16
                    },
                    connector: {
                        visible: true,
                        width: 0.5
                    },
                    position: "columns",
                    customizeText: function (arg) {
                        return arg.valueText + " ( " + arg.percentText + ")";
                    }
                }
            }]
        });
    }
    function LoadStatusBarChart(status)
    {
        callbackDsClient.PerformCallback(status);
    }

    function DataSourceLoadedComplete(s, e)
    {
        show_bar_chart( $.parseJSON(e.result) );
    }

    function ChangeDataSource()
    {
        var ds = eval('<% ChartSource()%>');        
        show_bar_chart(ds);
    }

   
            show_bar_chart();
</script>
<dx:ASPxCallback runat="server" ID="callbackDs" OnCallback="callbackDs_Callback" ClientInstanceName="callbackDsClient">
    <ClientSideEvents CallbackComplete="DataSourceLoadedComplete" />
</dx:ASPxCallback>