var portalApp = angular.module('PortalApp',
    ['ngResource', 'ngSanitize', 'ngAnimate', 'dx', 'ngMask', 'ui.bootstrap', 'ui.select', 'ui.layout', 'ngRoute', 'firebase', 'ui.router']);

angular.module('PortalApp')
    .controller('MainCtrl', ['$rootScope', '$uibModal', '$timeout', '$state', function ($rootScope, $uibModal, $timeout, $state) {
        $rootScope.globaldata = {};
        $rootScope.AlertModal = null;
        $rootScope.ConfirmModal = null;
        $rootScope.loadingCover = document.getElementById('LodingCover');
        $rootScope.panelLoading = false;
        $rootScope.isPromptModalArea = false;
        $rootScope.loadPanelPosition = (function () {
            var dataPanelDiv = document.getElementById('dataPanelDiv');
            if (dataPanelDiv != null) {
                return { of: '#dataPanelDiv' }
            } else {
                return { of: 'body' }
            }
        })();
        $rootScope.$state = $state;
        $rootScope.alert = function (message) {
            $rootScope.alertMessage = message ? message : '';
            $rootScope.AlertModal = $uibModal.open({
                templateUrl: 'AlertModal',
            });
        }
        $rootScope.alertOK = function () {
            $rootScope.AlertModal.close();
        }
        $rootScope.confirm = function (message, confrimFunc) {
            $rootScope.confirmMessage = message ? message : '';
            $rootScope.ConfirmModal = $uibModal.open({
                templateUrl: 'ConfirmModal'
            });
            $rootScope.ConfirmModal.confrimFunc = confrimFunc;
            return $rootScope.ConfirmModal.result;
        }
        $rootScope.confirmYes = function () {
            $rootScope.ConfirmModal.close(true);
            if ($rootScope.ConfirmModal.confrimFunc) {
                $rootScope.ConfirmModal.confrimFunc(true);
            }

        }
        $rootScope.confirmNo = function () {
            $rootScope.ConfirmModal.close(false);
            if ($rootScope.ConfirmModal.confrimFunc) {
                $rootScope.ConfirmModal.confrimFunc(false);
            }
        }
        $rootScope.prompt = function (message, callback,  showArea){
            $rootScope.promptMessage = message ? message : '';
            $rootScope.promptModalTxt = '';
            $rootScope.isPromptModalArea = showArea || false;
            $rootScope.promptModal = $uibModal.open({
                templateUrl: 'PromptModal'
            });
            $rootScope.promptModal.promptFunc = callback;

        }
        $rootScope.promptYes = function () {
            $rootScope.promptModal.close($rootScope.promptModalTxt);
            if ($rootScope.promptModal.promptFunc) {
                $rootScope.promptModal.promptFunc($("#promptModalTxt").val());
            }

        }
        $rootScope.promptNo = function () {
            $rootScope.promptModal.close(false);
            if ($rootScope.promptModal.promptFunc) {
                $rootScope.promptModal.promptFunc(null)
            }
        }
        $rootScope.showLoading = function (divId) {
            $($rootScope.loadingCover).show();
        }
        $rootScope.hideLoading = function (divId) {
            $($rootScope.loadingCover).hide();
        }
        $rootScope.toggleLoading = function () {
            $timeout(function () {
                $rootScope.panelLoading = !$scope.panelLoading;
            })
        }
        $rootScope.startLoading = function () {
            $timeout(function () {
                $rootScope.panelLoading = true;
            })
        }
        $rootScope.stopLoading = function () {
            $timeout(function () {
                $rootScope.panelLoading = false;
            });
        }
    }]);

(function () {
    var ITEM_ID = 'itemId';

    function portalRouteProvider($routeProvider) {


        this.$get = angular.noop;
        this.ITEM_ID = ITEM_ID;
        this.routesFor = function (resourceName, urlPrefix, routePrefix) {
            var baseUrl = resourceName.toLowerCase();            
            var baseRoute = '/' + resourceName.toLowerCase();
            routePrefix = routePrefix || urlPrefix;

            if (angular.isString(urlPrefix) && urlPrefix !== '') {
                baseUrl = urlPrefix + '/' + baseUrl;
            }

            if (routePrefix !== null && routePrefix !== undefined && routePrefix !== '') {
                baseRoute = '/' + routePrefix + baseRoute;
            }

            var templateUrl = function (operation) {
                return '/js/Views/' + resourceName.toLowerCase() + '/' + resourceName.toLowerCase() + '-' + operation.toLowerCase() + '.tpl.html';
            };
            var controllerName = function (operation) {
                return resourceName + operation + 'Ctrl';
            };

            var routeBuilder = {
                whenList: function (resolveFns) {
                    routeBuilder.when(baseRoute, {
                        templateUrl: templateUrl('List'),
                        controller: controllerName('List'),
                        resolve: resolveFns
                    });
                    return routeBuilder;
                },
                whenNew: function (resolveFns) {
                    routeBuilder.when(baseRoute + '/new', {
                        templateUrl: templateUrl('Edit'),
                        controller: controllerName('Edit'),
                        resolve: resolveFns
                    });
                    return routeBuilder;
                },
                whenEdit: function (resolveFns) {
                    routeBuilder.when(baseRoute + '/:' + ITEM_ID, {
                        templateUrl: templateUrl('Edit'),
                        controller: controllerName('Edit'),
                        resolve: resolveFns
                    });
                    return routeBuilder;
                },
                whenOther: function (resolveFns, name, suffixUrl) {
                    var _url = suffixUrl ? suffixUrl : '';
                    routeBuilder.when(baseRoute +'/'+ name.toLowerCase() +'/'+ _url, {
                        templateUrl: templateUrl(name),
                        controller: controllerName(name),
                        resolve: resolveFns
                    });
                    return routeBuilder;
                },
                whenView: function (resolveFns) {
                    routeBuilder.when(baseRoute + '/view/:' + ITEM_ID, {
                        templateUrl: templateUrl('View'),
                        controller: controllerName('View'),
                        resolve: resolveFns
                    });
                    return routeBuilder;
                },
                when: function (path, route) {
                    $routeProvider.when(path, route);
                    return routeBuilder;
                },
                otherwise: function (params) {                    
                    $routeProvider.otherwise(params);
                    return routeBuilder;
                },
                $routeProvider: $routeProvider
            };
            return routeBuilder;
        };
    }
    portalRouteProvider.$inject = ['$routeProvider'];

    portalRouteProvider.ITEM_ID = ITEM_ID;
    angular.module('PortalApp').provider('portalRoute', portalRouteProvider);
})();


(function () {
    var ITEM_ID = 'itemId';

    function portalUIRouteProvider($stateProvider) {


        this.super = $stateProvider;
        this.$get = angular.noop;
        this.ITEM_ID = ITEM_ID;


        this.statesFor = function (resourceName) {
            if (!resourceName) {
                console.error("resourceName must be defined in $stateProvider");
            }
            var stateTemplateUrl = function (statePath) {
                return '/js/Views/' + resourceName.toLowerCase() + '/' + templateFile(statePath) + '.tpl.html';
            };

            var stateUrl = function(statePath)
            {

                return  '/' +( !statePath ? resourceName : statePath.replace(".","/") );
            }
            var stateControllerName = function (statePath) {
                return _.camelCase(resourceName + ' ' + (statePath || '') ) + 'Ctrl';
            };

            var templateFile = function (statePath) {

                if (!statePath) {
                    return 'index'
                }

                return _.kebabCase(statePath.toLowerCase());
            }
            var stateBuilder = {


                state: function (statePath, resolveFns) {
                    $stateProvider.state(resourceName, {
                        url: stateUrl(statePath),
                        templateUrl: stateTemplateUrl(statePath),
                        controller: stateControllerName(statePath),
                        resolve: resolveFns
                    });
                    return stateBuilder;
                },
                otherwise: function (params) {
                    $stateProvider.otherwise(params);
                    return stateBuilder;
                },
                $stateProvider: $stateProvider
            };
            stateBuilder.state(null, null);
            return stateBuilder;

        };
    }


    portalUIRouteProvider.$inject = ['$stateProvider'];

    portalUIRouteProvider.ITEM_ID = ITEM_ID;
    angular.module('PortalApp').provider('portalUIRoute', portalUIRouteProvider);
})();
angular.module('PortalApp').factory('AssignCorp', function (ptBaseResource, CorpEntity, $http, DivError) {
    var _class = function ()
    {
        this.onAssignSucceed = null;
        this.BBLE = null;
    }

        _class.prototype.test = function()
    {
        this.text = "1234555";
    }

    _class.prototype.selectTeamChange = function ()
    {
        var team = this.Name;
        this.Signer = null;
        me = this;
        $http.get('/api/CorporationEntities/CorpSigners?team=' + team).success(function (signers) {
            me.signers = signers
        });
    }

    _class.prototype.assginCorpClick = function () {

        var _assignCrop = this;

        var eMessages = new DivError('assignBtnForm').getMessage();
        if (_.any(eMessages)) {
            AngularRoot.alert(eMessages.join(' <br />'));
            return false;
        }

        var assignApi = "/api/CorporationEntities/AvailableCorpBySigner?team=" + _assignCrop.Name + "&signer=" + _assignCrop.Signer;

        var confirmMsg = ' THIS PROCESS CANNOT BE REVERSED. Please confirm - The team is ' + _assignCrop.Name + ', and servicer is not Wells Fargo.';

        if (_assignCrop.isWellsFargo) {

            confirmMsg = ' THIS PROCESS CANNOT BE REVERSED. Please confirm - The team is ' + _assignCrop.Name + ', and Wells Fargo signer is ' + _assignCrop.Signer + '';
        }

                $http.get(assignApi).success(function (data) {

            AngularRoot.confirm(confirmMsg).then(function (r) {
                if (r) {
                    _assignCrop.assignCorpSuccessed(data);
                }
            });
        });
    }

    _class.prototype.assignCorpSuccessed = function (data) {
        var _assignCrop = this;
        $http.post('/api/CorporationEntities/Assign?bble=' + _assignCrop.BBLE, JSON.stringify(data)).success(function () {
            _assignCrop.Crop = data.CorpName;
            _assignCrop.CropData = data;

            if (_assignCrop.onAssignSucceed) {
                _assignCrop.onAssignSucceed(data);               
            } else {
                console.error("AssignCorp: onAssignSucceed not implement.")
            }
        });
    }

    _class.prototype.newOfferId = 0    
    return _class;
});
angular.module('PortalApp').factory('AuditLog', function (ptBaseResource) {
    var auditLog = ptBaseResource('AuditLog', 'AuditId', null, {
        load: {
            method: "GET",
            url: '/api/auditlog/:TableName/:RecordId'
           , params: {
               RecordId: '@RecordId',
               TableName: '@TableName'

           },
            isArray: true,
            options: { noError: true }
        },
    });

    return auditLog;

});
angular.module('PortalApp').factory('ptBaseResource', function ($resource) {
    var BaseUri = '/api';

    var PtBaseResource = function (apiName, key, paramDefaults, actions) {
        var uri = BaseUri + '/' + apiName + '/:' + key;
        var primaryKey = {};
        primaryKey[key] = '@' + key;

        var _actions = {
            'update': { method: 'PUT' }
        };

        angular.extend(primaryKey, paramDefaults)
        angular.extend(_actions, actions);

        var Resource = $resource(uri, primaryKey, _actions);

        Resource.all = function () { }
        Resource.CType = function (obj, Class) {

            if (obj == null || obj == undefined) {
                return null;
            }

            if (obj instanceof Class) {
                return obj;
            }
            var _new = new Class();
            angular.extend(_new, obj);
            angular.extend(obj, _new);
            return _new;
        }

        Resource.prototype.hasId = function () {
            return this[key] != null && this[key] != 0;
        }
        Resource.prototype.errorMsg = [];

        Resource.prototype.clearErrorMsg = function () {
            this.errorMsg = []
        }

        Resource.prototype.getErrorMsg = function () {
            return this.errorMsg;
        }
        Resource.prototype.getErrorMsgStr = function () {
            return this.errorMsg.join('<br />');
        }
        Resource.prototype.hasErrorMsg = function () {
            return this.errorMsg && this.errorMsg.length > 0;
        }

        Resource.prototype.pushErrorMsg = function (msg) {
            if (!this.errorMsg) { this.errorMsg = [] };
            this.errorMsg.push(msg);
        }

        Resource.prototype.$put = function () {}

        Resource.prototype.$cType = function (_this, Class) {
            Resource.CType(this, Class);
        }
        return Resource;

    }

    return PtBaseResource;
});
angular.module('PortalApp').factory('BusinessCheck', function (ptBaseResource) {
    var businessCheck = ptBaseResource('BusinessCheck', 'Id', null, null);
    businessCheck.CheckStatus = {
        Active : 0,
        Canceled : 1,
        Completed : 2
    }
    businessCheck.prototype.CheckStatus = {};
    angular.extend(businessCheck.prototype.CheckStatus, businessCheck.CheckStatus);

    businessCheck.prototype.isVoid = function ()
    {
        return this.Status == this.CheckStatus.Canceled;
    }
    return businessCheck;

});
angular.module('PortalApp').factory('CheckRequest', function (ptBaseResource, BusinessCheck) {
    var checkRequest =  ptBaseResource("CheckRequest",'Id',null,null);

        checkRequest.prototype.getTotalAmount = function ()
    {
        if(this.Checks)
        {
            var _checks = _.map(this.Checks, function (o) { return checkRequest.CType(o, BusinessCheck) });

                        return _.sum(_.filter(_checks, function (o) { return !o.isVoid() }), 'Amount');
        }

        return 0;
    }

    checkRequest.prototype.Type = 'Short Sale';
    checkRequest.prototype.Checks = [];

    return checkRequest;

});
angular.module('PortalApp').factory('CorpEntity', function (ptBaseResource, LeadsInfo) {

    var corpEntity = ptBaseResource('CorporationEntities', 'EntityId',null,
    { assign: { url: '/api/CorporationEntities/:EntityId/BBLE', method: 'Post' } });


    corpEntity.prototype.assignDateNow = function()
    {
        if(this.AssignOn)
        {
            var now = Date.now();
            var assignOn = Date.parse(this.AssignOn);
            var times = now - assignOn;

                        return get_time_diff(assignOn);
        }
    }

        function get_time_diff(datetime) {
        var datetime = typeof datetime !== 'undefined' ? datetime : "2014-01-01 01:02:03.123456";

        var datetime = new Date(datetime).getTime();
        var now = new Date().getTime();

        if (isNaN(datetime)) {
            return "";
        }

        console.log(datetime + " " + now);

        if (datetime < now) {
            var milisec_diff = now - datetime;
        } else {
            var milisec_diff = datetime - now;
        }

        var days = Math.floor(milisec_diff / 1000 / 60 / (60 * 24)) + 1;

        var date_diff = new Date(milisec_diff);

        return days  + " Days " 
    }

    return corpEntity;
});

angular.module('PortalApp')
    .factory('DivError', function () {
        var _class = function (id) {
            this.id = id;
        }
        _class.prototype.getMessage = function () {
            var eMessages = [];
            $('#' + this.id + ' ul:not(.form_ignore) .ss_warning:not(.form_ignore)').each(function () {
                eMessages.push($(this).attr('data-message'));
            });
            return eMessages;
        }

        _class.prototype.passValidate = function () {
            return this.getMessage().length == 0;
        }

        _class.prototype.boolValidate = function (base, boolKey) {
            if (!base)
            {
                return false;
            }
            var boolVal = base[boolKey];

            return boolVal === undefined;
        }

        _class.prototype.multipleValidated = function (base, boolKey, arraykey) {
            if (!base)
            {
                return false;
            }
            var boolVal = base[boolKey];
            var arrayVal = base[arraykey];
            var hasWarning = (boolVal === undefined) || (boolVal && arrayVal == false);
            return hasWarning;
        }


        return _class;
    });
angular.module('PortalApp')
    .factory('DocNewVersionConfig', function () {
        CONSTANT_DATE = '8/11/2016';
        var docNewVersionConfig = function()
        {
            this.date = CONSTANT_DATE;
        }
        docNewVersionConfig.getInstance = function()
        {
            return new docNewVersionConfig();
        }

                return docNewVersionConfig;
    })

angular.module('PortalApp').factory('DocSearch', function (ptBaseResource, LeadResearch, LeadsInfo, $http) {

    var docSearch = ptBaseResource('LeadInfoDocumentSearches', 'BBLE', null, {
        completed: { method: "post", url: '/api/LeadInfoDocumentSearches/:BBLE/Completed' }
    });

    docSearch.Status = {
        New: 0,
        Completed: 1
    }

    docSearch.prototype.initTeam = function () {
        var self = this
        $http.get('/Services/TeamService.svc/GetTeam?userName=' + this.CreateBy).success(function (data) {
            self.team = data;
        });
    }

    docSearch.prototype.initLeadsResearch = function () {
        var self = this;
        var data1 = null;
        if (self.LeadResearch == null) {
            self.LeadResearch = new LeadResearch();
            data1 = self.LeadResearch.initFromLeadsInfo(self.BBLE);
        } else {
            var _LeadSearch = new LeadResearch();
            angular.extend(_LeadSearch, self.LeadResearch);
            self.LeadResearch = _LeadSearch;
            if (self.LeadResearch.ownerName) {
                self.LeadResearch.getOwnerSSN(self.BBLE);
            }
            if (self.LeadResearch.initFromLeadsInfo) {
                data1 = self.LeadResearch.initFromLeadsInfo(self.BBLE);
            }
        }

        return data1;
    }
    docSearch.prototype.actionTest = function () {
        this.$update()
    }

    docSearch.prototype._leadResearch = function () {
        if (this.LeadResearch && !(this instanceof LeadResearch)) {
            angular.extend(this.LeadResearch, new LeadResearch())
        }
        return this.LeadResearch;
    }

    docSearch.prototype.markCompleted = function (bble, status, note) {

        payload = {
            bble: bble,
            status: status,
            note: note
        }

        return $http({
            method: 'POST',
            url: '/api/LeadInfoDocumentSearches/MarkCompleted',
            data: payload
        })
    };

    return docSearch;
});



angular.module('PortalApp')

    .factory('DocSearchEavesdropper', function (DocNewVersionConfig) {
        var docSearchEavesdropper = function () {

        }

        docSearchEavesdropper.prototype.setEavesdropper = function (_eavesdropper, revFunc) {
            this.eavesDropper = _eavesdropper;
            this.endorseCheckFuncs();
            this._registerCheckFuncs();
            this.endorse(revFunc);
        }

        docSearchEavesdropper.prototype.endorse = function (revFunc) {
            if (!this.eavesDropper) {
                console.error('unable to eavesdropper it not set yet');
            }

            if (typeof revFunc != 'function')
                console.error("set rev function have been set up");

            this.endorseCheckFuncs();

            this.revFunc = revFunc;
        }

        docSearchEavesdropper.prototype.start2Eaves = function () {
            this.endorseCheckFuncs();

            if (this.endorseCheckDate(DocNewVersionConfig.getInstance().date)
                || this.endorseCheckVersion()) {
                this.revFunc(true);
            } else {
                this.revFunc(false);
            }

        }

        docSearchEavesdropper.prototype.endorseCheckFuncs = function () {
            var eaves = this.eavesDropper;

            if (typeof eaves.endorseCheckDate != 'function') {
                console.error("eavesDropper functions is not null");
            }

            if (typeof eaves.endorseCheckVersion != 'function') {
                console.error("eavesDropper functions is not null");
            }

        }
        docSearchEavesdropper.prototype._registerCheckFuncs = function () {
            var eaves = this.eavesDropper;
            this.endorseCheckFuncs();

            this.endorseCheckDate = eaves.endorseCheckDate;
            this.endorseCheckVersion = eaves.endorseCheckVersion;
        }

        docSearchEavesdropper.prototype.unendorse = function () {
            this.eavesDropper = null;
        }

        return docSearchEavesdropper;
    });
angular.module('PortalApp').factory('DxGridModel', function ($location, $routeParams) {

    var dxGridModel = function (opt, texts) {
        angular.extend(this, opt);
        if (texts) {
            this.editing = texts;
        }
        this.initFormUrl();

    }

    var EditingModel = function (texts) {
        this.editMode = "cell";
        if (texts)
        {
            this.texts = texts;
        }

            }



    dxGridModel.prototype.setNewText = function(texts)
    {
        this.editing = new EditingModel(texts);
    }
    dxGridModel.prototype.initFormUrl = function () {
        var path = '';

        path = $location.path();

        if (path.indexOf('view') >= 0) {
            console.log("in view UI all input should be disabled");
            return;
        }

        if (path.indexOf('new') >= 0 || parseInt($routeParams['itemId']) >= 0) {
            this.editing.insertEnabled = true;
            this.editing.removeEnabled = true;
            this.editing.editEnabled = true;
        }
    }

    return dxGridModel;
});

function dxModel() {


}






function dxGridColumnModel(opt) {

    _.extend(this, opt);
    if (this.dataType == 'date') {

        this.customizeText = this.customizeTextDateFunc;
    }

}
dxGridColumnModel.prototype.customizeTextDateFunc = function(e) {

    var date = e.value
    if (date) {
        return PortalUtility.FormatISODate(new Date(date));
    } 
    return ''
}

angular.module('PortalApp').factory('EmployeeModel', ['$resource','$http', function($resource,$http){

    var resource = $resource('/api/employees/:id');

    resource.getEmpNames = function () {
        var promise = $http({
            method: 'GET',
            url: '/api/employeenames'
        })

        return promise;
    }

        return resource;
}])
angular.module('PortalApp').factory('HomeOwner', function (ptBaseResource) {

    var homeOwner = ptBaseResource('Homeowner', 'BBLE');

    return homeOwner;
});

angular.module('PortalApp').factory('LeadResearch', function ($http,LeadsInfo) {

    var leadResearch = function () {

    }


    leadResearch.prototype.getOwnerSSN = function (BBLE) {
        var self = this;
        $http.post("/api/homeowner/ssn/" + BBLE, JSON.stringify(self.ownerName)).
            success(function (ssn, status, headers, config) {
                self.ownerSSN = ssn;
            });
    }

    leadResearch.prototype.initFromLeadsInfo = function(BBLE)
    {
        var self = this;

        var data1 = LeadsInfo.get({ BBLE: BBLE.trim() }, function () {
            self.ownerName = self.ownerName || data1.Owner;
            self.getOwnerSSN(BBLE);




                    });
        return data1;
    }

    return leadResearch;
});
angular.module('PortalApp').factory('LeadsInfo', function (ptBaseResource) {

    var leadsInfo = ptBaseResource('LeadsInfo', 'BBLE',null,
    { verify: { url: '/api/LeadsInfo/Verify' } });


    return leadsInfo;
});
angular.module('PortalApp').factory('NewOfferListGrid', function ($http) {
    var _class = function (data) {

        var opt = {
            dataSource: data,
            headerFilter: {
                visible: true
            },
            searchPanel: {
                visible: true,
                width: 250
            },
            paging: {
                pageSize: 10
            },
            columnAutoWidth: true,
            wordWrapEnabled: true,
            onRowPrepared: this.onRowPrepared,
            columns: [{
                dataField: 'Title',
                caption: 'Address',
                cellTemplate: this.cellTemplate
            },
                'OfferType', {
                    dataField: 'CreateBy',
                    caption: 'Submit By'
                }, {
                    dataField: 'CreateDate',
                    caption: 'Contract Date',
                    dataType: 'date',
                    sortOrder: 'desc',
                    format: 'shortDate'
                },
                {
                    caption: 'History',
                    width: 80,
                    alignment: 'center',
                    cellTemplate: function (container, options) {
                        $("<i>").attr("class", "fa fa-history")
                                .attr("style", "cursor:pointer")
                                .on('dxclick', function () {
                                    auditLog.show('PropertyOffer', options.data.OfferId);
                                    $("#divAuditLog").modal("show");
                                }).appendTo(container);
                    }
                }
            ]
        };
        angular.extend(this, opt)
    }
    _class.prototype.onRowPrepared = function (rowInfo) {
        if (rowInfo.rowType != 'data')
            return;
        rowInfo.rowElement
            .addClass('myRow');
    }
    _class.prototype.cellTemplate = function (container, options) {
        $('<a/>').addClass('dx-link-MyIdealProp')
            .text(options.value)
            .on('dxclick', function () {
                var request = options.data;

                PortalUtility.ShowPopWindow("New Offer", "/NewOffer/ShortSaleNewOffer.aspx?BBLE=" + request.BBLE);
            })
            .appendTo(container);
    }

    return _class;
});

angular.module('PortalApp').factory('PreSign', function (ptBaseResource,CheckRequest,LeadsInfo) {

    var preSign = ptBaseResource('PreSign', 'Id', null, {
        getByBBLE: {
            method: "GET",
            url: '/api/PreSign/BBLE/:BBLE',
            params: {
                BBLE: '@BBLE',
            },
            options: { noError: true }
        },
        financeList: {
            method: "GET", url: '/api/PreSign/CheckRequests', isArray: true
        }

      });

    preSign.prototype.validation = function ()
    {
        this.clearErrorMsg();
        if (!this.ExpectedDate) {
            this.pushErrorMsg("Please fill expected date !");

                   }
        if ((!this.Parties) || this.Parties.length < 1) {
            this.pushErrorMsg("Please fill at least one Party !");
        }
        this.CheckRequestData = preSign.CType(this.CheckRequestData, CheckRequest);

        if (this.NeedCheck &&  this.CheckRequestData.Checks.length < 1) {
           this.pushErrorMsg("Check Request is enabled. Please enter checks to be issued.");

                   }

        if (this.CheckRequestData && this.CheckRequestData.getTotalAmount() > this.DealAmount) {
           this.pushErrorMsg("The check's total amount must less than the deal amount, Please correct! ");           
        }

        if (!this.ApprovalFile) {
            this.pushErrorMsg("Please attach the approval file.");

        }

        return this.hasErrorMsg() == false;
    }
    preSign.prototype.BBLE = '';




    return preSign;
});

angular.module('PortalApp').factory('PropertyOffer', function (ptBaseResource, AssignCorp) {
    var propertyOffer = ptBaseResource('PropertyOffer', 'OfferId', null, {
        getByBBLE: {
            url: '/api/businessform/PropertyOffer/Tag/:BBLE',
            params: {
                BBLE: '@BBLE',
            }
        }

    });




    propertyOffer.prototype.assignOfferId = function (onAssignCorpSuccessed) {
        this.assignCrop.newOfferId = this.BusinessData.OfferId;
        this.assignCrop.BBLE = this.Tag;
        this.assignCrop.onAssignSucceed = onAssignCorpSuccessed;

           }

    propertyOffer.prototype.Type = 'Short Sale';
    propertyOffer.prototype.FormName = 'PropertyOffer';

    propertyOffer.prototype.refreshSave = function (formdata) {
        this.DataId = formdata.DataId;
        this.Tag = formdata.Tag;
        this.CreateDate = formdata.CreateDate;
        this.CreateBy = formdata.CreateBy;
    }
    propertyOffer.prototype.DealSheetMetaData = {
        ContractOrMemo: {
            Sellers: [{}],
            Buyers: [{}]
        },
        Deed: {
            Sellers: [{}]
        },
        CorrectionDeed: {
            Sellers: [{}],
            Buyers: [{}]
        }
    };   

    return propertyOffer;
});


angular.module('PortalApp').factory('QueryUrl', function ($http) {
    var _class = function () {
        var query_string = {};
        var query = window.location.search.substring(1);
        var vars = query.split("&");
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            if (typeof query_string[pair[0]] === "undefined") {
                query_string[pair[0]] = decodeURIComponent(pair[1]);
            } else if (typeof query_string[pair[0]] === "string") {
                var arr = [query_string[pair[0]], decodeURIComponent(pair[1])];
                query_string[pair[0]] = arr;
            } else {
                query_string[pair[0]].push(decodeURIComponent(pair[1]));
            }
        }
        return query_string;
    }


        return _class;
});

angular.module('PortalApp').factory('ScopeHelper', function ($http) {
    var _class = function () {


    }
    _class.getScope = function (id) {
        return angular.element(document.getElementById(id)).scope();
    }
    _class.getShortSaleScope = function () {


                return _class.getScope('ShortSaleCtrl');
    }
    _class.getLeadsSearchScope = function()
    {
        return _class.getScope('LeadTaxSearchCtrl');
    }
    return _class;
});


