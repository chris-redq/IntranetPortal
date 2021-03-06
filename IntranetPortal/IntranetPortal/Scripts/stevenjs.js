﻿wx_deubg = true;
wx_show_bug = false;
PortalUtility = {
    customizeDateText: function (cellInfo) {
        if(cellInfo.target == "headerFilter")
            return cellInfo.valueText;

        if (!cellInfo.value)
            return ""                                
        var dt = PortalUtility.FormatLocalDateTime(cellInfo.value);
        if (dt)
            return moment(dt).format('MM/DD/YYYY hh:mm a');

        return ""
    },
    customizeDateText2: function (cellInfo) {
        if (cellInfo.target == "headerFilter")
            return cellInfo.valueText;

        if (!cellInfo.value)
            return ""
        var dt = PortalUtility.FormatLocalDateTime(cellInfo.value);
        if (dt)
            return moment(dt).format('MM/DD/YYYY');

        return ""
    },
    FormatLocalDateTime: function (utcDate) {
        if (!utcDate)
            return

        // create Date object for current location
        var d = new Date();

        // convert to msec
        // add local time zone offset
        // get UTC time in msec
        var utc = new Date(utcDate.getTime() + d.getTimezoneOffset() * 60000);

        // return time as a string
        return utc
    },
    FormatISODate: function (utcDate) {
        return moment(PortalUtility.FormatLocalDateTime(utcDate)).format('MM/DD/YYYY');
    },
    GoToCase: function (url) {
        window.location.href = url;
    },
    fileWindows: {},
    ShowPopWindow: function (windowId, url, width, height) {
        this.OpenWindow(url, windowId, width, height);
    },
    OpenWindow: function (url, title, width, height) {
        for (var win in this.fileWindows) {
            if (this.fileWindows.hasOwnProperty(win) && win == title) {
                var caseWin = this.fileWindows[win];
                if (!caseWin.closed) {
                    caseWin.focus();
                    return;
                }
            }
        }

        var vwidth = width ? width : 1350;
        var vheight = height ? height : 930;
        var left = (screen.width / 2) - (vwidth / 2);
        var top = (screen.height / 2) - (vheight / 2);
        this.fileWindows[title] = window.open(url, title, 'Width=' + vwidth + 'px,Height=' + vheight + 'px, top=' + top + ', left=' + left);
        return win;
    },
    QueryUrl: function () {
        // This function is anonymous, is executed immediately and 
        // the return value is assigned to QueryString!
        var query_string = {};
        var query = window.location.search.substring(1);
        var vars = query.split("&");
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            // If first entry with this name
            if (typeof query_string[pair[0]] === "undefined") {
                query_string[pair[0]] = decodeURIComponent(pair[1]);
                // If second entry with this name
            } else if (typeof query_string[pair[0]] === "string") {
                var arr = [query_string[pair[0]], decodeURIComponent(pair[1])];
                query_string[pair[0]] = arr;
                // If third or later entry with this name
            } else {
                query_string[pair[0]].push(decodeURIComponent(pair[1]));
            }
        }
        return query_string;
    },
    ShowConfirm: function () {

    }
}

if (typeof $.formatCurrency != 'function') {
    $.getScript("https://bowercdn.net/c/jquery-formatcurrency-1.4.0/jquery.formatCurrency-1.4.0.min.js");
}
function clickCollapse(e) {

    var buttonClassName = e.className;
    var openClass = "-minus";
    var isOpen = buttonClassName.indexOf(openClass) > 0;

    collapse_doc_list($(e).parents(".doc_list_section:first"), isOpen);
}
function collapse_doc_list(div, isOpen) {
    var e = div.find(".collapse_btn_e");
    if (e.length == 0) {
        return;
    }
    e = e[0];
    var buttonClassName = e.className;
    var openClass = "-minus";
    var clossClass = "-plus";


    var toSwich = !isOpen;
    var changeToClass = isOpen ? buttonClassName.replace(openClass, clossClass) : buttonClassName.replace(clossClass, openClass);
    e.className = changeToClass;
    debugger;
    div.find(".doc_collapse_div:first").css("display", toSwich ? "block" : "none");
}
function collapse_all(collapse_all) {
    $(".doc_list_section").each(function (ind) {
        collapse_doc_list($(this), collapse_all);
    })

}


function d_alert(s) {
    if (wx_deubg) {
        alert(s);
    }
}
function d_assert(cond, s) {
    if (cond) {
        d_alert(s);
    }
}
function d_log(s) {
    if (wx_deubg) {
        console.log(s);
    }
}
function d_log_assert(cond, s) {
    if (cond) {
        d_log(s)
    }
}

