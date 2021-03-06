﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MgrTeam.aspx.vb" Inherits="IntranetPortal.MgrTeamPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Team Management</title>
</head>
<body>
    <form id="form1" runat="server">
        <script>

            var currentTeamId = null;

            //function is called on changing focused row
            function OnGridFocusedRowChanged() {
                // The values will be returned to the OnGetRowValues() function 
                if (gvTeams.GetFocusedRowIndex() >= 0) {

                    if (lbEmployees.InCallback()) {
                        postponedCallbackRequired = true;
                    }
                    else {
                        if (gvTeams.GetFocusedRowIndex() >= 0) {
                            //alert(gridLeads.GetFocusedRowIndex());
                            var rowKey = gvTeams.GetRowKey(gvTeams.GetFocusedRowIndex());
                            if (rowKey && rowKey != null)
                                currentTeamId = rowKey;
                                lbEmployees.PerformCallback("Load|" + rowKey);
                        }
                    }
                }
            }

            function OnRemoveEmployeeClick(s,e) {
                if (currentTeamId != null)
                {
                    if (confirm("Are you sure to remove " + lbEmployees.GetValue() + "?"))
                        lbEmployees.PerformCallback("Remove|" + currentTeamId + "|" + lbEmployees.GetValue());
                }
                e.processOnServer = false;                
            }

            function OnAddEmployeeClick(s, e) {
                if (currentTeamId != null) {
                    lbEmployees.PerformCallback("Add|" + currentTeamId + "|" + cbEmps.GetText());
                }             
                e.processOnServer = false;
            }
        </script>
        <div>
            <div style="float: left; width: 1000px; margin-right: 10px;">
                <dx:ASPxRoundPanel runat="server" HeaderText="Teams" Width="100%">
                    <PanelCollection>
                        <dx:PanelContent>
                            <%--<dx:ASPxListBox Visible="false" runat="server" ID="lbRoles" Width="100%" Height="350px" AutoPostBack="true"></dx:ASPxListBox>--%>
                            <dx:ASPxGridView ClientInstanceName="gvTeams" runat="server" ID="gvTeams" KeyFieldName="TeamId" Theme="Moderno" 
                                OnRowInserting="gvTeams_RowInserting" OnRowUpdating="gvTeams_RowUpdating" 
                                OnCellEditorInitialize="gvTeams_CellEditorInitialize" OnDataBinding="gvTeams_DataBinding">
                                <Columns>
                                    <dx:GridViewDataColumn FieldName="Name"></dx:GridViewDataColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Manager"></dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataComboBoxColumn FieldName="Assistant"></dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataColumn FieldName="OfficeNo"></dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="Address"></dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="Description"></dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="LeadsCreateLimit" Width="40px" Caption="Limit"></dx:GridViewDataColumn>
                                    <dx:GridViewDataCheckColumn FieldName="Active"></dx:GridViewDataCheckColumn>
                                    <dx:GridViewCommandColumn ShowEditButton="true" VisibleIndex="0" ShowNewButtonInHeader="true" />
                                </Columns>
                                <SettingsBehavior AllowFocusedRow="true" />
                                <SettingsEditing Mode="Inline"></SettingsEditing>
                                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                <ClientSideEvents FocusedRowChanged="OnGridFocusedRowChanged" />
                            </dx:ASPxGridView>
                            <br />                      
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </div>
            <dx:ASPxRoundPanel runat="server" HeaderText="Employees" Width="300px">
                <PanelCollection>
                    <dx:PanelContent>
                        <dx:ASPxListBox runat="server" ID="lbEmployees" Width="100%" Height="350px" Theme="Moderno" ClientInstanceName="lbEmployees" OnCallback="lbEmployees_Callback"></dx:ASPxListBox>
                        <br />
                        <dx:ASPxTokenBox runat="server" Width="100%" ID="cbEmps" TextSeparator=";" Theme="Moderno" ClientInstanceName="cbEmps"></dx:ASPxTokenBox>
                        <br />
                        <dx:ASPxButton runat="server" Text="Add" ID="btnAddEmp" AutoPostBack="false" Theme="Moderno">
                            <ClientSideEvents Click="OnAddEmployeeClick" />
                        </dx:ASPxButton>
                        <dx:ASPxButton runat="server" Text="Remove" ID="btnRemoveEmp" AutoPostBack="false" Theme="Moderno">
                            <ClientSideEvents Click="OnRemoveEmployeeClick" />
                        </dx:ASPxButton>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxRoundPanel>
            <dx:ASPxLabel ID="lblError" Text="" runat="server" ForeColor="Red"></dx:ASPxLabel>
        </div>
    </form>
</body>
</html>
