﻿//
if (plant == "CCFlow") {
    // CCFlow
    dynamicHandler = "/WF/Comm/Handler.ashx";
} else {
    // JFlow
    dynamicHandler = basePath + "/WF/Comm/ProcessRequest.do";
}

/* 把一个 @XB=1@Age=25 转化成一个js对象.  */
function AtParaToJson(json) {
    var jsObj = {};
    if (json) {
        var atParamArr = json.split('@');
        $.each(atParamArr, function (i, atParam) {
            if (atParam != '') {
                var atParamKeyValue = atParam.split('=');
                if (atParamKeyValue.length == 2) {
                    jsObj[atParamKeyValue[0]] = atParamKeyValue[1];
                }
            }
        });
    }
    return jsObj;
}


function GetPKVal() {

    var val = this.GetQueryString("OID"); 

    if (val==undefined || val=="")
        val = GetQueryString("No");

    if (val == undefined || val == "")
        val = GetQueryString("WorkID");

    if (val == undefined || val == "")
        val = GetQueryString("NodeID");

    if (val == undefined || val == "")
        val = GetQueryString("MyPK");

    if (val == undefined || val == "")
        val = GetQueryString("PKVal");

    if (val == "null" || val == "" || val == undefined)
        return null;
    return val;
}

//处理url，删除无效的参数.
function DearUrlParas(urlParam) {

    //如何获得全部的参数？ &FK_Node=120&FK_Flow=222 放入到url里面去？
    //var href = window.location.href;
    //var urlParam = href.substring(href.indexOf('?') + 1, href.length);

    if (urlParam == null || urlParam == undefined)
        urlParam = window.location.search.substring(1);

    var params = {};
    if (urlParam == "" && urlParam.length == 0) {
        urlParam = "1=1"
    } else {
        $.each(urlParam.split("&"), function (i, o) {
            if (o) {
                var param = o.split("=");
                if (param.length == 2) {
                    var key = param[0];
                    var value = param[1];

                    if (key == "DoType")
                        return true;

                    if (value == "null" || typeof value == "undefined")
                        return true;

                    if (value != null && typeof value != "undefined"
                            && value != "null"
                            && value != "undefined") {
                        value = value.trim();
                        if (value != "" && value.length > 0) {
                            if (typeof params[key] == "undefined") {
                                params[key] = value;
                            }
                        }
                    }
                }
            }
        });
    }
    urlParam = "";
    $.each(params, function (i, o) {
        urlParam += i + "=" + o + "&";
    });

    urlParam = urlParam.replace("&&", "&");
    urlParam = urlParam.replace("&&", "&");
    urlParam = urlParam.replace("&&", "&");
    return urlParam;
}

function GetRadioValue(groupName) {
    var obj;
    obj = document.getElementsByName(groupName);
    if (obj != null) {
        var i;
        for (i = 0; i < obj.length; i++) {
            if (obj[i].checked) {
                return obj[i].value;
            }
        }
    }
    return null;
}

//获得所有的checkbox 的id组成一个string用逗号分开, 以方便后台接受的值保存.
function GenerCheckIDs() {

    var checkBoxIDs = "";
    var arrObj = document.all;

    for (var i = 0; i < arrObj.length; i++) {

        if (arrObj[i].type != 'checkbox')
            continue;

        var cid = arrObj[i].name;
        if (cid == null || cid == "" || cid == '')
            continue;

        checkBoxIDs += arrObj[i].id + ',';
    }
    return checkBoxIDs;
}

//填充下拉框.
function GenerBindDDL(ddlCtrlID, data, noCol, nameCol, selectVal) {

    if (noCol == null)
        noCol = "No";

    if (nameCol == null)
        nameCol = "Name";

    //判断data是否是一个数组，如果是一个数组，就取第1个对象.
    var json = data;

    //如果他的数量==0，就return.
    if (json.length == 0)
        return;

    if (data[0].length == 1)
        json = data[0];

    // 清空默认值, 写一个循环把数据给值.
    $("#" + ddlCtrlID).empty();

    if (json[0][noCol] == undefined) {
        alert('@在绑定[' + ddlCtrlID + ']错误，No列名' + noCol + '不存在,无法行程期望的下拉框value . ');
        return;
    }

    if (json[0][nameCol] == undefined) {
        alert('@在绑定[' + ddlCtrlID + ']错误，Name列名' + nameCol + '不存在,无法行程期望的下拉框value. ');
        return;
    }
    $("#" + ddlCtrlID).append("<option value=''>- 请选择 -</option>");
    for (var i = 0; i < json.length; i++) {
        $("#" + ddlCtrlID).append("<option value='" + json[i][noCol] + "'>" + json[i][nameCol] + "</option>");
    }

    //设置选中的值.
    if (selectVal != undefined) {

        var v = $("#" + ddlCtrlID)[0].options.length;
        if (v == 0)
            return;

        $("#" + ddlCtrlID).val(selectVal);

        var v = $("#" + ddlCtrlID).val();
        if (v == null) {
            $("#" + ddlCtrlID)[0].options[0].selected = true;
        }
    }
}

/*绑定枚举值.*/
function GenerBindEnumKey(ctrlDDLId, enumKey, selectVal) {
    $.ajax({
        type: 'post',
        async: true,
        url: dynamicHandler + "?DoType=EnumList&EnumKey=" + enumKey + "&m=" + Math.random(),
        dataType: 'html',
        success: function (data) {

            data = JSON.parse(data);
            //绑定枚举值.
            GenerBindDDL(ctrlDDLId, data, "IntKey", "Lab", selectVal);
            return;
        }
    });
}


/* 绑定枚举值外键表.*/
function GenerBindEntities(ctrlDDLId, ensName, selectVal, filter) {

    $.ajax({
        type: 'post',
        async: true,
        url: dynamicHandler + "?DoType=EnsData&EnsName=" + ensName + "&Filter=" + filter + "&m=" + Math.random(),
        dataType: 'html',
        success: function (data) {

            //            if (data.indexof('err@') ==0 )
            //            {
            //               alert(data);
            //               return ;
            //            }

            data = JSON.parse(data);
            //绑定枚举值.
            GenerBindDDL(ctrlDDLId, data, "No", "Name", selectVal);
            return;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            /*错误信息处理*/
            alert("GenerBindEntities,错误:参数:EnsName" + ensName + " , 异常信息 responseText:" + jqXHR.responseText + "; status:" + jqXHR.status + "; statusText:" + jqXHR.statusText + "; \t\n textStatus=" + textStatus + ";errorThrown=" + errorThrown);
        }
    });
}


