﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="NGShortSalePropertyTab.ascx.vb" Inherits="IntranetPortal.NGShortSalePropertyTab" %>
<%@ Import Namespace="IntranetPortal" %>

<div>
    <h4 class="ss_form_title">Property Address</h4>
    <div class="ss_border">
        <ul class="ss_form_box clearfix">
            <li class="ss_form_item">
                <label class="ss_form_input_title">Block/Lot</label>
                <input class="ss_form_input" readonly="readonly" ng-value="SsCase.LeadsInfo.Block ?SsCase.LeadsInfo.Block +'/'+SsCase.LeadsInfo.Lot:''">
            </li>
            <li class="ss_form_item" style="display: none">
                <label class="ss_form_input_title">BBLE</label>
                <input class="ss_form_input" ng-model="SsCase.LeadsInfo.BBLE">
            </li>
            <%--
            <li class="ss_form_item" style="visibility: hidden">
                <label class="ss_form_input_title">BBLE</label>
                <input class="ss_form_input" readonly="readonly" ng-model="SsCase.LeadsInfo.BBLE">
            </li>
            <li class="ss_form_item" style="visibility: hidden">
                <label class="ss_form_input_title">BBLE</label>
                <input class="ss_form_input" ng-model="SsCase.LeadsInfo.BBLE">
            </li>
            --%>
            <li class="ss_form_item" style="width: 66.6%">
                <label class="ss_form_input_tflitle">Address</label>
                <input class="ss_form_input" readonly="readonly" ng-model="SsCase.LeadsInfo.PropertyAddress" style="width: 93.5%;">
            </li>
        </ul>
    </div>
</div>

