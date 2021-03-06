﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="LegalSecondaryActions.ascx.vb" Inherits="IntranetPortal.LegalSecondaryActions" %>
<%-- Legal Secondary actions in Legal and need check in Leads UI--%>

<style>
    .legal_action_div {
        /*display:none;*/
    }
</style>
<div id="Estate" class="legal_action_div  animate-show" ng-show="CheckShow('Estate')">
    <div class="ss_form">
        <h4 class="ss_form_title">Estate</h4>
        <ul class="ss_form_box clearfix">


            <li class="ss_form_item">
                <label class="ss_form_input_title">hold Reason</label>
                <select class="ss_form_input" ng-model="LegalCase.SecondaryInfo.EstateHoldReason">
                    <option>Tenants in common</option>
                    <option>Joint Tenants w/right of survivorship</option>
                    <option>Tenancy by the entirety</option>

                </select>
            </li>

            <li class="ss_form_item">
                <span class="ss_form_input_title">Estate set up</span>

                <select class="ss_form_input" ng-model="LegalCase.SecondaryInfo.EstateSetUp">
                    <option value="Unknown">Unknown</option>
                    <option value="Yes">Yes</option>
                    <option value="No">No</option>
                </select>

            </li>
            <li class="ss_form_item">

                <span class="ss_form_input_title">borrower Died</span>

                <select class="ss_form_input" ng-model="LegalCase.SecondaryInfo.EstateBorrowerDied">
                    <option value="">Unknown</option>
                    <option value="Yes">Yes</option>
                    <option value="No">No</option>
                </select>
                <%-- <input type="radio" id="pdf_check100" name="1" class="ss_form_input" value="true">
                                                                                    <label for="pdf_check50" class="input_with_check">
                                                                                        <span class="box_text">Yes </span>
                                                                                    </label>
                                                                                    <input type="radio" id="pdf_check101" name="1" class="ss_form_input" value="true">
                                                                                    <label for="pdf_check50" class="input_with_check">
                                                                                        <span class="box_text">Tenancy by the entirety </span>
                                                                                    </label>--%>

            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">prior action</label>

                <input class="ss_form_input" ng-model="LegalCase.SecondaryInfo.EstatePriorAction">
            </li>
            <li class="ss_form_item ss_form_item_line">
                <label class="ss_form_input_title">note</label>
                <textarea class="edit_text_area text_area_ss_form" ng-model="LegalCase.SecondaryInfo.EstateNotes"></textarea>
            </li>
        </ul>
    </div>
</div>

<div class="legal_action_div  animate-show" ng-show="CheckShow('Miscellaneous')">
    <div class="ss_form">
        <h4 class="ss_form_title">Miscellaneous</h4>
        <ul class="ss_form_box clearfix">

            <li class="ss_form_item ss_form_item_line">
                <label class="ss_form_input_title">Miscellaneous</label>
                <textarea class="edit_text_area text_area_ss_form" ng-model="LegalCase.SecondaryInfo.Miscellaneous"></textarea>
            </li>
        </ul>
    </div>
