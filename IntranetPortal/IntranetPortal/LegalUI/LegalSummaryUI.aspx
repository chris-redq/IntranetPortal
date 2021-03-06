﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LegalSummaryUI.aspx.vb" Inherits="IntranetPortal.LegalSummaryUI" MasterPageFile="~/Content.Master" %>

<%@ Register Src="~/UserControl/Common.ascx" TagPrefix="uc1" TagName="Common" %>



<asp:Content runat="server" ContentPlaceHolderID="head">
    <script>
        function SearchGrid() {

            var filterCondition = "";
            var key = document.getElementById("QuickSearch").value;

            if (key.trim() == "") {
                AllLeadsGridClient.ClearFilter();
                if (MangerReportGridClient.GetVisible())
                {
                    MangerReportGridClient.ClearFilter();
                }
                return;
            }

            filterCondition = "[CaseName] LIKE '%" + key + "%' OR [CaseName] LIKE '%" + key + "%'";
            filterCondition += " OR [ResearchBy] LIKE '%" + key + "%'";
            filterCondition += " OR [Attorney] LIKE '%" + key + "%'";
            filterCondition += " OR [BBLE] LIKE '%" + key + "%'";
            AllLeadsGridClient.ApplyFilter(filterCondition);
            if (MangerReportGridClient.GetVisible())
            {
                MangerReportGridClient.ApplyFilter(filterCondition)
            }
            return false;
        }
        function ShowCaseInfo(BBLE) {
            var url = '/LegalUI/LegalUI.aspx?bble=' + BBLE;
            OpenLeadsWindow(url, 'Legal')
        }
        function SwitchToMangerReport()
        {
            AllLeadsGridClient.SetVisible(!AllLeadsGridClient.GetVisible());
            MangerReportGridClient.SetVisible(!MangerReportGridClient.GetVisible());
            var mangerMode = MangerReportGridClient.GetVisible();
            $("#mangerExport").css("display", mangerMode ? "" : "none");
            $("#Export").css("display", !mangerMode ? "" : "none");
            if (MangerReportGridClient.GetVisible())
            {
                document.getElementById('topGrid').className = 'col-md-12'
              
               
            } else {
                document.getElementById('topGrid').className = 'col-md-8'
            }
            MangerReportGridClient.PerformCallback();
        }
        $(document).ready(function()
        {
            MangerReportGridClient.SetVisible(false);
            var mangerMode = MangerReportGridClient.GetVisible();
            $("#mangerExport").css("display", mangerMode ? "" : "none");
        }
        )
    </script>
</asp:Content>


