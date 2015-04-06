﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LegalUI.aspx.vb" Inherits="IntranetPortal.LegalUI" MasterPageFile="~/Content.Master" %>


<asp:Content ContentPlaceHolderID="MainContentPH" runat="server">
    <div>
        <div style="width: 665px; align-content: center; height: 100%">

            <!-- Nav tabs -->

            <ul class="nav nav-tabs clearfix" role="tablist" style="height: 70px; background: #ff400d; font-size: 18px; color: white;">
                <li class="active short_sale_head_tab">
                    <a href="#property_info" role="tab" data-toggle="tab" class="tab_button_a">
                        <i class="fa fa-info-circle  head_tab_icon_padding"></i>
                        <div class="font_size_bold">Legal</div>
                    </a>
                </li>


                <li style="margin-right: 30px; color: #ffa484; float: right">

                    <i class="fa fa-mail-forward  sale_head_button sale_head_button_left tooltip-examples" title="" onclick="tmpBBLE=leadsInfoBBLE; popupCtrReassignEmployeeListCtr.PerformCallback();popupCtrReassignEmployeeListCtr.ShowAtElement(this);" data-original-title="Re-Assign"></i>
                    <i class="fa fa-envelope sale_head_button sale_head_button_left tooltip-examples" title="" onclick="ShowEmailPopup(leadsInfoBBLE)" data-original-title="Mail"></i>
                    <i class="fa fa-print sale_head_button sale_head_button_left tooltip-examples" title="" onclick="" data-original-title="Print"></i>
                </li>
            </ul>



            <style>
                .AttachmentSpan {
                    margin-left: 10px;
                    border: 1px solid #efefef;
                    padding: 3px 20px 3px 10px;
                    background-color: #ededed;
                }
            </style>

            <div id="ctl00_MainContentPH_ASPxSplitter1_ASPxCallbackPanel2_contentSplitter_SendMail_PopupSendMail_PW-1" class="dxpcLite_MetropolisBlue1 dxpclW" style="height: 700px; width: 630px; cursor: default; z-index: 10000; display: none;">
                <div class="dxpc-mainDiv dxpc-shadow">
                    <div class="dxpc-header dxpc-withBtn" id="ctl00_MainContentPH_ASPxSplitter1_ASPxCallbackPanel2_contentSplitter_SendMail_PopupSendMail_PWH-1">

                        <div class="clearfix">
                            <div class="pop_up_header_margin">
                                <i class="fa fa-envelope with_circle pop_up_header_icon"></i>
                                <span class="pop_up_header_text">Email</span>
                            </div>
                            <div class="pop_up_buttons_div">
                                <i class="fa fa-times icon_btn" onclick="popupSendEmailClient.Hide()"></i>
                            </div>
                        </div>

                    </div>
                    <div class="dxpc-contentWrapper">
                        <div class="dxpc-content" id="ctl00_MainContentPH_ASPxSplitter1_ASPxCallbackPanel2_contentSplitter_SendMail_PopupSendMail_PWC-1">
                        </div>
                    </div>
                </div>
            </div>


            <div class="tab-content">

                <div class="tab-pane active" id="property_info">



                    <script src="/scripts/jquery.formatCurrency-1.1.0.js"></script>
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
                            short_sale_case_data.PropertyInfo.UpdateBy = "Chris Yan";


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



                    <div id="" style="width: 100%;">

                        <input hidden="" id="short_sale_case_id" value="23">
                        <div style="padding-top: 5px">
                            <div style="height: 850px; overflow: auto;" id="prioity_content">


                                <div style="height: 80px; font-size: 30px; margin-left: 30px; margin-top: 20px;" class="font_gray">
                                    <div style="font-size: 30px">
                                        <span>
                                            <i class="fa fa-refresh"></i>
                                            <span style="margin-left: 19px;">10/16/2014 9:30:16 PM</span>
                                        </span>

                                        <span class="time_buttons" style="margin-right: 30px" onclick="ShowPopupMap('https://iapps.courts.state.ny.us/webcivil/ecourtsMain', 'eCourts')">eCourts</span>
                                        <span class="time_buttons" onclick="ShowDOBWindow(&quot;&quot;,&quot;1593 &quot;, &quot;48  &quot;)">DOB</span>
                                        <span class="time_buttons" onclick="ShowAcrisMap(&quot;3015930048 &quot;)">Acris</span>
                                        <span class="time_buttons" onclick="ShowPropertyMap(&quot;3015930048 &quot;)">Maps</span>

                                    </div>

                                    <span style="font-size: 14px; margin-top: -5px; float: left; margin-left: 53px; visibility: visible">Started on 10/16/2014 9:30:16 PM</span>
                                </div>


                                <div class="font_deep_gray" style="border-top: 1px solid #dde0e7; font-size: 20px">
                                    <div id="ctl00_MainContentPH_ASPxSplitter1_ASPxCallbackPanel2_contentSplitter_ShortSaleOverVew_ShortSaleCaseSavePanel_leadsCommentsCallbackPanel">

                                        <input type="hidden" name="ctl00$MainContentPH$ASPxSplitter1$ASPxCallbackPanel2$contentSplitter$ShortSaleOverVew$ShortSaleCaseSavePanel$leadsCommentsCallbackPanel$hfCaseId" id="ctl00_MainContentPH_ASPxSplitter1_ASPxCallbackPanel2_contentSplitter_ShortSaleOverVew_ShortSaleCaseSavePanel_leadsCommentsCallbackPanel_hfCaseId" value="23">
                                    </div>
                                    <table id="ctl00_MainContentPH_ASPxSplitter1_ASPxCallbackPanel2_contentSplitter_ShortSaleOverVew_ShortSaleCaseSavePanel_leadsCommentsCallbackPanel_LP" class="dxcpLoadingPanelWithContent_MetropolisBlue1 dxlpLoadingPanelWithContent_MetropolisBlue1" style="left: 0px; top: 0px; z-index: 30000; display: none;">
                                        <tbody>
                                            <tr>
                                                <td class="dx" style="padding-right: 0px;">
                                                    <img class="dxlp-loadingImage dxlp-imgPosLeft" src="/DXR.axd?r=1_15-8xia9" alt="" style="vertical-align: middle;"></td>
                                                <td class="dx" style="padding-left: 0px;"><span id="ctl00_MainContentPH_ASPxSplitter1_ASPxCallbackPanel2_contentSplitter_ShortSaleOverVew_ShortSaleCaseSavePanel_leadsCommentsCallbackPanel_TL">Loading…</span></td>
                                            </tr>
                                        </tbody>
                                    </table>


                                    <div class="note_item" style="background: white">

                                        <i class="fa fa-plus-circle note_img tooltip-examples" title="" style="color: #3993c1; cursor: pointer" onclick="aspxAddLeadsComments.ShowAtElement(this)" data-original-title="Add Notes"></i>


                                    </div>
                                </div>



                                <div>
                                    <!--detial Nav tabs -->

                                    <ul class="nav nav-tabs overview_tabs" role="tablist" style="">
                                        <li class="short_sale_tab active">
                                            <a class="shot_sale_tab_a " href="#home" role="tab" data-toggle="tab" onclick="refreshSummary()">Summary</a></li>
                                        <li class="short_sale_tab">
                                            <a class="shot_sale_tab_a " href="#Property" role="tab" data-toggle="tab" onclick="refreshSummary()">Foreclosure Review</a></li>
                                        <li class="short_sale_tab "><a class="shot_sale_tab_a " href="#Mortgages" role="tab" data-toggle="tab">Secondary Actions</a></li>

                                    </ul>

                                    <!-- Tab panes -->
                                    <div class="tab-content">
                                        <div class="tab-pane active" id="home">
                                            <div class="short_sale_content">


                                                <div>

                                                    <div>
                                                        <h4 class="ss_form_title">Property</h4>
                                                        <ul class="ss_form_box clearfix">
                                                            <li class="ss_form_item" style="width: 100%">
                                                                <label class="ss_form_input_title">address</label>

                                                                <input class="ss_form_input" style="width: 93.5%;" name="lender" value="421 HART ST, BEDFORD STUYVESANT,NY 11221">
                                                            </li>
                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">Block</label>
                                                                <input class="ss_form_input" data-field="PropertyInfo.Block">
                                                            </li>
                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">lot</label>
                                                                <input class="ss_form_input" data-field="PropertyInfo.Lot">
                                                            </li>
                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">Class</label>
                                                                <input class="ss_form_input" value="A0">
                                                            </li>
                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">Condition</label>
                                                                <input class="ss_form_input" value="Good">
                                                            </li>

                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">Vacant/Occupied </label>
                                                                <input class="ss_form_input">
                                                            </li>
                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">Status</label>
                                                                <select class="ss_form_input">
                                                                    <option>Eviction</option>
                                                                    <option>Short Sale</option>
                                                                </select>
                                                            </li>
                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title ss_ssn">Agent </label>
                                                                <input class="ss_form_input">
                                                            </li>

                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">Use</label>
                                                                <input class="ss_form_input" value="">
                                                            </li>

                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">Owner of Record </label>
                                                                <input class="ss_form_input">
                                                            </li>

                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">Case Contact person/owner </label>
                                                                <input class="ss_form_input">
                                                            </li>
                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">Phone number </label>
                                                                <input class="ss_form_input">
                                                            </li>

                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">Case Contact person/owner </label>
                                                                <input class="ss_form_input">
                                                            </li>
                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">email  </label>
                                                                <input class="ss_form_input">
                                                            </li>
                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">Address</label>
                                                                <input class="ss_form_input">
                                                            </li>



                                                        </ul>
                                                    </div>






                                                    <div data-array-index="0" data-field="Mortgages" class="ss_array" style="display: inline;">

                                                        <div class="ss_form">
                                                            <h4 class="ss_form_title"><span class="title_index ">Foreclosure </span></h4>
                                                            <ul class="ss_form_box clearfix">

                                                                <li class="ss_form_item ss_mortages_stauts">
                                                                    <label class="ss_form_input_title">Plaintiff</label>

                                                                    <input class="ss_form_input" data-item="Lender" data-item-type="1">
                                                                </li>
                                                                <li class="ss_form_item">
                                                                    <label class="ss_form_input_title">Servicer</label>
                                                                    <input class="ss_form_input" data-item="Lender" data-item-type="1">
                                                                </li>
                                                                <li class="ss_form_item">
                                                                    <label class="ss_form_input_title">Defendant</label>
                                                                    <input class="ss_form_input" data-item="Loan" data-item-type="1">
                                                                </li>
                                                                <li class="ss_form_item">
                                                                    <label class="ss_form_input_title">Attorney of record </label>
                                                                    <input class="ss_form_input input_currency" onblur="$(this).formatCurrency();" data-item="LoanAmount" data-item-type="1">
                                                                </li>
                                                                <li class="ss_form_item">
                                                                    <label class="ss_form_input_title">last court date</label>
                                                                    <input class="ss_form_input ss_date" >
                                                                </li>
                                                                <li class="ss_form_item">
                                                                    <label class="ss_form_input_title">next court date </label>
                                                                    <input class="ss_form_input ss_date">
                                                                </li>
                                                                <li class="ss_form_item">
                                                                    <label class="ss_form_input_title">Sale date  </label>
                                                                    <input class="ss_form_input ss_date">
                                                                </li>

                                                                <li class="ss_form_item">
                                                                    <span class="ss_form_input_title">HAMP filed</span>

                                                                    <input type="checkbox" id="pdf_check_yes30" name="1" class="ss_form_input" value="YES">
                                                                    <label for="pdf_check_yes30" class="input_with_check">
                                                                        <span class="box_text">Yes </span>
                                                                    </label>

                                                                </li>
                                                                <li class="ss_form_item">
                                                                    <label class="ss_form_input_title">Last Update </label>
                                                                    <input class="ss_form_input ss_date">
                                                                </li>
                                                            </ul>
                                                        </div>

                                                    </div>

                                                    <div class="ss_form">
                                                        <h4 class="ss_form_title">Secondary</h4>
                                                        <ul class="ss_form_box clearfix">

                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">Client </label>
                                                                <input class="ss_form_input" value="">
                                                            </li>
                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">Attorney of record </label>
                                                                <input class="ss_form_input" value="">
                                                            </li>
                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">Opposing party  </label>
                                                                <input class="ss_form_input" value="">
                                                            </li>

                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">Status </label>
                                                                <input class="ss_form_input" value="">
                                                            </li>

                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">Tasks </label>
                                                                <input class="ss_form_input" value="">
                                                            </li>

                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">Attorney working file  </label>
                                                                <input class="ss_form_input" value="">
                                                            </li>

                                                            <li class="ss_form_item">
                                                                <label class="ss_form_input_title">Last Update </label>
                                                                <input class="ss_form_input" value="">
                                                            </li>


                                                        </ul>
                                                    </div>



                                                </div>

                                            </div>
                                        </div>
                                        <div class="tab-pane " id="Property">
                                            <div class="short_sale_content">


                                                <div class="clearfix">
                                                    <div style="float: right">
                                                        <input type="button" class="rand-button short_sale_edit" value="Edit" onclick="switch_edit_model(this, short_sale_case_data)">
                                                    </div>
                                                </div>

                                                <div>
                                                    <h4 class="ss_form_title">Property</h4>
                                                    <ul class="ss_form_box clearfix">

                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Street Number</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Street Name</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.StreetName">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">City</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.City">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">State</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.State">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Zip</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Zipcode">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">&nbsp;</label>
                                                            <input class="ss_form_input ss_form_hidden" value=" ">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">BLOCK</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Block">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Lot</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Lot">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Building type</label>
                                                            <select class="ss_form_input" data-field="PropertyInfo.BuildingType">
                                                                <option value="House">House</option>
                                                                <option value="Apartment">Apartment</option>
                                                                <option value="Condo">Condo</option>
                                                                <option value="Cottage/cabin">Cottage/cabin</option>
                                                                <option value="Duplex">Duplex</option>
                                                                <option value="Flat">Flat</option>
                                                                <option value="In-Law">In-Law</option>
                                                                <option value="Loft">Loft</option>
                                                                <option value="Townhouse">Townhouse</option>
                                                                <option value="Manufactured">Manufactured</option>
                                                                <option value="Assisted living">Assisted living</option>
                                                                <option value="Land">Land</option>
                                                            </select>

                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Borrower</label>
                                                            <input class="ss_form_input">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Co-Borrower</label>
                                                            <input class="ss_form_input">
                                                        </li>

                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">&nbsp;</label>
                                                            <input class="ss_form_input ss_form_hidden" value=" ">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Language</label>
                                                            <input class="ss_form_input">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Mental Capacity </label>
                                                            <input class="ss_form_input">
                                                        </li>

                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">&nbsp;</label>
                                                            <input class="ss_form_input ss_form_hidden" value=" ">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Divorce</label>
                                                            <input class="ss_form_input">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Borrowers Relationship </label>
                                                            <input class="ss_form_input">
                                                        </li>

                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">&nbsp;</label>
                                                            <input class="ss_form_input ss_form_hidden" value=" ">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Plaintiff </label>
                                                            <input class="ss_form_input">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Plaintiff Attorney</label>
                                                            <input class="ss_form_input">
                                                        </li>

                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Servicer</label>
                                                            <input class="ss_form_input">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Defendant(s) </label>
                                                            <input class="ss_form_input">
                                                        </li>

                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Attorney of record  </label>
                                                            <input class="ss_form_input">
                                                        </li>

                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Estate</label>
                                                            <input class="ss_form_input" type="number" data-field="PropertyInfo.NumOfStories">
                                                        </li>

                                                        <li class="ss_form_item">
                                                            <span class="ss_form_input_title">c/o(<span class="linkey_pdf">pdf</span>)</span>

                                                            <input type="radio" class="ss_form_input" data-field="PropertyInfo.CO" data-radio="Y" id="key_PropertyInfo_checkYes_CO" name="pdf" value="YES">
                                                            <label for="key_PropertyInfo_checkYes_CO" class="input_with_check">
                                                                <span class="box_text">Yes</span>
                                                            </label>

                                                            <input type="radio" class="ss_form_input" data-field="PropertyInfo.CO" id="none_pdf_checkey_no21" name="pdf" value="NO">
                                                            <label for="none_pdf_checkey_no21" class="input_with_check">
                                                                <span class="box_text"><span class="box_text">No</span></span>
                                                            </label>

                                                        </li>


                                                    </ul>
                                                </div>


                                                <div class="ss_form">
                                                    <h4 class="ss_form_title">Case Background</h4>
                                                    <ul class="ss_form_box clearfix">

                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Originator</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">2nd Mortgage </label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Deed Xfer </label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Tax Lien </label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">UCC </label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">HPD</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Questionable Satisfactions</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>

                                                    </ul>
                                                </div>

                                                <div class="ss_form">
                                                    <h4 class="ss_form_title">Mortgage</h4>
                                                    <ul class="ss_form_box clearfix">

                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Original Lender </label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Active/Dissolved Date </label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Loan Amount 1st/2nd Mtg </label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Type of Loan </label>
                                                            <select class="ss_form_input" data-field="PropertyInfo.Number">
                                                                <option>Type 1</option>
                                                            </select>
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">signed user </label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Owner occupied</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <span class="ss_form_input_title">FREDDIE MAC</span>

                                                            <input type="radio" id="check1" name="FREDDIE_MAC__index__" data-item="Freddie" data-item-type="1" data-radio="Y" value="YES" class="ss_form_input">
                                                            <label for="check1" class="input_with_check"><span class="box_text">Yes</span></label>

                                                            <input type="radio" id="check2" name="FREDDIE_MAC__index__" data-item="Freddie" data-item-type="1" value="NO" class="ss_form_input">
                                                            <label for="check2" class="input_with_check"><span class="box_text">No</span></label>
                                                        </li>

                                                    </ul>
                                                </div>
                                                <div class="ss_form">
                                                    <h4 class="ss_form_title">Note</h4>
                                                    <ul class="ss_form_box clearfix">
                                                        <li class="ss_form_item" style="width: 100%">
                                                            <label class="ss_form_input_title">Note</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                    </ul>
                                                </div>

                                                <div class="ss_form">
                                                    <h4 class="ss_form_title">Court Activity</h4>
                                                    <ul class="ss_form_box clearfix">

                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Date</label>
                                                            <input class="ss_form_input ss_date" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Affidavit Date</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Affidavit Company</label>
                                                            <input class="ss_form_input">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Prior Index Date</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Prior Index Opposing </label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Conferences Date</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Conferences Attended</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Conferences Referee</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Status</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Sale Date</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">HAMP  submitted Date</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">HAMP  submitted TYPE</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">HAMP  submitted Resubmission?</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>

                                                    </ul>
                                                </div>
                                                <div class="ss_form">
                                                    <h4 class="ss_form_title">Assignments</h4>
                                                    <ul class="ss_form_box clearfix">

                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Assignor</label>
                                                            <input class="ss_form_input ss_date" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Assignee</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Assignment Date</label>
                                                            <input class="ss_form_input">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Signed</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Signed Address </label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>


                                                    </ul>
                                                </div>
                                                <div class="ss_form">
                                                    <h4 class="ss_form_title">Loan Pool Trust</h4>
                                                    <ul class="ss_form_box clearfix">

                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Name</label>
                                                            <input class="ss_form_input ss_date" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Documentation ID</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">documents Notes</label>
                                                            <input class="ss_form_input">
                                                        </li>



                                                    </ul>
                                                </div>
                                                <div class="ss_form">
                                                    <h4 class="ss_form_title">Bankruptcy</h4>
                                                    <ul class="ss_form_box clearfix">

                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Prior</label>
                                                            <input class="ss_form_input ss_date" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">chapter</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Disposition</label>
                                                            <input class="ss_form_input">
                                                        </li>

                                                    </ul>
                                                </div>

                                                <div class="ss_form">
                                                    <h4 class="ss_form_title">Bankruptcy</h4>
                                                    <ul class="ss_form_box clearfix">

                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Prior</label>
                                                            <input class="ss_form_input ss_date" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">chapter</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Disposition</label>
                                                            <input class="ss_form_input">
                                                        </li>

                                                    </ul>
                                                </div>
                                                <div class="ss_form">
                                                    <h4 class="ss_form_title">Statute of Limitations</h4>
                                                    <ul class="ss_form_box clearfix">

                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">LP Date</label>
                                                            <input class="ss_form_input ss_date" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Default Date</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <span class="ss_form_input_title">foreclosure</span>

                                                            <input type="checkbox" id="pdf_check_yes39" name="1" class="ss_form_input" value="YES">
                                                            <label for="pdf_check_yes39" class="input_with_check">
                                                                <span class="box_text">Yes </span>
                                                            </label>

                                                        </li>
                                                        
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Prior Plaintiff</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Prior attorney</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                    </ul>
                                                </div>
                                                <div class="ss_form">
                                                    <h4 class="ss_form_title">Estate</h4>
                                                    <ul class="ss_form_box clearfix">

                                                        <li class="ss_form_item">

                                                            <span class="ss_form_input_title">borrower Died</span>

                                                            <input type="checkbox" id="pdf_check_yes40" name="1" class="ss_form_input" value="YES">
                                                            <label for="pdf_check_yes40" class="input_with_check">
                                                                <span class="box_text">Yes </span>
                                                            </label>


                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">held Reason</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>

                                                        <li class="ss_form_item">
                                                            <span class="ss_form_input_title">Estate set up</span>

                                                          
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                         <li class="ss_form_item">
                                                          <label class="ss_form_input_title">prior action</label>

                                                          
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number">
                                                        </li>
                                                          
                                                    </ul>
                                                </div>
                                                <div class="ss_form">
                                                    <h4 class="ss_form_title">Defenses/Conclusion</h4>
                                                    <ul class="ss_form_box clearfix">


                                                        <li class="ss_form_item" style="width: 100%">
                                                            <label class="ss_form_input_title">Defenses/Conclusion</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number" style="width: 93%">
                                                        </li>

                                                      

                                                    </ul>
                                                </div>
                                               
                                                <div class="ss_form">
                                                    <h4 class="ss_form_title">Action Plan</h4>
                                                    <ul class="ss_form_box clearfix">


                                                        <li class="ss_form_item" style="width: 100%">
                                                            <label class="ss_form_input_title">Action Plan</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number" style="width: 93%">
                                                        </li>

                                                      

                                                    </ul>
                                                </div>
                                                 <div class="ss_form">
                                                    <h4 class="ss_form_title">Etrack</h4>
                                                    <ul class="ss_form_box clearfix">


                                                        <li class="ss_form_item" style="width: 100%">
                                                            <label class="ss_form_input_title">Etrack</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Number" style="width: 93%">
                                                        </li>

                                                        

                                                    </ul>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="tab-pane" id="Mortgages">
                                            <div class="short_sale_content">


                                                <div class="clearfix">
                                                    <div style="float: right">
                                                        <input type="button" class="rand-button short_sale_edit" value="Edit" onclick="switch_edit_model(this, short_sale_case_data)">
                                                    </div>
                                                </div>

                                                <div>
                                                    <h4 class="ss_form_title">Action</h4>
                                                    <ul class="ss_form_box clearfix">

                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Estate</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Block">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Partition</label>
                                                            <input class="ss_form_input" data-field="PropertyInfo.Lot">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Breach of Contract</label>
                                                            <input class="ss_form_input">
                                                        </li>
                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Quiet Title</label>
                                                            <input class="ss_form_input">
                                                        </li>

                                                        <li class="ss_form_item">
                                                            <label class="ss_form_input_title">Other </label>
                                                            <input class="ss_form_input">
                                                        </li>
                                                    </ul>
                                                </div>

                                                <div class="ss_form" style="padding-bottom: 20px;">
                                                    <h4 class="ss_form_title">Legal  Notes <i class="fa fa-plus-circle  color_blue_edit collapse_btn tooltip-examples" title="" onclick="addOccupantNoteClick( 0  ,this);" data-original-title="Add"></i></h4>


                                                    <div class="note_input" style="display: none" data-index="0">
                                                        {{#Notes}}
                                                    <div class="clearence_list_item">
                                                        <div class="clearence_list_content clearfix" style="margin-bottom: 10px">
                                                            <div class="clearence_list_text" style="margin-top: 0px;">
                                                                <div class="clearence_list_text14">
                                                                    <i class="fa fa-caret-right clearence_caret_right_icon"></i>
                                                                    <i class="fa fa-times color_blue_edit icon_btn tooltip-examples" title="" style="float: right" onclick="deleteAccoupantNote(0,{{id}})" data-original-title="Delete"></i>
                                                                    <span class="clearence_list_text14">{{Notes}}
                                                                            <br>

                                                                        <span class="clearence_list_text12">{{CreateDate}} by {{CreateBy}}
                                                                        </span>
                                                                    </span>

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                        {{/Notes}}
                                                    </div>
                                                    <div class="note_output">


                                                        <div class="clearence_list_item">
                                                            <div class="clearence_list_content clearfix" style="margin-bottom: 10px">
                                                                <div class="clearence_list_text" style="margin-top: 0px;">
                                                                    <div class="clearence_list_text14">
                                                                        <i class="fa fa-caret-right clearence_caret_right_icon"></i>
                                                                        <i class="fa fa-times color_blue_edit icon_btn tooltip-examples" title="" style="float: right" onclick="deleteAccoupantNote(0,0)" data-original-title="Delete"></i>
                                                                        <span class="clearence_list_text14">Note for test
                                            <br>

                                                                            <span class="clearence_list_text12">4/6/2015 9:57:43 AM by 123
                                                                            </span>
                                                                        </span>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>


                                            </div>
                                        </div>

                                    </div>

                                </div>
                            </div>
                        </div>

                    </div>



                </div>


            </div>
        </div>
    </div>
</asp:Content>
