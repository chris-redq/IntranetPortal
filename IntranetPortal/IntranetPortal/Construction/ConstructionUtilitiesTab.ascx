﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ConstructionUtilitiesTab.ascx.vb" Inherits="IntranetPortal.ConstructionUtilitiesTab" %>

<div class="ss_form">
    <h4 class="ss_form_title">Utility Company</h4>
    <ul class="ss_form_box clearfix">
        <li class="ss_form_item3">
            <div dx-tag-box="{
                dataSource: [{name: 'ConED'},
                            {name: 'Energy Service'},
                            {name: 'National Grid'},
                            {name: 'DEP'},
                            {name: 'MISSING Water Meter'},
                            {name: 'Taxes'},
                            {name: 'ADT'},
                            {name: 'Insurance'}],
                valueExpr: 'name',
                displayExpr: 'name',
                bindingOptions: {values: 'CSCase.CSCase.Utilities.Company'}}">
            </div>
        </li>
    </ul>
</div>

<%-- ConED --%>

<div class="ss_form" ng-init="CSCase.CSCase.Utilities.ConED_Shown=false" ng-show="CSCase.CSCase.Utilities.ConED_Shown">
    <h4 class="ss_form_title">ConED<pt-collapse model="CSCase.CSCase.Utilities.ConED_Collapsed" /></h4>

    <div class="ss_border" collapse="CSCase.CSCase.Utilities.ConED_Collapsed" ng-init="CSCase.CSCase.Utilities.ConED_Collapsed=false">
        <tabset class="tab-switch">
        <tab ng-repeat="floor in CSCase.CSCase.Utilities.ConED" active="floor.active" disable="floor.disabled" >
        <tab-heading>Floor {{floor.FloorNum}} </tab-heading>
        
            <ul class="ss_form_box clearfix">
            <li class="ss_form_item">
                <label class="ss_form_input_title">Floor #</label>
                <input class="ss_form_input" ng-model="floor.FloorNum">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Account Number</label>
                <input class="ss_form_input" ng-model="floor.AccountNum">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">DATE</label>
                <input class="ss_form_input" type="text" ng-model="floor.Date" ss-date />
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Rep Name</label>
                <input class="ss_form_input" ng-model="floor.RepName" />
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Account Open</label>
                <pt-radio name="CSCase-Utilities-ConED-AccountOpen" model="floor.AccountOpen"></pt-radio>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Energy service required</label>
                <pt-radio name="CSCase-Utilities-ConED-EnergyServiceRequired" model="floor.EnergyServiceRequired"></pt-radio>
            </li>

        </ul>
        
            <ul class="ss_form_box clearfix">
            <li class="ss_form_item">
                <label class="ss_form_input_title">Service</label>
                <pt-radio name="CSCase-Utilities-ConED-Service" model="floor.Service" true-value="on" false-value="off"></pt-radio>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Meter Number</label>
                <input class="ss_form_input" ng-model="floor.MeterNum">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Missing/Damaged Meter</label>
                <select class="ss_form_input" ng-model="floor.MissingMeter">
                    <option value="PLP">PLP</option>
                    <option value="bsmt">bsmt</option>
                    <option value="1st_Fillable">1st Fillable</option>
                    <option value="2nd_Fillable">2nd Fillable</option>
                    <option value="3rd_Fillable">3rd Fillable</option>
                </select>
            </li>
        </ul>
        
            <div class="cssSlideUp" ng-show="!floor.Service">
            <div class="arrow_box" style="width: 40%">
                <label class="ss_form_input_title">Service Off Reason</label>
                <select class="ss_form_input" ng-model="floor.ServiceOffReason">
                    <option value="Service_Cut_From_Street">Service Cut From Street</option>
                    <option value="Meter_Locked">Meter Locked</option>
                    <option value="Meter_Mising">Meter Mising</option>
                    <option value="Meter_Damaged">Meter Damaged</option>
                </select>
            </div>
        </div>

            <div class="ss_form_item">
            <label class="ss_form_input_title">Appointments</label>
            <input class="ss_form_input" type="text" ng-model="floor.Appointments" ss-date>
        </div>
        
            <div class="ss_form_item">
            <label class="ss_form_input_title">Upload</label>
            <pt-file file-id="CSCase-Utilities-EnergyService-Upload" file-model="floor.Upload"></pt-file>
        </div>
        
            <div>
            <label class="ss_form_input_title">Note</label>
            <textarea class="edit_text_area text_area_ss_form " ng-model="floor.Note"></textarea>
        </div>
        </tab>
        <pt-add></pt-add>
        </tabset>
    </div>
