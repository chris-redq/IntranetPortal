﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DocumentsUI.ascx.vb" Inherits="IntranetPortal.DocumentsUI" %>

<script src="/Scripts/stevenjs.js"></script>
<script type="text/javascript">
    init_tooltip_and_scroll();
    function UploadFiles() {
        var url = "";
        if (typeof leadsInfoBBLE == undefined || leadsInfoBBLE == null)
            url = '/UploadFilePage.aspx?b=<%= LeadsBBLE%>';
        else
            url = '/UploadFilePage.aspx?b=' + leadsInfoBBLE;

        //var centerLeft = parseInt((window.screen.availWidth - 640) / 2);
        //var centerTop = parseInt(((window.screen.availHeight - 400) / 2) - 50);          
        if (popupCtrUploadFiles) {
            popupCtrUploadFiles.SetContentUrl(url);
            popupCtrUploadFiles.Show();

            popupCtrUploadFiles.CloseUp.AddHandler(function (s, e) {
                if (typeof BindDocuments != "undefined")
                    BindDocuments(true);
            });
        }
        else
            window.open(url, 'Upload Files', popup_params(640, 400));
    }
     
    var IsDocumentLoaded = false;
    function BindDocuments(refreshDocuments) {
        if (!IsDocumentLoaded || refreshDocuments) {
            
            cbpDocumentUI.PerformCallback(leadsInfoBBLE);
            isLoaded = true;
        }
    }

    function OnFilePopUpClick(s, e) { 
        if (tmpFileId == null)
            return;

        if (e.item.index == 0) {
            NavigateUrl("/DownloadFile.aspx?spFile=" + tmpFileId);
        } else if (e.item.index == 1) {
            /*download*/
            NavigateUrl("/DownloadFile.aspx?spFile=" + tmpFileId + "&o=1");
        }
        else {

        }
    }

    function NavigateUrl(url) {
        window.open(url, '_blank');
    }

    var tmpFileId = null;
    function clickFileLink(s, docId) {
        tmpFileId = docId;
        AspFilePopupMenu.ShowAtElement(s.GetMainElement());
    }

    function GetSelectedAttachment() {
        var allFiles = [];
        $('input:checkbox.FileCheckbox').each(function () {
            if (this.checked) {
                var file = {
                    "Name": $(this).val(),
                    "UniqueId": $(this).attr("data-UniqueId")
                }
                allFiles.push(file);
            }
        });

        return allFiles;
    }

</script>

<dx:ASPxPopupMenu ID="ASPxPopupMenu11" runat="server" ClientInstanceName="AspFilePopupMenu"
    PopupElementID="numberLink" ShowPopOutImages="false" AutoPostBack="false"
    PopupHorizontalAlign="Center" PopupVerticalAlign="Below" PopupAction="LeftMouseClick"
    ForeColor="#3993c1" Font-Size="14px" CssClass="fix_pop_postion_s" Paddings-PaddingTop="15px" Paddings-PaddingBottom="18px">
    <ItemStyle Paddings-PaddingLeft="20px" />
    <Items>
        <dx:MenuItem Text="Preview" Name="Preview">
        </dx:MenuItem>
        <dx:MenuItem Text="Download" Name="Download">
        </dx:MenuItem>
        <%--<dx:MenuItem Text="Preview History" Name="History">
        </dx:MenuItem>--%>
    </Items>
     <ClientSideEvents ItemClick="function(s,e){OnFilePopUpClick(s,e);}" />
</dx:ASPxPopupMenu>

