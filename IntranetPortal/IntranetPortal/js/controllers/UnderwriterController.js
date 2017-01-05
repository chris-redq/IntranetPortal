﻿/**
 * Author: Shaopeng Zhang
 * Date: 2016/11/02
 * Description: General Controller for underwriting
 * Update: 
 *          --- 2016/11/02
 *              1. Add Enable Editing Function to unlock datainput area.
 */
angular.module("PortalApp").controller("UnderwriterController",
[
    "$scope", "ptCom", "ptUnderwriting", "$location", "DocSearch", "$state",
    function($scope, ptCom, ptUnderwriting, $location, DocSearch, $state) {

        $scope.data = {};
        $scope.archive = {};
        $scope.currentDataCopy = {};
        $scope.isProtectedView = true;

        $scope.init = function(bble) {
            //ptCom.startLoading()
            //$scope.feedData();
            $scope.load(bble);
            $scope.loadArchivedList(bble);
        };
        $scope.load = function(bble) {
            $scope.data = ptUnderwriting.load(bble);
            if ($scope.data.$promise) {
                $scope.data.$promise.then(function() {
                    $scope.calculate();
                });
            }
        };
        $scope.save = function() {
            ptCom.confirm("Are you going to Save?",
                function(response) {
                    if (response) {
                        ptUnderwriting.save($scope.data).then(function(d) {
                                //debugger;
                                if (d.data) {
                                    $scope.data = d.data;
                                }
                                ptCom.alert("Save Successfully.");
                            },
                            function() {
                                ptCom.alert("Failed to save.");
                            });
                    }
                });
        };

        // Snapshot current values of forms, and sava copy in database for future analysis
        $scope.archiveFunc = function() {
            ptCom.prompt("Please give a name to this archive.",
                function(msg) {
                    //debugger;
                    if (msg != null) {
                        ptUnderwriting.archive($scope.data, msg).then(function(d) {
                                alert("Archive succesful.");
                            },
                            function() {
                                alert("Sorry. Some error.");
                            });
                    }

                });
        };

        // Load all achived version in databases
        $scope.loadArchivedList = function(bble) {
            if (bble) {
                ptUnderwriting.loadArchivedList(bble).then(function(d) {
                    $scope.archivedList = d.data;
                });
            }
        };

        // Load a single archived entry in database
        $scope.loadArchived = function(archive) {
            //debugger;
            if (archive.Id) {
                ptUnderwriting.loadArchived(archive.Id).then(function(d) {
                        if (d.data) {
                            //debugger;
                            angular.copy($scope.data, $scope.currentDataCopy);
                            ptCom.assignReference($scope.data, d.data, [], ["Id"]);
                            $scope.archive = archive;
                            $scope.archive.isLoaded = true;
                            ptCom.alert("Load successfully");

                        }
                    },
                    function(d) {
                        ptCom.alert("Failed to load.");
                    });
            }

        };

        // Restore from achived version to current version
        $scope.restoreCurrent = function() {
            if ($scope.currentDataCopy) {
                ptCom.assignReference($scope.data, $scope.currentDataCopy);
                $scope.archive.isLoaded = false;
                ptCom.alert("Restore to current version.");
            }
        };

        // Core function to apply predefined rule, and update model values.
        $scope.calculate = function() {
            $scope.$applyAsync(function() {
                ptUnderwriting.calculator.calculate($scope.data);
            });
        };

        $scope.enableEditing = function() {
            $scope.$broadcast("pt-editable-div-unlock");
            $scope.isProtectedView = false;
        };

        $scope.$watch(function() {
                return $state.$current.name;
            },
            function(newVal, oldVal) {
                if (newVal === "underwriter.datainput") {
                    if ($scope.isProtectedView == false) {
                        $scope.enableEditing();
                    }
                }
            });


        // Init controller;
        $scope.BBLE = ptCom.getGlobal("BBLE") || "";
        $scope.viewmode = ptCom.getGlobal("viewmode") || 0;
        $scope.init($scope.BBLE);

        // A predefined model to validate with excel data.
        $scope.feedData = function () {
            $scope.data.PropertyInfo.TaxClass = "A0",
            $scope.data.PropertyInfo.ActualNumOfUnits = 1;
            $scope.data.PropertyInfo.SellerOccupied = true;
            $scope.data.PropertyInfo.PropertyTaxYear = 4297.0;
            $scope.data.DealCosts.HOI = 20000.0;
            $scope.data.DealCosts.AgentCommission = 2500;
            $scope.data.RehabInfo.AverageLowValue = 205166;
            $scope.data.RehabInfo.RenovatedValue = 510000;
            $scope.data.RehabInfo.RepairBid = 75000;
            $scope.data.RehabInfo.DealTimeMonths = 6;

            $scope.data.LienInfo.FirstMortgage = 340000;
            $scope.data.LienInfo.SecondMortgage = 284000;
            $scope.data.LienCosts.PropertyTaxes = 9113.32;
            $scope.data.LienCosts.WaterCharges = 1101.33;
            $scope.data.LienCosts.PersonalJudgements = 14892.09;
            $scope.update();
        };
    }
]);