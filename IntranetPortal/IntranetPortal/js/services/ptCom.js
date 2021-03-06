/**
 * Author: Shaopeng Zhang
 * Date: ???
 * Description: An utility library provide common function in angular
 * Update: 
 *          2016/11/02:
 *              1. add function parseSearch to get paires from Location.Search
 *          
 **/

angular.module("PortalApp").service("ptCom", ["$rootScope", function ($rootScope) {
    var that = this;

    this.arrayAdd = function (model, data) {
        if (model) {
            data = data ? data : {};
            model.push(data);
        }
    };
    // delete a element from a array with promte
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
    this.capitalizeFirstLetter = function (string) {
        return string.charAt(0).toUpperCase() + string.slice(1);
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
    this.formatName = function (firstName, middleName, lastName) {
        var result = '';
        if (firstName) result += that.capitalizeFirstLetter(firstName) + ' ';
        if (middleName) result += that.capitalizeFirstLetter(middleName) + ' ';
        if (lastName) result += that.capitalizeFirstLetter(lastName);
        return result;
    };
    this.ensureArray = function (scope, modelName) {
        /* caution: due to the ".", don't eval to create an array more than one level*/
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
    // when use jquery.extend, jquery will override the dst even src is null,
    // this function convert null recursively to make the extend works as expected 
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
    this.removeOverlay = function () {
        $rootScope.removeOverlay();
    }; // get next index of value in the array, 
    this.stopLoading = function () {
        $rootScope.stopLoading();
    }
    this.startLoading = function () {
        $rootScope.startLoading();
    }
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
    /**
     * assign all reference property from source to target
     * @param: target
     * @param: source
     * @param: skipped //reference that will not be replaced by source
     * @param: keeped // level two 
     */
    // Method to copy value to object's reference property.
    // Caution: didn't use recursive, so only go one level deep.
    this.assignReference = function (target, source,
                      /* optional */ skipped,
                      /* optional */ keeped) {

        var temp = {}; // object for backuping keeped values
        var props = Object.keys(source);
        for (i = 0; i < props.length ; i++) {
            // init target's property to prevent null point exception.
            if (target[props[i]] == null) target[props[i]] = {};
            // skip some reference
            if (skipped && skipped.indexOf(props[i]) >= 0) {
                continue;
            }
            if (typeof source[props[i]] == 'object') {
                // keep some value inside reference, usually id or something ;)
                if (keeped && keeped.length) {
                    temp[props[i]] = {};
                    for (j = 0; j < keeped.length; j++) {
                        if (target[props[i]] && target[props[i]][keeped[j]]) {
                            temp[props[i]][keeped[j]] = target[props[i]][keeped[j]];
                        }
                    }
                }
                if (source[props[i]] != null) {
                    target[props[i]] = source[props[i]];
                } else {
                    target[props[i]] = {};
                }
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

    this.parseSearch = function (/*string*/ searchString) {
        var result = {};
        if (!searchString || typeof searchString != 'string')   //not a string
            return result;
        if (searchString.slice(0, 1) != '?')    //not a search string
            return result;
        var entriesString = searchString.slice(1).replace(/%20/g, '');  //remove leading ?
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
        $rootScope.globaldata[key] = value;
    }
    this.getGlobal = function (key) {
        if ($rootScope.globaldata[key] != null) {
            return $rootScope.globaldata[key];
        } else {
            return undefined;
        }
    }
    this.getCurrentUser = function () {
        var element = $("#CurrentUser");
        return element[0].value;
    }
}])