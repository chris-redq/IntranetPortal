﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="NewOfferItem.ascx.vb" Inherits="IntranetPortal.NewOfferItem" %>

<style type="text/css">
    a.dx-link-MyIdealProp:hover {
        font-weight: 500;
        cursor: pointer;
    }

    .myRow:hover {
        background-color: #efefef;
    }

    label.xlink {
        color: black;
    }

    label.xlink:hover {
        color: blue;
    }
</style>
<h4 id="NewOffer_<%= ClientID %>" style="padding-top: 5px">
    <%--<img src="../images/<%= If(Not IsTitleStatus, "grid_task_icon.png", "grid_upcoming_icon.png") %>" class="vertical-img" />--%>
    <% If ManagerView = IntranetPortal.PropertyOfferManage.ManagerView.Completed %>
     <label class='grid-title-icon'>OF</label>
    <% ElseIf ManagerView = IntranetPortal.PropertyOfferManage.ManagerView.InProcess %>
     <label class='grid-title-icon'>I.P</label>
    <% ElseIf ManagerView = IntranetPortal.PropertyOfferManage.ManagerView.SSAccepted %>
     <label class='grid-title-icon'>SS</label>
    <% ElseIf ManagerView = IntranetPortal.PropertyOfferManage.ManagerView.PendingOF %>
     <label class='grid-title-icon'>PO</label>
    <% End if %>
   
    <a href="/NewOffer/NewOfferList.aspx?view=<%=CInt(ManagerView)%>">
        <label class="xlink">&nbsp;<%= ManagerView.ToString %></label>
        <label class="employee_lest_head_number_label" style="margin-left: 5px; color: white;"></label>
    </a>
</h4>
<div id="gridContainer" runat="server" style="margin: 3px; height: 330px"></div>
<script>
    var CategoryItem_<%= Me.ClientID%> = {
        url: "/api/propertyoffer?summary=true&mgrview=<%=CInt(ManagerView)%>",
        dxGridName: "#<%= gridContainer.ClientID%>",
        headName: "NewOffer_<%= ClientID%>",
        loadData: function () {
            var tab = this;
            $.getJSON(tab.url).done(function (data) {                
                var dataGrid = $(tab.dxGridName).dxDataGrid({
                    dataSource: data.data,
                    rowAlternationEnabled: true,
                    pager: {
                        showInfo: true
                    },
                    paging: {
                        enabled: true,
                    },
                    onRowPrepared: function (rowInfo) {
                        if (rowInfo.rowType != 'data')
                            return;
                        rowInfo.rowElement
                        .addClass('myRow');
                    },
                    onContentReady: function (e) {
                        var spanTotal = $('#' + tab.headName).find('.employee_lest_head_number_label')[0];
                        if (spanTotal) {
                            $(spanTotal).html(data.count);
                        }
                    },
                    showColumnHeaders:false,
                    columns: [{
                        dataField: "Title",
                        caption: "Name",
                        cellTemplate: function (container, options) {
                            $('<a/>').addClass('dx-link-MyIdealProp')
                                .text(options.value)
                                .on('dxclick', function(){
                                    //Do something with options.data;
                                    var url = '/ViewLeadsInfo.aspx?id=' + options.data.BBLE;
                                     <% If ManagerView = IntranetPortal.PropertyOfferManage.ManagerView.SSAccepted %>
                                     url = '/NewOffer/NewOfferAccepted.aspx?bble=' + options.data.BBLE;
                                     <% End if %>
                                    PortalUtility.ShowPopWindow("View Case - " + options.data.BBLE, url);                                                                       
                                })
                                .appendTo(container);
                        },
                    }],
                }).dxDataGrid('instance');              
            });
        }
    }
    
    $(function(){
        CategoryItem_<%= Me.ClientID%>.loadData();    
    });    

</script>