</div>


<%-- EnergyService --%>
<div class="ss_form" ng-init="CSCase.CSCase.Utilities.EnergyService_Shown=false" ng-show="CSCase.CSCase.Utilities.EnergyService_Shown">
    <h4 class="ss_form_title">Energy Service<pt-collapse model="CSCase.CSCase.Utilities.EnergyService_Collapsed" /></h4>
    <div class="ss_border" collapse="CSCase.CSCase.Utilities.EnergyService_Collapsed" ng-init="CSCase.CSCase.Utilities.EnergyService_Collapsed=false">
        <ul class="ss_form_box clearfix">
            <li class="ss_form_item">
                <label class="ss_form_input_title">Floor #</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.EnergyService_FloorNum">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Account Number</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.EnergyService_AccountNum">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Date</label>
                <input class="ss_form_input" type="text" ng-model="CSCase.CSCase.Utilities.EnergyService_Date" ss-date>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Case Number</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.EnergyService_CaseNum">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Lic Electrician</label>
                <input type="text" class="ss_form_input" ng-model="CSCase.CSCase.Utilities.EnergyService_LicElectrician" ng-change="" typeahead="contact.Name for contact in ptContactServices.getContacts($viewValue)" typeahead-on-select="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Electric permit pulled Date</label>
                <input class="ss_form_input" type="text" ng-model="CSCase.CSCase.Utilities.EnergyService_ElecPermPulledDate" ss-date>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">City permit pulled date</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.EnergyService_CityPermitPulledDate" ss-date>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Check List Submitted Date</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.EnergyService_DateCheckListSubmitted" ss-date>
            </li>
        </ul>
        <ul class="ss_form_box clearfix">
            <li class="ss_form_item">
                <label class="ss_form_input_title">Energy Service</label>
                <pt-radio name="CSCase-Utilities-EnergyService-EnergyServiceCheck" model="CSCase.CSCase.Utilities.EnergyService_EnergyServiceCheck" true-value="on" false-value="off"></pt-radio>
            </li>
        </ul>
        <div class="cssSlideUp" ng-show="!CSCase.CSCase.Utilities.EnergyService_EnergyServiceCheck">
            <div class="arrow_box" style="width: 40%">
                <label class="ss_form_input_title">Service Off Reason</label>
                <select class="ss_form_input">
                    <option value="Street_Turn_On">Street turn on</option>
                    <option value="Upgrade_Service">Upgrade Service</option>
                    <option value="New_Meter">New meter</option>
                </select>
            </div>
        </div>

        <div class="ss_form_item">
            <label class="ss_form_input_title">Appointments</label>
            <input class="ss_form_input" type="text" ng-model="CSCase.CSCase.Utilities.EnergyService_Appointments" ss-date>
        </div>
        <div class="ss_form_item">
            <label class="ss_form_input_title">Upload</label>
            <pt-file file-id="CSCase-Utilities-EnergyService-Upload" file-model="CSCase.CSCase.Utilities.EnergyService_Upload"></pt-file>
        </div>
        <div>
            <label class="ss_form_input_title">Notes</label>
            <textarea class="edit_text_area text_area_ss_form" ng-model="CSCase.CSCase.Utilities.EnergyService_Notes"></textarea>
        </div>


    </div>
</div>

