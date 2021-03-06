﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ShortSaleOverVew.ascx.vb" Inherits="IntranetPortal.ShortSaleOverVew" %>
<%@ Import Namespace="IntranetPortal" %>
<%@ Register Src="~/ShortSale/ShortSaleSummaryTab.ascx" TagPrefix="uc1" TagName="ShortSaleSummaryTab" %>
<%@ Register Src="~/ShortSale/ShortSalePropertyTab.ascx" TagPrefix="uc1" TagName="ShortSalePropertyTab" %>
<%@ Register Src="~/ShortSale/ShortSalePartiesTab.ascx" TagPrefix="uc1" TagName="ShortSalePartiesTab" %>
<%@ Register Src="~/ShortSale/ShortSaleEvictionTab.ascx" TagPrefix="uc1" TagName="ShortSaleEvictionTab" %>
<%@ Register Src="~/ShortSale/ShortSaleHomewonerTab.ascx" TagPrefix="uc1" TagName="ShortSaleHomewonerTab" %>
<%@ Register Src="~/ShortSale/ShortSaleMortgageTab.ascx" TagPrefix="uc1" TagName="ShortSaleMortgageTab" %>
<%@ Register Src="~/PopupControl/VendorsPopup.ascx" TagPrefix="uc1" TagName="VendorsPopup" %>

    <%--<script src="/bower_components/jquery-formatcurrency/jquery.formatCurrency-1.4.0.js"></script>--%>
<script type="text/javascript">
    function init_currency() {
        $('.input_currency').formatCurrency();
    }
    $(document).ready(function () {
        // Handler for .ready() called.
        init_currency();
    });
    var short_sale_case_data = null;

    function getShortSaleInstanceComplete(s, e) {
        debugger;
        short_sale_case_data = e != null ? $.parseJSON(e.result) : ShortSaleCaseData; //ShortSaleCaseData;//;
        ShortSaleCaseData = short_sale_case_data;
        short_sale_case_data.PropertyInfo.UpdateBy = "<%=Page.User.Identity.Name%>";


        ShortSaleDataBand(1);

        clearHomeOwner();
        //console.log("the data is give to save is 222", JSON.stringify(ShortSaleCaseData));
        var strJson = JSON.stringify(ShortSaleCaseData);

        //d_alert(strJson);
        if (e == null) {
            SaveClicklCallbackCallbackClinet.PerformCallback(strJson);
        }
    }
    function saveComplete(s, e) {
        //RefreshContent();
        ShortSaleCaseData = $.parseJSON(e.result);
        clearArray(ShortSaleCaseData.Mortgages);
        clearArray(ShortSaleCaseData.PropertyInfo.Owners);

        ShortSaleDataBand(2);
    }

    function ShowAcrisMap(propBBLE) {
        //var url = "http://www.oasisnyc.net/map.aspx?zoomto=lot:" + propBBLE;
        ShowPopupMap("https://a836-acris.nyc.gov/DS/DocumentSearch/BBL", "Acris");
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
        aspxAcrisControl.SetContentHtml("Loading...");
        aspxAcrisControl.SetContentUrl(url);

        aspxAcrisControl.SetHeaderText(header);
        //header = header + "(Borough:" + ShortSaleCaseData.PropertyInfo.Borough + "Lot:" + ShortSaleCaseData.PropertyInfo.Lot + ")";
        $('#pop_up_header_text').html(header)
        aspxAcrisControl.Show();
    }

    function SaveLeadsComments(s, e) {
        var comments = txtLeadsComments.GetText();
        leadsCommentsCallbackPanel.PerformCallback("Add|" + comments);
        txtLeadsComments.SetText("");
        aspxAddLeadsComments.Hide();
    }

    function DeleteComments(commentId) {
        leadsCommentsCallbackPanel.PerformCallback("Delete|" + commentId);
    }

</script>
<dx:ASPxCallback ID="SaveClicklCallback" ClientInstanceName="SaveClicklCallbackCallbackClinet" runat="server" OnCallback="SaveClicklCallback_Callback">
    <ClientSideEvents CallbackComplete="saveComplete" />
</dx:ASPxCallback>
<dx:ASPxCallback ID="getShortSaleInstance" ClientInstanceName="getShortSaleInstanceClient" runat="server" OnCallback="getShortSaleInstance_Callback">
    <ClientSideEvents CallbackComplete="getShortSaleInstanceComplete" />
</dx:ASPxCallback>

