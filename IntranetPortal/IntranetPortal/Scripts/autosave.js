﻿/* code area steven*/
$.wait = function (ms) {
    var defer = $.Deferred();
    setTimeout(function () { defer.resolve(); }, ms);
    return defer;
};
function NGAddArrayitemScope(scopeId, model) {
    var $scope = angular.element(document.getElementById(scopeId)).scope();
    if (model) {
        var array = $scope.$eval(model);
        if (!array) {
            $scope.$eval(model + '=[{}]');
        } else {

            $scope.$eval(model + '.push({})');

        }
        $scope.$apply();
    }
}

function ScopeCaseDataChanged(getDataFunc) {
    if ($('#CaseData').length === 0) {
        alert("can not find input case data elment");
        $('<input type="hidden" id="CaseData" />').appendTo(document.body);
        return false;
    }

    return $('#CaseData').val() != "" && $('#CaseData').val() != JSON.stringify(getDataFunc());
}

function ScopeResetCaseDataChange(getDataFunc) {
    var caseData = getDataFunc();
    if ($('#CaseData').length == 0) {

        $('<input type="hidden" id="CaseData" />').appendTo(document.body);
    }
    $('#CaseData').val(JSON.stringify(getDataFunc()));
}

function ScopeAutoSave(getDataFunc, SaveFunc, headEelem, makeSrueRefersh) {
    if ($(headEelem).length <= 0) {
        return;
    }
    if (typeof GetDataReadOnly !== 'undefined' && !GetDataReadOnly()) {
        return;
    }
    // delay the first run after 30 second!
    $.wait(30000).then(function () {
        window.setInterval(function () {
            //if (makeSrueRefersh)
            //{
            //    var m = makeSrueRefersh;
            //    CheckLastUpdateChangedByOther(m.urlFunc, m.reLoadUIfunc, m.loadUIIdFunc, m.urlModfiyUserFunc)
            //}
            if (ScopeCaseDataChanged(getDataFunc)) {
                var sucessFunc = function () {
                };
                SaveFunc(sucessFunc);
                //ScopeResetCaseDataChange(getDataFunc)
            }
        }, 30000);
    });
}


function ScopeSetLastUpdateTime(url) {
    $.getJSON(url, function (data) {
        $('#LastUpdateTime').val(JSON.stringify(data));
    });
}

function CheckLastUpdateChangedByOther(urlFunc, reLoadUIfunc, loadUIIdFunc, urlModfiyUserFunc) {
    var url = urlFunc();
    $.getJSON(url, function (data) {
        var lastUpdateTime = JSON.stringify(data);
        var localUpdateTime = $('#LastUpdateTime').val();
        if (localUpdateTime && localUpdateTime != lastUpdateTime) {
            if (urlModfiyUserFunc) {
                $.getJSON(urlModfiyUserFunc(), function (mUser) {
                    if (mUser) {
                        if (mUser != "sameuser") {
                            alert(mUser + " change your file at " + lastUpdateTime + ", system will load the refreshest data ! Will missing some data which you inputed.");
                        }
                        reLoadUIfunc(loadUIIdFunc());
                    }

                });
            } else {
                alert("Someone change your file at " + lastUpdateTime + ", system will load the refreshest data ! Will missing some data which you inputed.");
                reLoadUIfunc(loadUIIdFunc());
            }

        }
    });
}

function ScopeDateChangedByOther(urlFunc, reLoadUIfunc, loadUIIdFunc, urlModfiyUserFunc) {

    window.setInterval(function () {
        CheckLastUpdateChangedByOther(urlFunc, reLoadUIfunc, loadUIIdFunc, urlModfiyUserFunc);
    }, 10000);

}

Array.prototype.getUnique = function () {
    var u = {}, a = [];
    for (var i = 0, l = this.length; i < l; ++i) {
        if (u.hasOwnProperty(this[i])) {
            continue;
        }
        a.push(this[i]);
        u[this[i]] = 1;
    }
    return a;
}