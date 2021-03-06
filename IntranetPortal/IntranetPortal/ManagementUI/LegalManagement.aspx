﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Content.Master" CodeBehind="LegalManagement.aspx.vb" Inherits="IntranetPortal.LegalManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css">
    <style type="text/css">
        .ui-helper-hidden-accessible {
            display: none;
        }

        .ui-tooltip {
            font-size: 12px;
        }

        .ui-helper-reset {
            font-size: 14px;
        }
        .ui-draggable {
            cursor:move;
        }

        .ui-draggable:hover{
            background-color:#efefef;
        }
        .drag-helper{
            background-color:#efefef;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPH" runat="server">
    <div style="position: absolute; top: 0px; bottom: 0px; width: 100%; overflow-y: scroll; align-content: center; padding: 10px">
        <input type="text" class="active" id="txt" hidden="hidden" />
        <% If Not Request.QueryString("type") IsNot Nothing %>
        <h3 style="margin-bottom: 10px">Legal Settings</h3>
        <div id="tabs">
            <ul>
                <li><a href="#tabs-1">Foreclosure Status</a></li>
                <li><a href="#tabs-2">Others</a></li>
            </ul>

            <div id="tabs-1">
                <script type="text/html" id="SaveButtonTemplate">
                    <div class="dx-edit-button dx-datagrid-save-button dx-widget dx-button-has-icon dx-button dx-button-normal" title="Save changes" onclick="DataStatus.Save()" role="button" aria-label="edit-button-save" aria-disabled="true">
                        <div class="dx-button-content"><i class="dx-icon dx-icon-edit-button-save"></i></div>
                    </div>
                </script>

                <div id="gridStatus" style="width: 700px"></div>

                <script>

                    var DataStatus = {
                        rootUrl: "/api/DataStatus",
                        category: null,
                        store: null,
                        dragable: false,
                        dataChanged: false,
                        loadCategory: function (category) {
                            this.category = category;
                            var url = this.rootUrl + "/" + this.category;
                            var ds = this;
                            $.getJSON(url).done(function (data) {
                                ds.store = new DevExpress.data.ArrayStore({
                                    key: 'Status',
                                    data: data
                                });

                                $("#gridStatus").dxDataGrid({
                                    dataSource: ds.store,
                                    paging: {
                                        enabled: false
                                    },
                                    editing: {
                                        editMode: "row",
                                        editEnabled: true,
                                        removeEnabled: false,
                                        insertEnabled: true
                                    },
                                    columns: [{
                                        dataField: "Name",
                                        caption: "Name",
                                        width: 230
                                    }, {
                                        dataField: "DisplayOrder",
                                        allowEditing: false,
                                        sortIndex: 1,
                                        sortOrder: "asc",
                                        visible: false
                                    }, {
                                        dataField: "Description",
                                    }, {
                                        dataField: "Active",
                                        sortIndex: 0,
                                        sortOrder: "desc",
                                        width: 80
                                    }, {
                                        dataField: "Status",
                                        dataType: "number",
                                        visible: false,
                                    }],
                                    onRowPrepared: function (rowInfo) {                                      
                                        if (rowInfo.rowType != 'data')
                                            return;
                                        rowInfo.rowElement
                                        .addClass('myRow')
                                        .data('keyValue', rowInfo.key);
                                    },
                                    onEditingStart: function (e) {
                                        console.log("Editing start");
                                        ds.dragable = true;
                                    },
                                    onRowUpdated:function(e){
                                        ds.Save();
                                    },
                                    onRowInserted:function(e){
                                        ds.Save();
                                    },
                                    onContentReady: function (e) {
                                        console.log("content ready");
                                        if (!ds.dragable)
                                            ds.initDragging(e.element);

                                        //var panel = e.element.find('.dx-datagrid-header-panel');
                                        //if (panel.find(".dx-datagrid-save-button").length) {
                                        //} else {
                                        //    var saveButton = $("#SaveButtonTemplate").html();
                                        //    panel.append(saveButton);
                                        //}

                                        ds.dragable = false;
                                    },
                                    onInitNewRow: function (info) {
                                        info.data.Status = -1;
                                        info.data.Category = ds.category;
                                    }
                                });                                
                            });
                        },

                        initDragging: function ($gridElement) {
                            var ds = this;
                          
                            $gridElement.find('.myRow').draggable({
                                helper: 'clone',
                                start: function (event, ui) {
                                    var $originalRow = $(this),
                                        $clonedRow = ui.helper;
                                    var $originalRowCells = $originalRow.children(),
                                        $clonedRowCells = $clonedRow.children();
                                    for (var i = 0; i < $originalRowCells.length; i++)
                                        $($clonedRowCells.get(i)).width($($originalRowCells.get(i)).width());
                                    $clonedRow.width($originalRow.width()).addClass('drag-helper');
                                }
                            });

                            $gridElement.find('.myRow').droppable({
                                drop: function (event, ui) {
                                    var draggingRowKey = ui.draggable.data('keyValue');
                                    var targetRowKey = $(this).data('keyValue');
                                    var draggingIndex = null,
                                        targetIndex = null;
                                    ds.store.byKey(draggingRowKey).done(function (item) {
                                        draggingIndex = item.DisplayOrder;
                                    });
                                    ds.store.byKey(targetRowKey).done(function (item) {
                                        targetIndex = item.DisplayOrder;
                                    });
                                    var draggingDirection = (targetIndex < draggingIndex) ? 1 : -1;
                                    var dataItems = null
                                    ds.store.load().done(function (data) {
                                        dataItems = data;
                                    });
                                    for (var dataIndex = 0; dataIndex < dataItems.length; dataIndex++) {
                                        if ((dataItems[dataIndex].DisplayOrder > Math.min(targetIndex, draggingIndex)) && (dataItems[dataIndex].DisplayOrder < Math.max(targetIndex, draggingIndex))) {
                                            dataItems[dataIndex].DisplayOrder += draggingDirection;
                                        }
                                    }
                                    ds.store.update(draggingRowKey, { DisplayOrder: targetIndex });
                                    ds.store.update(targetRowKey, { DisplayOrder: targetIndex + draggingDirection });
                                    $gridElement.dxDataGrid('instance').refresh();
                                    ds.Save();
                                }
                            });
                        },

                        Save: function () {
                            var url = "/api/DataStatus/Save";
                            var data = JSON.stringify(this.store.createQuery().toArray());
                            var ds = this;
                            $.ajax({
                                type: "POST",
                                url: url,
                                data: data,
                                success: function (data) {
                                    //alert("Success.");
                                    ds.store = new DevExpress.data.ArrayStore({
                                        key: 'Status',
                                        data: data
                                    });
                                    $("#gridStatus").dxDataGrid('instance').option('dataSource', ds.store);
                                },
                                contentType: "application/json"
                            });
                        }
                    }

                </script>
            </div>

            <div id="tabs-2">
                Comming soon..
            </div>

        </div>
        <script>
            $(function () {
                $("#tabs").tabs();
            });

            $(function () {
                DataStatus.loadCategory('<%= IntranetPortal.Data.LegalCase.ForeclosureStatusCategory%>');
            });
        </script>
        <% Else %>
        <h3>Data Status Management</h3>
        <div id="selectBox" style="width: 700px"></div>
        <script>
            var categoryData = [
                { "Name": "LegalCaseStatus" },
                { "Name": "LegalFCDataStatus" }
            ];
            $("#selectBox").dxSelectBox({
                dataSource: new DevExpress.data.DataSource("/api/DataStatus/Categories"),
                onValueChanged: function (e) {
                    DataStatus.loadCategory(e.value);
                }
            });
        </script>
        <% end if %>
    </div>
</asp:Content>
