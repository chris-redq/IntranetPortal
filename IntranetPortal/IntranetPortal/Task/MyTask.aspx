﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MyTask.aspx.vb" Inherits="IntranetPortal.MyTask" MasterPageFile="~/Content.Master" %>
<%@ Register Src="~/Task/TasklistControl.ascx" TagPrefix="uc1" TagName="TasklistControl" %>

<asp:Content ContentPlaceHolderID="MainContentPH" runat="server">
    <dx:ASPxSplitter ID="ASPxSplitter1" runat="server" Height="100%" Width="100%" ClientInstanceName="splitterTaskPage" Orientation="Horizontal" FullscreenMode="true">
        <Panes>
            <dx:SplitterPane Name="leadPanel" ShowCollapseBackwardButton="True" MinSize="100px" MaxSize="400px" Size="270px" PaneStyle-Paddings-Padding="2px">
                <ContentCollection>
                    <dx:SplitterContentControl ID="SplitterContentControl1" runat="server">
                        <uc1:TasklistControl runat="server" ID="TasklistControl" />
                    </dx:SplitterContentControl>
                </ContentCollection>
            </dx:SplitterPane>
            <dx:SplitterPane Name="contentPanel" ShowCollapseForwardButton="True" PaneStyle-BackColor="#f9f9f9" ScrollBars="Auto" ContentUrl="about:blank" PaneStyle-Paddings-Padding="0px" ContentUrlIFrameName="FrmTaskContent">               
            </dx:SplitterPane>
        </Panes>
    </dx:ASPxSplitter>
</asp:Content>