<dx:ASPxCallbackPanel ID="ShortSaleCaseSavePanel" ClientInstanceName="ShortSaleCaseSavePanelClient" runat="server" Width="100%">
    <PanelCollection>
        <dx:PanelContent>
            <input hidden id="short_sale_case_id" value="<%=shortSaleCaseData.CaseId %>" />
            <div style="padding-top: 5px">
                <div style="height: 850px; overflow: auto;" id="prioity_content">
                    <%--time label--%>

                    <div style="height: 80px; font-size: 30px; margin-left: 30px; margin-top: 20px;" class="font_gray">
                        <div style="font-size: 30px">
                            <i class="fa fa-user"></i>
                            <span style="margin-left: 19px;">
                                <% If (Not String.IsNullOrEmpty(shortSaleCaseData.CaseName)) Then%>

                                <%= Regex.Replace( shortSaleCaseData.CaseName,"-(?!.*-).*$","")%>
                                <% End If%>
                                &nbsp;
                            </span>

                            <% If shortSaleCaseData.PropertyInfo IsNot Nothing Then%>
                            <span class="time_buttons" style="margin-right: 30px" onclick="ShowPopupMap('https://iapps.courts.state.ny.us/webcivil/ecourtsMain', 'eCourts')">eCourts</span>
                            <span class="time_buttons" onclick='ShowDOBWindow("<%= shortSaleCaseData.PropertyInfo.Borough%>","<%= shortSaleCaseData.PropertyInfo.Block%>", "<%= shortSaleCaseData.PropertyInfo.Lot%>")'>DOB</span>
                            <span class="time_buttons" onclick='ShowAcrisMap("<%= shortSaleCaseData.BBLE %>")'>Acris</span>
                            <span class="time_buttons" onclick='ShowPropertyMap("<%= shortSaleCaseData.BBLE %>")'>Maps</span>
                            <% End If%>
                        </div>
                        <%--data format June 2, 2014 6:37 PM--%>
                        <span style="font-size: 14px; margin-top: -5px; float: left; margin-left: 53px;"><%=If(String.IsNullOrEmpty(shortSaleCaseData.OwnerFirstName),"",shortSaleCaseData.OwnerFirstName &" " &shortSaleCaseData.OwnerLastName) %></span>
                    </div>

                    <%--note list--%>
                    <div class="font_deep_gray" style="border-top: 1px solid #dde0e7; font-size: 20px">
                        <dx:ASPxCallbackPanel runat="server" ID="leadsCommentsCallbackPanel" ClientInstanceName="leadsCommentsCallbackPanel" OnCallback="leadsCommentsCallbackPanel_Callback">
                            <PanelCollection>
                                <dx:PanelContent>
                                    <% Dim i = 0%>
                                    <asp:HiddenField ID="hfCaseId" runat="server" />
                                    <% For Each comment In shortSaleCaseData.Comments%>
                                    <div class="note_item" style='<%= If((i mod 2)=0,"background: #e8e8e8","")%>'>
                                        <i class="fa fa-exclamation-circle note_img"></i>
                                        <span class="note_text"><%= comment.Comments%></span>
                                        <i class="fa fa-arrows-v" style="float: right; line-height: 40px; padding-right: 20px; font-size: 18px; color: #b1b2b7; display: none"></i>
                                        <i class="fa fa-times" style="float: right; padding-right: 25px; line-height: 40px; font-size: 18px; color: #b1b2b7; cursor: pointer" onclick="DeleteComments(<%= comment.CommentId %>)"></i>
                                    </div>
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
                                                <input type="button" value="Add" onclick="SaveLeadsComments()" class="rand-button" style="background-color: #3993c1" />
                                                <dx:ASPxButton runat="server" ID="btnAdd" Text="Add" AutoPostBack="false" CssClass="rand-button" BackColor="#3993c1" Visible="false">
                                                    <ClientSideEvents Click="SaveLeadsComments" />
                                                </dx:ASPxButton>
                                                &nbsp;
                                    <dx:ASPxButton runat="server" ID="ASPxButton4" Text="Close" AutoPostBack="false" CssClass="rand-button" BackColor="#77787b" Visible="false">
                                        <ClientSideEvents Click="function(s,e){aspxAddLeadsComments.Hide();}" />
                                    </dx:ASPxButton>
                                                <input type="button" value="Close" onclick="aspxAddLeadsComments.Hide()" class="rand-button" style="background-color: #3993c1" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
                    <%--detial tabs--%>
                    <div>
                        <!--detial Nav tabs -->
                        <style>
                            /*.shot_sale_tab_a {
                        important;
                        }*/
                        </style>
                        <ul class="nav nav-tabs overview_tabs" role="tablist" style='<%= If(isEviction,"display:none","") %>'>
                            <li class="active short_sale_tab">
                                <a class="shot_sale_tab_a" href="#home" role="tab" data-toggle="tab" onclick="refreshSummary()">Summary</a></li>
                            <li class="short_sale_tab"><a class="shot_sale_tab_a " href="#Property" role="tab" data-toggle="tab">Property Info</a></li>
                            <li class="short_sale_tab"><a class="shot_sale_tab_a " href="#Mortgages" role="tab" data-toggle="tab">Mortgages</a></li>
                            <li class="short_sale_tab"><a class="shot_sale_tab_a " href="#DealInfo" role="tab" data-toggle="tab">Deal Info</a></li>
                            <li class="short_sale_tab"><a class="shot_sale_tab_a " href="#Homewoner" role="tab" data-toggle="tab">Homeowner</a></li>
                            <%--<li class="short_sale_tab"><a class="shot_sale_tab_a  " href="#Eviction" role="tab" data-toggle="tab">Eviction</a></li>--%>
                            <li class="short_sale_tab"><a class="shot_sale_tab_a " href="#Parties" role="tab" data-toggle="tab">Parties</a></li>
                        </ul>
                        <%--<dx:ASPxCallbackPanel ID="overviewCallbackPanel" runat="server" ClientInstanceName="overviewCallbackPanelClinet" OnCallback="overviewCallbackPanel_Callback">--%>
                        <!-- Tab panes -->
                        <div class="tab-content">
                            <div class="tab-pane <%= If(isEviction,"","active") %>" id="home">
                                <div class="short_sale_content">

                                    <uc1:ShortSaleSummaryTab runat="server" ID="ShortSaleSummaryTab" />
                                </div>
                            </div>
                            <div class="tab-pane" id="Property">
                                <div class="short_sale_content">
                                    <uc1:ShortSalePropertyTab runat="server" ID="ShortSalePropertyTab" />
                                </div>
                            </div>
                            <div class="tab-pane" id="Mortgages">
                                <div class="short_sale_content">
                                    <uc1:ShortSaleMortgageTab runat="server" ID="ShortSaleMortgageTab" />
                                </div>
                            </div>
                            <div class="tab-pane" id="DealInfo">
                                <div class="short_sale_content">
                                    <div class="clearfix">
                                        <div style="float: right">
                                            <input type="button" class="rand-button short_sale_edit" value="Edit" onclick='switch_edit_model(this, short_sale_case_data)' />
                                        </div>
                                    </div>

                                    <div class="ss_form">
                                        <h4 class="ss_form_title">Listing Info</h4>
                                        <ul class="ss_form_box clearfix">

                                            <li class="ss_form_item">
                                                <label class="ss_form_input_title">MLS #</label>
                                                <input class="ss_form_input " data-field="ListMLS">
                                            </li>
                                            <li class="ss_form_item">
                                                <label class="ss_form_input_title">MLS Status</label>
                                                <select class="ss_form_input" data-field="MLSStatus">
                                                    <option>NYS MLS</option>
                                                    <option>MLS LI </option>
                                                    <option>Brooklyn MLS</option>
                                                </select>
                                            </li>
                                            <li class="ss_form_item">
                                                <label class="ss_form_input_title">Listing Date</label>
                                                <input class="ss_form_input ss_date" data-field="ListingDate">
                                            </li>
                                            <li class="ss_form_item">
                                                <label class="ss_form_input_title">Listing Expiry Date</label>
                                                <input class="ss_form_input ss_date" data-field="ListingExpireDate">
                                            </li>
                                            <li class="ss_form_item">
                                                <label class="ss_form_input_title">LockBox</label>
                                                <input class="ss_form_input" data-field="Lockbox">
                                            </li>
                                            <li class="ss_form_item">
                                                <label class="ss_form_input_title">List Price</label>
                                                <input class="ss_form_input currency_input" data-field="ListPrice">
                                            </li>

                                            <li class="ss_form_item">
                                                <label class="ss_form_input_title">Document Missing</label>
                                                <input type="checkbox" id="pdf_check120" name="DocumentMissing" value="YES" class="ss_form_input ss_visable" data-field="DocumentMissing">
                                                <label for="pdf_check120" class="input_with_check">
                                                    <span class="box_text">Yes </span>
                                                </label>
                                            </li>
                                            <li class="ss_form_item" data-visiable="DocumentMissing">
                                                <label class="ss_form_input_title">Missed Document</label>
                                                <input class="ss_form_input " data-field="MissingDocDescription">
                                            </li>
                                            <li class="ss_form_item">
                                                <label class="ss_form_input_title">Start Intake</label>
                                                <input type="radio" id="pdf_check119" name="1" value="YES" class="ss_form_input" data-field="StartIntake">
                                                <label for="pdf_check119" class="input_with_check">
                                                    <span class="box_text">Yes </span>
                                                </label>
                                                <input type="radio" id="pdf_check1191" name="1" value="NO" class="ss_form_input">
                                                <label for="pdf_check1191" class="input_with_check">
                                                    <span class="box_text">No </span>
                                                </label>
                                            </li>
                                        </ul>
                                    </div>
                                    <script type="text/javascript">

                                        function DocumentChanged(e) {
                                            var checked = e.checked // $(e).porp("checked")
                                            $("#missed_doc_li").css("display", checked ? '' : "none");
                                        }
                                    </script>
                                    <div class="ss_form">
                                        <h4 class="ss_form_title">Value Info </h4>
                                        <%-- <script>
                                            function dateValueChagne(e) {
                                                var vdate = $(e).val()
                                                var montadd =
                                                    {
                                                        Appraisal: 3,
                                                        BPO: 6
                                                    }
                                                var add = montadd[$('#ValueSelect').val()];
                                                if (add) {
                                                    var edate = new Date(vdate)
                                                    edate.setMonth(edate.getMonth() + add);
                                                    var ldate = edate.toLocaleDateString();
                                                    $('#expries_date').val(ldate);
                                                }

                                            }
                                        </script>
                                        <table class="table table-striped" style="margin-top: 20px">
                                            <thead>
                                                <tr>
                                                    <th>Method</th>
                                                    <th>Bank Value</th>
                                                    <th>Date Of Value</th>
                                                    <th>Expires on</th>

                                                </tr>
                                            </thead>
                                            <tr>
                                                <td>
                                                    <select class="form-control" id="ValueSelect">
                                                        <option>Appraisal </option>
                                                        <option>BPO </option>
                                                        <option>Desktop Review </option>
                                                    </select>
                                                </td>
                                                <td>
                                                    <input class="form-control currency_input" />
                                                </td>
                                                <td>
                                                    <input class="form-control ss_date" onchange="dateValueChagne(this)" />
                                                </td>
                                                <td>
                                                    <input class="form-control ss_date" id="expries_date" />
                                                </td>

                                            </tr>
                                        </table>--%>
                                        <asp:HiddenField ID="hfBBLE" runat="server" />
                                        <dx:ASPxGridView ID="gvPropertyValueInfo" runat="server" KeyFieldName="ValueId" Width="100%" Theme="Moderno" OnDataBinding="gvPropertyValueInfo_DataBinding"
                                            OnRowInserting="gvPropertyValueInfo_RowInserting" OnRowUpdating="gvPropertyValueInfo_RowUpdating" OnRowDeleting="gvPropertyValueInfo_RowDeleting">
                                            <Columns>
                                                <dx:GridViewDataComboBoxColumn FieldName="Method" Width="150px">
                                                    <PropertiesComboBox Native="true" Style-CssClass="form-control">
                                                        <Items>
                                                            <dx:ListEditItem Value="Exterior Appraisal" Text="Exterior Appraisal" />
                                                            <dx:ListEditItem Value="Interior Appraisal" Text="Interior Appraisal" />
                                                            <dx:ListEditItem Value="Exterior BPO" Text="Exterior BPO" />
                                                            <dx:ListEditItem Value="Interior BPO" Text="Interior BPO" />

                                                        </Items>
                                                    </PropertiesComboBox>
                                                </dx:GridViewDataComboBoxColumn>
                                                <dx:GridViewDataTextColumn FieldName="BankValue" PropertiesTextEdit-DisplayFormatString="C2">
                                                    <PropertiesTextEdit Native="true" Style-CssClass="form-control">
                                                    </PropertiesTextEdit>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="MNSP" PropertiesTextEdit-DisplayFormatString="C2">
                                                    <PropertiesTextEdit Native="true" Style-CssClass="form-control">
                                                    </PropertiesTextEdit>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataDateColumn FieldName="DateOfValue" Width="120px">
                                                </dx:GridViewDataDateColumn>
                                                <dx:GridViewDataDateColumn FieldName="ExpiredOn" Width="120px"></dx:GridViewDataDateColumn>
                                                <dx:GridViewCommandColumn ShowEditButton="true" ShowDeleteButton="true" ShowNewButtonInHeader="true"></dx:GridViewCommandColumn>
                                            </Columns>
                                            <SettingsEditing Mode="Inline"></SettingsEditing>
                                        </dx:ASPxGridView>
                                    </div>
                                    <div class="ss_form">
                                        <h4 class="ss_form_title">Offer Info</h4>
                                        <ul class="ss_form_box clearfix">
                                            <li class="ss_form_item">
                                                <label class="ss_form_input_title">Offer Submited </label>

                                                <input type="checkbox" id="pdf_check9995" name="HasOfferSubmit" value="true" class="ss_form_input ss_visable" data-field="HasOfferSubmit">
                                                <label for="pdf_check9995" class="input_with_check">
                                                    <span class="box_text">Yes </span>
                                                </label>
                                                <%-- <input type="radio" id="pdf_check9993" name="HasOfferSubmit" value="false" class="ss_form_input ss_visable" data-field="HasOfferSubmit">
                                                <label for="pdf_check9993" class="input_with_check">
                                                    <span class="box_text">Yes </span>
                                                </label>
                                                <input type="radio" id="pdf_check9994" name="HasOfferSubmit" value="true" class="ss_form_input">
                                                <label for="pdf_check9994" class="input_with_check">
                                                    <span class="box_text">No </span>
                                                </label>--%>
                                               
                                            </li>

                                            <li class="ss_form_item" data-visiable="HasOfferSubmit">
                                                <label class="ss_form_input_title">Offer Submitted Amount</label>
                                                <input class="ss_form_input currency_input" data-field="OfferSubmited">
                                            </li>
                                            <li class="ss_form_item" data-visiable="HasOfferSubmit">
                                                <label class="ss_form_input_title">Offer Submitted Date </label>
                                                <input class="ss_form_input ss_date" data-field="OfferDate">
                                            </li>

                                            <li class="ss_form_item">
                                                <label class="ss_form_input_title">Lender Counter </label>
                                                <input class="ss_form_input" data-field="LenderCounter">
                                            </li>
                                            <li class="ss_form_item">
                                                <label class="ss_form_input_title">Date Counter Submitted </label>
                                                <input class="ss_form_input ss_date" data-field="CounterSubmited">
                                            </li>
                                            <li class="ss_form_item">
                                                <label class="ss_form_input_title">Counter Offer</label>
                                                <input class="ss_form_input" data-field="CounterOffer">
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane" id="Homewoner">
                                <div class="short_sale_content">
                                    <uc1:ShortSaleHomewonerTab runat="server" ID="ShortSaleHomewonerTab" />
                                </div>
                            </div>
                            <div class="tab-pane  <%= If(isEviction,"active","") %>" id="Eviction">
                                <div class="short_sale_content" style='<%= If(isEviction,"margin-top: 0px;","") %>'>
                                    <uc1:ShortSaleEvictionTab runat="server" ID="ShortSaleEvictionTab" />
                                </div>
                            </div>
                            <div class="tab-pane" id="Parties">
                                <div class="short_sale_content">
                                    <uc1:ShortSalePartiesTab runat="server" ID="ShortSalePartiesTab" />
                                </div>

                            </div>
                        </div>
                        <%--</dx:ASPxCallbackPanel>--%>
                    </div>
                </div>
            </div>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

<dx:ASPxPopupControl ClientInstanceName="aspxAcrisControl" Width="1000px" Height="800px"
    ID="ASPxPopupControl1" HeaderText="Acris" Modal="true" CloseAction="CloseButton" ShowMaximizeButton="true"
    runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
    <HeaderTemplate>
        <div class="clearfix">
            <div class="pop_up_header_margin">
                <i class="fa fa-tasks with_circle pop_up_header_icon"></i>
                <span class="pop_up_header_text" id="pop_up_header_text">Acris</span> <span class="pop_up_header_text"><%= shortSaleCaseData.PropertyInfo.PropertyAddress%></span>
            </div>
            <div class="pop_up_buttons_div">
                <i class="fa fa-times icon_btn" onclick="aspxAcrisControl.Hide()"></i>
            </div>
        </div>
    </HeaderTemplate>
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