<asp:Content ContentPlaceHolderID="MainContentPH" runat="server">
    <uc1:Common runat="server" ID="Common" />
    <div class=" container-fluid wx_scorll_list" style="padding: 20px 30px" data-bottom="60">
        <div>
            <div class="row">
                <div class="col-md-8" id="topGrid">
                    <div class="row">
                        <div class="col-md-8">
                            <h3>Available LEGAL FILES</h3>
                        </div>
                        <div class="col-md-4  form-inline">
                            <input type="text" style="margin-right: 20px" id="QuickSearch" class="form-control" placeholder="Quick Search" onkeydown="javascript:if(event.keyCode == 13){ SearchGrid(); return false;}" />
                            <i class="fa fa-search icon_btn tooltip-examples  grid_buttons" style="margin-right: 20px; font-size: 19px" onclick="SearchGrid()" title="search"></i>
                            <asp:LinkButton ID="lbExportExcel" runat="server" Text="<i class='fa fa fa-file-excel-o report_head_button report_head_button_padding tooltip-examples' id='Export' title='export to excel'></i>" OnClick="lbExportExcel_Click"></asp:LinkButton>
                            <asp:LinkButton ID="mangerExport" runat="server" Text="<i class='fa fa fa-file-excel-o report_head_button report_head_button_padding tooltip-examples' id='mangerExport' title='export to excel'></i>" OnClick="mangerExport_Click"></asp:LinkButton>
                            <%If Page.User.IsInRole("Admin") Or Page.User.IsInRole("Legal-Manager") Then%> 
                            <i class='fa fa-user report_head_button report_head_button_padding tooltip-examples' title='switch to manager report It may be take long time becase a lot calculate' onclick="SwitchToMangerReport()"></i>
                            <%End If %>
                        </div>
                    </div>
                    <div class="row" style="margin-top: 10px">
                        <dx:ASPxGridView ID="gdCases" runat="server" KeyFieldName="BBLE" Theme="Moderno" CssClass="table" ClientInstanceName="AllLeadsGridClient" OnDataBinding="gdCases_DataBinding">
                            <Columns>
                                <dx:GridViewDataColumn FieldName="BBLE" Visible="false">
                                    <Settings HeaderFilterMode="CheckedList" />

                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn FieldName="CaseName">
                                    <Settings HeaderFilterMode="CheckedList" />
                                    <DataItemTemplate>
                                        <div style="cursor: pointer;" class="font_black" onclick='<%# String.Format("ShowCaseInfo({0})", Eval("BBLE"))%>'><%# Eval("CaseName")%></div>
                                    </DataItemTemplate>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn FieldName="LegalStatusString" Caption="Case Status">
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn FieldName="StuatsStr" Caption="Process Stauts">
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn FieldName="ResearchBy" Caption="Research">
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn FieldName="Attorney">
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataColumn>

                            </Columns>

                            <Settings ShowHeaderFilterButton="true" />

                        </dx:ASPxGridView>
                        <dx:ASPxGridView ID="MangerReportGrid" runat="server" KeyFieldName="BBLE" Theme="Moderno" CssClass="table" ClientInstanceName="MangerReportGridClient" OnDataBinding="MangerReportGrid_DataBinding" OnCustomCallback="MangerReportGrid_CustomCallback">
                            <Columns>
                                <dx:GridViewDataColumn FieldName="BBLE" Visible="false">
                                    <Settings HeaderFilterMode="CheckedList" />

                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn FieldName="CaseName">
                                    <Settings HeaderFilterMode="CheckedList" />
                                    <DataItemTemplate>
                                        <div style="cursor: pointer;" class="font_black" onclick='<%# String.Format("ShowCaseInfo({0})", Eval("BBLE"))%>'><%# Eval("CaseName")%></div>
                                    </DataItemTemplate>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn FieldName="LegalStatusString" Caption="Case Status">
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn FieldName="StuatsStr" Caption="Process Status">
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn FieldName="JSecondaryActions" Caption="Secondary Actions">
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataColumn>
                                
                                <dx:GridViewDataColumn FieldName="ResearchBy" Caption="Research">
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn FieldName="Attorney">
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataColumn>
                               
                                <dx:GridViewDataColumn FieldName="NextStep" >
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataColumn>
                                  <dx:GridViewDataColumn FieldName="UpdateDate" Caption="Date Worked">
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn FieldName="FollowUp" Caption="Follow Up date">
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn FieldName="Agent">
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn Caption="MiscComments" FieldName="MiscCommentsWithOurtHtml" >
                                    <DataItemTemplate>
                                        <div><%# Eval("MiscComments") %></div>
                                    </DataItemTemplate>
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn FieldName="Team">
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataColumn>
                                

                            </Columns>

                            <Settings ShowHeaderFilterButton="true" />
                        </dx:ASPxGridView>

                        <dx:ASPxGridViewExporter ID="CaseExporter" runat="server" GridViewID="gdCases"></dx:ASPxGridViewExporter>
                        <dx:ASPxGridViewExporter ID="MangerReportExporter" runat="server" GridViewID="MangerReportGrid"></dx:ASPxGridViewExporter>
                    </div>
                </div>
                <div class="col-md-4">
                    <h3>Upcomming FC Sale / Auction Dates  </h3>
                    <dx:ASPxGridView ID="gridUpCommingFCSale" runat="server" KeyFieldName="BBLE" OnDataBinding="gridUpCommingFCSale_DataBinding">
                        <Columns>
                            <dx:GridViewDataColumn FieldName="PropertyAddress">
                                <DataItemTemplate>
                                    <div style="cursor: pointer;" class="font_black" onclick='<%# String.Format("ShowCaseInfo({0})", Eval("BBLE"))%>'><%# Eval("PropertyAddress")%></div>
                                </DataItemTemplate>
                            </dx:GridViewDataColumn>
                            <dx:GridViewDataColumn FieldName="SaleDate"></dx:GridViewDataColumn>
                        </Columns>
                        <SettingsPager PageSize="10"></SettingsPager>
                    </dx:ASPxGridView>
                </div>

            </div>
            <div class="row">
                <div class="col-md-2">
                    <h3>OSC's</h3>
                    <dx:ASPxGridView ID="gridOSCs" KeyFieldName="BBLE" runat="server" OnDataBinding="gridOSCs_DataBinding" Settings-ShowColumnHeaders="false">
                        <Columns>
                            <dx:GridViewDataColumn FieldName="CaseName">
                                <Settings HeaderFilterMode="CheckedList" />
                                <DataItemTemplate>
                                    <div style="cursor: pointer;" class="font_black" onclick='<%# String.Format("ShowCaseInfo({0})", Eval("BBLE"))%>'><%# Eval("CaseName")%></div>
                                </DataItemTemplate>
                            </dx:GridViewDataColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </div>
                <div class="col-md-2">
                    <h3>Partitions</h3>
                    <dx:ASPxGridView ID="gridPartitions" KeyFieldName="BBLE" runat="server" OnDataBinding="gridPartitions_DataBinding" Settings-ShowColumnHeaders="false" >
                          <Columns>
                            <dx:GridViewDataColumn FieldName="CaseName">
                                <Settings HeaderFilterMode="CheckedList" />
                                <DataItemTemplate>
                                    <div style="cursor: pointer;" class="font_black" onclick='<%# String.Format("ShowCaseInfo({0})", Eval("BBLE"))%>'><%# Eval("CaseName")%></div>
                                </DataItemTemplate>
                            </dx:GridViewDataColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </div>
                <div class="col-md-2">
                    <h3>QTA's</h3>
                    <dx:ASPxGridView ID="gridQTAs" runat="server" KeyFieldName="BBLE" OnDataBinding="gridQTAs_DataBinding" Settings-ShowColumnHeaders="false">
                          <Columns>
                            <dx:GridViewDataColumn FieldName="CaseName">
                                <Settings HeaderFilterMode="CheckedList" />
                                <DataItemTemplate>
                                    <div style="cursor: pointer;" class="font_black" onclick='<%# String.Format("ShowCaseInfo({0})", Eval("BBLE"))%>'><%# Eval("CaseName")%></div>
                                </DataItemTemplate>
                            </dx:GridViewDataColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </div>
                <div class="col-md-2">
                    <h3>Deed Reversions</h3>
                    <dx:ASPxGridView ID="gridDeedReversions" KeyFieldName="BBLE" runat="server" OnDataBinding="gridDeedReversions_DataBinding" Settings-ShowColumnHeaders="false">
                          <Columns>
                            <dx:GridViewDataColumn FieldName="CaseName">
                                <Settings HeaderFilterMode="CheckedList" />
                                <DataItemTemplate>
                                    <div style="cursor: pointer;" class="font_black" onclick='<%# String.Format("ShowCaseInfo({0})", Eval("BBLE"))%>'><%# Eval("CaseName")%></div>
                                </DataItemTemplate>
                            </dx:GridViewDataColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </div>
                <div class="col-md-2">
                    <h3>SP's and other Misc. actions</h3>
                    <dx:ASPxGridView ID="gridSPAndOther" KeyFieldName="BBLE" runat="server" OnDataBinding="gridSPAndOther_DataBinding" Settings-ShowColumnHeaders="false">
                          <Columns>
                            <dx:GridViewDataColumn FieldName="CaseName">
                                <Settings HeaderFilterMode="CheckedList" />
                                <DataItemTemplate>
                                    <div style="cursor: pointer;" class="font_black" onclick='<%# String.Format("ShowCaseInfo({0})", Eval("BBLE"))%>'><%# Eval("CaseName")%></div>
                                </DataItemTemplate>
                            </dx:GridViewDataColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </div>
            </div>
        </div>

    </div>

</asp:Content>