/*when has value then is send object by value */
function get_sub_property(obj, id_str, value) {

    if (value != null) {
        value = currency2Number(value)
    }
    if (id_str == null) {
        d_alert("find id_str is null " + id_str);
        return;
    }
    var props = id_str.split(".");

    if (props.length < 1) {
        if (value != null) {
            obj[id_str] = value
        }
        else {
            return obj[id_str];
        }

    }

    var t_obj = obj;
    for (var i = 0; i < props.length; i++) {
        var prop = props[i];
        if (prop.indexOf("[") > 0) {
            var arr = prop.split("[");
            prop = parseInt(arr[1].replace("]", ""));

            //d_alert("get prop with " + arr[0] + "index  =" + prop);
            t_obj = t_obj[arr[0]];
        }
        if (t_obj[prop] == null) {

            if (value != null) {
                //d_alert("create new property" + prop);
                t_obj[prop] = new Object();
            } else {
                //wx_deubg ? "null" :
                return "";
            }

        }
        /* give a property a value */

        if (i == props.length - 1) {
            if (value != null) {
                // d_alert("value a new value to " + id_str + " = " + value);
                if (value instanceof Date) {
                    d_alert("the date is data" + value);
                }

                t_obj[prop] = value;
            }
        }

        t_obj = t_obj[prop];
    }

    return t_obj;
}
function expand_array_item(e) {

    var div = $(e).parents(".ss_array");
    var current_div = div.find(".collapse_div")
    var isopen = current_div.css("display") != "none";
    var field = div.attr("data-field");

    $(".ss_array[data-field='" + field + "']").each(
        function (ind) {
            control_array_div($(this), false);
        }
        );

    control_array_div(div, !isopen)


}
function control_array_btn(div, current_div) {
    var is_open = current_div.css("display") != "none";
    var btn = div.find(".expand_btn");
    if (btn.length == 0) {
        return;
    }
    var e_class = btn.attr("class");

    if (is_open) {
        btn.attr("class", e_class.replace("-expand", "-compress"));
    } else {
        btn.attr("class", e_class.replace("-compress", "-expand"));

    }
}
function control_array_div(div, is_open) {
    var current_div = div.find(".collapse_div");
    if (current_div.length == 0) {
        return;
    }
    var btn_func = function () {
        control_array_btn(div, current_div)
    }
    if (is_open) {
        current_div.slideDown(btn_func);
    } else {
        current_div.slideUp(btn_func);
        //current_div.hide();
    }

    //control_array_btn(div, current_div);
    // current_div.css("display", is_open ? "inline" : "none");


}
function ShortSaleDataBand(data_stauts) {


    var is_save = data_stauts == 1;
    if (ShortSaleCaseData == null) {
        d_alert("ShortSaleCaseData is null");
        return;
    }

    if (ShortSaleCaseData.PropertyInfo == null) {
        d_alert("ShortSaleCaseData.PropertyInfo is null");
        return;
    }
    var ss_data = ShortSaleCaseData;
    //var field_name = "PropertyInfo.Number";
    ShorSaleArrayDataBand(data_stauts);
    var inputs = $(".ss_form_input");
    inputs.each(function (index) {

        var field = ($(this).attr("data-field"));
        if (!field) {
            return;
        }
        var elem = $(this);
        var data_value = null;
        if (is_save) {
            /* if radio box not check then*/
            //if (!fieldNotChange(ShortSaleCaseData, field) && elem.attr("type")=="radio") {
            data_value = get_sub_property(ShortSaleCaseData, field, ss_field_data(elem, null));

            // }

        }
        data_value = get_sub_property(ss_data, field, null);
        ss_field_data(elem, data_value);
        //d_assert(field == "Evivtion", "get evition " + data_value);
    });

    /*band short sale arrary item*/


    //d_log_assert(is_save, "the short sale after save " + JSON.stringify(ShortSaleCaseData));
    /*use for ui*/
    onRefreashDone();
    /**/
    LoadOccupantNotes();
    init_visiable();
}
function format_input() {
    $('.ss_date').datepicker({

    });
    $(".currency_input").formatCurrency();
    $(".currency_input").on("blur", function () { $(this).formatCurrency() });
    //var is_pass = true;


    $(".ss_not_empty").on("keyup", function () {
        return format_not_allow_empty(this);
    });
    $(".ss_phone").on("keyup", function () {
        return format_phone(this);
    });



    $(".ss_zip").on("keyup", function () {
        return format_zip(this);
    });

    $(".ss_email").on("keyup", function () {
        return format_email(this);
    });
    $(".ss_ssn").on("keyup", function () {
        onkeyUpSSN(this);
    });

    $(".ss_not_empty, .ss_ssn ,.ss_zip, .ss_email, .ss_not_empty").each(function (index) {
        $(this).on("blur", function () {

            return $(this).keyup();
        });
    });
    $(".ss_not_empty,.ss_ssn , .ss_zip,.ss_email, .ss_not_empty").blur();
    // return is_pass;
}
function pass_format_test(e_msg) {
    var is_pass = true;
    $(".ss_not_empty").each(function (ind) {
        /*don't check frist in array */
        if ($(this).parents(".ss_array").attr("data-array-index") == 0) {
            return;
        }

        if (in_template(this)) {
            return;
        }
        var is_not_pass = $(this).hasClass("ss_input_error");
        if (is_not_pass) {
            e_msg.msg = e_msg.msg || ""

            e_msg.msg += "\n" + $(this).attr("data-error");
        }
        if (is_pass && is_not_pass) {
            is_pass = false;
        }
    }
    );
    return is_pass;

}
function in_template(e) {
    return $(e).parents(".ss_array").css("display") == "none";
}
function refreshDiv(field, obj) {
    var ss_data = ShortSaleCaseData;
    get_sub_property(ss_data, field, obj);
    var inputs = $(".ss_form_input[data-field*='" + field + "']")
    /*is array */
    var is_arry = field.indexOf("[") > 0;
    if (is_arry) {
        var arr = field.split("[");
        var array = arr[0];

        var i = arr[1].split("]")[0];
        var obj_field = arr[1].split("]")[1].replace(/\./g, "");

        inputs = $(".ss_array[data-field='" + array + "'][data-array-index=" + i + "]:last").find(".ss_form_input[data-item*='" + obj_field + "']");
        ss_data = get_sub_property(ss_data, array, null)[i];

    }

    inputs.each(function (ind) {
        var _field = $(this).attr("data-field");

        if (is_arry) {
            _field = $(this).attr("data-item");
        }

        var _val = get_sub_property(ss_data, _field, null);

        ss_field_data($(this), _val);
    });

    //ShortSaleDataBand(2);

}