<div style="color: #999ca1;">
    <div style='padding: 35px 20px 35px 20px' class="border_under_line">
        <div style="width: 100%">
            <div class="font_30">
                <i class="fa fa-file"></i>&nbsp;
                <span class="font_light">Documents</span>
                <span class="time_buttons" onclick="UploadFiles()" style='<%= if(viewMode, "display:none", "") %>'>Upload File</span>
            </div>
            <div style="padding-left: 39px;" class="clearfix">
                <span style="font-size: 14px;"><%= LeadsName %></span>
                <span class="color_blue expand_button" style="padding-right: 25px" onclick="collapse_all(true)">Collapse All</span>
                <span class="color_blue expand_button" onclick="collapse_all(false)">Expand All&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
            </div>
        </div>
    </div> 
    <dx:ASPxCallbackPanel runat="server" ID="cbpDocumentUI" ClientInstanceName="cbpDocumentUI" OnCallback="cbpDocumentUI_Callback">
        <PanelCollection>
            <dx:PanelContent>
                <asp:DataList runat="server" ID="datalistCategory" RepeatColumns="1" Width="100%" ItemStyle-VerticalAlign="Top">
                    <ItemTemplate>
                        <%--border_right add line right--%>
                        <div class="doc_list_section ">
                            <div id="default-example" data-collapse="">
                                <h3 class="doc_list_title  color_balck">&nbsp;<i class="fa fa-minus-square-o color_blue collapse_btn_e" onclick="clickCollapse(this)"></i> &nbsp;&nbsp; <%# Eval("Key")%> </h3>
                                <div class="doc_collapse_div" style="padding-top: 5px">
                                    <div class="doc_list_section doc_list_section_sub">
                                        <asp:Repeater runat="server" ID="rptFolders" OnItemDataBound="rptFiles_ItemDataBound">
                                            <ItemTemplate>
                                                <h4 class="doc_list_title  color_balck" style="font-size: 16px"><i class="fa fa-minus-square-o color_blue collapse_btn_e" onclick="clickCollapse(this)"></i>&nbsp;&nbsp;<%# Eval("Key")%> </h4>
                                                <div class="doc_collapse_div" style="padding-top: 5px">
                                                    <asp:Repeater runat="server" ID="rptFiles">
                                                        <ItemTemplate>
                                                            <%--                <tr onclick="PreviewDocument('<%# String.Format("/DownloadFile.aspx?id={0}", Eval("FileID"))%>', '<%# Eval("ContentType")%>');" style="cursor:pointer" onmouseover="this.bgColor = '#D1DEFB';" onmouseout="this.bgColor = '';">--%>

                                                            <div class="clearfix ">
                                                                <input type="checkbox" name="vehicle" data-uniqueid="<%# Eval("Description")%>" class="FileCheckbox" value="<%# Eval("Name")%>" id="<%# String.Format("doc_list_id_{0}", Eval("Description"))%>" />
                                                                <label class="doc_list_checks doc_border_left check_margin doc_list_checks_sub" for='<%# String.Format("doc_list_id_{0}", Eval("Description"))%>' style="width: 94%">
                                                                    <span class="color_balck ">
                                                                        <%-- NavigateUrl='<%# String.Format("/DownloadFile.aspx?id={0}&spFile={1}", Eval("FileID"), Eval("Description"))%>' Text='<%# Eval("Name")%>'--%>
                                                                        <dx:ASPxHyperLink runat="server" CssClass="doc_file_name" ClientSideEvents-Click='<%# String.Format("function(s,e){{clickFileLink(s,""{0}"")}}", Eval("Description"))%>' Text='<%# Eval("Name")%>' Target="_blank"></dx:ASPxHyperLink>
                                                                    </span>
                                                                    <span class="checks_data_text">
                                                                        <dx:ASPxLabel runat="server" Text='<%# String.Format("{0}", Eval("CreateBy"))%>'></dx:ASPxLabel>
                                                                        &nbsp;<dx:ASPxLabel runat="server" Text='<%# String.Format("{0:MMM d, yyyy}", Eval("CreateDate"))%>'></dx:ASPxLabel>
                                                                    </span>
                                                                </label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Repeater runat="server" ID="rptFiles">
                                            <ItemTemplate>
                                                <%--                <tr onclick="PreviewDocument('<%# String.Format("/DownloadFile.aspx?id={0}", Eval("FileID"))%>', '<%# Eval("ContentType")%>');" style="cursor:pointer" onmouseover="this.bgColor = '#D1DEFB';" onmouseout="this.bgColor = '';">--%>

                                                <div class="clearfix ">
                                                    <input type="checkbox" name="vehicle" data-uniqueid="<%# Eval("Description")%>" class="FileCheckbox" value="<%# Eval("Name")%>" id="<%# String.Format("doc_list_id_{0}", Eval("Description"))%>" />
                                                    <label class="doc_list_checks doc_border_left check_margin doc_list_checks_sub" for='<%# String.Format("doc_list_id_{0}", Eval("Description"))%>' style="width: 94%">
                                                        <span class="color_balck ">
                                                            <%-- NavigateUrl='<%# String.Format("/DownloadFile.aspx?id={0}&spFile={1}", Eval("FileID"), Eval("Description"))%>' Text='<%# Eval("Name")%>'--%>
                                                            <dx:ASPxHyperLink runat="server" CssClass="doc_file_name" ClientSideEvents-Click='<%# String.Format("function(s,e){{clickFileLink(s,""{0}"")}}", Eval("Description"))%>' Text='<%# Eval("Name")%>' Target="_blank"></dx:ASPxHyperLink>
                                                        </span>
                                                        <span class="checks_data_text">
                                                            <dx:ASPxLabel runat="server" Text='<%# String.Format("{0}", Eval("CreateBy"))%>'></dx:ASPxLabel>
                                                            &nbsp;<dx:ASPxLabel runat="server" Text='<%# String.Format("{0:MMM d, yyyy}", Eval("CreateDate"))%>'></dx:ASPxLabel>
                                                        </span>
                                                    </label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
            </dx:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="function(s,e){IsDocumentLoaded= true;}" Init="function(s,e){IsDocumentLoaded= false;}" />
    </dx:ASPxCallbackPanel> 
</div>