<%-- NationalGrid --%>
<div class="ss_form" ng-init="CSCase.CSCase.Utilities.NationalGrid_Shown=false" ng-show="CSCase.CSCase.Utilities.NationalGrid_Shown">
    <h4 class="ss_form_title">National Grid<pt-collapse model="CSCase.CSCase.Utilities.NationalGrid_Collapsed" /></h4>
    <div class="ss_border" collapse="CSCase.CSCase.Utilities.NationalGrid_Collapsed" ng-init="CSCase.CSCase.Utilities.NationalGrid_Collapsed=false">
        <ul class="ss_form_box clearfix">
            <li class="ss_form_item">
                <label class="ss_form_input_title">Floor #</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.NationalGrid_FloorNum">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Account Number</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.NationalGrid_AccountNum">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Date</label>
                <input class="ss_form_input" type="text" ng-model="CSCase.CSCase.Utilities.NationalGrid_Date" ss-date>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Rep Name</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.NationalGrid_RepName">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Account Open</label>
                <pt-radio name="CSCase-Utilities-NationalGrid-AccountOpen" model="CSCase.CSCase.Utilities.NationalGrid_AccountOpen"></pt-radio>
            </li>
        </ul>
        <ul class="ss_form_box clearfix">
            <li class="ss_form_item">
                <label class="ss_form_input_title">Service</label>
                <pt-radio name="CSCase-Utilities-NationalGrid-Service" model="CSCase.CSCase.Utilities.NationalGrid_Service" true-value="on" false-value="off"></pt-radio>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Meter Number</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.NationalGrid_MeterNumber">
            </li>
        </ul>
        <div class="cssSlideUp" ng-show="!CSCase.CSCase.Utilities.NationalGrid_Service">
            <div class="arrow_box" style="width: 40%">
                <label class="ss_form_input_title">Service Off Reason</label>
                <select class="ss_form_input" ng-model="CSCase.CSCase.Utilities.NationalGrid_ServiceOffReason">
                    <option value="Service_Cut_From_Street">Service Cut From Street</option>
                    <option value="Meter_Locked">Meter Locked</option>
                    <option value="Meter_Mising">Meter Mising</option>
                    <option value="Meter_Damaged">Meter Damaged</option>
                </select>
            </div>
        </div>
        <div class="ss_form_item">
            <label class="ss_form_input_title">Appointments</label>
            <input class="ss_form_input" type="text" ng-model="CSCase.CSCase.Utilities.NationalGrid_Appointments" ss-date>
        </div>
        <div class="ss_form_item">
            <label class="ss_form_input_title">Upload</label>
            <pt-file file-id="CSCase-Utilities-EnergyService_Upload" file-model="CSCase.CSCase.Utilities.NationalGrid_Upload"></pt-file>
        </div>
        <div>
            <label class="ss_form_input_title">Notes</label>
            <textarea class="edit_text_area text_area_ss_form" ng-model="CSCase.CSCase.Utilities.NationalGrid_Notes"></textarea>
        </div>

    </div>
</div>

<%-- DEP --%>
<div class="ss_form" ng-init="CSCase.CSCase.Utilities.DEP_Shown=false" ng-show="CSCase.CSCase.Utilities.DEP_Shown">
    <h4 class="ss_form_title">DEP<pt-collapse model="CSCase.CSCase.Utilities.DEP_Collapsed" /></h4>
    <div class="ss_border" collapse="CSCase.CSCase.Utilities.DEP_Collapsed" ng-init="CSCase.CSCase.Utilities.DEP_Collapsed=false">
        <ul class="ss_form_box clearfix">
            <li class="ss_form_item">
                <label class="ss_form_input_title">Account Number</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.DEP_AccountNum">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Date</label>
                <input class="ss_form_input" type="text" ng-model="CSCase.CSCase.Utilities.DEP_Date" ss-date>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Account Name</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.DEP_AccountName">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Open Charges</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.DEP_OpenCharges">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Cancellation Date</label>
                <input class="ss_form_input" type="text" ng-model="CSCase.CSCase.Utilities.DEP_CancellationDate" ss-date>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Service</label>
                <pt-radio name="CSCase-Utilities-DEP-Service" model="CSCase.CSCase.Utilities.DEP_Service" true-value="on" false-value="off"></pt-radio>
            </li>

        </ul>
        <ul class="ss_form_box clearfix">
            <li class="ss_form_item">
                <label class="ss_form_input_title">Meter Number</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.DEP_MeterNumber">
            </li>
        </ul>
        <ul class="ss_form_box clearfix">
            <li class="ss_form_item">
                <label class="ss_form_input_title">Water Lien</label>
                <pt-radio name="CSCase-Utilities-DEP-WaterLien" model="CSCase.CSCase.Utilities.DEP_WaterLien"></pt-radio>
            </li>
            <li class="ss_form_item" ng-show="CSCase.CSCase.Utilities.DEP_WaterLien">
                <label class="ss_form_input_title">Upload Payment Agreement</label>
                <pt-file file-id="CSCase-Utilities-DEP-PaymentAgreement" file-model="CSCase.CSCase.Utilities.DEP_PaymentAgreement"></pt-file>
            </li>
        </ul>
        <div class="ss_form_item">
            <label class="ss_form_input_title">Appointments</label>
            <input class="ss_form_input" type="text" ng-model="CSCase.CSCase.Utilities.DEP_Appointments" ss-date>
        </div>
        <div>
            <label class="ss_form_input_title">Notes</label>
            <textarea class="edit_text_area text_area_ss_form" ng-model="CSCase.CSCase.Utilities.DEP_Notes"></textarea>
        </div>
    </div>
