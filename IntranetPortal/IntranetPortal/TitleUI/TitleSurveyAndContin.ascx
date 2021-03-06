﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TitleSurveyAndContin.ascx.vb" Inherits="IntranetPortal.TitleSurveyAndContin" %>
<div class="ss_form ">
    <h4 class="ss_form_title ">Contins</h4>
    <div class="ss_border">
        <ul class="ss_form_box clearfix">
            <li class="ss_form_item ">
                <span class="label label-primary">Title Contin</span>
            </li>
            <li class="ss_form_item ">
                <label class="ss_form_input_title ">Date Requested</label>
                <input class="ss_form_input " pt-date ng-model="Form.FormData.surveyAndContin.Title_Date_Requested">
            </li>
            <li class="ss_form_item ">
                <label class="ss_form_input_title ">Date Received</label>
                <input class="ss_form_input " pt-date ng-model="Form.FormData.surveyAndContin.Title_Date_Received">
            </li>
            <li class="ss_form_item ">
                <span class="label label-primary">Tax and Water Contin</span>
            </li>
            <li class="ss_form_item ">
                <label class="ss_form_input_title ">Date Requested</label>
                <input class="ss_form_input " pt-date ng-model="Form.FormData.surveyAndContin.Tax_and_Water_Date_Requested">
            </li>
            <li class="ss_form_item ">
                <label class="ss_form_input_title ">Date Received</label>
                <input class="ss_form_input " pt-date ng-model="Form.FormData.surveyAndContin.Tax_and_Water_Date_Received">
            </li>
            <li class="ss_form_item ">
                <span class="label label-primary">ER Contin</span>
            </li>
            <li class="ss_form_item ">
                <label class="ss_form_input_title ">Date Requested</label>
                <input class="ss_form_input " pt-date ng-model="Form.FormData.surveyAndContin.ER_Date_Requested">
            </li>
            <li class="ss_form_item ">
                <label class="ss_form_input_title ">Date Received</label>
                <input class="ss_form_input " pt-date ng-model="Form.FormData.surveyAndContin.ER_Date_Received">
            </li>
        </ul>
    </div>
</div>
<div class="ss_form ">
    <h4 class="ss_form_title ">SURVEYS</h4>
    <div class="ss_border">
        <ul class="ss_form_box clearfix">
            <li class="ss_form_item ">
                <label class="ss_form_input_title ">Survey Located</label>
                <pt-radio name="Survey_Located" model="Form.FormData.surveyAndContin.Survey_Located"></pt-radio>
            </li>
            <li class="ss_form_item ">
                <label class="ss_form_input_title ">New Survey Needed</label>
                <pt-radio name="New_Survey_Needed" model="Form.FormData.surveyAndContin.New_Survey_Needed"></pt-radio>
            </li>
            <li class="ss_form_item ">
                <label class="ss_form_input_title ">Order Date</label>
                <input class="ss_form_input " pt-date ng-model="Form.FormData.surveyAndContin.Survey_Order_Date">
            </li>
            <li class="ss_form_item ">
                <label class="ss_form_input_title ">Received Date</label>
                <input class="ss_form_input " ng-model="Form.FormData.surveyAndContin.Survey_Received_Date">
            </li>
            <li class="ss_form_item ">
                <label class="ss_form_input_title ">Submitted to Title Company</label>
                <pt-radio name="Submitted_to_Title_Company" model="Form.FormData.surveyAndContin.Submitted_to_Title_Company"></pt-radio>
            </li>
            <li class="ss_form_item ">
                <label class="ss_form_input_title ">Survery Reading Recevied</label>
                <pt-radio name="Survery_Reading_Recevied" model="Form.FormData.surveyAndContin.Survery_Reading_Recevied"></pt-radio>
            </li>
        </ul>
    </div>
</div>
