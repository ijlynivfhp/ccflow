﻿if ($.fn.datagrid) {
    $.fn.datagrid.defaults.loadMsg = '正在处理，请稍待。。。';
}
if ($.fn.treegrid && $.fn.datagrid) {
    $.fn.treegrid.defaults.loadMsg = $.fn.datagrid.defaults.loadMsg;
}
if ($.messager) {
    $.messager.defaults.ok = '确定';
    $.messager.defaults.cancel = '取消';
}
function getArgsFromHref(sArgName) {
    var sHref = window.location.href;
    var args = sHref.split("?");
    var retval = "";
    if (args[0] == sHref) /*参数为空*/
    {
        return retval; /*无需做任何处理*/
    }

    var str = args[1];
    args = str.split("&");
    for (var i = 0; i < args.length; i++) {
        str = args[i];
        var arg = str.split("=");
        if (arg.length <= 1) continue;
        if (arg[0] == sArgName) retval = arg[1];
    }
    while (retval.indexOf('#') >= 0) {
        retval = retval.replace('#', '');
    }
    return retval;
}
var ensName = '';
//加载表格数据
function LoadGridData(pageNumber, pageSize) {
    ensName = getArgsFromHref("EnsName");
    //实体名
    if (ensName == '') {
        $("body").html("<b style='color:red;'>请传入正确的参数名。如：SearchEasyUI.aspx?EnsName=BP.GPM.Depts<b>");
        return;
    }

    var params = {
        method: "getensgriddata",
        EnsName: ensName,
        pageNumber: pageNumber,
        pageSize: pageSize
    };
    queryData(params, function (js, scope) {
        $("#pageloading").hide();
        if (js) {
            if (js == "") js = "[]";

            //系统错误
            if (js.status && js.status == 500) {
                $("body").html("<b style='color:red;'>请传入正确的参数名。如：SearchEasyUI.aspx?EnsName=BP.GPM.Depts<b>");
                return;
            }

            var pushData = eval('(' + js + ')');
            var fitColumns = true;
            if (pushData.columns.length > 7) {
                fitColumns = false;
            }
            $('#ensGrid').datagrid({
                columns: [pushData.columns],
                data: pushData.data,
                width: 'auto',
                height: 'auto',
                striped: true,
                rownumbers: true,
                singleSelect: true,
                pagination: true,
                remoteSort: false,
                fitColumns: fitColumns,
                pageNumber: pageNumber,
                pageSize: pageSize,
                pageList: [20, 30, 40, 50],
                onDblClickCell: function (index, field, value) {
                    EditEntityForm();
                },
                loadMsg: '数据加载中......'
            });

            var pg = $("#ensGrid").datagrid("getPager");
            if (pg) {
                $(pg).pagination({
                    onRefresh: function (pageNumber, pageSize) {
                        LoadGridData(pageNumber, pageSize);
                    },
                    onSelectPage: function (pageNumber, pageSize) {
                        LoadGridData(pageNumber, pageSize);
                    }
                });
            }

        }
    }, this);
}
//查看详细信息
function CheckEntityForm() {
    EditEntityForm();
}
//查看页面
function EditEntityForm() {
    var enName = $("#enName").val();
    var PK = $("#enPK").val();
    if (enName == "") {
        $.messager.alert('提示', '没有找到类名！', 'info');
        return;
    }
    var url = "UIEn.aspx?EnName=" + enName;
    var row = $('#ensGrid').datagrid('getSelected');
    if (row) {
        url = "UIEn.aspx?EnName=" + enName + "&PK=" + row[PK];
        var winWidth = document.body.clientWidth;
        //计算显示宽度
        winWidth = winWidth * 0.9;
        if (winWidth > 820) winWidth = 820;

        var winheight = document.body.clientHeight;
        //计算显示高度
        winheight = winheight * 0.98
        if (winheight > 780) winheight = 780;

        $("<div id='dialogEnPanel'></div>").append($("<iframe width='100%' height='100%' frameborder=0 src='" + url + "'/>")).dialog({
            title: "窗口",
            width: winWidth,
            height: winheight,
            autoOpen: true,
            modal: true,
            resizable: true,
            onClose: function () {
                $("#dialogEnPanel").remove();
                var pg = $('#ensGrid').datagrid('getPager');
                var curPage = $(pg).pagination.pageNumber;
                LoadGridData(curPage, 20);
            },
            buttons: [{
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#dialogEnPanel').dialog("close");
                }
            }]
        });
    } else {
        $.messager.alert('提示', '请选择记录后再进行查看！', 'info');
    }
}
function DDL_mvals_OnChange(ctrl, ensName, attrKey) {

    var idx_Old = ctrl.selectedIndex;

    if (ctrl.options[ctrl.selectedIndex].value != 'mvals')
        return;
    if (attrKey == null)
        return;

    var url = 'SelectMVals.aspx?EnsName=' + ensName + '&AttrKey=' + attrKey;
    var val = window.showModalDialog(url, 'dg', 'dialogHeight: 450px; dialogWidth: 450px; center: yes; help: no');
    if (val == '' || val == null) {
        ctrl.selectedIndex = 0;
    }
}
//公共方法
function queryData(param, callback, scope, method, showErrMsg) {
    if (!method) method = 'GET';
    $.ajax({
        type: method, //使用GET或POST方法访问后台
        dataType: "text", //返回json格式的数据
        contentType: "application/json; charset=utf-8",
        url: "Search.htm", //要访问的后台地址
        data: param, //要发送的数据
        async: false,
        cache: false,
        complete: function () { }, //AJAX请求完成时隐藏loading提示
        error: function (XMLHttpRequest, errorThrown) {
            callback(XMLHttpRequest);
        },
        success: function (msg) {//msg为返回的数据，在这里做数据绑定
            var data = msg;
            callback(data, scope);
        }
    });
}
//3秒后加载
setTimeout("LoadGridData(1,20)", 500);