</div>

<%-- Missing Water Metor--%>
<div class="ss_form" ng-init="CSCase.CSCase.Utilities.MissingMeter_Shown=false" ng-show="CSCase.CSCase.Utilities.MissingMeter_Shown">
    <h4 class="ss_form_title">Missing Water Meter<pt-collapse model="CSCase.CSCase.Utilities.MissingMeter_Collapsed" /></h4>
    <div class="ss_border" collapse="CSCase.CSCase.Utilities.MissingMeter_Collapsed" ng-init="CSCase.CSCase.Utilities.MissingMeter_Collapsed=false;">
        <ul class="ss_form_box clearfix">
            <li class="ss_form_item">
                <label class="ss_form_input_title">Account Number</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.MissingMeter_AccountNum">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Plumber</label>
                <input type="text" class="ss_form_input" ng-model="CSCase.CSCase.Utilities.MissingMeter_Plumber" ng-change="" typeahead="contact.Name for contact in ptContactServices.getContacts($viewValue)" typeahead-on-select="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Permit pulled date</label>
                <input class="ss_form_input" type="text" ng-model="CSCase.CSCase.Utilities.MissingMeter_PermitPulledDate" ss-date>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Meter Registered Date</label>
                <input class="ss_form_input" type="text" ng-model="CSCase.CSCase.Utilities.MissingMeter_MeterRegisteredDate" ss-date>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Meter Size</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.MissingMeter_MeterSize">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Tap Size</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.MissingMeter_TapSize">
            </li>
        </ul>
        <div>
            <label class="ss_form_input_title">Upload Letter To DEP</label>
            <pt-file class="ss_form_input" file-id="CSCase-Utilities-MissingMeter-Upload" file-model="CSCase.CSCase.Utilities.MissingMeter_Upload"></pt-file>
        </div>
        <div>
            <label class="ss_form_input_title">Notes</label>
            <textarea class="edit_text_area text_area_ss_form" ng-model="CSCase.CSCase.Utilities.MissingMeter_Notes"></textarea>
        </div>
    </div>
</div>