</div>
<div id="Partition" class="legal_action_div  animate-show" ng-show="CheckShow('Partition')">
    <div class="ss_form">
        <h4 class="ss_form_title">Partition</h4>
        <ul class="ss_form_box clearfix">

            <li class="ss_form_item">
                <label class="ss_form_input_title">Owner</label>
                <div class="contact_box" dx-select-box="InitContact('LegalCase.SecondaryInfo.OwnerId')"></div>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Parties 1</label>
                <div class="contact_box" dx-select-box="InitContact('LegalCase.SecondaryInfo.PartitionPartiesId')">
                </div>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Parties 2</label>
                <div class="contact_box" dx-select-box="InitContact('LegalCase.SecondaryInfo.PartitionPartie2Id')">
                </div>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Parties 3</label>
                <div class="contact_box" dx-select-box="InitContact('LegalCase.SecondaryInfo.PartitionPartie3Id')">
                </div>
            </li>

            <li class="ss_form_item">
                <label class="ss_form_input_title">Relationship Of Parties</label>
                <select class="ss_form_input" ng-model="LegalCase.SecondaryInfo.RelationshipOfParties" ng-options='o as o for o  in [ "Married","Divorced","Seperated","Family members","Other"]'>
                </select>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Action</label>
                <select class="ss_form_input" ng-model="LegalCase.SecondaryInfo.Relationshi">
                    <option value="Action1">Action1 </option>
                    <option value="Action1">Action2 </option>
                </select>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">held reason</label>
                <select class="ss_form_input" ng-model="LegalCase.SecondaryInfo.PartitionHeldReason">
                    <option value="Tenants in common ">Tenants in common                     </option>
                    <option value="Joint Tenants w/right of survivorship">Joint Tenants w/right of survivorship </option>
                    <option value="Tenancy by the entirety">Tenancy by the entirety               </option>
                </select>
            </li>
            <li class="ss_form_item ss_form_item_line">
                <label class="ss_form_input_title">note</label>
                <textarea class="edit_text_area text_area_ss_form" ng-model="LegalCase.SecondaryInfo.PartitionNotes"></textarea>
            </li>
        </ul>
    </div>
</div>


<div id="Breach_of_Contract" class="legal_action_div animate-show" ng-show="CheckShow('Breach of Contract')">
    <div class="ss_form">
        <h4 class="ss_form_title">Breach of Contract</h4>
        <ul class="ss_form_box clearfix">


            <li class="ss_form_item">
                <label class="ss_form_input_title">parties 1</label>
                <div class="contact_box" dx-select-box="InitContact('LegalCase.SecondaryInfo.BreachOfContractParties1Id')">
                </div>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">parties 2</label>
                <div class="contact_box" dx-select-box="InitContact('LegalCase.SecondaryInfo.BreachOfContractParties2Id')">
                </div>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Breach type</label>
                <select class="ss_form_input" ng-model="LegalCase.SecondaryInfo.BreachOfContractBreachType">
                    <option>Breach  type 1 </option>
                    <option>Breach  type 2 </option>
                </select>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Breach date </label>
                <input class="ss_form_input " pt-date="" ng-model="LegalCase.SecondaryInfo.BreachOfContractDate" />
            </li>

            <li class="ss_form_item">
                <label class="ss_form_input_title">breach learned </label>
                <input class="ss_form_input" ng-model="LegalCase.SecondaryInfo.BreachOfContractBreachLearned" />
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Money damages amount</label>
                <input class="ss_form_input" mask-money="" ng-model="LegalCase.SecondaryInfo.BreachOfContractBreachMoneyDamagesAmount" />
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">money damages for</label>
                <input class="ss_form_input" ng-model="LegalCase.SecondaryInfo.BreachOfContractBreachMoneyDamagesAmountFor" />
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">money damages check Id</label>
                <input class="ss_form_input" ng-model="LegalCase.SecondaryInfo.MoneyDamagesCheckId" />
            </li>

            <li class="ss_form_item ss_form_item_line">
                <label class="ss_form_input_title">note</label>
                <textarea class="edit_text_area text_area_ss_form" ng-model="LegalCase.SecondaryInfo.BreachOfContractNotes"></textarea>
            </li>
        </ul>
    </div>
    <style>
        .checktoggle {
        }
    </style>

    <%--    <div class="ss_form">
        <h4 class="ss_form_title" style="width: 59%; display: inline-block">Breach of Contract money damages </h4>
        <div style="display: inline-block">
            <input type="checkbox" id="checkshow" name="1" class="ss_form_input checktoggle" value="YES">
            <label for="checkshow" class="input_with_check">
                <span class="box_text">Yes </span>
            </label>
        </div>
        <ul class="ss_form_box clearfix" style="display: none">
            <li class="ss_form_item">
                <label class="ss_form_input_title">amount</label>
                <input class="ss_form_input currency_input" ng-model="LegalCase.SecondaryInfo.Against" />
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Reason</label>
                <input class="ss_form_input " ng-model="LegalCase.SecondaryInfo.Against" />
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">check Id</label>
                <input class="ss_form_input " ng-model="LegalCase.SecondaryInfo.Against" />
            </li>
        </ul>
    </div>--%>
