﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TitlePreclosing.ascx.vb" Inherits="IntranetPortal.TitlePreclosing" %>
<div class="ss_form ">
    <h4 class="ss_form_title ">POA</h4>
    <ul class="ss_form_box clearfix">
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="POA_Upload" file-model="FormData.preclosing.POA_Upload"></pt-file>
        </li>
        <li class="ss_form_item  ss_form_item_line">
            <label class="ss_form_input_title ">Notes</label>
            <textarea class="edit_text_area text_area_ss_form " model="FormData.preclosing.POA_Notes"></textarea>
        </li>
    </ul>
</div>
<div class="ss_form ">
    <h4 class="ss_form_title ">WILLS</h4>
    <ul class="ss_form_box clearfix">
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="WILLS_Upload" file-model="FormData.preclosing.WILLS_Upload"></pt-file>
        </li>
        <li class="ss_form_item  ss_form_item_line">
            <label class="ss_form_input_title ">Notes</label>
            <textarea class="edit_text_area text_area_ss_form " model="FormData.preclosing.WILLS_Notes"></textarea>
        </li>
    </ul>
</div>
<div class="ss_form ">
    <h4 class="ss_form_title ">Short Sale Documents</h4>
    <ul class="ss_form_box clearfix">
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">HUD</label>
            <pt-radio name="ShortSaleDocuments_HUD0" model="FormData.preclosing.HUD"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="HUD_Upload" file-model="FormData.preclosing.HUD_Upload"></pt-file>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Approval</label>
            <pt-radio name="ShortSaleDocuments_Approval0" model="FormData.preclosing.Approval"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="Approval_Upload" file-model="FormData.preclosing.Approval_Upload"></pt-file>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Servicing Agrement/POA</label>
            <pt-radio name="ShortSaleDocuments_ServicingAgrement/POA0" model="FormData.preclosing.Servicing_Agrement_POA"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="Servicing_Agrement_POA_Upload" file-model="FormData.preclosing.Servicing_Agrement_POA_Upload"></pt-file>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Contract of Sale</label>
            <pt-radio name="ShortSaleDocuments_ContractofSale0" model="FormData.preclosing.Contract_of_Sale"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="Contract_of_Sale_Upload" file-model="FormData.preclosing.Contract_of_Sale_Upload"></pt-file>
        </li>
        <li class="ss_form_item  ss_form_item_line">
            <label class="ss_form_input_title ">Notes</label>
            <textarea class="edit_text_area text_area_ss_form " model="FormData.preclosing.Short_Sale_Documents_Notes"></textarea>
        </li>
    </ul>
</div>
<div class="ss_form ">
    <h4 class="ss_form_title ">CORP/LLC DOCUMENTS</h4>
    <ul class="ss_form_box clearfix">
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Filing Receipts</label>
            <pt-radio name="Filing_Receipts" model="FormData.preclosing.Filing_Receipts"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="Filing_Receipts_Upload" file-model="FormData.preclosing.Filing_Receipts_Upload"></pt-file>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Proof of Publication</label>
            <pt-radio name="Proof_of_Publication" model="FormData.preclosing.Proof_of_Publication"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="Proof_of_Publication_Upload" file-model="FormData.preclosing.Proof_of_Publication_Upload"></pt-file>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Operating Agreeements</label>
            <pt-radio name="Operating_Agreeements" model="FormData.preclosing.Operating_Agreeements"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="Operating_Agreeements_Upload" file-model="FormData.preclosing.Operating_Agreeements_Upload"></pt-file>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">By Laws of Corp</label>
            <pt-radio name="By_Laws_of_Corp" model="FormData.preclosing.By_Laws_of_Corp"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="By_Laws_of_Corp_Upload" file-model="FormData.preclosing.By_Laws_of_Corp_Upload"></pt-file>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Certificate of Good Standing</label>
            <pt-radio name="Certificate_of_Good_Standing" model="FormData.preclosing.Certificate_of_Good_Standing"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="Certificate_of_Good_Standing_Upload" file-model="FormData.preclosing.Certificate_of_Good_Standing_Upload"></pt-file>
        </li>
    </ul>
</div>
<div class="ss_form ">
    <h4 class="ss_form_title ">Closing Requirements</h4>
    <ul class="ss_form_box clearfix">
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Deed reverting title</label>
            <pt-radio name="Deed_reverting_title" model="FormData.preclosing.Deed_reverting_title"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="Deed_reverting_title_Upload" file-model="FormData.preclosing.Deed_reverting_title_Upload"></pt-file>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Correction deed</label>
            <pt-radio name="Correction_deed" model="FormData.preclosing.Correction_deed"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="Correction_deed_Upload" file-model="FormData.Correction_deed_Upload"></pt-file>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Closing Deed and Acris</label>
            <pt-radio name="Closing_Deed_and_Acris" model="FormData.preclosing.Closing_Deed_and_Acris"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="Closing_Deed_and_Acris_Upload" file-model="FormData.Closing_Deed_and_Acris_Upload"></pt-file>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">no consideration affidavit</label>
            <pt-radio name="no_consideration_affidavit" model="FormData.preclosing.no_consideration_affidavit"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="no_consideration_affidavit_Upload" file-model="FormData.no_consideration_affidavit_Upload"></pt-file>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">no demand affidavit</label>
            <pt-radio name="no_demand_affidavit" model="FormData.preclosing.no_demand_affidavit"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="no_demand_affidavit_Upload" file-model="FormData.no_demand_affidavit_Upload"></pt-file>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">original marriage/death certificates</label>
            <pt-radio name="original_marriage_death_certificates" model="FormData.preclosing.original_marriage_death_certificates"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="original_marriage_death_certificates_Upload" file-model="FormData.original_marriage_death_certificates_Upload"></pt-file>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Original letter of Administration</label>
            <pt-radio name="Original_letter_of_Administration" model="FormData.preclosing.Original_letter_of_Administration"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="Original_letter_of_Administration_Upload" file-model="FormData.Original_letter_of_Administration_Upload"></pt-file>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Original POA</label>
            <pt-radio name="Original_POA" model="FormData.preclosing.Original_POA"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="Original_POA_Upload" file-model="FormData.Original_POA_Upload"></pt-file>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Affidavit of full force and effect if closing by POA</label>
            <pt-radio name="Affidavit_of_full_force" model="FormData.preclosing.Affidavit_of_full_force"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="Affidavit_of_full_force_Upload" file-model="FormData.Affidavit_of_full_force_Upload"></pt-file>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Termination of Contract</label>
            <pt-radio name="Termination_of_Contract" model="FormData.preclosing.Termination_of_Contract"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="Termination_of_Contract_Upload" file-model="FormData.Termination_of_Contract_Upload"></pt-file>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">LLC Resignation and Assignment of LLC</label>
            <pt-radio name="LLC_Resignation_and_Assignment_LLC" model="FormData.preclosing.LLC_Resignation_and_Assignment_LLC"></pt-radio>
        </li>
        <li class="ss_form_item ">
            <label class="ss_form_input_title ">Upload</label>
            <pt-file file-bble="" file-id="LLC_Resignation_and_Assignment_LLC_Upload" file-model="FormData.LLC_Resignation_and_Assignment_LLC_Upload"></pt-file>
        </li>
    </ul>
</div>
