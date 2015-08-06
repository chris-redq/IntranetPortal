﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConstructionUI.aspx.vb" Inherits="IntranetPortal.ConstructionUI" MasterPageFile="~/Content.Master"%>

<%@ Register Src="~/UserControl/DocumentsUI.ascx" TagPrefix="uc1" TagName="DocumentsUI" %>
<%@ Register Src="~/UserControl/ActivityLogs.ascx" TagPrefix="uc1" TagName="ActivityLogs" %>
<%@ Register Src="~/Construction/ConstructionCaseList.ascx" TagPrefix="uc1" TagName="ConstructionCaseList" %>




<asp:Content runat="server" ContentPlaceHolderID="head">


</asp:Content>


<asp:Content runat="server" ContentPlaceHolderID="MainContentPH">

      <dx:ASPxSplitter ID="ASPxSplitter1" runat="server" Height="100%" Width="100%" ClientInstanceName="splitter" Orientation="Horizontal" FullscreenMode="true">
            <Panes>
                <dx:SplitterPane Name="listPanel" ShowCollapseBackwardButton="True" MinSize="100px" MaxSize="400px" Size="280px" PaneStyle-Paddings-Padding="0">
                    <ContentCollection>
                        <dx:SplitterContentControl ID="SplitterContentControl1" runat="server">
                            <uc1:ConstructionCaseList runat="server" id="ConstructionCaseList" />
                        </dx:SplitterContentControl>
                    </ContentCollection>
                </dx:SplitterPane>
                <dx:SplitterPane ShowCollapseBackwardButton="True" ScrollBars="None" PaneStyle-Paddings-Padding="0px" Name="dataPane">
                    <ContentCollection>
                        <dx:SplitterContentControl>                            
                            <div style="align-content: center; height: 100%">
                                <!-- Nav tabs -->                                                           
                                <div class="legal-menu row" style="margin-left: 0px; margin-right: 0px">
                                    <ul class="nav nav-tabs clearfix" role="tablist" style="background: #ff400d; font-size: 18px; color: white; height: 70px">
                                        <li class="active short_sale_head_tab">
                                            <a href="#LegalTab" role="tab" data-toggle="tab" class="tab_button_a">
                                                <i class="fa fa-info-circle  head_tab_icon_padding"></i>
                                                <div class="font_size_bold">Construction</div>
                                            </a>
                                        </li>
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
                                                <ul class="nav  clearfix" role="tablist">
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
                                            <i class="fa fa-save sale_head_button sale_head_button_left tooltip-examples" title="" ng-click="SaveLegal()" data-original-title="Save"></i>
                                            <i class="fa fa-envelope sale_head_button sale_head_button_left tooltip-examples" title="" onclick="ShowEmailPopup(leadsInfoBBLE)" data-original-title="Mail"></i>
                                            <i class="fa fa-print sale_head_button sale_head_button_left tooltip-examples" title="" onclick="" data-original-title="Print"></i>
                                        </li>
                                    </ul>
                                </div>                               
                                <div id="ctl00_MainContentPH_ASPxSplitter1_ASPxCallbackPanel2_contentSplitter_SendMail_PopupSendMail_PW-1" class="dxpcLite_MetropolisBlue1 dxpclW" style="height: 700px; width: 630px; cursor: default; z-index: 10000; display: none;">
                                    <div class="dxpc-mainDiv dxpc-shadow">
                                        <div class="dxpc-header dxpc-withBtn" id="ctl00_MainContentPH_ASPxSplitter1_ASPxCallbackPanel2_contentSplitter_SendMail_PopupSendMail_PWH-1">
                                            <div class="clearfix">
                                                <div class="pop_up_header_margin">
                                                    <i class="fa fa-envelope with_circle pop_up_header_icon"></i>
                                                    <span class="pop_up_header_text ">Email</span>
                                                </div>
                                                <div class="pop_up_buttons_div">
                                                    <i class="fa fa-times icon_btn" onclick="popupSendEmailClient.Hide()"></i>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div class="tab-content">
                                    <div class="tab-pane active" id="ConstructionTab">
                                        
                                    </div>
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
                            </div>

                            <dx:ASPxPopupControl ClientInstanceName="popupCtrUploadFiles" Width="950px" Height="840px" ID="ASPxPopupControl2"
                                HeaderText="Upload Files" AutoUpdatePosition="true" Modal="true" CloseAction="CloseButton"
                                runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
                                <HeaderTemplate>
                                    <div class="clearfix">
                                        <div class="pop_up_header_margin">
                                            <i class="fa fa-cloud-upload with_circle pop_up_header_icon"></i>
                                            <span class="pop_up_header_text">Upload Files</span>
                                        </div>
                                        <div class="pop_up_buttons_div">
                                            <i class="fa fa-times icon_btn" onclick="popupCtrUploadFiles.Hide()"></i>
                                        </div>
                                    </div>
                                </HeaderTemplate>
                                <ContentCollection>
                                    <dx:PopupControlContentControl runat="server">
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                                <ClientSideEvents CloseUp="function(s,e){}" />
                            </dx:ASPxPopupControl>

                        </dx:SplitterContentControl>
                    </ContentCollection>
                </dx:SplitterPane>
                <dx:SplitterPane ShowCollapseBackwardButton="True" PaneStyle-BackColor="#f9f9f9" PaneStyle-Paddings-Padding="0px" Name="LogPanel">
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
                                    <%--<li><a role="tab" data-toggle="tab">Settings</a></li>--%>
                                    <li style="margin-right: 30px; color: #7396a9; float: right">
                                        <%--<div style="display: inline-block">
                                            <a href="/LegalUI/LegalUI.aspx?SecondaryAction=true"><i class="fa fa-arrow-right sale_head_button sale_head_button_left tooltip-examples" style="margin-right: 10px; color: #7396A9" title="Secondary" onclick=""></i></a>
                                        </div>--%>

                                        <%--                                        <i class="fa fa-codepen  sale_head_button sale_head_button_left tooltip-examples" title="" onclick="popupSelectAttorneyCtr.PerformCallback();popupSelectAttorneyCtr.ShowAtElement(this);" data-original-title="Research"></i>
                                        <i class="fa fa-mail-forward  sale_head_button sale_head_button_left tooltip-examples" title="" onclick="popupSelectAttorneyCtr.PerformCallback();popupSelectAttorneyCtr.ShowAtElement(this);" data-original-title="Attorney"></i>
                                        <i class="fa fa-check sale_head_button sale_head_button_left tooltip-examples" title="" ng-click="AttorneyComplete()" data-original-title="Close"></i>--%>

                                        <%--<i class="fa fa-repeat sale_head_button tooltip-examples" title="Follow Up" onclick="ASPxPopupMenuClientControl.ShowAtElement(this);"></i>--%>
                                        <%-- <i class="fa fa-file sale_head_button sale_head_button_left tooltip-examples" title="New File" onclick="LogClick('NewFile')"></i>--%>
                                        <%--<i class="fa fa-folder-open sale_head_button sale_head_button_left tooltip-examples" title="Active" onclick="LogClick('Active')"></i>--%>
                                        <%--<i class="fa fa-sign-out  sale_head_button sale_head_button_left tooltip-examples" title="Eviction" onclick="tmpBBLE=leadsInfoBBLE;popupEvictionUsers.PerformCallback();popupEvictionUsers.ShowAtElement(this);"></i>--%>
                                        <%-- <i class="fa fa-pause sale_head_button sale_head_button_left tooltip-examples" title="On Hold" onclick="LogClick('OnHold')"></i>--%>
                                        <%--<i class="fa fa-check-circle sale_head_button sale_head_button_left tooltip-examples" title="Closed" onclick="LogClick('Closed')"></i>--%>
                                        <i class="fa fa-print  sale_head_button sale_head_button_left tooltip-examples" title="Print" onclick="PrintLogInfo()"></i>
                                    </li>
                                </ul>
                                <dx:ASPxCallbackPanel runat="server" ID="cbpLogs" ClientInstanceName="cbpLogs" OnCallback="cbpLogs_Callback">
                                    <PanelCollection>
                                        <dx:PanelContent>
                                            <uc1:ActivityLogs runat="server" ID="ActivityLogs" DisplayMode="Legal" />                                            
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>
                            </div>
                        </dx:SplitterContentControl>
                    </ContentCollection>
                </dx:SplitterPane>
            </Panes>
        </dx:ASPxSplitter>



</asp:Content>