/*
绑定外键表.
*/
function GenerBindSFTable(ctrlDDLId, sfTable, selectVal) {

    $.ajax({
        type: 'post',
        async: true,
        url: dynamicHandler + "?DoType=SFTable&SFTable=" + sfTable + "&m=" + Math.random(),
        dataType: 'html',
        success: function (data) {
            data = JSON.parse(data);
            //绑定枚举值.
            GenerBindDDL(ctrlDDLId, data, "No", "Name", selectVal);
            return;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            /*错误信息处理*/
            alert("GenerBindSFTable,错误:参数:EnsName" + ensName + " , 异常信息 responseText:" + jqXHR.responseText + "; status:" + jqXHR.status + "; statusText:" + jqXHR.statusText + "; \t\n textStatus=" + textStatus + ";errorThrown=" + errorThrown);
        }
    });
}

/* 绑定SQL.
1. 调用这个方法，需要在 SQLList.xml 配置一个SQL , sqlKey 就是该sql的标记.
2, paras 就是向这个sql传递的参数, 比如： @FK_Mapdata=BAC@KeyOfEn=MyFild  .
*/
function GenerBindSQL(ctrlDDLId, sqlKey, paras, colNo, colName, selectVal) {

    if (colNo == null)
        colNo = "NO";
    if (colName == null)
        colName = "NAME";

    $.ajax({
        type: 'post',
        async: true,
        url: dynamicHandler + "?DoType=SQLList&SQLKey=" + sqlKey + "&Paras=" + paras + "&m=" + Math.random(),
        dataType: 'html',
        success: function (data) {

            if (data.indexOf('err@') == 0) {
                alert(data);
            }

            data = JSON.parse(data);

            //绑定枚举值.
            GenerBindDDL(ctrlDDLId, data, colNo, colName, selectVal);

            return;
        }
    });
}

/*为页面的所有字段属性赋值. */
function GenerFullAllCtrlsVal(data) {

    //判断data是否是一个数组，如果是一个数组，就取第1个对象.
    var json = data;
    if (data.length == 1)
        json = data[0];

    var unSetCtrl = "";
    for (var attr in json) {

        var val = json[attr]; //值

        var div = document.getElementById(attr);
        if (div != null) {
            div.innerHTML = val;
            continue;
        }

        // textbox
        var tb = document.getElementById('TB_' + attr);
        if (tb != null) {
            if (val != null && '' != val && !isNaN(val) && !(/^\+?[1-9][0-9]*$/.test(val + '')))
                val = val.replace(new RegExp("~", "gm"), "'");
            //val = val.replace( /~/g,  "'");   //替换掉特殊字符,设置的sql语句的引号.

            if (tb.tagName.toLowerCase() != "input") {
                tb.innerHTML = val;
            }
            else {
                tb.value = val;
            }

            continue;
        }

        //checkbox.
        var cb = document.getElementById('CB_' + attr);
        if (cb != null) {
            if (val == "1")
                cb.checked = true;
            else
                cb.checked = false;
            continue;
        }

        //下拉框.
        var ddl = document.getElementById('DDL_' + attr);
        if (ddl != null) {

            if (ddl.options.length == 0)
                continue;

            $("#DDL_" + attr).val(val); // 操作权限.
            continue;
        }

        // RadioButton. 单选按钮.
        var rb = document.getElementById('RB_' + attr + "_" + val);
        if (rb != null) {
            rb.checked = true;
            continue;
        }

        // 处理参数字段.....................
        if (attr == "AtPara") {
            //val=@Title=1@SelectType=0@SearchTip=2@RootTreeNo=0
            $.each(val.split("@"), function (i, o) {
                if (o == "") {
                    return true;
                }
                var kv = o.split("=");
                if (kv.length == 2) {
                    json[kv[0]] = kv[1];
                    var suffix = kv[0];
                    var val = kv[1];

                    // textbox
                    tb = document.getElementById('TBPara_' + suffix);
                    if (tb != null) {

                        val = val.replace(new RegExp("~", "gm"), "'");
                        tb.value = val;
                        return true;
                    }

                    //下拉框.
                    ddl = document.getElementById('DDLPara_' + suffix);
                    if (ddl != null) {
                        if (ddl.options.length == 0)
                            return true;
                        $("#DDL_" + suffix).val(val); // 操作权限.
                        return true;
                    }

                    //checkbox.
                    cb = document.getElementById('CBPara_' + suffix);
                    if (cb != null) {
                        if (val == "1")
                            cb.checked = true;
                        else
                            cb.checked = false;
                        return true;
                    }

                    // RadioButton. 单选按钮.
                    rb = document.getElementById('RBPara_' + suffix + "_" + val);
                    if (rb != null) {
                        rb.checked = true;
                        return true;
                    }
                }
            });
        }
        unSetCtrl += "@" + attr + " = " + val;
    }
}


/*为页面的所有 div 属性赋值. */
function GenerFullAllDivVal(data) {

    //判断data是否是一个数组，如果是一个数组，就取第1个对象.
    var json = data;
    if (data.length == 1)
        json = data[0];

    var unSetCtrl = "";
    for (var attr in json) {

        var val = json[attr]; //值

        var div = document.getElementById(attr);

        if (div != null) {
            div.innerHTML = val;
            continue;
        }
    }

    // alert('没有找到的控件类型:' + unSetCtrl);
}

function DoCheckboxValue(frmData, cbId) {
    if (frmData.indexOf(cbId + "=") == -1) {
        frmData += "&" + cbId + "=0";
    }
    else {
        frmData.replace(cbId + '=on', cbId + '=1');
    }

    return frmData;
}


/*隐藏与显示.*/
function ShowHidden(ctrlID) {

    var ctrl = document.getElementById(ctrlID);
    if (ctrl.style.display == "block") {
        ctrl.style.display = 'none';
    } else {
        ctrl.style.display = 'block';
    }
}

