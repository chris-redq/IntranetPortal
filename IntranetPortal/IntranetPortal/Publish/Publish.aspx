﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Publish.aspx.vb" Inherits="IntranetPortal.Publish" MasterPageFile="~/Content.Master" %>

<%@ Register Src="~/Publish/PropertyList.ascx" TagPrefix="uc1" TagName="PropertyList" %>

<asp:Content ContentPlaceHolderID="MainContentPH" runat="server">    
    <div style="background: url(/images/MyIdealProptery.png) no-repeat center fixed; background-size: 260px, 280px; background-color: #dddddd; width: 100%; height: 100%;">
        <!-- Be careful with Padding  -->
        <dx:ASPxSplitter ID="ASPxSplitter1" runat="server" Height="100%" Width="100%" ClientVisible="true" ClientInstanceName="splitter" Orientation="Vertical" FullscreenMode="true">
            <Panes>
                <dx:SplitterPane Name="leadContent">
                    <Panes>
                        <dx:SplitterPane Name="leadPanel" ShowCollapseBackwardButton="True" MinSize="100px" MaxSize="400px" Size="270px" PaneStyle-Paddings-Padding="2px">
                            <ContentCollection>
                                <dx:SplitterContentControl ID="SplitterContentControl1" runat="server">
                                    <uc1:PropertyList runat="server" ID="PropertyList" />
                                </dx:SplitterContentControl>
                            </ContentCollection>
                        </dx:SplitterPane>
                        <dx:SplitterPane Name="contentPanel" ShowCollapseForwardButton="True" PaneStyle-BackColor="#f9f9f9" ScrollBars="Auto" PaneStyle-Paddings-Padding="0px" ContentUrl="about:blank">                        
                        </dx:SplitterPane>
                    </Panes>
                </dx:SplitterPane>
            </Panes>
        </dx:ASPxSplitter>
    </div>
</asp:Content>