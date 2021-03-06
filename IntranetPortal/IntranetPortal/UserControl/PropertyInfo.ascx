﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PropertyInfo.ascx.vb" Inherits="IntranetPortal.PropertyInfo" %>
<%@ Implements Interface="System.Web.UI.ICallbackEventHandler" %>
<%@ Register Src="~/PopupControl/VendorsPopup.ascx" TagPrefix="uc1" TagName="VendorsPopup" %>
<%@ Register Src="~/ShortSale/NGShortSaleInLeadsView.ascx" TagPrefix="uc1" TagName="NGShortSaleInLeadsView" %>
<%@ Register Src="~/UserControl/Common.ascx" TagPrefix="uc1" TagName="Common" %>
<%@ Register Src="~/PopupControl/PreAssignPopup.ascx" TagPrefix="uc1" TagName="PreAssignPopup" %>

<uc1:PreAssignPopup runat="server" ID="PreAssignPopup" />
<%--@ Register Src="~/ShortSale/ShortSaleInLeadsView.ascx" TagPrefix="uc1" TagName="ShortSaleInLeadsView" --%>

<%--<script src="/bower_components/jquery-formatcurrency/jquery.formatCurrency-1.4.0.js"></script>--%>
<script type="text/javascript">
    leadStatus = "";
    leadSubstatus = "";

    function ShowAcrisMap(propBBLE, boro, block, lot) {
        OpenLeadsWindow('/PopupControl/PropertyMap.aspx?v=1&bble=' + leadsInfoBBLE, 'Acris');
        return;
        ShowPopupMap("http://a836-acris.nyc.gov/bblsearch/bblsearch.asp?borough=" + boro + "&block=" + block + "&lot=" + lot, "Acris");
        $("#addition_info").html($("#borugh_block_lot_data").val());
    }

    function ShowDOBWindow(boro, block, lot) {
        if (block == null || block == "" || lot == null || lot == "" || boro == null || boro == "") {
            alert("The property info isn't complete. Please try to refresh data.");
            return;
        }

        var url = "http://a810-bisweb.nyc.gov/bisweb/PropertyProfileOverviewServlet?boro=" + boro + "&block=" + encodeURIComponent(block) + "&lot=" + encodeURIComponent(lot);
        ShowPopupMap(url, "DOB");
        $("#addition_info").html(' ');
    }

    function ShowPopupMap(url, header) {
        if (header == "eCourts") {
            $("#addition_info").html($('#LinesDefendantAndIndex').val());
        }

        aspxAcrisControl.SetContentHtml("Loading...");
        aspxAcrisControl.SetContentUrl(url);
        aspxAcrisControl.SetHeaderText(header);
        $('#pop_up_header_text').html(header);
        aspxAcrisControl.Show();
    }

    function ViewLeads(propBBLE) {
        var url = '/ViewLeadsInfo.aspx?id=' + propBBLE;
        window.open(url, 'View Leads Info', 'Width=1350px,Height=930px');
    }

    function init_currency() {
        if (typeof $('.input_currency').formatCurrency != "undefined") {
            $('.input_currency').formatCurrency();
        }
    }

    function SaveLeadsComments(s, e) {
        var comments = txtLeadsComments.GetText();
        leadsCommentsCallbackPanel.PerformCallback("Add|" + comments);
        txtLeadsComments.SetText("");
        aspxAddLeadsComments.Hide();
    }

    function ShowDiv() {
        var display = document.getElementById("divOtherProperties").style.display;

        if (display == "block") {
            document.getElementById("divOtherProperties").style.display = "none";
        }
        else
            document.getElementById("divOtherProperties").style.display = "block";
    }

    function DeleteComments(commentId) {
        leadsCommentsCallbackPanel.PerformCallback("Delete|" + commentId);
    }

    function RequestDocSearch() {
        $('#btnRequest').hide();
        $('#docSearchLoading').show();
        $.ajax({
            type: "POST",
            url: '/api/LeadInfoDocumentSearches',
            data: JSON.stringify({ "BBLE": leadsInfoBBLE }),
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                alert('Submit successful..');
                if (typeof gridTrackingClient != "undefined")
                    gridTrackingClient.Refresh();
                $('#docSearchLoading').hide();
                $('#waitingSearch').show();
            },
            error: function (data) {
                alert('Some error Occurred! Detail: ' + JSON.stringify(data));
                $('#btnRequest').show();
                $('#docSearchLoading').hide();
            }
        });
    }

    function openZoningUrl(zoingcode) {
        window.open("http://www.nyc.gov/html/dcp/pdf/zone/zoning_handbook/" + zoingcode + ".pdf");
    }

    LoanModStatusCtrl = {
        reload: function () {
            CallServer('getLeadsStatus' + '|' + leadsInfoBBLE);
        },
        updatesSubStatus: function () {
            $("#inprocessbtn").removeClass("btn-primary");
            $("#completedbtn").removeClass("btn-primary");
            if (leadSubstatus === "0") {
                $("#inprocessbtn").addClass("btn-primary");
            } else if (leadSubstatus === "1") {
                $("#completedbtn").addClass("btn-primary");
            }
            if (gridLeads)
                gridLeads.Refresh();
            if (gridTrackingClient)
                gridTrackingClient.Refresh();
        },
        changesSubStatus: function (substatusCode) {
            if (substatusCode != undefined) {
                var that = this;
                this.substatusCode = substatusCode;
                var substatusStr = substatusCode == 0 ? "In progress" : "Completed";
                AngularRoot.confirm("Change the Loan Mod Status to " + substatusStr + "?", function (e) {
                    if (e) {
                        leadSubstatus = String(substatusCode);
                        that.changesSubStatus_callback(that.substatusCode, that.updatesSubStatus)
                    }
                })
            }
        },
        changesSubStatus_callback: function (substatusStr, callback) {
            var url = "api/Leads/UpdateLeadsSubstatus/" + leadsInfoBBLE.trim() + "/" + substatusStr;
            $.ajax(
                {
                    type: "POST",
                    url: url,
                    contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                    success: callback
                })
        }
    }

    LeadTypeCtr = {
        initData: function () {
            this.currentType = $('#hdLeadType').val();
            this.updateType();
        },
        currentType: 10,
        updateType: function () {
            $("#btnShortSale").removeClass("btn-primary");
            $("#btnStraightSale").removeClass("btn-primary");
            $("#btnTaxliens").removeClass("btn-primary");
            $("#btnNotes").removeClass("btn-primary");
            if (LeadTypeCtr.currentType == 10) {
                $("#btnShortSale").addClass("btn-primary");
            } else if (LeadTypeCtr.currentType == 13) {
                $("#btnStraightSale").addClass("btn-primary");
            } else if (LeadTypeCtr.currentType == 3) {
                $("#btnTaxliens").addClass("btn-primary");
            } else if (LeadTypeCtr.currentType == 14) {
                $("#btnNotes").addClass("btn-primary");
            }

            TaxLiensCtr.init(LeadTypeCtr.currentType);

            if (gridTrackingClient)
                gridTrackingClient.Refresh();
        },
        changeType: function (type) {
            if (type != undefined && this.currentType != type) {
                var that = this;

                var substatusStr = type == 10 ? "Short Sale" : type == 13 ? "Straight Sale" : type == 14 ? "Mortgage Note" : "Tax Liens";
                AngularRoot.confirm("Change the Lead Type to " + substatusStr + "?", function (e) {
                    if (e) {
                        that.currentType = type;
                        var typeString = String(type);
                        that.changesType_callback(that.currentType, that.updateType)
                    }
                });
            }
        },
        changesType_callback: function (type, callback) {
            var url = "api/Leads/UpdateLeadsType/" + leadsInfoBBLE.trim() + "/" + type;
            $.ajax({
                type: "POST",
                url: url,
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                success: callback
            })
        }
    }

    function ReceiveServerData(rValue) {
        if (rValue.startsWith("getLeadsStatusResult")) {
            var temp = rValue.split("|");
            leadStatus = temp[1];
            leadSubstatus = temp[2];
            if (leadStatus && leadStatus === "20") {
                $("#loanModStatusDiv").css("visibility", "visible");
                LoanModStatusCtrl.updatesSubStatus();
            }
        }
    }

    LeadTagCtr = {
        inited: false,
        init: function () {
            var ctr = this;
            var tagData = $('#hdLeadTags').val();

            if ($("#leadTagBox").children().length == 0) {
                $("#leadTagBox").dxTagBox({
                    items: ['Mortgage Foreclosure', 'Tax lien', 'Tax Lien Lp'],
                    applyValueMode: 'useButtons',
                    value: (tagData == '' ? [] : tagData.split(';')),
                    onValueChanged: function (e) {
                        ctr.save(e.value);
                    }
                });
            }
        },
        save: function (tags) {
            var url = "api/Leads/" + leadsInfoBBLE.trim() + "/tags";
            $.ajax(
                {
                    type: "POST",
                    url: url,
                    data: JSON.stringify(tags),
                    contentType: 'application/json',
                    success: function () {
                        AngularRoot.alert('Tags is updated.');

                        if (gridTrackingClient)
                            gridTrackingClient.Refresh();
                    }
                });
        }
    }

    TaxLiensCtr = {
        init: function (type) {
            if (type == 3) {
                this.show();
            } else {
                this.hide();
            }
        },
        show: function () {
            $('#divTaxLiens').show();
            girdTaxliens.PerformCallback("load|" + leadsInfoBBLE.trim());
        },
        hide: function () {
            $('#divTaxLiens').hide();
        }
    }

    PropertyInfoCtr = {
        init: function () {
            LeadTypeCtr.initData();
            // LeadTagCtr.init();

            if (leadsInfoBBLE)
                LoanModStatusCtrl.reload();
        }
    }

    $(document).ready(function () {
        init_currency();

        PropertyInfoCtr.init();
    });