<div class="ss_form" id="home_breakdown_table_new">
    <h4 class="ss_form_title">Home Breakdown <i class="fa fa-plus-circle icon_btn color_blue tooltip-examples " ng-click="NGAddArraryItem(SsCase.PropertyInfo.PropFloors,'SsCase.PropertyInfo.PropFloors', true)" title="Add"></i>
    </h4>
    <table class="table table-striped table-bordered table-responsive">
        <tr>
            <th>Unit</th>
            <th>Room</th>
            <th>Occupancy</th>
            <th>Access</th>
            <th></th>
        </tr>
        <tr class="icon_btn" ng-repeat="floor in SsCase.PropertyInfo.PropFloors" id="floor{{$index}}">
            <td ng-click="setVisiblePopup(SsCase.PropertyInfo.PropFloors[$index], true)">{{$index+1}}</td>
            <td class="col-sm-3" ng-click="setVisiblePopup(SsCase.PropertyInfo.PropFloors[$index], true)">
                <div class="content">
                    <div class="row" style="margin: 0px">
                        <div class="col-sm-12" style="padding: 0px">
                            <span><b>Description:</b> {{floor.Description}}</span>
                        </div>
                        <div class="col-sm-12" style="padding: 0px">
                            <span><b>Bedroom:</b> {{floor.Bedroom}}</span>
                        </div>
                        <div class="col-sm-12" style="padding: 0px">
                            <span><b>Bathroom:</b> {{floor.Bathroom}}</span>
                        </div>
                        <div class="col-sm-12" style="padding: 0px">
                            <span><b>Living Room:</b> {{floor.Livingroom}}</span>
                        </div>
                        <div class="col-sm-12" style="padding: 0px">
                            <span><b>Kitchen:</b> {{floor.Kitchen}}</span>
                        </div>
                        <div class="col-sm-12" style="padding: 0px">
                            <span><b>Dining Room:</b> {{floor.Diningroom}}</span>
                        </div>
                    </div>
                </div>
            </td>
            <td class="col-sm-4" ng-click="setVisiblePopup(SsCase.PropertyInfo.PropFloors[$index], true)">
                <div class="content">
                    <div class="row" style="margin: 0px">
                        <div class="col-sm-12" style="padding: 0px">
                            <span><b>Occupancy:</b> {{floor.Occupied}}</span>
                        </div>
                        <div ng-repeat="occupant in floor.Occupants" ng-show="floor.Occupied  == 'Seller' || floor.Occupied  == 'Tenants (Coop)' || floor.Occupied == 'Tenants (Non-Coop)'">
                            <div class="col-sm-12" style="padding: 0px">
                                <span><b>Name:</b> {{occupant.Name}}</span>
                            </div>
                            <div class="col-sm-12" style="padding: 0px">
                                <span><b>Phone:</b> {{occupant.Phone}}</span>
                            </div>
                            <hr />
                        </div>
                    </div>
                </div>
            </td>
            <td class="col-sm-4" ng-click="setVisiblePopup(SsCase.PropertyInfo.PropFloors[$index], true)">
                <div class="content">
                    <div class="row" style="margin: 0px">
                        <div class="col-sm-12" style="padding: 0px">
                            <span><b>Access:</b> {{floor.Access}}</span>
                        </div>
                        <div class="col-sm-12" style="padding: 0px">
                            <span><b>Lockbox:</b> {{floor.LockBox}}</span>
                        </div>
                        <div class="col-sm-12" style="padding: 0px">
                            <span><b>LockupDate:</b> {{floor.LockupDate |date: 'M/d/yyyy'}}</span>
                        </div>
                        <div class="col-sm-12" style="padding: 0px">
                            <span><b>LockedBy:</b> {{floor.LockedBy}}</span>
                        </div>
                        <div class="col-sm-12" style="padding: 0px">
                            <span><b>LastChecked:</b> {{floor.LastChecked |date: 'M/d/yyyy'}}</span>
                        </div>
                    </div>
                </div>
            </td>
            <td><i class="fa fa-times icon_btn tooltip-examples text-danger" ng-click="NGremoveArrayItem(SsCase.PropertyInfo.PropFloors, $index)" title="Delete"></i>
                <div dx-popup="{    
                                height: 750,
                                width: 600, 
                                title: 'Unit '+ ($index+1),
                                dragEnabled: true,
                                showCloseButton: true,
                                shading: false,
                                bindingOptions:{ visible: 'SsCase.PropertyInfo.PropFloors['+$index+'].visiblePopup' },
                                scrolling: {mode: 'virtual' },
                            }">
                    <div data-options="dxTemplate:{ name: 'content' }">
                        <div style="height: 88%; padding: 0 5px; overflow-y: auto; overflow-x: hidden">
                            <div class="row">
                                <div class="col-sm-6">
                                    <label>Description</label>
                                    <input class="form-control" ng-model="floor.Description" />
                                </div>
                                <div class="col-sm-6">
                                    <label>Bedroom</label>
                                    <input class="form-control" ng-model="floor.Bedroom" />
                                </div>
                                <div class="col-sm-6">
                                    <label>Bathroom</label>
                                    <input class="form-control" ng-model="floor.Bathroom" />
                                </div>
                                <div class="col-sm-6">
                                    <label>Livingroom</label>
                                    <input class="form-control" ng-model="floor.Livingroom" />
                                </div>
                                <div class="col-sm-6">
                                    <label>Kitchen</label>
                                    <input class="form-control" ng-model="floor.Kitchen" />
                                </div>
                                <div class="col-sm-6">
                                    <label>Diningroom</label>
                                    <input class="form-control" ng-model="floor.Diningroom" />
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-sm-12">
                                    <label>Occupancy&nbsp<i class="fa fa-plus-circle icon_btn tooltip-examples text-primary" ng-show="floor.Occupied  == 'Seller' || floor.Occupied  == 'Tenants (Coop)' || floor.Occupied == 'Tenants (Non-Coop)'" ng-click="NGAddArraryItem(SsCase.PropertyInfo.PropFloors[$index].Occupants,'SsCase.PropertyInfo.PropFloors['+$index+'].Occupants')" title="Add"></i></label>
                                    <select class="form-control" ng-model="floor.Occupied">
                                        <option>Vacant</option>
                                        <option>Seller</option>
                                        <option>Tenants (Coop)</option>
                                        <option>Tenants (Non-Coop)</option>
                                    </select>
                                </div>
                                <div ng-repeat="occupant in floor.Occupants" ng-show="floor.Occupied  == 'Seller' || floor.Occupied  == 'Tenants (Coop)' || floor.Occupied == 'Tenants (Non-Coop)'">
                                    <div class="col-sm-6">
                                        <label>Name</label>
                                        <input class="form-control" ng-model="occupant.Name" />
                                    </div>
                                    <div class="col-sm-5">
                                        <label>Phone</label>
                                        <input class="form-control" ng-model="occupant.Phone" />
                                    </div>
                                    <div class="col-sm-1">
                                        <label>&nbsp</label>
                                        <div class="text-right">
                                            <i class="fa fa-times icon_btn tooltip-examples text-danger" ng-click="NGremoveArrayItem(SsCase.PropertyInfo.PropFloors[$parent.$index].Occupants, $index)" title="Delete"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-sm-6">
                                    <label>Access</label>
                                    <input class="form-control" ng-model="floor.Access" />
                                </div>
                                <div class="col-sm-6">
                                    <label>Lockbox</label>
                                    <input class="form-control" ng-model="floor.LockBox" />
                                </div>
                                <div class="col-sm-6">
                                    <label>LockupDate</label>
                                    <input class="form-control" pt-date ng-model="floor.LockupDate" />
                                </div>
                                <div class="col-sm-6">
                                    <label>LockedBy</label>
                                    <input class="form-control" ng-model="floor.LockedBy" />
                                </div>
                                <div class="col-sm-6">
                                    <label>LastChecked</label>
                                    <input class="form-control" pt-date ng-model="floor.LastChecked" />
                                </div>
                            </div>
                        </div>
                        <hr />
                        <button type="button" class="btn btn-primary pull-right" ng-click="setVisiblePopup(SsCase.PropertyInfo.PropFloors[$index], false)">Save</button>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>



