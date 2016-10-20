﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Content.Master" CodeBehind="DocSearchList.aspx.vb" Inherits="IntranetPortal.DocSearchList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPH" runat="server">

    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.11.2/moment.min.js" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment-timezone/0.5.0/moment-timezone.min.js" type="text/javascript"></script>

    <style type="text/css">
        a.dx-link-MyIdealProp:hover {
            font-weight: 500;
            cursor: pointer;
        }

        .myRow:hover {
            background-color: #efefef;
        }

        iframe {
            border: none;
            width: 100%;
            height: 100%;
        }

        #xwrapper {
            float: left;
            height: 875px;
        }

        #preview {
            float: left;
            height: 875px;
        }

        #hideicon {
            visibility: hidden;
        }

            #hideicon:hover {
                cursor: pointer;
            }
    </style>

    <input type="text" style="display: none" />
    <div class="content">
        <div id="xwrapper">
            <div id="gridContainer" style="margin: 10px"></div>
        </div>

        <div id="preview" style="visibility: hidden">
            <iframe id="previewWindow"></iframe>
        </div>
    </div>
    <script>
        function resizeIframe(obj) {
            obj.style.height = obj.contentWindow.document.body.scrollHeight + 'px';
        }

        function GoToCase(CaseId) {
            var url = '/ViewLeadsInfo.aspx?id=' + CaseId;
            window.location.href = url;
        }

        var highlighter = (function () {
            var highlightedElement;

            return {
                setHighlight: function (el) {
                    this.clearHighlight();
                    highlightedElement = el;
                    $(el).css('background-color', '#c0c0c0');
                },
                clearHighlight: function () {
                    $(highlightedElement).css('background-color', '');
                    highlightedElement = undefined;
                }
            }
        })();
        var onSelectionChangedCallback = function (e) {
            //debugger;
            var bble = e.selectedRowKeys[0].BBLE || '';
            var status = e.selectedRowKeys[0].Status || 0;
            previewControl.showCaseInfo(bble, status)
        }
        previewControl = {
            showCaseInfo: function (CaseId, status) {
                if (status == 0) {
                    <% If HttpContext.Current.User.IsInRole("Underwriter") %>
                    var url = '/PopupControl/LeadTaxSearchRequest.aspx?mode=2&BBLE=' + CaseId
                    <% ELSE%>
                    var url = '/PopupControl/LeadTaxSearchRequest.aspx?mode=1&BBLE=' + CaseId
                    <% End IF%>
                    PortalUtility.ShowPopWindow("Doc Search - " + CaseId, url);
                } else {
                    $("#xwrapper").css("width", "50%");
                    $("#preview").css("visibility", "visible");
                    $("#preview").css("width", "50%");
                    <% If HttpContext.Current.User.IsInRole("Underwriter") %>
                    var url = '/PopupControl/LeadTaxSearchRequest.aspx?mode=3&BBLE=' + CaseId + '#/';
                    <% ELSE%>
                    var url = '/PopupControl/LeadTaxSearchRequest.aspx?mode=1&BBLE=' + CaseId + '#/';
                    <% End IF%>
                    $("#previewWindow").attr("src", url);
                    $("#hideicon").css("visibility", "visible");
                }

            },

            undo: function () {
                $("#preview").css("width", "0%");
                $("#preview").css("visibility", "hidden");
                $("#hideicon").css("visibility", "hidden");
                $("#xwrapper").css("width", "100%");
                $("#previewWindow").attr("src", "");
            }

        }

        $(document).ready(function () {
            var url = "/api/LeadInfoDocumentSearches";
            var that = this;
            $.getJSON(url).done(function (data) {
                var dataGrid = $("#gridContainer").dxDataGrid({
                    dataSource: data,
                    searchPanel: {
                        visible: true,
                        width: 240,
                        placeholder: "Search..."
                    },
                    headerFilter: {
                        visible: true
                    },
                    rowAlternationEnabled: true,
                    pager: {
                        showInfo: true,
                    },
                    paging: {
                        enabled: true,
                        //pageSize: 10
                    },
                    onRowPrepared: function (rowInfo) {
                        if (rowInfo.rowType != 'data')
                            return;
                        rowInfo.rowElement
                        .addClass('myRow');
                    },
                    onContentReady: function (e) {
                        var spanTotal = e.element.find('.spanTotal')[0];
                        if (spanTotal) {
                            $(spanTotal).html("Total Count: " + e.component.totalCount());
                        } else {
                            var panel = e.element.find('.dx-datagrid-pager');
                            if (!panel.find(".dx-pages").length) {
                                $("<span />").addClass("spanTotal").html("Total Count: " + e.component.totalCount()).appendTo(e.element);
                            } else {
                                panel.append($("<span />").addClass("spanTotal").html("Total Count: " + e.component.totalCount()))
                            }
                        }
                        highlightcallback(e);
                    },
                    onSelectionChanged: onSelectionChangedCallback,
                    selection: {
                        mode: 'single'
                    },
                    summary: {
                        groupItems: [{
                            column: "BBLE",
                            summaryType: "count",
                            displayFormat: "{0}",
                        }]
                    },
                    columns: [
                        {
                            dataField: 'BBLE',
                            visible: false
                        },
                        {
                            dataField: 'UpdateDate',
                            caption: 'UpdateDate',
                            sortIndex: 1,
                            sortOrder: 'desc',
                            visible: false
                        },
                        {
                            dataField: "CaseName",
                            width: 400,
                            caption: "Property Address",
                        }, {
                            dataField: "CreateBy",
                            caption: "UW Requested By"
                        }, {
                            dataField: "CreateDate",
                            caption: "UW Requested Date",
                            dataType: "date",
                            customizeText: function (cellInfo) {
                                if (!cellInfo.value)
                                    return ""

                                var dt = PortalUtility.FormatLocalDateTime(cellInfo.value);
                                if (dt)
                                    return moment(dt).format('MM/DD/YYYY');

                                return ""
                            }
                        }, {
                            dataField: "Status",
                            caption: "Search Status",

                            alignment: "left",
                            customizeText: function (cell) {
                                switch (cell.value) {
                                    case 1:
                                        return 'Completed';
                                    default:
                                        return 'New';

                                }
                            }
                        }, {
                            dataField: "CompletedBy",
                            caption: "Search Completed By"
                        }, {
                            dataField: "CompletedOn",
                            caption: "Search Completion Date",
                            dataType: "date",
                            customizeText: function (cellInfo) {
                                if (!cellInfo.value)
                                    return ""

                                var dt = PortalUtility.FormatLocalDateTime(cellInfo.value);
                                if (dt)
                                    return moment(dt).format('MM/DD/YYYY');

                                return ""
                            },
                            sortIndex: 0,
                            sortOrder: 'desc',
                        }, {
                            dataField: 'UnderwriteStatus',
                            caption: 'UW Decision',
                            alignment: "left",
                            customizeText: function (cell) {
                                switch (cell.value) {
                                    case 1:
                                        return 'Accepted';
                                    case 2:
                                        return 'Rejected';
                                    default:
                                        return 'Pending';

                                }
                            }
                        }, {
                            dataField: 'UnderwriteCompletedOn',
                            caption: 'UW Completion Date',
                            dataType: "date",
                            customizeText: function (cellInfo) {
                                if (!cellInfo.value)
                                    return ""

                                var dt = PortalUtility.FormatLocalDateTime(cellInfo.value);
                                if (dt)
                                    return moment(dt).format('MM/DD/YYYY');

                                return ""
                            }

                        }, {
                            caption: "Duration",
                            width: '80px',
                            allowSorting: true,
                            calculateCellValue: function (data) {
                                //debugger;
                                if (!data.UnderwriteCompletedOn || !data.CreateDate) {
                                    return Infinity;
                                }
                                return new Date(data.UnderwriteCompletedOn) - new Date(data.CreateDate);
                            },
                            customizeText: function (cellInfo) {
                                if (cellInfo.value == Infinity) return "";
                                return moment.duration(cellInfo.value).humanize();
                            }
                        }]
                }).dxDataGrid('instance');
                $(".dx-datagrid-header-panel").prepend($("<div id='uw-properties-title' style='margin-bottom: -35px'><label class='grid-title-icon' style='display: inline-block'>UW</label><span id='useFilterApplyButton' style='z-index: 999'></span></div>"))
                $(".dx-datagrid-header-panel").prepend($("<span id='hideicon' class='btn btn-blue pull-right' data-toggle='tooltip' data-placement='right' title='hide right panel' onclick='previewControl.undo()'><i class='fa fa-angle-double-right fa-lg'></i></span>"))
                var filterDataDelegate = function (data) {
                    previewControl.undo();
                    filterData(data.value);
                }

                var columns = ['CaseName', 'CreateBy', 'CreateDate', 'Status', 'CompletedBy', 'CompletedOn', 'UnderwriteStatus', 'UnderwriteCompletedOn', 'Duration'];

                var displayall = function () {
                    _.forEach(columns, function (v, i) {
                        dataGrid.columnOption(v, 'visible', true)
                    })

                }
                // use to hide some columns that not needed.
                var hidesome = function (arraylike) {
                    _.forEach(arraylike, function (v, i) {
                        dataGrid.columnOption(v, 'visible', false)
                    })
                }

                var filterData = function (data) {
                    dataGrid.clearFilter();
                    displayall();
                    switch (data) {
                        case 1:
                            dataGrid.filter(['Status', '=', '0']);
                            hidesome(['CompletedBy', 'CompletedOn', 'UnderwriteCompletedOn', 'Duration'])
                            break;
                        case 2:
                            dataGrid.filter(['Status', '=', '1']);
                            break;
                        case 3:
                            dataGrid.filter([['UnderwriteStatus', '=', '0'], ['Status', '=', '1']]);
                            hidesome(['UnderwriteCompletedOn', 'Duration'])
                            break;
                        case 4:
                            dataGrid.filter([['UnderwriteStatus', '=', '1'], ['Status', '=', '1']]);
                            break;
                        case 5:
                            dataGrid.filter([['UnderwriteStatus', '=', '2'], ['Status', '=', '1']]);
                    }
                }
                var filterBox = $("#useFilterApplyButton").dxSelectBox({
                    items: [{
                        key: 0,
                        name: "All"
                    }, {
                        key: 1,
                        name: "New Search"
                    }, {
                        key: 3,
                        name: "Pending Underwriting"
                    }, {
                        key: 2,
                        name: "Completed Search"
                    }, {
                        key: 4,
                        name: "Accepted Underwriting"
                    }, {
                        key: 5,
                        name: "Rejected Underwriting"
                    }],
                    valueExpr: "key",
                    displayExpr: "name",
                    width: '250',
                    onValueChanged: filterDataDelegate
                }).dxSelectBox('instance');
                var hashnum = parseInt(location.hash.split('/')[1]);
                var bble = location.hash.split('/')[2];
                if (hashnum) {
                    filterBox.option('value', hashnum);
                } else {
                    filterBox.option('value', 0);
                }
                //debugger;
                if (bble) {
                    previewControl.showCaseInfo(bble);
                }

                //high light column by refresh
                var highlightcallback = function (e) {
                    // debugger;
                    if (bble) {
                        var grid = e.element.dxDataGrid('instance');
                        var data = grid.option('dataSource');
                        var items = []
                        _.forEach(data, function (v, i) {
                            if (v.BBLE.trim() == bble.trim())
                                items.push(v)
                        });
                        if (items.length > 0) {
                            grid.selectRows(items, true);
                        }
                        bble = undefined;
                    }
                }


            });


        })

    </script>

</asp:Content>