function OpenDialogAndCloseRefresh(url, dlgTitle, dlgWidth, dlgHeight, dlgIcon, fnClosed) {
    ///<summary>使用EasyUiDialog打开一个页面，页面中嵌入iframe【id="eudlgframe"】</summary>
    ///<param name="url" type="String">页面链接</param>
    ///<param name="dlgTitle" type="String">Dialog标题</param>
    ///<param name="dlgWidth" type="int">Dialog宽度</param>
    ///<param name="dlgHeight" type="int">Dialog高度</param>
    ///<param name="dlgIcon" type="String">Dialog图标，必须是一个样式class</param>
    ///<param name="fnClosed" type="Function">窗体关闭调用的方法（注意：此方法中可以调用dialog中页面的内容；如此方法启用，则关闭窗体时的自动刷新功能会失效）</param>

    var dlg = $('#eudlg');
    var iframeId = "eudlgframe";

    if (dlg.length == 0) {
        var divDom = document.createElement('div');
        divDom.setAttribute('id', 'eudlg');
        document.body.appendChild(divDom);
        dlg = $('#eudlg');
        dlg.append("<iframe frameborder='0' src='' scrolling='auto' id='" + iframeId + "' style='width:100%;height:100%'></iframe>");
    }

    dlg.dialog({
        title: dlgTitle,
        left: document.body.clientWidth > dlgWidth ? (document.body.clientWidth - dlgWidth) / 2 : 0,
        top: document.body.clientHeight > dlgHeight ? (document.body.clientHeight - dlgHeight) / 2 : 0,
        width: dlgWidth,
        height: dlgHeight,
        iconCls: dlgIcon,
        resizable: true,
        modal: true,
        onClose: function () {
            if (fnClosed) {
                fnClosed();
                return;
            }

            Reload();
        },
        cache: false
    });

    dlg.dialog('open');
    $('#' + iframeId).attr('src', url);
}

function Reload() {
    ///<summary>重新加载当前页面</summary>
    var newurl = "";
    var urls = window.location.href.split('?');
    var params;

    if (urls.length == 1) {
        window.location.href = window.location.href + "?t=" + Math.random();
    }

    newurl = urls[0] + '?1=1';
    params = urls[1].split('&');

    for (var i = 0; i < params.length; i++) {
        if (params[i].indexOf("1=1") != -1 || params[i].toLowerCase().indexOf("t=") != -1) {
            continue;
        }

        newurl += "&" + params[i];
    }

    window.location.href = newurl + "&t=" + Math.random();
}

function ConvertDataTableFieldCase(dt, isLower) {
    ///<summary>转换datatable的json对象中的属性名称的大小写形式</summary>
    ///<param name="dt" type="Array">datatable json化后的[]数组</param>
    ///<param name="isLower" type="Boolean">是否转换成小写模式，默认转换成大写</param>
    if (!dt || !IsArray(dt)) {
        return dt;
    }

    if (dt.length == 0 || IsObject(dt[0]) == false) {
        return dt;
    }

    var newArr = [];
    var obj;

    for (var i = 0; i < dt.length; i++) {
        obj = {};

        for (var field in dt[i]) {
            obj[isLower ? field.toLowerCase() : field.toUpperCase()] = dt[i][field];
        }

        newArr.push(obj);
    }

    return newArr;
}

//通用的aj访问与处理工具.
function AjaxServiceGener(param, myUrl, callback, scope) {

    $.ajax({
        type: "GET", //使用GET或POST方法访问后台
        dataType: "html", //返回json格式的数据
        contentType: "application/json; charset=utf-8",
        url: Handler + myUrl, //要访问的后台地址
        data: param, //要发送的数据
        async: true,
        cache: false,
        complete: function () { }, //AJAX请求完成时隐藏loading提示
        error: function (XMLHttpRequest, errorThrown) {
            callback(XMLHttpRequest);
        },
        success: function (data) { //msg为返回的数据，在这里做数据绑定
            callback(data, scope);
        }
    });
}

function IsArray(obj) {
    ///<summary>判断是否是数组</summary>
    ///<param name="obj" type="All Type">要判断的对象</param>
    return Object.prototype.toString.call(obj) == "[object Array]";
}

function IsObject(obj) {
    ///<summary>判断是否是Object对象</summary>
    ///<param name="obj" type="All Type">要判断的对象</param>
    return typeof obj != "undefined" && obj.constructor == Object;
}

function To(url) {
    //window.location.href = url;
    window.name = "dialogPage"; window.open(url, "dialogPage")
}

function WinOpen(url, winName) {
    var newWindow = window.open(url, winName, 'width=700,height=400,top=100,left=300,scrollbars=yes,resizable=yes,toolbar=false,location=false,center=yes,center: yes;');
    newWindow.focus();
    return;
}

function WinOpenFull(url, winName) {
    var newWindow = window.open(url, winName, 'width=' + window.screen.availWidth + ',height=' + window.screen.availHeight + ',scrollbars=yes,resizable=yes,toolbar=false,location=false,center=yes,center: yes;');
    newWindow.focus();
    return;
}

// document绑定esc键的keyup事件, 关闭弹出窗
function closeWhileEscUp() {
    $(document).bind("keyup", function (e) {
        e = e || window.event;
        var key = e.keyCode || e.which || e.charCode;
        if (key == 27) {
            // 可能需要调整if判断的顺序
            if (parent && typeof parent.doCloseDialog === 'function') {
                parent.doCloseDialog.call();
            } else if (typeof doCloseDialog === 'function') {
                doCloseDialog.call();
            } else if (parent && parent.parent && typeof parent.parent.doCloseDialog === "function") {
                parent.parent.doCloseDialog.call();
            } else {
                window.close();
            }
        }
    });
}
function DBAccess() {

    var url = Handler + "?SQL=select * from sss";
    $.ajax({
        type: "GET", //使用GET或POST方法访问后台
        dataType: "json", //返回json格式的数据
        contentType: "application/json; charset=utf-8",
        url: url, //要访问的后台地址
        async: true,
        cache: false,
        complete: function () { }, //AJAX请求完成时隐藏loading提示
        error: function (XMLHttpRequest, errorThrown) {
            callback(XMLHttpRequest);
        },
        success: function (data) { //msg为返回的数据，在这里做数据绑定
            callback(data, scope);
        }
    });

}


/* 关于实体的类
GEEntity_Init
var pkval="Demo_DtlExpImpDtl1";  
var EnName="BP.WF.Template.MapDtlExt";
GEntity en=new GEEntity(EnName,pkval);
var strs=  en.ImpSQLNames;
// var strss=en.GetValByKey('ImpSQLNames');
en.ImpSQLNames=aaa;
en.Updata();
*/