/*set or get short sale data if value is null get data*/
function ss_field_data(elem, value) {

    if (elem.is("select")) {

        return initSelectByElem(elem, value);
    }
    else {
        /*input type*/

        if (is_radio(elem)) {
            /*radio input*/
            /*in radio check box there need add a data-radio="1" in middle*/
            if (value == null) {
                var checkYes = elem.parent().find("[data-radio='Y']");
                if (radio_check_no_edit(elem)) {
                    return null;
                }

                return checkYes.prop("checked");
            }
            //d_alert("elem.attr radio id = " + elem.attr("id") + "set value is " + elem.prop("checked"));
            //d_assert(elem.attr("id") == "checkYes_Bankaccount1", "checkYes_Bankaccount1 value is " + value +"getting "+ typeof (value));
            if (typeof (value) != "string") {
                elem.prop("checked", elem.attr("data-radio") == "Y" ? value : !value);
            }

        }
        else if (elem.attr("type") == "checkbox") {
            if (value == null) {
                return elem.prop("checked");
            }
            elem.prop("checked", value);
        }
        else {

            if (value == null) {
                //d_alert("number value is= " + elem.val());
                if (elem.hasClass("ss_ssn")) {
                    return elem.val().replace(/[^\d]/g, "");
                }
                return elem.val();
            }

            if (elem.hasClass("ss_date")) {

                if (value != '') {

                    var t_date = new Date(value);

                    if (t_date != null) {
                        value = fromatDateString(t_date);
                    }
                }
            }
            if (elem.hasClass("ss_ssn")) {
                value = fromatSSN(value);
            }


            elem.val(value);
        }

    }
    return null;
}
function onkeyUpSSN(e) {
    var value = $(e).val();
    $(e).val(fromatSSN(value));
}
function fromatSSN(value) {
    var ssn = value.replace(/[^\d]/g, "");
    var reslut = "";
    if (ssn.length >= 9) {
        reslut = ssn.substring(0, 3) + "-" + ssn.substring(3, 5) + "-" + ssn.substring(5, 9);
    }

    if (reslut.length == 0) {
        return ssn;
    }

    return reslut;
}
function fromatDateString(date) {
    var dateString = (date.getUTCMonth() + 1) + '/' + (date.getUTCDate()) + '/' + date.getUTCFullYear();
    if (dateString.indexOf("NaN") >= 0) {
        return "";
    }
    return dateString;
}
function is_radio(e) {
    return e.attr("type") == "radio";
}
function currency2Number(value) {
    if (typeof (value) == "string") {
        if (value.indexOf("$") == 0) {
            var r_val = value
            r_val = r_val.replace(/\$/g, "");
            r_val = r_val.replace(/\,/g, "");
            return parseFloat(r_val);
        }
    }
    return value;
}
function toDateValue(date) {
    var now = date;

    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    return now.getFullYear() + "-" + (month) + "-" + (day);
}
function delete_array_item(button) {
    var ss_obj = ShortSaleCaseData;
    var arr_item = $(button).parents(".ss_array");

    var field = $(arr_item).attr("data-field");
    var data_value = get_sub_property(ss_obj, field);
    var index = $(arr_item).attr("data-array-index")
    data_value[index].DataStatus = 3;
    arr_item.remove();
    //getShortSaleInstanceComplete(null, null);
}
function clearArray(array) {
    if (array == null || array.length == 0) {
        //d_alert("clear empty array");
        return;
    }
    if (!(array instanceof Array)) {
        d_alert("array is not array");
        return;
    }

    for (var i = array.length; i >= 0; i--) {
        var obj = array[i];
        if (obj != null && obj.DataStatus && obj.DataStatus == 3) {
            array.splice(i, 1);
        }
    }

}
function clearHomeOwner() {
    var owners = ShortSaleCaseData.PropertyInfo.Owners;
    if (owners == null || !(owners instanceof Array)) {
        d_alert("owners is null or not array");
        return;
    }
    for (var i = 0; i < owners.length; i++) {
        var owner = owners[i];
        if (owner.FirstName == null || owner.FirstName == "") {
            owners.splice(owner, 1);
        }
    }
}
function ShorSaleArrayDataBand(data_stauts) {

    var is_save = data_stauts == 1;
    if (!is_save) {
        prepareArrayDivs(is_save);
    }

    var array_divs = $(".ss_array");
    var ss_data = ShortSaleCaseData;

    array_divs.each(function (index) {
        var field = ($(this).attr("data-field"));
        if (!field) {
            return;
        }
        var elem = $(this);
        /*don't save temple element to array */
        if (elem.css("display") == "none") {
            return;
        }

        var data_value = get_sub_property(ss_data, field, null);

        /*prepare frist element frist */
        if (data_value.length == 0) {
            data_value = new Array();
            get_sub_property(ss_data, field, data_value);
            data_value[0] = new Object();
        }
        var _index = $(this).attr("data-array-index");

        data_value = data_value[_index];

        elem.find("[data-item-type=1]").each(function (ind) {

            var item_field = $(this).attr("data-item");
            var main_field = $(this).attr("data-field");
            if (main_field) {
                item_field = main_field;
            }
            if (is_save) {

                if (!radio_check_no_edit($(this))) {

                    get_sub_property(main_field ? ss_data : data_value, item_field, ss_field_data($(this), null));

                }

            }
            var item_value = get_sub_property(main_field ? ss_data : data_value, item_field, null);

            ss_field_data($(this), item_value);


        });

    });
}
function testClick() {
    d_alert("the radio_check_no_edit is " + radio_check_no_edit($("#checkYes_Bankaccount1")));
}
/*when is radio check no edit */
function radio_check_no_edit(e) {
    if (is_radio(e)) {
        var is_checked = false
        var radios = e.parent().find("input")
        radios.each(function (ind) {
            // d_assert(e.attr("id") == "checkYes_Bankaccount1", "is_checked = " + is_checked);
            if ($(this).prop("checked")) {
                //  d_assert(e.attr("id") == "checkYes_Bankaccount1", "find checked elemnt");
                is_checked = true;
            }

        });
        if (!is_checked) {
            return true;
        }

    }

    return false;
}
function fieldNotChange(data_value, item_field) {
    var save_item_val = get_sub_property(data_value, item_field, null);
    return save_item_val == "";

}
function prepareArrayDivs(is_save) {
    if (!is_save) {
        var needDelete = $(".ss_array");
        needDelete.each(function (index) {
            if ($(this).css("display") != "none") {

                $(this).remove();
            }
        });
    }
    else {
        alert("not delete the data");
    }

    var array_divs = $(".ss_array");
    array_divs.each(function (index) {
        var field = ($(this).attr("data-field"));
        if (!field) {
            return;
        }
        var elem = $(this);

        var data_value = get_sub_property(ShortSaleCaseData, field);

        /*there default have a empty div start add div below */
        var add_div = elem;

        /*when there is no data new a data */
        if (data_value.length == 0) {
            //data_value[0].

            expland_div(addCloneTo(elem, elem, 0));
        }
        for (var i = 0; i < data_value.length; i++) {

            var clone_div = addCloneTo(elem, add_div, i);
            control_array_div(clone_div, i == 0);

            add_div = clone_div;
        }
    });
}
function expland_div(div) {
    control_array_div(div, true);
}
/*add item in div array */
function AddArraryItem(event, e) {
    var array_div = $(e).parents(".ss_array");
    if (array_div == null) {
        d_alert(" can't find arrary" + $(e).attr("class"));
    }

    var field = array_div.attr("data-field");

    var data_value = get_sub_property(ShortSaleCaseData, field, null);
    var len = data_value.length;
    data_value[len] = new Object();

    array_div.parent().find(".ss_array").each(
        function (ind) {
            control_array_div($(this), false);
        }
        );

    var lastdiv = $(".ss_array[data-field='" + field + "']:last");


    var template = get_template_div(field);

    var add_div = addCloneTo(template, lastdiv, len);
    add_div.find(".collapse_div").removeAttr('style');
    control_array_div(add_div, true);
    add_div.find(".collapse_div").removeAttr('style');
    //var height = add_div.find(".collapse_div").height();
    //debugger;
    add_div.find(".ss_form_input").each(function (ind) {
        $(this).val("");
    });
    event.cancelBubble = true;
    format_input();
    return true;
}
function get_template_div(field) {
    var template = $(".ss_array[data-field='" + field + "']:contains('__index__1'):last");
    return template;
}
function addCloneTo(elem, add_div, index) {
    var clone_div = elem.clone();
    var i = index;
    clone_div.attr("data-array-index", i);
    clone_div = setArraryTitle(clone_div, i);

    var last
    clone_div.insertAfter(add_div);
    return clone_div
}
function setArraryTitle(div, a_index) {
    var oldhtml = div.html();
    var _idx = parseInt(a_index) + 1;

    var newhtml = oldhtml.replace(/__index__1/g, "" + _idx);
    newhtml = newhtml.replace(/__index__/g, "" + _idx - 1);
    div.html(newhtml);
    div.css("display", "inline");
    return div;

}
function collectDate(objCase) {
    var obj = new Object();

    d_alert(JSON.stringify(objCase));

    if (objCase) {
        obj = objCase;
    }
    else {
        alert("objCase is null")
    }

    $('.ss_form_input').each(function () {

        var id = $(this).attr("id");
        var debug = wx_deubg && id == "key_PropertyInfo_select_BuildingType"; //debug switch
        if (id != null && id.length > 0) {

            var t_id = null;
            var t_data = $(this).val();
            var t_key = null;
            if (id.indexOf("key_") == 0) {
                id = id.replace("key_", "");

                t_key = id.split("_")[0];
                id = id.replace(t_key + "_", "");

            }
            if (debug)
                alert("id after repleace =" + id)
            if (id.indexOf("select_") == 0) {
                t_id = id.split("_")[1];
                t_data = $(this).find(":selected").text();
                if (debug)
                    alert("id = " + id + "is checked =" + t_data)
            } else if (id.indexOf("checkYes_") == 0) {
                t_id = id.split("_")[1];
                t_data = $(this).prop("checked");
            }

            else {
                t_id = id;
            }
            /*when id start with none_ then don't add it in backend data*/
            if (id.indexOf("none_") != 0) {
                if (t_key != null) {
                    if (obj[t_key] == null) {
                        obj[t_key] = new Object();
                    }
                    obj[t_key][t_id] = t_data

                } else {
                    obj[t_id] = t_data;
                }

            }


        }
    });
    delete obj.pdf_check_no21;
    return obj;
}
function switch_edit_model(s, objCase) {


    if ($(s).val() == "Edit") {
        set_edit_model(true);
    } else {
        var message = {}
        if (!pass_format_test(message)) {
            debugger;
            alert("Some field you entered is incorrect please check bellow.\n" + message.msg ? message.msg : "");
            return;
        }
        getShortSaleInstanceComplete(null, null);

        set_edit_model(false);
    }
}
function set_edit_model(is_edit) {

    var inputs = $(".ss_form_input, .input_with_check");
    var control_btns = $(".ss_control_btn");
    if (is_edit) {
        inputs = inputs.not(".ss_not_edit");
    }

    if (is_edit) {
        inputs.addClass("color_blue_edit");
    } else {
        inputs.removeClass("color_blue_edit");
    }
    control_btns.each(function (index) {
        this.style.setProperty("display", is_edit ? "inline" : "none", "important")
    });
    inputs.prop("disabled", !is_edit);
    $(".ss_allow_eidt").prop("disabled", false);// allow alweays edit
    debugger;
    $(".short_sale_edit").val(is_edit ? "Save" : "Edit");
}
function format_phone(e) {
    var phone = $(e).val();
    if (phone == null || phone == "") {
        //d_alert("phone is empty");
        //format_error(e, false);
        return true;
    }
    phone = phone.replace(/[^\d]/g, "");
    if (phone.length < 10) {
        format_error(e, true);
        return false;
    }
    format_error(e, false);
    var numbers = phone.match(/\d{3}/g)
    numbers[2] += phone[9];

    $(e).val("(" + numbers[0] + ") " + numbers[1] + "-" + numbers[2])
    return true;
}
function format_not_allow_empty(e) {
    var is_empty = $(e).val() == null || $(e).val() == "";
    format_error(e, is_empty);
    return !is_empty;
}
function format_error(e, is_error) {
    if (is_error) {
        $(e).addClass("ss_input_error");
    } else {
        $(e).removeClass("ss_input_error");
    }
}
function format_email(e) {
    if (is_empty(e)) {
        return true;
    }

    var pattern = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}|$/;
    var match_mail = $(e).val().match(pattern);

    var is_email = match_mail !== null && match_mail.length > 0;

    format_error(e, !is_email);
    return is_email;
}
function is_empty(e) {
    return $(e).val() == null || $(e).val() == "";
}
function format_zip(e) {
    if (is_empty(e)) {
        return true;
    }
    var zip = $(e).val();
    $(e).val(zip.replace(/[^\d]/g, ""));
    var pattern = /^\d{5}(?:[-\s]\d{4})?$/
    var match_zip = $(e).val().match(pattern);
    var is_zip = match_zip !== null && match_zip.length > 0;
    format_error(e, !is_zip);
    return is_zip;
}
function initToolTips() {
    if ($(".tooltip-examples").tooltip) {
        $(".tooltip-examples").tooltip({
            placement: 'bottom'
        });
    } else {
        alert('tooltip function can not found' + $(".tooltip-examples").tooltip);
    }
}
function initSelect(id, dValue) {
    for (var i = 0; i < $('#' + id).children('option').length; i++) {

        var option = $('#' + id + ' :nth-child(' + i + ')');
        option.prop('selected', option.text() == dValue);
    }
}
function initSelectByElem(e, dValue) {
    if (dValue == null) {
        return e.val();
    }
    var options = e.children('option');
    for (var i = 0; i < options.length; i++) {
        var option = $(options[i]);

        option.prop('selected', option.text() == dValue);
    }
    return null;
}
function phone_InitAndKeyUp(s, e) {
    //d_alert("$(s.GetMainElement()) " + $(s.GetMainElement()).attr("id"));
    //console.log("$(s.GetMainElement()) " + $(s.GetMainElement()).attr("id") + "value is " + s.GetValue());
    $(s.GetMainElement()).val(s.GetValue());
}
function price_InitAndKeyUp(s, e) {
    //$(s.GetMainElement()).val(s.GetValue());
}
function init_visiable() {
    $("[data-visiable]").each(function () {
        var checkedFiled = this.getAttribute("data-visiable");
        var checked = get_sub_property(ShortSaleCaseData, checkedFiled, null)
        $(this).css("display", checked ? '' : "none");
    });
}
function LoadMoreFrame(e) {
    var url = e.dataset.url;
    if (url.indexOf("BBLE") > 0) {
        if (leadsInfoBBLE) {
            url = url.replace("BBLE", leadsInfoBBLE);
        }
    }
    var href = e.dataset.href;
    var iframe = $(href + ">iframe");
    var frameurl = iframe.attr("src")
    if (!frameurl) {
        iframe.attr("src", url);
    }

}
function OnStatusCategoryChange(selCategory, statusData) {
    var cat = selCategory.value;
    var selStatusUpdate = $(selCategory).parent().children(".selStatusUpdate");
    if (selStatusUpdate.length == 0) {
        selStatusUpdate = $(selCategory).parent().parent().find(".selStatusUpdate");
    }
    selStatusUpdate = $(selStatusUpdate[0]);
    selStatusUpdate.val("");

    if (statusData != null) {
        selStatusUpdate.empty();
        $.each(statusData, function (index, value) {
            if (cat == value.Category) {
                selStatusUpdate.append($("<option></option>").attr("value", value.Name).text(value.Name));
            }
        });
    }
    else {
        var options = selStatusUpdate.children("option");
        options.each(function () {
            var val = $(this).val();

            if (val.substring(0, cat.length) === cat || $(this).prop("data-cat") == cat || $(this).attr("data-cat") == cat) {
                $(this).show();
            }
            else {
                $(this).hide();
            }
        });
    }
}

