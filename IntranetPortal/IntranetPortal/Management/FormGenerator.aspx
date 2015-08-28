﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="FormGenerator.aspx.vb" Inherits="IntranetPortal.FormGenerator" MasterPageFile="~/Content.Master" %>

<asp:Content runat="server" ContentPlaceHolderID="head">
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="MainContentPH">
    <div id="FormCtrl" ng-controller="FormCtrl">

        <div class="container-fluid">
            <div class="row">
                <div class="col-md-6">
                    <div id="template">

                        <div class="ss_form" ng-repeat="form in FormItems">
                            <h4 class="ss_form_title">{{form.head}}</h4>

                            <ul class="ss_form_box clearfix">
                                <li class="ss_form_item" ng-repeat="item in form.items" ng-class="NeedChangeElement(item.type,'notes')?'ss_form_item_line':''">
                                    <label class="ss_form_input_title">{{item.label}}</label>
                                    <input class="ss_form_input" tempt-type="{{GetType(item.type)}}{{'Quotation'}}" tempt-ng-model="{{GenerateModel(form.head,item.label)}}" ng-if="(!item.type)||(!NeedChangeElement(item.type))">
                                    <select class="ss_form_input" tempt-ng-model="{{GenerateModel(form.head,item.label)}}" ng-if="NeedChangeElement(item.type,'select')">
                                        <option></option>
                                        <option ng-repeat="option in item.options">{{option}}</option>
                                    </select>
                                    <tempt-pt-file file-bble="{{BBLEModel}}" file-id="{{GetUniqueId(form.head,item.label)}}" file-model="{{GenerateModel(form.head,item.label)}}" ng-if="NeedChangeElement(item.type,'file')"></tempt-pt-file>
                                    <textarea class="edit_text_area text_area_ss_form" model="{{GenerateModel(form.head,item.label)}}" ng-if="NeedChangeElement(item.type,'notes')"></textarea>
                                    <input type="text" class="ss_form_input" tempt-ng-model="{{GenerateModel(form.head,item.label)}}" tempt-ng-change="{{GenerateModel(form.head,item.label)}}Id=null" tempt-typeahead="contact.Name for contact in ptContactServices.getContacts($viewValue)" tempt-typeahead-on-select="{{GenerateModel(form.head,item.label)}}Id=$item.ContactId" tempt-bind-id="{{GenerateModel(form.head,item.label)}}Id" ng-if="NeedChangeElement(item.type,'contact')">
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    BBLE model:
                    <input type="text" class="form-control" ng-model="BBLEModel" />
                    <button type="button" ng-click="GetReslut()">GetReslut</button>
                    <textarea ng-model="Resluts"></textarea>
                </div>


            </div>
        </div>


    </div>

    <script>
        var portalApp = angular.module('PortalApp');

        portalApp.controller('FormCtrl', function ($scope, $http, $element, $timeout, ptContactServices, ptCom) {
            $scope.qniueId = 0
            $scope.ptContactServices = ptContactServices;
            $scope.FormItems = [
                {
                    head: 'Documents Received On',
                    items: [
                        { label: 'Documents Received' },

                        { label: 'Date Recorded select', type: 'select', options: ['Yes', 'No'] },
                        { label: 'Deed', type: 'file' },
                        { label: 'Deed Acris Document', type: 'file' },
                        { label: 'Memorandum of Contract', type: 'file' },
                        { label: 'Memorandum of Contract Acris Docs', type: 'file' },
                        { label: 'Contract of Sales', type: 'file' },
                        { label: 'Notes', type: 'notes' },

                        { label: 'Are Documents Notarized', type: 'select', options: ['Yes', 'No'] },
                    ]
                },
            {
                head: 'Record Documents',
                items: [
                    { label: 'Deed', type: 'select', options: ['Yes', 'No'] },
                    { label: 'Memo', type: 'select', options: ['Yes', 'No'] },
                    { label: 'Requested On', type: 'date' },
                    { label: 'Requested By', type: 'contact' },
                    { label: 'Date of Submission', type: 'date' },
                    { label: 'Transaction ID' },
                    { label: 'Rejected', type: 'select', options: ['Yes', 'No'] },
                    { label: 'Rejected notes', type: 'notes' },

                    { label: 'Recorded', type: 'select', options: ['Yes', 'No'] },
                    { label: 'Date Recorded ', type: 'date' },
                    { label: 'Recorded File', type: 'file' },
                ]
            }

            ];
            //$scope.FormItems = [];
            $scope.GenerateModel = function () {
                var model = Array.prototype.slice.call(arguments).filter(function (o) { return o }).join(".").replace(/ /gi, "")
                return model
            }
            $scope.GetType = function (type) {
                var types = [{ type: 'date', model: 'ss-date' }, { type: 'money', model: 'money-mask' }, { type: 'phone', model: 'mask="(999) 999-9999"' }]
                var mType = types.filter(function (o) { return o.type == type })[0];
                return mType ? mType.model : '';
            }

            $scope.GetUniqueId = function () {
                var id = Array.prototype.slice.call(arguments).filter(function (o) { return o }).join("_").replace(/ /gi, "")
                //$scope.qniueId++;
                id += $scope.qniueId;//Math.floor((Math.random() * 1000) + 1);
                return id;
            }
            $scope.NeedChangeElement = function (type, ElemType) {
                var NeedChangeS = ["select", "contact", 'file', 'notes'];

                var iType = NeedChangeS.filter(function (o) { return o == type })[0];
                return ElemType ? iType == ElemType : iType
            }

            $scope.GetReslut = function () {

                var t = $('#template').clone();
                t.not(':visible').remove();
                var html = t.html();
                html = html.replace(/-->/g, '-->\n');
                html = html.replace(/<!--*.*-->/g, '');
                html = html.replace(/ng-scope/g, '')
                html = html.replace(/ng-if=.*\"/g, '')
                html = html.replace(/ng-repeat.*\"/g, '')
                html = html.replace(/ng-binding/g, '')

                html = html.replace(/tempt-type=\"/g, '')
                html = html.replace(/Quotation\"/g, '')

                html = html.replace(/tt-file/g, 'pt-file')
                html = html.replace(/file-ng-model/g, 'file-model');
                html = html.replace(/\n/g, '');
                html = html.replace(/tempt-/g, '');

                $scope.Resluts = html;
            }
        });

    </script>
</asp:Content>