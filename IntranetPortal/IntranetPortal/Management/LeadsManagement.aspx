﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LeadsManagement.aspx.vb" EnableEventValidation="false" Inherits="IntranetPortal.LeadsManagement" MasterPageFile="~/Content.Master" %>

<%@ Register Src="~/UserControl/LeadsInfo.ascx" TagPrefix="uc1" TagName="LeadsInfo" %>
<%@ Register Src="~/UserControl/LeadsSubMenu.ascx" TagPrefix="uc1" TagName="LeadsSubMenu" %>
<%@ Register Src="~/UserControl/AssignRulesControl.ascx" TagPrefix="uc1" TagName="AssignRulesControl" %>
<%@ Register Src="~/PopupControl/AssignLeadsPopup.ascx" TagPrefix="uc1" TagName="AssignLeadsPopup" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var postponedCallbackRequired = false;
        var leadsInfoBBLE = null;
        var tempBBLE = null;

        //function is called on changing focused row
        function OnGridFocusedRowChanged(s, e) {
            // The values will be returned to the OnGetRowValues() function 
            var rowIndex = s.GetFocusedRowIndex();
            if (rowIndex >= 0) {
                NavigateToLeadsInfo(s.GetRowKey(rowIndex));
                return;
            }
        }

        function OnSelectionChanged(s, e) {
            var counnt = s.GetSelectedRowCount();
            $("#gridSelectCount").html(counnt);
            if (counnt > 0) {
                document.getElementById("btnAssign").disabled = false;
            } else
                document.getElementById("btnAssign").disabled = true;
        }

        function NavigateToLeadsInfo(bble) {
            if (bble == null || bble == "")
            {
                AngularRoot.alert("The leads can't load. Please try again.")
                return;
            }

            if (leadsInfoBBLE == bble)
                return
            else {
                leadsInfoBBLE = bble;
                var url = "/ViewLeadsInfo.aspx?b=" + bble;
                var contentPane = splitter.GetPaneByName("RightPane");
                contentPane.SetContentUrl(url);
                return;
            }
        }

        function OnGetRowValues(values) {
            if (values == null) {
                gridLeads.GetValuesOnCustomCallback(gridLeads.GetFocusedRowIndex(), OnGetRowValues);
            }
            else {

                leadsInfoBBLE = values;
                ContentCallbackPanel.PerformCallback(values);
            }
        }

        function OnChangeLeadsType(s, e) {
            if (tempBBLE != null) {
                updateLeadsType.PerformCallback(tempBBLE + "|" + e.item.name);
            }
        }

        function OnEndCallback(s, e) {
            return;
        }

        function onInitScorllBar() {
            return;
        }

        function filterOutRecycel(filtedRecyceled) {
            var condtion = filtedRecyceled ? '[IsRecycled] = false' : '';
            gridLeads.ApplyFilter(condtion);
        }

        function filterByType(key)
        {
            if (key.trim() == "") {
                gridLeads.ClearFilter();
                return;
            }

            var filterCondition = "";        
            filterCondition = "[TypeText] LIKE '%" + key + "%' or [LeadsTags] LIKE '%" + key + "%'";
            //filterCondition += " OR [Neighborhood] LIKE '%" + key + "%'";
            gridLeads.ApplyFilter(filterCondition);
        }

        var lastSearchKey = "";
        function SearchGrid() {
            if (gridLeads.InCallback()) {
                AngularRoot.alert("Page is busy, please try later.")
                return;
            }

            var key = document.getElementById("QuickSearch").value;
            if (key.trim() == lastSearchKey) {
                return;
            } else {
                lastSearchKey = key.trim();
            }

            if (key.trim() == "") {
                gridLeads.ClearFilter();
                return;
            }

            var filterCondition = "";
            filterCondition = "[PropertyAddress] LIKE '%" + key + "%' OR [BBLE] LIKE '" + key + "%'";
            filterCondition += " OR [Neighborhood] LIKE '%" + key + "%'";
            //filterCondition += " OR [Neighborhood] LIKE '%" + key + "%'";
            gridLeads.ApplyFilter(filterCondition);
        }

    </script>
    <style type="text/css">
        .rand-button:disabled {
            background-color: gray;
        }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPH" runat="server">
    <dx:ASPxSplitter ID="ASPxSplitter1" runat="server" Height="100%" Width="100%" ClientInstanceName="splitter" FullscreenMode="true">
        <Styles>
            <Pane>
                <Paddings Padding="2px" />
            </Pane>
            <Separator>
                <Border BorderStyle="None" />
            </Separator>
            <VerticalSeparator>
                <Border BorderStyle="None" />
            </VerticalSeparator>
        </Styles>
        <Panes>
            <dx:SplitterPane Size="850px" MinSize="300px" ShowCollapseBackwardButton="True">
                <PaneStyle>
                    <Border BorderStyle="None" />
                </PaneStyle>
                <Separators Visible="False"></Separators>
                <Panes>
                    <dx:SplitterPane Size="100px">
                        <ContentCollection>
                            <dx:SplitterContentControl>
                                <div style="margin: 10px 20px 10px 10px; text-align: left; padding-left: 5px" class="clearfix">
                                    <div style="font-size: 24px;" class="clearfix">
                                        <i class="fa fa-check-square-o with_circle" style="width: 48px; height: 48px; line-height: 48px;"></i>&nbsp;
                                        <span style="color: #234b60; font-size: 30px;">
                                            <dx:ASPxLabel Text="Assign Leads" ID="lblLeadCategory" Font-Size="30px" ClientInstanceName="LeadCategory" runat="server"></dx:ASPxLabel>
                                        </span>
                                        <span style="font-size: 18px; margin-left: 20px">
                                            <span id="gridSelectCount">0</span> selected
                                        </span>
                                        <span style="margin-left: 10px">
                                            <input type="checkbox" id="cbfilterOutRecycel" name="cbfilterOutExist" class="font_12" onchange="filterOutRecycel(this.checked)">
                                            <label for="cbfilterOutRecycel" class="font_12" style="padding-top: 20px; float: none">
                                                <span class="upcase_text">Don't Show Recycled Leads</span>
                                            </label>
                                        </span>
                                        <span style="margin-left: 10px">
                                            <label for="cbfilterOutRecycel" class="font_12" style="padding-top: 20px; float: none">
                                                <span class="upcase_text">Leads Type</span>
                                            </label>
                                            <select id="selectLeadsType" class="form-control" onchange="filterByType(this.value)" style="width:100px;display:inline-block;">
                                                <option></option>
                                                <option value="TaxLien">Tax Lien</option>
                                                <option value="VacantLand">Vacan Leads</option>
                                                <option value="Deceased">Deceased</option>
                                                <% If Not String.IsNullOrEmpty(IntranetPortal.Core.PortalSettings.GetValue("CustomTags"))  %>
                                                <% for Each tag In IntranetPortal.Core.PortalSettings.GetValue("CustomTags").Split(";") %>
                                                   <option value="<%= tag %>"><%= tag %></option>
                                                <% Next %>
                                                <% End If %>
                                            </select>
                                        </span>
                                        <div style="float: right">
                                            <asp:LinkButton ID="btnExport" Visible="false" runat="server" OnClick="btnExport_Click" Text='<i class="fa  fa-file-excel-o  report_head_button report_head_button_padding tooltip-examples" title="Export to Excel"></i>'>                                                                
                                            </asp:LinkButton>
                                            <input type="button" value="Create Leads" hidden="hidden" class="rand-button rand-button-blue rand-button-pad" onclick="window.location.href = '/LeadsGenerator/LeadsGenerator.aspx'" />
                                        </div>
                                        <div style="width: 100%">                                           
                                            <div style="text-align: center" class="form-inline">
                                                <div class="input-group">
                                                    <input type="text" class="form-control" style="width:300px" placeholder="Search for BBLE, Address, Neighbor..." id="QuickSearch" onkeydown="javascript:if(event.keyCode == 13){ SearchGrid();return false; }" />                                                   
                                                    <span class="input-group-btn">
                                                        <button class="btn btn-secondary" type="button" onclick="SearchGrid()"><i class="fa fa-search tooltip-examples"></i></button>
                                                    </span>
                                                </div>                                              
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </dx:SplitterContentControl>
                        </ContentCollection>
                    </dx:SplitterPane>
                    <dx:SplitterPane Name="gridPanel">
                        <ContentCollection>
                            <dx:SplitterContentControl>
                                <dx:ASPxGridView runat="server" Settings-ShowColumnHeaders="false" OnDataBinding="gridLeads_DataBinding"
                                    ID="gridLeads" ClientInstanceName="gridLeads" Width="100%" KeyFieldName="BBLE" OnHtmlRowPrepared="gridLeads_HtmlRowPrepared" OnCustomCallback="gridLeads_CustomCallback"
                                    EnableViewState="true">
                                    <Columns>
                                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" SelectAllCheckboxMode="Page" VisibleIndex="0" Name="colSelect" Visible="true" Width="25px">
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataColumn FieldName="BBLE" Caption="BBLE" Width="1px" ExportWidth="100">
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataTextColumn FieldName="PropertyAddress" Settings-AllowHeaderFilter="False">
                                            <Settings AllowHeaderFilter="False"></Settings>
                                            <DataItemTemplate>
                                                <%# String.Format("<span style=""font-weight: 900;"">{0}</span>-{1}", String.Format("{0} {1}", Eval("Number"), Eval("Street")).Trim, Eval("Owner"))%>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Neighborhood" Width="85px" Caption="Neighbor" Settings-HeaderFilterMode="CheckedList"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataSpinEditColumn FieldName="NYCSqft" Width="60px" Caption="SQFT" Settings-HeaderFilterMode="CheckedList"></dx:GridViewDataSpinEditColumn>
                                        <dx:GridViewDataTextColumn FieldName="LotDem" Width="100px" Settings-HeaderFilterMode="CheckedList"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="PropertyClass" Caption="Class" Width="60px" Settings-HeaderFilterMode="CheckedList"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="MortgageCombo" Width="95px" Caption="MtgCOMBO" PropertiesTextEdit-DisplayFormatString="C" Settings-HeaderFilterMode="CheckedList"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="C1stServicer" Caption="Servicer" Width="80px" Settings-HeaderFilterMode="CheckedList"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="TaxLiensAmount" Caption="TaxCOMBO" Width="70px" PropertiesTextEdit-DisplayFormatString="C" Settings-HeaderFilterMode="CheckedList"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="TypeText" Caption="Type" Width="50px" CellStyle-HorizontalAlign="Center" CellStyle-VerticalAlign="Middle" Settings-HeaderFilterMode="CheckedList">
                                            <DataItemTemplate>
                                                <dx:ASPxImage EmptyImage-Url="~/images/ide.png" EmptyImage-Width="16" EmptyImage-Height="16" runat="server" ID="imgType" Width="24" Height="24" CssClass="always_show">
                                                </dx:ASPxImage>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataColumn FieldName="Comments" Width="60px" Caption="Recycle" Settings-HeaderFilterMode="CheckedList" Visible="false">
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox runat="server" ID="chkRecycled" ToolTip="Recycled" Checked='<%# Eval("IsRecycled")%>' ReadOnly="true" Visible='<%# Eval("IsRecycled")%>'></dx:ASPxCheckBox>
                                            </DataItemTemplate>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataTextColumn FieldName="RecycleFrom" Caption="Agent" Width="70px"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="LeadsTags" Visible="false"></dx:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsBehavior AllowClientEventsOnLoad="true" AllowFocusedRow="true" EnableRowHotTrack="True" />
                                    <Settings ShowFilterRowMenu="true" ShowHeaderFilterButton="true" ShowColumnHeaders="true" GridLines="Both" VerticalScrollableHeight="1000"></Settings>
                                    <SettingsPager Mode="EndlessPaging" PageSize="50"></SettingsPager>
                                    <Styles>
                                        <Header HorizontalAlign="Center"></Header>
                                        <Row Cursor="pointer" />
                                        <AlternatingRow BackColor="#F5F5F5"></AlternatingRow>
                                        <RowHotTrack BackColor="#FF400D"></RowHotTrack>
                                    </Styles>
                                    <Border BorderStyle="None"></Border>
                                    <ClientSideEvents FocusedRowChanged="OnGridFocusedRowChanged" SelectionChanged="OnSelectionChanged" />
                                </dx:ASPxGridView>
                                <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="gridLeads" OnRenderBrick="gridExport_RenderBrick">
                                </dx:ASPxGridViewExporter>
                                <dx:ASPxPopupMenu ID="ASPxPopupMenu3" runat="server" ClientInstanceName="leadsTypeMenu"
                                    AutoPostBack="false" PopupHorizontalAlign="Center" PopupVerticalAlign="Below" PopupAction="LeftMouseClick" ForeColor="#3993c1" Font-Size="14px" CssClass="fix_pop_postion_s" Paddings-PaddingTop="15px" Paddings-PaddingBottom="18px">
                                    <Items>
                                        <dx:MenuItem Text="Development" Name="DevelopmentOpportunity" Image-Url="/images/lr_dev_opportunity.png">
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="Foreclosure" Name="Foreclosure" Image-Url="/images/lr_forecosure.png">
                                        </dx:MenuItem>
                                        <dx:MenuItem Text="Has Equity" Name="HasEquity" Image-Url="/images/lr_has_equity.png"></dx:MenuItem>
                                        <dx:MenuItem Text="Tax Lien" Name="TaxLien" Image-Url="/images/lr_tax_lien.png">
                                        </dx:MenuItem>
                                    </Items>
                                    <ClientSideEvents ItemClick="OnChangeLeadsType" />
                                </dx:ASPxPopupMenu>
                            </dx:SplitterContentControl>
                        </ContentCollection>
                    </dx:SplitterPane>
                    <dx:SplitterPane Size="70px">
                        <ContentCollection>
                            <dx:SplitterContentControl>
                                <table id="assign_leads_footer" style="float: right; position: fixed; bottom: 0; height: 70px; background: white">
                                    <tr>
                                        <td style="padding-left: 30px;">
                                            <dx:ASPxLabel Text="Select Employee:" ID="ASPxLabel1" runat="server" Font-Size="Large"></dx:ASPxLabel>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox runat="server" CssClass="edit_drop" ClientInstanceName="listboxEmployee" ID="listboxEmployee" TextField="Name" ValueField="EmployeeID" DropDownStyle="DropDownList" IncrementalFilteringMode="StartsWith">
                                                <ValidationSettings ErrorDisplayMode="None">
                                                    <RequiredField IsRequired="true" />
                                                </ValidationSettings>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td>
                                            <dx:ASPxCheckBox runat="server" Text="Archive Logs" ID="cbArchived"></dx:ASPxCheckBox>
                                        </td>
                                        <td>&nbsp;&nbsp;</td>
                                        <td>
                                            <input type="button" id="btnAssign" value="Assign" class="rand-button rand-button-blue rand-button-pad" disabled="disabled" onclick="{ if (listboxEmployee.GetIsValid()) gridLeads.PerformCallback('AssignLeads'); }" />
                                            &nbsp;&nbsp;
                                      <input type="button" value="Rules" class="rand-button rand-button-blue rand-button-pad" onclick="AssignLeadsPopupClient.Show();" />
                                            <button type="button" onclick="popupAssignRules.Show();" style="display: none">Rules Old</button>
                                        </td>
                                    </tr>
                                </table>
                            </dx:SplitterContentControl>
                        </ContentCollection>
                    </dx:SplitterPane>
                </Panes>
            </dx:SplitterPane>
            <dx:SplitterPane Name="RightPane" ScrollBars="Auto" ContentUrl="about:blank">
                <ContentCollection>
                    <dx:SplitterContentControl>
                        <%--  <uc1:LeadsInfo runat="server" ID="LeadsInfo" ShowLogPanel="false" />--%>
                    </dx:SplitterContentControl>
                </ContentCollection>
            </dx:SplitterPane>
        </Panes>
    </dx:ASPxSplitter>
    <dx:ASPxCallback runat="server" ID="updateLeadsType" ClientInstanceName="updateLeadsType" OnCallback="updateLeadsType_Callback">
        <ClientSideEvents EndCallback="function(){gridLeads.Refresh();}" />
    </dx:ASPxCallback>
    <dx:ASPxPopupControl ID="popupAssignRules" runat="server" ClientInstanceName="popupAssignRules"
        Width="630px" Height="700px" CloseAction="CloseButton" MaxWidth="800px" MinWidth="150px" ShowFooter="true"
        HeaderText="Assign Leads Rules" Modal="true" ContentUrl="~/AssignLeadsRulesPage.aspx"
        EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
        <FooterContentTemplate>
            <div style="height: 30px; vertical-align: central">
                <span class="time_buttons" onclick="popupAssignRules.Hide()">Close</span>
            </div>
        </FooterContentTemplate>
    </dx:ASPxPopupControl>
    <uc1:AssignLeadsPopup runat="server" ID="AssignLeadsPopup" />
    <uc1:LeadsSubMenu runat="server" ID="LeadsSubMenu" />
</asp:Content>
