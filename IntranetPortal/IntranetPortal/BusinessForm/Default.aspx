﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Content.Master" CodeBehind="Default.aspx.vb" Inherits="IntranetPortal.BusinessFormDefault" %>

<%@ Register Src="~/UserControl/DocumentsUI.ascx" TagPrefix="uc1" TagName="DocumentsUI" %>
<%@ Register Src="~/UserControl/ActivityLogs.ascx" TagPrefix="uc1" TagName="ActivityLogs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPH" runat="server">
    <dx:ASPxSplitter ID="contentSplitter" runat="server" Height="100%" Width="100%" ClientInstanceName="splitter" Orientation="Horizontal" FullscreenMode="true">
        <Panes>
            <%-- list panel  --%>
            <dx:SplitterPane Name="listPanel" ShowCollapseBackwardButton="True" MinSize="100px" MaxSize="400px" Size="280px" PaneStyle-Paddings-Padding="0">
                <PaneStyle>
                    <Paddings Padding="0px"></Paddings>
                </PaneStyle>
                <ContentCollection>
                    <dx:SplitterContentControl ID="SplitterContentControl1" runat="server">
                    </dx:SplitterContentControl>
                </ContentCollection>
            </dx:SplitterPane>
            <%-- data panel     --%>
            <dx:SplitterPane ShowCollapseBackwardButton="True" ScrollBars="None" PaneStyle-Paddings-Padding="0px" Name="dataPane">
                <PaneStyle>
                    <Paddings Padding="0px"></Paddings>
                </PaneStyle>
                <ContentCollection>
                    <dx:SplitterContentControl>
                        <div class="legal-menu row" style="margin: 0px;">
                            <ul class="nav nav-tabs clearfix" role="tablist" style="background: #ff400d; font-size: 18px; color: white; height: 70px">
                                <asp:Repeater runat="server" ID="rptTopmenu">
                                    <ItemTemplate>
                                        <li class="<%# ActivieTab(Container.ItemIndex)%> short_sale_head_tab">
                                            <a href='#<%#Eval("Name")%>' role="tab" data-toggle="tab" class="tab_button_a">
                                                <i class="fa fa-info-circle  head_tab_icon_padding"></i>
                                                <div class="font_size_bold"><%# Eval("Name")%></div>
                                            </a>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <li class="short_sale_head_tab">
                                    <a href="#DocumentTab" role="tab" data-toggle="tab" class="tab_button_a" onclick="BindDocuments(false)">
                                        <i class="fa fa-file head_tab_icon_padding"></i>
                                        <div class="font_size_bold">Documents</div>
                                    </a>
                                </li>
                                <li class="short_sale_head_tab">
                                    <a class="tab_button_a">
                                        <i class="fa fa-list-ul head_tab_icon_padding"></i>
                                        <div class="font_size_bold">&nbsp;&nbsp;&nbsp;&nbsp;More&nbsp;&nbsp;&nbsp;&nbsp;</div>
                                    </a>
                                    <div class="shot_sale_sub">
                                        <ul class="nav clearfix" role="tablist">
                                            <li class="short_sale_head_tab">
                                                <a role="tab" class="tab_button_a" data-toggle="tab" href="#more_leads" data-url="/ViewLeadsInfo.aspx?HiddenTab=true&id=BBLE" data-href="#more_leads" onclick="LoadMoreFrame(this)">
                                                    <i class="fa fa-folder head_tab_icon_padding"></i>
                                                    <div class="font_size_bold">Leads</div>
                                                </a>
                                            </li>
                                            <li class="short_sale_head_tab" ng-show="LegalCase.InShortSale">
                                                <a role="tab" class="tab_button_a" data-toggle="tab" href="#more_short_sale" data-url="/ShortSale/ShortSale.aspx?HiddenTab=true&bble=BBLE" data-href="#more_short_sale" onclick="LoadMoreFrame(this)">
                                                    <i class="fa fa-sign-out head_tab_icon_padding"></i>
                                                    <div class="font_size_bold">Short Sale</div>
                                                </a>
                                            </li>
                                            <li class="short_sale_head_tab" ng-show="LegalCase.InShortSale">
                                                <a role="tab" class="tab_button_a" data-toggle="tab" href="#more_evction" data-url="/ShortSale/ShortSale.aspx?HiddenTab=true&isEviction=true&bble=BBLE" data-href="#more_evction" onclick="LoadMoreFrame(this)">
                                                    <i class="fa fa-sign-out head_tab_icon_padding"></i>
                                                    <div class="font_size_bold">Eviction</div>
                                                </a>
                                            </li>
                                        </ul>
                                    </div>
                                </li>
                                <li class="pull-right" style="margin-right: 30px; color: #ffa484">
                                    <i class="fa fa-save sale_head_button sale_head_button_left tooltip-examples" title="" onclick="FormControl.SaveData()" data-original-title="Save"></i>
                                    <i class="fa fa-mail-forward  sale_head_button sale_head_button_left tooltip-examples" title="Re-Assign" onclick="popupSelectAttorneyCtr.PerformCallback('type|Users');popupSelectAttorneyCtr.ShowAtElement(this);"></i>
                                    <i class="fa fa-envelope sale_head_button sale_head_button_left tooltip-examples" title="" onclick="ShowEmailPopup(leadsInfoBBLE)" data-original-title="Mail"></i>
                                    <i class="fa fa-print sale_head_button sale_head_button_left tooltip-examples" title="" onclick="" data-original-title="Print"></i>
                                </li>
                            </ul>

                            <script type="text/javascript">
                                var refreshList = false;
                            </script>
                            
                            <dx:ASPxPopupControl ClientInstanceName="popupSelectAttorneyCtr" Width="300px" Height="300px"
                                MaxWidth="800px" MaxHeight="800px" MinHeight="150px" MinWidth="150px" ID="ASPxPopupControl3"
                                HeaderText="Select Employee" AutoUpdatePosition="true" Modal="true" OnWindowCallback="ASPxPopupControl3_WindowCallback"
                                runat="server" EnableViewState="false" EnableHierarchyRecreation="True">
                                <ContentCollection>
                                    <dx:PopupControlContentControl runat="server" Visible="false" ID="PopupContentReAssign">
                                        <asp:HiddenField runat="server" ID="hfUserType" />
                                        <dx:ASPxListBox runat="server" ID="listboxEmployee" ClientInstanceName="listboxEmployeeClient" Height="270" SelectedIndex="0" Width="100%">
                                        </dx:ASPxListBox>
                                        <dx:ASPxButton Text="Assign" runat="server" ID="btnAssign" AutoPostBack="false">
                                            <ClientSideEvents Click="function(s,e){
                                        var item = listboxEmployeeClient.GetSelectedItem();
                                        if(item == null)
                                        {
                                             alert('Please select Employee.');
                                             return;
                                         }
                                        popupSelectAttorneyCtr.PerformCallback('Save|' + leadsInfoBBLE + '|' + item.text);
                                        refreshList = true;
                                        popupSelectAttorneyCtr.Hide();
                                        }" />
                                        </dx:ASPxButton>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                                <ClientSideEvents Closing="function(s,e){
                                            if(refreshList)
                                            { 
                                              if (typeof gridTrackingClient != 'undefined')
                                                    gridTrackingClient.Refresh();
                                              if (typeof gridCase != 'undefined')
                                                    gridCase.Refresh();
                                              refreshList = false;
                                            }
                                        }" />
                            </dx:ASPxPopupControl>
                        </div>
                        <div class="tab-content">
                            <asp:Repeater runat="server" ID="rptBusinessControl" OnItemDataBound="rptBusinessControl_ItemDataBound">
                                <ItemTemplate>
                                    <div class="<%# ActivieTab(Container.ItemIndex)%> tab-pane" id="<%# Eval("Name")%>">
                                        <asp:Panel runat="server" ID="pnlHolder">
                                        </asp:Panel>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <div class="tab-pane" id="DocumentTab">
                                <uc1:DocumentsUI runat="server" ID="DocumentsUI" />
                            </div>
                            <div class="tab-pane load_bg" id="more_leads">
                                <iframe width="100%" height="100%" class="more_frame" frameborder="0"></iframe>
                            </div>
                            <div class="tab-pane load_bg" id="more_evction">
                                <iframe width="100%" height="100%" class="more_frame" frameborder="0"></iframe>
                            </div>
                            <div class="tab-pane load_bg" id="more_short_sale">
                                <iframe width="100%" height="100%" class="more_frame" frameborder="0"></iframe>
                            </div>
                        </div>
                        <input type="hidden" id="CaseData" />
                        <script type="text/javascript">                            
                            var FormControl = {
                                BBLE: null,
                                ShowActivityLog: <%= If(FormData.ShowActivityLog, "true", "false")%>,
                               
                                CurrentTab: {
                                    Name: null,
                                    BusinessData: null,
                                    EnableAutoSave:true,
                                    ActivityLogMode:null
                                },
                                InitTab: function (name, data) {
                                    this.CurrentTab.Name = name;
                                    this.CurrentTab.BusinessData = data;
                                    this.CurrentTab.EnableAutoSave = true;

                                    if(this.CurrentTab.EnableAutoSave){
                                        ScopeAutoSave(angular.element(document.getElementById(name + 'Controller')).scope().Get, this.SaveData,"CaseData");
                                    }

                                },
                                LoadData: function (dataId) {
                                    var tab = this.CurrentTab;
                                    var url = "/api/BusinessForm/" + tab.BusinessData + "/" + dataId
                                    $.ajax({
                                        type: "GET",
                                        url: url,
                                        dataType: 'json',
                                        success: function (data) {
                                            //console.log(data);
                                            angular.element(document.getElementById(tab.Name + 'Controller')).scope().Load(data);

                                            if(tab.EnableAutoSave){
                                                //auto save reset data
                                                ScopeResetCaseDataChange(angular.element(document.getElementById(tab.Name + 'Controller')).scope().Get);
                                            }                                            
                                        },
                                        error: function (data) {
                                            AngularRoot.alert("Failed to load data." + data)
                                        }
                                    });
                                    this.LoadActivityLog();                                    
                                },
                                LoadActivityLog:function()
                                {
                                    if(this.ShowActivityLog)
                                    {
                                        this.BBLE = leadsInfoBBLE;
                                        if(typeof cbpLogs != "undefined")
                                            cbpLogs.PerformCallback(this.BBLE);
                                    }
                                },
                                SaveData: function () {
                                    var tab = this.CurrentTab;
                                    var data = angular.element(document.getElementById(tab.Name + 'Controller')).scope().Get(true);
                                    if(data.DataId){    //verify that data is an exsiting one
                                        var url = "/api/BusinessForm/"
                                        $.ajax({
                                            type: "POST",
                                            url: url,
                                            data:JSON.stringify(data),
                                            dataType: 'json',
                                            contentType: "application/json",
                                            success: function (data) {
                                                AngularRoot.alert("Save successful.")
                                            },
                                            error: function (data) {
                                                AngularRoot.alert("Failed to save data." + data);
                                            }
                                        });
                                    }else{
                                        console.log("Try to save a not exist data.")
                                    }
                                }
                            }

                            $(function () {
                                FormControl.InitTab('<%= FormData.DefaultControl.Name%>', '<%= FormData.DefaultControl.BusinessData%>');
                            });
                        </script>

                    </dx:SplitterContentControl>
                </ContentCollection>
            </dx:SplitterPane>

            <dx:SplitterPane ShowCollapseBackwardButton="True" PaneStyle-BackColor="#f9f9f9" PaneStyle-Paddings-Padding="0px" Name="LogPanel" Visible="false">
                <PaneStyle BackColor="#F9F9F9">
                    <Paddings Padding="0px"></Paddings>
                </PaneStyle>
                <ContentCollection>
                    <dx:SplitterContentControl>
                        <div style="font-size: 12px; color: #9fa1a8;">
                            <ul class="nav nav-tabs clearfix" role="tablist" style="height: 70px; background: #295268; font-size: 18px; color: white">
                                <li class="short_sale_head_tab activity_light_blue">
                                    <a href="#property_info" role="tab" data-toggle="tab" class="tab_button_a">
                                        <i class="fa fa-history head_tab_icon_padding"></i>
                                        <div class="font_size_bold">Activity Log</div>
                                    </a>
                                </li>
                                <li style="margin-right: 30px; color: #7396a9; float: right">
                                    <i class="fa fa-print  sale_head_button sale_head_button_left tooltip-examples" title="Print" onclick="PrintLogInfo()"></i>
                                </li>
                            </ul>
                            <dx:ASPxCallbackPanel runat="server" ID="cbpLogs" ClientInstanceName="cbpLogs" OnCallback="cbpLogs_Callback">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <uc1:ActivityLogs runat="server" ID="ActivityLogs" />
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxCallbackPanel>

                            <!-- Follow up function  -->
                            <script type="text/javascript">

                                function OnCallbackMenuClick(s, e) {

                                    if (e.item.name == "Custom") {
                                        ASPxPopupSelectDateControl.PerformCallback("Show");
                                        ASPxPopupSelectDateControl.ShowAtElement(s.GetMainElement());
                                        e.processOnServer = false;
                                        return;
                                    }

                                    SetFollowUp(e.item.name)
                                    e.processOnServer = false;
                                }

                                function SetFollowUp(type, dateSelected) {
                                    if (typeof dateSelected == 'undefined')
                                        dateSelected = new Date();
                                    debugger;
                                    $.ajax({
                                        url: '/api/Followup/?category=' + type + '&type=17&bble=' + leadsInfoBBLE,
                                        type: 'POST',
                                        data: dateSelected.toJSON(),
                                        cache: false,
                                        success: function (data) {                                          
                                            AngularRoot.alert('Successful.');
                                        },
                                        error: function (data) {
                                            AngularRoot.alert('Some error Occurred!');
                                        }
                                    });
                                }

                            </script>

                            <dx:ASPxPopupMenu ID="ASPxPopupCallBackMenu2" runat="server" ClientInstanceName="ASPxPopupMenuClientControl"
                                AutoPostBack="false" PopupHorizontalAlign="Center" PopupVerticalAlign="Below" PopupAction="LeftMouseClick"
                                ForeColor="#3993c1" Font-Size="14px" CssClass="fix_pop_postion_s" Paddings-PaddingTop="15px" Paddings-PaddingBottom="18px">
                                <ItemStyle Paddings-PaddingLeft="20px">
                                    <Paddings PaddingLeft="20px"></Paddings>
                                </ItemStyle>

                                <Paddings PaddingTop="15px" PaddingBottom="18px"></Paddings>
                                <Items>
                                    <dx:MenuItem Text="Tomorrow" Name="Tomorrow"></dx:MenuItem>
                                    <dx:MenuItem Text="Next Week" Name="NextWeek"></dx:MenuItem>
                                    <dx:MenuItem Text="30 Days" Name="ThirtyDays">
                                    </dx:MenuItem>
                                    <dx:MenuItem Text="60 Days" Name="SixtyDays">
                                    </dx:MenuItem>
                                    <dx:MenuItem Text="Custom" Name="Custom">
                                    </dx:MenuItem>
                                </Items>
                                <ClientSideEvents ItemClick="OnCallbackMenuClick" />
                            </dx:ASPxPopupMenu>
                            <dx:ASPxPopupControl ClientInstanceName="ASPxPopupSelectDateControl" Width="360px" Height="250px"
                                MaxWidth="800px" MaxHeight="150px" MinHeight="150px" MinWidth="150px" ID="pcMain"
                                HeaderText="Select Date" Modal="false"
                                runat="server" PopupHorizontalAlign="LeftSides" PopupVerticalAlign="Below" EnableHierarchyRecreation="True">
                                <ContentCollection>
                                    <dx:PopupControlContentControl runat="server" ID="pcMainPopupControl">
                                        <table>
                                            <tr>
                                                <td>
                                                    <dx:ASPxCalendar ID="ASPxCalendar1" runat="server" ClientInstanceName="callbackCalendar" ShowClearButton="False" ShowTodayButton="False" Visible="true"></dx:ASPxCalendar>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="color: #666666; font-size: 10px; align-content: center; text-align: center; padding-top: 2px;">
                                                    <dx:ASPxButton ID="ASPxButton1" runat="server" Text="OK" AutoPostBack="false" CssClass="rand-button rand-button-blue">
                                                        <ClientSideEvents Click="function(){ASPxPopupSelectDateControl.Hide();SetFollowUp('CustomDays',callbackCalendar.GetSelectedDate());}"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                    &nbsp;
                                                    <dx:ASPxButton runat="server" Text="Cancel" AutoPostBack="false" CssClass="rand-button rand-button-gray">
                                                        <ClientSideEvents Click="function(){ASPxPopupSelectDateControl.Hide();}"></ClientSideEvents>
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                        </div>
                    </dx:SplitterContentControl>
                </ContentCollection>
            </dx:SplitterPane>




        </Panes>
    </dx:ASPxSplitter>



</asp:Content>