</script>

<input type="hidden" id="borugh_block_lot_data" value='(Borough: <%=  LeadsInfoData.BoroughName %> , Block:<%=LeadsInfoData.Block %> ,Lot:<%=LeadsInfoData.Lot %>)' />
<input type="hidden" id="LinesDefendantAndIndex" value='<%= LinesDefendantAndIndex()%>' />
<input type="hidden" id="hdLeadType" value="<%=IIf(LeadsInfoData.Type.HasValue, LeadsInfoData.Type, 10) %>" />
<input type="hidden" id="hdLeadTags" value="<%= LeadsInfoData.LeadsTags %>" />
<div class="tab-pane active" id="property_info" style="padding-top: 5px">
    <%--witch scroll bar--%>
    <%--/*display:none need delete when realse--%>
    <div style="height: 850px; overflow: auto;" id="prioity_content">
        <%--refresh label--%>
        <dx:ASPxPanel ID="UpatingPanel" runat="server">
            <PanelCollection>
                <dx:PanelContent runat="server">
                    <div style="margin: 30px 20px; margin-bottom: 0px; height: 30px; background: #ffefe4; color: #ff400d; border-radius: 15px; font-size: 14px; line-height: 30px;">
                        <i class="fa fa-spinner fa-spin" style="margin-left: 30px"></i>
                        <span style="padding-left: 22px">Hey! We're checking our databases for more info. We'll notify you when we find something!</span>
                    </div>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxPanel>

        <%--time label--%>
        <div style="height: 80px; font-size: 30px; margin-left: 30px; margin-top: 20px;" class="font_gray">
            <div style="font-size: 30px">
                <span>
                    <% If (LeadsInfoData.LastUpdate.HasValue) Then%>
                    <i class="fa fa-refresh"></i>
                    <span style="margin-left: 19px;">
                        <%= LeadsInfoData.LastUpdate.ToString%>
                    </span>
                    <% Else%>
                    <i class="fa fa-home"></i>
                    <span style="margin-left: 19px;"><%= LeadsInfoData.PropertyAddress %></span>
                    <% End If%>
                </span>
                <span class="time_buttons" style="margin-right: 30px" onclick="ShowPopupMap('https://iapps.courts.state.ny.us/webcivil/ecourtsMain', 'eCourts')">eCourts</span>
                <span class="time_buttons" onclick='ShowDOBWindow("<%= LeadsInfoData.Borough%>","<%= LeadsInfoData.Block%>", "<%= LeadsInfoData.Lot%>")'>DOB</span>
                <span class="time_buttons" onclick='ShowAcrisMap("<%= LeadsInfoData.BBLE %>",<%=LeadsInfoData.Borough%>,<%=LeadsInfoData.Block %>,<%=LeadsInfoData.Lot %>)'>Acris</span>
                <span class="time_buttons" onclick='ShowPropertyMap("<%= LeadsInfoData.BBLE %>")'>Maps</span>
                <%--<span class="time_buttons" onclick='preAssignPopopClient.Show()'>Pre sign</span>--%>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <% If IntranetPortal.Employee.IsManager(Page.User.Identity.Name) Then %>
                <% If not IntranetPortal.Employee.IsInOutsideTeam(Page.User.Identity.Name) %>
                <span class="time_buttons" onclick='PortalUtility.OpenWindow("/NewOffer/HomeownerIncentive.aspx?#/preassign/new?BBLE=" + leadsInfoBBLE, "Pre-Deal " + leadsInfoBBLE, 800,900)'>HOI</span>
                <span class="time_buttons" onclick='PortalUtility.ShowPopWindow("New Offer " + leadsInfoBBLE, "/NewOffer/ShortSaleNewOffer.aspx?BBLE=" + leadsInfoBBLE)'>New Offer</span>
                <% End If %>
                <span class="time_buttons" onclick='PortalUtility.ShowPopWindow("Underwriting Request" + leadsInfoBBLE, "/Underwriter/UnderwritingRequest.aspx?BBLE=<%= LeadsInfoData.BBLE %>#/")'>Request Underwriting</span>
                <% End If %>
            </div>
            <%--data format June 2, 2014 6:37 PM--%>
            <span style="font-size: 14px; margin-top: -5px; float: left; margin-left: 53px; <%= If( LeadsInfoData.CreateDate.HasValue,"visibility:visible","visibility:hidden")%>">Started on <%= LeadsInfoData.CreateDate.ToString%></span>
        </div>

        <%--note list--%>
        <div class="font_deep_gray" style="border-top: 1px solid #dde0e7; font-size: 20px">
            <dx:ASPxCallbackPanel runat="server" ID="leadsCommentsCallbackPanel" ClientInstanceName="leadsCommentsCallbackPanel" OnCallback="leadsCommentsCallbackPanel_Callback">
                <PanelCollection>
                    <dx:PanelContent>
                        <% Dim i = 0%>
                        <% If LeadsInfoData.EcourtData IsNot Nothing Then%>
                        <div class="note_item" style="background-color: #e4ffe4">
                            <% If (LeadsInfoData.EcourtData.IsNewUpdate) Then %>
                            <span style="background-color: red; color: white; border-radius: 14px; min-width: 24px; padding: 3px 7px; font-weight: 900; font-size: 14px">New</span>
                            <%Else %>
                            <i class="fa fa-exclamation-circle note_img"></i>
                            <%end if %>
                            <span class="note_text">
                                <% If LeadsInfoData.EcourtData.Active Then %>
                                Current Active Cases: <b><%= String.Join(",", LeadsInfoData.EcourtData.ActiveCaseNumbers) %> </b>
                                <% Else %>
                                <b style="color: red; font-weight: 600">No Active Cases in Ecourt. (Case Inactive <%= HumanizeTime(LeadsInfoData.EcourtData.InActiveDate) %>) </b>
                                <% End If %>
                            </span>
                            <!-- case status -->
                            <% If LeadsInfoData.EcourtData.LatestUpdatedCases IsNot Nothing %>
                            <div style="display: block; margin-left: 56px">
                                <% For each cas In LeadsInfoData.EcourtData.LatestUpdatedCases %>
                                <i class="note_img"></i>
                                <span style="font-size: 14px;">
                                    <%= String.Format("Case (<b>{0}</b>) changed to {1} on {2:d}.", cas.FormattedCaseIndexNumber, cas.CaseStatus, cas.UpdateDate) %>
                                </span>
                                <br />
                                <% Next %>
                            </div>
                            <% End if %>
                        </div>
                        <% i += 1%>
                        <%-- else %>
                        <div class="note_item" style="background-color: #e4ffe4">
                            <i class="fa fa-exclamation-circle note_img"></i>
                            <span class="note_text">
                                <b style="color: red">No Cases found in Ecourt, please check.</b>
                            </span>
                        </div>
                        <% i += 1 --%>
                        <% End If%>

                        <% If LeadsInfoData.OtherProperties IsNot Nothing AndAlso LeadsInfoData.OtherProperties.Count > 0 Then%>
                        <div class="note_item" style='<%= If((i mod 2)=0,"background: #e8e8e8;height:inherit","height:inherit")%>'>
                            <i class="fa fa-exclamation-circle note_img"></i>
                            <span class="note_text" style="cursor: pointer" onclick="ShowDiv()">Other properties of current owner: </span>
                            <div id="divOtherProperties" style="display: block">
                                <% For Each li In LeadsInfoData.OtherProperties%>
                                <i class="note_img"></i><a href="#" style="font-size: 14px" onclick="ViewLeads(<%= li.BBLE %>);"><%= li.LeadsName %></a><br />
                                <%Next%>
                            </div>
                        </div>
                        <% i += 1%>
                        <% End If%>

                        <div class="note_item" style='<%= If((i mod 2)=0,"background: #e8e8e8","")%>'>
                            <i class="fa fa-exclamation-circle note_img"></i>
                            <div style="display: inline-block;">
                                <span class="note_text">Lead Type: </span>
                                <div class="btn-group" style="margin-left: 15px">
                                    <button id="btnShortSale" type="button" class="btn btn-sm btn-default" onclick="LeadTypeCtr.changeType(10)">Short Sale</button>
                                    <button id="btnStraightSale" type="button" class="btn btn-sm btn-default" onclick="LeadTypeCtr.changeType(13)">Straight Sale</button>
                                    <button id="btnTaxliens" type="button" class="btn btn-sm btn-default" onclick="LeadTypeCtr.changeType(3)">Tax Lien</button>
                                    <button id="btnNotes" type="button" class="btn btn-sm btn-default" onclick="LeadTypeCtr.changeType(14)">Mortgage Note</button>
                                </div>
                            </div>
                        </div>
                        <% i += 1%>

                        <!-- disable the lien tags function
                        <div class="note_item" style='<%= If((i mod 2)=0,"background: #e8e8e8","")%>'>
                            <i class="fa fa-exclamation-circle note_img"></i>
                            <div style="display: inline-block;">
                                <span class="note_text">Lien Type: </span>
                                <div class="btn-group" style="margin-left: 15px">
                                    <div class="tagBox" id="leadTagBox" style="width: 500px; margin-right: 15px; display: inline-block; float: left"></div>
                                </div>
                            </div>
                        </div>
                        <% i += 1%>
                        -->
                        <asp:HiddenField ID="hfBBLE" runat="server" />
                     

                        <% For Each comment In LeadsInfoData.UserComments%>
                        <div class="note_item" style='<%= If((i mod 2)=0,"background: #e8e8e8","")%>'>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 30px">
                                        <i class="fa fa-exclamation-circle note_img"></i>
                                    </td>
                                    <td>
                                        <div class="note_text"><%= comment.Comments%></div>
                                    </td>
                                    <td style="width: 30px; padding-right: 25px;">
                                        <i class="fa fa-times" style="font-size: 18px; color: #b1b2b7; cursor: pointer" onclick="DeleteComments(<%= comment.CommentId %>)"></i>

                                    </td>
                                </tr>
                            </table>
                        </div>

                        <%-- <i class="fa fa-arrows-v" style="float: right; line-height: 40px; padding-right: 20px; font-size: 18px; color: #b1b2b7; display: none"></i>--%>

                        <% i += 1%>
                        <% Next%>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>

            <div class="note_item" style="background: white">
                <%--<button class="btn" data-container="body" type="button" data-toggle="popover" data-placement="right" data-content="Vivamus sagittis lacus vel augue laoreet rutrum faucibus.">--%>
                <i class="fa fa-plus-circle note_img tooltip-examples" title="Add Notes" style="color: #3993c1; cursor: pointer" onclick="aspxAddLeadsComments.ShowAtElement(this)"></i>
                <%--</button>--%>
            </div>
        </div>

        <dx:ASPxPopupControl ClientInstanceName="aspxAddLeadsComments" Width="550px" Height="50px" ID="ASPxPopupControl2"
            HeaderText="Add Comments" ShowHeader="false"
            runat="server" EnableViewState="false" PopupHorizontalAlign="OutsideRight" PopupVerticalAlign="Middle" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dx:PopupControlContentControl>
                    <table>
                        <tr style="padding-top: 3px;">
                            <td style="width: 380px; vertical-align: central">
                                <dx:ASPxTextBox runat="server" ID="txtLeadsComments" ClientInstanceName="txtLeadsComments" Width="360px"></dx:ASPxTextBox>
                            </td>
                            <td style="text-align: right">
                                <div>
                                    <dx:ASPxButton runat="server" ID="btnAdd" Text="Add" AutoPostBack="false" CssClass="rand-button" BackColor="#3993c1">
                                        <ClientSideEvents Click="SaveLeadsComments" />
                                    </dx:ASPxButton>
                                    &nbsp;
                                    <dx:ASPxButton runat="server" ID="ASPxButton4" Text="Close" AutoPostBack="false" CssClass="rand-button" BackColor="#77787b">
                                        <ClientSideEvents Click="function(s,e){aspxAddLeadsComments.Hide();}" />
                                    </dx:ASPxButton>
                                </div>
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>

        <%-- loan mod label  --%>
        <div id="loanModStatusDiv" class="pull-right" style="visibility: hidden; margin-right: 10px">
            <h4 style="display: inline"><span class="label" style="background-color: #ff4000">Loan Mod: </span></h4>
            <div class="btn-group">
                <button id="inprocessbtn" type="button" class="btn btn-sm btn-default" onclick="LoanModStatusCtrl.changesSubStatus(0)">In Progress</button>
                <button id="completedbtn" type="button" class="btn btn-sm btn-default" onclick="LoanModStatusCtrl.changesSubStatus(1)">Completed</button>
            </div>
        </div>

        <%--property form--%>
        <div style="margin: 20px" class="clearfix">
            <div class="form_head">General</div>

            <ul class="ss_form_box clearfix">
                <li class="ss_form_item" style="width: 66%">
                    <label class="ss_form_input_title">address</label>
                    <input class="ss_form_input" style="width: 91%" value="<%= LeadsInfoData.PropertyAddress %>">
                </li>
                <li class="ss_form_item">
                    <label class="ss_form_input_title">Unit#</label>
                    <input class="ss_form_input font_black" value="<%= LeadsInfoData.UnitNum%>">
                </li>
                <li class="ss_form_item">
                    <label class="ss_form_input_title">BBLE</label>
                    <input class="ss_form_input font_black" id="BBLEId" value="<%= If(LeadsInfoData.IsApartment, LeadsInfoData.BuildingBBLE, LeadsInfoData.BBLE)%>">
                </li>
                <li class="ss_form_item">
                    <label class="ss_form_input_title">City</label>
                    <input class="ss_form_input" value="<%= LeadsInfoData.City %>">
                </li>
                <li class="ss_form_item">
                    <label class="ss_form_input_title">Neighborhood</label>
                    <input class="ss_form_input" value="<%= LeadsInfoData.Neighborhood %>">
                </li>
                <li class="ss_form_item">
                    <label class="ss_form_input_title">Block | Lot</label>
                    <input class="ss_form_input" value="<%= LeadsInfoData.Block & "| " & LeadsInfoData.Lot %>">
                </li>
                <li class="ss_form_item">
                    <label class="ss_form_input_title">NYC SQFT</label>
                    <input class="ss_form_input" value="<%= LeadsInfoData.NYCSqft %>">
                </li>
                <li class="ss_form_item">
                    <label class="ss_form_input_title">YEAR BUILT</label>
                    <input class="ss_form_input" value="<%= LeadsInfoData.YearBuilt %>">
                </li>
                <li class="ss_form_item">
                    <label class="ss_form_input_title">BUILDING DEM</label>
                    <input class="ss_form_input" value="<%= LeadsInfoData.BuildingDem %>">
                </li>
                <li class="ss_form_item">
                    <label class="ss_form_input_title">LOT DEM</label>
                    <input class="ss_form_input" value="<%= LeadsInfoData.LotDem %>">
                </li>
                <li class="ss_form_item">
                    <label class="ss_form_input_title">STORIES</label>
                    <input class="ss_form_input" value="<%= LeadsInfoData.NumFloors%>">
                </li>
                <li class="ss_form_item">
                    <label class="ss_form_input_title">PROPERTY CLASS</label>
                    <input class="ss_form_input" value="<%= LeadsInfoData.PropertyClassCode%>">
                </li>
                <li class="ss_form_item">
                    <label class="ss_form_input_title">MAX FAR</label>
                    <input class="ss_form_input" value="<%= String.Format("{0:0.##}", LeadsInfoData.MaxFar)%>">
                </li>
                <li class="ss_form_item">
                    <label class="ss_form_input_title">
                        Zoning (<span style="color: #0e9ee9; cursor: pointer"
                            <% If (Not String.IsNullOrEmpty(LeadsInfoData.Zoning)) Then%>
                            onclick="openZoningUrl('<%= LeadsInfoData.Zoning.ToLower.Trim%>')"
                            <%End If%>>PDF</span>)</label>
                    <input class="ss_form_input" value="<%= LeadsInfoData.Zoning%>">
                </li>
                <li class="ss_form_item">
                    <label class="ss_form_input_title">UNBUILT SQFT</label>
                    <input class="ss_form_input" value="<%= String.Format("{0:0.##}", LeadsInfoData.UnbuiltSqft) %>">
                </li>
                <li class="ss_form_item">
                    <label class="ss_form_input_title">SALE DATE</label>
                    <input class="ss_form_input" value="<%= LeadsInfoData.SaleDate%>">
                </li>
                <li class="ss_form_item">
                    <label class="ss_form_input_title">ACTUAL FAR</label>
                    <input class="ss_form_input" value="<%= LeadsInfoData.ActualFar%>">
                </li>
            </ul>
        </div>

        <% If LeadsInfoData.LeadsStatus = IntranetPortal.LeadStatus.InProcess Then%>
        <uc1:NGShortSaleInLeadsView runat="server" ID="NGShortSaleInLeadsView" />
        <%  End If %>

        <%-------end-----%>
        <dx:ASPxCallbackPanel runat="server" ID="callPanelReferrel" ClientInstanceName="callPanelClientReferrel" OnCallback="callPanelReferrel_Callback">
            <PanelCollection>
                <dx:PanelContent>
                    <div style="margin: 20px;" class="clearfix">
                        <div class="form_head" style="margin-top: 40px;">REFERRAL <i class="fa fa-save  color_blue_edit collapse_btn" title="Save Referral" onclick="callPanelClientReferrel.PerformCallback('Save')"></i></div>

                        <%--line 1--%>
                        <div class="form_div_node form_div_node_line_margin">
                            <span class="form_input_title">Name</span>
                            <input class="text_input" value="<%# LeadsInfoData.Referrel.ReferrelName %>" runat="server" id="txtReferrelName" />
                        </div>

                        <div class="form_div_node form_div_node_margin form_div_node_line_margin">
                            <span class="form_input_title">Phone No.</span>
                            <input class="text_input" value="<%# LeadsInfoData.Referrel.PhoneNo %>" runat="server" id="txtReferrelPhone" />
                        </div>

                        <div class="form_div_node form_div_node_margin form_div_node_line_margin">
                            <span class="form_input_title">Email</span>
                            <input class="text_input" value="<%# LeadsInfoData.Referrel.Email %>" runat="server" id="txtReferrelEmail" />
                        </div>
                        <%----end line ----%>
                    </div>
                </dx:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="function(s,e){alert('Saved.');}" />
        </dx:ASPxCallbackPanel>

        <%--zestimat form--%>
        <div style="margin: 20px;" class="clearfix">
            <div class="form_head" style="margin-top: 40px;">Value</div>
            <%--line 1--%>
            <div class="form_div_node form_div_node_line_margin">
                <span class="form_input_title">Zestimate</span>
                <input class="text_input input_currency" onblur="$(this).formatCurrency();" type="text" value="$<%=LeadsInfoData.EstValue %>" />
            </div>
            <%----end line ----%>
        </div>

        <uc1:Common runat="server" ID="Common" />
        <dx:ASPxCallbackPanel runat="server" ID="cbpMortgageData" ClientInstanceName="callbackPanelMortgage" OnCallback="cbpMortgageData_Callback">
            <PanelCollection>
                <dx:PanelContent>
                    <%--Mortgage form--%>
                    <div style="margin: 20px;" class="clearfix">
                        <div class="form_head" style="margin-top: 40px;">
                            MORTGAGE AND VIOLATIONS 
                            <i class="fa fa-save  color_blue_edit collapse_btn tooltip-examples" title="Save Mortgage" onclick="callbackPanelMortgage.PerformCallback('Save')"></i>
                            <%--
                            <% Dim docSearch = IntranetPortal.Data.LeadInfoDocumentSearch.GetInstance(hfBBLE.Value)  %>

                            <% If docSearch IsNot Nothing AndAlso docSearch.Status = IntranetPortal.Data.LeadInfoDocumentSearch.SearchStatus.Completed Then %>
                            <i class="fa fa-eye  color_blue_edit collapse_btn tooltip-examples" title="View search result" onclick="OpenLeadsWindow('/PopupControl/LeadTaxSearchRequest.aspx?BBLE=<%=hfBBLE.Value%>','Entities',667,900)"></i>
                            <%Else %>
                            <% If IntranetPortal.Employee.IsManager(Page.User.Identity.Name) Then %>
                            <% If docSearch IsNot Nothing AndAlso docSearch.Status = IntranetPortal.Data.LeadInfoDocumentSearch.SearchStatus.NewSearch Then %>

                            <i class="fa fa-refresh fa-spin fa-fw color_blue_edit  tooltip-examples"></i>
                            <span>search in process </span>
                            <% Else %>
                            <i class="fa fa-search-plus color_blue_edit collapse_btn tooltip-examples" title="Request a search" id="btnRequest" onclick="RequestDocSearch()"></i>
                            <%End if %>
                            <span id="waitingSearch" style="display: none">
                                <i class="fa fa-refresh fa-spin fa-fw color_blue_edit  tooltip-examples"></i>
                                <span>search in process </span>
                            </span>
                            <span style="display: none" id="docSearchLoading">
                                <i class="fa fa-refresh fa-spin fa-fw color_blue_edit  tooltip-examples"></i>
                                <span>submitting </span>
                            </span>

                            <% End If %>
                            <% End If %>
                            --%>
                        </div>

                        <%--line 1--%>
                        <div class="form_div_node form_div_node_line_margin">
                            <span class="form_input_title">1st Mortgage</span>
                            <dx:ASPxTextBox runat="server" ID="txtC1stMotgr" DisplayFormatString="C" Native="true" CssClass="text_input input_currency" Text='<%#LeadsInfoData.C1stMotgrAmt  %>'></dx:ASPxTextBox>
                            <%--<input class="text_input input_currency" onblur="$(this).formatCurrency();" value="$<%= LeadsInfoData.C1stMotgrAmt%>" style="display: none" />--%>
                        </div>

                        <div class="form_div_node form_div_node_margin form_div_node_line_margin form_div_node_no_under_line clearfix">
                            <span class="form_input_title"></span>
                            <br />

                            <%--class="circle-radio-boxes"--%>
                            <input type="checkbox" id="cb1stFannie" value="Fannie" runat="server" checked='<%# LeadsInfoData.MortgageData.C1stFannie.HasValue AndAlso LeadsInfoData.MortgageData.C1stFannie %>' />
                            <label for="<%= cb1stFannie.ClientID %>" class=" form_div_radio_group">
                                <span class="form_span_group_text">Fannie</span>
                            </label>
                            <input type="checkbox" id="cb1stFHA" style="margin-left: 66px" runat="server" checked='<%# LeadsInfoData.MortgageData.C1stFHA.HasValue AndAlso LeadsInfoData.MortgageData.C1stFHA %>' />
                            <label for="<%= cb1stFHA.ClientID %>" class=" form_div_radio_group form_div_node_margin">
                                <span class="form_span_group_text">FHA</span>
                            </label>
                        </div>

                        <div class="form_div_node form_div_node_margin form_div_node_line_margin">
                            <span class="form_input_title">Servicer</span>
                            <input class="text_input" value='<%# LeadsInfoData.MortgageData.C1stServicer %>' id="txt1stServicer" runat="server" />
                        </div>
                        <%--end line --%>

                        <%--line 2--%>
                        <div class="form_div_node form_div_node_line_margin">
                            <span class="form_input_title">2nd Mortgage</span>
                            <dx:ASPxTextBox runat="server" ID="txtC2ndMotgr" DisplayFormatString="C" Native="true" CssClass="text_input input_currency" Text='<%#LeadsInfoData.C2ndMotgrAmt  %>'></dx:ASPxTextBox>
                            <%--                            <input class="text_input input_currency" onblur="$(this).formatCurrency();" value="$<%=LeadsInfoData.C2ndMotgrAmt%>" />--%>
                        </div>

                        <div class="form_div_node form_div_node_margin form_div_node_line_margin form_div_node_no_under_line clearfix">
                            <span class="form_input_title"></span>
                            <br />

                            <%--class="circle-radio-boxes"--%>
                            <input type="checkbox" id="cb2ndFannie" name="cb2ndFannie" value="Fannie" runat="server" checked='<%# LeadsInfoData.MortgageData.C2ndFannie.HasValue AndAlso LeadsInfoData.MortgageData.C2ndFannie %>' />
                            <label for="<%= cb2ndFannie.ClientID %>" class=" form_div_radio_group">
                                <span class="form_span_group_text">Fannie</span>
                            </label>
                            <input type="checkbox" id="cb2ndFHA" name="cb2ndFHA" value="FHA" style="margin-left: 66px" runat="server" checked='<%# LeadsInfoData.MortgageData.C2ndFHA.HasValue AndAlso LeadsInfoData.MortgageData.C2ndFHA %>' />
                            <label for="<%= cb2ndFHA.ClientID %>" class=" form_div_radio_group form_div_node_margin">
                                <span class="form_span_group_text">FHA</span>
                            </label>
                        </div>

                        <div class="form_div_node form_div_node_margin form_div_node_line_margin">
                            <span class="form_input_title">Servicer</span>
                            <input class="text_input" value="<%# LeadsInfoData.MortgageData.C2ndServicer %>" id="txt2ndServicer" runat="server" />
                        </div>

                        <%----end line ----%>

                        <%--line 3--%>

                        <div class="form_div_node form_div_node_line_margin">
                            <span class="form_input_title">3rd Mortgage</span>
                            <dx:ASPxTextBox runat="server" ID="txtC3rdMotgr" DisplayFormatString="C" Native="true" CssClass="text_input input_currency" Text='<%#LeadsInfoData.C3rdMortgrAmt  %>'></dx:ASPxTextBox>
                        </div>

                        <div class="form_div_node form_div_node_margin form_div_node_line_margin form_div_node_no_under_line clearfix">
                            <span class="form_input_title"></span>
                            <br />

                            <%--class="circle-radio-boxes"--%>
                            <input type="checkbox" id="cb3rdFannie" name="cb3rdFannie" value="Fannie" runat="server" checked='<%# LeadsInfoData.MortgageData.C3rdFannie.HasValue AndAlso LeadsInfoData.MortgageData.C3rdFannie %>' />
                            <label for="<%= cb3rdFannie.ClientID %>" class=" form_div_radio_group">
                                <span class="form_span_group_text">Fannie</span>
                            </label>
                            <input type="checkbox" id="cb3rdFHA" name="cb3rdFHA" value="" style="margin-left: 66px" runat="server" checked='<%# LeadsInfoData.MortgageData.C3rdFHA.HasValue AndAlso LeadsInfoData.MortgageData.C3rdFHA%>' />
                            <label for="<%= cb3rdFHA.ClientID %>" class=" form_div_radio_group form_div_node_margin">
                                <span class="form_span_group_text">FHA</span>
                            </label>
                        </div>

                        <div class="form_div_node form_div_node_margin form_div_node_line_margin">
                            <span class="form_input_title">Servicer</span>
                            <input class="text_input" value="<%# LeadsInfoData.MortgageData.C3rdServicer%>" id="txt3rdServicer" runat="server" />
                        </div>

                        <div class="form_div_node form_div_node_line_margin form_div_node_small">
                            <span class="form_input_title">Taxes</span>
                            <dx:ASPxTextBox runat="server" ID="txtTaxesAmt" DisplayFormatString="C" Native="true" CssClass="text_input input_currency" Text='<%#LeadsInfoData.TaxesAmt  %>'></dx:ASPxTextBox>
                            <%--<input class="text_input input_currency" onblur="$(this).formatCurrency();" value="$<%=LeadsInfoData.TaxesAmt%>" />--%>
                        </div>

                        <div class="form_div_node form_div_node_line_margin form_div_node_small" style="border: none; visibility: hidden">
                            <div class="form_head" style="margin-left: 20px">
                                Tax Liens:
                            </div>
                        </div>
                        <div class="form_div_node form_div_node_line_margin form_div_node_small" style="border: none">
                            <span class="form_input_title">&nbsp;</span>
                            <input class="text_input" value=" " />
                        </div>
                        <div class="form_div_node form_div_node_line_margin">
                            <span class="form_input_title">water</span>
                            <dx:ASPxTextBox runat="server" ID="txtWaterAmt" DisplayFormatString="C" Native="true" CssClass="text_input input_currency" Text='<%#LeadsInfoData.WaterAmt  %>'></dx:ASPxTextBox>
                        </div>
                        <div class="form_div_node form_div_node_margin form_div_node_line_margin" style="visibility: hidden">
                            <span class="form_input_title">Tax Liens Date</span>
                            <input class="text_input" value="<%= LeadsInfoData.TaxLiensDateText %>" id="Text1" />
                        </div>
                        <div class="form_div_node form_div_node_margin form_div_node_line_margin" style="visibility: hidden">
                            <span class="form_input_title">Tax Liens Amount</span>
                            <input class="text_input" value="<%= LeadsInfoData.TaxLiensAmount %>" id="Text2" />
                        </div>

                        <%----end line ----%>
                        <div style="width: 230px" class="clearfix">
                            <div class="form_div_node form_div_node_line_margin form_div_node_small">
                                <span class="form_input_title">ecb/dob</span>
                                <dx:ASPxTextBox runat="server" ID="txtViolationAmt" DisplayFormatString="C" Native="true" CssClass="text_input input_currency" Text='<%#LeadsInfoData.ViolationAmount  %>'></dx:ASPxTextBox>
                                <%--<input class="text_input" value="<%= LeadsInfoData.ViolationAmount %>" />--%>
                            </div>
                            <%--line 7--%>
                            <div class="form_div_node form_div_node_line_margin form_div_node_small">
                                <span class="form_input_title" style="color: #ff400d">Total debt</span>
                                <dx:ASPxTextBox runat="server" ID="ASPxTextBox1" DisplayFormatString="C" Native="true" CssClass="text_input input_currency" Text='<%#LeadsInfoData.TotalDebt  %>'></dx:ASPxTextBox>
                            </div>
                            <%----end line ----%>
                        </div>
                        <%----end line ----%>
                    </div>
                    <%-------end-----%>
                </dx:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="function(s,e){alert('Saved.');}" />
        </dx:ASPxCallbackPanel>
        

        <% If (LeadsInfoData.LisPens IsNot Nothing AndAlso LeadsInfoData.LisPens.Count > 0) Then%>
        <div style="margin: 20px;" class="clearfix">
            <div class="form_head" style="margin-top: 40px;">Estimated Mortgage Default</div>
            <%--line 1--%>
            <div class="form_div_node form_div_node_line_margin">
                <span class="form_input_title">Default</span>
                <input class="text_input input_currency" onblur="$(this).formatCurrency();" type="text" value="<%=String.Format("{0:C}", LeadsInfoData.EstimatedMortageDefault) %>" />
            </div>
            <%----end line ----%>
        </div>
        <%End If%>

        <% If LeadsInfoData.PropertyNotes IsNot Nothing AndAlso LeadsInfoData.PropertyNotes.Count > 0Then %>
        <% dim propNote = LeadsInfoData.PropertyNotes(0) %>
        <div style="margin: 20px;" class="clearfix">
            <div class="form_head" style="margin-top: 40px;">Mortgage Notes</div>

            <div class="form_div_node form_div_node_line_margin">
                <span class="form_input_title">BPO Value</span>
                <input class="text_input" value="<%= string.Format("{0:c}", propNote.BPOValue) %>" />
            </div>

            <div class="form_div_node form_div_node_margin form_div_node_line_margin">
                <span class="form_input_title">FC Status</span>
                <input class="text_input" value="<%= propNote.FCStatus  %>" />
            </div>

            <div class="form_div_node form_div_node_margin form_div_node_line_margin">
                <span class="form_input_title">Last Paid Installment</span>
                <input class="text_input" value="<%= string.Format("{0:d}", propNote.LastPaidInstallmentDt) %>" />
            </div>

            <div class="form_div_node form_div_node_line_margin">
                <span class="form_input_title">Current Occupancy</span>
                <input class="text_input" value="<%= propNote.CurrentOccupancy %>" />
            </div>

            <div class="form_div_node form_div_node_margin form_div_node_line_margin">
                <span class="form_input_title">DelinquentInterest</span>
                <input class="text_input" value="<%= string.Format("{0:c}", propNote.DelinquentInterest)  %>" />
            </div>

            <div class="form_div_node form_div_node_margin form_div_node_line_margin">
                <span class="form_input_title">CurrentUPB</span>
                <input class="text_input" value="<%= string.Format("{0:c}", propNote.CurrentUPB) %>" />
            </div>

            <div class="form_div_node form_div_node_line_margin">
                <span class="form_input_title">Forbearance Amount</span>
                <input class="text_input" value="<%= string.Format("{0:c}", propNote.ForbearanceAmount) %>" />
            </div>

            <div class="form_div_node form_div_node_margin form_div_node_line_margin">
                <span class="form_input_title">EscrowAdvanceBalance</span>
                <input class="text_input" value="<%= string.Format("{0:c}", propNote.EscrowAdvanceBalance)  %>" />
            </div>

            <div class="form_div_node form_div_node_margin form_div_node_line_margin">
                <span class="form_input_title">Total Due</span>
                <input class="text_input" value="<%= string.Format("{0:c}", propNote.TotalDue) %>" />
            </div>
        </div>
        <%  End if %>
        <%--Liens table--%>
        <div style="margin: 20px;" class="clearfix">
            <div class="form_head" style="margin-top: 40px;">Mortgages Liens</div>
            <dx:ASPxGridView runat="server" ID="gridLiens" KeyFieldName="LisPenID" Width="100%" ViewStateMode="Disabled">
                <SettingsBehavior AllowDragDrop="false" AllowSort="false" AllowGroup="false" />
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="Type" Settings-AllowSort="False"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Effective" Settings-AllowSort="False">
                        <DataItemTemplate>
                            <%# DateTime.Parse(Eval("Effective")).ToShortDateString %>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn FieldName="Expiration" PropertiesDateEdit-DisplayFormatString="g" Settings-AllowSort="False"></dx:GridViewDataDateColumn>
                    <dx:GridViewDataTextColumn FieldName="Plaintiff" Settings-AllowSort="False"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Defendant" Settings-AllowSort="False"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Index" Settings-AllowSort="False"></dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
        </div>
        <%--end--%>

        <%-- Tax liens --%>
        <div style="margin: 20px; display: none" class="clearfix" id="divTaxLiens">
            <div class="form_head" style="margin-top: 40px;">Tax Liens</div>
            <dx:ASPxGridView runat="server" ClientInstanceName="girdTaxliens"
                OnCustomCallback="ASPxGridView1_CustomCallback" ID="ASPxGridView1"
                KeyFieldName="Year" Width="100%" ViewStateMode="Disabled">
                <SettingsBehavior AllowDragDrop="false" AllowSort="false" AllowGroup="false" />
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="Year" Settings-AllowSort="False"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Property" PropertiesTextEdit-DisplayFormatString="c2" Settings-AllowSort="False"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="CIS" PropertiesTextEdit-DisplayFormatString="c2" Settings-AllowSort="False"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="NoticingFees" Settings-AllowSort="False" PropertiesTextEdit-DisplayFormatString="c2"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Surchages" Settings-AllowSort="False" PropertiesTextEdit-DisplayFormatString="c2"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="LienTotal" Settings-AllowSort="False" PropertiesTextEdit-DisplayFormatString="c2"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="InterestRate" Settings-AllowSort="False" PropertiesTextEdit-DisplayFormatString="p1"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Schedule" Settings-AllowSort="False"></dx:GridViewDataTextColumn>
                </Columns>
            </dx:ASPxGridView>
        </div>
        <%-- end --%>
    </div>
</div>
<!-- custom scrollbar plugin -->

<dx:ASPxPopupControl ClientInstanceName="aspxAcrisControl" Width="1000px" Height="800px"
    ID="ASPxPopupControl1" HeaderText="Acris" Modal="true" CloseAction="CloseButton" ShowMaximizeButton="true"
    runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
    <HeaderTemplate>
        <div class="clearfix">
            <div class="pop_up_header_margin">
                <i class="fa fa-tasks with_circle pop_up_header_icon"></i>
                <span class="pop_up_header_text" id="pop_up_header_text">Acris</span> <span class="pop_up_header_text"><%= LeadsInfoData.PropertyAddress%> </span>
            </div>
            <div class="pop_up_buttons_div">
                <i class="fa fa-times icon_btn" onclick="aspxAcrisControl.Hide()"></i>
            </div>
        </div>
        <div style="text-align: center" id="addition_info"></div>
    </HeaderTemplate>
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