<%-- Taxes --%>
<div class="ss_form" ng-init="CSCase.CSCase.Utilities.Taxes_Shown=false" ng-show="CSCase.CSCase.Utilities.Taxes_Shown">
    <h4 class="ss_form_title">Taxes<pt-collapse model="CSCase.CSCase.Utilities.Taxes_Collapsed" /></h4>
    <div class="ss_border" collapse="CSCase.CSCase.Utilities.Taxes_Collapsed" ng-init="CSCase.CSCase.Utilities.Taxes_Collapsed=false">
        <ul class="ss_form_box clearfix">
            <li class="ss_form_item">
                <label class="ss_form_input_title">Account Number</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.Taxes_AccountNum">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Account Type</label>
                <select class="ss_form_input" ng-model="CSCase.CSCase.Utilities.Taxes_AccountType">
                    <option value="ERP">ERP</option>
                    <option value="Registration">Registration</option>
                    <option value="Tax">Tax</option>
                </select>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Account Name</label>
                <input type="text" class="ss_form_input" ng-model="CSCase.CSCase.Utilities.Taxes_NameAccountIsUnder" ng-change="" typeahead="contact.Name for contact in ptContactServices.getContacts($viewValue)" typeahead-on-select="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Open Charges</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.Taxes_OpenCharges">
            </li>
        </ul>
        <ul class="ss_form_box clearfix">
            <li class="ss_form_item">
                <label class="ss_form_input_title">Tax Lien</label>
                <pt-radio name="CSCase-Utilities-Taxes-TaxLien" model="CSCase.CSCase.Utilities.Taxes_TaxLien"></pt-radio>
            </li>
            <li class="ss_form_item" ng-show="!CSCase.CSCase.Utilities.Taxes_TaxLien">
                <label class="ss_form_input_title">Upload Payment Agreement</label>
                <pt-file file-id="CSCase-Utilities-Taxes_PaymentAgreement" file-model="CSCase.CSCase.Utilities.Taxes_PaymentAgreement"></pt-file>
            </li>
        </ul>
    </div>
</div>

<%-- ADT --%>
<div class="ss_form" ng-init="CSCase.CSCase.Utilities.ADT_Shown=false" ng-show="CSCase.CSCase.Utilities.ADT_Shown">
    <h4 class="ss_form_title">ADT<pt-collapse model="CSCase.CSCase.Utilities.ADT_Collapsed" /></h4>
    <div class="ss_border" collapse="CSCase.CSCase.Utilities.ADT_Collapsed" ng-init="CSCase.CSCase.Utilities.ADT_Collapsed=false">
        <ul class="ss_form_box clearfix">
            <li class="ss_form_item">
                <label class="ss_form_input_title">Account Number</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.ADT_AccountNum">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Date Requested</label>
                <input class="ss_form_input" type="text" ng-model="CSCase.CSCase.Utilities.ADT_DateRequest" ss-date>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Installation Date</label>
                <input class="ss_form_input" type="text" ng-model="CSCase.CSCase.Utilities.ADT_InstallationDate" ss-date>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Cancellation Date</label>
                <input class="ss_form_input" type="text" ng-model="CSCase.CSCase.Utilities.ADT_CancellatonDate" ss-date>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Access Code</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.InitialIntake.ADT">
            </li>
        </ul>
    </div>
</div>

<%-- Insurance --%>
<div class="ss_form" ng-init="CSCase.CSCase.Utilities.Insurance_Shown=false" ng-show="CSCase.CSCase.Utilities.Insurance_Shown">
    <h4 class="ss_form_title">Insuracne<pt-collapse model="CSCase.CSCase.Utilities.Insurance_Collapsed" /></h4>
    <div class="ss_border" collapse="CSCase.CSCase.Utilities.Insurance_Collapsed" ng-init="CSCase.CSCase.Utilities.Insurance_Collapsed=false">
        <ul class="ss_form_box clearfix">
            <li class="ss_form_item">
                <label class="ss_form_input_title">Account Number</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.Insurance_AccountNum">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Policy Number</label>
                <input class="ss_form_input" ng-model="CSCase.CSCase.Utilities.Insurance_PolicyNum">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Expiration Date</label>
                <input class="ss_form_input" type="text" ng-model="CSCase.CSCase.Utilities.Insurance_ExpirationDate" ss-date>
            </li>

            <li class="ss_form_item">
                <label class="ss_form_input_title">Cancellation Date</label>
                <input class="ss_form_input" type="text" ng-model="CSCase.CSCase.Utilities.Insurance_CancellationDate" ss-date>
            </li>
        </ul>
        <div class="ss_form_item">
            <label class="ss_form_input_title">Upload</label>
            <pt-file file-id="CSCase-Utilities-InsuranceUpload" file-model="CSCase.CSCase.Utilities.Insurance_Upload"></pt-file>
        </div>
        <div class="ss_form_item">
            <label class="ss_form_input_title">Renewal Remind</label>
            <input class="ss_form_input" type="text" ng-model="CSCase.CSCase.Utilities.Insurance_CalenderOption" ss-date>
        </div>
    </div>

</div>