angular.module('PortalApp').factory('Team', function ($http) {
    var _class = function () {


    }
    _class.getTeams = function (successCall) {

        $http.get('/api/CorporationEntities/Teams')
            .success(successCall);

    }

    _class.prototype.isAvailable = function () {
        return this.Available == true;
    }
    return _class;
});
angular.module('PortalApp')
    .factory('ptUnderwriter', ['$http', 'ptBaseResource', 'DocSearch', 'LeadsInfo', function ($http, ptBaseResource, DocSearch, LeadsInfo) {

        var underwriter = ptBaseResource('underwriter', 'BBLE', null, {});

        var underwritingFactory = {
            UnderwritingModel: function () {
                this.PropertyInfo = {
                    PropertyAddress: '',
                    CurrentOwner: '',
                    TaxClass: '',
                    LotSize: '',
                    BuildingDimension: '',
                    Zoning: '',
                    PropertyTaxYear: 0.0,
                    ActualNumOfUnits: 0,
                    OccupancyStatus: undefined,
                    SellerOccupied: false,
                    NumOfTenants: 0,
                }
                this.DealCosts = {
                    MoneySpent: 0.0,
                    HAFA: false,
                    HOI: 0.0,
                    HOIRatio: 0.0,
                    COSTermination: 0.0,
                    AgentCommission: 0.0
                }
                this.RehabInfo = {
                    AverageLowValue: 0.0,
                    RenovatedValue: 0.0,
                    RepairBid: 0.0,
                    NeedsPlans: false,
                    DealTimeMonths: 0,
                    SalesCommission: 0.0,
                    DealROICash: 0.0
                }
                this.LienInfo = {
                    FirstMortgage: 0.0,
                    SecondMortgage: 0.0,
                    COSRecorded: false,
                    DeedRecorded: false,
                    OtherLiens: undefined,
                    LisPendens: false,
                    FHA: false,
                    FannieMae: false,
                    FreddieMac: false,
                    Servicer: '',
                    ForeclosureIndexNum: '',
                    ForeclosureStatus: undefined,
                    ForeclosureNote: '',
                    AuctionDate: undefined,
                    DefaultDate: undefined,
                    CurrentPayoff: 0.0,
                    PayoffDate: undefined,
                    CurrentSSValue: 0.0
                }
                this.LienCosts = {
                    TaxLienCertificate: 0.0,
                    PropertyTaxes: 0.0,
                    WaterCharges: 0.0,
                    ECBCityPay: 0.0,
                    DOBCivilPenalty: 0.0,
                    HPDCharges: 0.0,
                    HPDJudgements: 0.0,
                    PersonalJudgements: 0.0,
                    NYSTaxWarrants: 0.0,
                    FederalTaxLien: 0.0,
                    SidewalkLiens: false,
                    ParkingViolation: 0.0,
                    TransitAuthority: 0.0,
                    VacateOrder: false,
                    RelocationLien: 0.0,
                    RelocationLienDate: undefined
                }
                this.MinimumBaselineScenario = {};
                this.BestCaseScenario = {
                };
                this.Others = {};

                this.CashScenario = {};
                this.LoanScenario = {};
                this.FlipScenario = {};

                this.RentalInfo = {
                    DeedPurchase: 0.0,
                    CurrentlyRented: false,
                    RepairBidTotal: 0.0,
                    NumOfUnits: 0,
                    MarketRentTotal: 0.0,
                    RentalTime: 0
                }
                this.RentalModel = {};


                this.Liens = {};
                this.DealExpenses = {};
                this.ClosingCost = {};
                this.Construction = {}; 
                this.CarryingCosts = {
                    RETaxs: 0.0,
                    Utilities: 0,
                    Insurance: 0,
                    Sums: 0
                };
                this.Resale = {};
                this.LoanTerms = {};
                this.LoanCosts = {};
                this.FlipCalculation = {};
                this.MoneyFactor = {};
                this.HOI = {};
                this.HOIBestCase = {};
                this.InsurancePremium = {};

            },
            build: function () {
                var data = new this.UnderwritingModel();
                return data;
            }
        }

        underwriter.calculator = (function () {

            var insurancePolicyCalculation = function (v1, policyCol, cosCol, fromCol, toCol) {
                if (v1 < fromCol[0]) {
                    return 402;
                } else {
                    if (v1 <= toCol[0])
                        return (v1 - fromCol[0]) * policyCol[0];
                    else {
                        if (v1 <= toCol[1])
                            return (v1 - fromCol[1]) * policyCol[1] + cosCol[1];
                        else {
                            if (v1 <= toCol[2])
                                return (v1 - fromCol[2]) * policyCol[2] + cosCol[1];
                            else {
                                if (v1 <= toCol[3])
                                    return (v1 - fromCol[3]) * policyCol[3] + cosCol[2];
                                else {
                                    if (v1 <= toCol[4])
                                        return (v1 - fromCol[4]) * policyCol[4] + cosCol[3];
                                    else {
                                        if (v1 <= toCol[5])
                                            return (v1 - fromCol[5]) * policyCol[5] + cosCol[4];
                                        else {
                                            if (v1 <= toCol[6])
                                                return (v1 - fromCol[6]) * policyCol[6] + cosCol[5];
                                            else {
                                                if (v1 <= toCol[7])
                                                    return (v1 - fromCol[7]) * policyCol[7] + cosCol[6];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var RentalHelper = function (isRented, rentalTime, model) {
                var i, j, k = 0, temp = model.NetMontlyRent;
                this._model = [];
                this._model[k] = {
                    Month: k + 1,
                    Rent: 0,
                    Interest: -(model.TotalUpfront * model.CostOfMoneyRate / 12.0)
                };
                this._model[k].Total = -(model.TotalUpfront - this._model[k].Interest);
                k++;

                for (; k < 3; k++) {
                    this._model[k] = {
                        Month: k + 1,
                        Rent: isRented ? 0 : model.NetMontlyRent,
                        Interest: this._model[k - 1].Total * model.CostOfMoneyRate / 12.0
                    }
                    this._model[k].Total = this._model[k - 1].Total + this._model[k].Interest + this._model[k].Rent;
                }

                for (i = 0; i < 7; i++, temp = temp * 1.02) {
                    for (j = 0; j < 12; j++, k++) {
                        this._model[k] = {
                            Month: k + 1,
                            Rent: temp,
                        }
                        if (k < 28) {
                            this._model[k].Interest = this._model[k - 1].Total < 0 ? this._model[k - 1].Total * model.CostOfMoneyRate / 12 : 0.0;
                        } else {
                            this._model[k].Interest = this._model[k - 1].Total < 0 ? this._model[k - 1].Total * 0.18 / 12 : 0.0;
                        }
                        this._model[k].Total = this._model[k - 1].Total < 0 ? this._model[k - 1].Total + this._model[k].Interest + this._model[k].Rent : this._model[k - 1].Total + this._model[k].Rent;
                    }

                }

                this.costOfMoney = 0.0;

                for (i = 0; i < 60; i++) {
                    this.costOfMoney += this._model[i].Interest;
                }
                this.costOfMoney = -this.costOfMoney;

                this.totalCost = model.TotalUpfront + this.costOfMoney;
                for (var m = 1 ; m < this._model.length; m++) {
                    this._model[m].ROI = this._model[m].Total / this.totalCost / this._model[m].Month * 12;
                }

                this.totalMonths = 0;
                this.targetProfit = 0.0;
                for (m = 1; m < this._model.length; m++) {
                    if (this._model[m].ROI > model.MinROI) {
                        this.totalMonths = this._model[m].Month;
                        this.targetProfit = this._model[m].Total;
                        break;
                    }
                }
                this.totalMonths = rentalTime ? rentalTime : this.totalMonths;

                this.breakeven = 0;
                for (i = 0; i < 60; i++) {
                    if (this._model[i].Total > 0) {
                        this.breakeven = this._model[i].Month;
                        break;
                    }
                }

                this.ROIYear = this.targetProfit / this.totalCost / this.totalMonths * 12;
                this.ROITotal = this.targetProfit / this.totalCost;
            }

            var importData = function (d) {
                if (d.docSearch && d.docSearch.LeadResearch) {

                    var r = d.docSearch.LeadResearch;

                    d.PropertyInfo.PropertyTaxYear = r.leadsProperty_Taxes_per_YR_Property_Taxes_Due || 0.0;
                    d.LienInfo.FirstMortgage = r.mortgageAmount || 0.0;
                    d.LienInfo.SecondMortgage = r.secondMortgageAmount || 0.0;
                    d.LienInfo.COSRecorded = r.Has_COS_Recorded || false;
                    d.LienInfo.DeedRecorded = r.Has_Deed_Recorded || false;
                    d.LienInfo.FHA = r.fha || false;
                    d.LienInfo.FannieMae = r.fannie || false;
                    d.LienInfo.FreddieMac = r.Freddie_Mac_ || false;
                    d.LienInfo.Servicer = r.servicer;
                    d.LienInfo.ForeclosureIndexNum = r.LP_Index___Num_LP_Index___Num;
                    d.LienInfo.ForeclosureNote = r.notes_LP_Index___Num;
                    d.LienCosts.TaxLienCertificate = (function () {
                        var total = 0.0;
                        if (r.TaxLienCertificate) {
                            for (var i = 0; i < r.TaxLienCertificate.length; i++) {
                                total += parseFloat(r.TaxLienCertificate[i].Amount);
                            }
                        }
                        return total;
                    })();
                    d.LienCosts.PropertyTaxes = r.propertyTaxes || 0.0;
                    d.LienCosts.WaterCharges = r.waterCharges || 0.0;
                    d.LienCosts.HPDCharges = r.Open_Amount_HPD_Charges_Not_Paid_Transferred || 0.0;
                    d.LienCosts.ECBCityPay = r.Amount_ECB_Tickets || 0.0;
                    d.LienCosts.DOBCivilPenalty = r.dobWebsites || 0.0;
                    d.LienCosts.PersonalJudgements = r.Amount_Personal_Judgments || 0.0;
                    d.LienCosts.HPDJudgements = r.HPDjudgementAmount || 0.0;
                    d.LienCosts.NYSTaxWarrants = r.Amount_NYS_Tax_Lien || 0.0; 
                    d.LienCosts.FederalTaxLien = r.irsTaxLien || 0.0; 
                    d.LienCosts.VacateOrder = r.has_Vacate_Order_Vacate_Order || false;
                    d.LienCosts.RelocationLien = (function () {
                        if (r.has_Vacate_Order_Vacate_Order)
                            return parseFloat(r.Amount_Vacate_Order) || 0.0;
                    })()

                }
                if (d.leadsInfo) {
                    d.PropertyInfo.PropertyAddress = d.leadsInfo.PropertyAddress;
                    d.PropertyInfo.CurrentOwner = d.leadsInfo.Owner;
                    d.PropertyInfo.TaxClass = d.leadsInfo.TaxClass;
                    d.PropertyInfo.LotSize = d.leadsInfo.LotDem;
                    d.PropertyInfo.BuildingDimension = d.leadsInfo.BuildingDem;
                    d.PropertyInfo.Zoning = d.leadsInfo.Zoning;
                    d.PropertyInfo.FARActual = d.leadsInfo.ActualFar;
                    d.PropertyInfo.FARMax = d.leadsInfo.MaxFar;
                }
            }

            var applyFixedRules = function (d) {

                d.RehabInfo.SalesCommission = 0.05;
                d.RehabInfo.DealROICash = 0.35;
                d.InsurancePremium.From = [35001, 50001, 100001, 500001, 1000001, 5000001, 10000001, 15000001];
                d.InsurancePremium.To = [50000, 100000, 500000, 1000000, 5000000, 10000000, 15000000];
                d.InsurancePremium.OwnersPolicyRate = [.00667, .00543, .00436, .00398, .00366, .00325, .00307, .00276];
                d.InsurancePremium.LoanPolicyRate = [0.00555, 0.00454, 0.00364, 0.00331, 0.00305, 0.00271, 0.00255, 0.00231];
                d.InsurancePremium.CostOwnersPolicy = _.zip(d.InsurancePremium.From,
                                                            d.InsurancePremium.To,
                                                            d.InsurancePremium.OwnersPolicyRate).reduce(function (cum, v) {
                                                                var l = cum.length;
                                                                cum[l - 1] = (v[1] - v[0]) * v[2] + cum[l - 1];
                                                                cum[l] = cum[l - 1];
                                                                return cum;
                                                            }, [402])

                d.InsurancePremium.CostLoanPolicy = _.zip(d.InsurancePremium.From,
                                                          d.InsurancePremium.To,
                                                          d.InsurancePremium.LoanPolicyRate).reduce(function (cum, v) {
                                                              var l = cum.length;
                                                              cum[l - 1] = (v[1] - v[0]) * v[2] + cum[l - 1];
                                                              cum[l] = cum[l - 1];
                                                              return cum;
                                                          }, [344])
                d.FlipCalculation.FlipROI = 0.15;
                d.MoneyFactor.CostOfMoney = 0.0;
                d.MoneyFactor.InterestOnMoney = 0.18;
                d.Liens.LienPayoffsSettlement = 1.0
                d.Liens.TaxLienCertificateSettlement = 0.09 / 12;
                d.Liens.WaterChargesSettlement = 1.0;
                d.Liens.PropertyTaxesSettlement = 1.0;
                d.Liens.ECBCityPaySettlement = 1.0;
                d.Liens.DOBCivilPenaltiesSettlement = 1.0;
                d.Liens.HPDChargesSettlement = 1.0;

                d.Liens.HPDJudgementsSettlement = 0.15;
                d.Liens.PersonalJudgementsSettlement = 0.40;
                d.Liens.ParkingViolationSettlement = 1.0;
                d.Liens.TransitAuthoritySettlement = 1.0;
                d.Liens.RelocationLienSettlement = .09 / 365;


                d.ClosingCost.TitleBill = 1200.00;
                d.ClosingCost.BuyerAttorney = 1250.00;
                d.Resale.Concession = 0.0;
                d.Resale.Attorney = 1250.0;
                d.Resale.NDC = 500;
                d.LoanTerms.LoanRate = 0.12;
                d.LoanTerms.LoanPoints = 2;
                d.LoanTerms.LoanTermMonths = 12;
                d.LoanTerms.LTV = 0.6;
                d.HOI.Value = 0.25;
                d.RentalModel.CostOfMoneyRate = 0.16;
                d.RentalModel.MinROI = 0.18;
                d.RentalModel.Insurance = 85.0;

            };

            var applyRule = function (d) {
                var float = function (data) {
                    if (data)
                        return parseFloat(data);
                    else
                        return 0.0;
                }
                var int = function (data) {
                    if (data)
                        return parseInt(data);
                    else
                        return 0;
                }

                d.PropertyInfo.PropertyType = (function () { return /.*(A|B|C0|21|R).*/.exec(d.PropertyInfo.TaxClass) ? 1 : 2 })();

                d.Liens.TaxLienCertificate = float(d.LienCosts.TaxLienCertificate) * (1.0 + d.Liens.TaxLienCertificateSettlement * float(d.RehabInfo.DealTimeMonths));
                d.Liens.PropertyTaxes = float(d.LienCosts.PropertyTaxes) * d.Liens.PropertyTaxesSettlement;
                d.Liens.WaterCharges = float(d.LienCosts.WaterCharges) * d.Liens.WaterChargesSettlement;
                d.Liens.ECBCityPay = float(d.LienCosts.ECBCityPay) * d.Liens.ECBCityPaySettlement;
                d.Liens.DOBCivilPenalties = float(d.LienCosts.DOBCivilPenalty) * d.Liens.DOBCivilPenaltiesSettlement;
                d.Liens.HPDCharges = float(d.LienCosts.HPDCharges) * d.Liens.HPDChargesSettlement;
                d.Liens.HPDJudgements = float(d.LienCosts.HPDJudgements) * d.Liens.HPDJudgementsSettlement;
                d.Liens.PersonalJudgements = float(d.LienCosts.PersonalJudgements) * d.Liens.PersonalJudgementsSettlement;
                d.Liens.NYSTaxWarrantsSettlement = (function () {
                    return float(d.LienCosts.NYSTaxWarrantsSettlement) < 12500 ? 1.0 : 0.0
                })();
                d.Liens.NYSTaxWarrants = float(d.LienCosts.NYSTaxWarrants) * d.Liens.NYSTaxWarrantsSettlement;
                d.Liens.FederalTaxLienSettlement = (function () {
                    return float(d.LienCosts.FederalTaxLien) < 12500 ? 1.0 : 0.0
                })();
                d.Liens.FederalTaxLien = float(d.LienCosts.FederalTaxLien) * d.Liens.FederalTaxLienSettlement;
                d.Liens.ParkingViolation = float(d.LienCosts.ParkingViolation) * d.Liens.ParkingViolationSettlement;
                d.Liens.TransitAuthority = float(d.LienCosts.TransitAuthority) * d.Liens.TransitAuthoritySettlement;
                d.Liens.RelocationLien = (function () {
                    if (!d.LienCosts.RelocationLienDate)
                        return 0.0;
                    else {
                        return float(d.LienCosts.RelocationLien) * (1.0 + (moment().diff(moment(d.LienCosts.RelocationLienDate), 'days') + 180) * d.Liens.RelocationLienSettlement)
                    }
                })();
                d.DealCosts.HAFA = (d.PropertyInfo.SellerOccupied || int(d.PropertyInfo.NumOfTenants) > 0) &&
                                    !d.LienInfo.FHA &&
                                    !d.LienInfo.FannieMae &&
                                    !d.LienInfo.FreddieMac &&
                                    float(d.DealCosts.HOI) > 0.0;

                d.DealExpenses.MoneySpent = float(d.DealCosts.MoneySpent);
                d.DealExpenses.HOILienSettlement = float(d.DealCosts.HOIRatio);
                d.DealExpenses.HOILien = d.DealCosts.HAFA ? float(d.DealCosts.HOI) * d.DealExpenses.HOILienSettlement - 10000.00 : float(d.DealCosts.HOI) * d.DealExpenses.HOILienSettlement;
                d.DealExpenses.COSTermination = float(d.DealCosts.COSTermination);
                d.DealExpenses.Tenants = float(d.PropertyInfo.NumOfTenants) * 7000.00;
                d.DealExpenses.Agent = float(d.DealCosts.AgentCommission);

                d.Construction.Construction = float(d.RehabInfo.RepairBid);
                d.Construction.Architect = d.RehabInfo.NeedsPlans ? 8500 : 0;

                d.CarryingCosts.RETaxs = float(d.PropertyInfo.PropertyTaxYear) / 12 * float(d.RehabInfo.DealTimeMonths);
                d.CarryingCosts.Utilities = 150 * Math.pow(float(d.PropertyInfo.ActualNumOfUnits), 2) + 400 * float(d.PropertyInfo.ActualNumOfUnits);

                d.Resale.ProbableResale = float(d.RehabInfo.RenovatedValue);
                d.Resale.Commissions = d.Resale.ProbableResale * float(d.RehabInfo.SalesCommission);
                d.Resale.TransferTax = (function () {
                    var pr = parseFloat(d.Resale.ProbableResale);
                    var rate;
                    if (d.PropertyInfo.PropertyType == 1) {
                        if (pr <= 500000) {
                            rate = 0.01;
                        } else if (pr > 500000) {
                            rate = 0.01425;
                        }
                    } else {
                        if (pr <= 500000) {
                            rate = 0.01425;
                        } else if (pr > 500000) {
                            rate = 0.02625;
                        }
                    }

                    return (rate + 0.004) * pr;
                })();
                d.LoanTerms.LoanAmount = d.Resale.ProbableResale * d.LoanTerms.LTV;
                d.CarryingCosts.Insurance = d.LoanTerms.LoanAmount / 100.0 * 0.45 / 12 * float(d.RehabInfo.DealTimeMonths);
                d.LoanCosts.LoanClosingCost = (function () {
                    var la = d.LoanTerms.LoanAmount;
                    var rate;
                    if (d.PropertyInfo.PropertyType == 1) {
                        if (la <= 500000) {
                            rate = 0.02;

                        } else if (la > 500000) {
                            rate = 0.02125
                        }
                    } else {
                        if (la <= 500000) {
                            rate = 0.02;

                        } else if (la > 500000) {
                            rate = 0.0275;
                        }
                    }
                    return la * rate + 275 + 1500;
                })();
                d.LoanCosts.Points = d.LoanTerms.LoanAmount * d.LoanTerms.LoanPoints / 100.0;
                d.LoanCosts.LoanInterest = d.LoanTerms.LoanAmount * d.LoanTerms.LoanRate / 12.0 * float(d.RehabInfo.DealTimeMonths);

                d.Liens.Sums = d.Liens.TaxLienCertificate + d.Liens.PropertyTaxes + d.Liens.WaterCharges + d.Liens.ECBCityPay + d.Liens.DOBCivilPenalties + d.Liens.HPDCharges + d.Liens.HPDJudgements + d.Liens.PersonalJudgements + d.Liens.NYSTaxWarrants + d.Liens.FederalTaxLien + d.Liens.ParkingViolation + d.Liens.TransitAuthority + d.Liens.RelocationLien;
                d.DealExpenses.Sums = d.DealExpenses.MoneySpent + d.DealExpenses.HOILien + d.DealExpenses.COSTermination + d.DealExpenses.Tenants + d.DealExpenses.Agent;
                d.ClosingCost.PartialSums = d.ClosingCost.TitleBill + d.ClosingCost.BuyerAttorney;
                d.Construction.Sums = d.Construction.Construction + d.Construction.Architect;
                d.CarryingCosts.Sums = d.CarryingCosts.Insurance + d.CarryingCosts.RETaxs + d.CarryingCosts.Utilities;
                d.Resale.Sums = d.Resale.Concession + d.Resale.Commissions + d.Resale.TransferTax + d.Resale.Attorney + d.Resale.NDC;
                d.Liens.LienPayoffs = (d.Resale.ProbableResale - (d.Liens.Sums + d.DealExpenses.Sums + d.ClosingCost.PartialSums + d.Construction.Sums + d.CarryingCosts.Sums + d.Resale.Sums) - (d.Liens.Sums + d.DealExpenses.Sums + d.ClosingCost.PartialSums + d.Construction.Sums + d.CarryingCosts.Sums) * float(d.RehabInfo.DealROICash)) / ((float(d.RehabInfo.DealROICash) + 1) * 1.0058);
                d.Liens.AdditonalCostsSums = d.Liens.WaterCharges + d.Liens.ECBCityPay + d.Liens.DOBCivilPenalties + d.Liens.HPDCharges + d.Liens.HPDJudgements + d.Liens.PersonalJudgements + d.Liens.NYSTaxWarrants + d.Liens.FederalTaxLien + d.Liens.ParkingViolation + d.Liens.TransitAuthority + d.Liens.RelocationLien;

                d.InsurancePremium.PurchasePrice = d.Liens.LienPayoffs;
                d.InsurancePremium.LoanAmountDiscounted = d.LoanTerms.LoanAmount >= d.Liens.LienPayoffs ? d.Liens.LienPayoffs : d.LoanTerms.LoanAmount;
                d.InsurancePremium.LoanAmountFullPremium = d.LoanTerms.LoanAmount - d.InsurancePremium.LoanAmountDiscounted;
                d.InsurancePremium.OwnersPolicy = insurancePolicyCalculation(d.InsurancePremium.PurchasePrice, d.InsurancePremium.OwnersPolicyRate, d.InsurancePremium.CostOwnersPolicy, d.InsurancePremium.From, d.InsurancePremium.To)
                d.InsurancePremium.OwnersLoanPolicyDiscounted = insurancePolicyCalculation(d.InsurancePremium.LoanAmountDiscounted, d.InsurancePremium.LoanPolicyRate, d.InsurancePremium.CostLoanPolicy, d.InsurancePremium.From, d.InsurancePremium.To)
                d.InsurancePremium.OwnersLoanPolicyFullPremium = insurancePolicyCalculation(d.InsurancePremium.LoanAmountFullPremium, d.InsurancePremium.LoanPolicyRate, d.InsurancePremium.CostLoanPolicy, d.InsurancePremium.From, d.InsurancePremium.To)
                d.InsurancePremium.TitleInsurance = d.InsurancePremium.OwnersLoanPolicyDiscounted * 1.3 + d.InsurancePremium.OwnersLoanPolicyFullPremium;
                d.ClosingCost.OwnersPolicy = d.InsurancePremium.OwnersPolicy;
                d.ClosingCost.Sums = d.ClosingCost.OwnersPolicy + d.ClosingCost.TitleBill + d.ClosingCost.BuyerAttorney;
                d.LoanCosts.LoanPolicy = d.InsurancePremium.TitleInsurance - d.InsurancePremium.OwnersPolicy;
                d.FlipCalculation.FlipPrice = (d.Resale.ProbableResale - (d.ClosingCost.Sums + d.Construction.Sums + d.CarryingCosts.Sums + d.Resale.Sums) - (d.ClosingCost.Sums + d.Construction.Sums + d.CarryingCosts.Sums) * d.FlipCalculation.FlipROI) / ((d.FlipCalculation.FlipROI + 1) * 1.0);

                d.LoanCosts.Sums = d.LoanCosts.LoanPolicy + d.LoanCosts.LoanClosingCost + d.LoanCosts.Points + d.LoanCosts.LoanInterest;
                d.HOI.PurchasePriceAllIn = (d.Resale.ProbableResale - (d.ClosingCost.Sums + d.Construction.Sums + d.CarryingCosts.Sums + d.Resale.Sums + d.LoanCosts.Sums) - (d.ClosingCost.Sums + d.Construction.Sums + d.CarryingCosts.Sums + d.LoanCosts.Sums) * d.HOI.Value) / ((d.HOI.Value + 1) * 1.0);
                d.HOI.TotalInvestment = d.HOI.PurchasePriceAllIn + d.ClosingCost.Sums + d.Construction.Sums + d.CarryingCosts.Sums + d.LoanCosts.Sums;
                d.HOI.CashRequirement = d.HOI.TotalInvestment - d.LoanTerms.LoanAmount;
                d.HOI.NetProfit = d.Resale.ProbableResale - d.Resale.Sums - d.HOI.TotalInvestment;
                d.HOI.ROILoan = d.HOI.NetProfit / d.HOI.TotalInvestment;

                d.HOIBestCase.PurchasePriceAllIn = float(d.RehabInfo.AverageLowValue) + d.Liens.AdditonalCostsSums + d.DealExpenses.Sums - d.DealExpenses.HOILien;
                d.HOIBestCase.TotalInvestment = d.HOIBestCase.PurchasePriceAllIn + d.ClosingCost.Sums + d.Construction.Sums + d.CarryingCosts.Sums + d.LoanCosts.Sums;
                d.HOIBestCase.CashRequirement = d.HOIBestCase.TotalInvestment - d.LoanTerms.LoanAmount;
                d.HOIBestCase.NetProfit = d.Resale.ProbableResale - d.Resale.Sums - d.HOIBestCase.TotalInvestment;
                d.HOIBestCase.ROILoan = d.HOIBestCase.NetProfit / d.HOIBestCase.TotalInvestment;

                d.CashScenario.Purchase_LienPayoffs = d.Liens.LienPayoffs + d.Liens.TaxLienCertificate + d.Liens.PropertyTaxes;
                d.CashScenario.Purchase_OffHUDCosts = d.Liens.AdditonalCostsSums;
                d.CashScenario.Purchase_DealCosts = d.DealExpenses.Sums;
                d.CashScenario.Purchase_ClosingCost = d.ClosingCost.Sums;
                d.CashScenario.Purchase_Construction = d.Construction.Sums;
                d.CashScenario.Purchase_CarryingCosts = d.CarryingCosts.Sums;
                d.CashScenario.Purchase_TotalInvestment = d.CashScenario.Purchase_LienPayoffs + d.CashScenario.Purchase_OffHUDCosts + d.CashScenario.Purchase_DealCosts + d.CashScenario.Purchase_ClosingCost + d.CashScenario.Purchase_Construction + d.CashScenario.Purchase_CarryingCosts;
                d.CashScenario.Resale_SalePrice = d.Resale.ProbableResale;
                d.CashScenario.Resale_Concession = d.Resale.Concession;
                d.CashScenario.Resale_Commissions = d.Resale.Commissions;
                d.CashScenario.Resale_ClosingCost = d.Resale.TransferTax + d.Resale.Attorney + d.Resale.NDC;
                d.CashScenario.Resale_NetProfit = d.CashScenario.Resale_SalePrice - (d.CashScenario.Resale_Concession + d.CashScenario.Resale_Commissions + d.CashScenario.Resale_ClosingCost) - d.CashScenario.Purchase_TotalInvestment;
                d.CashScenario.Time = float(d.RehabInfo.DealTimeMonths);
                d.CashScenario.CashRequired = d.CashScenario.Purchase_TotalInvestment;
                d.CashScenario.ROI = d.CashScenario.Resale_NetProfit / d.CashScenario.Purchase_TotalInvestment;
                d.CashScenario.ROIAnnual = d.CashScenario.ROI / d.RehabInfo.DealTimeMonths * 12;
                d.LoanScenario.Purchase_PurchasePrice = d.Liens.LienPayoffs + d.Liens.TaxLienCertificate + d.Liens.PropertyTaxes;
                d.LoanScenario.Purchase_AdditonalCosts = d.Liens.AdditonalCostsSums;
                d.LoanScenario.Purchase_DealCosts = d.DealExpenses.Sums;
                d.LoanScenario.Purchase_ClosingCost = d.ClosingCost.Sums;
                d.LoanScenario.Purchase_Construction = d.Construction.Sums;
                d.LoanScenario.Purchase_CarryingCosts = d.CarryingCosts.Sums;
                d.LoanScenario.Purchase_LoanClosingCost = d.LoanCosts.LoanPolicy + d.LoanCosts.LoanClosingCost + d.LoanCosts.Points;
                d.LoanScenario.Purchase_LoanInterest = d.LoanCosts.LoanInterest;
                d.LoanScenario.Purchase_TotalInvestment = d.Liens.LienPayoffs + d.Liens.Sums + d.DealExpenses.Sums + d.ClosingCost.Sums + d.Construction.Sums + d.CarryingCosts.Sums + d.LoanCosts.Sums;
                d.LoanScenario.Resale_SalePrice = d.Resale.ProbableResale;
                d.LoanScenario.Resale_Concession = d.Resale.Concession;
                d.LoanScenario.Resale_Commissions = d.Resale.Commissions;
                d.LoanScenario.Resale_ClosingCost = d.Resale.TransferTax + d.Resale.Attorney + d.Resale.NDC;
                d.LoanScenario.Resale_NetProfit = d.LoanScenario.Resale_SalePrice - (d.LoanScenario.Resale_Concession + d.LoanScenario.Resale_Commissions + d.LoanScenario.Resale_ClosingCost) - d.LoanScenario.Purchase_TotalInvestment;
                d.LoanScenario.Time = float(d.RehabInfo.DealTimeMonths);
                d.LoanScenario.LoanAmount = d.LoanTerms.LoanAmount;
                d.LoanScenario.LTV = d.LoanScenario.LoanAmount / d.LoanScenario.Resale_SalePrice;
                d.LoanScenario.CashRequirement = d.LoanScenario.Purchase_TotalInvestment - d.LoanScenario.LoanAmount;
                d.LoanScenario.ROI = d.LoanScenario.Resale_NetProfit / d.LoanScenario.Purchase_TotalInvestment;
                d.LoanScenario.ROIAnnual = d.LoanScenario.ROI / d.RehabInfo.DealTimeMonths * 12;
                d.LoanScenario.CashROI = d.LoanScenario.Resale_NetProfit / d.LoanScenario.CashRequirement;
                d.LoanScenario.CashROIAnnual = d.LoanScenario.CashROI / d.RehabInfo.DealTimeMonths * 12;
                d.FlipScenario.Purchase_TotalCost = d.Liens.LienPayoffs + d.Liens.Sums + d.DealExpenses.Sums;
                d.FlipScenario.FlipPrice_SalePrice = d.FlipCalculation.FlipPrice;

                d.FlipScenario.Purchase_PurchasePrice = d.FlipScenario.FlipPrice_SalePrice;
                d.FlipScenario.Purchase_ClosingCost = d.ClosingCost.Sums;
                d.FlipScenario.Purchase_Construction = d.Construction.Sums;
                d.FlipScenario.Purchase_CarryingCosts = d.CarryingCosts.Sums;
                d.FlipScenario.Purchase_TotalInvestment = d.FlipScenario.Purchase_PurchasePrice + d.FlipScenario.Purchase_ClosingCost + d.FlipScenario.Purchase_Construction + d.FlipScenario.Purchase_CarryingCosts;
                d.FlipScenario.Resale_SalePrice = d.Resale.ProbableResale;
                d.FlipScenario.Resale_Concession = d.Resale.Concession;
                d.FlipScenario.Resale_Commissions = d.Resale.Commissions;
                d.FlipScenario.Resale_ClosingCost = d.Resale.TransferTax + d.Resale.Attorney + d.Resale.NDC;
                d.FlipScenario.Resale_NetProfit = d.FlipScenario.Resale_SalePrice - (d.FlipScenario.Resale_Commissions + d.FlipScenario.Resale_ClosingCost) - d.FlipScenario.Purchase_TotalInvestment;
                d.FlipScenario.FlipProfit = d.FlipScenario.FlipPrice_SalePrice - d.FlipScenario.Purchase_TotalCost;
                d.FlipScenario.CashRequirement = d.FlipScenario.Purchase_TotalInvestment
                d.FlipScenario.ROI = d.FlipScenario.Resale_NetProfit / d.FlipScenario.Purchase_TotalInvestment;
                d.Others.MaximumLienPayoff = d.Liens.LienPayoffs + d.Liens.TaxLienCertificate + d.Liens.PropertyTaxes;
                d.Others.MaximumSSPrice = d.Liens.LienPayoffs + d.Liens.Sums;
                d.Others.MaxHOI = d.HOIBestCase.NetProfit - d.HOI.NetProfit;
                d.MinimumBaselineScenario.PurchasePriceAllIn = d.Liens.LienPayoffs + d.Liens.Sums + d.DealExpenses.Sums;
                d.MinimumBaselineScenario.TotalInvestment = d.LoanScenario.Purchase_TotalInvestment;
                d.MinimumBaselineScenario.CashRequirement = d.LoanScenario.CashRequirement;
                d.MinimumBaselineScenario.NetProfit = d.LoanScenario.Resale_NetProfit;
                d.MinimumBaselineScenario.ROI = d.LoanScenario.ROI;
                d.BestCaseScenario.PurchasePriceAllIn = float(d.RehabInfo.AverageLowValue) + d.Liens.AdditonalCostsSums + d.DealExpenses.Sums;
                d.BestCaseScenario.TotalInvestment = d.BestCaseScenario.PurchasePriceAllIn + d.ClosingCost.Sums + d.Construction.Sums + d.CarryingCosts.Sums + d.LoanCosts.Sums;
                d.BestCaseScenario.CashRequirement = d.BestCaseScenario.TotalInvestment - d.LoanTerms.LoanAmount;
                d.BestCaseScenario.NetProfit = d.LoanScenario.Resale_SalePrice - (d.LoanScenario.Resale_Concession + d.LoanScenario.Resale_Commissions + d.LoanScenario.Resale_ClosingCost) - (d.BestCaseScenario.PurchasePriceAllIn + d.ClosingCost.Sums + d.Construction.Sums + d.CarryingCosts.Sums + d.LoanCosts.Sums);
                d.BestCaseScenario.ROI = d.BestCaseScenario.NetProfit / d.BestCaseScenario.TotalInvestment
                d.RentalModel.NumOfUnits = int(d.RentalInfo.NumOfUnits);
                d.RentalModel.DeedPurchase = float(d.RentalInfo.DeedPurchase);
                d.RentalModel.TotalRepairs = float(d.RentalInfo.RepairBidTotal);
                d.RentalModel.AgentCommission = float(d.DealCosts.AgentCommission);
                d.RentalModel.TotalUpfront = d.RentalModel.DeedPurchase + d.RentalModel.TotalRepairs + d.RentalModel.AgentCommission;
                d.RentalModel.Rent = float(d.RentalInfo.MarketRentTotal);
                d.RentalModel.ManagementFee = d.RentalModel.Rent * 0.1;
                d.RentalModel.Maintenance = 50 + (d.RentalModel.NumOfUnits - 1) * 25;
                d.RentalModel.MiscRepairs = 75 * d.RentalModel.NumOfUnits;
                d.RentalModel.NetMontlyRent = d.RentalModel.Rent - d.RentalModel.ManagementFee - d.RentalModel.Maintenance - d.RentalModel.Insurance - d.RentalModel.MiscRepairs;
                var helper = new RentalHelper(d.RentalInfo.CurrentlyRented, d.RentalInfo.RentalTime, d.RentalModel);
                d.RentalModel.TotalMonth = helper.totalMonths;
                d.RentalModel.CostOfMoney = helper.costOfMoney;
                d.RentalModel.TotalCost = helper.totalCost;
                d.RentalModel.Breakeven = helper.breakeven;
                d.RentalModel.TargetTime = d.RentalModel.TotalMonth;
                d.RentalModel.TargetProfit = helper.targetProfit;
                d.RentalModel.ROIYear = helper.ROIYear;
                d.RentalModel.ROITotal = helper.ROITotal;
            }

            return {
                calculate: applyRule,
                applyFixedRules: applyFixedRules,
                importData: importData,
            }
        })();

        underwriter.load = function ( bble) {
            var data = underwritingFactory.build();
            underwriter.calculator.applyFixedRules(data);

            if (bble) {
                var _data = underwriter.get({ BBLE: bble.trim() }, function (d) {
                    _data.BBLE = bble;
                    _.defaults(_data, data);
                    if (!_data.Id) {  
                        data.docSearch = DocSearch.get({ BBLE: bble.trim() }, function () {
                            data.leadsInfo = LeadsInfo.get({ BBLE: bble.trim() }, function () {
                                underwriter.calculator.importData(data);
                            })

                        });
                    }

                })
                return _data;
            }
            return data;
        }

        underwriter.save = function (data) {
            return $http({
                method: 'POST',
                url: '/api/underwriter',
                data: data,
                headers: {
                    'Content-Type': 'application/json'
                }

            })

        }

        underwriter.archive = function (data, msg) {

            return $http({
                method: 'POST',
                url: '/api/underwriter/archive',
                data: [data, msg]
            })
        }

        underwriter.loadArchivedList = function (bble) {
            return $http({
                method: 'GET',
                url: '/api/underwriter/archived/' + bble
            })
        }

        underwriter.loadArchived = function (id) {

            return $http({
                method: 'GET',
                url: '/api/underwriter/archived/id/' + id
            })
        }

        return underwriter;

    }]);
angular.module('PortalApp')
    .factory('UnderwritingRequest', ['$http', 'ptBaseResource', 'DocSearch', function ($http, ptBaseResource, DocSearch) {

        var resource = ptBaseResource('UnderwritingRequest', 'BBLE', null, {});
        resource.saveByBBLE = function (data, bble) {
            if (bble) {
                data.BBLE = bble;
            }
            var promise = $http({
                method: 'POST',
                url: '/api/UnderwritingRequest',
                data: data
            })
            return promise;
        }

        resource.createSearch = function (BBLE) {
            var promise = $http({
                method: "POST",
                url: '/api/LeadInfoDocumentSearches',
                data: JSON.stringify({ "BBLE": BBLE }),
                header: {
                    'Content-Type': 'application/json'
                }
            });
            return promise;
        }

        return resource;
    }]);
angular.module('PortalApp').factory('Wizard', function (WizardStep) {
    var _class = function () {

           }
    _class.prototype.filteredSteps = [];
    _class.prototype.setFilteredSteps = function(filteredSteps)
    {
        this.filteredSteps = filteredSteps;
    }
    _class.prototype.scope = { step: 1 };
    _class.prototype.MaxStep = function()
    {
        return this.filteredSteps.length;
    }
    _class.prototype.setScope = function (scope)
    {
        this.scope = scope;
    }
    _class.prototype.currentStep = function()
    {
        return this.filteredSteps[this.scope.step - 1];
    }


            return _class;
});
angular.module('PortalApp').factory('WizardStep', function () {
    var _class = function (step) {

                this.title = step.title;
        this.next = step.next;
        this.init = step.init;
        angular.extend(this, step);
    }
    _class.prototype.title = "";
    _class.prototype.next = function ()
    {
        return true;
    }
    _class.prototype.init = function()
    {
        return true;
    }

    return _class;
});
angular.module("PortalApp")
.directive('dsSummary', function () {
    return {
        restrict: 'E',
        scope: {
            summary:'='
        },
        templateUrl: '/js/Views/LeadDocSearch/dsSummary.html'
    };
})
angular.module("PortalApp")
.directive('newDsSummary', function () {
    return {
        restrict: 'E',
        templateUrl: '/js/Views/LeadDocSearch/new_ds_summary.html',
    };
})


angular.module("PortalApp").service("ptCom", ["$rootScope", function ($rootScope) {
    var that = this;

    this.arrayAdd = function (model, data) {
        if (model) {
            data = data ? data : {};
            model.push(data);
        }
    };


    this.arrayRemove = function (model, index, confirm, callback) {
        if (model && index < model.length) {
            if (confirm) {
                that.confirm("Delete This?", "").then(function (r) {
                    if (r) {
                        var deleteObj = model.splice(index, 1)[0];
                        if (callback) callback(deleteObj);
                    }
                });
            } else {
                model.splice(index, 1);
            }
        }
    };

    this.formatAddr = function (strNO, strName, aptNO, city, state, zip) {
        var result = '';
        if (strNO) result += strNO + ' ';
        if (strName) result += strName + ', ';
        if (aptNO) result += 'Apt ' + aptNO + ', ';
        if (city) result += city + ', ';
        if (state) result += state + ', ';
        if (zip) result += zip;
        return result;
    };

    this.capitalizeFirstLetter = function (string) {
        return string.charAt(0).toUpperCase() + string.slice(1);
    };

    this.formatName = function (firstName, middleName, lastName) {
        var result = '';
        if (firstName) result += that.capitalizeFirstLetter(firstName) + ' ';
        if (middleName) result += that.capitalizeFirstLetter(middleName) + ' ';
        if (lastName) result += that.capitalizeFirstLetter(lastName);
        return result;
    };
    this.ensureArray = function (scope, modelName) {
        if (!scope.$eval(modelName)) {
            scope.$eval(modelName + '=[]');
        }
    };
    this.ensurePush = function (scope, modelName, data) {
        that.ensureArray(scope, modelName);
        data = data ? data : {};
        var model = scope.$eval(modelName);
        model.push(data);
    };
    this.nullToUndefined = function (obj) {
        for (var property in obj) {
            if (obj.hasOwnProperty(property)) {
                if (obj[property] === null) {
                    obj[property] = undefined;
                } else {
                    if (typeof obj[property] === "object") {
                        that.nullToUndefined(obj[property]);
                    }
                }
            }
        }
    };
    this.printDiv = function (divID) {
        var divToPrint = document.getElementById(divID);
        var popupWin = window.open('', '_blank', 'width=300,height=300');
        popupWin.document.open();
        popupWin.document.write('<html><head>' +
        '<link rel="stylesheet" href="http://fonts.googleapis.com/css?family=Source+Sans+Pro:200,300,400,600,700,900" />' +
        '<link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" />' +
        '<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/normalize/3.0.3/normalize.min.css" />' +
        '<link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" />' +
        '<link rel="stylesheet" href="/Content/bootstrap-datepicker3.css" />' +
        '<link rel="stylesheet" href="http://cdn3.devexpress.com/jslib/15.1.6/css/dx.common.css" />' +
        '<link rel="stylesheet" href="http://cdn3.devexpress.com/jslib/15.1.6/css/dx.light.css" />' +
        '<link rel="stylesheet" href="/css/stevencss.css"type="text/css" />' +
        '</head><body onload="window.print()">' + divToPrint.innerHTML + '</html>');
        popupWin.document.close();
    };

    this.postRequest = function (url, data) {
        $.post(url, data, function (retData) {
            $("body").append("<iframe src='" + retData.url + "' style='display: none;' ></iframe>");
        });
    };
    this.alert = function (message) {
        $rootScope.alert(message);
    };


    this.confirm = function (message, callback) {
        return $rootScope.confirm(message, callback);
    };

    this.prompt = function (message, callback, showArea) {
        return $rootScope.prompt(message, callback, showArea);
    }

    this.addOverlay = function () {
        $rootScope.addOverlay();
    };
    this.stopLoading = function () {
        $rootScope.stopLoading();
    }
    this.startLoading = function () {
        $rootScope.startLoading();
    }

    this.removeOverlay = function () {
        $rootScope.removeOverlay();
    }; 
    this.next = function (array, value, from) {
        return array.indexOf(value, from);
    };
    this.previous = function (array, value, from) {
        var index = -1;
        for (var i = 0 ; i < from ; i++) {
            if (array[i] === value)
                index = i;
        }
        return index;
    };
    this.saveBlob = function (blob, fileName) {
        var a = document.createElement("a");
        a.style = "display: none";
        var xurl = window.URL.createObjectURL(blob);
        a.href = xurl;
        a.download = fileName;

        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(xurl);
        document.body.removeChild(a);

    };
    this.toUTCLocaleDateString = function (d) {
        var tempDate = new Date(d);
        return (tempDate.getUTCMonth() + 1) + "/" + tempDate.getUTCDate() + "/" + tempDate.getUTCFullYear();
    };

    this.assignReference = function (target, source,  skipped,  keeped) {
        var temp = {}; 
        var props = Object.keys(source);
        for (i = 0; i < props.length ; i++) {
            if (typeof source[props[i]] == 'object') {
                if (skipped && skipped.indexOf(props[i]) >= 0) {
                    continue;
                }
                if (keeped && keeped.length) {
                    temp[props[i]] = {};
                    for (j = 0; j < keeped.length; j++) {
                        if (target[props[i]] && target[props[i]][keeped[j]]) {
                            temp[props[i]][keeped[j]] = target[props[i]][keeped[j]];
                        }
                    }
                }
                target[props[i]] = source[props[i]];
                if (keeped && keeped.length) {
                    for (j = 0; j < keeped.length; j++) {
                        if (temp[props[i]] && temp[props[i]][keeped[j]]) {
                            target[props[i]][keeped[j]] = temp[props[i]][keeped[j]];
                        }
                    }
                }
            }
        }

    }

    this.parseSearch = function ( searchString) {
        var result = {};
        if (!searchString || typeof searchString != 'string')   
            return result;
        if (searchString.slice(0, 1) != '?')    
            return result;
        var entriesString = searchString.slice(1).replace(/%20/g, '');  
        var entries = entriesString.split("&");
        for (var i = 0; i < entries.length; i++) {
            entry = entries[i].split("=");
            if (entry.length > 1) {
                result[entry[0]] = entry[1];
            }
        }
        return result;
    }

    this.setGlobal = function (key, value) {
        $rootScope.globaldata[key] = value
    }

    this.getGlobal = function (key) {
        if ($rootScope.globaldata[key] != null) {
            return $rootScope.globaldata[key];
        } else {
            return undefined;
        }
    }   
}])
angular.module("PortalApp").service('ptConstructionService', ['$http', function ($http) {
    this.getConstructionCases = function (bble, callback) {
        var url = "/api/ConstructionCases/" + bble;
        $http.get(url)
            .success(function (data) {
                if (callback) callback(data);
            }).error(function (data) {
                console.log("Get Construction Data fails.");
            });
    };
    this.saveConstructionCases = function (bble, data, callback) {
        if (bble && data) {
            bble = bble.trim();
            var url = "/api/ConstructionCases/" + bble;
            $http.put(url, data)
                .success(function (res) {
                    if (callback) callback(res);
                }).error(function () {
                    alert('Save CSCase fails.');
                });
        }
    };
    this.getDOBViolations = function (bble, callback) {
        if (bble) {
            var url = "/api/ConstructionCases/GetDOBViolations?bble=" + bble;
            $http.get(url)
            .success(function (res) {
                if (callback) callback(null, res);
            }).error(function () {
                if (callback) callback("load dob violations fails");
            });
        } else {
            if (callback) callback("bble is missing");
        }
    };
    this.getECBViolations = function (bble, callback) {
        if (bble) {
            var url = "/api/ConstructionCases/GetECBViolations?bble=" + bble;
            $http.get(url)
            .success(function (res) {
                if (callback) callback(null, res);
            }).error(function () {
                if (callback) callback("load ecb violations fails");
            });
        }
    };
}
])
angular.module("PortalApp")
    .service('ptContactServices', ['$http', 'limitToFilter', function ($http, limitToFilter) {

    var allContact;
    var allTeam;

    (function () {
        if (!allContact) {
            $http.get('/Services/ContactService.svc/LoadContacts')
           .success(function (data, status) {
               allContact = data;
           }).error(function (data, status) {
               allContact = [];
           });
        }

        if (!allTeam) {
            $http.get('/Services/TeamService.svc/GetAllTeam')
            .success(function (data, status) {
                allTeam = data;
            })
            .error(function (data, status) {
                allTeam = [];
            });
        }

    }());

    this.getAllContacts = function () {
        if (allContact) return allContact;
        return $http.get('/Services/ContactService.svc/LoadContacts')
            .then(function (response) {
                return limitToFilter(response.data, 10);
            });
    };

    this.getContacts = function (args,  groupId) {
        groupId = groupId === undefined ? null : groupId;
        return $http.get('/Services/ContactService.svc/GetContacts?args=' + args, { noIndicator:true})
            .then(function (response) {
                if (groupId) return limitToFilter(response.data.filter(function (x) { return x.GroupId == groupId }), 10);
                return limitToFilter(response.data, 10);
            });
    };
    this.getContactsByID = function (id) {
        if (allContact) return allContact.filter(function (o) { return o.ContactId == key });
        return $http.get('/Services/ContactService.svc/GetAllContacts?id=' + id)
            .then(function (response) {
                return limitToFilter(response.data, 10);
            });
    };
    this.getContactsByGroup = function (groupId) {
        if (allContact) return allContact.filter(function (x) { return x.GroupId == groupId });
    };


            this.getContact = function (id, name) {
        if (allContact) return allContact.filter(function (o) { if (o.Name && name) return o.ContactId == id && o.Name.trim().toLowerCase() === name.trim().toLowerCase() })[0] || {};
        return {};
    };
    this.getContactById = function (id) {
        if (allContact) return allContact.filter(function (o) { return o.ContactId == id; })[0];
        return null;
    };

    this.getContactByName = function (name) {
        if (allContact) return allContact.filter(function (o) { if (o.Name && name) return o.Name.trim().toLowerCase() === name.trim().toLowerCase() })[0];
        return {};
    };


            this.getEntities = function (name, status) {
        status = status === undefined ? 'Available' : status;
        name = name ? '' : name;
        return $http.get('/Services/ContactService.svc/GetCorpEntityByStatus?n=' + name + '&s=' + status)
            .then(function (res) {
                return limitToFilter(res.data, 10);
            });
    };

        this.getTeamByName = function (teamName) {
        if (allTeam) {
            return allTeam.filter(function (o) { if (o.Name && teamName) return o.Name.trim() == teamName.trim() })[0];
        }
        return {};

    };
    }])
angular.module("PortalApp")
    .factory('ptEntityService', ['$http', function ($http) {
    return {
        getEntityByBBLE: function (bble, callback) {
            var url = '/api/CorporationEntities/ByBBLE?BBLE=' + bble;
            $http.get(url).then(function success(res) {
                if (callback) callback(null, res.data);
            }, function error(res) {
                if (callback) callback("load fail", res.data);
            });
        }
    };
}]);
angular.module("PortalApp").service('ptFileService', function () {

        this.isIE = function (fileName) {
        return fileName.indexOf(':\\') > -1;
    };
    this.getFileName = function (fullPath) {
        var paths;
        if (fullPath) {
            if (this.isIE(fullPath)) {
                paths = fullPath.split('\\');
                return this.cleanName(paths[paths.length - 1]);
            } else {
                paths = fullPath.split('/');
                return this.cleanName(paths[paths.length - 1]);
            }
        }
        return '';
    };
    this.getFileExt = function (fullPath) {
        if (fullPath && fullPath.indexOf('.') > -1) {
            var exts = fullPath.split('.');
            return exts[exts.length - 1].toLowerCase();
        }
        return '';
    };
    this.getFileFolder = function (fullPath) {
        if (fullPath) {
            var paths = fullPath.split('/');
            var folderName = paths[paths.length - 2];
            var topFolders = ['Construction', 'Title'];
            if (topFolders.indexOf(folderName) < 0) {
                return folderName;
            } else {
                return '';
            }
        }
        return '';
    };
    this.resetFileElement = function (el) {
        el.val('');
        el.wrap('<form>').parent('form').trigger('reset');
        el.unwrap();
        el.prop('files')[0] = null;
        el.replaceWith(el.clone());
    };
    this.cleanName = function (filename) {
        return filename.replace(/[^a-z0-9_\-\.()]/gi, '_');
    };
    this.getThumb = function (thumbId) {
        return '/downloadfile.aspx?thumb=' + thumbId;

    };
    this.trunc = function (fileName, length) {
        return _.trunc(fileName, length);

    };
    this.isPicture = function (fullPath) {
        var ext = this.getFileExt(fullPath);
        var pictureExts = ['jpg', 'jpeg', 'gif', 'bmp', 'png'];
        return pictureExts.indexOf(ext) > -1;
    };
    this.makePreviewUrl = function (filePath) {
        var ext = this.getFileExt(filePath);
        switch (ext) {
            case 'pdf':
                return '/pdfViewer/web/viewer.html?file=' + encodeURIComponent('/downloadfile.aspx?pdfUrl=') + encodeURIComponent(filePath);
                break;
            case 'xls':
            case 'xlsx':
            case 'doc':
            case 'docx':
                return '/downloadfile.aspx?fileUrl=' + encodeURIComponent(filePath) + '&edit=true';
                break;
            case 'jpg':
            case 'jpeg':
            case 'bmp':
            case 'gif':
            case 'png':
                return '/downloadfile.aspx?fileUrl=' + encodeURIComponent(filePath);
                break;
            default:
                return '/downloadfile.aspx?fileUrl=' + encodeURIComponent(filePath);

        }
    };
    this.onFilePreview = function (filePath) {
        var ext = this.getFileExt(filePath);
        switch (ext) {
            case 'pdf':
                window.open('/pdfViewer/web/viewer.html?file=' + encodeURIComponent('/downloadfile.aspx?pdfUrl=') + encodeURIComponent(filePath));
                break;
            case 'xls':
            case 'xlsx':
            case 'doc':
            case 'docx':
                window.open('/downloadfile.aspx?fileUrl=' + encodeURIComponent(filePath) + '&edit=true');
                break;
            case 'jpg':
            case 'jpeg':
            case 'bmp':
            case 'gif':
            case 'png':
                $.fancybox.open('/downloadfile.aspx?fileUrl=' + encodeURIComponent(filePath));
                break;
            case 'txt':
                $.fancybox.open([
                    {
                        type: 'ajax',
                        href: '/downloadfile.aspx?fileUrl=' + encodeURIComponent(filePath),
                    }
                ]);
                break;
            default:
                window.open('/downloadfile.aspx?fileUrl=' + encodeURIComponent(filePath));

        }
    };
    this.uploadFile = function (data, bbleORoptions, rename, folder, type, callback) {
        if (typeof bbleORoptions == 'string') {
            switch (type) {
                case 'construction':
                    this.uploadConstructionFile(data, bbleORoptions, rename, folder, callback);
                    break;
                case 'title':
                    this.uploadTitleFile(data, bbleORoptions, rename, folder, callback);
                    break;
                default:
                    this.uploadConstructionFile(data, bbleORoptions, rename, folder, callback);
                    break;
            }
        } else {
            var options = bbleORoptions;
            var url = options.url || '';
            var filename = options.filename || '';
            var callback = options.callback;
            if (!data || !url || !filename) {
                callback('error');
            }
            $.ajax({
                url: url,
                type: 'POST',
                data: data,
                cache: false,
                processData: false,
                success: function (data) {
                    callback(null, data, filename);
                },
                error: function (data) {
                    callback('fail to upload.', data, filename);
                }

            })
        }

           };
    this.uploadTitleFile = function (data, bble, rename, folder, callback) {
        var rename =  rename || '';
        var folder =  folder || '';
        if (!data || !bble) {
            callback('Upload infomation missing!');
        } else {
            bble = bble.trim();
            $.ajax({
                url: '/api/Title/UploadFiles?bble=' + bble + '&fileName=' + rename + '&folder=' + folder,
                type: 'POST',
                data: data,
                cache: false,
                processData: false,
                contentType: false,
                success: function (data1) {
                    callback(null, data1, rename);
                },
                error: function () {
                    callback('Upload fails!', null, rename);
                }
            });
        }
    };
    this.uploadConstructionFile = function (data, bble, rename, folder, callback) {
        var rename = rename || '';
        var folder = folder || '';
        if (!data || !bble) {
            callback('Upload infomation missing!');
        } else {
            bble = bble.trim();
            $.ajax({
                url: '/api/ConstructionCases/UploadFiles?bble=' + bble + '&fileName=' + rename + '&folder=' + folder,
                type: 'POST',
                data: data,
                cache: false,
                processData: false,
                contentType: false,
                success: function (data1) {
                    callback(null, data1, rename);
                },
                error: function () {
                    callback('Upload fails!', null, rename);
                }
            });
        }
    };
})
angular.module("PortalApp")
    .factory('ptHomeBreakDownService', ["$http", function ($http) {
    return {
        loadByBBLE: function (bble, callback) {
            var url = '/ShortSale/ShortSaleServices.svc/LoadHomeBreakData?bble=' + bble;
            $http.get(url)
                .success(function (data) {
                    callback(data);
                }).error(function () {
                    console.log('load home breakdown fail. BBLE: ' + bble);
                });
        },
        save: function (bble, data, callback) {
            var url = '/ShortSale/ShortSaleServices.svc/SaveBreakData';
            var postData = {
                "bble": bble,
                "jsonData": JSON.stringify(data)
            };
            $http.post(url, postData)
                .success(function (res) {
                    callback(res);
                }).error(function () {
                    console.log('save home breakdone fail. BBLE: ' + bble);
                });

        }
    };
}
    ])
angular.module("PortalApp")
    .service('ptLeadsService', ["$http", function ($http) {
    this.getLeadsByBBLE = function (bble, callback) {
        var leadsInfoUrl = "/ShortSale/ShortSaleServices.svc/GetLeadsInfo?bble=" + bble;
        $http.get(leadsInfoUrl)
        .success(function (data) {
            callback(data);
        }).error(function (data) {
            console.log("Get Short sale Leads failed BBLE =" + bble + " error : " + JSON.stringify(data));
        });
    };
}
    ])
angular.module("PortalApp")
    .factory('ptLegalService', function () {
    return {
        load: function (bble, callback) {
            var url = '/LegalUI/LegalUI.aspx/GetCaseData';
            var d = { bble: bble };
            $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify(d),
                dataType: 'json',
                contentType: "application/json",
                success: function (res) {
                    callback(null, res);
                },
                error: function () {
                    callback('load data fails');
                }
            });
        },
        savePreQuestions: function (bble, createBy, data, callback) {
            var url = '/LegalUI/LegalServices.svc/StartNewLegalCase';
            var d = {
                bble: bble,
                casedata: JSON.stringify({ PreQuestions: data }),
                createBy: createBy,
            };
            $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify(d),
                dataType: "json",
                contentType: "application/json",
                success: function (res) {
                    callback(null, res);
                },
                error: function () {
                    callback('load data fails');
                }
            });
        }
    };
    })
angular.module("PortalApp").factory('PortalHttpInterceptor', ['$log', '$q', '$timeout', 'ptCom', function ($log, $q, $timeout, ptCom) {
    $log.debug('$log is here to show you that this is a regular factory with injection');
    var myInterceptor = {
        delayHide: function () {
            $timeout(ptCom.stopLoading, 300);
        },
        BuildAjaxErrorMessage: function (response) {
            var message = "";
            if (response.status > 300 || response.status < 200 || response.status == 203) {
                var dataObj = JSON.parse(response.responseText);
                if (dataObj) {
                    var eMssage = dataObj.ExceptionMessage || dataObj.Message || dataObj.message;
                    var messageObj = { Message: eMssage };
                    message = myInterceptor.BuildErrorMessgeStr(messageObj);
                } else {
                    message = JSON.stringify(response)
                }
            }

            return message;
        },
        BuildErrorMessgeStr: function (messageObj) {

            return 'Error : ' + messageObj.Message || 'No Message' + '<br/> <small>(' + messageObj.urlName || 'No additional Info' + ')</small>';
        }, ErrorUrl: function (url) {
            return url.toString().replace(/\//g, ' ').replace('api', '');
        },
        BuildErrorMessage: function (data) {
            var urlName;
            if (data) {
                if (data.data) {
                    var messageObj = {
                        Message: data.data.ExceptionMessage || data.data.Message,
                        status: data.status,
                        statusText: data.statusText,
                        Url: data.config.url,
                        Method: data.config.method,
                    }
                    console.log(data);
                    urlName = messageObj.Url.toString().replace(/\//g, ' ').replace('api', '');

                    return 'Error : ' + messageObj.Message + '<br/> <small>(' + urlName + ')</small>';

                }
            }
            urlName = data.config.url.toString().replace(/\//g, ' ').replace('api', '');
            return 'Get error !' + '<br/> <small>( Status: ' + data.status + ' ' + urlName + ' )</small>';
        },
        request: function (config) {
            if (!config.noIndicator) {
                if (config.url.indexOf('template') < 0) {
                    ptCom.startLoading();
                }
            }

                         return config;
        },
        responseError: function (rejection) {
            myInterceptor.delayHide();
            try {
                var opt = rejection.config.options;
                if (!(opt && opt.noError)) {
                    ptCom.alert(myInterceptor.BuildErrorMessage(rejection));
                }
            }
            catch (error) {
                console.error(error);
            }
            return $q.reject(rejection);
        },

        requestError: function (rejection) {
            myInterceptor.delayHide();
            myInterceptor.alert(myInterceptor.BuildErrorMessage(rejection));
            return $q.reject(rejection);
        },

        response: function (response) {
            myInterceptor.delayHide();
            return response;
        },

    };

    return myInterceptor;
}]);
angular.module("PortalApp")
    .service('ptShortsSaleService', ['$http', function ($http) {
    this.getShortSaleCase = function (caseId, callback) {
        var url = "/ShortSale/ShortSaleServices.svc/GetCase?caseId=" + caseId;
        $http.get(url)
            .success(function (data) {
                callback(data);
            })
            .error(function (data) {
                console.log("Get Short sale failed CaseId= " + caseId + ", error : " + JSON.stringify(data));
            });
    };
    this.getShortSaleCaseByBBLE = function (bble, callback) {
        var url = "/ShortSale/ShortSaleServices.svc/GetCaseByBBLE?bble=" + bble;
        $http.get(url)
            .success(function (data) {
                callback(data);
            }).error(function () {
                console.log("Get Short Sale By BBLE fails.");
            }
        );

    };
    this.getBuyerTitle = function (bble, callback) {
        var url = "/api/ShortSale/GetBuyerTitle?bble=";
        $http.get(url + bble)
        .then(function succ(res) {
            if (callback) callback(null, res);
        }, function error() {
            if (callback) callback("Fail to get buyer title for bble: " + bble, null);
        });
    };
    }])
angular.module("PortalApp").service("ptTime", [function () {
    var that = this;

    this.isPassByDays = function (start, end, count) {
        var start_date = new Date(start);
        var end_date = new Date(end);

        var millisecondsPerDay = 1000 * 60 * 60 * 24;
        var millisBetween = end_date.getTime() - start_date.getTime();
        var days = millisBetween / millisecondsPerDay;

        if (days > count) {
            return true;
        }

        return false;
    }
    this.isPassOrEqualByDays = function (start, end, count) {
        var start_date = new Date(start);
        var end_date = new Date(end);

        var millisecondsPerDay = 1000 * 60 * 60 * 24;
        var millisBetween = end_date.getTime() - start_date.getTime();
        var days = millisBetween / millisecondsPerDay;

        if (days >= count) {
            return true;
        }

        return false;
    }
    this.isLessOrEqualByDays = function (start, end, count) {
        var start_date = new Date(start);
        var end_date = new Date(end);

        var millisecondsPerDay = 1000 * 60 * 60 * 24;
        var millisBetween = end_date.getTime() - start_date.getTime();
        var days = millisBetween / millisecondsPerDay;

        if (days >= 0 && days <= count) {
            return true;
        }

        return false;
    }
    this.isPassByMonths = function (start, end, count) {
        var start_date = new Date(start);
        var end_date = new Date(end);
        var months = (end_date.getFullYear() - start_date.getFullYear()) * 12 + end_date.getMonth() - start_date.getMonth();

        if (months > count) return true;
        else return false;
    }
    this.isPassOrEqualByMonths = function (start, end, count) {
        var start_date = new Date(start);
        var end_date = new Date(end);
        var months = (end_date.getFullYear() - start_date.getFullYear()) * 12 + end_date.getMonth() - start_date.getMonth();

        if (months >= count) return true;
        else return false;
    }

    }])

angular.module("PortalApp").filter('booleanToString', function () {

    return function (v) {
        if (v == undefined) return "N/A"
        else if (v) return "Yes"
        else return "No"
    }
})

angular.module("PortalApp").filter("ByContact", function () {
    return function (movies, contact) {
        var items = {

            out: []
        };
        if ($.isEmptyObject(contact) || contact.Type === null) {
            return movies;
        }
        angular.forEach(movies, function (value, key) {
            if (value.Type === contact.Type) {
                if (contact.CorpName === '' || contact.CorpName === value.CorpName) {
                    items.out.push(value);
                }
            }
        });
        return items.out;
    };
})
angular.module("PortalApp").filter('percentage', function ($filter) {

    return function (v) {
        if (v) {
            vf = parseFloat(v);
            return $filter('number')(v * 100.0, 2) + "%";

        }

    }
})

angular.module("PortalApp")
.filter('unsafe', ['$sce', function ($sce) { return $sce.trustAsHtml; }])
angular.module("PortalApp")
    .directive('auditLogs', ['AuditLog', function (AuditLog) {
        return {
            restrict: 'E',
            templateUrl: '/js/directives/AuditLogs.tpl.html',
            scope: {
                tableName: '@',
                recordId: '=',
            },
            link: function (scope, el, attrs) {
                setTimeout(function () {
                    AuditLog.load({ TableName: scope.tableName, RecordId: scope.recordId }, function (data) {
                        var result = _.groupBy(data, function (item) {
                            return item.EventDate;
                        });
                        scope.AuditLogs = result;
                    })
                }, 1000);               
            }
        }
    }])
angular.module("PortalApp")
    .directive('bindId', ['ptContactServices', function (ptContactServices) {
        return {
            restrict: 'A',
            link: function postLink(scope, el, attrs) {
                scope.$watch(attrs.bindId, function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        var contact = ptContactServices.getContactById(newValue);
                        if (contact) scope.$eval(attrs.ngModel + "='" + contact.Name + "'");
                    }
                });
            }

        }
    }])
angular.module("PortalApp")
    .directive('initGrid', ['$parse', function ($parse) {
        return {
            link: function (scope, element, attrs, ngModelController) {
                var gridOptions = null;
                eval("gridOptions =" + attrs.dxDataGrid);
                if (gridOptions) {
                    var option = gridOptions.bindingOptions.dataSource;
                    var array = scope.$eval(option);

                    if (array == null || array == undefined)
                        eval('scope.' + option + '=[];');

                    scope.$watch(attrs.initGrid, function (newValue) {
                        var array = scope.$eval(option);
                        if (array == null || array == undefined)
                            eval('scope.' + option + '=[];');
                    });
                }
            }
        };
    }]);
angular.module("PortalApp")
    .directive('preCondition', function () {
        return {
            require: 'ngModel',           
            link: function (scope, element, attrs, ngModelController) {
                scope.$watch(attrs.preCondition, function (newVal, oldVal) {
                    if (!newVal)
                        eval('scope.' + attrs.ngModel + '=null');                  
                }, true);

            }
        };
    })
angular.module("PortalApp")
    .directive('ptAdd', function () {
        return {
            restrict: 'E',
            template: '<i class="fa fa-plus-circle icon_btn text-primary tooltip-examples" title="Add"></i>',
        }
    })
angular.module("PortalApp")
    .directive('ptCollapse', function () {
        return {
            restrict: 'E',
            template: '<i class="fa fa-compress icon_btn text-primary" ng-show="!model" ng-click="model=!model"></i>' +
                '<i class="fa fa-expand icon_btn text-primary" ng-show="model" ng-click="model=!model"></i>',
            scope: {
                model: '=',
            },
            link: function postLink(scope, el, attrs) {
                var bVal = scope.model;
                scope.model = bVal === undefined ? false : bVal;
            }

        }
    })
angular.module("PortalApp")
    .directive('ptDate', function () {
        return {
            restrict: 'A',
            scope: true,
            compile: function (tel, tAttrs) {
                return {
                    post: function (scope, el, attrs) {
                        $(el).datepicker({
                            forceParse: false,
                        });
                        scope.$watch(attrs.ngModel, function (newValue, oldValue) {
                            var dateStr = newValue;
                            if (dateStr && typeof dateStr === 'string' && dateStr.indexOf('T') > -1) {
                                var dd = new Date(dateStr);
                                dd = (dd.getUTCMonth() + 1) + '/' + (dd.getUTCDate()) + '/' + dd.getUTCFullYear();
                                $(el).datepicker('update', new Date(dd))
                            }
                        });
                    }
                }
            }
        };
    })
angular.module("PortalApp")
    .directive('ptDel', function () {
        return {
            restrict: 'E',
            template: '<i class="fa fa-times icon_btn text-danger tooltip-examples" title="Delete"></i>',
        }
    })
angular.module("PortalApp")
    .directive('ptEditableDiv', [function () {
        return {
            restrict: 'A',
            scope: {
                ptLock: '='
            },
            link: function (scope, el, attrs) {
                angular.element(el).addClass("pt-editable-div");
                scope.isLocked = true;
                scope.unlock = function () {
                    angular.element(".pt-editable-div input, .pt-editable-div textarea, .pt-editable-div select").prop('disabled', false);
                    scope.isLocked = false;
                }
                scope.lock = function () {
                    angular.element(".pt-editable-div input, .pt-editable-div textarea, .pt-editable-div select").prop('disabled', true);
                    scope.isLocked = true;
                }
                scope.$on('pt-editable-div-lock', function () {
                    scope.lock();
                })
                scope.$on('pt-editable-div-unlock', function () {
                    scope.unlock();
                })
                if (scope.ptLock) {
                    scope.lock();
                }
            }
        }
    }])
angular.module("PortalApp")
    .directive('ptEditor', [function () {
        return {

            templateUrl: '/js/directives/ptEditor.html',
            require: 'ngModel',
            scope: {
                ptModel: '=ngModel'
            },
            link: function (scope, el, attrs, ctrl) {
                scope.contentShown = true;
                var ckdiv = $(el).find("div.ptEditorCK")[0];
                var ck = CKEDITOR.replace(ckdiv, {
                    allowedContent: true,
                    height: 400,
                });
                scope.editorShown = false;

                scope.showCK = function () {
                    scope.contentShown = false;
                    scope.editorShown = true;
                }
                scope.closeCK = function () {
                    scope.contentShown = true;
                    scope.editorShown = false;
                }

                ck.on('pasteState', function () {
                    scope.$apply(function () {
                        ctrl.$setViewValue(ck.getData());
                    });
                });

                ctrl.$render = function (value) {
                    ck.setData(ctrl.$modelValue);
                };

                ck.setData(ctrl.$modelValue);
            }
        };
    }])
angular.module("PortalApp")
    .directive('ptFile', ['ptFileService', '$timeout', 'ptCom',function (ptFileService, $timeout, ptCom) {
        return {
            restrict: 'E',
            templateUrl: '/js/directives/ptFile.tpl.html',
            scope: {
                fileModel: '=',
                fileBble: '=',  
                uploadType: '@',
                uploadUrl: '@',
                fileName: '@', 
                disableDelete: '=?',
                disableModify: '=?',
                ngDisabled: '=?'
            },
            link: function (scope, el, attrs) {
                scope.ptFileService = ptFileService;
                scope.fileId = "ptFile" + scope.$id;
                var mode = 0; 
                debugger;
                if (attrs['fileBble'] == undefined) {
                    mode = 1; 
                }
                scope.uploadType = scope.uploadType || 'construction';
                scope.ngDisabled = scope.ngDisabled || false;
                scope.disableModify = scope.disableModify || scope.ngDisabled;
                scope.disableDelete = scope.disableDelete || scope.ngDisabled;
                scope.fileChoosed = false;
                scope.loading = false;

                scope.delFile = function () {
                    scope.fileModel = null;
                }
                scope.delChoosed = function () {
                    scope.File = null;
                    scope.fileChoosed = false;
                    var fileEl = el.find('input:file')[0]
                    fileEl.value = ''
                }
                scope.toggleLoading = function () {
                    scope.loading = !scope.loading;
                }
                scope.startLoading = function () {
                    scope.loading = true;
                }
                scope.stopLoading = function () {
                    $timeout(function () {
                        scope.loading = false;
                    });
                }
                scope.uploadFile = function () {
                    debugger;
                    scope.startLoading();
                    var data = new FormData();
                    data.append("file", scope.File);
                    var filename = ptFileService.getFileName(scope.File.name); 
                    var rename = scope.fileName || '';
                    var callback = function (error, data) {
                        scope.stopLoading();
                        if (error) {
                            ptCom.alert(error);
                        } else {
                            scope.$apply(function () {
                                scope.fileModel = {}
                                scope.fileModel.path = data[0];
                                scope.fileModel.thumb = data[1] || '';
                                scope.fileModel.name = ptFileService.getFileName(scope.fileModel.path);
                                scope.fileModel.uploadTime = new Date();
                                scope.delChoosed();
                            });
                        }

                    } 
                    if (mode == 0) {
                        ptFileService.uploadFile(data, scope.fileBble, filename , rename , scope.uploadType, callback);
                    } else {
                        ptFileService.uploadFile(data, {
                            url: scope.uploadUrl,
                            filename: rename || filename,
                            callback: callback
                        })
                    }
                }
                el.find('input:file').bind('change', function () {
                    var file = this.files[0];
                    if (file) {
                        scope.$apply(function () {
                            scope.File = file;
                            scope.fileChoosed = true;
                        });
                    }
                });
                scope.modifyName = function (model) {
                    if (model) {
                        scope.ModifyNamePop = true;
                        scope.NewFileName = model.name ? model.name : '';
                        scope.editingFileModel = model;
                        scope.editingFileExt = ptFileService.getFileExt(scope.NewFileName);
                    }

                }
                scope.onModifyNamePopClose = function () {
                    scope.NewFileName = '';
                    scope.editingFileModel = null;
                    scope.editingFileExt = '';
                    scope.ModifyNamePop = false;

                }
                scope.onModifyNamePopSave = function () {
                    if (scope.NewFileName) {
                        if (scope.NewFileName.indexOf('.') > -1) {
                            scope.editingFileModel.name = scope.NewFileName;
                        } else {
                            scope.editingFileModel.name = scope.NewFileName + '.' + scope.editingFileExt;
                        }
                    }
                    scope.editingFileModel = null;
                    scope.editingFileExt = '';
                    scope.ModifyNamePop = false;
                }
            }
        }
    }])
angular.module("PortalApp")
    .directive('ptFiles', ['$timeout', 'ptFileService', 'ptCom', function ($timeout, ptFileService, ptCom) {
        return {
            restrict: 'E',
            templateUrl: '/js/directives/ptFiles.tpl.html',
            scope: {
                fileModel: '=',
                fileBble: '=',
                fileId: '@',
                fileColumns: '@',
                folderEnable: '@',
                baseFolder: '@',
                uploadType: '@' 
            },
            link: function (scope, el, attrs) {
                scope.ptFileService = ptFileService;
                scope.ptCom = ptCom;

                scope.files = []; 
                scope.columns = []; 
                scope.nameTable = []; 
                scope.currentFolder = '';
                scope.showFolder = false;
                scope.uploadType = scope.uploadType || 'construction';
                scope.loading = false;
                scope.baseFolder = scope.baseFolder ? scope.baseFolder : '';
                scope.count = 0; 


                if (scope.fileColumns) {
                    scope.columns = scope.fileColumns.split('|');
                    _.each(scope.columns, function (elm) {
                        elm.trim();
                    });
                }

                scope.folders = _.without(_.uniq(_.pluck(scope.fileModel, 'folder')), undefined, '')

                scope.$watch('fileModel', function () {
                    scope.currentFolder = '';
                    scope.baseFolder = scope.baseFolder ? scope.baseFolder : '';
                    scope.folders = _.without(_.uniq(_.pluck(scope.fileModel, 'folder')), undefined, '')
                })

                $(el).find('input:file').change(function () {
                    var files = this.files;
                    scope.addFiles(files);
                    this.value = '';
                });

                $(el).find('.drop-area')
                    .on('dragenter', function (e) {
                        e.preventDefault();
                        $(this).addClass('drop-area-hover');
                    })
                    .on('dragover', function (e) {
                        e.preventDefault();
                        $(this).addClass('drop-area-hover');
                    })
                    .on('dragleave', function (e) {
                        $(this).removeClass('drop-area-hover');
                    })
                    .on('drop', function (e) {
                        e.stopPropagation();
                        e.preventDefault();
                        $(this).removeClass('drop-area-hover');
                        scope.OnDropTextarea(e);
                        debugger;
                    });

                scope.OnDropTextarea = function (event) {
                    if (event.originalEvent.dataTransfer) {
                        var files = event.originalEvent.dataTransfer.files;
                        scope.addFiles(files);
                    } else {
                        alert("Your browser does not support the drag files.");
                    }
                }

                scope.changeFolder = function (folderName) {
                    scope.currentFolder = folderName;
                    scope.showFolder = true;
                }
                scope.addFolder = function (folderName) {
                    if (scope.folders.indexOf(folderName) < 0) {
                        scope.folders.push(folderName);
                    }
                    scope.currentFolder = folderName;
                    scope.showFolder = true;
                }
                scope.hideFolder = function () {
                    scope.currentFolder = "";
                    scope.showFolder = false;
                }
                scope.toggleNewFilePop = function () {
                    scope.NewFolderPop = !scope.NewFolderPop
                    scope.NewFolderName = '';
                }
                scope.newFolderPopSave = function () {
                    scope.addFolder(scope.NewFolderName);
                    scope.NewFolderPop = false;
                }

                scope.addFiles = function (files) {
                    for (var i = 0; i < files.length; i++) {
                        var file = files[i];
                        scope.$apply(function () {
                            if (scope.nameTable.indexOf(file.name) < 0) {
                                scope.files.push(file);
                                scope.nameTable.push(file.name);
                            }
                        });
                    }
                }

                scope.removeChoosed = function (index) {
                    scope.nameTable.splice(scope.nameTable.indexOf(scope.files[index].name), 1);
                    scope.files.splice(index, 1);
                }

                scope.clearChoosed = function () {
                    scope.nameTable = [];
                    scope.files = [];
                }

                scope.showUpoading = function () {
                    scope.uploadProcess = true;
                    scope.dynamic = 1;
                }

                scope.hideUpoading = function () {
                    scope.clearChoosed();
                    scope.uploadProcess = false;
                }

                scope.showUploadErrors = function () {
                    var error = _.some(scope.result, function (el) {
                        return el.error
                    });
                    return !scope.uploading && error;
                }


                scope.uploadFile = function () {

                    var targetFolder = (scope.baseFolder ? scope.baseFolder + '/' : '') + (scope.currentFolder ? scope.currentFolder + '/' : '')
                    var len = scope.files.length;

                    scope.fileModel = scope.fileModel ? scope.fileModel : [];
                    scope.result = []; 

                    scope.showUpoading();
                    scope.uploading = true;

                    for (var i = 0; i < len; i++) {
                        var f = {};
                        f.name = ptFileService.getFileName(scope.files[i].name);
                        f.folder = scope.currentFolder;
                        f.uploadTime = new Date();
                        for (var j = 0; j < scope.columns.length; j++) {
                            var column = scope.columns[j];
                            f[column] = '';
                        }
                        scope.result.push(f);
                    }

                    for (var j = 0; j < len; j++) {
                        var data = new FormData();
                        data.append("file", scope.files[j]);
                        var targetName = ptFileService.getFileName(scope.files[j].name);
                        ptFileService.uploadFile(data, scope.fileBble, targetName, targetFolder, scope.uploadType, function callback(error, data, targetName) {
                            var targetElement;
                            if (error) {
                                scope.countCallback(len);
                                targetElement = _.filter(scope.result, function (el) {
                                    return el.name == targetName
                                })[0];
                                if (targetElement) {
                                    targetElement.error = error;
                                }

                            } else {
                                scope.countCallback(len);
                                targetElement = _.filter(scope.result, function (el) {
                                    return el.name == targetName
                                })[0];
                                if (targetElement) {
                                    targetElement.path = data[0];
                                    if (data[1]) {
                                        targetElement.thumb = data[1];
                                    }
                                }
                                scope.fileModel.push(targetElement);
                            }
                        });
                    }

                }

                scope.countCallback = function (total) {
                    if (scope.count >= total - 1) {
                        $timeout(function () {
                            scope.count = scope.count + 1;
                            scope.dynamic = Math.floor(scope.count / total * 100);
                            scope.count = 0;
                            scope.uploading = false;
                            scope.clearChoosed();
                        });
                    } else {
                        $timeout(function () {
                            scope.count = scope.count + 1;
                            scope.dynamic = Math.floor(scope.count / total * 100);
                        })
                    }
                }

                scope.modifyName = function (mdl, indx) {
                    if (mdl[indx]) {
                        scope.ModifyNamePop = true;
                        scope.NewFileName = mdl[indx].name ? mdl[indx].name : '';
                        scope.editingFileModel = mdl;
                        scope.editingIndx = indx;
                        scope.editingFileExt = ptFileService.getFileExt(scope.NewFileName);
                    }

                }

                scope.onModifyNamePopClose = function () {
                    scope.NewFileName = '';
                    scope.editingFileModel = null;
                    scope.editingIndx = null;
                    scope.ModifyNamePop = false;
                    scope.editingFileExt = '';
                }

                scope.onModifyNamePopSave = function () {
                    if (scope.NewFileName) {
                        if (scope.NewFileName.indexOf('.') > -1) {
                            scope.editingFileModel[scope.editingIndx].name = scope.NewFileName;
                        } else {
                            scope.editingFileModel[scope.editingIndx].name = scope.NewFileName + '.' + scope.editingFileExt;
                        }
                    }
                    scope.editingFileModel = null;
                    scope.editingIndx = null;
                    scope.ModifyNamePop = false;
                    scope.editingFileExt = '';
                }

                scope.getThumb = function (model) {
                    if (model && model.thumb) {
                        return ptFileService.getThumb(model.thumb);
                    } else {
                        return '/images/no_image.jpg';
                    }

                }

                scope.fancyPreview = function (file) {
                    if (ptFileService.isPicture(file.name)) {
                        $.fancybox.open(ptFileService.makePreviewUrl(file.path));
                    }
                }

                scope.filterError = function (v) {
                    return v.error;
                }

            }

        }

    }])
angular.module("PortalApp")
    .directive('ptFinishedMark', [function () {
        return {
            restrict: 'E',
            template: '<span ng-if="ssStyle==0">' + '<button type="button" class="btn btn-default" ng-show="!ssModel" ng-click="confirm()">{{text1?text1:"Confirm"}}</button>' + '<button type="button" class="btn btn-success" ng-show="ssModel" ng-dblclick="deconfirm()">{{text2?text2:"Complete"}}&nbsp<i class="fa fa-check-circle"></i></button>' + '</span>' + '<span ng-if="ssStyle==1">' + '<span class="label label-default" ng-show="!ssModel" ng-click="confirm()">{{text1?text1:"Confirm"}}</span>' + '<span class="label label-success" ng-show="ssModel" ng-dblclick="deconfirm()">{{text2?text2:"Complete"}}&nbsp<i class="fa fa-check-circle"></i></span>' + '</span>',
            scope: {
                ssModel: '=',
                text1: '@',
                text2: '@',
                ssStyle: '@'
            },
            link: function (scope, el, attrs) {
                if (!scope.ssModel) scope.ssModel = false;
                if (scope.ssStyle && scope.ssStyle.toLowerCase() === 'label') {
                    scope.ssStyle = 1;
                } else {
                    scope.ssStyle = 0;
                }
                scope.confirm = function () {
                    scope.ssModel = true;
                }
                scope.deconfirm = function () {
                    scope.ssModel = false;
                }
            }
        }
    }])
angular.module("PortalApp")
    .directive('ptInitBind', function () { 
        return {
            restrict: 'A',
            require: '?ngBind',
            link: function (scope, el, attrs) {
                scope.$watch(attrs.ptInitBind, function (newVal) {
                    if (!scope.$eval(attrs.ngBind) && newVal) {
                        if (typeof newVal === 'string') newVal = newVal.replace(/'/g, "\\'");
                        scope.$eval(attrs.ngBind + "='" + newVal + "'");
                    }
                });
            }
        }
    })
angular.module("PortalApp")
    .directive('ptInitModel', function () {
        return {
            restrict: 'A',
            require: '?ngModel',
            priority: 99,
            link: function (scope, el, attrs) {
                scope.$watch(attrs.ptInitModel, function (newVal) {
                    if (!scope.$eval(attrs.ngModel) && newVal) {
                        if (typeof newVal === 'string') newVal = newVal.replace(/'/g, "\\'");
                        scope.$eval(attrs.ngModel + "='" + newVal + "'");
                    }
                });
            }
        }
    })
angular.module("PortalApp")
    .directive('ptInputMask', function () {
        return {
            restrict: 'A',
            link: function (scope, el, attrs) {
                $(el).mask(attrs.inputMask);
                $(el).on('change', function () {
                    scope.$eval(attrs.ngModel + "='" + el.val() + "'");
                });
            }
        };
    })
angular.module("PortalApp")
    .directive('ptLink', ['ptFileService', function (ptFileService) {
        return {
            restrict: 'E',
            scope: {
                ptModel: '='
            },
            template: '<a ng-click="onFilePreview(ptModel.path)">{{trunc(ptModel.name,20)}}</a>',
            link: function (scope, el, attrs) {
                scope.onFilePreview = ptFileService.onFilePreview;
                scope.trunc = ptFileService.trunc;
            }

        }
    }])
angular.module("PortalApp")
    .directive('ptNumberMask', function () {
        return {
            restrict: 'A',
            link: function (scope, el, attrs) {
                var isValidate = attrs.hasOwnProperty('isvalidate');
                var format = attrs['maskformat'] || '';
                var formatConfig;
                switch (format) {
                    case 'integer':
                        formatConfig = {
                            symbol: "",
                            roundToDecimalPlace: 0
                        }
                        break;
                    case 'money':
                        formatConfig = {

                        }
                        break;
                    case 'percentage':
                        formatConfig = {
                            symbol: "%",
                            positiveFormat: '%n%s',
                            negativeFormat: '(%n%s)'
                        }
                        break;
                    default:
                        formatConfig = {
                            symbol: ""
                        }

                }
                var rule = /^-?(\d+|\d*\.\d+)$/;
                var validate = function (val) {
                    if (typeof (val) == 'number') {
                        return true;
                    } else if (typeof (val) == 'string') {
                        return !!rule.exec(val);
                    } else {
                        return false;
                    }

                }

                scope.$watch(attrs.ngModel, function (newvalue) {
                    if ($(el).is(":focus")) return;
                    if (format == 'percentage') {
                        $(el)[0].value = newvalue * 100;
                    }
                    $(el).formatCurrency(formatConfig);
                });
                $(el).on('blur', function () {
                    debugger;
                    if (isValidate) {
                        var res = validate(this.value);
                        if (!res) {
                            $(this).css("background-color", "yellow");
                            $(this).attr('error', 'true');

                        } else {
                            $(this).css("background-color", "");
                            $(this).attr('error', '');
                            if (format == 'percentage') {
                                $(el)[0].value = $(el)[0].value * 100;
                            }
                            $(this).formatCurrency(formatConfig);
                        }
                    } else {
                        if (format == 'percentage') {
                            $(el)[0].value = $(el)[0].value * 100;
                        }
                        $(this).formatCurrency(formatConfig);
                    }
                });
                $(el).on('focus', function () {

                    $(this).toNumber();
                    if (format == 'percentage') {
                        $(el)[0].value = $(el)[0].value / 100;
                    }
                });
            },
        };
    })
angular.module("PortalApp")
    .directive('ptNumberMaskPatch', function () {
        return {
            priority: 1,
            restrict: 'A',
            link: function (scope, el, attrs) {
                var format = attrs['maskformat'] || '';
                scope.$watch(attrs.ngModel, function (newvalue) {
                    if ($(el).is(":focus")) return;
                    if (format == 'money') {
                        if (typeof newvalue == 'string') {
                            var cleanedvalue = newvalue.replace("$", "").replace(",", "")
                            angular.element(el).scope().$eval(attrs.ngModel + "='" + cleanedvalue + "'")
                        }
                    }
                });
                $(el).on('blur', function () {
                    if (format == 'money') {
                        if (typeof this.value == 'string') {
                            var cleanedvalue = this.value.replace("$", "").replace(",", "");
                            if (cleanedvalue.length != this.value.length) {
                                var targetScope = angular.element(el).scope();
                                targetScope.$eval(attrs.ngModel + "='" + cleanedvalue + "'");
                                targetScope.$apply();
                            }
                        }
                    }
                })
            }
        }
    })

angular.module("PortalApp")
    .directive('ptRadio', function () {
        return {
            restrict: 'E',
            template: '<input type="checkbox" id="{{name}}Y" ng-model="model" class="ss_form_input" ng-disabled="ngDisabled">' +
                '<label for="{{name}}Y" class="input_with_check"><span class="box_text">{{trueValue}}&nbsp</span></label>' +
                '<input type="checkbox" id="{{name}}N" ng-model="model" ng-true-value="false" ng-false-value="true" class="ss_form_input" ng-disabled="ngDisabled">' +
                '<label for="{{name}}N" class="input_with_check"><span class="box_text">{{falseValue}}&nbsp</span></label>',
            scope: {
                model: '=',
                name: '@',
                defaultValue: '@',
                trueValue: '@',
                falseValue: '@',
                ngDisabled: '='
            },
            link: function (scope, el, attrs) {
                scope.trueValue = scope.trueValue ? scope.trueValue : 'yes';
                scope.falseValue = scope.falseValue ? scope.falseValue : 'no';
                scope.defaultValue = scope.defaultValue === 'true' ? true : false;
                if (typeof scope.model != 'undefined') {
                    scope.model = scope.model == null ? scope.defaultValue : scope.model;
                }

            }

        }
    })
angular.module("PortalApp")
    .directive('ptRadioInit', function () {
        return {
            restrict: 'A',
            link: function (scope, el, attrs) {
                scope.$eval(attrs.ngModel + "=" + attrs.ngModel + "==null?" + attrs.radioInit + ":" + attrs.ngModel);
                scope.$watch(attrs.ngModel, function () {
                    var bVal = scope.$eval(attrs.ngModel);
                    bVal = bVal != null && (bVal == 'true' || bVal == true);
                    scope.$eval(attrs.ngModel + "=" + bVal.toString());
                });
            }
        }
    })
angular.module("PortalApp")
    .directive('ptRequired', function () {
        return {
            restrict: 'A',
            link: function (scope, el, attrs) {
                var eltype = $(el)[0].type;

                if (eltype != 'text' && eltype != 'textarea' && eltype != 'select-one') {
                    return;
                }


                var validate = function (v) {
                    if (eltype == 'text' || eltype == 'textarea') {
                        if (v && typeof v == 'string' && v.trim().length > 0) {
                            return true;
                        } else {
                            return false
                        }
                    } else if (eltype == 'select-one') {
                        return v == undefined || (typeof v == 'string' && (v.trim().length == 0 || v.indexOf('?') == 0)) ? false : true;
                    } else {
                        return false;
                    }
                }

                var callback = function () {
                    var res = validate($(el)[0].value);
                    if (!res) {
                        $(el).css("background-color", "yellow");
                        $(el).attr('error', 'true');
                        if ($(el)[0].type == 'text' || $(el)[0].type == 'textarea') {
                            $(el)[0].placeholder = 'content is required.'
                        }

                    } else {
                        $(el).css("background-color", "");
                        $(el).attr('error', '');
                        if ($(el)[0].type == 'text' || $(el)[0].type == 'textarea') {
                            $(el)[0].placeholder = ''
                        }
                    }
                }

                $(el).on('blur', callback);
                scope.$on('ptSelfCheck', callback);

            },
        };
    })
angular.module("PortalApp")
.controller('BuyerEntityCtrl', ['$scope', '$http', 'ptContactServices', 'CorpEntity', function ($scope, $http, ptContactServices, CorpEntity) {
    $scope.EmailTo = [];
    $scope.EmailCC = [];
    $scope.ptContactServices = ptContactServices;
    $scope.selectType = 'All Entities';
    $scope.loadPanelVisible = true;
    $scope.encodeURIComponent = window.encodeURIComponent;

    $scope.CorpEntites = CorpEntity.query(function () {
        $scope.currentContact = $scope.CorpEntites[0];
        $scope.loadPanelVisible = false;
    }, function () {
        alert('Get All buyers Entities error : ' + JSON.stringify(data));
    });

        $http.get('/Services/TeamService.svc/GetAllTeam')
        .success(function (data) {
            $scope.AllTeam = data;
        }).error(function (data) {
            alert('Get All Team name  error : ' + JSON.stringify(data));
        });
    $scope.Groups = [
        { GroupName: 'All Entities' },
        { GroupName: 'Available' },
        { GroupName: 'Assigned Out' },
        {
            GroupName: 'Current Offer',
            SubGroups:
            [
                { GroupName: 'NHA Current Offer' }, { GroupName: 'Isabel Current Offer' },
                { GroupName: 'Quiet Title Action' }, { GroupName: 'Deed Purchase' },
                { GroupName: 'Straight Sale' }, { GroupName: 'Jay Current Offer' }
            ]
        },
        {
            GroupName: 'Sold',
            SubGroups: [
                { GroupName: 'Purchased' }, { GroupName: 'Partnered' },
                { GroupName: 'Sold (Final Sale)/Recyclable' }
            ]
        },
        { GroupName: 'In House' },
        { GroupName: 'Agent Corps' },
        { GroupName: 'Not for Use' },
        { GroupName: 'Reserve' }
    ];

    $scope.ChangeGroups = function (name) {
        $scope.selectType = name;
    }
    $scope.GetTitle = function () {
        return ($scope.SelectedTeam ? ($scope.SelectedTeam === "" ? "All Team's " : $scope.SelectedTeam + "s ") : "") + $scope.selectType;
    }
    $scope.ExportExcel = function () {
        JSONToCSVConvertor($scope.filteredCorps, true, $scope.GetTitle());

    }
    $scope.GroupCount = function (g) {
        if (!$scope.CorpEntites) {
            return 0;
        }
        if (g.GroupName == 'All Entities') {
            if ($scope.SelectedTeam) {
                return $scope.CorpEntites.filter(function (o) { return o.Office && o.Office.toLowerCase().trim() == $scope.SelectedTeam.toLowerCase().trim() }).length;
            }
            return $scope.CorpEntites.length;
        }
        var count = 0
        if (g.SubGroups) {
            for (var i = 0; i < g.SubGroups.length; i++) {
                count += $scope.GroupCount(g.SubGroups[i]);
            }
            return count
        }
        var corps = $scope.CorpEntites.filter(function (o) { return (o.Status && o.Status.toLowerCase().trim() == g.GroupName.toLowerCase().trim()) });
        if ($scope.SelectedTeam) {
            corps = corps.filter(function (o) { return o.Office && o.Office.toLowerCase().trim() == $scope.SelectedTeam.toLowerCase().trim() });
        }
        return corps.length;
    }
    $scope.lookupDataSource = new DevExpress.data.ODataStore({
        url: "/odata/LeadsInfoes",
        key: "CategoryID",
        keyType: "Int32"
    });
    $scope.TeamCount = function (teamName) {
        if (!$scope.CorpEntites) {
            return 0;
        }
        var crops = [];
        crops = teamName ? $scope.CorpEntites.filter(function (o) { return o.Office && o.Office.toLowerCase().trim() == teamName.toLowerCase().trim() }) : $scope.CorpEntites;


        if ($scope.selectType && $scope.selectType != $scope.Groups[0].GroupName) {
            crops = crops.filter(function (o) { return o.Status && o.Status.toLowerCase().trim() == $scope.selectType.toLowerCase().trim() })
        }

        return crops.length;
    }
    $scope.EmployeeDataSource = function () {
        var employees = $scope.ptContactServices.getContactsByGroup(4);

        var mSource = new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                if (loadOptions.searchValue) {
                    var q = loadOptions.searchValue;
                    return employees.filter(function (e) { e.Email && (e.Email.toLowerCase() == q.toLowerCase() || e.Name.toLowerCase() == q.toLowerCase()) }).slice(0, 10);
                }
                return employees.slice(0, 10);
            },
            byKey: function (key, extra) {
            },


        });
        return {
            dataSource: mSource,
            searchEnabled: true,
            placeholder: 'Type to Search',
            displayExpr: 'Email',
            valueExpr: 'Email',
            bindingOptions: {
                values: 'EmailTo'
            }
        };
    }
    $scope.EntitiesFilter = function (entity) {
        if ($scope.selectType == 'All Entities' || (entity.Status && $scope.selectType.toLowerCase().trim() == entity.Status.toLowerCase().trim()))
            return true;
        var subs = $scope.Groups.filter(function (o) { return o.GroupName == $scope.selectType })[0];
        if (subs && subs.SubGroups) {
            for (var i = 0; i < subs.SubGroups.length; i++) {
                var sg = subs.SubGroups[i];
                if (entity.Status && sg.GroupName.toLowerCase().trim() == entity.Status.toLowerCase().trim()) {
                    return true;
                }
            }
        }

        return false;
    }
    $scope.selectCurrent = function (contact) {
        $scope.currentContact = contact;
    }
    $scope.SaveCurrent = function () {
        $scope.loadPanelVisible = true;
        $http.post('/Services/ContactService.svc/SaveCorpEntitiy', { c: JSON.stringify($scope.currentContact) }).success(function (data, status, headers, config) {
            $scope.loadPanelVisible = false;
            alert("Save succeed!")
        }).error(function (data, status, headers, config) {
            alert("Get error save corp entitiy : " + JSON.stringify(data))
            $scope.loadPanelVisible = false;
        });
    }
    $scope.AllGroups = function () {
        var HasSub = $scope.Groups.filter(function (o) { return o.SubGroups != null });
        var groups = [];
        for (var i = 0; i < HasSub.length; i++) {
            groups = groups.concat(HasSub[i].SubGroups);
        }
        var HasNotSub = $scope.Groups.filter(function (o) { return o.SubGroups == null && o.GroupName != 'All Entities' });
        groups = groups.concat(HasNotSub);
        return groups;
    }
    $scope.addContactFunc = function () {
        $scope.loadPanelVisible = true;
        $http.post('/Services/ContactService.svc/SaveCorpEntitiy', { c: JSON.stringify($scope.addContact) }).success(function (data, status, headers, config) {
            $scope.loadPanelVisible = false;
            if (!data) {
                alert("Already have a entitity named " + $scope.addContact.CorpName + "! please pick other name");
                return;
            }
            data = CorpEntity.CType(data, CorpEntity);
            $scope.currentContact = data;

            $scope.CorpEntites.push($scope.currentContact);
            alert("Add entity succeed !")
        }).error(function (data, status, headers, config) {
            $scope.loadPanelVisible = false;
            alert('Add buyer Entities error : ' + JSON.stringify(data))
        });
    }
    $scope.AssginEntity = function () {

        $scope.loadPanelVisible = true;

               $scope.currentContact.$assign(function () {
            $scope.loadPanelVisible = false;
            alert("Assigned succeed !");
        },function () {
            $scope.loadPanelVisible = false;
            alert('Can not find BBLE of address:(' + $scope.currentContact.PropertyAssigned + ") Please make sure this address is available");
        });

    }
    $scope.ChangeTeam = function (team) {
        $scope.SelectedTeam = team;
    }
    $scope.OpenLeadsView = function () {
        var bble = $scope.currentContact.BBLE
        var url = '/ViewLeadsInfo.aspx?id=' + bble;
        OpenLeadsWindow(url, "View Leads Info " + bble);
    }
    $scope.UploadFile = function (fileUploadId, type, field) {
        $scope.loadPanelVisible = true;

        var contact = $scope.currentContact;
        var entityId = contact.EntityId;

        var fileData = document.getElementById(fileUploadId).files[0];

        $.ajax({
            url: '/services/ContactService.svc/UploadFile?id=' + entityId + '&type=' + type,
            type: 'POST',
            data: fileData,
            cache: false,
            dataType: 'json',
            processData: false, 
            contentType: "application/octet-stream", 
            success: function (data) {
                alert('successful..');
                $scope.currentContact[field] = data;
                $scope.loadPanelVisible = false;
                $scope.$apply();
            },
            error: function (data) {
                alert('Some error Occurred!');
                $scope.loadPanelVisible = false;
                $scope.$apply();
            }
        });
    }

}]);
angular.module('PortalApp')
.controller('ConstructionCtrl', ['$scope', '$http', '$interpolate', 'ptCom', 'ptContactServices', 'ptEntityService', 'ptShortsSaleService', 'ptLeadsService', 'ptConstructionService',
function ($scope, $http, $interpolate, ptCom, ptContactServices, ptEntityService, ptShortsSaleService, ptLeadsService, ptConstructionService) {

    var CSCaseModel = function () {
        this.CSCase = {
            InitialIntake: {},
            Photos: {},
            Utilities: {
                Company: [],
                Insurance_Type: []
            },
            Violations: {
                DOBViolations: [],
                ECBViolations: []
            },
            ProposalBids: {},
            Plans: {},
            Contract: {},
            Signoffs: {},
            Comments: []
        }
    }
    var PercentageModel = function () {
        this.intake = {
            count: 0,
            finished: 0,
        };
        this.signoff = {
            count: 0,
            finished: 0
        };
        this.construction = {
            count: 0,
            finished: 0
        };
        this.test = {
            count: 0,
            finished: 0
        }
    }

    $scope._ = _;

    $scope.ReloadedData = {}
    $scope.CSCase = new CSCaseModel();
    $scope.percentage = new PercentageModel();

    $scope.UTILITY_SHOWN = {
        'ConED': 'CSCase.CSCase.Utilities.ConED_Shown',
        'Energy Service': 'CSCase.CSCase.Utilities.EnergyService_Shown',
        'National Grid': 'CSCase.CSCase.Utilities.NationalGrid_Shown',
        'DEP': 'CSCase.CSCase.Utilities.DEP_Shown',
        'MISSING Water Meter': 'CSCase.CSCase.Utilities.MissingMeter_Shown',
        'Taxes': 'CSCase.CSCase.Utilities.Taxes_Shown',
        'ADT': 'CSCase.CSCase.Utilities.ADT_Shown',
        'Insurance': 'CSCase.CSCase.Utilities.Insurance_Shown',
    };
    $scope.HIGHLIGHTS = [
                            { message: 'Plumbing signed off at {{CSCase.CSCase.Signoffs.Plumbing_SignedOffDate}}', criteria: 'CSCase.CSCase.Signoffs.Plumbing_SignedOffDate' },
                            { message: 'Electrical signed off at {{CSCase.CSCase.Signoffs.Electrical_SignedOffDate}}', criteria: 'CSCase.CSCase.Signoffs.Electrical_SignedOffDate' },
                            { message: 'Construction signed off at {{CSCase.CSCase.Signoffs.Construction_SignedOffDate}}', criteria: 'CSCase.CSCase.Signoffs.Construction_SignedOffDate' },
                            { message: 'HPD Violations has all finished', criteria: 'CSCase.CSCase.Violations.HPD_OpenHPDViolation === false' }
    ];
    $scope.WATCHED_MODEL = [
                                {
                                    model: 'CSCase.CSCase.Signoffs.Plumbing_SignedOffDate',
                                    backedModel: 'ReloadedData.Backed_Plumbing_SignedOffDate',
                                    info: 'Plumbing Sign Off Date'
                                },
                                {
                                    model: 'CSCase.CSCase.Signoffs.Construction_SignedOffDate',
                                    backedModel: 'ReloadedData.Backed_Construction_SignedOffDate',
                                    info: 'Construction Sign Off Date'
                                },
                                {
                                    model: 'CSCase.CSCase.Signoffs.Electrical_SignedOffDate',
                                    backedModel: 'ReloadedData.Electrical_SignedOffDate',
                                    info: 'Electrical Sign Off Date'
                                }];



    $scope.arrayRemove = ptCom.arrayRemove;
    $scope.ptContactServices = ptContactServices;
    $scope.ensurePush = function (modelName, data) { ptCom.ensurePush($scope, modelName, data); }
    $scope.getRunnerList = function () {
        var url = "/api/ConstructionCases/GetRunners";
        $http.get(url)
            .then(function (res) {
                if (res.data) {
                    $scope.RUNNER_LIST = res.data;
                }
            });
    }();
    $scope.setPopupVisible = function (modelName, bVal) {
        $scope.$eval(modelName + '=' + bVal);
    }

    $scope.reload = function () {
        $scope.ReloadedData = {};
        $scope.CSCase = new CSCaseModel();
        $scope.ensurePush('CSCase.CSCase.Utilities.Floors', { FloorNum: '?', ConED: {}, EnergyService: {}, NationalGrid: {} });
        $scope.percentage = new PercentageModel();
        $scope.clearWarning();
    }
    $scope.init = function (bble, callback) {
        ptCom.startLoading();
        bble = bble.trim();
        $scope.reload();
        var done1, done2, done3, done4; 

        ptConstructionService.getConstructionCases(bble, function (res) {
            ptCom.nullToUndefined(res);
            $.extend(true, $scope.CSCase, res);
            $scope.initWatchedModel();
            done1 = true;
            if (done1 && done2 && done3 & done4) {
                ptCom.stopLoading();
                if (callback) callback();
            }

        });
        ptShortsSaleService.getShortSaleCaseByBBLE(bble, function (res) {
            $scope.SsCase = res;
            done2 = true;
            if (done1 && done2 && done3 & done4) {
                ptCom.stopLoading();
                if (callback) callback();
            }

        });
        ptLeadsService.getLeadsByBBLE(bble, function (res) {
            $scope.LeadsInfo = res;
            done3 = true;
            if (done1 && done2 && done3 & done4) {
                ptCom.stopLoading();
                if (callback) callback();
            }
        });
        ptEntityService.getEntityByBBLE(bble, function (error, data) {
            if (data) {
                $scope.EntityInfo = data;
            } else {
                $scope.EntityInfo = {};
                console.log(error);
            }
            done4 = true;
            if (done1 && done2 && done3 & done4) {
                ptCom.stopLoading();
                if (callback) callback();
            }
        })

    }

    $scope.ChangeStatus = function (scuessfunc, status) {
        $http.post('/api/ConstructionCases/ChangeStatus/' + leadsInfoBBLE, status)
            .success(function () {
                if (scuessfunc) {
                    scuessfunc();
                } else {
                    ptCom.alert("Successed !");
                }
            }).error(function (data, status) {
                ptCom.alert("Fail to save data. status " + status + "Error : " + JSON.stringify(data));
            });
    }

    $scope.saveCSCase = function () {
        var data = angular.toJson($scope.CSCase);
        ptConstructionService.saveConstructionCases($scope.CSCase.BBLE, data, function (res) {
            ScopeSetLastUpdateTime($scope.GetTimeUrl());
            ptCom.alert("Save successfully!");
        });
        $scope.updateInitialFormOwner();
        $scope.checkWatchedModel();
    }


    $scope.$watch('CSCase.CSCase.Utilities.Company', function (newValue) {
        if (newValue) {
            var ds = $scope.UTILITY_SHOWN;
            var target = $scope.CSCase.CSCase.Utilities.Company;
            _.each(target, function (k, i) {
                $scope.$eval(ds[k] + '=false');
            });
            _.each(newValue, function (el, i) {
                $scope.$eval(ds[el] + '=true');
            });
        }
    }, true);

    $scope.$watch('CSCase.CSCase.Utilities.ConED_EnergyServiceRequired', function (newVal) {

        if (newVal) {
            if ($scope.CSCase.CSCase.Utilities.Company.indexOf('Energy Service') < 0) {
                $scope.CSCase.CSCase.Utilities.Company.push('Energy Service');
                $scope.ReloadedData.EnergyService_Collapsed = false;
            }
        } else {
            var index = $scope.CSCase.CSCase.Utilities.Company.indexOf('Energy Service');
            if (index !== -1) $scope.CSCase.CSCase.Utilities.Company.splice(index, 1);
        }


    });

    $scope.sendNotice = function (id, name) {
        confirm("Send Intake Sheet To " + name + " ?");
    }

    $scope.showPopover = function (e) {
        aspxConstructionCommentsPopover.ShowAtElement(e.target);
    }

    $scope.addComment = function (comment) {
        var newComments = {}
        newComments.comment = comment;
        newComments.caseId = $scope.CaseId;
        newComments.createBy = Current_User;
        newComments.createDate = new Date();
        $scope.CSCase.CSCase.Comments.push(newComments);
    }
    $scope.addCommentFromPopup = function () {
        var comment = $scope.addCommentTxt;
        $scope.addComment(comment);
        $scope.addCommentTxt = '';
    }

    $scope.activeTab = 'CSInitialIntake';
    $scope.updateActive = function (id) {
        $scope.activeTab = id;
    }

    $scope.isHighlight = function (criteria) {
        return $scope.$eval(criteria);
    }
    $scope.highlightMsg = function (msg) {
        var msgstr = $interpolate(msg)($scope);
        return msgstr;
    }

    $scope.initWatchedModel = function () {
        _.each($scope.WATCHED_MODEL, function (el, i) {
            $scope.$eval(el.backedModel + '=' + el.model);
        });
    }
    $scope.checkWatchedModel = function () {
        var res = "";
        _.each($scope.WATCHED_MODEL, function (el, i) {
            if ($scope.$eval(el.backedModel + "!=" + el.model)) {
                $scope.$eval(el.backedModel + "=" + el.model);
                res += (el.info + " changes to " + $scope.$eval(el.model) + ".<br>");
            }
        });
        if (res) AddActivityLog(res);
    }


    $scope.HeaderEditing = false;
    $scope.toggleHeaderEditing = function (open) {
        $scope.HeaderEditing = !$scope.HeaderEditing;
        if (open) $("#ConstructionTitleInput").focus();
    }

    $scope.addNewDOBViolation = function () {
        $scope.ensurePush('CSCase.CSCase.Violations.DOBViolations');
        $scope.setPopupVisible('ReloadedData.DOBViolations_PopupVisible_' + ($scope.CSCase.CSCase.Violations.DOBViolations.length - 1), true);
    }
    $scope.addNewECBViolation = function () {
        $scope.ensurePush('CSCase.CSCase.Violations.ECBViolations');
        $scope.setPopupVisible('ReloadedData.ECBViolations_PopupVisible_' + ($scope.CSCase.CSCase.Violations.ECBViolations.length - 1), true);
    }

    $scope.fetchDOBViolations = function () {
        ptCom.confirm("<h3 style='color:red'>Warning!!!</h3>Getting information from DOB takes a while.<br>And it will <b>REPLACE</b> your current Data, are you sure to continue?", "Warning")
        .then(function (confirmed) {
            if (confirmed) {
                ptConstructionService.getDOBViolations($scope.CSCase.BBLE, function (error, res) {
                    if (error) {
                        ptCom.alert(error);
                        console.log(error)
                    } else {
                        ptCom.confirm("<h3 style='color:red'>Warning!!!</h3>Your current DOB Violation data will be replaced.", "Warning")
                        .then(function (confirmed) {
                            if (confirmed) {

                                if (res && res.DOB_TotalDOBViolation) $scope.CSCase.CSCase.Violations.DOB_TotalDOBViolation = res.DOB_TotalDOBViolation;
                                if (res && res.DOB_TotalOpenViolations) $scope.CSCase.CSCase.Violations.DOB_TotalOpenViolations = res.DOB_TotalOpenViolations;
                                if (res && res.violations) $scope.CSCase.CSCase.Violations.DOBViolations = res.violations;

                            }
                        })

                    }
                })
            }

        })

    }
    $scope.fetchECBViolations = function () {
        ptCom.confirm("<h3 style='color:red'>Warning!!!</h3>Getting information from DOB takes a while.<br>And it will <b>REPLACE</b> your current Data, are you sure to continue?", "Warning")
        .then(function (confirmed) {
            if (confirmed) {
                ptConstructionService.getECBViolations($scope.CSCase.BBLE, function (error, res) {
                    if (error) {
                        ptCom.alert(error);
                        console.log(error)
                    } else {
                        ptCom.confirm("<h3 style='color:red'>Warning!!!</h3>Your current DOB Violation data will be replaced.", "Warning")
                        .then(function (confirmed) {
                            if (confirmed) {
                                if (res && res.ECP_TotalViolation) $scope.CSCase.CSCase.Violations.ECP_TotalViolation = res.ECP_TotalViolation;
                                if (res && res.ECP_TotalOpenViolations) $scope.CSCase.CSCase.Violations.ECP_TotalOpenViolations = res.ECP_TotalOpenViolations;
                                if (res && res.violations) {
                                    $scope.CSCase.CSCase.Violations.ECBViolations = _.filter(res.violations, function (el, i) {
                                        return el.DOBViolationStatus.slice(0, 4) == "OPEN"
                                    });
                                }
                            }
                        })

                    }
                })
            }

        })
    }

    $scope.test = $scope.checkIntake;
    $scope.intakeComplete = function () {
        if (!$scope.checkIntake(function (el) {
            el.prev().css('background-color', 'yellow')
        })) {
            $scope.CSCase.IntakeCompleted = true;
            AddActivityLog("Intake Process have finished!");
            $scope.saveCSCase();
        } else {
            ptCom.alert("Intake Complete Fails.\nPlease check highlights for missing information!");
        }
    }
    $scope.checkIntake = function (callback) {
        $scope.clearWarning();
        $scope.percentage.intake.count = 0;
        $scope.percentage.intake.finished = 0;
        $(".intakeCheck").each(function (idx) {
            var test;
            var model = $(this).attr('ng-model') || $(this).attr('ss-model') || $(this).attr('file-model') || $(this).attr('model');
            if (model) {
                if (model.slice(0, 4) === "floor") {
                    test = _.has($(this).scope().floor, model.split(".").splice(1).join('.'));
                    if (!test) {
                        if (callback) callback($(this))
                    } else {
                        $scope.percentage.intake.finished++;
                    }
                } else {
                    test = $scope.$eval(model)
                    if (test === undefined) {
                        if (callback) callback($(this))
                    } else {
                        $scope.percentage.intake.finished++;
                    }
                }
            }
            $scope.percentage.intake.count++;
        })
        var errors = $scope.percentage.intake.count - $scope.percentage.intake.finished;
        return errors;
    }
    $scope.clearWarning = function () {
        $(".intakeCheck").each(function (idx) {
            $(this).prev().css('background-color', 'transparent');
        });
    }
    $scope.updatePercentage = function () {
        $scope.checkIntake();
    }

    $scope.GetTimeUrl = function () {
        return $scope.CSCase.BBLE ? "/api/ConstructionCases/LastLastUpdate/" + $scope.CSCase.BBLE : "";
    }
    $scope.GetCSCaseId = function () {
        return $scope.CSCase.BBLE;
    }
    $scope.GetModifyUserUrl = function () {
        return "/api/ConstructionCases/LastModifyUser/" + $scope.CSCase.BBLE;
    }

    $scope.printWindow = function () {
        window.open("/Construction/ConstructionPrint.aspx?bble=" + $scope.CSCase.BBLE, 'Print', 'width=1024, height=800');
    }

    $scope.openInitialForm = function () {
        window.open("/Construction/ConstructionInitialForm.aspx?bble=" + $scope.CSCase.BBLE, 'Initial Form', 'width=1280, height=960')
    }
    $scope.openBudgetForm = function () {
        window.open("/Construction/ConstructionBudgetForm.aspx?bble=" + $scope.CSCase.BBLE, 'Budget Form', 'width=1024, height=768')
    }

    $scope.updateInitialFormOwner = function () {
        var url = "/api/ConstructionCases/UpdateInitialFormOwner?BBLE=" + $scope.CSCase.BBLE + "&owner=" + $scope.CSCase.CSCase.InitialIntake.InitialFormAssign
        $http({
            method: 'POST',
            url: url
        }).then(function success(res) {
            console.log("Assign Initial Form owner Success.")
        }, function error(res) {
            console.log("Fail to assign Initial Form owner.")
        });
    }

    $scope.getOrdersLength = function () {
        return
    }
}]);
angular.module("PortalApp").controller("DialerManagementController",
['$scope', 'EmployeeModel', 'ptCom', '$http',
function ($scope, EmployeeModel, ptCom, $http) {
    $scope.lookup = undefined;
    EmployeeModel.getEmpNames().then(
        function success(d) {
            var employeesList = d.data;
            $("#lookup").dxLookup({
                items: employeesList,
                value: employeesList[0],
                showPopupTitle: false
            });
            $scope.lookup = $("#lookup").dxLookup("instance");
        });

    $scope.CreateContactList = function () {
        if ($scope.lookup) {
            var emp = $scope.lookup.option('value');
            if (emp) {
                $http({
                    method: "POST",
                    url: '/api/dialer/CreateContactList/' + emp
                }).then(function (d) {
                    ptCom.alert("Contact For " + emp + " is: " + d.data);
                }, function () {
                    ptCom.alert("Fail to create list, may existing");
                }
                    )
            } else {
                ptCom.alert("Employee not select corretly");
            }
        } else {
            ptCom.alert("Lookup not init yet.")
        }
    }
    $scope.SyncNewLeadsFolder = function () {
        if ($scope.lookup) {
            var emp = $scope.lookup.option("value");
            if (emp) {
                $http({
                    method: 'POST',
                    url: '/api/dialer/SyncNewLeadsFolder/' + emp
                }).then(function sucs(resp) {
                    ptCom.alert("Sync " + resp.data + " leads to new folder");
                }, function err() {
                    ptCom.alert("fail to sync new folder")
                })
            }

        } else {
            ptCom.alert("Lookup not init yet. ")
        }
    }

}])
angular.module('PortalApp')
    .controller('LeadTaxSearchCtrl', function ($scope, $http, $element, $timeout, ptContactServices, ptCom, DocSearch, LeadsInfo, DocSearchEavesdropper, DivError, $location) {
        leadsInfoBBLE = $('#BBLE').val();
        $scope.ShowInfo = $('#ShowInfo').val();
        $scope.ptContactServices = ptContactServices;

        $scope.DivError = new DivError('DocSearchErrorDiv');

        $scope.DocSearch = {}

        $scope.endorseCheckDate = function (date) {
            return false;
            var that = $scope.DocSearch;

            if (that.CreateDate > date) {
                return true;
            }
            return false;
        }

        $scope.endorseCheckVersion = function () {
            var that = $scope.DocSearch;
            if (that.Version) {
                return true;
            }
            return false;
        }

        $scope.GoToNewVersion = function (versions) {
            $scope.newVersion = versions;
        }



        $scope.versionController = new DocSearchEavesdropper()
        $scope.versionController.setEavesdropper($scope, $scope.GoToNewVersion);

        $scope.multipleValidated = function (base, boolKey, arraykey) {
            var boolVal = base[boolKey];
            var arrayVal = base[arraykey];
            var hasWarning = (boolVal === null) || (boolVal && arrayVal == false);
            return hasWarning;
        }
        $scope.init = function (bble) {

            leadsInfoBBLE = bble || $('#BBLE').val();
            $scope.ShowInfo = $('#ShowInfo').val();
            if (!leadsInfoBBLE) {
                console.log("Can not load page without BBLE !")
                return;
            }

            $scope.DocSearch = DocSearch.get({ BBLE: leadsInfoBBLE.trim() }, function () {
                $scope.LeadsInfo = LeadsInfo.get({ BBLE: leadsInfoBBLE.trim() }, function () {

                                         $scope.DocSearch.initLeadsResearch();
                    $scope.DocSearch.initTeam();
                    $scope.versionController.start2Eaves();
                });

            });

        }

        $scope.init(leadsInfoBBLE)


        $scope.newVersionValidate = function () {
            if (!$scope.newVersion) {
                return true;
            }

            if (!$scope.DivError.passValidate()) {
                return false;
            }

            return true;






        }

        $scope.SearchComplete = function (isSave) {
            if (!isSave) {
                if (!$scope.newVersionValidate()) {
                    var msg = $scope.DivError.getMessage();

                    AngularRoot.alert(msg[0]);
                    return;
                };
            }


            $scope.DocSearch.BBLE = $scope.DocSearch.BBLE.trim();
            $scope.DocSearch.ResutContent = $("#search_summary_div").html();

            if (isSave) {
                $scope.DocSearch.$update(null, function () {
                    AngularRoot.alert("Save successfully!");
                });
            } else {
                $scope.DocSearch.$completed(null, function () {

                    AngularRoot.alert("Document completed!")
                    gridCase.Refresh();
                });
            }

            $scope.test = function () {
                $scope.$digest();
            }

        }



        $scope.$watch('DocSearch.LeadResearch.fha', function (nv, ov) {
            if (nv == true) {
                if ($scope.DocSearch.LeadResearch.fannie) $scope.DocSearch.LeadResearch.fannie = false;
                if ($scope.DocSearch.LeadResearch.Freddie_Mac_) $scope.DocSearch.LeadResearch.Freddie_Mac_ = false;
            }
        })
        $scope.$watch('DocSearch.LeadResearch.fannie', function (nv, ov) {
            if (nv == true) {
                if ($scope.DocSearch.LeadResearch.fha) $scope.DocSearch.LeadResearch.fha = false;
                if ($scope.DocSearch.LeadResearch.Freddie_Mac_) $scope.DocSearch.LeadResearch.Freddie_Mac_ = false;
            }
        })

        $scope.$watch('DocSearch.LeadResearch.Freddie_Mac_', function (nv, ov) {
            if (nv == true) {
                if ($scope.DocSearch.LeadResearch.fannie) $scope.DocSearch.LeadResearch.fannie = false;
                if ($scope.DocSearch.LeadResearch.fha) $scope.DocSearch.LeadResearch.fha = false;
            }
        })

        $scope.markCompleted = function (status, msg) {

            msg = 'Please provide Note or press no to cancel';
            ptCom.prompt(msg, function (result) {
                if (result != null) {
                    $scope.DocSearch.markCompleted($scope.DocSearch.BBLE, status, result).then(function succ(d) {
                        $scope.DocSearch.UnderwriteStatus = d.data.UnderwriteStatus;
                        $scope.DocSearch.UnderwriteCompletedBy = d.data.UnderwriteCompletedBy;
                        $scope.DocSearch.UnderwriteCompletedOn = d.data.UnderwriteCompletedOn;
                        $scope.DocSearch.UnderwriteCompletedNotes = d.data.UnderwriteCompletedNotes;
                    }, function err() {
                        console.log("fail to update docsearch");
                    });
                }

                            }, true);

        }

        try {
            var modePatten = /mode=\d/;
            var matches = modePatten.exec(location.search);
            if (matches && matches[0]) {
                $scope.viewmode = parseInt(matches[0].split('=')[1]);
            } else {
                $scope.viewmode = 0;
            }
        } catch (ex) {
            $scope.viewmode = 0;
        }
    });
angular.module('PortalApp').controller('LegalCtrl', ['$scope', '$http', 'ptContactServices', 'ptCom', 'ptTime', '$window', function ($scope, $http, ptContactServices, ptCom, ptTime, $window) {

    $scope.ptContactServices = ptContactServices;
    $scope.ptCom = ptCom;
    $scope.isPassByDays = ptTime.isPassByDays;
    $scope.isPassOrEqualByDays = ptTime.isPassOrEqualByDays;
    $scope.isLessOrEqualByDays = ptTime.isLessOrEqualByDays;
    $scope.isPassByMonths = ptTime.isPassByMonths;
    $scope.isPassOrEqualByMonths = ptTime.isPassOrEqualByMonths;

    $scope.LegalCase = {
        PropertyInfo: {},
        ForeclosureInfo: {
            PlaintiffId: 638
        },
        SecondaryInfo: {
            StatuteOfLimitations: [],
        },
        PreQuestions: {},
        SecondaryTypes: []
    };
    $scope.TestRepeatData = [];
    $scope.History = [];
    $scope.SecondaryTypeSource = ["Statute Of Limitations", "Estate", "Miscellaneous", "Deed Reversal", "Partition", "Breach of Contract", "Quiet Title", ""];
    if (typeof LegalShowAll == 'undefined' || LegalShowAll == null) {
        $scope.LegalCase.SecondaryInfo.SelectTypes = $scope.SecondaryTypeSource;
    }

    $scope.filterSelected = true;
    $scope.PickedContactId = null;

    $scope.hSummery = [
                {
                    "Name": "CaseStauts",
                    "CallFunc": "HighLightStauts(LegalCase.CaseStauts,4)",
                    "Description": "Last milestone document recorded on Clerk Minutes after O/REF. ",
                    "ArrayName": ""
                },
                {
                    "Name": "EveryOneIn",
                    "CallFunc": "LegalCase.ForeclosureInfo.WasEstateFormed != null",
                    "Description": "There is an estate.",
                    "ArrayName": ""
                },
                {
                    "Name": "BankruptcyFiled",
                    "CallFunc": "LegalCase.ForeclosureInfo.BankruptcyFiled == true",
                    "Description": "Bankruptcy filed",
                    "ArrayName": ""
                },
                {
                    "Name": "Efile",
                    "CallFunc": "LegalCase.ForeclosureInfo.Efile == true",
                    "Description": "Has E-filed",
                    "ArrayName": ""
                },
                {
                    "Name": "EfileN",
                    "CallFunc": "LegalCase.ForeclosureInfo.Efile == false",
                    "Description": "No E-filed",
                    "ArrayName": ""
                },
                {
                    "Name": "ClientPersonallyServed",
                    "CallFunc": "false",
                    "Description": "Client personally is not served. ",
                    "ArrayName": "AffidavitOfServices"
                },
                {
                    "Name": "NailAndMail",
                    "CallFunc": "true",
                    "Description": "Nail and Mail.",
                    "ArrayName": "AffidavitOfServices"
                },
                {
                    "Name": "BorrowerLiveInAddrAtTimeServ",
                    "CallFunc": "false",
                    "Description": "Borrower didn\'t live in service Address at time of Serv.",
                    "ArrayName": "AffidavitOfServices"
                },
                {
                    "Name": "BorrowerEverLiveHere",
                    "CallFunc": "false",
                    "Description": "Borrower didn\'t ever live in service address.",
                    "ArrayName": "AffidavitOfServices"
                },
                {
                    "Name": "ServerInSererList",
                    "CallFunc": "true",
                    "Description": "process server is in server list.",
                    "ArrayName": "AffidavitOfServices"
                },
                {
                    "Name": "isServerHasNegativeInfo",
                    "CallFunc": "true",
                    "Description": "Web search provide any negative information on process server. ",
                    "ArrayName": "AffidavitOfServices"
                },
                {
                    "Name": "AffidavitServiceFiledIn20Day",
                    "CallFunc": "false",
                    "Description": "Affidavit of service wasn\'t file within 20 days of service.",
                    "ArrayName": "AffidavitOfServices"
                },
                {
                    "Name": "AnswerClientFiledBefore",
                    "CallFunc": "LegalCase.ForeclosureInfo.AnswerClientFiledBefore == false",
                    "Description": "Client hasn\'t ever filed an answer before.",
                    "ArrayName": ""
                },
                {
                    "Name": "NoteIsPossess",
                    "CallFunc": "LegalCase.ForeclosureInfo.NoteIsPossess == false",
                    "Description": "We Don't possess a copy of the note.",
                    "ArrayName": ""
                },
                {
                    "Name": "NoteEndoresed",
                    "CallFunc": "LegalCase.ForeclosureInfo.NoteEndoresed == false",
                    "Description": "Note wasn\'t endores.",
                    "ArrayName": ""
                },
                {
                    "Name": "NoteEndorserIsSignors",
                    "CallFunc": "LegalCase.ForeclosureInfo.NoteEndorserIsSignors == true",
                    "Description": "The endorser is in signors list.",
                    "ArrayName": ""
                },
                {
                    "Name": "HasDocDraftedByDOCXLLC",
                    "CallFunc": "true",
                    "Description": "There are documents drafted by DOCX LLC .",
                    "ArrayName": "Assignments"
                },
                {
                    "Name": "LisPendesRegDate",
                    "CallFunc": "isPassOrEqualByDays(LegalCase.ForeclosureInfo.LisPendesDate, LegalCase.ForeclosureInfo.LisPendesRegDate, 5)",
                    "Description": "Date of registration 5 days after Lis Pendens letter",
                    "ArrayName": ""
                },
                {
                    "Name": "AccelerationLetterMailedDate",
                    "CallFunc": "isPassOrEqualByMonths(LegalCase.ForeclosureInfo.DefaultDate,LegalCase.ForeclosureInfo.AccelerationLetterMailedDate,12 )",
                    "Description": "Acceleration letter mailed to borrower after 12 months of Default Date. ",
                    "ArrayName": ""
                },
                {
                    "Name": "AccelerationLetterRegDate",
                    "CallFunc": "isPassOrEqualByDays(LegalCase.ForeclosureInfo.AccelerationLetterMailedDate,LegalCase.ForeclosureInfo.AccelerationLetterRegDate,3 )",
                    "Description": "Date of registration for Acceleration letter filed  3 days after acceleration letter mailed date",
                    "ArrayName": ""
                },
                {
                    "Name": "AffirmationFiledDate",
                    "CallFunc": "isPassByDays(LegalCase.ForeclosureInfo.JudgementDate,LegalCase.ForeclosureInfo.AffirmationFiledDate,0)",
                    "Description": "Affirmation filed after Judgement. ",
                    "ArrayName": ""
                },
                {
                    "Name": "AffirmationReviewerByCompany",
                    "CallFunc": "LegalCase.ForeclosureInfo.AffirmationReviewerByCompany == false",
                    "Description": "The affirmation reviewer wasn\'t employe by the servicing company. ",
                    "ArrayName": ""
                },
                {
                    "Name": "MortNoteAssInCert",
                    "CallFunc": "LegalCase.ForeclosureInfo.MortNoteAssInCert == false",
                    "Description": "In the Certificate of Merit, the Mortgage, Note and Assignment aren\'t included. ",
                    "ArrayName": ""
                },
                {
                    "Name": "MissInCert",
                    "CallFunc": "checkMissInCertValue()",
                    "Description": "Mortgage Note or Assignment are missing. ",
                    "ArrayName": ""
                },
                {
                    "Name": "CertificateReviewerByCompany",
                    "CallFunc": "LegalCase.ForeclosureInfo.CertificateReviewerByCompany == false",
                    "Description": "The certificate  reviewer wasn\'t employe by the servicing company. ",
                    "ArrayName": ""
                },
                {
                    "Name": "LegalCase.ItemsRedacted",
                    "CallFunc": "LegalCase.ForeclosureInfo.ItemsRedacted == false",
                    "Description": "Are items of personal information Redacted.",
                    "ArrayName": ""
                },
                {
                    "Name": "RJIDate",
                    "CallFunc": "isPassByMonths(LegalCase.ForeclosureInfo.SAndCFiledDate, LegalCase.ForeclosureInfo.RJIDate, 12)",
                    "Description": "RJI filed after 12 months of S&C.",
                    "ArrayName": ""
                },
                {
                    "Name": "ConferenceDate",
                    "CallFunc": "isLessOrEqualByDays(LegalCase.ForeclosureInfo.RJIDate, LegalCase.ForeclosureInfo.ConferenceDate, 60)",
                    "Description": "Conference date scheduled 60 days before RJI",
                    "ArrayName": ""
                },
                {
                    "Name": "OREFDate",
                    "CallFunc": "isPassByMonths(LegalCase.ForeclosureInfo.RJIDate, LegalCase.ForeclosureInfo.OREFDate, 12)",
                    "Description": "O/REF filed after 12 months after RJI.",
                    "ArrayName": ""
                },
                {
                    "Name": "JudgementDate",
                    "CallFunc": "isPassByMonths(LegalCase.ForeclosureInfo.RJIDate, LegalCase.ForeclosureInfo.OREFDate, 12)",
                    "Description": "Judgement submitted 12 months after O/REF. ",
                    "ArrayName": ""
                }];

    $scope.querySearch = function (query) {
        var createFilterFor = function (query) {
            var lowercaseQuery = angular.lowercase(query);
            return function filterFn(contact) {
                return contact.Name && (contact.Name.toLowerCase().indexOf(lowercaseQuery) !== -1);
            };
        }
        var results = query ?
            $scope.allContacts.filter(createFilterFor(query)) : [];
        return results;
    }
    $scope.loadContacts = function () {
        var contacts = AllContact ? AllContact : [];
        return contacts.map(function (c, index) {
            c.image = 'https://storage.googleapis.com/material-icons/external-assets/v1/icons/svg/ic_account_circle_black_48px.svg';
            if (c.Name) {
                c._lowername = c.Name.toLowerCase();
            }
            return c;
        });
    };
    $scope.allContacts = $scope.loadContacts();
    $scope.contacts = [$scope.allContacts[0]];
    $scope.AllJudges = AllJudges ? AllJudges : [];

    $scope.addTest = function () {
        $scope.TestRepeatData[$scope.TestRepeatData.length] = $scope.TestRepeatData.length;
    };

    $scope.RoboSingerDataSource = new DevExpress.data.DataSource({
        store: new DevExpress.data.CustomStore({
            load: function (loadOptions) {
                if (AllRoboSignor) {
                    if (loadOptions.searchValue) {
                        return AllRoboSignor.filter(function (o) { if (o.Name) { return o.Name.toLowerCase().indexOf(loadOptions.searchValue.toLowerCase()) >= 0 } return false });
                    }
                    return [];
                }
            },
            byKey: function (key) {
                if (AllRoboSignor) {
                    return AllRoboSignor.filter(function (o) { return o.ContactId == key })[0];
                }

            },
            searchExpr: ["Name"]
        })
    });
    $scope.InitContact = function (id, dataSourceName) {
        return {
            dataSource: dataSourceName ? $scope[dataSourceName] : $scope.ContactDataSource,
            valueExpr: 'ContactId',
            displayExpr: 'Name',
            searchEnabled: true,
            minSearchLength: 2,
            noDataText: "Please input to search",
            bindingOptions: { value: id }
        }
    };
    $scope.TestContactId = function (c) {
        $scope.$eval(c + '=' + '192');
    };
    $scope.GetContactById = function (id) {
        return AllContact.filter(function (o) { return o.ContactId == id })[0];
    };

    $scope.CheckPlace = function (p) {
        if (p) {
            return p === 'NY';
        }
        return false;
    };

    $scope.SaveLegal = function (scuessfunc) {
        if (!LegalCaseBBLE || LegalCaseBBLE !== leadsInfoBBLE) {
            alert("Case not load completed please wait!");
            return;
        }
        var json = JSON.stringify($scope.LegalCase);
        var data = { bble: LegalCaseBBLE, caseData: json };
        $http.post('LegalUI.aspx/SaveCaseData', data).
            success(function () {
                if (scuessfunc) {
                    scuessfunc()
                } else {
                    $scope.LogSaveChange();
                    alert("Save Successed !");
                }
                ResetCaseDataChange();
            }).
            error(function (data, status) {
                alert("Fail to save data. status " + status + "Error : " + JSON.stringify(data));
            });
    };

    $scope.CompleteResearch = function () {
        var json = JSON.stringify($scope.LegalCase);
        var data = { bble: leadsInfoBBLE, caseData: json, sn: taskSN };
        $http.post('LegalUI.aspx/CompleteResearch', data).success(function () {
            alert("Submit Success!");
            if (typeof gridTrackingClient !== 'undefined')
                gridTrackingClient.Refresh();

        }).error(function (data) {
            alert("Fail to save data :" + JSON.stringify(data));
            console.log(data);
        });
    }

    $scope.BackToResearch = function (comments) {
        var json = JSON.stringify($scope.LegalCase);

        var data = { bble: leadsInfoBBLE, caseData: json, sn: taskSN, comments: comments };
        $http.post('LegalUI.aspx/BackToResearch', data).
            success(function () {
                alert("Submit Success!");
                if (typeof gridTrackingClient !== 'undefined')
                    gridTrackingClient.Refresh();
            }).error(function (data1) {
                alert("Fail to save data :" + JSON.stringify(data1));
                console.log(data1);
            });
    }

    $scope.CloseCase = function (comments) {
        var data = { bble: leadsInfoBBLE, comments: comments };
        $http.post('LegalUI.aspx/CloseCase', data).
            success(function () {
                alert("Submit Success!");
                if (typeof gridTrackingClient !== 'undefined')
                    gridTrackingClient.Refresh();

            }).error(function (data) {
                alert("Fail to save data :" + JSON.stringify(data));
                console.log(data);
            });
    }

    $scope.AttorneyComplete = function () {
        var json = JSON.stringify($scope.LegalCase);

        var data = { bble: leadsInfoBBLE, caseData: json, sn: taskSN };
        $http.post('LegalUI.aspx/AttorneyComplete', data).
            success(function () {
                alert("Submit Success!");
                if (typeof gridTrackingClient !== 'undefined')
                    gridTrackingClient.Refresh();

            }).
            error(function () {
                alert("Fail to save data.");
            });

    }


    $scope.LogSaveChange = function () {
        for (var i in $scope.LogChange) {
            var changeObject = $scope.LogChange[i];
            var old = changeObject.old;
            var now = changeObject.now()
            if (old != now) {
                var elem = '#LealCaseStatusData'
                var OldStatus = $(elem + ' option[value="' + old + '"]').html();
                var NowStatus = $(elem + ' option[value="' + now + '"]').html();

                if (!OldStatus) {
                    AddActivityLog(changeObject.msg.replace(" from", '') + ' to ' + NowStatus);
                }
                else {
                    AddActivityLog(changeObject.msg + OldStatus + ' to ' + NowStatus);
                }

                $scope.LogChange[i].old = now;
            }
        }
    }

    $scope.LoadLeadsCase = function (BBLE) {
        ptCom.startLoading();
        var data = { bble: BBLE };
        var leadsInfoUrl = "/ShortSale/ShortSaleServices.svc/GetLeadsInfo?bble=" + BBLE;
        var shortsaleUrl = '/ShortSale/ShortSaleServices.svc/GetCaseByBBLE?bble=' + BBLE;
        var taxlienUrl = '/api/TaxLiens/' + BBLE;
        var legalecoursUrl = "/api/LegalECourtByBBLE/" + BBLE;

        $http.post('LegalUI.aspx/GetCaseData', data).
            success(function (data, status, headers, config) {
                $scope.LegalCase = $.parseJSON(data.d);
                $scope.LegalCase.BBLE = BBLE
                $scope.LegalCase.LegalComments = $scope.LegalCase.LegalComments || [];
                $scope.LegalCase.ForeclosureInfo = $scope.LegalCase.ForeclosureInfo || {};
                $scope.LogChange = {
                    'TaxLienFCStatus': { "old": $scope.LegalCase.TaxLienFCStatus, "now": function () { return $scope.LegalCase.TaxLienFCStatus; }, "msg": 'Tax Lien FC status changed from ' },
                    'CaseStauts': { "old": $scope.LegalCase.CaseStauts, "now": function () { return $scope.LegalCase.CaseStauts; }, "msg": 'Mortgage foreclosure status changed from ' }
                }

                var arrays = ["AffidavitOfServices", "Assignments", "MembersOfEstate"];
                for (a in arrays) {
                    var porp = arrays[a]
                    var array = $scope.LegalCase.ForeclosureInfo[porp];
                    if (!array || array.length === 0) {
                        $scope.LegalCase.ForeclosureInfo[porp] = [];
                        $scope.LegalCase.ForeclosureInfo[porp].push({});
                    }
                }
                $scope.LegalCase.SecondaryTypes = $scope.LegalCase.SecondaryTypes || []
                $scope.showSAndCFrom();

                LegalCaseBBLE = BBLE;
                ptCom.stopLoading();

                ResetCaseDataChange();
                CaseNeedComment = true;
            }).
            error(function () {
                ptCom.stopLoading();
                alert("Fail to load data : " + BBLE);
            });


        $http.get(shortsaleUrl)
            .success(function (data) {
                $scope.ShortSaleCase = data;
            }).error(function () {
                alert("Fail to Short sale case  data : " + BBLE);
            });



        $http.get(leadsInfoUrl)
            .success(function (data) {
                $scope.LeadsInfo = data;
                $scope.LPShow = $scope.ModelArray('LeadsInfo.LisPens');
            }).error(function (data) {
                alert("Get Short Sale Leads failed BBLE =" + BBLE + " error : " + JSON.stringify(data));
            });

        $http.get(taxlienUrl)
            .success(function (data) {
                $scope.TaxLiens = data;
                $scope.TaxLiensShow = $scope.ModelArray('TaxLiens');
            }).error(function (data) {
                alert("Get Tax Liens failed BBLE = " + BBLE + " error : " + JSON.stringify(data));
            });

        $http.get(legalecoursUrl)
            .success(function (data) {
                $scope.LegalECourt = data;
            }).error(function () {
                $scope.LegalECourt = null;
            });

    }

    $scope.ModelArray = function (model) {
        var array = $scope.$eval(model);
        return (array && array.length > 0) ? 'Yes' : '';
    }

    $scope.HighLightFunc = function (funcStr) {
        var args = funcStr.split(",");

    }

    $scope.AddSecondaryArray = function () {
        var selectType = $scope.LegalCase.SecondaryInfo.SelectedType;
        if (selectType) {
            var name = selectType.replace(/\s/g, '');
            var arr = $scope.LegalCase.SecondaryInfo[name];
            if (name === 'StatuteOfLimitations') {
                alert('match');
            }
            if (!arr || !Array.isArray($scope.LegalCase.SecondaryInfo[name])) {
                $scope.LegalCase.SecondaryInfo[name] = [];
            }
            $scope.LegalCase.SecondaryInfo[name].push({});
        }
    }
    $scope.LegalCase.SecondaryInfo.SelectedType = $scope.SecondaryTypeSource[0];
    $scope.SecondarySelectType = function () {
        $scope.LegalCase.SecondaryInfo.SelectTypes = $scope.LegalCase.SecondaryInfo.SelectTypes || [];
        var selectTypes = $scope.LegalCase.SecondaryInfo.SelectTypes;
        if (!_.contains(selectTypes, $scope.LegalCase.SecondaryInfo.SelectedType)) {
            selectTypes.push($scope.LegalCase.SecondaryInfo.SelectedType);
        }

    }
    $scope.CheckShow = function (filed) {
        if (typeof LegalShowAll === 'undefined' || LegalShowAll === null) {
            return true;
        }
        if ($scope.LegalCase.SecondaryInfo) {
            return $scope.LegalCase.SecondaryInfo.SelectedType == filed;
        }

        return false;
    }

    $scope.SaveLegalJson = function () {
        $scope.LegalCaseJson = JSON.stringify($scope.LegalCase)
    }

    $scope.ShowContorl = function (m) {
        var t = typeof m;
        if (t === "string") {
            return m === 'true'
        }
        return m;

    }

    $scope.DocGenerator = function (tplName) {
        if (!$scope.LegalCase.SecondaryInfo) {
            $scope.LegalCase.SecondaryInfo = {}
        }
        var Tpls = [{
            "tplName": 'OSCTemplate.docx',
            data: {
                "Plantiff": $scope.LegalCase.ForeclosureInfo.Plantiff,
                "PlantiffAttorney": $scope.LegalCase.ForeclosureInfo.PlantiffAttorney,
                "PlantiffAttorneyAddress": $scope.LegalCase.ForeclosureInfo.PlantiffAttorneyAddress,
                "FCFiledDate": $scope.LegalCase.ForeclosureInfo.FCFiledDate,
                "FCIndexNum": $scope.LegalCase.ForeclosureInfo.FCIndexNum,
                "BoroughName": $scope.LeadsInfo.BoroughName,
                "Block": $scope.LeadsInfo.Block,
                "Lot": $scope.LeadsInfo.Lot,
                "Defendant": $scope.LegalCase.SecondaryInfo.Defendant,

                "Defendants": $scope.LegalCase.SecondaryInfo.OSC_Defendants ? ',' + $scope.LegalCase.SecondaryInfo.OSC_Defendants.map(function (o) { return o.Name }).join(",") : ' ',
                "DefendantAttorneyName": $scope.LegalCase.SecondaryInfo.DefendantAttorneyName,
                "DefendantAttorneyPhone": ptContactServices.getContact($scope.LegalCase.SecondaryInfo.DefendantAttorneyId, $scope.LegalCase.SecondaryInfo.DefendantAttorneyName).OfficeNO,
                "DefendantAttorneyAddress": ptContactServices.getContact($scope.LegalCase.SecondaryInfo.DefendantAttorneyId, $scope.LegalCase.SecondaryInfo.DefendantAttorneyName).Address,
                "CourtAddress": $scope.GetCourtAddress($scope.LeadsInfo.Borough),
                "PropertyAddress": $scope.LeadsInfo.PropertyAddress,

            }
        },
        {
            "tplName": 'DeedReversionTemplate.docx',
            data: {
                "Plantiff": $scope.LegalCase.SecondaryInfo.DeedReversionPlantiff,
                "PlantiffAttorney": $scope.LegalCase.SecondaryInfo.DeedReversionPlantiffAttorney,
                "PlantiffAttorneyAddress": $scope.ptContactServices.getContact($scope.LegalCase.SecondaryInfo.DeedReversionPlantiffAttorneyId, $scope.LegalCase.SecondaryInfo.DeedReversionPlantiffAttorney).Address,
                "PlantiffAttorneyPhone": $scope.ptContactServices.getContact($scope.LegalCase.SecondaryInfo.DeedReversionPlantiffAttorneyId, $scope.LegalCase.SecondaryInfo.DeedReversionPlantiffAttorney).OfficeNO,
                "IndexNum": $scope.LegalCase.SecondaryInfo.DeedReversionIndexNum || ' ',
                "BoroughName": $scope.LeadsInfo.BoroughName,
                "Block": $scope.LeadsInfo.Block,
                "Lot": $scope.LeadsInfo.Lot,
                "Defendant": $scope.LegalCase.SecondaryInfo.DeedReversionDefendant,
                "Defendants": $scope.LegalCase.SecondaryInfo.DeedReversionDefendants ? ',' + $scope.LegalCase.SecondaryInfo.DeedReversionDefendants.map(function (o) { return o.Name }).join(",") : ' ',
                "CourtAddress": $scope.GetCourtAddress($scope.LeadsInfo.Borough),
                "PropertyAddress": $scope.LeadsInfo.PropertyAddress,

            },


        },
        {
            "tplName": 'SpecificPerformanceComplaintTemplate.docx',
            data: {
                "Plantiff": $scope.LegalCase.SecondaryInfo.SPComplaint_Plantiff,
                "PlantiffAttorney": $scope.LegalCase.SecondaryInfo.SPComplaint_PlantiffAttorney,
                "PlantiffAttorneyAddress": $scope.ptContactServices.getContact($scope.LegalCase.SecondaryInfo.SPComplaint_PlantiffAttorneyId, $scope.LegalCase.SecondaryInfo.SPComplaint_PlantiffAttorney).Address,
                "PlantiffAttorneyPhone": $scope.ptContactServices.getContact($scope.LegalCase.SecondaryInfo.SPComplaint_PlantiffAttorneyId, $scope.LegalCase.SecondaryInfo.SPComplaint_PlantiffAttorney).OfficeNO,
                "IndexNum": $scope.LegalCase.SecondaryInfo.SPComplaint_IndexNum || ' ',
                "BoroughName": $scope.LeadsInfo.BoroughName,
                "Block": $scope.LeadsInfo.Block,
                "Lot": $scope.LeadsInfo.Lot,
                "Defendant": $scope.LegalCase.SecondaryInfo.SPComplaint_Defendant,
                "Defendants": $scope.LegalCase.SecondaryInfo.SPComplaint_Defendants ? ',' + $scope.LegalCase.SecondaryInfo.SPComplaint_Defendants.map(function (o) { return o.Name }).join(",") : ' ',
                "CourtAddress": $scope.GetCourtAddress($scope.LeadsInfo.Borough),
                "PropertyAddress": $scope.LeadsInfo.PropertyAddress,
            },

        },
        {
            "tplName": 'QuietTitleComplantTemplate.docx',
            data: {
                "Plantiff": $scope.LegalCase.SecondaryInfo.QTA_Plantiff,
                "PlantiffAttorney": $scope.LegalCase.SecondaryInfo.QTA_PlantiffAttorney,
                "PlantiffAttorneyAddress": $scope.ptContactServices.getContact($scope.LegalCase.SecondaryInfo.QTA_PlantiffAttorneyId, $scope.LegalCase.SecondaryInfo.QTA_PlantiffAttorney).Address,
                "PlantiffAttorneyPhone": $scope.ptContactServices.getContact($scope.LegalCase.SecondaryInfo.QTA_PlantiffAttorneyId, $scope.LegalCase.SecondaryInfo.QTA_PlantiffAttorney).OfficeNO,
                "OriginalMortgageLender": $scope.LegalCase.SecondaryInfo.QTA_OrgMorgLender,
                "Mortgagee": $scope.LegalCase.SecondaryInfo.QTA_Mortgagee,
                "IndexNum": $scope.LegalCase.SecondaryInfo.QTA_IndexNum || ' ',
                "BoroughName": $scope.LeadsInfo.BoroughName,
                "Block": $scope.LeadsInfo.Block,
                "Lot": $scope.LeadsInfo.Lot,
                "Defendant": $scope.LegalCase.SecondaryInfo.QTA_Defendant,
                "Defendant2": $scope.LegalCase.SecondaryInfo.QTA_Defendant2,
                "Defendants": $scope.LegalCase.SecondaryInfo.QTA_Defendants ? ',' + $scope.LegalCase.SecondaryInfo.QTA_Defendants.map(function (o) { return o.Name }).join(",") : ' ',
                "CourtAddress": $scope.GetCourtAddress($scope.LeadsInfo.Borough),
                "PropertyAddress": $scope.LeadsInfo.PropertyAddress,
                "FCFiledDate": $scope.LegalCase.ForeclosureInfo.FCFiledDate,
                "FCIndexNum": $scope.LegalCase.ForeclosureInfo.FCIndexNum,
                "DefaultDate": $scope.LegalCase.ForeclosureInfo.QTA_DefaultDate,
                "DeedToPlaintiffDate": $scope.LegalCase.SecondaryInfo.QTA_DeedToPlaintiffDate,
            },

        },
        {
            "tplName": 'Partition_Temp.docx',
            data: {
                "Plantiff": $scope.LegalCase.SecondaryInfo.PartitionsPlantiff,
                "PlantiffAttorney": $scope.LegalCase.SecondaryInfo.PartitionsPlantiffAttorney,
                "PlantiffAttorneyAddress": $scope.ptContactServices.getContact($scope.LegalCase.SecondaryInfo.PartitionsPlantiffAttorneyId, $scope.LegalCase.SecondaryInfo.PartitionsPlantiffAttorney).Address,
                "PlantiffAttorneyPhone": $scope.ptContactServices.getContact($scope.LegalCase.SecondaryInfo.PartitionsPlantiffAttorneyId, $scope.LegalCase.SecondaryInfo.PartitionsPlantiffAttorney).OfficeNO,
                "OriginalMortgageLender": $scope.LegalCase.SecondaryInfo.PartitionsOriginalLender,
                "MortgageDate": $scope.LegalCase.SecondaryInfo.PartitionsMortgageDate,
                "IndexNum": $scope.LegalCase.SecondaryInfo.PartitionsIndexNum || ' ',
                "BoroughName": $scope.LeadsInfo.BoroughName,
                "Block": $scope.LeadsInfo.Block,
                "Lot": $scope.LeadsInfo.Lot,
                "Defendant": $scope.LegalCase.SecondaryInfo.PartitionsDefendant,
                "Defendant1": $scope.LegalCase.SecondaryInfo.PartitionsDefendant1,

                "PropertyAddress": $scope.LeadsInfo.PropertyAddress,
                "MortgageAmount": $scope.LegalCase.SecondaryInfo.PartitionsMortgageAmount,
                "DateOfRecording": $scope.LegalCase.SecondaryInfo.PartitionsDateOfRecording,
                "CRFN": $scope.LegalCase.SecondaryInfo.PartitionsCRFN,
                "OriginalLender": $scope.LegalCase.SecondaryInfo.PartitionsOriginalLender,


            },

        }
        ];
        $scope.DocGenerator2 = function (tplName, data, successFunc) {
            $http.post("/Services/Documents.svc/DocGenrate", {
                "tplName": tplName,
                "data": JSON.stringify(data)
            }).success(function (data) {
                successFunc(data);
            }).error(function (data, status) {
                alert("Fail to save data. status: " + status + " Error : " + JSON.stringify(data));
            });
        };
        var tpl = Tpls.filter(function (o) { return o.tplName == tplName })[0];

        if (tpl) {
            for (var v in tpl.data) {
                var filed = tpl.data[v];
                if (!filed) {
                    alert("Some data missing please check " + v + "Please check!")
                    return;
                }
            }
            $scope.DocGenerator2(tpl.tplName, tpl.data, function (url) {
                STDownloadFile(url, tpl.tplName.replace("Template", ""));
            });
        } else {
            alert("can find tlp " + tplName)
        }
    }

    $scope.CheckSecondaryTags = function (tag) {
        return $scope.LegalCase.SecondaryTypes.filter(function (t) { return t == tag })[0];
    }
    $scope.GetCourtAddress = function (boro) {
        var address = ['', '851 Grand Concourse Bronx, NY 10451', '360 Adams St. Brooklyn, NY 11201', '8811 Sutphin Boulevard, Jamaica, NY 11435'];
        return address[boro - 1];
    }

    $scope.evalVisible = function (h) {
        var result = false;
        if (h.ArrayName) {
            if ($scope.LegalCase.ForeclosureInfo[h.ArrayName]) {
                angular.forEach($scope.LegalCase.ForeclosureInfo[h.ArrayName], function (el, idx) {
                    result = result || (el[h.Name] == (h.CallFunc === 'true'));
                })
            }
        } else {
            result = $scope.$eval(h.CallFunc);
        }
        return result;
    };

    angular.forEach($scope.hSummery, function (el, idx) {
        $scope.$watch(function () { return $scope.evalVisible(el); }, function (newV) {
            el.visible = newV;
        })

    })

    $scope.GetCaseInfo = function () {
        var CaseInfo = { Name: '', Address: '' }
        var caseName = $scope.LegalCase.CaseName
        if (caseName) {
            CaseInfo.Address = caseName.replace(/-(?!.*-).*$/, '');
            var matched = caseName.match(/-(?!.*-).*$/);
            if (matched && matched[0]) {
                CaseInfo.Name = matched[0].replace('-', '')
            }
        }
        return CaseInfo;
    }

    $scope.AddArrayItem = function (model) {
        model = model || [];
        model.push({});
    }
    $scope.DeleteItem = function (model, index) {
        model.splice(index, 1);
    }

    $scope.isLess08292013 = false;
    $scope.isBigger08302013 = false;
    $scope.isBigger03012015 = false;
    $scope.showSAndCFormFlag = false;

    $scope.showSAndCFrom = function () {
        var date = new Date($scope.LegalCase.ForeclosureInfo.SAndCFiledDate);
        if (date - new Date("08/29/2013") > 0) {
            $scope.isLess08292013 = false;
        } else {
            $scope.isLess08292013 = true;
        }
        if ($scope.isLess08292013) {
            $scope.isBigger08302013 = false;
        } else {
            $scope.isBigger08302013 = true;
        } if (date - new Date("03/01/2015") > 0) {
            $scope.isBigger03012015 = true;
        } else {
            $scope.isBigger03012015 = false;
        }
        $scope.showSAndCFormFlag = $scope.isLess08292013 | $scope.isBigger08302013 | $scope.isBigger03012015;
    };
    $scope.HighLightStauts = function (model, index) {
        return parseInt(model) > index ? true : false;
    };
    $scope.addToEstateMembers = function (index) {
        $scope.LegalCase.ForeclosureInfo.MembersOfEstate.push({ "id": index, "name": $scope.LegalCase.membersText });
        $scope.LegalCase.membersText = '';
    }
    $scope.delEstateMembers = function (index) {
        $scope.LegalCase.ForeclosureInfo.MembersOfEstate.splice(index, 1);
    }
    $scope.ShowECourts = function (borough) {
        $http.post('/CallBackServices.asmx/GetBroughName', { bro: $scope.LegalCase.PropertyInfo.Borough }).success(function (data) {
            var urls = ['http://bronxcountyclerkinfo.com/law/UI/User/lne.aspx', ' http://iapps.courts.state.ny.us/kcco/', ' https://iapps.courts.state.ny.us/qcco/'];
            var url = urls[borough - 2];
            var title = $scope.LegalCase.CaseName;
            var subTitle = ' (' + 'Brough: ' + data.d + ' Block: ' + $scope.LegalCase.PropertyInfo.Block + ' Lot: ' + $scope.LegalCase.PropertyInfo.Lot + ')';
            ShowPopupMap(url, title, subTitle);
        })

    }

    $scope.missingItems = [
        { id: 1, label: "Mortgage" },
        { id: 2, label: "Note" },
        { id: 3, label: "Assignment" },
    ];

    $scope.updateMissInCertValue = function (value) {
        $scope.LegalCase.ForeclosureInfo.MissInCert = value;
    }

    $scope.checkMissInCertValue = function () {
        if ($scope.LegalCase.ForeclosureInfo.MortNoteAssInCert) return false;
        if (!$scope.LegalCase.ForeclosureInfo.MissInCert || $scope.LegalCase.ForeclosureInfo.MissInCert.length == 0)
            return true;
        else return false;
    }

    $scope.initMissInCert = function () {
        return {
            dataSource: $scope.missingItems,
            valueExpr: 'id',
            displayExpr: 'label',
            onValueChanged: function (e) {
                e.model.updateMissInCertValue(e.values);
            }
        };
    }

    $scope.ShowAddPopUp = function (event) {
        $scope.addCommentTxt = "";
        aspxAddLeadsComments.ShowAtElement(event.target);
    }

    $scope.SaveLegalComments = function () {

        $scope.LegalCase.LegalComments.push({ id: $scope.LegalCase.LegalComments.length + 1, Comment: $scope.addCommentTxt });
        $scope.SaveLegal(function () {
            console.log("ADD comments" + $scope.addCommentTxt);
            aspxAddLeadsComments.Hide();
        });
    }

    $scope.DeleteComments = function (index) {
        $scope.LegalCase.LegalComments.splice(index, 1);
        $scope.SaveLegal(function () {
            console.log("Deleted comments");
        });
    }


    $scope.AddActivityLog = function () {
        if (typeof AddActivityLog === "function") {
            AddActivityLog($scope.MustAddedComment);
        }
    }

    $scope.CheckWorkHours = function () {
        $http.get("/api/WorkingLogs/Legal/" + $scope.LegalCase.BBLE).success(function (data) {
            $scope.TotleHours = data;
            $("#WorkPopUp").modal();
        });
    }

    $scope.showHistory = function () {
        var url = "/api/legal/SaveHistories/" + $scope.LegalCase.BBLE;
        $scope.History = [];
        $http.get(url).success(function (data) {
            $scope.History = data;
            $("#HistoryPopup").modal();
        })
    }

    $scope.loadHistoryData = function (logid) {
        if (logid) {
            var url = "/api/Legal/HistoryCaseData/" + logid;
            $http.get(url).success(function (data) {
                $scope.LegalCase = $.parseJSON(data);
                var BBLE = $scope.LegalCase.BBLE;
                if (BBLE) {
                    var leadsInfoUrl = "/ShortSale/ShortSaleServices.svc/GetLeadsInfo?bble=" + BBLE;
                    var shortsaleUrl = '/ShortSale/ShortSaleServices.svc/GetCaseByBBLE?bble=' + BBLE;
                    var taxlienUrl = '/api/TaxLiens/' + BBLE;
                    var legalecoursUrl = "/api/LegalECourtByBBLE/" + BBLE;


                    $scope.LegalCase.LegalComments = $scope.LegalCase.LegalComments || [];
                    $scope.LegalCase.ForeclosureInfo = $scope.LegalCase.ForeclosureInfo || {};
                    $scope.LogChange = {
                        'TaxLienFCStatus': { "old": $scope.LegalCase.TaxLienFCStatus, "now": function () { return $scope.LegalCase.TaxLienFCStatus; }, "msg": 'Tax Lien FC Status changed from ' },
                        'CaseStauts': { "old": $scope.LegalCase.CaseStauts, "now": function () { return $scope.LegalCase.CaseStauts; }, "msg": 'Mortgae foreclosure Status changed from ' }
                    }
                    var arrays = ["AffidavitOfServices", "Assignments", "MembersOfEstate"];
                    for (a in arrays) {
                        var porp = arrays[a]
                        var array = $scope.LegalCase.ForeclosureInfo[porp];
                        if (!array || array.length === 0) {
                            $scope.LegalCase.ForeclosureInfo[porp] = [];
                            $scope.LegalCase.ForeclosureInfo[porp].push({});
                        }
                    }
                    $scope.LegalCase.SecondaryTypes = $scope.LegalCase.SecondaryTypes || []
                    $scope.showSAndCFrom();

                    $http.get(shortsaleUrl)
                        .success(function (data) {
                            $scope.ShortSaleCase = data;
                        }).error(function () {
                            alert("Fail to Short sale case  data : " + BBLE);
                        });



                    $http.get(leadsInfoUrl)
                        .success(function (data) {
                            $scope.LeadsInfo = data;
                            $scope.LPShow = $scope.ModelArray('LeadsInfo.LisPens');
                        }).error(function (data) {
                            alert("Get Short Sale Leads failed BBLE =" + BBLE + " error : " + JSON.stringify(data));
                        });

                    $http.get(taxlienUrl)
                        .success(function (data) {
                            $scope.TaxLiens = data;
                            $scope.TaxLiensShow = $scope.ModelArray('TaxLiens');
                        }).error(function (data) {
                            alert("Get Tax Liens failed BBLE = " + BBLE + " error : " + JSON.stringify(data));
                        });

                    $http.get(legalecoursUrl)
                        .success(function (data) {
                            $scope.LegalECourt = data;
                        }).error(function () {
                            $scope.LegalECourt = null;
                        });

                    LegalCaseBBLE = BBLE;
                }
            }).error(function () {
                alert("Fail to load data : ");
            });

        }


    }

    $scope.openHistoryWindow = function (logid) {
        $window.open('/LegalUI/Legalinfo.aspx?logid=' + logid, '_blank', 'width=1024, height=768')
    }
}]);
var PreSignHelper = (function () {
    var onAccoutingReview = function (cellinfo) {
        var element = angular.element('#pt-preassign-accouting-ctrl');
        if (element.length > 0) {
            scope = element.scope();
            scope.load(cellinfo);
        }
    }

    return {
        onAccoutingReview: onAccoutingReview,
    }
})();


var portalApp = angular.module('PortalApp');


portalApp.config(function (portalRouteProvider) {

    var newPreSignResolve = ['$route', 'PreSign', function ($route, PreSign) {
        var preSign = new PreSign();
        preSign.BBLE = $route.current.params.BBLE.toString();
        return preSign;
    }];

    var preSignListResolve = ['PreSign', function (PreSign) {
        return PreSign.query();
    }];
    var preSignItemResolve = ['$route', 'PreSign', function ($route, PreSign) {
        var preSignId = $route.current.params[portalRouteProvider.ITEM_ID]
        return PreSign.get({ Id: preSignId });
    }];

    var preSignFinanceListResolve = ['PreSign', function (PreSign) {
        return PreSign.financeList();
    }];


    var config = portalRouteProvider.routesFor('preAssign')
        .whenNew({ PreSignItem: newPreSignResolve })
        .whenEdit({ PreSignItem: preSignItemResolve })
        .whenView({ PreSignItem: preSignItemResolve })
        .whenList({ PreSignList: preSignListResolve })
        .whenOther({ PreSignFinaceList: preSignFinanceListResolve }, 'Finance', 'list')

});
var CONSTANT_ASSIGN_PARTIES_GRID_OPTION = {
    bindingOptions: {
        dataSource: 'preAssign.Parties'
    },
    paging: {
        pageSize: 10
    },
    pager: {
        showPageSizeSelector: true,
        allowedPageSizes: [5, 10, 20],
        showInfo: true
    },
    columns: [{
        dataField: "Name",
        validationRules: [{
            type: "required"
        }]
    }],
    sorting: { mode: 'none' },
    summary: {
        totalItems: [{
            column: "Name",
            summaryType: "count"
        }]
    },
    initRowText: function () {
        self = this;
        self.editing.texts = { deleteRow: 'Delete' }
    }
}

var CONSTANT_ASSIGN_CHECK_GRID_OPTION = {
    bindingOptions: {
        dataSource: 'preAssign.CheckRequestData.Checks',
    },
    sorting: { mode: 'none' },
    paging: {
        pageSize: 10
    },

    pager: {

        showInfo: true
    },
    wordWrapEnabled: true,
    columns: [{
        dataField: "PaybleTo",
        caption: 'Payable To',
        validationRules: [{
            type: "required"
        }]
    }, {
        dataField: 'Amount',
        dataType: 'number',
        format: 'currency',
        precision: 2,
        validationRules: [{
            type: "required"
        }]
    }, new dxGridColumnModel(
    {
        dataField: 'Date',
        dataType: 'date',
        caption: 'Date of Release',
        validationRules: [{
            type: "required"
        }]
    }), {
        dataField: 'Description',
        validationRules: [{
            type: "required"
        }]
    }, {
        dataField: 'Comments',
        caption: 'Void Reason'
    }],
    initEdit: function () {
        var self = this;
        var voidReasonColumn = {
            dataField: 'Comments',
            caption: 'Void Reason',
            allowEditing: false
        }
        if (self.columns.indexOf(voidReasonColumn) < 0) {
            self.columns.push(voidReasonColumn)
        }
    },
    summary: {
        calculateCustomSummary: function (options) {
            if (options.name == 'SumAmount') {
                options.totalValue = _.sum(_.filter(options.component._options.dataSource, function (o) {
                    return o.Status != 1;
                }), "Amount"); 
            }
        },
        totalItems: [{
            column: "Name",
            summaryType: "count"
        }, {
            name: "SumAmount",
            showInColumn: "Amount",
            summaryType: "sum",
            displayFormat: "Sum: {0}",
            valueFormat: "currency",
            precision: 2,
            summaryType: "custom"
        }]
    }
};

var CONSTANT_ASSIGN_LIST_GRID_OPTION = {
    bindingOptions: {
        dataSource: 'preSignList'
    },
    headerFilter: {
        visible: true
    },
    searchPanel: {
        visible: true,
        width: 250
    },
    paging: {
        pageSize: 10
    },
    onRowPrepared: function (rowInfo) {
        if (rowInfo.rowType != 'data')
            return;
        rowInfo.rowElement
        .addClass('myRow');
    },
    columnAutoWidth: true,
    columns: [{
        dataField: 'Title',
        caption: 'Address',
        cellTemplate: function (container, options) {
            $('<a/>').addClass('dx-link-MyIdealProp')
                .text(options.value)
                .on('dxclick', function () {
                    var request = options.data;
                    PortalUtility.OpenWindow('/NewOffer/HomeownerIncentive.aspx#/preassign/view/' + request.Id, 'Pre Sign ' + request.BBLE, 800, 900);
                })
                .appendTo(container);
        }
    }, {
        dataField: 'CreateBy',
        caption: 'Request By'
    }, new dxGridColumnModel({
        dataField: 'CreateDate',
        caption: 'Request Date',
        dataType: 'date'
    }), new dxGridColumnModel({
        dataField: 'ExpectedDate',
        caption: 'Expected Date Of Sign',
        dataType: 'date'
    }), {
        dataField: 'DealAmount',
        format: 'currency',
        dataType: 'number',
        precision: 2
    }],
    wordWrapEnabled: true
}


portalApp.controller('preAssignEditCtrl', ['$scope', 'ptCom', 'PreSignItem', 'DxGridModel', '$location', 'PortalHttpInterceptor', '$http',
function ($scope, ptCom, PreSignItem, DxGridModel, $location, PortalHttpInterceptor, $http) {

        $scope.preAssign = PreSignItem;
    $scope.preAssign.CreateBy = $scope.preAssign.CreateBy || $('#currentUser').val();
    $scope.preAssign.CheckRequestData = $scope.preAssign.CheckRequestData || { Type: 'Short Sale', Checks: [] };
    if (!$scope.preAssign.Id) {
        $scope.preAssign.CheckRequestData = { Type: 'Short Sale', Checks: [] };
        $scope.preAssign.Parties = [];
        $scope.preAssign.NeedSearch = true;
        $scope.preAssign.NeedCheck = true;
    }
    if ($scope.preAssign.BBLE) {
        $http.get('/api/Leads/LeadsInfo/' + $scope.preAssign.BBLE, { options: { noError: true } })
             .then(function (d) {
                 $scope.preAssign.Title = d.data.PropertyAddress;
             });
    }
    $scope.accoutingMode = $("#accoutingMode").length > 0 || false;

    $scope.Save = function () { 
        if ($scope.preAssign.validation()) {
            if ($scope.preAssign.hasId()) {
                $scope.preAssign.$update(function () {
                    $location.path('/preassign/view/' + $scope.preAssign.Id);
                })
            } else {
                $scope.preAssign.$save(function () {
                    $location.path('/preassign/view/' + $scope.preAssign.Id);
                })
            }
        } else {
            var msg = $scope.preAssign.getErrorMsgStr();
            AngularRoot.alert(msg);
        }

    }

    $scope.CheckByBBLE = function () {
        var preAssign = $scope.preAssign;
        if (preAssign.$promise != null) {
            return;
        }
        if (preAssign.Id == 0 || preAssign.Id == null) {
            preAssign.$getByBBLE(function () {
                $location.path('/preassign/view/' + preAssign.Id);
            })
        }
    }
    $scope.CheckByBBLE();

    $scope.partiesGridOptions = new DxGridModel(CONSTANT_ASSIGN_PARTIES_GRID_OPTION, {
        editMode: "cell",
    });
    $scope.checkGridOptions = {
        bindingOptions: {
            dataSource: 'preAssign.CheckRequestData.Checks',
            columns: 'scopeColumns'
        },
        editing: {
            editMode: 'cell',
            texts: {
                deleteRow: 'delete',
                confirmDeleteMessage: ''
            },
            editEnabled: true,
            removeEnabled: false,
            insertEnabled: true 
        },
        sorting: { mode: 'none' },
        pager: {

            showInfo: true
        },
        wordWrapEnabled: true,
        summary: {
            calculateCustomSummary: function (options) {
                if (options.name == 'Balance') {
                     var AmountTotal = _.sum(_.filter(options.component._options.dataSource, function (o) {
                        return o.Status != 1;
                     }), "Amount");
                     var ConfirmedTotal = _.sum(_.filter(options.component._options.dataSource, function (o) {
                         return o.Status != 1;
                     }), "ConfirmedAmount");

                     options.totalValue = AmountTotal - ConfirmedTotal;
                }
            },
            totalItems: [ {
                name: "Balance",
                showInColumn: "Status",
                displayFormat: "Balance: {0}",
                valueFormat: "currency",
                precision: 2,
                summaryType: "custom"
            }]
        }
    };
    $scope.scopeColumns = [{
        caption: 'Status',
        cellTemplate: function (cellElement, cellInfo) {
            var Status = cellInfo.data && cellInfo.data.Status;
            switch (Status) {
                case 0:
                    cellElement.html('Unpaid');
                    break;
                case 1:
                    cellElement.html('Void');
                    break;
                case 2:
                    cellElement.html('Processed');
                    break;
                default:
                    cellElement.html('Unpaid');
            }
        },
        visible: true,
    }, {
        dataField: "PaybleTo",
        caption: 'Payable To',
        validationRules: [{
            type: "required"
        }]
    }, {
        dataField: 'Amount',
        dataType: 'number',
        format: 'currency',
        precision: 2,
        validationRules: [{
            type: "required"
        }]
    }, new dxGridColumnModel(
    {
        dataField: 'Date',
        dataType: 'date',
        caption: 'Date of Release',
        validationRules: [{
            type: "required"
        }]
    }), {
        dataField: 'Description',
        validationRules: [{
            type: "required"
        }]
    }, {
        dataField: 'Comments',
        caption: 'Void Reason',
        allowEditing: false
    }, ];
    $scope.checkGridOptions.onRowPrepared = function (e) {
        if (e.data && e.data.Status == 1) {
            e.rowElement.addClass('avoid-check');
        }
    }
    $scope.checkGridOptions.onEditingStart = function (e) {
        if (e.data.Status == 1 || e.data.CheckId) {
            e.cancel = true;
        }
    }

    $scope.AddCheck = function (e) {
        var cancel = false;
        e.data.RequestId = $scope.preAssign.CheckRequestData.RequestId;
        e.data.Date = new Date(e.data.Date).toISOString();
        var response = $.ajax({
            url: '/api/PreSign/' + $scope.preAssign.Id + '/AddCheck/' + $scope.preAssign.NeedCheck,
            type: 'POST',
            dataType: 'json',
            async: false,
            data: e.data,
            success: function (data, textStatus, xhr) {
                $scope.addedCheck = data;
                e.cancel = true;
                e.component.refresh();
                var pageExpectedDate = $scope.preAssign.ExpectedDate;
                var pageParties = $scope.preAssign.Parties;
                angular.extend($scope.preAssign, data) 
                if (pageExpectedDate) {
                    $scope.preAssign.ExpectedDate = pageExpectedDate;

                }
                if (pageParties) {
                    $scope.preAssign.Parties = pageParties;
                }

            }
        });
        var message = PortalHttpInterceptor.BuildAjaxErrorMessage(response);
        if (message) {
            AngularRoot.alert(message);
            e.cancel = true;
        };
        return cancel;
    }
    $scope.CancelCheck = function (e) {
        if (e.data.Status == 1) {
            $('#gridChecks').dxDataGrid('instance').refresh();
            return;
        }
        AngularRoot.prompt("Please input void reason", function (voidReason) {
            if (voidReason) {
                var response = $.ajax({
                    url: '/api/businesscheck/' + e.data.CheckId,
                    type: 'DELETE',
                    data: JSON.stringify(voidReason),
                    contentType: "application/json",
                    dataType: "json",
                    async: false,
                    success: function (data, textStatus, xhr) {
                        e.data.Status = 1;
                        e.data.Comments = voidReason;
                        $('#gridChecks').dxDataGrid('instance').refresh();
                    }
                });
                var message = PortalHttpInterceptor.BuildAjaxErrorMessage(response);
                if (message) {
                    AngularRoot.alert(message);
                };
            }
        }, true)
        $('#gridChecks').dxDataGrid('instance').refresh();
    }

    if ($location.path().indexOf('new') < 0) {
        $scope.checkGridOptions.onRowInserting = $scope.AddCheck;
        var accounting_col = {
            caption: 'Accouting',
            cellTemplate: function (cellElement, cellInfo) {
                var checkId = cellInfo.data && cellInfo.data.CheckId;
                var Status = cellInfo.data && cellInfo.data.Status;
                if (!checkId || Status == 1) return;
                var $button = $('<span type="button" class="btn btn-sm btn-default">').text('Process').on("click", $.proxy(PreSignHelper.onAccoutingReview, this, cellInfo));
                cellElement.append($button);
            },
            visible: false,
        }
        var void_col = {
            caption: 'Void',
            cellTemplate: function (cellElement, cellInfo) {
                var checkId = cellInfo.data && cellInfo.data.CheckId;
                var Status = cellInfo.data && cellInfo.data.Status;

                if (checkId
                    && (($scope.accoutingMode && Status == 2)
                    || (!$scope.accoutingMode && Status == 0))
                 ) {
                    var $button = $('<span type="button" class="btn btn-sm btn-default">').text('void').on("click", $.proxy($scope.CancelCheck, this, cellInfo));
                    cellElement.append($button);
                }
            },
            visible: true
        }
        $scope.scopeColumns.push(accounting_col);
        $scope.scopeColumns.push(void_col);
        $scope.$watch('preAssign.NeedCheck', function (newvalue, oldvalue) {
            if (oldvalue == true && newvalue == false) {
                $scope.preAssign.NeedCheck = true;
                ptCom.alert("Cannot change check request from yes to no.")
            }
        });
    }
    $scope.test = function () {
        console.log('test');
    }
    $scope.$on('$viewContentLoaded', function () {
        var checkGrid = $('#gridchecks').dxDataGrid('instance');
        if (checkGrid) {
            checkGrid.refresh();
        }
        var accoutingMode = $("#accoutingMode");
        if (accoutingMode.length > 0 && $location.path().indexOf('new') < 0) {
            $scope.scopeColumns[6].visible = true;
        }
    })

}]);

portalApp.controller('preAssignViewCtrl', ['$scope', 'PreSignItem', 'DxGridModel', 'CheckRequest',
function ($scope, PreSignItem, DxGridModel, CheckRequest) {
    $scope.preAssign = PreSignItem;
    $scope.preAssign.CheckRequestData = $scope.preAssign.CheckRequestData || { Checks: [{}] };
    $scope.partiesGridOptions = new DxGridModel(CONSTANT_ASSIGN_PARTIES_GRID_OPTION);
    $scope.checkGridOptions = {
        bindingOptions: {
            dataSource: 'preAssign.CheckRequestData.Checks',
            columns: 'scopeColumns'
        },
        sorting: { mode: 'none' },
        paging: {
            pageSize: 10
        },
        pager: {
            showInfo: true
        },
        wordWrapEnabled: true,
        initEdit: function () {
            var self = this;
            var voidReasonColumn = {
                dataField: 'Comments',
                caption: 'Void Reason',
                allowEditing: false
            }
            if (self.columns.indexOf(voidReasonColumn) < 0) {
                self.columns.push(voidReasonColumn)
            }
        },
        summary: {
            calculateCustomSummary: function (options) {
                if (options.name == 'Balance') {
                    var AmountTotal = _.sum(_.filter(options.component._options.dataSource, function (o) {
                        return o.Status != 1;
                    }), "Amount");
                    var ConfirmedTotal = _.sum(_.filter(options.component._options.dataSource, function (o) {
                        return o.Status != 1;
                    }), "ConfirmedAmount");

                    options.totalValue = AmountTotal - ConfirmedTotal;
                }
            },
            totalItems: [{
                name: "Balance",
                showInColumn: "Status",
                displayFormat: "Balance: {0}",
                valueFormat: "currency",
                precision: 2,
                summaryType: "custom"
            }]
        }
    };
    $scope.scopeColumns = [{
        caption: 'Status',
        cellTemplate: function (cellElement, cellInfo) {
            var Status = cellInfo.data && cellInfo.data.Status;
            switch (Status) {
                case 0:
                    cellElement.html('Unpaid');
                    break;
                case 1:
                    cellElement.html('Void');
                    break;
                case 2:
                    cellElement.html('Processed');
                    break;
                default:
                    cellElement.html('Unpaid');
            }
        },
        visible: true,
    }, {
        dataField: "PaybleTo",
        caption: 'Payable To',
        validationRules: [{
            type: "required"
        }]
    }, {
        dataField: 'Amount',
        dataType: 'number',
        format: 'currency',
        precision: 2,
        validationRules: [{
            type: "required"
        }]
    }, new dxGridColumnModel(
        {
            dataField: 'Date',
            dataType: 'date',
            caption: 'Date of Release',
            validationRules: [{
                type: "required"
            }]
        }), {
            dataField: 'Description',
            validationRules: [{
                type: "required"
            }]
        }, {
            dataField: 'Comments',
            caption: 'Void Reason'
        }, ],
    $scope.CheckRowPrepared = function (e) {
        if (e.data && e.data.Status == 1) {
            e.rowElement.addClass('avoid-check');
        }
    }
    $scope.checkGridOptions.onRowPrepared = $scope.CheckRowPrepared;

    $scope.$on('$viewContentLoaded', function (e) {
        var checkGrid = $('#gridchecks').dxDataGrid('instance');
        if (checkGrid) {
            checkGrid.refresh();
        }

        $("#preDealForm input").prop("disabled", true);
        $("#preDealForm select").prop("disabled", true);

    });
    $scope.$watch('preAssign.NeedCheck', function (newvalue, oldvalue) {
        if (oldvalue != null) {
            $scope.preAssign.NeedCheck = oldvalue;
        }
    });
}]);

portalApp.controller('preAssignFinanceCtrl', ['$scope', 'PreSignFinaceList',
function ($scope, PreSignFinaceList) {
    $scope.preSignList = PreSignFinaceList;
    $scope.CheckRowPrepared = function (e) {
        if (e.data && e.data.Status == 1) {
            e.rowElement.addClass('avoid-check');
        }
    }
    $scope.preSignRecordsGridOpt = angular.extend({}, CONSTANT_ASSIGN_LIST_GRID_OPTION);
    $scope.preSignRecordsGridOpt.masterDetail = {
        enabled: true,
        template: function (container, options) {
            var opt = {
                dataSource: options.data.Checks,
                columnAutoWidth: true,
                columns: [{
                    dataField: 'PaybleTo',
                    caption: 'Payable To',
                }, {
                    dataField: 'Amount',
                    format: 'currency',
                    dataType: 'number',
                    precision: 2
                }, {
                    dataField: 'Date',
                    caption: 'Date of Release',
                    dataType: 'date',
                    format: 'shortDate'
                }, {
                    dataField: 'Description'
                }, {
                    dataField: 'Comments',
                    caption: 'Void Reason'
                }],
                onRowPrepared: $scope.CheckRowPrepared,
            }
            $("<div>").text("Checks: ").appendTo(container);
            $("<div>")
                .addClass("internal-grid")
                .dxDataGrid(opt).appendTo(container);

        }
    }
    $scope.preSignRecordsGridOpt.selection = null;
    $scope.preSignRecordsGridOpt.columns = [{
        dataField: 'PropertyAddress',
        caption: 'Address'
    }, {
        dataField: 'RequestBy',
        caption: 'Request By'
    }, {
        dataField: 'Type',
        caption: 'Request Type'
    }, {
        dataField: 'RequestDate',
        caption: 'Request Date',
        dataType: 'date'
    }, {
        dataField: 'CheckAmount',
        format: 'currency',
        dataType: 'number',
        precision: 2
    }, ]

}]);

portalApp.controller('preAssignListCtrl', ['$scope', 'PreSignList',
function ($scope, PreSignList) {
    $scope.preSignList = PreSignList;
    $scope.preSignRecordsGridOpt = angular.extend({}, CONSTANT_ASSIGN_LIST_GRID_OPTION);
}]);

portalApp.controller('ptPreAssignAccoutingCtrl', ['$scope', '$http', '$uibModal', 'ptCom',
function ($scope, $http, $uibModal, ptCom) {
    $scope.cellinfo = {};
    $scope.editmode = false;
    $scope.toggleEdit = function () {
        if ($scope.editmode) {
            $scope.editmode = false;
        } else {
            $scope.editmode = true;
        }
    }
    $scope.load = function (cellinfo) {
        $scope.cellinfo = cellinfo;
        $scope.editmode = false;
        if ($scope.cellinfo.data.Status == 0) {
            if ($scope.cellinfo.data.ConfirmedAmount == null) {
                $scope.cellinfo.data.ConfirmedAmount = $scope.cellinfo.data.Amount;
            }
            $scope.editmode = true;
        }

        $scope.modal = $uibModal.open({
            templateUrl: 'pt-preassign-accouting-modal',
            scope: $scope
        })
    }
    $scope.update = function () {
        ptCom.confirm("Are you going to Save?", function (response) {
            if (response) {
                $scope.closeModal();
                if ($scope.cellinfo.data.CheckId) {
                    $http({
                        url: '/api/BusinessCheck/' + $scope.cellinfo.data.CheckId + '/Process',
                        method: 'POST',
                        data: $scope.cellinfo.data
                    }).then(
                       function (d) {
                           $scope.cellinfo.data.Status = 2;
                           $('#gridChecks').dxDataGrid('instance').refresh();
                           ptCom.alert("Save Successful");
                       }, function () {
                           ptCom.alert("fail to save");
                       })
                }
            }
        })
        $('#gridChecks').dxDataGrid('instance').refresh();
    }
    $scope.closeModal = function () {
        if ($scope.modal) {
            $scope.modal.close();
        }
    }

}])


portalApp.controller('preAssignCtrl', function ($scope, ptCom, PortalHttpInterceptor, $http) {

    $scope.showHistroy = function () {
        auditLog.show(null, $scope.preAssign.Id);
    }

});
angular.module('PortalApp')
.controller("ReportWizardCtrl", function ($scope, $http, $timeout, ptCom) {
    $scope.camel = _.camelCase;

    $scope.step = 1;
    $scope.collpsed = [];
    $scope.CurrentQuery = null;
    $scope.reload = function (callback) {
        $scope.step = 1;
        $scope.CurrentQuery = null;
        $http.get(CUSTOM_REPORT_TEMPLATE)
            .then(function (res) {
                $scope.Fields = res.data[0].Fields;
                $scope.BaseTable = res.data[0].BaseTable;
                $scope.IncludeAppId = res.data[0].IncludeAppId;
                if (callback) callback();
            });
        $scope.loadSavedReport();
    };
    $scope.loadSavedReport = function () {
        $http.get("/api/Report/Load")
            .then(function (res) {
                $scope.SavedReports = res.data;
            });
    };
    $scope.deleteSavedReport = function (q) {
        var _confirm = confirm("Are you sure to delete?");
        if (_confirm) {
            if (q.ReportId) {
                $http({
                    method: "DELETE",
                    url: "/api/Report/Delete/" + q.ReportId,
                }).then(function (res) {
                    $scope.reload();
                    alert("Delete Success.");
                });
            } else {
                alert("Delete Fails!");
            }
        }

    }; 
    $scope.load = function (q) {
        $scope.reload(
            function () {
                if (q.ReportId) {
                    $http.get("/api/Report/Load/" + q.ReportId)
                    .then(function (res) {
                        var data = res.data;
                        $scope.CurrentQuery = data;
                        $scope.Fields = JSON.parse(data.Query);
                        $scope.generate();

                        var gridState = JSON.parse(data.Layout);
                        $("#queryReport").dxDataGrid("instance").state(gridState);
                    });
                }
            }
        );
    };
    $scope.someCheck = function (category) {
        return _.some(category.fields, { checked: true });
    };
    $scope.addFilter = function (f) {
        if (!f.filters) f.filters = [];
        f.filters.push({});
    };
    $scope.removeFilter = function (f, i) {
        f.filters.splice(i, 1);
    };
    $scope.updateStringFilter = function (x) {
        if (!x.criteria || !x.input1) {
            x.WhereTerm = "";
            x.CompareOperator = "";
            x.value1 = "";
            return;
        } else {
            switch (x.criteria) {
                case "1":
                    x.WhereTerm = "CreateCompare";
                    x.CompareOperator = "Like";
                    x.value1 = x.input1.trim() + "%";
                    break;
                case "2":
                    x.WhereTerm = "CreateCompare";
                    x.CompareOperator = "Like";
                    x.value1 = "%" + x.input1.trim();
                    break;
                case "3":
                    x.WhereTerm = "CreateCompare";
                    x.CompareOperator = "Like";
                    x.value1 = "%" + x.input1.trim() + "%";
                    break;
                default:
                    x.WhereTerm = "";
                    x.CompareOperator = "";
                    x.value1 = "";
            }
        }
    };
    $scope.updateDateFilter = function (x) {
        if (!x.criteria || !x.input1) {
            x.WhereTerm = "";
            x.CompareOperator = "";
            x.value1 = "";
            return;
        } else {
            switch (x.criteria) {
                case "1":
                    x.WhereTerm = "CreateCompare";
                    x.CompareOperator = "Less";
                    x.value1 = x.input1;
                    break;
                case "2":
                    x.WhereTerm = "CreateCompare";
                    x.CompareOperator = "Greater";
                    x.value1 = x.input1;
                    break;
                case "3":
                    x.WhereTerm = "CreateCompare";
                    x.CompareOperator = "Equal";
                    x.value1 = x.input1;
                    break;
                default:
                    x.WhereTerm = "";
                    x.CompareOperator = "";
                    x.value1 = "";
            }
        }

    };
    $scope.updateNumberFilter = function (x) {

        if (!x.criteria || !x.input1 || (x.criteria == "5" && !x.input2)) {
            x.WhereTerm = "";
            x.CompareOperator = "";
            x.value1 = "";
            x.value2 = "";
            return;
        } else {
            switch (x.criteria) {
                case "0":
                    x.WhereTerm = "CreateCompare";
                    x.CompareOperator = "Equal";
                    x.value1 = x.input1.trim();
                    x.value2 = "";
                    break;
                case "1":
                    x.WhereTerm = "CreateCompare";
                    x.CompareOperator = "Less";
                    x.value1 = x.input1.trim();
                    x.value2 = "";
                    break;
                case "2":
                    x.WhereTerm = "CreateCompare";
                    x.CompareOperator = "LessOrEqual";
                    x.value1 = x.input1.trim();
                    x.value2 = "";
                    break;
                case "3":
                    x.WhereTerm = "CreateCompare";
                    x.CompareOperator = "Greater";
                    x.value1 = x.input1.trim();
                    x.value2 = "";
                    break;
                case "4":
                    x.WhereTerm = "CreateCompare";
                    x.CompareOperator = "GreaterOrEqual";
                    x.value1 = x.input1.trim();
                    x.value2 = "";
                    break;
                case "5":
                    x.WhereTerm = "CreateBetween";
                    x.CompareOperator = "";
                    x.value1 = x.input1.trim();
                    x.value2 = x.input2.trim();
                    break;
                default:
                    x.WhereTerm = "";
                    x.CompareOperator = "";
                    x.value1 = "";
                    x.value2 = "";
            }
        }
    };
    $scope.updateListFilter = function (x) {
        if (!x.input1 || x.input1.length < 1) {
            x.WhereTerm = "";
            x.CompareOperator = "";
            x.value1 = "";
        } else {
            x.WhereTerm = "CreateIn";
            x.CompareOperator = "";
            x.value1 = x.input1;
        }
    };    
    $scope.updateBooleanFilter = function (x) {
        if (!x.input1) {
            x.WhereTerm = "";
            x.CompareOperator = "";
            x.value1 = "";
        } else {
            switch (x.input1) {
                case "1":
                    x.WhereTerm = "CreateCompare";
                    x.CompareOperator = "Equal";
                    x.value1 = true;
                    break;
                case "0":
                    x.WhereTerm = "CreateCompare";
                    x.CompareOperator = "Equal";
                    x.value1 = false;
                    break;
                default:
                    x.WhereTerm = "";
                    x.CompareOperator = "";
                    x.value1 = "";
            }
        }
    };

    $scope.onSaveQueryPopCancel = function () {
        $scope.NewQueryName = '';
        $scope.SaveQueryPop = false;
    };
    $scope.onSaveQueryPopSave = function () {
        if (!$scope.NewQueryName) {
            alert("New query name is empty!");
            $scope.NewQueryName = '';
            $scope.SaveQueryPop = false;
        } else {

            var data = {};

            data.Name = $scope.NewQueryName;

            data.Query = JSON.stringify($scope.Fields);
            data.sqlText = $scope.sqlText;
            data.Layout = JSON.stringify($("#queryReport").dxDataGrid("instance").state());

            data = JSON.stringify(data);

            $http({
                method: "POST",
                url: "/api/Report/Save",
                data: data,
            }).then(function (res) {
                $scope.NewQueryName = '';
                $scope.SaveQueryPop = false;
                $scope.reload();
                alert("Save successful!");
            });
        }
    };

    $scope.update = function () {

        var data = $scope.CurrentQuery;

        data.Query = JSON.stringify($scope.Fields);
        data.sqlText = $scope.sqlText;
        data.Layout = JSON.stringify($("#queryReport").dxDataGrid("instance").state());

        data = JSON.stringify(data);

        $http({
            method: "POST",
            url: "/api/Report/Save",
            data: data,
        }).then(function (res) {
            $scope.NewQueryName = '';
            $scope.SaveQueryPop = false;
            $scope.reload();
            alert("Save successful!");
        });
    };
    $scope.isBindColumn = function (f) {

        if (!f.table || !f.column) {
            return false;
        } else {
            return true;
        }
    };
    $scope.next = function () {
        $scope.step = $scope.step + 1;
    };
    $scope.prev = function () {
        $scope.step = $scope.step - 1;
    };
    $scope.filterDate = function (model) {
        var dtPatn = /\d{4}-\d{2}-\d{2}/;
        if (model) {
            _.each(model, function (el, idx) {
                if (el) {
                    _.forOwn(el, function (v, k) {
                        if (v && typeof (v) === 'string' && v.match(dtPatn)) {
                            el[k] = ptCom.toUTCLocaleDateString(v);
                        }
                    });
                }

            });
        }
    };
    $scope.generate = function () {
        var result = [];
        var BaseTable = $scope.BaseTable ? $scope.BaseTable : '';
        var IncludeAppId = $scope.IncludeAppId ? $scope.IncludeAppId : '';
        _.each($scope.Fields, function (el, i) {
            _.each(el.fields, function (el, i) {
                if (el.checked) {
                    result.push(el);
                }
            });
        });
        if (result.length > 0) {
            $scope.step = 3;
            $http({
                method: "POST",
                url: "/api/Report/QueryData?baseTable=" + BaseTable + "&includeAppId=" + IncludeAppId,
                data: JSON.stringify(result),
            }).then(function (res) {
                var rdata = res.data[0];
                $scope.filterDate(rdata);
                $scope.reportData = rdata;
                $scope.sqlText = res.data[1];
            });
        } else {
            alert("Query is empty!");
        }
    };
    $scope.reload();
    var PreLoadReportId = $('#txtReportID').val()
    if (PreLoadReportId>0)
    {
        $scope.LoadByID = true;
        $scope.load({ReportId: PreLoadReportId,UseSql:true})
    }
});
angular.module("PortalApp")
.controller('ShortSaleCtrl', ['$scope', '$http', '$timeout', 'ptContactServices', 'ptCom', 
    function ($scope, $http, $timeout, ptContactServices, ptCom) {
        $scope.ptContactServices = ptContactServices;
        $scope.capitalizeFirstLetter = ptCom.capitalizeFirstLetter;
        $scope.formatName = ptCom.formatName;
        $scope.formatAddr = ptCom.formatAddr;
        $scope.ptCom = ptCom;
        $scope.MortgageTabs = [];
        $scope.SsCase = {
            PropertyInfo: { Owners: [{ isCorp: false }] },
            CaseData: {},
            Mortgages: [{}]
        };
        $scope.SsCaseApprovalChecklist = {};
        $scope.Approval_popupVisible = false;
        $http.get('/Services/ContactService.svc/getbanklist').success(function (data) {
            $scope.bankNameOptions = data;
            if ($scope.bankNameOptions) {
                $scope.bankNameOptions.push({Name:'N/A'});
            }

         }).error(function (data) {
            $scope.bankNameOptions = [];
        });
        $scope.ensurePush = function (modelName, data) { ptCom.ensurePush($scope, modelName, data); }
        $scope.MoveToConstruction = function (scuessfunc) {
            var json = $scope.SsCase;
            var data = { bble: leadsInfoBBLE };

            $http.post('ShortSaleServices.svc/MoveToConstruction', JSON.stringify(data))
                .success(function () {
                    if (scuessfunc) {
                        scuessfunc();
                    } else {
                        ptCom.alert("Move to Construction successful!");
                    }
                }).error(function (data1, status) {
                    ptCom.alert("Fail to save data. status :" + status + "Error : " + JSON.stringify(data1));
                });
        };

        $scope.MoveToTitle = function (scuessfunc) {
            var json = $scope.SsCase;
            var data = { bble: leadsInfoBBLE };

            $http.post('ShortSaleServices.svc/MoveToTitle', JSON.stringify(data))
                .success(function () {
                    if (scuessfunc) {
                        scuessfunc();
                    } else {
                        ptCom.alert("Move to Title successful !");
                    }
                }).error(function (data1, status) {
                    ptCom.alert("Fail to save data. status " + status + "Error : " + JSON.stringify(data1));
                });
        }; 

        $scope.GetShortSaleCase = function (caseId, callback) {
            if (!caseId) {
                console.log("Can not find case Id ");
                return;
            }

            ptCom.startLoading();
            var done1, done2;
            $http.get("ShortSaleServices.svc/GetCase?caseId=" + caseId)
                .success(function (data) {
                    $scope.SsCase = data;
                    leadsInfoBBLE = $scope.SsCase.BBLE;
                    window.caseId = caseId;

                    $http.get("ShortSaleServices.svc/GetLeadsInfo?bble=" + $scope.SsCase.BBLE).success(function (data1) {
                        $scope.ReloadedData = {};
                        $scope.SsCase.LeadsInfo = data1;
                        $('#CaseData').val(JSON.stringify($scope.SsCase));
                        if (callback) { callback(); }
                        done1 = true;
                        if (done1 && done2) {
                            ptCom.stopLoading();
                        }

                    }).error(function (data1) {
                        ptCom.stopLoading();
                        ptCom.alert("Get Short sale Leads failed BBLE =" + $scope.SsCase.BBLE + " error : " + JSON.stringify(data1));
                    });

                    $http.get('/LegalUI/LegalServices.svc/GetLegalCase?bble=' + leadsInfoBBLE).success(function (data1) {
                        $scope.LegalCase = data1;
                        done2 = true;
                        if (done1 && done2) {
                            ptCom.stopLoading();
                        }
                    }).error(function (data1) {
                        ptCom.stopLoading();
                        console.log("Fail to load data : " + leadsInfoBBLE + " :" + JSON.stringify(data1)); 
                    });
                }).error(function (data) {
                    ptCom.stopLoading();
                    ptCom.alert("Get Short sale failed CaseId= " + caseId + ", error : " + JSON.stringify(data));
                });
        };
        $scope.GetLoadId = function () {
            return window.caseId;
        };
        $scope.GetModifyUserUrl = function () {
            return 'ShortSaleServices.svc/GetModifyUserUrl?caseId=' + window.caseId;
        };

        $scope.NGAddArraryItem = function (item, model, popup) {
            if (model) {
                var array = $scope.$eval(model);
                if (!array) { $scope.$eval(model + '=[{}]'); }
                else { $scope.$eval(model + '.push({})'); }
            } else { item.push({}); }
            if (popup) { $scope.setVisiblePopup(item[item.length - 1], true); }

        };
        $scope.NGremoveArrayItem = function (item, index, disable) {
            var r = window.confirm("Delete This?");
            if (r) {
                if (disable) item[index].DataStatus = 3;
                else item.splice(index, 1);
            }
        };

        var UpdatedProperties = ['UpdateTime', 'UpdateDate', 'UpdateBy', 'OwnerId', 'MortgageId', 'OfferId', 'ValueId', 'CallbackDate', 'LastUpdate'];
        var autoSaveError = false;

        $scope.AutoSaveShortSale = function (callback) {
            var json = $scope.SsCase;
            var data = { caseData: JSON.stringify(json) };

            $http.post('ShortSaleServices.svc/SaveCase', JSON.stringify(data)).
                    success(function (data) {
                        autoSaveError = false;
                        RemoveDeletedMortgages();

                        SyncObjects(data, $scope.SsCase);

                        if (!callback) {
                            ptCom.alert("Save Successed !");
                        }

                        if (callback) { callback(); }

                    }).error(function (data1, status) {
                        if (!autoSaveError) {
                            autoSaveError = true;
                            var message = (data1 && typeof data1 == 'object' && data1.message) ? data1.message : JSON.stringify(data1);
                            ptCom.alert("Error in AutoSave. status " + status + "Error : " + message);
                        }
                    });
        };

        var RemoveDeletedMortgages = function () {
            _.remove($scope.SsCase.Mortgages, { DataStatus: 3 })
            console.log($scope.SsCase.Mortgages);
        }

        var SyncObjects = function (obj, toObj) {
            var copy = toObj;

            if (obj instanceof Date) {
                if (copy == null)
                    copy = new Date();

                copy = new Date();
                copy.setTime(obj.getTime());

                return;
            }

            if (obj instanceof Array) {
                if (copy == null)
                    copy = [];

                for (var i = 0, len = obj.length; i < len; i++) {
                    SyncObjects(obj[i], copy[i]);
                }

                return;
            }

            if (obj instanceof Object) {
                if (copy == null)
                    copy = {};

                for (var attr in obj) {
                    if (obj.hasOwnProperty(attr)) {
                        if (null == obj[attr] || "object" != typeof obj[attr]) {
                            if (typeof copy[attr] == 'undefined' || copy[attr] == null || copy[attr] != obj[attr]) {
                                if (UpdatedProperties.indexOf(attr) > 0) {
                                    copy[attr] = obj[attr];
                                }
                            }
                        }
                        else {
                            SyncObjects(obj[attr], copy[attr]);
                        }
                    }
                }

                return;
            }

            throw new Error("Unable to copy obj! Its type isn't supported.");
        }

        $scope.SaveShortSale = function (callback) {
            var json = $scope.SsCase;
            var data = { caseData: JSON.stringify(json) };

            $http.post('ShortSaleServices.svc/SaveCase', JSON.stringify(data)).
                    success(function () {
                        $scope.GetShortSaleCase($scope.SsCase.CaseId);
                        if (!callback) {
                            ptCom.alert("Save Successed !");
                        }

                        if (callback) { callback(); }

                    }).error(function (data1, status) {
                        var message = (data1 && typeof data1 == 'object' && data1.message) ? data1.message : JSON.stringify(data1);
                        ptCom.alert("Fail to save data. status " + status + "Error : " + message);
                    });
        };
        $scope.ShowAddPopUp = function (event) {
            $scope.addCommentTxt = "";
            aspxAddLeadsComments.ShowAtElement(event.target);
        };
        $scope.AddComments = function () {

            $http.post('ShortSaleServices.svc/AddComments', { comment: $scope.addCommentTxt, caseId: $scope.SsCase.CaseId }).success(function (data) {
                $scope.SsCase.Comments.push(data);
            }).error(function (data, status) {
                ptCom.alert("Fail to AddComments. status " + status + "Error : " + JSON.stringify(data));
            });

        };
        $scope.DeleteComments = function (index) {
            var comment = $scope.SsCase.Comments[index];
            $http.get('ShortSaleServices.svc/DeleteComment?commentId=' + comment.CommentId).success(function (data) {
                $scope.SsCase.Comments.splice(index, 1);
            }).error(function (data, status) {
                ptCom.alert("Fail to delete comment. status " + status + "Error : " + JSON.stringify(data));
            });
        };
        $scope.GetCaseInfo = function () {
            var CaseInfo = { Name: '', Address: '' };
            var caseName = $scope.SsCase.CaseName;
            if (caseName) {
                CaseInfo.Address = caseName.replace(/-(?!.*-).*$/, '');
                var matched = caseName.match(/-(?!.*-).*$/);
                if (matched && matched[0]) {
                    CaseInfo.Name = matched[0].replace('-', '');
                }
            }
            return CaseInfo;
        };

        $scope.setVisiblePopup = function (model, value) {
            if (model) model.visiblePopup = value;
            _.defer(function () { $scope.$apply(); });

        };


        $scope.approvalSave = function () {
            if ($scope.approvalSuccCallback) $scope.approvalSuccCallback();
            $scope.SaveShortSale();
            $scope.Approval_popupVisible = false;
        };
        $scope.approvalCancl = function () {
            if ($scope.approvalCanclCallback) $scope.approvalCanclCallback();
            $scope.Approval_popupVisible = false;
        };
        $scope.regApproval = function (succ, cancl) {
            if (!$scope.approvalSuccCallback) $scope.approvalSuccCallback = succ;
            if (!$scope.approvalCanclCallback) $scope.approvalCanclCallback = cancl;
        };
        $scope.toggleApprovalPopup = function () {
            $scope.$apply(function () {
                $scope.Approval_popupVisible = !$scope.Approval_popupVisible;
            });
        }; 

        $scope.ValuationWatchField = {
            Method: 'Type of Valuation',
            DateOfCall: 'Date of Call',
            AgentName: 'BPO Agent',
            AgentPhone: 'Agent Phone #',
            DateOfValue: 'Date of Valuation',
            TimeOfValuation: 'Time of Valuation',
            Access: 'Access',
            IsValuationComplete: 'Valuation Completed',
            DateComplate: 'Complete Date'
        };
        $scope.Valuation_popupVisible = false;
        $scope.Valuation_Show_Option = 1;
        $scope.addPendingValue = function () {
            $scope.SsCase.ValueInfoes.push({ Pending: true });
        };
        $scope.removePendingValue = function (el) {
            var index = $scope.SsCase.ValueInfoes.indexOf(el);
            $scope.NGremoveArrayItem($scope.SsCase.ValueInfoes, index);
        };
        $scope.ensurePendingValue = function () {
            var existPending = false;
            _.each($scope.SsCase.ValueInfoes, function (el, index) {
                if (el.Pending) existPending = true;
            });
            if (!existPending) $scope.addPendingValue();
        };
        $scope.setPendingModified = function () {
            $scope.oldPendingValues = [];
            _.each($scope.SsCase.ValueInfoes, function (el, index) {
                if (el.Pending) {
                    var newEl = {};
                    for (var property in el) {
                        if (el.hasOwnProperty(property)) {
                            newEl[property] = el[property];
                        }
                    }
                    $scope.oldPendingValues.push(newEl);
                }
            });
        };
        $scope.checkPendingModified = function () {
            var updates = '';
            _.each($scope.SsCase.ValueInfoes, function (el, index) {
                if (el.Pending) {
                    var oldEl = $scope.oldPendingValues.filter(function (e, i) { return e.$$hashKey == el.$$hashKey })[0];
                    if (!oldEl) {
                        for (var property in el) {
                            if ($scope.ValuationWatchField.hasOwnProperty(property)) {
                                updates += 'Valuation' + index + ' <b>' + $scope.ValuationWatchField[property] + '</b> changes to <b>' + el[property] + '</b><br/>';
                            }
                        }
                    } else {
                        for (var property in el) {
                            if ($scope.ValuationWatchField[property] && el[property] !== oldEl[property]) {
                                updates += 'Valuation' + index + ' <b>' + $scope.ValuationWatchField[property] + '</b> changes to <b>' + el[property] + '</b><br/>';
                            }
                        }
                    }

                }
            }); 
            return updates;
        };
        $scope.restorePendingModified = function () {
            _.remove($scope.SsCase.ValueInfoes, function (el, index) {
                return el.Pending;
            });
            _.each($scope.oldPendingValues, function (el, index) {
                $scope.SsCase.ValueInfoes.push(el);
            });
        };
        $scope.valuationCanl = function () {
            $scope.restorePendingModified();
            if ($scope.valuationCanclCallback) $scope.valuationCanclCallback();
            $scope.Valuation_popupVisible = false;
        };
        $scope.valuationSave = function () {
            var updates = $scope.checkPendingModified();
            if ($scope.valuationSuccCallback) $scope.valuationSuccCallback(updates);
            $scope.Valuation_popupVisible = false;

        };
        $scope.valuationCompl = function (el) {
            var updates = $scope.checkPendingModified();
            if ($scope.valuationSuccCallback) $scope.valuationSuccCallback(updates);
            el.Pending = false;
            $scope.Valuation_popupVisible = false;
        };
        $scope.regValuation = function (succ, cancl) {
            if (!$scope.valuationSuccCallback) $scope.valuationSuccCallback = succ;
            if (!$scope.valuationCanclCallback) $scope.valuationCanclCallback = cancl;
        };
        $scope.toggleValuationPopup = function (status) {
            $scope.$apply(function () {
                $scope.Valuation_Show_Option = status;
                $scope.setPendingModified();
                $scope.ensurePendingValue();
                $scope.Valuation_popupVisible = !$scope.Valuation_popupVisible;
            });
        }; 

        $scope.UpdateMortgageStatus = function (selType1, selStatusUpdate, selCategory) {
            var index = 0;
            switch (selType1) {
                case '2nd Lien':
                    index = 1;
                    break;
                case '3d Lien':
                    index = 2;
                    break;
                default:
                    index = 0;
            }

            $timeout(function () {
                if ($scope.SsCase.Mortgages[index]) {
                    $scope.SsCase.Mortgages[index].Category = selCategory;
                    $scope.SsCase.Mortgages[index].Status = selStatusUpdate;
                }

            });
        }; 
    }]);

var portalApp = angular.module('PortalApp');

portalApp.config(function (portalUIRouteProvider) {

    portalUIRouteProvider
        .statesFor('newoffer')

});


portalApp.controller('newofferNewofferCtrl', function ($scope) {
    $scope.text = 'newofferNewofferCtrl';
});
portalApp.controller('newofferSsinfoCtrl', function ($scope) {
    $scope.text = 'newofferSsinfoCtrl';
});
portalApp.controller('newofferCtrl', function ($scope) {
    $scope.text = 'newofferCtrl';
});


var portalApp = angular.module('PortalApp');

portalApp.controller('shortSalePreSignCtrl', function ($scope, ptCom, $http,
    ptContactServices, $location,PortalHttpInterceptor,

    PropertyOffer
    , WizardStep, Wizard, DivError, LeadsInfo, DocSearch,
    Team, NewOfferListGrid, ScopeHelper, QueryUrl, AssignCorp
   ) {

    $scope.ptContactServices = ptContactServices;
    $scope.QueryUrl = new QueryUrl();

    if ($scope.QueryUrl.model == 'List') {

        PropertyOffer.query(function (data) {
            $scope.newOfferGridOpt = new NewOfferListGrid(data);

        });
    }

    $scope.SSpreSign =     {
        Type: 'Short Sale',
        FormName: 'PropertyOffer',
        DealSheet: {
            ContractOrMemo: {
                Sellers: [{}],
                Buyers: [{}]
            },
            Deed: {
                Sellers: [{}]
            },
            CorrectionDeed: {
                Sellers: [{}],
                Buyers: [{}]
            }
        }
    };

        angular.extend($scope.SSpreSign, new PropertyOffer());
    $scope.SSpreSign.Type = $scope.SSpreSign.Type || 'Short Sale'
    $scope.SSpreSign.assignCrop = new AssignCorp();


    if (PortalUtility.QueryUrl().BBLE) {
        $scope.DocSearch = DocSearch.get(PortalUtility.QueryUrl());
    }

        $scope.DeadType = {
        ShortSale: false,
        Contract: true,
        Memo: false,
        Deed: false,
        CorrectionDeed: false,
        POA: false
    };
    $scope.ensurePush = function (modelName, data) {
        ptCom.ensurePush($scope, modelName, data);
    };
    $scope.arrayRemove = ptCom.arrayRemove;
    $scope.NGAddArraryItem = ptCom.AddArraryItem;
    $scope.GenerateDocument = function () {
        $http.post('/api/PropertyOffer/GeneratePackage/' + $scope.SSpreSign.BBLE, JSON.stringify($scope.SSpreSign)).success(function (url) {
            var oldUrl = window.location.href;
            STDownloadFile(url, $scope.SSpreSign.BBLE.trim() + '.zip');
            $scope.SSpreSign.Status = 2;

            $scope.constractFromData();
            $http.post('/api/businessform/', JSON.stringify($scope.SSpreSign)).success(function (formdata) {
                $scope.refreshSave(formdata);
                window.location.href = oldUrl;
            });
        })
    }
    $scope.shortSaleInfoNext = function () {

        var ss = ScopeHelper.getShortSaleScope();
        var _sellers = ss.SsCase.PropertyInfo.Owners;

        var _dealSheet = $scope.SSpreSign.DealSheet;
        var eMessages = new DivError('ShortSaleCtrl').getMessage();

        if (_.any(eMessages)) {
            AngularRoot.alert(eMessages.join(' <br />'));
            return false;
        }
        _dealSheet.CorrectionDeed.PropertyAddress = $scope.SSpreSign.PropertyAddress;
        var _sellers = _.map(_sellers, function (o) {
            o.Name = ss.formatName(o.FirstName, o.MiddleName, o.LastName);
            o.Address = $scope.SSpreSign.PropertyAddress; 
            o.PropertyAddress = $scope.SSpreSign.PropertyAddress;
            return o
        });

        _dealSheet.ContractOrMemo.Sellers = $.extend(true, _dealSheet.ContractOrMemo.Sellers || [], _sellers);
        _dealSheet.Deed.Sellers = $.extend(true, _dealSheet.Deed.Sellers || [], _sellers);
        _dealSheet.CorrectionDeed.Sellers = _dealSheet.CorrectionDeed.Sellers || [];
        _dealSheet.Deed.PropertyAddress = $scope.SSpreSign.PropertyAddress;
        return true;
    }

    $scope.SelectTeamChange = function () {
        var team = $scope.SSpreSign.assignCrop.Name;
        $scope.SSpreSign.assignCrop.Signer = null;
        $http.get('/api/CorporationEntities/CorpSigners?team=' + team).success(function (signers) {
            $scope.SSpreSign.assignCrop.signers = signers
        });
    }



    $scope.constractFromData = function () {
        var ss = ScopeHelper.getShortSaleScope();

        $scope.SSpreSign.DeadType = $scope.DeadType

        $scope.SSpreSign.SsCase = ss ? ss.SsCase : null;
        $scope.SSpreSign.Tag = $scope.SSpreSign.BBLE;
        $scope.SSpreSign.FormData = null;
        $scope.SSpreSign.FormData = JSON.stringify($scope.SSpreSign);
    }
    $scope.searchInfoNext = function () {
        var eMessages = $scope.getErrorMessage('preSignSearchInfo');
        if (_.any(eMessages)) {
            AngularRoot.alert(eMessages.join(' <br />'));
            return false;
        }

        var leadSearch = ScopeHelper.getLeadsSearchScope();
        return true;
    }
    $scope.getErrorMessage = function (id) {
        var eMessages = [];
        $('#' + id + ' ul:not(.form_ignore) .ss_warning:not(.form_ignore)').each(function () {
            eMessages.push($(this).attr('data-message'));
        });
        return eMessages
    }
    $scope.ContractNext = function () {
        var eMessages = $scope.getErrorMessage('preSignContract');
        if (_.any(eMessages)) {
            AngularRoot.alert(eMessages.join(' <br />'));
            return false;
        }
        return true;
    }


    $scope.DeedNext = function () {
        var deedCrop = $scope.SSpreSign.DealSheet.Deed;

        if (!deedCrop.EntityId) {

            var response = $.ajax({
                type: "POST",
                dataType: 'application/json',
                data: $scope.SSpreSign.DealSheet.Deed.Buyer,
                url: '/api/CorporationEntities/AssignDeedCorp?bble=' + $scope.SSpreSign.BBLE,
                async: false
            });

            var errorMsg = PortalHttpInterceptor.BuildAjaxErrorMessage(response);
            if (!errorMsg) {
                $scope.SSpreSign.DealSheet.Deed.EntityId = $scope.SSpreSign.DealSheet.Deed.Buyer.EntityId;
                return true;
            } else {
                AngularRoot.alert(errorMsg);
                deedCrop.EntityId = null;
                return false;
            }
        }

        var eMessages = $scope.getErrorMessage('preAssignDeed');
        if (_.any(eMessages)) {
            AngularRoot.alert(eMessages.join(' <br />'));
            return false;
        }
        return true;
    }
    $scope.preAssignCorrectionDeed = function () {
        var eMessages = $scope.getErrorMessage('preAssignCorrectionDeed');
        if (_.any(eMessages)) {
            AngularRoot.alert(eMessages.join(' <br />'));
            return false;
        }

        return true;
    }

    $scope.preAssignCorrectionPOA = function () {
        var eMessages = $scope.getErrorMessage('preSignPOA');
        if (_.any(eMessages)) {
            AngularRoot.alert(eMessages.join(' <br />'));
            return false;
        }
        return true;
    }

    $scope.onAssignCorpSuccessed = function (data) {
        $scope.SSpreSign.Status = 1;
        $scope.constractFromData();
        $http.post('/api/businessform/', JSON.stringify($scope.SSpreSign)).success(function (formdata) {
            $scope.refreshSave(formdata);
        });
    }
    $scope.AssignCorpSuccessed = function (data) {
        var _assignCrop = $scope.SSpreSign.assignCrop;
        $http.post('/api/CorporationEntities/Assign?bble=' + $scope.SSpreSign.BBLE, JSON.stringify(data)).success(function () {
            _assignCrop.Crop = data.CorpName;
            _assignCrop.CropData = data;



        });
    }

    $scope.assginCropClick = function () {
        var _assignCrop = $scope.SSpreSign.assignCrop;
        var eMessages = $scope.getErrorMessage('assignBtnForm');
        if (_.any(eMessages)) {
            AngularRoot.alert(eMessages.join(' <br />'));
            return false;
        }
        var assignApi = "/api/CorporationEntities/AvailableCorpBySigner?team=" + _assignCrop.Name + "&signer=" + _assignCrop.Signer;

        var confirmMsg = ' THIS PROCESS CANNOT BE REVERSED. Please confirm - The team is ' + _assignCrop.Name + ', and servicer is not Wells Fargo.';

        if (_assignCrop.isWellsFargo) {

            confirmMsg = ' THIS PROCESS CANNOT BE REVERSED. Please confirm - The team is ' + _assignCrop.Name + ', and Wells Fargo signer is ' + _assignCrop.Signer + '';
        }


        $http.get(assignApi).success(function (data) {

            AngularRoot.confirm(confirmMsg).then(function (r) {
                if (r) {
                    $scope.AssignCorpSuccessed(data);
                }
            });
        });

    }

    Team.getTeams(function (data) {
        $scope.CorpTeam = data;

    });

    $scope.AssignCropsNext = function () {

        var eMessages = $scope.getErrorMessage('preSignAssignCrops');
        if (_.any(eMessages)) {
            AngularRoot.alert(eMessages.join(' <br />'));
            return false;
        }
        var _dealSheet = $scope.SSpreSign.DealSheet;

        var _cropData = $scope.SSpreSign.assignCrop.CropData;
        _dealSheet.ContractOrMemo.Buyer = _cropData;
        return true;
    }


    $scope.DocRequiredNext = function (noAlert) {

        if (!_.any($scope.DeadType)) {
            if (!noAlert) {
                AngularRoot.alert("Please select at least one type to continue!")
            }
            return false;
        }
        return true;
    }
    $scope.DeedWizardInit = function () {
        var deedCrop = $scope.SSpreSign.DealSheet.Deed;

        if (!deedCrop.EntityId) {
            $http.get('/api/CorporationEntities/DeedCorpsByTeam?team=' + $scope.SSpreSign.assignCrop.Name).success(function (data) {
                $scope.SSpreSign.DealSheet.Deed.Buyer = data;

            });
        }

    }
    $scope.steps = [new WizardStep({
        title: "New Offer",
    }),

        new WizardStep({
            title: "Pre Sign",
            caption: 'SS Info',
            next: $scope.shortSaleInfoNext,
        }),

        new WizardStep({
            title: "Assign Crops",
            caption: 'Assign Corp',
            next: $scope.AssignCropsNext
        }),
        new WizardStep({
            title: "Documents Required",
            caption: 'Doc Required',
            next: $scope.DocRequiredNext
        }),

         new WizardStep({
             title: 'Contract',
             caption: 'Contract Or Memo',
             sheet: 'Contract',
             next: $scope.ContractNext
         }),
         new WizardStep({
             title: 'Deed',
             sheet: 'Deed',
             next: $scope.DeedNext,
             init: $scope.DeedWizardInit
         }),
         new WizardStep({
             title: 'CorrectionDeed',
             caption: 'Correction Deed',
             sheet: 'CorrectionDeed',
             next: $scope.preAssignCorrectionDeed
         }),
         new WizardStep({
             title: 'POA',
             sheet: 'POA',
             next: $scope.preAssignCorrectionPOA
         }), new WizardStep({
             title: "Finish",
             init: previewForm
         }),
    ];
    $scope.CheckSearchInfo = function (needSearch, searchCompleted) {
        var searchWized = {
            title: "Search Info",
            next: $scope.searchInfoNext
        };
        if (needSearch || searchCompleted) {
            $scope.steps.splice(1, 0, searchWized)
        } else {
        }
    }
    $scope.CheckSearchInfo($('.pt-need-search-input').val(), $('.pt-search-completed').val());

    $scope.CheckCurrentStep = function (BBLE) {
        $scope.SSpreSign = PropertyOffer.getByBBLE({ BBLE: BBLE.trim() }, function (data) {
            if (data.FormData) {
                if (data.FormData.DataId == 0)
                {
                    data.FormData.DataId = data.DataId;
                    data.FormData.CreateBy = data.CreateBy;
                    data.FormData.CreateDate = data.CreateDate;
                }

                angular.extend($scope.SSpreSign, data.FormData);

                $scope.DeadType = $scope.SSpreSign.DeadType;                
                $scope.SSpreSign.Status = data.BusinessData.Status;


                var ss = ScopeHelper.getShortSaleScope();
                if (ss) {
                    ss.SsCase = $scope.SSpreSign.SsCase;
                }
            } else {
                $scope.SSpreSign.BBLE = data.Tag;
            }

            if (!$scope.SSpreSign.assignCrop) {
                $scope.SSpreSign.assignCrop = new AssignCorp();
            } else {
                var _new = new AssignCorp();
                var obj = $scope.SSpreSign.assignCrop;
                angular.extend(_new, obj);
                angular.extend(obj, _new);
                $scope.SSpreSign.assignCrop = _new;
            }
            $scope.SSpreSign.assignOfferId($scope.onAssignCorpSuccessed);

            if (!$scope.SSpreSign.DealSheet) {
                $scope.SSpreSign.DealSheet = $scope.SSpreSign.DealSheetMetaData;
            }

                                    if (BBLE) {
                LeadsInfo.get({ BBLE: BBLE.trim() }, function (data) {
                    $scope.SSpreSign.PropertyAddress = data.PropertyAddress;
                    $scope.SSpreSign.BBLE = BBLE;
                });
            }
            $scope.SSpreSign.Type = $scope.SSpreSign.Type || 'Short Sale'
        });
    }

    var BBLE = $("#BBLE").val();
    if (BBLE) {
        LeadsInfo.get({ BBLE: BBLE.trim() }, function (data) {
            $scope.SSpreSign.PropertyAddress = data.PropertyAddress;
            $scope.SSpreSign.BBLE = BBLE;
        });
        $scope.CheckCurrentStep(BBLE);
    }

    $scope.step = 1;

    $scope.wizard = new Wizard();
    $scope.filteredSteps = [];

    $scope.wizard.setFilteredSteps($scope.filteredSteps);
    $scope.wizard.setScope($scope);

    $scope.MaxStep = function () {
        return $scope.filteredSteps.length;
    }

    $scope.currentStep = function () {
        return $scope.filteredSteps[$scope.step - 1];
    }

    $scope.refreshSave = function (formdata) {
        $scope.SSpreSign.DataId = formdata.DataId;
        $scope.SSpreSign.Tag = formdata.Tag;
        $scope.SSpreSign.CreateDate = formdata.CreateDate;
        $scope.SSpreSign.CreateBy = formdata.CreateBy;

        if (!$scope.SSpreSign.assignCrop) {
            $scope.SSpreSign.assignCrop = new AssignCorp();
        } else {
            var _new = new AssignCorp();
            var obj = $scope.SSpreSign.assignCrop;
            angular.extend(_new, obj);
            angular.extend(obj, _new);
            $scope.SSpreSign.assignCrop = _new;
        }
    }
    $scope.NextStep = function () {
        var cStep = $scope.currentStep();
        if (cStep.next) {
            if (cStep.next()) {
                $scope.constractFromData();
                $http.post('/api/businessform/', JSON.stringify($scope.SSpreSign)).success(function (formdata) {
                    $scope.SSpreSign.refreshSave(formdata);
                    $scope.step++;
                    cStep = $scope.currentStep();
                    if (cStep.init) {
                        cStep.init();
                    }
                })
            }
        } else {
            $scope.step++;
        }

    }
    $scope.PrevStep = function () {
        $scope.step--;
    }
    $scope.borrwerGrid = {
        dataSource: [{
            Name: "Borrower 1",
            "Address": "123 Main ST, New York NY 10001",
            "SSN": "123456789",
            "Phone": "212 123456",
            "Email": "email@gmail.com",
            "MailAddress": "123 Main ST, New York NY 10001"
        }],
        columns: ['Name', 'Address', 'SSN', 'Email', 'Phone', {
            dataField: "Type",
            lookup: {
                dataSource: ["Borrower", "Co-Borrower"],
            }
        }, 'MailAddress'],
        editing: {
            editEnabled: true,
            insertEnabled: true,
            removeEnabled: true
        }
    }
})
portalApp.filter('wizardFilter', function () {
    return function (items, sheetFilter) {
        var filtered = [];
        angular.forEach(items, function (item) {
            if (typeof item.sheet != 'undefined') {
                if (!sheetFilter) {
                    console.error("there are no filter please check filter")
                }
                for (key in sheetFilter) {
                    if (sheetFilter[key] && item.sheet == key) {
                        filtered.push(item)
                    }
                }
            } else {
                filtered.push(item);
            }

        });
        return filtered;
    };
});
portalApp.filter('ordered', function () {
    return function (item) {

        var orderDic = {
            "1": '1st',
            "2": "2nd",
            "3": "3rd"
        };

        return orderDic[item];
    };
});






angular.module("PortalApp")
.controller("TitleController", ['$scope', '$http', 'ptCom', 'ptContactServices', 'ptLeadsService', 'ptShortsSaleService', function ($scope, $http, ptCom, ptContactServices, ptLeadsService, ptShortsSaleService) {
    $scope.OwnerModel = function (name) {
        this.name = name;
        this.Mortgages = [{}];
        this.Lis_Pendens = [{}];
        this.Judgements = [{}];
        this.ECB_Notes = [{}];
        this.PVB_Notes = [{}];
        this.Bankruptcy_Notes = [{}];
        this.UCCs = [{}];
        this.FederalTaxLiens = [{}];
        this.MechanicsLiens = [{}];
        this.TaxLiensSaleCerts = [{}];
        this.VacateRelocationLiens = [{}];
        this.shownlist = [false, false, false, false, false, false, false, false, false, false, false];
    };
    $scope.FormModel = function () {
        this.FormData = {
            Comments: [],
            Owners: [new $scope.OwnerModel("Prior Owner Liens"), new $scope.OwnerModel("Current Owner Liens")],
            preclosing: {
                ApprovalData: [{}]
            },
            docs: {}
        };
    };

    $scope.StatusList = [
        {
            num: 0,
            desc: 'Initial Review'
        }, {
            num: 1,
            desc: 'Clearance'
        }
    ];
    $scope.arrayRemove = ptCom.arrayRemove;
    $scope.ptCom = ptCom;
    $scope.ptContactServices = ptContactServices;
    $scope.ensurePush = function (modelName, data) { ptCom.ensurePush($scope, modelName, data); };
    $scope.Form = new $scope.FormModel();
    $scope.ReloadedData = {};

    $scope.Load = function (data) {
        $scope.Form = new $scope.FormModel();
        $scope.ReloadedData = {};
        ptCom.nullToUndefined(data);
        $.extend(true, $scope.Form, data);
        if (!$scope.Form.FormData.Owners[0].shownlist) {
            $scope.Form.FormData.Owners[0].shownlist = [false, false, false, false, false, false, false, false, false, false, false];
            $scope.Form.FormData.Owners[1].shownlist = [false, false, false, false, false, false, false, false, false, false, false];
        }
        $scope.BBLE = data.Tag;
        if ($scope.BBLE) {
            ptLeadsService.getLeadsByBBLE($scope.BBLE, function (res) {
                $scope.LeadsInfo = res;
            });
            ptShortsSaleService.getBuyerTitle($scope.BBLE, function (error, res) {
                if (error) console.log(error);
                if (res) $scope.BuyerTitle = res.data;
            });
            $scope.getStatus($scope.BBLE);
        }
        $scope.$broadcast('ownerliens-reload');
        $scope.$broadcast('clearance-reload');
        $scope.$broadcast('titledoc-reload')

        $scope.checkReadOnly(TitleControlReadOnly);
        $scope.$apply();
    };
    $scope.Get = function (isSave) {
        if (isSave) {
            $scope.updateBuyerTitle();
        }
        return $scope.Form;
    }; 

    $scope.checkReadOnly = function (ro) {

        if (ro) {
            $("#TitleUIContent input").attr("disabled", true);
            if ($("#TitleROBanner").length == 0) {
                $("#title_prioity_content").before("<div class='barner-warning text-center' id='TitleROBanner' >Readonly</div>");
            }

        }
    };
    $scope.completeCase = function () {
        if ($scope.CaseStatus != 1 && $scope.BBLE) {
            ptCom.confirm("You are going to complated the case?", "")
                .then(function (r) {
                    if (r) {
                        $http({
                            method: 'POST',
                            url: '/api/Title/Completed',
                            data: JSON.stringify($scope.BBLE)
                        }).then(function success() {
                            $scope.CaseStatus = 1;
                            $scope.Form.FormData.CompletedDate = new Date();
                            ptCom.alert("The case have moved to Completed");
                        }, function error() { });
                    }
                });
        } else if ($scope.BBLE) {
            ptCom.confirm("You are going to uncomplated the case?", "")
                .then(function (r) {
                    if (r) {
                        $http({
                            method: 'POST',
                            url: '/api/Title/UnCompleted',
                            data: JSON.stringify($scope.BBLE)
                        }).then(function success() {
                            $scope.CaseStatus = -1;
                            ptCom.alert("Uncomplete case successful");
                        }, function error() { });
                    }
                });
        }
    };
    $scope.updateCaseStatus = function () {
        if ($scope.CaseStatus && $scope.BBLE) {
            $scope.ChangeStatusIsOpen = false;
            ptCom.confirm("You are going to change case status?", "")
               .then(function (r) {
                   if (r) {
                       $http({
                           method: 'POST',
                           url: '/api/Title/UpdateStatus?bble=' + $scope.BBLE,
                           data: JSON.stringify($scope.CaseStatus)
                       }).then(function success() {
                           ptCom.alert("The case status has changed!");
                       }, function error() { });
                   }
               });
        }
    };
    $scope.getStatus = function (bble) {
        $http.get('/api/Title/GetCaseStatus?bble=' + bble)
        .then(function succ(res) {
            $scope.CaseStatus = res.data;
        }, function error() {
            $scope.CaseStatus = -1;
            console.log("get status error");
        });
    };
    $scope.generateXML = function () {
        $http({
            url: "/api/Title/GenerateExcel",
            method: "POST",
            data: JSON.stringify($scope.Form)
        }).then(function (res) {
            STDownloadFile("/api/Title/GetGeneratedExcel", "titlereport.xlsx");
        });
    };
    $scope.updateBuyerTitle = function () {
        var updateFlag = false;
        var data = $scope.BuyerTitle;
        var newdata = $scope.Form.FormData.info;
        if (data && newdata) {

            if (newdata.Company != data.CompanyName) {
                data.CompanyName = newdata.Company;
                updateFlag = true;
            }

            if (newdata.Title_Num != data.OrderNumber) {
                data.OrderNumber = newdata.Title_Num;
                updateFlag = true;
            }

            if (ptCom.toUTCLocaleDateString(newdata.Order_Date) != ptCom.toUTCLocaleDateString(data.ReportOrderDate)) {
                updateFlag = true;
            }
            data.ReportOrderDate = newdata.Order_Date;

            if (ptCom.toUTCLocaleDateString(newdata.Confirmation_Date) != ptCom.toUTCLocaleDateString(data.ConfirmationDate)) {
                updateFlag = true;
            }
            data.ConfirmationDate = newdata.Confirmation_Date;

            if (ptCom.toUTCLocaleDateString(newdata.Received_Date) != ptCom.toUTCLocaleDateString(data.ReceivedDate)) {
                updateFlag = true;
            }
            data.ReceivedDate = newdata.Received_Date;

            if (ptCom.toUTCLocaleDateString(newdata.Initial_Reivew_Date) != ptCom.toUTCLocaleDateString(data.ReviewedDate)) {
                updateFlag = true;
            }
            data.ReviewedDate = newdata.Initial_Reivew_Date;

            if (updateFlag) {
                $http({
                    url: "/api/ShortSale/UpdateBuyerTitle",
                    method: 'POST',
                    data: JSON.stringify(data)
                }).then(function succ(res) {
                    if (!res) console.log("fail to update buyertitle");
                }
                , function error() {
                    console.log("fail to update buyertitle");
                });
            }
        }
    };
}])
.controller("TitleCommentCtrl", ['$scope', function ($scope) {
    $scope.showPopover = function (e) {
        aspxConstructionCommentsPopover.ShowAtElement(e.target);
    };
    $scope.addComment = function (comment, user) {
        var newComments = {};
        newComments.comment = comment;
        newComments.caseId = $scope.CaseId;
        newComments.createBy = user;
        newComments.createDate = new Date();
        $scope.Form.FormData.Comments.push(newComments);
    };
    $scope.addCommentFromPopup = function (user) {
        var comment = $scope.addCommentTxt;
        $scope.addComment(comment, user);
        $scope.addCommentTxt = '';
    };
    $scope.$on('titleComment', function (e, args) {
        $scope.addComment(args.message);
    }); 
}])
.controller('TitleLienCtrl', ['$scope', 'ptCom', '$timeout', function ($scope, ptCom, $timeout) {
    $scope.Form = $scope.$parent.Form;
    $scope.OwnerLienPopup = [false, false];

    $scope.reload = function () {
        $scope.Form = $scope.$parent.Form;
        $scope.OwnerLienPopup = [false, false];
    }

    $scope.setPopVisible = function (model, step, index) {
        $scope.OwnerLienPopup[index] = true;
        model.popStep = step ? step : 0;

    }

    $scope.setPopHide = function (model, index) {
        $scope.OwnerLienPopup[index] = false;
        model.popStep = 0;
    }

    $scope.showWatermark = function (model) {
        var result = true;
        _.each(model, function (n) {
            result &= !n;
        });
        return result;
    }

    $scope.showNext = function (model) {
        var step = model.popStep;
        return ptCom.next(model.shownlist, true, step) >= 0;
    }
    $scope.next = function (model, index) {
        var step = model.popStep;
        if ($scope.showNext(model)) {
            model.popStep = ptCom.next(model.shownlist, true, step) + 1;
        } else {
            $scope.setPopHide(model, index);
        }

    }
    $scope.showPrevious = function (model) {
        var step = model.popStep;
        return ptCom.previous(model.shownlist, true, step) >= 0;
    }

    $scope.previous = function (model, index) {
        var step = model.popStep;
        if ($scope.showPrevious(model)) {
            model.popStep = ptCom.previous(model.shownlist, true, step - 1) + 1;
        } else {
            $scope.setPopHide(model, index);
        }
    }

    $scope.$on('ownerliens-reload', function (e, args) {
        $scope.reload();
    });
    $scope.swapOwnerPos = function (index) {
        $timeout(function () {
            var temp1 = $scope.Form.FormData.Owners[index];
            $scope.Form.FormData.Owners[index] = $scope.Form.FormData.Owners[index - 1];
            $scope.Form.FormData.Owners[index - 1] = temp1;
        });
    };
}])
.controller('TitleFeeClearanceCtrl', ['$scope', function ($scope) {
    var FeeClearanceModel = function () {
        this.data = [
            {
                name: 'Purchase Price',
                cost: 0.0,
                lastupdate: null, note: ''

            },
            {
                name: '2nd Lien',
                cost: 0.0,
                lastupdate: null, note: ''
            },
            {
                name: 'Taxes due',
                cost: 0.0,
                lastupdate: null, note: ''
            },
            {
                name: 'Water',
                cost: 0.0,
                lastupdate: null, note: ''
            },
            {
                name: 'Multi Dwelling',
                cost: 0.0,
                lastupdate: null, note: ''

            },
            {
                name: 'PVB',
                cost: 0.0,
                lastupdate: null, note: ''
            },
            {
                name: 'ECBS',
                cost: 0.0,
                lastupdate: null, note: ''
            },
            {
                name: 'Judgments',
                cost: 0.0,
                lastupdate: null, note: ''
            },
            {
                name: 'Taxes on HUD',
                cost: 0.0,
                lastupdate: null, note: ''
            },
            {
                name: 'Water on HUD',
                cost: 0.0,
                lastupdate: null, note: ''
            },
            {
                name: 'HPD on HUD',
                cost: 0.0,
                lastupdate: null, note: ''
            },
            {
                name: 'ECB on hud',
                cost: 0.0,
                lastupdate: null, note: ''
            }],
        this.total = 0.0

    }

    $scope.FormData = $scope.$parent.Form.FormData;
    $scope.FormData.FeeClearance = new FeeClearanceModel();
    $scope.reload = function () {
        $scope.FormData = $scope.$parent.Form.FormData;
        if ($scope.$parent.Form.FormData.FeeClearance) {
            $scope.FormData.FeeClearance = $scope.FormData.FeeClearance;
        } else {
            $scope.FormData.FeeClearance = new FeeClearanceModel();
        }
    }
    $scope.updateTotal = function (d) {
        d.lastupdate = new Date();
        var total = 0.0;
        _.each($scope.FormData.FeeClearance.data, function (el, idx) {

            if (typeof el.cost == 'string' && el.cost.length > 0) {
                el.cost = el.cost.replace(/[^0-9|\.|\-|\+]/, '');
            }

            total += parseFloat(el.cost) ? parseFloat(el.cost) : 0.0;
        })
        $scope.FormData.FeeClearance.total = total;
    }
    $scope.$on('clearance-reload', function (e, args) {
        $scope.reload();
    })
}])
.controller("TitleDocCtrl", ['$scope', '$http', 'ptCom', function ($scope, $http, ptCom) {
    $scope.transferors = [
        {
            name: 'Ron Borovinsky'
        }, {
            name: 'Jay Gottlieb'
        }, {
            name: 'Princess Simeon'
        }, {
            name: 'Eliezer Herts'
        }, {
            name: 'Magda Brillite'
        }, {
            name: 'Tomer Aronov'
        }, {
            name: 'Moisey Iskhakov'
        }, {
            name: 'Albert Gavriyelov'
        }, {
            name: 'Yvette Guizie'
        }, {
            name: 'Michael Gendin'
        }, {
            name: 'Rosanne Alicea'
        }
    ]
    $scope.data = $scope.$parent.Form.FormData.docs;
    $scope.ReloadedData = {};

    $scope.generatePackage = function () {
        $("#generatedDocsLink").hide()
        $("#generatedDocWarning").hide()
        ptCom.startLoading();

        $http({
            url: "/api/Title/GeneratePakcage",
            method: "POST",
            data: $scope.data
        }).then(function (res) {
            ptCom.stopLoading();
            if (res.data.length > 0) {
                $("#generatedDocsLink").show();
            } else {
                $("#generatedDocWarning").show();
            }
        }, function () {
            ptCom.stopLoading();
            $("#generatedDocWarning").show();
        })
    }

    $scope.$on('titledoc-reload', function (e, args) {
        $scope.data = $scope.$parent.Form.FormData.docs;
        $scope.ReloadedData = {};
    })


}])
angular.module("PortalApp").controller("UnderwriterController",
                ['$scope', 'ptCom', 'ptUnderwriter', '$location', 'DocSearch', '$state', function ($scope, ptCom, ptUnderwriter, $location, DocSearch, $state) {


                    $scope.data = {};
                    $scope.archive = {};
                    $scope.currentDataCopy = {};
                    $scope.isProtectedView = true;

                    $scope.init = function (bble) {
                        $scope.load(bble);
                        $scope.loadArchivedList(bble);
                    }

                    $scope.load = function (bble) {
                        $scope.data = ptUnderwriter.load(bble);
                        if ($scope.data.$promise) {
                            $scope.data.$promise.then(function () {
                                $scope.calculate();
                            })
                        }
                    }

                    $scope.save = function () {
                        ptCom.confirm("Are you going to Save?", function (response) {
                            if (response) {
                                ptUnderwriter.save($scope.data).then(function (d) {
                                    if (d.data) {
                                        $scope.data = d.data;
                                    }
                                    ptCom.alert("Save Successful");
                                }, function () {
                                    ptCom.alert("fail to save");
                                })
                            }
                        })


                    }

                    $scope.archiveFunc = function () {
                        ptCom.prompt('Please give a name to this archive.', function (msg) {
                            if (msg != null) {
                                ptUnderwriter.archive($scope.data, msg).then(function (d) {
                                    alert("Archive succesful.")
                                }, function () {
                                    alert("Sorry...Some error...")
                                })
                            }

                        })


                    }

                    $scope.loadArchivedList = function (bble) {
                        if (bble) {
                            ptUnderwriter.loadArchivedList(bble).then(function (d) {
                                $scope.archivedList = d.data;
                            })
                        }
                    }

                    $scope.loadArchived = function (archive) {
                        if (archive.Id) {
                            ptUnderwriter.loadArchived(archive.Id).then(function (d) {
                                if (d.data) {
                                    angular.copy($scope.data, $scope.currentDataCopy);
                                    ptCom.assignReference($scope.data, d.data, [], ['Id']);
                                    $scope.archive = archive;
                                    $scope.archive.isLoaded = true;
                                    ptCom.alert("Load successful");

                                }
                            }, function (d) {
                                ptCom.alert("Fail to load.");
                            })
                        }

                    }

                    $scope.restoreCurrent = function () {
                        if ($scope.currentDataCopy) {
                            ptCom.assignReference($scope.data, $scope.currentDataCopy);
                            $scope.archive.isLoaded = false;
                            ptCom.alert("Restore to current version.")
                        }
                    }

                    $scope.calculate = function () {
                        $scope.$applyAsync(function () {
                            ptUnderwriter.calculator.calculate($scope.data);
                        });
                    }

                    $scope.feedData = function () {
                        $scope.data.PropertyInfo.TaxClass = 'A0',
                        $scope.data.PropertyInfo.ActualNumOfUnits = 1
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
                    }

                    $scope.enableEditing = function () {
                        $scope.$broadcast('pt-editable-div-unlock');
                        $scope.isProtectedView = false;
                    }

                    $scope.$watch(function () {
                        return $state.$current.name
                    }, function (newVal, oldVal) {
                        if (newVal == 'underwriter.datainput') {
                            if ($scope.isProtectedView == false) {
                                $scope.enableEditing();
                            }
                        }
                    })




                    $scope.BBLE = ptCom.getGlobal("BBLE") || "";
                    $scope.viewmode = ptCom.getGlobal("viewmode") || 0;
                    $scope.init($scope.BBLE);

                }]);
angular.module("PortalApp")
.controller('UnderwritingRequestController', ['$scope', '$http', '$location', '$state', 'UnderwritingRequest', 'ptCom', 'DocSearch', function ($scope, $http, $location, $state, UnderwritingRequest, ptCom, DocSearch) {
    $scope.init = function (bble) {
        $scope.data = {};
        if ($scope.BBLE) {
            $scope.data = UnderwritingRequest.get({ BBLE: $scope.BBLE.trim() }, function () {
                $scope.search = DocSearch.get({ BBLE: bble.trim() });
            })
        }
    }


    $scope.cleanForm = function () {
        var oldId = $scope.data.Id;
        $scope.data = {};
        $scope.data.Id = oldId;
        $scope.formCleaned = true;

    }

    $scope.checkValidate = function (async) {
        if (!async) {
            return _.some($('input, textarea, select'), function (v) {
                return $(v).attr('error') == 'true';
            })
        } else {
            var dfd = $.Deferred();

            var err = _.some($('input, textarea, select'), function (v) {
                return $(v).attr('error') == 'true';
            });

            if (err) {
                dfd.resolve();
            } else {
                dfd.reject();
            }

            return dfd;
        }

    }

    $scope.selfCheck = function () {
        $scope.$broadcast('ptSelfCheck');
        var startFlag = false
        var checkingcounter = 0;
        $scope.$on('ptSelfCheckStart', function () {
            startFlag = true;
            checkingcounter++;
        });
    }

    $scope.save = function (isSlient) {
        $scope.$broadcast('ptSelfCheck');
        if ($scope.checkValidate()) {
            ptCom.alert('Please correct Highlight Field first.');
            return;
        }
        UnderwritingRequest.saveByBBLE($scope.data, $scope.BBLE).then(function () {
            if (!isSlient) {
                ptCom.alert('Save Successful!')
            }
        }, function () {
            if (!isSlient) {
                ptCom.alert('Fail to Save!')
            }
        });

    }

    $scope.requestDocSearch = function (isResubmit) {
        $scope.$broadcast('ptSelfCheck');
        if ($scope.checkValidate()) {
            ptCom.alert('Please correct Highlight Field first.');
            return;
        }
        UnderwritingRequest.createSearch($scope.BBLE).then(function (r) {
            $scope.search.CreateDate = new Date().toISOString();
            ptCom.alert('Property Search Submitted to Underwriting. Thank you!');
            $scope.data.Status = 1;
            if (isResubmit) {
                debugger;
                $scope.search.CompletedOn = undefined;
                $scope.search.Expired = false;
                $scope.formCleaned = false;
            }
            $scope.save(true);
        }, function () {
            ptCom.alert('Fail to create search')
        })
    }


    $scope.remainDays = function () {
        if (!$scope.search || !$scope.search.CompletedOn) {
            return "more than 60";
        } else {
            var timenow = new Date().getTime();
            var timeCompleted = new Date($scope.search.CompletedOn);
            var diff = timenow - timeCompleted;
            var dayinmsec = 1000 * 60 * 60 * 24;
            return 60 - Math.ceil(diff / dayinmsec);
        }

    }

    $scope.completedOver60days = function () {
        if (!$scope.search || $scope.search.CompletedOn == undefined) {
            return false;
        }
        else {
            return $scope.remainDays() < 0 ? true : false;
        }

    }


    $scope.viewmode = ptCom.getGlobal("viewmode") || ptCom.parseSearch(location.search).mode || 0;
    $scope.BBLE = ptCom.getGlobal("BBLE") || ptCom.parseSearch(location.search).BBLE || "";
    $scope.init($scope.BBLE);
}]);
angular.module("PortalApp").controller("UnderwritingSummaryController", ['$scope', 'ptCom', 'ptUnderwriter', 'DocSearch', function ($scope, ptCom, ptUnderwriter, DocSearch) {

    $scope.showStoryHistory = function () {
        var scope = angular.element('#uwrview').scope();
        if (scope.data && scope.data.Id) {
            auditLog.toggle('UnderwritingRequest', scope.data.Id);
        }
    }

    $scope.markCompleted = function(status, msg) {
        msg = 'Please provide Note or press no to cancel';
        ptCom.prompt(msg, function (result) {
            if (result != null && $scope.search) {
                debugger;
                DocSearch.markCompleted($scope.search.BBLE, status, result).then(function succ(d) {
                    $scope.search.UnderwriteStatus = d.data.UnderwriteStatus;
                    $scope.search.UnderwriteCompletedBy = d.data.UnderwriteCompletedBy;
                    $scope.search.UnderwriteCompletedOn = d.data.UnderwriteCompletedOn;
                    $scope.search.UnderwriteCompletedNotes = d.data.UnderwriteCompletedNotes;
                }, function err() {
                    console.log("fail to update docsearch");
                });
            }

        }, true);

    }
    $scope.loadAdditionalInfo = function (bble) {
        $scope.search = DocSearch.get({ BBLE: bble.trim()})
    }
    try {
        var searchs = ptCom.parseSearch(location.search);
        if (searchs) {
            $scope.BBLE = searchs.BBLE || "";
            $scope.viewmode = searchs.mode || 0;
        }
        ptCom.setGlobal("BBLE", $scope.BBLE);
        ptCom.setGlobal("viewmode", $scope.viewmode);
        $scope.loadAdditionalInfo($scope.BBLE);
    } catch (ex) {
        ptCom.setGlobal("BBLE", "");
        ptCom.setGlobal("viewmode", 0);
    }
}]);
angular.module("PortalApp")
    .controller('VendorCtrl', ["$scope", "$http" ,"$element", function ($scope, $http, $element) {

    $($('[title]')).tooltip({
        placement: 'bottom'
    });
    $scope.popAddgroup = function (Id) {
        $scope.AddGroupId = Id;
        $('#AddGroupPopup').modal();
    }
    $scope.AddGroups = function () {
        $http.post('/CallBackServices.asmx/AddGroups', { gid: $scope.AddGroupId, groupName: $scope.addGroupName, addUser: $('#CurrentUser').val() }).
           success(function (data) {
               $scope.initGroups();
           });
    }
    $scope.ChangeGroups = function (g) {

        $scope.selectType = g.Id == null ? "All Vendors" : g.GroupName;
        if (g.Id == null) {
            g.Id = 0;
        }
        $http.post('/CallBackServices.asmx/GetContactByGroup', { gId: g.Id }).
            success(function (data) {
                $scope.InitDataFunc(data);
            });
    };
    $scope.InitData = function (data) {
        $scope.allContacts = data.slice();
        var gropData = data;
        $scope.showingContacts = gropData;

        return gropData;
    }
    $scope.initGroups = function () {
        $http.post('/CallBackServices.asmx/GetAllGroups', {}).
         success(function (data, status, headers, config) {
             $scope.Groups = data.d;

                      }).error(function (data, status, headers, config) {


             alert("error get GetAllGroups: " + status + " error :" + data.d);
         });
    }

    $scope.initGroups();
    $scope.InitDataFunc = function (data) {
        var gropData = $scope.InitData(data.d);
        var allContacts = gropData;
        if (allContacts.length > 0) {
            $scope.currentContact = gropData[0];
            m_current_contact = $scope.currentContact;

        }
    }
    $http.post('/CallBackServices.asmx/GetContact', { p: '1' }).
        success(function (data, status, headers, config) {
            $scope.InitDataFunc(data);
            $scope.AllTest = data.d;

        }).error(function (data, status, headers, config) {
            $scope.LogError = data
            alert("error get contacts: " + status + " error :" + data.d);
        });

    $scope.initLenderList = function () {
        $http.post('/CallBackServices.asmx/GetLenderList', {}).success(function (data, status) {
            $scope.lenderList = _.uniq(data.d);
        });
    }

    $scope.initLenderList();

    $scope.predicate = "Name";
    $scope.group_text_order = "group_text";
    $scope.addContact = {};
    $scope.selectType = "All Vendors";
    $scope.query = {};
    $scope.addContactFunc = function () {
        var addType = $scope.query.Type;
        if (!$scope.addContact || !$scope.addContact.Name) {
            alert("Please fill vender Name !");
            return;
        }
        if (addType != null) {
            $scope.addContact.Type = addType;


        }
        var addC = $scope.addContact;

        debugger;
        $http.post("/CallBackServices.asmx/AddContact", { contact: $scope.addContact }).
        success(function (data, status, headers, config) {
            if (data.d.Name == 'Same')
            {
                alert("Already have " + $scope.addContact.Name + " in system please change name to identify !")
                return;
            }
            $scope.allContacts.push(data.d);
            $scope.InitData($scope.allContacts);
            var addContact = data.d;

            $scope.currentContact = addContact;
            m_current_contact = $scope.currentContact;
            $scope.initLenderList();
            var stop = $(".popup_employee_list_item_active:first").position().top;
            $('#employee_list').scrollTop(stop);
            alert("Add" + $scope.currentContact.Name + " succeed !");
        }).
        error(function (data, status, headers, config) {
            var message = data&& data.Message ?data.Message :JSON.stringify(data)

            alert("Add contact error: " + message);
        });
    }

        $scope.filterContactFunc = function (e, type) {

        var text = angular.element(e.currentTarget).html();
        if (typeof (type) == 'string') {
            $scope.query = {};
            $scope.selectType = text;
            return true;
        } else {
            $scope.query.Type = type;
        }

        var corpName = type == 4 && text != 'Lenders' ? text : '';
        $scope.query.CorpName = corpName;


        $scope.addContact.CorpName = corpName;

        $scope.selectType = text;
        return true;
    }

    $scope.SaveCurrent = function () {

                $http.post("/CallBackServices.asmx/SaveContact", { json: $scope.currentContact }).
        success(function (data, status, headers, config) {
            alert("Save succeed!");
            $scope.initLenderList();
        }).
        error(function (data, status, headers, config) {
            alert("geting SaveCurrent error" + status + "error:" + JSON.stringify(data.d));
        });
    }

    $scope.FilterContact = function (type) {
        $scope.showingContacts = $scope.allContacts;
        if (type < 0) {
            return;
        }
        var contacts = $scope.allContacts;

        for (var i = 0; i < contacts.length; i++) {
            if (contacts.Type != type) {
                $scope.showingContacts.splice(i, 1);
            }

        }

    }
    $scope.selectCurrent = function (selectContact) {
        $scope.currentContact = selectContact;
        m_current_contact = selectContact;
    }

}]);
angular.module('PortalApp').component('ptAudit', {

    templateUrl: '/js/components/ptAudit.tpl.html',
    bindings: {
        label: '@',
        objName: '@',
        recordId: '<',
    },
    controller: function ($scope, $element, $attrs, $http) {
        var ctrl = this;
        ctrl.init = function () {
            if (ctrl.objName != null && ctrl.recordId != null) {
                ctrl.updateData();
            }
        }
        ctrl.show = function (objName,  recordId) {
            if (objName != null || recordId != null) {
                ctrl.objectName = objName || ctrl.objectName;
                ctrl.recordId = recordId || ctrl.recordId;
                ctrl.updateData();
            }
            ctrl.showDetail = true;
            return;
        }
        ctrl.hide = function () {
            ctrl.showDetail = false;
        }
        ctrl.toggle = function (objName, recordId) {
            if (ctrl.showDetail) {
                ctrl.hide();
            } else {
                ctrl.show(objName, recordId)
            }
        }
        ctrl.updateData = function () {
            $http({
                method: 'GET',
                url: '/api/auditlog/' + ctrl.objName + "/" + ctrl.recordId
            }).then(function (d) {
                var result = _.groupBy(d.data, function (item) {
                    return item.EventDate;
                });
                ctrl.AuditLogs = result;
                if (result && Object.keys(result).length > 0) {
                    ctrl.logsize = Object.keys(result).length;
                } else {
                    ctrl.logsize = 0;
                }
            })

        }
        ctrl.init();
    }
})
angular.module('PortalApp').component('ptHomeowner', {

    templateUrl: '/js/Views/LeadDocSearch/searchOwner.tpl.html',
    controller: function ($http, ptCom) {
        this.init = function (bble) {
            var that = this;
            if (bble) {
                $http({
                    method: 'GET',
                    url: '/api/homeowner/' + bble,
                }).then(function (r) {
                    that.rawdata = r.data;
                })
            }
        }

        this.parseDate = function (dateField) {
            if (dateField) {
                return (dateField.yearField ? dateField.yearField : 'xxxx') +
                       "/" + (dateField.monthField ? dateField.monthField : 'xx') +
                       "/" + (dateField.dayField ? dateField.dayField : 'xx');
            }

        } 

        this.parseAddress = function (addressField) {
            if (addressField) {
                return (addressField.line1Field ? addressField.line1Field + ' ' : '') +
                       (addressField.line2Field ? addressField.line2Field + ' ' : '') +
                       (addressField.line3Field ? addressField.line3Field + ' ' : '') +
                       ', ' +
                       (addressField.cityField ? addressField.cityField + ', ' : 'Unknown City,') +
                       (addressField.stateField ? addressField.stateField + ', ' : 'Unknown State,') +
                       (addressField.zipField ? addressField.zipField : '')
            }
        }

        this.BBLE = ptCom.getGlobal("BBLE") || ptCom.parseSearch(location.search).BBLE || "";
        this.init(this.BBLE);
    }
});
angular.module('PortalApp').component('ptItemList', {

    templateUrl: '/js/components/ptItemList.html',
    bindings: {
        itemName: '@',
        itemUrl: '@',
        itemField: '@',
        onSelectionChanged: '&'
    },
    controller: function ($scope, $element, $attrs, $http) {
        $scope.list = {}
        $scope.searchOptions = {
            valueChangeEvent: "keyup",
            placeholder: "Search",
            mode: "search",
            onValueChanged: function (args) {
                $scope.list.searchValue(args.value);
                $scope.list.load();
            }
        };
        $scope.listOptions = {
            selectionMode: "single",
            onSelectionChanged: function (data) {
                $scope.$ctrl.onSelectionChanged(data);
            },
            columns: [
                {
                    dataField: $scope.$ctrl.itemField,
                    itemTemplate: function (data, index) {
                        var result = $("<div>").addClass("list-item");
                        $("<div>").text(data[$scope.$ctrl.itemField]).css("padding-left", "10px").appendTo(result);
                        return result;
                    }
                }
            ],

            bindingOptions: {
                dataSource: 'list',
            },

        }
        $scope.bindList = function () {
            $http({
                method: 'GET',
                url: $scope.$ctrl.itemUrl
            }).then(function (d) {
                $scope.list = new DevExpress.data.DataSource({
                    searchOperation: "contains",
                    searchExpr: $scope.$ctrl.itemField,
                    store: d.data
                });
            })
        }

        $scope.bindList();


    }

})
angular.module('PortalApp').component('ptSelectableInput', {

    template: '<div>' +
              '<select ng-model="$ctrl.selected">' +
              '<option ng-repeat="p in $ctrl.options">{{p}}</option>' +
              '</select>&nbsp;' +
              '<input type="text" ng-model="$ctrl.ngModel" ng-show="$ctrl.isOtherSelected" placeholder="other">' +
              '</input>' +
              '</div>',
    bindings: {
        optionss: '@',
        disableOptions: '=',
        ngModel: '='
    },
    controller: function ($scope, $element, $attrs, $http) {
        var ctrl = this;
        ctrl.options = ctrl.optionss.split("|");
        if (!ctrl.disableOptions || !ctrl.options) {
            ctrl.isOtherSelected = true;
        }
        $scope.$watch('$ctrl.selected', function (newValue, oldValue) {
            if (!newValue) {
                ctrl.ngModel = "";
                return;
            }
            if (newValue == 'other') {
                ctrl.isOtherSelected = true;
                ctrl.ngModel = "";
                return;
            }
            if (ctrl.options.indexOf(newValue) >= 0) {
                ctrl.isOtherSelected = false;
                ctrl.ngModel = newValue;
                return;
            }

            ctrl.ngModel = "";

        })
        $scope.$watch('$ctrl.ngModel', function (newValue, oldValue) {
            if (newValue != "other" && ctrl.options.indexOf(newValue) >= 0) {
                ctrl.selected = newValue;
            }
        })
    }

})