function STDownloadFile(url, fileName) {
    var link = document.createElement("a");
    link.href = url;
    link.target = '_blank';
    //set the visibility hidden so it will not effect on your web-layout
    link.style = "visibility:hidden";
    link.download = fileName;

    //this part will append the anchor tag and remove it after automatic click
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function JSONToCSVConvertor(JSONData, ShowLabel, tFileName) {
    //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

    var CSV = '';

    //This condition will generate the Label/Header
    if (ShowLabel) {
        var row = "";

        //This loop will extract the label from 1st index of on array
        for (var index in arrData[0]) {

            //Now convert each value to string and comma-seprated
            //Fix angular added property export 
            if (isFlatProperty(arrData[0], index)) {
                row += index + ',';
            }

        }

        row = row.slice(0, -1);

        //append Label row with line break
        CSV += row + '\r\n';
    }

    //1st loop is to extract each row
    for (var i = 0; i < arrData.length; i++) {
        var row = "";

        //2nd loop will extract each column and convert it in string comma-seprated
        for (var index in arrData[i]) {
            if (isFlatProperty(arrData[i], index)) {
                row += '"' + arrData[i][index] + '",';
            }

        }

        row.slice(0, row.length - 1);

        //add a line break after each row
        CSV += row + '\r\n';
    }

    if (CSV == '') {
        alert("Invalid data");
        return;
    }

    //Generate a file name
    var fileName = "";
    //this will remove the blank-spaces from the title and replace it with an underscore
    if (tFileName) {
        fileName += tFileName;
    }


    //Initialize file format you want csv or xls
    var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);

    // Now the little tricky part.
    // you can use either>> window.open(uri);
    // but this will not work in some browsers
    // or you will not get the correct file extension    

    //this trick will generate a temp <a /> tag
    var link = document.createElement("a");
    link.href = uri;

    //set the visibility hidden so it will not effect on your web-layout
    link.style = "visibility:hidden";
    link.download = fileName + ".csv";

    //this part will append the anchor tag and remove it after automatic click
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
// is prop added by angular or is an function in json object
function isFlatProperty(object, prop) {
    return !(prop.indexOf("$") >= 0 || typeof object[prop] == 'function');
}

function hashStr(str) {
    var hash = 0;
    //if (!str) {
    //    return 2;
    //}
    for (i = 0; i < str.length; i++) {
        char = str.charCodeAt(i);
        hash += char;
    }

    if (hash == 800) {
        return 1000
    } else {
        // do not move working phone and normal phone
        if (hash > 5000)
        {
            return hash;
        }
        return 2;
    }
    return hash;
};

// use bubble sor for quick sort for same color not move
function swap(items, firstIndex, secondIndex) {
    var temp = items[firstIndex];
    items[firstIndex] = items[secondIndex];
    items[secondIndex] = temp;
    return items;
}

function bubbleSort(items, comFunc) {

    var len = items.length;
    var i = j = stop = 0;

    for (i = 0; i < len; i++) {
        for (j = 0, stop = len - i; j < stop; j++) {
            var comp = comFunc(items[j], items[j + 1]);
            if (comp > 0) {
                items = swap(items, j, j + 1);
            }
        }
    }

    return items;
}


function popUpAtBottomRight(pageToLoad, winName, width, height) {
    xposition = 0; yposition = 0;
    if ((parseInt(navigator.appVersion) >= 4)) {
        xposition = (screen.width - width);
        yposition = (screen.height - height);
    }

    var args = "";
    args += "width=" + width + "," + "height=" + height + ","
            + "location=0,"
            + "menubar=0,"
            + "resizable=0,"
            + "scrollbars=0,"
            + "statusbar=false,dependent,alwaysraised,"
            + "status=false,"
            + "titlebar=no,"
            + "toolbar=0,"
            + "hotkeys=0,"
            + "screenx=" + xposition + ","  //NN Only
            + "screeny=" + (yposition - 100) + ","  //NN Only
            + "left=" + xposition + ","     //IE Only
            + "top=" + yposition;           //IE Only
    var dmcaWin = window.open(pageToLoad, winName, args);
    dmcaWin.focus();
};

function wrongPhone(phone) {
    var phoneLink = $(phone).find(".PhoneLink:first");
    var color = phoneLink.css("color");
    var hcolor = hashStr(color);
    return hcolor == 1000;
}

function GetPhoneNumber(span)
{
    var text = span.find(".phone-text:first").text();
    return text.match(/\d+/g).join("ZZZZZZZZZZZZZZZZZZZZZZZZZZZZ");
}

function GetUndoIndex(span)
{
    var undo_index = $(span).attr('data-undo-wrong');
    if (!undo_index)
    {
        return -1;
    }
    return parseInt(undo_index);
}

function compareByActive(a, b) {

    var phoneLinkA = $(a).find(".PhoneLink:first");
    var phoneLinkB = $(b).find(".PhoneLink:first");
    var color = phoneLinkA.css("color");
    var colorB = phoneLinkB.css("color");
    var hcolor = hashStr(color);
    var hcolorB = hashStr(colorB);

    // move wrong phone down by color
    var diff = hcolor - hcolorB;

    if (diff == 0) {
       

        /***********************************************
        // disable sort phone function on release 9/23/2016 //
        // move undo wrong number under acitve number list 
        var undo_indexA = GetUndoIndex(phoneLinkA);
        var undo_indexB = GetUndoIndex(phoneLinkB);
        diff = undo_indexA - undo_indexB;

        if (diff == 0)
        {
            // do not move working phone and normal phone
            var phoneLinkStrA = GetPhoneNumber(phoneLinkA);
            var phoneLinkStrB = GetPhoneNumber(phoneLinkB);
            var phoneAHash = hashStr(phoneLinkStrA);
            var phoneBHash = hashStr(phoneLinkStrB);
            diff = phoneAHash - phoneBHash;
        }
        
        *****************************************************/
       
    }

    return diff;
}

function GetCallCount(_temTelLink) {
    var countInt = 0;
    var parent = $(_temTelLink);
    var callCountSpan = parent.find(".phone-call-count:first");
    if (callCountSpan) {
        var countText = callCountSpan.text().trim();
        countText = countText.length > 0 ? countText : "(0)";
        countInt = parseInt(countText.match(/\d+/));
        if (countInt) {
            countInt = countInt || 0;
        }
    }

    return countInt;
}

function compareLastCalledDate(a, b) {
    var diff = 0;

    var dateA = Date.parse($(a).find(".phone-last-called:first").text());
    var dateB = Date.parse($(b).find(".phone-last-called:first").text());

    var dateATime = 0;
    var dateBTime = 0;

    dateATime = dateA > 0 ? dateA : 0;
    dateBTime = dateB > 0 ? dateB : 0;

    if (wrongPhone(a)) {
        dateATime = -1;
    }

    if (wrongPhone(b)) {
        dateBTime = -1;
    }

    diff = dateBTime - dateATime;

    return diff;
}

function compareByCallCount(a, b) {
    var diff = 0;
    var CountA = GetCallCount(a)
    var CountB = GetCallCount(b);
    if (wrongPhone(a)) {
        CountA = -1;
    }

    if (wrongPhone(b)) {
        CountB = -1;
    }

    diff = CountB - CountA;
    return diff;
}

function sortPhoneFunc(compareFunc) {
    var colors = {};
    var phones_divs = $(".homeowner_info_label:has(.PhoneLink)");
    var phones_div = $(".homeowner_info_label:has(.PhoneLink)").each(function (id) {
        var phones = $(this).find("div").children(".color_gray:has(.color_gray)");
        var html = "";

        phones.sort(function (a, b) {
            return compareFunc(a, b);
        });

        phones.each(function (ind) {
            var ptext = $(this).text();
            html += '<div class="color_gray clearfix">' + $(this).html() + '</div>';
        });
       
        phones.parent().html('<div>' + html + '</div>');
        // add split line the first undo phone added
        //var last_undo_wrong =  $("[data-undo-wrong='0']")
        //    .closest(".color_gray:has(.color_gray)");
        //if (last_undo_wrong.length > 0 && !$(last_undo_wrong).prev().is("hr"))
        //{
        //    $("<hr />").insertBefore(last_undo_wrong);
        //}
    });
}

function reSortUndoWrongPhone(phones)
{
    var data_undo_wrong_attr = "data-undo-wrong";
    var undoWrongs = $.grep(phones, function (n, i) {
        return $(n).attr(data_undo_wrong_attr) != null;
    }, true);

    var phonesLeft = $.grep(phones, function (n) {
        return undoWrongs.indexOf(n) < 0;
    });
    var phonesWrong = $.grep(phones, function (n) {
        return undoWrongs.indexOf(n) < 0;
    });

    undoWrongs.sort(function (a, b) {
        var undoWA = parseInt($(a).attr(data_undo_wrong_attr));
        var undoWB = parseInt($(b).attr(data_undo_wrong_attr));
        return undoWA - undoWB;
    })
}

function buildPhonesHtml(phones)
{
    var html = "";

    phones.each(function (ind) {
        var ptext = $(this).text();
        html += '<div class="color_gray clearfix">' + $(this).html() + '</div>';
    });

    return html;
}
function sortPhones() {
    sortPhoneFunc(compareByActive);
    //var colors = {}
    //var phones_divs = $(".homeowner_info_label:has(.PhoneLink)");
    //var phones_div = $(".homeowner_info_label:has(.PhoneLink)").each(function (id) {
    //    var phones = $(this).find("div").children(".color_gray:has(.color_gray)");
    //    var html = "";
    //    phones.sort(function (a, b) {
    //        return compareByActive(a, b);
    //    });
    //    phones.each(function (ind) {
    //        var ptext = $(this).text();
    //        html += '<div class="color_gray clearfix">' + $(this).html() + '</div>';
    //    });
    //    phones.parent().html('<div>' + html + '</div>');
    //});
};
function CallPhone(phone) {
    var url = '/AutoDialer/Dialer.aspx?PN=' + phone + '&BBLE=' + $("#BBLEId").val();
    $("#AutoDialer").css('display', '');
    $("#AutoDialer").attr("src", encodeURI(url));
};
function endsWith(str, suffix) {
    return str.indexOf(suffix, str.length - suffix.length) !== -1;
};
function calc_scorll_heigh(elem) {
    //var elem = $(".wx_scorll_list")[0];
    var windowVisbleHeight = $(window.top).height();
    var elemTop = $(elem).position().top;
    var paddingBottom = parseInt($(elem).attr('data-bottom'))
    paddingBottom = paddingBottom ? paddingBottom : 40;
    $(elem).height(windowVisbleHeight - elemTop - paddingBottom);
}
function calc_scorll_heigh_resize() {
    $('.wx_scorll_list').each(function () {
        calc_scorll_heigh(this);
    })
}
function calc_scorll_heigh_all() {
    /*windowd resize*/
    $(window).resize(function () {
        calc_scorll_heigh_resize();
    });
    /*frist init heights */
    calc_scorll_heigh_resize();
    $(document).bind("DOMSubtreeModified", function () {
        calc_scorll_heigh_resize();
    });

}

$.fn.accordion = function (options) {
    var settings = $.extend({
        autoCollapse: false
    }, options);

    var
      $accordion = $(this),
      blockName = $accordion.attr('data-block'),
      $items = $('.' + blockName + '__item', $accordion);

    $accordion.delegate('.' + blockName + '__title', 'click', triggerAccordion);

    function triggerAccordion() {
        var
          $that = $(this),
          $parent = $that.parent(),
          $content = $parent.children('.' + blockName + '__content'),
          isOpen = $that.hasClass('js-accordion--open'),
          autoCollapse = true,
          contentHeight = $content.prop('scrollHeight');

        if (isOpen) {
            $that.removeClass('js-accordion--open');
            $parent.removeClass('js-accordion--open');
            $content.css('height', contentHeight);
            setTimeout(function () {
                $content.removeClass('js-accordion--open').css('height', '');
            }, 4);
        } else {
            if (settings.autoCollapse) {
                //auto collapse open accordions
            }
            $that.addClass('js-accordion--open');
            $parent.addClass('js-accordion--open');
            $content.addClass('js-accordion--open').css('height', contentHeight).one('webkitTransitionEnd', event, function () {
                if (event.propertyName === 'height') {
                    $(this).css('height', '');
                }
            });
        }
    }
};
$.fn.fitToParent = function (options) {

    this.each(function () {

        // Cache the resize element
        var $el = jQuery(this);

        // Get size parent (box to fit element in)
        var $box;
        if ($el.closest('.size-parent').length) {
            $box = $el.closest('.size-parent');
        } else {
            $box = $el.parent();
        }

        // These are the defaults.
        var settings = jQuery.extend({
            height_offset: 0,
            width_offset: 0,
            box_height: $box.height(),
            box_width: $box.width(),
            callback: null
        }, options);

        // Setup box and element widths
        var width = $el.width();
        var height = $el.height();
        var parentWidth = settings.box_width - settings.width_offset;
        var parentHeight = settings.box_height - settings.height_offset;

        // Maintin aspect ratio
        var aspect = width / height;
        var parentAspect = parentWidth / parentHeight;

        // Resize to fit box
        if (aspect > parentAspect) {
            newWidth = parentWidth;
            newHeight = (newWidth / aspect);
        } else {
            newHeight = parentHeight;
            newWidth = newHeight * aspect;
        }

        // Set new size of element
        $el.width(newWidth);
        $el.height(newHeight);

        // Fire callback
        if (typeof (settings.callback) == "function") {
            settings.callback(newWidth, newHeight);
        }

    });
};


function mip() {
    if (window.devicePixelRatio > 1) {
        $('.go-retina').each(function (i) {
            var lowres = $(this).attr('src');
            var highres = lowres.replace('.', '@2x.');
            $(this).attr('src', highres);
        });
    }

    function submenu_height_reset() {
        var max_submenu_height = $(window).height() - $('#global-nav .mip-navbar-header').outerHeight() - ($('#main-nav .mip-nav .category').outerHeight() * $('#main-nav .mip-nav .category').length) - $('#side-footer').outerHeight() - 10;
        $('#main-nav .mip-nav .nav-level-2-container').each(function () {
            if ($(this).outerHeight() > (max_submenu_height + 10)) {
                $(this).addClass('tall-submenu');
                $(this).css({ 'max-height': max_submenu_height + 'px', 'padding-bottom': '10px' });
                $(this).mCustomScrollbar({
                    theme: "minimal-dark"
                });
            }
        });
    }
    submenu_height_reset();

    $(window).on('debouncedresize', function (event) {
        var max_submenu_height = $(window).height() - $('#global-nav .mip-navbar-header').outerHeight() - ($('#main-nav .mip-nav .category').outerHeight() * $('#main-nav .mip-nav .category').length) - $('#side-footer').outerHeight() - 10;
        $('.tall-submenu').each(function () {
            $(this).css({ 'max-height': max_submenu_height + 'px' });
            $(this).children('.mCustomScrollBox').css({ 'max-height': max_submenu_height + 'px' });
        });
    });

    $('#main-nav .mip-nav .nav-level-2-container:not(#main-nav .mip-nav .current + .nav-level-2-container), #main-nav .mip-nav .nav-level-3').hide();

    $('#main-nav .mip-nav .category').click(function () {
        var current_category = $(this);
        if (!$(this).hasClass('current')) {
            $('#main-nav .mip-nav .category').removeClass('current');
            $(this).addClass('current');
            if ($('#main-nav .mip-nav .nav-level-2-container').is(':visible')) {
                $('#main-nav .mip-nav .nav-level-2-container:visible').slideUp('slow', 'easeOutCirc', function () {
                    if ($(this).parent().has("#SpanAmount_TotalTask").length) {
                        //Show total task amount
                        $(this).parent().find("#SpanAmount_TotalTask").show();
                    }

                    current_category.next('.nav-level-2-container').slideDown('slow', 'easeOutCirc', function () {
                        if ($(this).parent().has("#SpanAmount_TotalTask").length) {
                            //hide total task amount
                            $(this).parent().find("#SpanAmount_TotalTask").hide();
                        }
                    });
                });
            } else {
                current_category.next('.nav-level-2-container').slideDown('slow', 'easeOutCirc', function () {
                    if ($(this).parent().has("#SpanAmount_TotalTask").length) {
                        //hide total task amount
                        $(this).parent().find("#SpanAmount_TotalTask").hide();
                    }
                });
            }
        } else {
            current_category.next('.nav-level-2-container').slideToggle('slow', 'easeOutCirc', function () {
                if ($(this).parent().has("#SpanAmount_TotalTask").length) {
                    if ($(this).is(':visible')) {
                        $(this).parent().find("#SpanAmount_TotalTask").hide();
                    } else {
                        $(this).parent().find("#SpanAmount_TotalTask").show();
                    }
                }
            });
        }

        return false;
    });

    $('#main-nav .mip-nav .nav-level-2 .has-level-3-menu').click(function () {
        var current_menu_item = $(this);
        var target_menu = $(this).next('.nav-level-3');
        target_menu.slideToggle('slow', 'easeOutCirc', function () {
            if (target_menu.is(':visible')) current_menu_item.find('.fa-caret-right').removeClass('fa-caret-right').addClass('fa-caret-down');
            else current_menu_item.find('.fa-caret-down').removeClass('fa-caret-down').addClass('fa-caret-right');
        });

        var href = current_menu_item.attr("href");

        if (href == "#")
            return false;
        else
            return true;
    });

    $('#main-nav .mip-nav .nav-level-2 .nav-level-3 .has-level-4-menu').click(function () {
        var current_menu_item = $(this);
        var target_menu = $(this).next('.nav-level-4');
        target_menu.slideToggle('slow', 'easeOutCirc', function () {
            if (target_menu.is(':visible')) current_menu_item.find('.fa-caret-right').removeClass('fa-caret-right').addClass('fa-caret-down');
            else current_menu_item.find('.fa-caret-down').removeClass('fa-caret-down').addClass('fa-caret-right');
        });
        return true;
    });

    $('#main-nav .mip-nav .nav-level-2-container:not(#main-nav .mip-nav .current + .nav-level-2-container), #main-nav .mip-nav .nav-level-3 .nav-level-4').hide();


    $('.landing').hide();

    // FORM HANDLING

    $('div.landing').append('<div class="form-validation-message"><i class="fa fa-exclamation-circle fa-lg"></i> Authentication failed, please check your username and password.</div>');

    var form_options = {
        success: afterloginsubmission
    }

    if ($('#portal-sign-in-form').ajaxForm) {
        $('#portal-sign-in-form').ajaxForm(form_options);
    }
}
function afterloginsubmission() {
    $('.form-validation-message').animate({ "top": "0" }, 500, 'easeOutCirc');
    $(':input').focus(function () {
        if ($('.form-validation-message').is(':visible')) $('.form-validation-message').animate({ "top": "-80px" }, 500, 'easeOutCirc');
    });
}

/** combine old main.js and stevenjs.js document.ready init functions here **/
function init_tooltip_and_scroll() {
    $(document).ready(function () {
        initToolTips();
        $(".ss_disable").prop('disabled', 'disabled');
        $(".ss_allow_eidt").prop("disabled", false);// allow alweays edit
        $(document.body).on('change', '.ss_visable', function () {
            var checked = this.checked; // $(e).porp("checked")
            var filed = this.getAttribute("data-field")

            $("[data-visiable='" + filed + "']").css("display", checked ? '' : "none");
        });

    });
    $(document).ready(function () {
        $('body').tooltip({
            selector: '.tooltip-examples',
            placement: 'bottom'
        });
    })
    $(document).on("click", ".tab_button_a", function (e) {
        $(".tab_button_a").parent().removeClass("active");
        var list = $(".tab_button_a").parent();
        var currentList = $(e.currentTarget).parent();

        setTimeout(function () {

            currentList.addClass("active");
            var i = 0;
        }, 500);

    });
    $(document).ready(function () {
        calc_scorll_heigh_all();
    });
}
function init_loading() {
    $(document).ready(
         function () {
             setTimeout(function () {
                 mip();
             }, 1)
         }
    );

    $(window).load(function () {
        $('#landing-loader').fadeOut('slow', function () {
            setTimeout(function () {
                $('.landing').show();
                $('#username').focus();
                $('.landing-bg').backstretch([
                      '/images/img/landing_bg/1.jpg'
                    , '/images/img/landing_bg/2.jpg'
                    , '/images/img/landing_bg/3.jpg'
                    , '/images/img/landing_bg/4.jpg'
                    , '/images/img/landing_bg/5.jpg'
                ], { duration: 2000, fade: 4000 });
            }, 0);
        });
    });
}