<div class="ss_form">
    <h4 class="ss_form_title">Building Info</h4>
    <div class="ss_border">
        <ul class="ss_form_box clearfix">

            <li class="ss_form_item">
                <label class="ss_form_input_title">C/O Class</label>
                <input class="ss_form_input" ng-model="SsCase.PropertyInfo.COClass">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Total Units</label>
                <input class="ss_form_input" ng-model="SsCase.PropertyInfo.NumOfUnit">
            </li>
            <li class="ss_form_item">&nbsp;
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Tax Class</label>
                <input class="ss_form_input" readonly="readonly" ng-model="SsCase.LeadsInfo.PropertyClass">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Total Units</label>
                <input class="ss_form_input" ng-model="SsCase.LeadsInfo.UnitNum" readonly="readonly">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Year Built</label>
                <input class="ss_form_input" ng-model="SsCase.LeadsInfo.YearBuilt" readonly="readonly">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Lot Size</label>
                <input class="ss_form_input" ng-model="SsCase.LeadsInfo.LotDem" readonly="readonly">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Building Size</label>
                <input class="ss_form_input" ng-model="SsCase.LeadsInfo.BuildingDem" readonly="readonly">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Building Stories</label>
                <input class="ss_form_input" ng-model="SsCase.LeadsInfo.NumFloors" readonly="readonly">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Calculated Sqft</label>
                <input class="ss_form_input" ng-model="SsCase.LeadsInfo.CalculatedSqft" readonly="readonly">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">NYC Sqft</label>
                <input class="ss_form_input" ng-model="SsCase.LeadsInfo.NYCSqft" readonly="readonly">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Unbuilt Sqft</label>
                <input class="ss_form_input" ng-model="SsCase.LeadsInfo.UnbuiltSqft" readonly="readonly" pt-number-mask>
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Zoning Code</label>
                <input class="ss_form_input" ng-model="SsCase.LeadsInfo.Zoning" readonly="readonly">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Max FAR</label>
                <input class="ss_form_input" ng-model="SsCase.LeadsInfo.MaxFar" readonly="readonly">
            </li>
            <li class="ss_form_item">
                <label class="ss_form_input_title">Actual FAR</label>
                <input class="ss_form_input" ng-model="SsCase.LeadsInfo.ActualFar" readonly="readonly">
            </li>

        </ul>
    </div>
</div>


<%--<script src="/bower_components/editable-table/mindmup-editabletable.js"></script>--%>
<script>
    function onRefreashDone() {
        //$("#home_breakdown_table").editableTableWidget();
        $(".ss_form_input, .input_with_check").not(".ss_allow_eidt").prop("disabled", true);
        initToolTips();
        format_input();
    }
</script>