</div>

<script>

    $(".checktoggle").click(function () {
        var box = $(this).parents(".ss_form").children(".ss_form_box");
        if (this.checked) {
            box.slideDown();
        } else {
            box.slideUp();
        }
    })
</script>
<div></div>
<%-- quiet title --%>
<div id="Quiet_Title" class="legal_action_div animate-show" ng-show="CheckShow('Quiet Title')">
    <div class="ss_form">
        <h4 class="ss_form_title">Quiet Title</h4>
        <ul class="ss_form_box clearfix">
            <li class="ss_form_item">
                <label class="ss_form_input_title">LP date</label>
                <input class="ss_form_input ss_date" ng-model="LegalCase.SecondaryInfo.LPDate" />
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Default date</label>
                <input class="ss_form_input ss_date" ng-model="LegalCase.SecondaryInfo.DefaultDate" />

            </li>
            <li class="ss_form_item">
                <span class="ss_form_input_title">foreclosure active</span>
                <input type="checkbox" id="pdf_check_yes121" name="121" class="ss_form_input" value="YES" ng-model="LegalCase.SecondaryInfo.InForeclosure" />
                <label for="pdf_check_yes121" class="input_with_check">
                    <span class="box_text">Yes </span>
                </label>

            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">foreclosure Action</label>
                <input class="ss_form_input ss_date" ng-model="LegalCase.SecondaryInfo.StatuteDisposition" />

            </li>

            <li class="ss_form_item">
                <label class="ss_form_input_title">Prior Plaintiff</label>
                <input type="text" class="ss_form_input" ng-model="LegalCase.SecondaryInfo.PriorPlaintiffName" ng-change="LegalCase.SecondaryInfo.PriorPlaintiffId=null" uib-typeahead="contact.Name for contact in ptContactServices.getContacts($viewValue)" typeahead-on-select="LegalCase.SecondaryInfo.PriorPlaintiffId=$item.ContactId" bind-id="LegalCase.SecondaryInfo.PriorPlaintiffId">
                <%--<div class="contact_box" dx-select-box="InitContact('LegalCase.SecondaryInfo.PriorPlaintiffId')">
                </div>--%>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Prior Plaintiff(Bank) gone out of business</label>
                <input type="checkbox" id="pdf_check_yes399" name="122" class="ss_form_input" value="true" ng-model="LegalCase.SecondaryInfo.PriorPlaintiffOutOfBusiness">
                <label for="pdf_check_yes399" class="input_with_check">
                    <span class="box_text">Yes </span>
                </label>
            </li>

            <li class="ss_form_item">
                <label class="ss_form_input_title">Prior Attorney</label>
                <input type="text" class="ss_form_input" ng-model="LegalCase.SecondaryInfo.PriorAttorneyName" ng-change="LegalCase.SecondaryInfo.PriorAttorneyId=null" uib-typeahead="contact.Name for contact in ptContactServices.getContacts($viewValue,3)" typeahead-on-select="LegalCase.SecondaryInfo.PriorAttorneyId=$item.ContactId" bind-id="LegalCase.SecondaryInfo.PriorAttorneyId">
                <%-- <div class="contact_box" dx-select-box="InitContact('LegalCase.SecondaryInfo.PriorPlaintiffId')">
                </div>--%>
            </li>

            <li class="ss_form_item">
                <label class="ss_form_input_title">last payment date</label>
                <input class="ss_form_input" pt-date="" ng-model="LegalCase.SecondaryInfo.LastPaymentDate" />
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Credit Report</label>
                <input class="ss_form_input " ng-model="LegalCase.SecondaryInfo.QuietTitleCreditReport" />
            </li>
            <li class="ss_form_item">
                <%--Who owns mortgage?--%>

                <label class="ss_form_input_title">Lender </label>
                <input type="text" class="ss_form_input" ng-model="LegalCase.SecondaryInfo.OriginalLenderName" ng-change="LegalCase.SecondaryInfo.OriginalLenderId=null" uib-typeahead="contact.Name for contact in ptContactServices.getContacts($viewValue,5)" typeahead-on-select="LegalCase.SecondaryInfo.OriginalLenderId=$item.ContactId" bind-id="LegalCase.SecondaryInfo.OriginalLenderId">
                <%-- <div class="contact_box" dx-select-box="InitContact('LegalCase.SecondaryInfo.OriginalLenderId')">
                    </div--%>
            </li>
            <li class="ss_form_item">
                <%--Do we know who owns the Note--%>
                <label class="ss_form_input_title">Mortage Owner</label>
                <input type="text" class="ss_form_input" ng-model="LegalCase.SecondaryInfo.QuietTitleMortageOwnerName" ng-change="LegalCase.SecondaryInfo.QuietTitleMortageOwnerId=null" uib-typeahead="contact.Name for contact in ptContactServices.getContacts($viewValue)" typeahead-on-select="LegalCase.SecondaryInfo.QuietTitleMortageOwnerId=$item.ContactId" bind-id="LegalCase.SecondaryInfo.QuietTitleMortageOwnerId">
                <%-- <div class="ss_form_input " dx-select-box="InitContact('LegalCase.SecondaryInfo.QuietTitleMortageOwnerId')"></div>--%>
            </li>
            <li class="ss_form_item">
                <%--Do we have the Deed--%>
                <span class="ss_form_input_title">Deed</span>
                <select class="ss_form_input" ng-model="LegalCase.SecondaryInfo.HasDeed">
                    <option value="Unknown">Unknown</option>
                    <option value="Yes">Yes</option>
                    <option value="No">No</option>
                </select>
                <%-- <input type="checkbox" id="pdf_check_yes122" name="1" class="ss_form_input" value="YES">
                                                                                        <label for="pdf_check_yes122" class="input_with_check">
                                                                                            <span class="box_text">Yes </span>
                                                                                        </label>--%>
            </li>
            <li class="ss_form_item">
                <%--Who is bringing the action--%>
                <label class="ss_form_input_title">Action User</label>
                <input type="text" class="ss_form_input" ng-model="LegalCase.SecondaryInfo.ActionUser" ng-change="LegalCase.SecondaryInfo.ActionUserId=null" uib-typeahead="contact.Name for contact in ptContactServices.getContacts($viewValue)" typeahead-on-select="LegalCase.SecondaryInfo.ActionUserId=$item.ContactId" bind-id="LegalCase.SecondaryInfo.ActionUserId">
                <%-- <div class="contact_box" dx-select-box="InitContact('LegalCase.SecondaryInfo.ActionUserId')">
                </div>--%>
            </li>
            <li class="ss_form_item" style="width: 97%">
                <%--Who is bringing the action--%>
                <label class="ss_form_input_title">Plaintiff Attorney</label>
                <input type="text" class="ss_form_input" ng-model="LegalCase.SecondaryInfo.PlaintiffAttorneyName" ng-change="LegalCase.SecondaryInfo.PlaintiffAttorneyId=null" uib-typeahead="contact.Name for contact in ptContactServices.getContacts($viewValue,3)" typeahead-on-select="LegalCase.SecondaryInfo.PlaintiffAttorneyId=$item.ContactId" bind-id="LegalCase.SecondaryInfo.PlaintiffAttorneyId">
            </li>

            <li class="ss_form_item">

                <label class="ss_form_input_title">Office#</label>
                <input type="text" class="ss_form_input" ng-model="ptContactServices.getContact(LegalCase.SecondaryInfo.PlaintiffAttorneyId, LegalCase.SecondaryInfo.PlaintiffAttorneyName).OfficeNO" mask="(999) 999-9999" clean="true" readonly="readonly">
            </li>
            <li class="ss_form_item">

                <label class="ss_form_input_title">Cell</label>
                <input type="text" class="ss_form_input" ng-model="ptContactServices.getContact(LegalCase.SecondaryInfo.PlaintiffAttorneyId, LegalCase.SecondaryInfo.PlaintiffAttorneyName).Cell" mask="(999) 999-9999" clean="true" readonly="readonly">
            </li>
            <li class="ss_form_item">

                <label class="ss_form_input_title">Address</label>
                <input type="text" class="ss_form_input" ng-model="ptContactServices.getContact(LegalCase.SecondaryInfo.PlaintiffAttorneyId, LegalCase.SecondaryInfo.PlaintiffAttorneyName).Address" readonly="readonly">
            </li>

            <li class="ss_form_item" style="width: 97%">
                <%--Who is bringing the action--%>
                <label class="ss_form_input_title">Defendant's Attorney</label>
                <input type="text" class="ss_form_input" ng-model="LegalCase.SecondaryInfo.DefendantAttorneyName" ng-change="LegalCase.SecondaryInfo.DefendantAttorneyId=null" uib-typeahead="contact.Name for contact in ptContactServices.getContacts($viewValue,3)" typeahead-on-select="LegalCase.SecondaryInfo.DefendantAttorneyId=$item.ContactId" bind-id="LegalCase.SecondaryInfo.DefendantAttorneyId">
            </li>
            <li class="ss_form_item">

                <label class="ss_form_input_title">Defendant</label>
                <input type="text" class="ss_form_input" ng-model="LegalCase.SecondaryInfo.Defendant">
            </li>
            <li class="ss_form_item">

                <label class="ss_form_input_title">Office#</label>
                <input type="text" class="ss_form_input" ng-model="ptContactServices.getContact(LegalCase.SecondaryInfo.DefendantAttorneyId, LegalCase.SecondaryInfo.DefendantAttorneyName).OfficeNO" mask="(999) 999-9999" clean="true" readonly="readonly">
            </li>

            <li class="ss_form_item">

                <label class="ss_form_input_title">Cell</label>
                <input type="text" class="ss_form_input" ng-model="ptContactServices.getContact(LegalCase.SecondaryInfo.DefendantAttorneyId, LegalCase.SecondaryInfo.DefendantAttorneyName).Cell" mask="(999) 999-9999" clean="true" readonly="readonly">
            </li>
            <li class="ss_form_item">

                <label class="ss_form_input_title">Address</label>
                <input type="text" class="ss_form_input" ng-model="ptContactServices.getContact(LegalCase.SecondaryInfo.DefendantAttorneyId, LegalCase.SecondaryInfo.DefendantAttorneyName).Address" readonly="readonly">
            </li>
            <li class="ss_form_item ss_form_item_line">
                 <h4 class="ss_form_title">Defendants <i class="fa fa-plus-circle icon_btn color_blue tooltip-examples" onclick="NGAddArrayitemScope('LegalCtrl','LegalCase.SecondaryInfo.Defendants')"  title="Add" style="font-size: 18px" data-original-title="Add"></i> </h4>
          
             </li>

             <li class="ss_form_item" ng-repeat="d in LegalCase.SecondaryInfo.Defendants track by $index">

                <label class="ss_form_input_title">Defendant {{$index +1}} <i class="fa fa-times icon_btn  tooltip-examples" ng-click="ptCom.arrayRemove(LegalCase.SecondaryInfo.Defendants,$index)"  title="Add" style="font-size: 18px" data-original-title="Delete"></i></label>
                <input type="text" class="ss_form_input" ng-model="d.Name">
            </li>
            <li class="ss_form_item ss_form_item_line">
                <label class="ss_form_input_title">note</label>
                <textarea class="edit_text_area text_area_ss_form" ng-model="LegalCase.SecondaryInfo.QuietTitleNotes"></textarea>
            </li>
        </ul>
    </div>
</div>


<%-- --------------------------------------------------------------%>