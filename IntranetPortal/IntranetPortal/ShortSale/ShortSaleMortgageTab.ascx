﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ShortSaleMortgageTab.ascx.vb" Inherits="IntranetPortal.ShortSaleMortgageTab" %>
<%@ Import Namespace="IntranetPortal.ShortSale" %>
<script src="/scripts/stevenjs.js"></script>
<div class="clearfix">
    <div style="float: right">
        <input type="button" class="rand-button short_sale_edit" value="Edit" onclick='swich_edit_model(this, short_sale_case_data)' />
    </div>
</div>



<div data-array-index="0" data-field="Mortgages" class="ss_array" >
    <h2>1St Mortgages</h2>
    <div class="ss_form">
        <h4 class="ss_form_title">1st Mortgage<i class="fa fa-minus-square-o color_blue collapse_btn" onclick="clickCollapse(this, 'mortgage1')"></i> &nbsp;<i class="fa fa-plus-circle icon_btn color_blue" title="Add"></i></h4>
        <ul class="ss_form_box clearfix" id="mortgage1">

            <li class="ss_form_item">
                <label class="ss_form_input_title">LENDER</label>
                <input class="ss_form_input" data-item="Lender" data-item-type="1" id="Lender" >
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Loan #</label>
                <input class="ss_form_input" data-item="Loan" data-item-type="1" id="Loan">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Loan Amount</label>
                <input class="ss_form_input currency_input" data-item="LoanAmount" data-item-type="1" id="LoanAmount" onblur="$(this).formatCurrency();">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Authorization Sent</label>
                <input class="ss_form_input" id="AuthorizationSent">
            </li>

            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Lien Postion</label>
                <select class="ss_form_input">
                    <option value="volvo">1 st</option>
                    <option value="saab">2 nd</option>
                    <option value="mercedes">3 rd</option>
                    <option value="audi">4 th</option>
                </select>

            </li>
        </ul>
    </div>

    <div class="ss_form">
        <h4 class="ss_form_title">Short sale dept <i class="fa fa-minus-square-o color_blue collapse_btn" onclick="clickCollapse(this, 'short_sale_dept')"></i></h4>
        <ul class="ss_form_box clearfix" id="short_sale_dept">

            <li class="ss_form_item">
                <label class="ss_form_input_title">phone #</label>
                <input class="ss_form_input" id="ShortSaleContact">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Extension</label>
                <input class="ss_form_input" value="123">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">customer service</label>
                <input class="ss_form_input" value="346-123-456">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Extension</label>
                <input class="ss_form_input" value="12">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Fax #</label>
                <input class="ss_form_input" value="347-123-456">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Email</label>
                <input class="ss_form_input" value="example@email.com">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
        </ul>
    </div>

    <div class="ss_form">
        <h4 class="ss_form_title">Processor<i class="fa fa-minus-square-o color_blue collapse_btn" onclick="clickCollapse(this, 'processor_list')"></i></h4>
        <ul class="ss_form_box clearfix" id="processor_list">
            <li class="ss_form_item">
                <label class="ss_form_input_title">Name</label>
                <input class="ss_form_input" id="Processor">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Date Assigned to Processor</label>
                <input class="ss_form_input " value="Jun 23 ,2014">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>

            <li class="ss_form_item">
                <label class="ss_form_input_title">Phone #</label>
                <input class="ss_form_input" value="347-123-456">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Extension</label>
                <input class="ss_form_input " value="416">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Fax #</label>
                <input class="ss_form_input " value="347-123-416">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">email</label>
                <input class="ss_form_input" value="">
            </li>
        </ul>

    </div>

    <div class="ss_form">
        <h4 class="ss_form_title">Negotiator<i class="fa fa-minus-square-o color_blue collapse_btn" onclick="clickCollapse(this, 'negotiator_list')"></i></h4>
        <ul class="ss_form_box clearfix" id="negotiator_list">
            <li class="ss_form_item">
                <label class="ss_form_input_title">Name</label>
                <input class="ss_form_input" id="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>

            <li class="ss_form_item">
                <label class="ss_form_input_title">Phone #</label>
                <input class="ss_form_input" value="347-123-456">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Extension</label>
                <input class="ss_form_input " value="616">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Fax #</label>
                <input class="ss_form_input " value="347-123-416">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">email</label>
                <input class="ss_form_input " value="">
            </li>
        </ul>
    </div>

    <div class="ss_form">
        <h4 class="ss_form_title">Supervisor<i class="fa fa-minus-square-o color_blue collapse_btn" onclick="clickCollapse(this, 'supervisor_list')"></i></h4>
        <ul class="ss_form_box clearfix" id="supervisor_list">
            <li class="ss_form_item">
                <label class="ss_form_input_title">Name</label>
                <input class="ss_form_input" id="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">extension</label>
                <input class="ss_form_input " value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>

            <li class="ss_form_item">
                <label class="ss_form_input_title">Phone #</label>
                <input class="ss_form_input" value="347-123-456">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Extension</label>
                <input class="ss_form_input " value="616">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Fax #</label>
                <input class="ss_form_input " value="347-123-416">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">email</label>
                <input class="ss_form_input " value="supervisor@email.com">
            </li>
        </ul>
    </div>

    <div class="ss_form">
        <h4 class="ss_form_title">Closer
        <i class="fa fa-minus-square-o color_blue collapse_btn" onclick="clickCollapse(this, 'closer_list')"></i>

        </h4>
        <ul class="ss_form_box clearfix" id="closer_list">
            <li class="ss_form_item">
                <label class="ss_form_input_title">closer</label>
                <input class="ss_form_input" id="Closer">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">extension</label>
                <input class="ss_form_input " value="56">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>

            <li class="ss_form_item">
                <label class="ss_form_input_title">Phone #</label>
                <input class="ss_form_input" value="347-123-456">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Extension</label>
                <input class="ss_form_input " value="616">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Fax #</label>
                <input class="ss_form_input " value="347-123-416">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">&nbsp;</label>
                <input class="ss_form_input ss_form_hidden" value="">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">email</label>
                <input class="ss_form_input " value="closer@email.com">
            </li>
        </ul>
    </div>

</div>