var Entity = (function () {

    var jsonString;

    var Entity = function (enName, pkval) {
        this.enName = enName;

        if (pkval != null && typeof pkval === "object") {
            jsonString = {};
            this.CopyJSON(pkval);
        } else {
            this.pkval = pkval || "";
            this.loadData();
        }
    };

    function setData(self) {
        if (typeof jsonString !== "undefined") {
            $.each(jsonString, function (n, o) {
                // 需要判断属性名与当前对象属性名是否相同
                if (typeof self[n] !== "function") {
                    self[n] = o;
                }
            });
        }
    }

    function getParams(self) {
        var params = {};
        $.each(jsonString, function (n, o) {
            if (typeof self[n] !== "function") {
                params[n] = self[n];
            }
        });
        return params;
    }

    function getParams1(self) {
        var params = ["t=" + new Date().getTime()];
        $.each(jsonString, function (n, o) {
            if (typeof self[n] !== "function" && (self[n] != o || true)) {
                params.push(n + "=" + self[n]);
            }
        });
        return params.join("&");
    }

    if (plant == "CCFlow") {
        // CCFlow
        dynamicHandler = "/WF/Comm/Handler.ashx";
    } else {
        // JFlow
        dynamicHandler = basePath + "/WF/Comm/ProcessRequest.do";
    }

    Entity.prototype = {

        constructor: Entity,

        loadData: function () {
            var self = this;
            $.ajax({
                type: 'post',
                async: false,
                url: dynamicHandler + "?DoType=Entity_Init&EnName=" + self.enName + "&PKVal=" + self.pkval + "&t=" + new Date().getTime(),
                dataType: 'html',
                success: function (data) {
                    if (data.indexOf("err@") != -1) {
                        alert(data);
                        return;
                    }
                    try {
                        jsonString = JSON.parse(data);
                        setData(self);
                    } catch (e) {
                        alert("解析错误: " + data);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Entity_Init 系统发生异常, status: " + XMLHttpRequest.status + " readyState: " + XMLHttpRequest.readyState + " enName=" + self.enName + " pkval=" + self.pkval);
                }
            });
        },

        SetValByKey: function (key, value) {
            this[key] = value;
        },

        GetValByKey: function (key) {
            return this[key];
        },

        Insert: function () {
            var self = this;
            var params = getParams(self);

            if (params.length == 0)
                params = getParams1(self);

            var result = "";
            $.ajax({
                type: 'post',
                async: false,
                url: dynamicHandler + "?DoType=Entity_Insert&EnName=" + self.enName + "&t=" + new Date().getTime(),
                dataType: 'html',
                data: params,
                success: function (data) {

                    result = data;
                    if (data.indexOf("err@") != -1) {
                        alert(data);
                        return 0; //插入失败.
                    }

                    data = JSON.parse(data);
                    result = data;

                    var self = this;
                    $.each(data, function (n, o) {
                        if (typeof self[n] !== "function") {
                            jsonString[n] = o;
                            self[n] = o;
                        }
                    });

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("系统发生异常, status: " + XMLHttpRequest.status + " readyState: " + XMLHttpRequest.readyState);
                }
            });
            return result;
        },

        Update: function () {
            var self = this;
            var params = getParams(self);
            var result;
            $.ajax({
                type: 'post',
                async: false,
                url: dynamicHandler + "?DoType=Entity_Update&EnName=" + self.enName + "&t=" + new Date().getTime(),
                dataType: 'html',
                data: params,
                success: function (data) {
                    result = data;
                    if (data.indexOf("err@") != -1) {
                        var err = data.replace('err@', '');
                        alert('更新异常:' + err + " \t\nEnName" + self.enName);
                        return;
                    }

                    $.each(params, function (n, o) {
                        jsonString[n] = o;
                    });
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Entity Update系统发生异常, status: " + XMLHttpRequest.status + " readyState: " + XMLHttpRequest.readyState);
                }
            });
            return result;
        },

        Save: function () {
            var self = this;
            var params = getParams(self);
            var result;
            $.ajax({
                type: 'post',
                async: false,
                url: dynamicHandler + "?DoType=Entity_Save&EnName=" + self.enName + "&t=" + new Date().getTime(),
                dataType: 'html',
                data: params,
                success: function (data) {
                    result = data;
                    if (data.indexOf("err@") != -1) {
                        alert(data);
                        return;
                    }
                    $.each(params, function (n, o) {
                        jsonString[n] = o;
                    });
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Save 系统发生异常, status: " + XMLHttpRequest.status + " readyState: " + XMLHttpRequest.readyState);
                }
            });
            return result;
        },

        Delete: function () {
            var self = this;
            //var params = getParams(self);
            var params = getParams1(this);

            var result;
            $.ajax({
                type: 'post',
                async: false,
                url: dynamicHandler + "?DoType=Entity_Delete&EnName=" + self.enName + "&PKVal=" + this.GetPKVal() + "&t=" + new Date().getTime(),
                dataType: 'html',
                data: params,
                success: function (data) {
                    result = data;
                    if (data.indexOf("err@") != -1) {
                        alert(data);
                        return;
                    }
                    $.each(jsonString, function (n, o) {
                        jsonString[n] = undefined;
                    });
                    setData(self);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Delete 系统发生异常, status: " + XMLHttpRequest.status + " readyState: " + XMLHttpRequest.readyState);
                }
            });
            return result;
        },

        Retrieve: function () {
            var self = this;
            var params = getParams1(this);
            var result;
            $.ajax({
                type: 'post',
                async: false,
                url: dynamicHandler + "?DoType=Entity_Retrieve&EnName=" + self.enName + "&" + params,
                dataType: 'html',
                success: function (data) {
                    result = data;
                    if (data.indexOf("err@") == 0) {
                        alert('查询失败:' + self.enName + "请联系管理员:\t\n" + data.replace('err@', ''));
                        return;
                    }

                    try {
                        jsonString = JSON.parse(data);
                        setData(self);
                        result = jsonString.Retrieve;

                    } catch (e) {
                        result = "err@解析错误: " + data;
                        alert(result);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    result = "Retrieve[" + self.enName + "]:err@系统发生异常, status: " + XMLHttpRequest.status + " readyState: " + XMLHttpRequest.readyState;
                    alert(jsonString);
                }
            });
            return result;
        },
        SetPKVal: function (pkVal) {
            self.pkval = pkVal;
            this["MyPK"] = pkval;
            this["OID"] = pkval;
            this["WorkID"] = pkval;
            this["NodeID"] = pkval;
            this["No"] = pkval;
        },
        GetPKVal: function () {

            var val = this["MyPK"];
            if (val == undefined || val == "")
                var val = this["OID"];
            if (val == undefined || val == "")
                var val = this["WorkID"];
            if (val == undefined || val == "")
                var val = this["NodeID"];
            if (val == undefined || val == "")
                var val = this["No"];
            if (val == undefined || val == "")
                var val = this.pkval;

            return val;
        },
        RetrieveFromDBSources: function () {
            var self = this;
            // var params = getParams1(this); //查询的时候不需要把参数传入里面去.

            var pkavl = this.GetPKVal();


            //  alert(self.GetPKVal()); 

            var result;
            $.ajax({
                type: 'post',
                async: false,
                url: dynamicHandler + "?DoType=Entity_RetrieveFromDBSources&EnName=" + self.enName + "&PKVal=" + pkavl,
                dataType: 'html',
                success: function (data) {
                    result = data;
                    if (data.indexOf("err@") == 0) {
                        alert(data);
                        //var str = "查询:" + self.enName + " pk=" + self.pkval + " 错误.\t\n" + data.replace('err@', '');
                        //alert('查询:' + str);
                        return;
                    }

                    try {
                        jsonString = JSON.parse(data);
                        setData(self);
                        result = jsonString.RetrieveFromDBSources;

                    } catch (e) {
                        result = "err@解析错误: " + data;
                        alert(result);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {

                    alert(JSON.stringify(XMLHttpRequest));
                    result = "RetrieveFromDBSources err@系统发生异常, status: " + XMLHttpRequest.status + " readyState: " + XMLHttpRequest.readyState;
                    alert(result);
                }
            });
            return result;
        },

        IsExits: function () {
            var self = this;
            var result;
            $.ajax({
                type: 'post',
                async: false,
                url: dynamicHandler + "?DoType=Entity_IsExits&EnName=" + self.enName + "&" + getParams1(self),
                dataType: 'html',
                success: function (data) {

                    if (data.indexOf("err@") != -1) {
                        alert(data);
                        return;
                    }

                    if (data == "1")
                        result = true;
                    else
                        result = false;
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    result = "err@系统发生异常,IsExits, status: " + XMLHttpRequest.status + " readyState: " + XMLHttpRequest.readyState;
                    alert(result);
                }
            });
            return result;
        },

        DoMethodReturnString: function (methodName) {
            var params = [];
            $.each(arguments, function (i, o) {
                if (i > 0)
                    params.push(o);
            });



            var self = this;
            var string;
            $.ajax({
                type: 'post',
                async: false,
                url: dynamicHandler + "?DoType=Entity_DoMethodReturnString&EnName=" + self.enName + "&PKVal=" + self.pkval + "&MethodName=" + methodName + "&paras=" + params.join(",") + "&t=" + new Date().getTime(),
                dataType: 'html',
                success: function (data) {
                    string = data;
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    string = "Entity.DoMethodReturnString err@系统发生异常, status: " + XMLHttpRequest.status + " readyState: " + XMLHttpRequest.readyState;
                    alert(string);
                }
            });

            return string;

        },

        DoMethodReturnJSON: function (methodName, params) {
            var jsonString = this.DoMethodReturnString(methodName, params);

            if (jsonString.indexOf("err@") != -1) {
                alert(jsonString);
                return jsonString;
            }

            try {
                jsonString = JSON.parse(jsonString);
            } catch (e) {
                jsonString = "err@json解析错误: " + jsonString;
                alert(jsonString);
            }
            return jsonString;
        },

        toString: function () {
            return JSON.stringify(this);
        },

        GetPara: function (key) {
            var atPara = this.AtPara;
            if (typeof atPara != "string" || typeof key == "undefined" || key == "") {
                return undefined;
            }
            var reg = new RegExp("(^|@)" + key + "=([^@]*)(@|$)");
            var results = atPara.match(reg);
            if (results != null) {
                return unescape(results[2]);
            }
            return undefined;
        },

        SetPara: function (key, value) {
            var atPara = this.AtPara;
            if (typeof atPara != "string" || typeof key == "undefined" || key == "") {
                return;
            }

            var m = "@" + key + "=";
            var index = atPara.indexOf(m);
            if (index == -1) {
                this.AtPara += "@" + key + "=" + value;
                return;
            }

            var p = atPara.substring(0, index + m.length);
            var s = atPara.substring(index + m.length, atPara.length);
            var i = s.indexOf("@");
            if (i == -1) {
                this.AtPara = p + value;
            } else {
                this.AtPara = p + value + s.substring(i, s.length);
            }

        },

        CopyURL: function () {
            var self = this;
            $.each(self, function (n, o) {
                if (typeof o !== "function") {
                    var value = GetQueryString(n);
                    if (value != null && typeof value !== "undefined" && $.trim(value) != "") {
                        self[n] = value;
                        jsonString[n] = value;
                    }
                }
            });
        },

        CopyForm: function () {

            $("input,select").each(function (i, e) {
                if (typeof $(e).attr("name") === "undefined" || $(e).attr("name") == "") {
                    $(e).attr("name", $(e).attr("id"));
                }
            });

            // 新版本20180107 2130
            var self = this;
            // 普通属性
            $("[name^=TB_],[name^=CB_],[name^=RB_],[name^=DDL_]").each(function (i, o) {
                var target = $(this);
                var name = target.attr("name");
                var key = name.replace(/^TB_|CB_|RB_|DDL_/, "");
                if (typeof self[key] === "function") {
                    return true;
                }
                if (name.match(/^TB_/)) {
                    self[key] = target.val();
                } else if (name.match(/^DDL_/)) {
                    self[key] = target.val();
                } else if (name.match(/^CB_/)) {
                    if (target.length == 1) {	// 仅一个复选框
                        if (target.is(":checked")) {
                            // 已选
                            self[key] = "1";
                        } else {
                            // 未选
                            self[key] = "0";
                        }
                    } else if (target.length > 1) {	// 多个复选框(待扩展)
                        // ?
                    }
                } else if (name.match(/^RB_/)) {
                    if (target.is(":checked")) {
                        // 已选
                        self[key] = "1";
                    } else {
                        // 未选
                        self[key] = "0";
                    }
                }
            });
            // 参数属性
            $("[name^=TBPara_],[name^=CBPara_],[name^=RBPara_],[name^=DDLPara_]").each(function (i, o) {
                var target = $(this);
                var name = target.attr("name");
                var value;
                if (name.match(/^TBPara_/)) {
                    value = target.val();
                    value = value.replace('@', ''); //替换掉@符号.
                } else if (name.match(/^DDLPara_/)) {
                    value = target.val();
                    value = value.replace('@', ''); //替换掉@符号.
                } else if (name.match(/^CBPara_/)) {
                    if (target.length == 1) {	// 仅一个复选框
                        if (target.is(":checked")) {
                            // 已选
                            value = "1";
                        } else {
                            // 未选
                            value = "0";
                        }
                    } else if (target.length > 1) {	// 多个复选框(待扩展)
                        // ?
                    }
                } else if (name.match(/^RBPara_/)) {
                    if (target.is(":checked")) {
                        // 已选
                        value = "1";
                    } else {
                        // 未选
                        value = "0";
                    }
                }
                var key = name.replace(/^TBPara_|CBPara_|RBPara_|DDLPara_/, "");
                self.SetPara(key, value);
            });
        },

        CopyJSON: function (json) {
            var count = 0;
            if (json) {
                var self = this;
                $.each(json, function (n, o) {
                    if (typeof self[n] !== "function") {
                        self[n] = o;
                        jsonString[n] = o;
                        count++;
                    }
                });
            }
            return count;
        },

        ToJsonWithParas: function () {
            var json = {};
            $.each(this, function (n, o) {
                if (typeof o !== "undefined") {
                    json[n] = o;
                }
            });
            if (typeof this.AtPara == "string") {
                $.each(this.AtPara.split("@"), function (i, o) {
                    if (o == "") {
                        return true;
                    }
                    var kv = o.split("=");
                    if (kv.length == 2) {
                        json[kv[0]] = kv[1];
                    }
                });
            }
            return json;
        }

    };

    return Entity;

})();

var Entities = (function () {

    var jsonString;

    var Entities = function () {
        this.ensName = arguments[0];
        this.Paras = getParameters(arguments);
        if (arguments.length >= 3) {
            this.loadData();
        }
    };

    function getParameters(args) {
        var params = "";
        var length;
        var orderBy;
        if (args.length % 2 == 0) {
            orderBy = args[args.length - 1];
            length = args.length - 1;
        } else {
            length = args.length;
        }
        for (var i = 1; i < length; i += 2) {
            params += "@" + args[i] + "=" + args[i + 1];
        }
        if (typeof orderBy !== "undefined") {
            params += "@OrderBy=" + orderBy;
        }
        return params;
    }

    Entities.prototype = {

        constructor: Entities,

        loadData: function () {
            var self = this;

            if (self.ensName == null || self.ensName == "" || self.ensName == "") {
                alert("在初始化实体期间EnsName没有赋值");
                return;
            }

            $.ajax({
                type: 'post',
                async: false,
                url: dynamicHandler + "?DoType=Entities_Init&EnsName=" + self.ensName + "&Paras=" + self.Paras + "&t=" + new Date().getTime(),
                dataType: 'html',
                success: function (data) {
                    if (data.indexOf("err@") != -1) {
                        alert(data);
                        return;
                    }
                    try {
                        jsonString = JSON.parse(data);
                        if ($.isArray(jsonString)) {
                            self.length = jsonString.length;
                            $.extend(self, jsonString);
                        } else {
                            alert("解析失败, 返回值不是集合");
                        }
                    } catch (e) {
                        alert("json解析错误: " + data);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Entities_Init err@系统发生异常, status: " + XMLHttpRequest.status + " readyState: " + XMLHttpRequest.readyState);
                }
            });
        },
        deleteIt: function () {

            var self = this;
            if (self.ensName == null || self.ensName == "" || self.ensName == "") {
                alert("在初始化实体期间EnsName没有赋值");
                return;
            }

            $.ajax({
                type: 'post',
                async: false,
                url: dynamicHandler + "?DoType=Entities_Delete&EnsName=" + self.ensName + "&Paras=" + self.Paras + "&t=" + new Date().getTime(),
                dataType: 'html',
                success: function (data) {
                    if (data.indexOf("err@") != -1) {
                        alert(data);
                        return;
                    }

                    //                    try {
                    //                        jsonString = JSON.parse(data);
                    //                        if ($.isArray(jsonString)) {
                    //                            self.length = jsonString.length;
                    //                            $.extend(self, jsonString);
                    //                        } else {
                    //                            alert("解析失败, 返回值不是集合");
                    //                        }
                    //                    } catch (e) {
                    //                        alert("json解析错误: " + data);
                    //                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Entities_Delte err@系统发生异常, status: " + XMLHttpRequest.status + " readyState: " + XMLHttpRequest.readyState);
                }
            });
        },

        Retrieve: function () {
            var args = [""];
            $.each(arguments, function (i, o) {
                args.push(o);
            });
            this.Paras = getParameters(args);
            this.loadData();
        },
        Delete: function () {
            var args = [""];
            $.each(arguments, function (i, o) {
                args.push(o);
            });
            this.Paras = getParameters(args);
            this.deleteIt();
        },
        DoMethodReturnString: function (methodName) {
            var params = [];
            $.each(arguments, function (i, o) {
                if (i > 0)
                    params.push(o);
            });

            var self = this;
            var string;
            $.ajax({
                type: 'post',
                async: false,
                url: dynamicHandler + "?DoType=Entities_DoMethodReturnString&EnsName=" + self.ensName + "&MethodName=" + methodName + "&paras=" + params.join(",") + "&t=" + new Date().getTime(),
                dataType: 'html',
                success: function (data) {
                    string = data;
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    string = "Entities_DoMethodReturnString err@系统发生异常, status: " + XMLHttpRequest.status + " readyState: " + XMLHttpRequest.readyState;
                    alert(string);
                }
            });

            return string;

        },

        DoMethodReturnJSON: function (methodName, params) {
            var jsonString = this.DoMethodReturnString(methodName, params);

            if (jsonString.indexOf("err@") != -1) {
                alert(jsonString);
                return jsonString;
            }

            try {
                jsonString = JSON.parse(jsonString);
            } catch (e) {
                jsonString = "err@json解析错误: " + jsonString;
                alert(jsonString);
            }
            return jsonString;
        },
        RetrieveAll: function () {
            var pathRe = "";
            if (plant == "JFlow" && (basePath == null || basePath == '')) {
                var rowUrl = window.document.location.href;
                pathRe = rowUrl.substring(0, rowUrl.indexOf('/SDKFlowDemo') + 1);
            }
            var self = this;
            $.ajax({
                type: 'post',
                async: false,
                url: pathRe + dynamicHandler + "?DoType=Entities_RetrieveAll&EnsName=" + self.ensName + "&t=" + new Date().getTime(),
                dataType: 'html',
                success: function (data) {
                    if (data.indexOf("err@") != -1) {
                        alert(data);
                        return;
                    }
                    try {
                        jsonString = JSON.parse(data);
                        if ($.isArray(jsonString)) {
                            self.length = jsonString.length;
                            $.extend(self, jsonString);
                        } else {
                            alert("解析失败, 返回值不是集合");
                        }
                    } catch (e) {
                        alert("json解析错误: " + data);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("RetrieveAll err@系统发生异常, status: " + XMLHttpRequest.status + " readyState: " + XMLHttpRequest.readyState);
                }
            });
        }

    };

    return Entities;

})();

var DBAccess = (function () {

    function DBAccess() {
    }

    if (plant == "CCFlow") {
        // CCFlow
        dynamicHandler = "/WF/Comm/Handler.ashx";
    } else {
        // JFlow
        dynamicHandler = basePath + "/WF/Comm/ProcessRequest.do";
    }

    DBAccess.RunSQL = function (sql) {

        var count = 0;

        $.ajax({
            type: 'post',
            async: false,
            url: dynamicHandler + "?DoType=DBAccess_RunSQL&t=" + new Date().getTime(),
            dataType: 'html',
            data: "SQL=" + sql,
            success: function (data) {
                count = parseInt(data);
                if (isNaN(count)) {
                    count = -1;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("RunSQL 系统发生异常, status: " + XMLHttpRequest.status + " readyState: " + XMLHttpRequest.readyState);
            }
        });

        return count;

    };
    //执行数据源返回json.
    DBAccess.RunDBSrc = function (dbSrc, dbType) {

        if (dbSrc == "" || dbSrc == null || dbSrc == undefined) {
            alert("数据源为空..");
            return;
        }

        if (dbType == undefined) {
            dbType = 0; //默认为sql.
            if (dbSrc.length <= 20) {
                dbType = 2; //可能是一个方法名称.
            }
            if (dbSrc.indexOf('/') != -1) {
                dbType = 1; //是一个url.
            }
        }

        if (dbSrc.indexOf('@') != -1) {
            //val = val.replace(/~/g, "'"); //替换掉特殊字符,设置的sql语句的引号.
            var alt = "如果关键字有多个，可以使用.  /myword/g 作为通配符替换。  ";
            alert("数据源参数没有替换" + dbSrc + " \t\n" + alt);
            return;
        }


        //执行的SQL
        if (dbType == 0) {
            return DBAccess.RunSQLReturnTable(dbSrc);
        }

        //执行URL
        if (dbType == 1 || dbType == "1") {
            return DBAccess.RunUrlReturnJSON(dbSrc);
        }

        //执行方法名称返回json.
        if (dbType == 2 || dbType == "2") {

            var str = DBAccess.RunFunctionReturnStr(dbSrc);
            if (str == null || str == undefined)
                return null;

            return JSON.prease(str);
        }
        //@谢 如何执行一个方法,
        //   alert("@没有处理执行方法。"); RunFunctionReturnJSON
    };

    //执行方法名返回str.
    DBAccess.RunFunctionReturnStr = function (funcName) {

        try {

            if (funcName.indexOf('(') == -1)
                return eval(funcName + "()");
            else
                return eval(funcName);

        } catch (e) {
            if (e.Message.indexOf('not defined') != -1)
                alert("执行方法[" + funcName + "]错误:" + e.message);
        }
    };

    DBAccess.RunSQLReturnTable = function (sql) {
        //sql = replaceAll(sql, "~", "'");

        sql = sql.replace(/~/g, "'");
        sql = sql.replace(/%/g, "-");
        var jsonString;

        $.ajax({
            type: 'post',
            async: false,
            url: dynamicHandler + "?DoType=DBAccess_RunSQLReturnTable" + "&t=" + new Date().getTime(),
            dataType: 'html',
            data: "SQL=" + sql,
            success: function (data) {
                if (data.indexOf("err@") != -1) {
                    alert(data);
                    return;
                }
                try {
                    jsonString = JSON.parse(data);
                } catch (e) {
                    alert("json解析错误: " + data);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("系统发生异常, status: " + XMLHttpRequest.status + " readyState: " + XMLHttpRequest.readyState);
            }
        });
        return jsonString;
    };

    DBAccess.RunUrlReturnString = function (url) {

        if (url == null || typeof url === "undefined") {
            alert("err@url无效");
            return;
        }

        if (url.match(/^http:\/\//)) {
            url = dynamicHandler + "?DoType=RunUrlCrossReturnString&t=" + new Date().getTime() + "&url=" + url
        }

        var string;

        $.ajax({
            type: 'post',
            async: false,
            url: url,
            dataType: 'html',
            success: function (data) {
                if (data.indexOf("err@") != -1) {
                    alert(data);
                    return;
                }
                string = data;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                string = "err系统发生异常, RunUrlReturnString status: " + XMLHttpRequest.status + " readyState: " + XMLHttpRequest.readyState + " \t\nurl:" + url;
                alert(string);
            }
        });

        return string;
    };

    DBAccess.RunUrlReturnJSON = function (url) {

        var jsonString = DBAccess.RunUrlReturnString(url);
        if (typeof jsonString === "undefined") {
            alert("执行错误:\t\n URL:" + url);
            return;
        }

        if (jsonString.indexOf("err@") != -1) {
            alert(jsonString + "\t\n URL:" + url);
            return jsonString;
        }

        try {

            jsonString = JSON.parse(jsonString);

        } catch (e) {
            jsonString = "err@json,RunUrlReturnJSON解析错误:" + jsonString;
            alert(jsonString);
        }
        return jsonString;
    };

    return DBAccess;

})();

var HttpHandler = (function () {

    var parameters = {};

    var formData;

    function HttpHandler(handlerName) {
        this.handlerName = handlerName;
    }

    function validate(s) {
        if (s == null || typeof s === "undefined") {
            return false;
        }
        s = s.replace(/^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g, "");
        if (s == "" || s == "null" || s == "undefined") {
            return false;
        }
        return true;
    }

    if (plant == "CCFlow") {
        // CCFlow
        dynamicHandler = "/WF/Comm/Handler.ashx";
    } else {
        // JFlow
        dynamicHandler = basePath + "/WF/Comm/ProcessRequest.do";
    }

    HttpHandler.prototype = {

        constructor: HttpHandler,

        AddUrlData: function () {
            var queryString = document.location.search.substr(1);
            if (queryString.length > 0) {
                var self = this;
                $.each(queryString.split("&"), function (i, o) {
                    var param = o.split("=");
                    if (param.length == 2 && validate(param[1])) {
                        (function (key, value) {
                            self.AddPara(key, value);
                        })(param[0], param[1]);
                    }
                });
            }
        },

        AddFormData: function () {
            formData = $("form").serialize();
        },

        AddPara: function (key, value) {
            parameters[key] = value;
        },

        Clear: function () {
            parameters = {};
            formData = undefined;
        },

        getParams: function () {
            var params = [];
            $.each(parameters, function (key, value) {
                params.push(key + "=" + value);
            });
            return params.join("&");
        },

        DoMethodReturnString: function (methodName) {

            var self = this;
            var jsonString;

           // alert(self.getParams());

            $.ajax({
                type: 'post',
                async: false,
                url: dynamicHandler + "?DoType=HttpHandler&DoMethod=" + methodName + "&HttpHandlerName=" + self.handlerName + "&" + self.getParams() + "&t=" + new Date().getTime(),
                data: formData,
                dataType: 'html',
                success: function (data) {
                    jsonString = data;
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    jsonString = " Handler DoMethodReturnString err@系统发生异常, status: " + XMLHttpRequest.status + " readyState: " + XMLHttpRequest.readyState;
                    alert(jsonString);
                }
            });

            return jsonString;

        },

        DoMethodReturnJSON: function (methodName) {

            var jsonString = this.DoMethodReturnString(methodName);

            if (jsonString.indexOf("err@") == 0) {
                alert('请查看控制台:' + jsonString);
                console.log(jsonString);
                return jsonString;
            }

            try {
                jsonString = JSON.parse(jsonString);
            } catch (e) {
                jsonString = "err@json解析错误: " + jsonString;
                alert(jsonString);
                console.log(jsonString);
            }
            return jsonString;
        }
    }
    return HttpHandler;

})();

var WebUser = function () {

    if (plant == "CCFlow") {
        // CCFlow
        dynamicHandler = "/WF/Comm/Handler.ashx";
    } else {
        // JFlow
        dynamicHandler = basePath + "/WF/Comm/ProcessRequest.do";
    }

    var jsonString = {};

    $.ajax({
        type: 'post',
        async: false,
        url: dynamicHandler + "?DoType=WebUser_Init&t=" + new Date().getTime(),
        dataType: 'html',
        success: function (data) {

            if (data.indexOf("err@") != -1) {
                if (data.indexOf('登陆信息丢失') != -1) {
                    alert("登录信息丢失，请重新登录。");
                } else {
                    alert(data);
                }
                return;
            }

            try {
                jsonString = JSON.parse(data);
            } catch (e) {
                alert("json解析错误: " + data);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("WebUser_Init err@系统发生异常, status: " + XMLHttpRequest.status + " readyState: " + XMLHttpRequest.readyState);
        }
    });

    var self = this;
    $.each(jsonString, function (n, o) {
        self[n] = o;
    });
};

//替换全部.
function replaceAll(str, oldKey, newKey) {

    if (str == undefined || str == null) {
        alert("要替换的原始字符串为空.");
        return str;
    }
    str = str.replace(new RegExp(oldKey, "gm"), newKey);
    return str;
}


//date = new Date();
//date = FormatDate(date, "yyyy-MM-dd");
function FormatDate(now, mask) {
    var d = now;
    var zeroize = function (value, length) {
        if (!length) length = 2;
        value = String(value);
        for (var i = 0, zeros = ''; i < (length - value.length); i++) {
            zeros += '0';
        }
        return zeros + value;
    }

    return mask.replace(/"[^"]*"|'[^']*'|\b(?:d{1,4}|m{1,4}|yy(?:yy)?|([hHMstT])\1?|[lLZ])\b/g, function ($0) {
        switch ($0) {
            case 'd': return d.getDate();
            case 'dd': return zeroize(d.getDate());
            case 'ddd': return ['Sun', 'Mon', 'Tue', 'Wed', 'Thr', 'Fri', 'Sat'][d.getDay()];
            case 'dddd': return ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'][d.getDay()];
            case 'M': return d.getMonth() + 1;
            case 'MM': return zeroize(d.getMonth() + 1);
            case 'MMM': return ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'][d.getMonth()];
            case 'MMMM': return ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'][d.getMonth()];
            case 'yy': return String(d.getFullYear()).substr(2);
            case 'yyyy': return d.getFullYear();
            case 'h': return d.getHours() % 12 || 12;
            case 'hh': return zeroize(d.getHours() % 12 || 12);
            case 'H': return d.getHours();
            case 'HH': return zeroize(d.getHours());
            case 'm': return d.getMinutes();
            case 'mm': return zeroize(d.getMinutes());
            case 's': return d.getSeconds();
            case 'ss': return zeroize(d.getSeconds());
            case 'l': return zeroize(d.getMilliseconds(), 3);
            case 'L': var m = d.getMilliseconds();
                if (m > 99) m = Math.round(m / 10);
                return zeroize(m);
            case 'tt': return d.getHours() < 12 ? 'am' : 'pm';
            case 'TT': return d.getHours() < 12 ? 'AM' : 'PM';
            case 'Z': return d.toUTCString().match(/[A-Z]+$/);
                // Return quoted strings with the surrounding quotes removed
            default: return $0.substr(1, $0.length - 2);
        }
    });
}