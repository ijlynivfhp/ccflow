$(function () {
    SetHegiht();
    //打开表单检查正则表达式
    if (typeof FormOnLoadCheckIsNull != 'undefined' && FormOnLoadCheckIsNull instanceof Function) {
        FormOnLoadCheckIsNull();
    }

});

//停止流程.
function DoStop(msg, flowNo, workid) {

    if (confirm('您确定要执行 [' + msg + '] ?') == false)
        return;

    var para = 'DoType=MyFlow_StopFlow&FK_Flow=' + flowNo + '&WorkID=' + workid;

    AjaxService(para, function (msg, scope) {

        alert(msg);
        if (msg.indexOf('err@') == 0) {
            return;
        } else {
            window.close();
        }
    });
}


//. 保存嵌入式表单. add 2015-01-22 for GaoLing.
function SaveSelfFrom() {

    // 不支持火狐浏览器。
    var frm = document.getElementById('SelfForm');
    if (frm == null) {
        alert('系统错误,没有找到SelfForm的ID.');
    }
    //执行保存.
    return frm.contentWindow.Save();
}

function SendSelfFrom() {
    if (SaveSelfFrom() == false) {
        alert('表单保存失败，不能发送。');
        return false;
    }
    return true;
}

function SetHegiht() {

    var screenHeight = document.documentElement.clientHeight;

    var messageHeight = $('#Message').height();
    var topBarHeight = 40;
    var childHeight = $('#childThread').height();
    var infoHeight = $('#flowInfo').height();

    var allHeight = messageHeight + topBarHeight + childHeight + childHeight + infoHeight;
    try {

        var BtnWord = $("#BtnWord").val();
        if (BtnWord == 2)
            allHeight = allHeight + 30;

        var frmHeight = $("#FrmHeight").val();
        if (frmHeight == NaN || frmHeight == "" || frmHeight == null)
            frmHeight = 0;

        if (screenHeight > parseFloat(frmHeight) + allHeight) {
            // $("#divCCForm").height(screenHeight - allHeight);

            $("#TDWorkPlace").height(screenHeight - allHeight - 10);

        }
        else {
            //$("#divCCForm").height(parseFloat(frmHeight) + allHeight);
            $("#TDWorkPlace").height(parseFloat(frmHeight) + allHeight - 10);
        }
    }
    catch (e) {
    }
}

$(window).resize(function () {
    //SetHegiht();
});
function SysCheckFrm() {
}
function Change() {
    var btn = document.getElementById('ContentPlaceHolder1_MyFlowUC1_MyFlow1_ToolBar1_Btn_Save');
    if (btn != null) {
        if (btn.value.valueOf('*') == -1)
            btn.value = btn.value + '*';
    }
}
var longCtlID = '';
function KindEditerSync() {
    try {
        if (editor1 != null) {
            editor1.sync();
        }
    }
    catch (err) {
    }
}
 

//20160106 by 柳辉
//获取页面参数
//sArgName表示要获取哪个参数的值
function GetPageParas(sArgName) {

    var sHref = window.location.href;
    var args = sHref.split("?");
    var retval = "";
    if (args[0] == sHref) /*参数为空*/{
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
    return retval;
}

//执行分支流程退回到分合流节点。
function DoSubFlowReturn(fid, workid, fk_node) {
    var url = 'ReturnWorkSubFlowToFHL.htm?FID=' + fid + '&WorkID=' + workid + '&FK_Node=' + fk_node;
    var v = WinShowModalDialog(url, 'df');
    window.location.href = window.history.url;
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

function DoDelSubFlow(fk_flow, workid) {
    if (window.confirm('您确定要终止进程吗？') == false)
        return;

    var para = 'DoType=DelSubFlow&FK_Flow=' + fk_flow + '&WorkID=' + workid;

    AjaxService(para, function (msg, scope) {
        alert(msg);
        window.location.href = window.location.href;
    });
}

//公共方法
function AjaxService(param, callback, scope, levPath) {
    $.ajax({
        type: "GET", //使用GET或POST方法访问后台
        dataType: "text", //返回json格式的数据
        contentType: "application/json; charset=utf-8",
        url: MyFlow, //要访问的后台地址
        data: param, //要发送的数据
        async: true,
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

function Do(warning, url) {
    if (window.confirm(warning) == false)
        return;
    window.location.href = url;
}
//设置底部工具栏
function SetBottomTooBar() {
    var form;
    //窗口的可视高度 
    var windowHeight = document.all ? document.getElementsByTagName("html")[0].offsetHeight : window.innerHeight;
    var pageHeight = Math.max(windowHeight, document.getElementsByTagName("body")[0].scrollHeight);
    form = document.getElementById('divCCForm');

    //        if (form) {
    //            if (pageHeight > 20) pageHeight = pageHeight - 20;
    //            form.style.height = pageHeight + "px";
    //        }
    //设置toolbar
    var toolBar = document.getElementById("bottomToolBar");
    if (toolBar) {
        document.getElementById("bottomToolBar").style.display = "";
    }
}

window.onload = function () {
    //  ResizeWindow();
    SetBottomTooBar();
};

//然浏览器最大化.
function ResizeWindow() {
    if (window.screen) {  //判断浏览器是否支持window.screen判断浏览器是否支持screen     
        var myw = screen.availWidth;   //定义一个myw，接受到当前全屏的宽     
        var myh = screen.availHeight;  //定义一个myw，接受到当前全屏的高     
        window.moveTo(0, 0);           //把window放在左上角     
        window.resizeTo(myw, myh);     //把当前窗体的长宽跳转为myw和myh     
    }
}

function OpenCC() {
    var url = $("#CC_Url").val();
    var v = window.showModalDialog(url, 'cc', 'scrollbars=yes;resizable=yes;center=yes;minimize:yes;maximize:yes;dialogHeight: 650px; dialogWidth: 850px; dialogTop: 100px; dialogLeft: 150px;');
    if (v == '1')
        return true;
    return false;
}

var LODOP; //声明为全局变量 

function printFrom() {
    var url = $("#PrintFrom_Url").val();
    LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
    LODOP.PRINT_INIT("打印表单");

    // LODOP.ADD_PRINT_URL(30, 20, 746, "100%", location.href);

    //LODOP.ADD_PRINT_HTM(20, 0, "100%", "100%", document.getElementById("divCCForm").innerHTML);
    LODOP.ADD_PRINT_URL(0, 0, "100%", "100%", url);

    LODOP.SET_PRINT_STYLEA(0, "HOrient", 3);
    LODOP.SET_PRINT_STYLEA(0, "VOrient", 3);
    //LODOP.SET_PRINT_PAGESIZE(0, 0, 0, "");
    LODOP.SET_PRINT_PAGESIZE(2, 2400, 2970, "A4");
    //		LODOP.SET_SHOW_MODE("MESSAGE_GETING_URL",""); //该语句隐藏进度条或修改提示信息
    //		LODOP.SET_SHOW_MODE("MESSAGE_PARSING_URL","");//该语句隐藏进度条或修改提示信息
    //  LODOP.PREVIEW();

    LODOP.PREVIEW();
}


//原有的
function OpenOfiice(fk_ath, pkVal, delPKVal, FK_MapData, NoOfObj, FK_Node) {
    var date = new Date();
    var t = date.getFullYear() + "" + date.getMonth() + "" + date.getDay() + "" + date.getHours() + "" + date.getMinutes() + "" + date.getSeconds();

    var url = 'WebOffice/AttachOffice.htm?DoType=EditOffice&DelPKVal=' + delPKVal + '&FK_FrmAttachment=' + fk_ath + '&PKVal=' + pkVal + "&FK_MapData=" + FK_MapData + "&NoOfObj=" + NoOfObj + "&FK_Node=" + FK_Node + "&T=" + t;
    //var url = 'WebOffice.aspx?DoType=EditOffice&DelPKVal=' + delPKVal + '&FK_FrmAttachment=' + fk_ath + '&PKVal=' + pkVal;
    // var str = window.showModalDialog(url, '', 'dialogHeight: 1250px; dialogWidth:900px; dialogTop: 100px; dialogLeft: 100px; center: no; help: no;resizable:yes');
    //var str = window.open(url, '', 'dialogHeight: 1200px; dialogWidth:1110px; dialogTop: 100px; dialogLeft: 100px; center: no; help: no;resizable:yes');
    window.open(url, '_blank', 'height=600,width=850,top=50,left=50,toolbar=no,menubar=no,scrollbars=yes, resizable=yes,location=no, status=no');
}

//关注 按钮.
function FocusBtn(btn, workid) {

    if (btn.value == '关注') {
        btn.value = '取消关注';
    }
    else {
        btn.value = '关注';
    }

    var para = "DoType=Focus&WorkID=" + workid;
    AjaxService(para, function (msg, scope) {
        // alert(msg);
    });
}

//确认 按钮.
function ConfirmBtn(btn, workid) {

    if (btn.value == '确认') {
        btn.value = '取消确认';
    }
    else {
        btn.value = '确认';
    }

    var para = "DoType=Confirm&WorkID=" + workid;
    AjaxService(para, function (msg, scope) {
        //  alert(msg);
    });
}


//以下是软通写的
//初始化网页URL参数
function initPageParam() {
    //新建独有
    pageData.UserNo = GetQueryString("UserNo");
    pageData.DoWhat = GetQueryString("DoWhat");
    pageData.IsMobile = GetQueryString("IsMobile");

    pageData.FK_Flow = GetQueryString("FK_Flow");
    pageData.FK_Node = GetQueryString("FK_Node");
    //FK_Flow=004&FK_Node=402&FID=0&WorkID=232&IsRead=0&T=20160920223812&Paras=
    pageData.FID = GetQueryString("FID") == null ? 0 : GetQueryString("FID");
    pageData.WorkID = GetQueryString("WorkID");
    pageData.IsRead = GetQueryString("IsRead");
    pageData.T = GetQueryString("T");
    pageData.Paras = GetQueryString("Paras");
    pageData.IsReadOnly = GetQueryString("IsReadOnly"); //如果是IsReadOnly，就表示是查看页面，不是处理页面
    pageData.IsStartFlow = GetQueryString("IsStartFlow"); //是否是启动流程页面 即发起流程

    pageData.DoType1 = GetQueryString("DoType")//View
    //$('#navIframe').attr('src', 'Admin/CCBPMDesigner/truck/centerTrakNav.html?FK_Flow=' + pageData.FK_Flow + "&FID=" + pageData.FID + "&WorkID=" + pageData.WorkID);
}

//将获取过来的URL参数转成URL中的参数形式  &
function pageParamToUrl() {
    var paramUrlStr = '';
    for (var param in pageData) {
        paramUrlStr += '&' + (param.indexOf('@') == 0 ? param.substring(1) : param) + '=' + pageData[param];
    }
    return paramUrlStr;
}
//初始化按钮
//var MyFlow = "MyFlow.ashx";
function initBar() {

    // 为啥要注释 else MyFlow = "MyFlow.do";
    if (plant == "CCFlow")
        MyFlow = "MyFlow.ashx";

    //else
    //MyFlow = "MyFlow.do";

    var url = MyFlow + "?DoType=InitToolBar&m=" + Math.random();

    $.ajax({
        type: 'post',
        async: true,
        data: pageData,
        url: url,
        dataType: 'html',
        success: function (data) {

            var barHtml = data;


            $('.Bar').html(barHtml);

            if ($('[name=Return]').length > 0) {
                $('[name=Return]').attr('onclick', '');
                $('[name=Return]').unbind('click');
                $('[name=Return]').bind('click', function () { initModal("returnBack"); $('#returnWorkModal').modal().show(); });
            }
            if ($('[name=Shift]').length > 0) {

                $('[name=Shift]').attr('onclick', '');
                $('[name=Shift]').unbind('click');
                $('[name=Shift]').bind('click', function () { initModal("shift"); $('#returnWorkModal').modal().show(); });
            }

            if ($('[name=workcheckBtn]').length > 0) {

                $('[name=workcheckBtn]').attr('onclick', '');
                $('[name=workcheckBtn]').unbind('click');
                $('[name=workcheckBtn]').bind('click', function () { initModal("workcheckBtn"); $('#returnWorkModal').modal().show(); });
            }

            if ($('[name=Askfor]').length > 0) {
                $('[name=Askfor]').attr('onclick', '');
                $('[name=Askfor]').unbind('click');
                $('[name=Askfor]').bind('click', function () { initModal("askfor"); $('#returnWorkModal').modal().show(); });
            }

            if ($('[name=HuiQian]').length > 0) {
                $('[name=HuiQian]').attr('onclick', '');
                $('[name=HuiQian]').unbind('click');
                $('[name=HuiQian]').bind('click', function () { initModal("HuiQian"); $('#returnWorkModal').modal().show(); });
            }

            if ($('[name=PackUp_zip]').length > 0) {
                $('[name=PackUp_zip]').attr('onclick', '');
                $('[name=PackUp_zip]').unbind('click');
                $('[name=PackUp_zip]').bind('click', function () { initModal("PackUp_zip"); $('#returnWorkModal').modal().show(); });
            }

            if ($('[name=PackUp_html]').length > 0) {
                $('[name=PackUp_html]').attr('onclick', '');
                $('[name=PackUp_html]').unbind('click');
                $('[name=PackUp_html]').bind('click', function () { initModal("PackUp_html"); $('#returnWorkModal').modal().show(); });
            }

            if ($('[name=PackUp_pdf]').length > 0) {
                $('[name=PackUp_pdf]').attr('onclick', '');
                $('[name=PackUp_pdf]').unbind('click');
                $('[name=PackUp_pdf]').bind('click', function () { initModal("PackUp_pdf"); $('#returnWorkModal').modal().show(); });
            }

            if ($('[name=SelectAccepter]').length > 0) {
                $('[name=SelectAccepter]').attr('onclick', '');
                $('[name=SelectAccepter]').unbind('click');
                $('[name=SelectAccepter]').bind('click', function () {
                    initModal("accepter");
                    $('#returnWorkModal').modal().show();
                });
            }
            if ($('[name=Delete]').length > 0) {
                var onclickFun = $('[name=Delete]').attr('onclick');
                if (onclickFun != undefined) {
                    if (plant == 'CCFlow') {
                        $('[name=Delete]').attr('onclick', onclickFun.replace('MyFlowInfo.htm', 'MyFlowInfo.aspx'));
                    } else {
                        $('[name=Delete]').attr('onclick', onclickFun.replace('MyFlowInfo.htm', 'MyFlowInfo.jsp'));
                    }
                }
            }
        }
    });
}

//初始化退回、移交、加签窗口
function initModal(modalType, toNode) {

    //初始化退回窗口的SRC
    var returnWorkModalHtml = '<div class="modal fade" id="returnWorkModal" data-backdrop="static">' +
       '<div class="modal-dialog">'
           + '<div class="modal-content" style="border-radius:0px;width:700px;text-align:left;">'
              + '<div class="modal-header">'
                  + '<button type="button" style="color:white;float: right;background: transparent;border: none;" data-dismiss="modal" aria-hidden="true">&times;</button>'
                   + '<h4 class="modal-title" id="modalHeader">工作退回</h4>'
               + '</div>'
               + '<div class="modal-body">'
                   + '<iframe style="width:100%;border:0px;height:400px;" id="iframeReturnWorkForm" name="iframeReturnWorkForm"></iframe>'
               + '</div>'
           + '</div><!-- /.modal-content -->'
       + '</div><!-- /.modal-dialog -->'
   + '</div>';

    $('body').append($(returnWorkModalHtml));

    var modalIframeSrc = '';
    if (modalType != undefined) {
        switch (modalType) {
            case "returnBack":
                $('#modalHeader').text("工作退回");
                modalIframeSrc = "./WorkOpt/ReturnWork.htm?FK_Node=" + pageData.FK_Node + "&FID=" + pageData.FID + "&WorkID=" + pageData.WorkID + "&FK_Flow=" + pageData.FK_Flow + "&s=" + Math.random()
                break;
            case "accpter":
                $('#modalHeader').text("工作移交");
                modalIframeSrc = "./WorkOpt/Accepter.htm?FK_Node=" + pageData.FK_Node + "&FID=" + pageData.FID + "&WorkID=" + pageData.WorkID + "&FK_Flow=" + pageData.FK_Flow + "&Info=&s=" + Math.random()
                break;
            case "workcheckBtn":
                $('#modalHeader').text("审核");
                modalIframeSrc = "./WorkOpt/WorkCheck.htm?FK_Node=" + pageData.FK_Node + "&FID=" + pageData.FID + "&WorkID=" + pageData.WorkID + "&FK_Flow=" + pageData.FK_Flow + "&Info=&s=" + Math.random()
                break;
            case "shift":
                $('#modalHeader').text("工作移交");
                modalIframeSrc = "./WorkOpt/Forward.htm?FK_Node=" + pageData.FK_Node + "&FID=" + pageData.FID + "&WorkID=" + pageData.WorkID + "&FK_Flow=" + pageData.FK_Flow + "&Info=&s=" + Math.random()
                break;
            case "askfor":
                $('#modalHeader').text("加签");
                modalIframeSrc = "./WorkOpt/Askfor.htm?FK_Node=" + pageData.FK_Node + "&FID=" + pageData.FID + "&WorkID=" + pageData.WorkID + "&FK_Flow=" + pageData.FK_Flow + "&Info=&s=" + Math.random()
                break;
            case "HuiQian":
                $('#modalHeader').text("会签");
                modalIframeSrc = "./WorkOpt/HuiQian.htm?FK_Node=" + pageData.FK_Node + "&FID=" + pageData.FID + "&WorkID=" + pageData.WorkID + "&FK_Flow=" + pageData.FK_Flow + "&Info=&s=" + Math.random()
                break;
            case "PackUp_zip":
            case "PackUp_html":
            case "PackUp_pdf":
                $('#modalHeader').text("打包下载/打印");
                var url = "./WorkOpt/Packup.htm?FileType=" + modalType.replace('PackUp_', '') + "&FK_Node=" + pageData.FK_Node + "&FID=" + pageData.FID + "&WorkID=" + pageData.WorkID + "&FK_Flow=" + pageData.FK_Flow + "&Info=&s=" + Math.random();
                // alert(url);
                modalIframeSrc = "./WorkOpt/Packup.htm?FileType=" + modalType.replace('PackUp_', '') + "&FK_Node=" + pageData.FK_Node + "&FID=" + pageData.FID + "&WorkID=" + pageData.WorkID + "&FK_Flow=" + pageData.FK_Flow + "&Info=&s=" + Math.random()
                break;
            case "accepter":
                $('#modalHeader').text("选择下一个节点及下一个节点接受人");
                modalIframeSrc = "./WorkOpt/Accepter.htm?FK_Node=" + pageData.FK_Node + "&FID=" + pageData.FID + "&WorkID=" + pageData.WorkID + "&FK_Flow=" + pageData.FK_Flow + "&s=" + Math.random()
                break;

            //发送选择接收节点和接收人     
            case "sendAccepter":
                $('#modalHeader').text("发送到节点：" + toNode.Name);
                modalIframeSrc = "./WorkOpt/Accepter.htm?FK_Node=" + pageData.FK_Node + "&FID=" + pageData.FID + "&WorkID=" + pageData.WorkID + "&FK_Flow=" + pageData.FK_Flow + "&ToNode=" + toNode.No + "&s=" + Math.random()
                break;
            default:
                break;
        }
    }
    $('#iframeReturnWorkForm').attr('src', modalIframeSrc);
}

//设置附件为只读
function setAttachDisabled() {
    //附件设置
    var attachs = $('iframe[src*="AttachmentUpload.htm"]');
    $.each(attachs, function (i, attach) {
        if (attach.src.indexOf('IsReadOnly') == -1) {
            $(attach).attr('src', $(attach).attr('src') + "&IsReadOnly=1");
        }
    })
}
//隐藏下方的功能按钮
function setToobarUnVisible() {
    //隐藏下方的功能按钮
    $('#bottomToolBar').css('display', 'none');
}

//隐藏下方的功能按钮
function setToobarDisiable() {
    //隐藏下方的功能按钮
    $('.Bar input').css('background', 'gray');
    $('.Bar input').attr('disabled', 'disabled');
}

function setToobarEnable() {
    //隐藏下方的功能按钮
    $('.Bar input').css('background', '#2884fa');
    $('.Bar input').removeAttr('disabled');
}
//设置表单元素不可用
function setFormEleDisabled() {
    //文本框等设置为不可用
    $('#divCCForm textarea').attr('disabled', 'disabled');
    $('#divCCForm select').attr('disabled', 'disabled');
    $('#divCCForm input[type!=button]').attr('disabled', 'disabled');
}

function CheckMinMaxLength() {

    var editor = document.activeEditor;
    if (editor) {
        var wordslen = editor.getContent().length,
            msg = "";

        if (wordslen > editor.MaxLen || wordslen < editor.MinLen) {
            msg += '@' + editor.BindFieldName + ' , 输入的值长度必须在:' + editor.MinLen + ', ' + editor.MaxLen + '之间. 现在输入是:' + wordslen;
        }

        if (msg != "") {
            alert(msg);
            return false;
        }
    }
    return true;

}

//保存
function Save() {

    //检查最小最大长度.
    var f = CheckMinMaxLength();
    if (f == false)
        return false;

    //必填项和正则表达式检查
    var formCheckResult = true;

    if (checkBlanks() == false) {
        formCheckResult = false;
    }

    if (checkReg() == false) {
        formCheckResult = false;
    }

    if (formCheckResult == false) {
        //alert("请检查表单必填项和正则表达式");
        return false;
    }

    setToobarDisiable();

    //获得表单数据.
    var frmData = getFormData(true, true);


    $.ajax({
        type: 'post',
        async: true,
        data: frmData,
        url: MyFlow + "?DoType=Save",
        dataType: 'html',
        success: function (data) {
            setToobarEnable();
            //刷新 从表的IFRAME
            var dtls = $('.Fdtl');
            $.each(dtls, function (i, dtl) {
                $(dtl).attr('src', $(dtl).attr('src'));
            });

            if (data.indexOf('保存成功') != 0 || data.indexOf('err@') == 0) {
                $('#Message').html(data.substring(4, data.length));
                $('#MessageDiv').modal().show();
            }
        }
    });
}

//退回工作
function returnWorkWindowClose(data) {

    if (data == "" || data == "取消") {
        $('#returnWorkModal').modal('hide');
        return;
    }

    $('#returnWorkModal').modal('hide');
    //通过下发送按钮旁的下拉框选择下一个节点
    if (data.indexOf('SaveOK@') == 0) {
        //说明保存人员成功,开始调用发送按钮.
        var toNode = 0;
        //含有发送节点 且接收
        if ($('#DDL_ToNode').length > 0) {
            var selectToNode = $('#DDL_ToNode  option:selected').data();
            toNode = selectToNode.No;
        }

        execSend(toNode);
        //$('[name=Send]:visible').click();
        return;
    } else {//可以重新打开接收人窗口
        winSelectAccepter = null;
    }

    if (data.indexOf('err@') == 0 || data == "取消") {//发送时发生错误
        $('#Message').html(data);
        $('#MessageDiv').modal().show();
    }
    else {
        OptSuc(data);
    }
}


//刷新子流程
function refSubSubFlowIframe() {
    var iframe = $('iframe[src*="SubFlow.aspx"]');
    //iframe[0].contentWindow.location.reload();
    iframe[0].contentWindow.location.href = iframe[0].src;
}
//回填扩展字段的值
function SetAth(data) {
    var atParamObj = $('#iframeAthForm').data();
    var tbId = atParamObj.tbId;
    var divId = atParamObj.divId;
    var athTb = $('#' + tbId);
    var athDiv = $('#' + divId);

    $('#athModal').modal('hide');
    //不存在或来自于viewWorkNodeFrm
    if (atParamObj != undefined && atParamObj.IsViewWorkNode != 1 && divId != undefined && tbId != undefined) {
        if (atParamObj.AthShowModel == "1") {
            athTb.val(data.join('*'));
            athDiv.html(data.join(';&nbsp;'));
        } else {
            athTb.val('@AthCount=' + data.length);
            athDiv.html("附件<span class='badge' >" + data.length + "</span>个");
        }
    } else {
        $('#athModal').removeClass('in');
    }
    $('#athModal').hide();
    var ifs = $("iframe[id^=track]").contents();
    if (ifs.length > 0) {
        for (var i = 0; i < ifs.length; i++) {
            $(ifs[i]).find(".modal-backdrop").hide();
        }
    }
}

//查看页面的附件展示  查看页面调用
function ShowViewNodeAth(athLab, atParamObj, src) {
    var athForm = $('iframeAthForm');
    var athModal = $('athModal');
    var athFormTitle = $('#athModal .modal-title');
    athFormTitle.text("上传附件：" + athLab);
    athModal.modal().show();
}

//window.onresize = function () {
//    if (pageData.Col == 8) {
//        if (jsonStr != undefined && jsonStr != '') {
//            var workNodeData = JSON.parse(jsonStr);
//            //设置CCFORM的表格宽度  
//            if (document.body.clientWidth > 992) {//处于中屏时设置宽度最小值
//                $('#CCForm').css('min-width', workNodeData.Sys_MapData[0].TableWidth);
//            }
//            else {
//                $('#CCForm').css('min-width', 0);
//            }
//        }
//    }
//}

 

//处理MapExt
function AfterBindEn_DealMapExt() {
    var workNode = JSON.parse(jsonStr);
    var mapExtArr = workNode.Sys_MapExt;

    for (var i = 0; i < mapExtArr.length; i++) {
        var mapExt = mapExtArr[i];
        switch (mapExt.ExtType) {
            case "PopVal": //PopVal窗返回值
                var tb = $('[name$=' + mapExt.AttrOfOper + ']');
                //tb.attr("placeholder", "请双击选择。。。");
                tb.attr("onclick", "ShowHelpDiv('TB_" + mapExt.AttrOfOper + "','','" + mapExt.MyPK + "','" + mapExt.FK_MapData + "','returnvalccformpopval');");
                tb.attr("ondblclick", "ReturnValCCFormPopValGoogle(this,'" + mapExt.MyPK + "','" + mapExt.FK_MapData + "', " + mapExt.W + "," + mapExt.H + ",'" + GepParaByName("Title", mapExt.AtPara) + "');");

                tb.attr('readonly', 'true');
                //tb.attr('disabled', 'true');
                var icon = '';
                var popWorkModelStr = '';
                var popWorkModelIndex = mapExt.AtPara != undefined ? mapExt.AtPara.indexOf('@PopValWorkModel=') : -1;
                if (popWorkModelIndex >= 0) {
                    popWorkModelIndex = popWorkModelIndex + '@PopValWorkModel='.length;
                    popWorkModelStr = mapExt.AtPara.substring(popWorkModelIndex, popWorkModelIndex + 1);
                }
                switch (popWorkModelStr) {
                    /// <summary>    
                    /// 自定义URL    
                    /// </summary>    
                    //SelfUrl =1,    
                    case "1":
                        icon = "glyphicon glyphicon-th";
                        break;
                    /// <summary>    
                    /// 表格模式    
                    /// </summary>    
                    // TableOnly,    
                    case "2":
                        icon = "glyphicon glyphicon-list";
                        break;
                    /// <summary>    
                    /// 表格分页模式    
                    /// </summary>    
                    //TablePage,    
                    case "3":
                        icon = "glyphicon glyphicon-list-alt";
                        break;
                    /// <summary>    
                    /// 分组模式    
                    /// </summary>    
                    // Group,    
                    case "4":
                        icon = "glyphicon glyphicon-list-alt";
                        break;
                    /// <summary>    
                    /// 树展现模式    
                    /// </summary>    
                    // Tree,    
                    case "5":
                        icon = "glyphicon glyphicon-tree-deciduous";
                        break;
                    /// <summary>    
                    /// 双实体树    
                    /// </summary>    
                    // TreeDouble    
                    case "6":
                        icon = "glyphicon glyphicon-tree-deciduous";
                        break;
                    default:
                        break;
                }
                tb.width(tb.width() - 40);
                tb.height('auto');
                var eleHtml = ' <div class="input-group form_tree" style="width:' + tb.width() + 'px;height:' + tb.height() + 'px">' + tb.parent().html() +
                '<span class="input-group-addon" onclick="' + "ReturnValCCFormPopValGoogle(document.getElementById('TB_" + mapExt.AttrOfOper + "'),'" + mapExt.MyPK + "','" + mapExt.FK_MapData + "', " + mapExt.W + "," + mapExt.H + ",'" + GepParaByName("Title", mapExt.AtPara) + "');" + '"><span class="' + icon + '"></span></span></div>';
                tb.parent().html(eleHtml);
                break;
            case "RegularExpression": //正则表达式  统一在保存和提交时检查
                var tb = $('[name$=' + mapExt.AttrOfOper + ']');
                //tb.attr(mapExt.Tag, "CheckRegInput('" + tb.attr('name') + "'," + mapExt.Doc.replace(/【/g, '[').replace(/】/g, ']').replace(/（/g, '(').replace(/）/g, ')').replace(/｛/g, '{').replace(/｝/g, '}') + ",'" + mapExt.Tag1 + "')");

                if (tb.attr('class') != undefined && tb.attr('class').indexOf('CheckRegInput') > 0) {
                    break;
                } else {
                    tb.addClass("CheckRegInput");
                    tb.data(mapExt)
                    //tb.data().name = tb.attr('name');
                    //tb.data().Doc = mapExt.Doc;
                    //tb.data().Tag1 = mapExt.Tag1;
                    //tb.attr("data-name", tb.attr('name'));
                    //tb.attr("data-Doc", tb.attr('name'));
                    //tb.attr("data-checkreginput", "CheckRegInput('" + tb.attr('name') + "'," + mapExt.Doc.replace(/【/g, '[').replace(/】/g, ']').replace(/（/g, '(').replace(/）/g, ')').replace(/｛/g, '{').replace(/｝/g, '}') + ",'" + mapExt.Tag1 + "')");
                }
                break;
            case "InputCheck": //输入检查
                //var tbJS = $("#TB_" + mapExt.AttrOfOper);
                //if (tbJS != undefined) {
                //    tbJS.attr(mapExt.Tag2, mapExt.Tag1 + "(this)");
                //}
                //else {
                //    tbJS = $("#DDL_" + mapExt.AttrOfOper);
                //    if (ddl != null)
                //        ddl.attr(mapExt.Tag2, mapExt.Tag1 + "(this);");
                //}
                break;
            case "TBFullCtrl": //自动填充
                var tbAuto = $("#TB_" + mapExt.AttrOfOper);
                if (tbAuto == null)
                    continue;

                tbAuto.attr("ondblclick", "ReturnValTBFullCtrl(this,'" + mapExt.MyPK + "');");
                tbAuto.attr("onkeyup", "DoAnscToFillDiv(this,this.value,\'TB_" + mapExt.AttrOfOper + "\', \'" + mapExt.MyPK + "\');");
                tbAuto.attr("AUTOCOMPLETE", "OFF");
                if (mapExt.Tag != "") {
                    /* 处理下拉框的选择范围的问题 */
                    var strs = mapExt.Tag.split('$');
                    for (var str in strs) {
                        var str = strs[k];
                        if (str = "") {
                            continue;
                        }

                        var myCtl = str.split(':');
                        var ctlID = myCtl[0];
                        var ddlC1 = $("#DDL_" + ctlID);
                        if (ddlC1 == null) {
                            continue;
                        }

                        //如果文本库数值为空，就让其返回.
                        var txt = tbAuto.val();
                        if (txt == '')
                            continue;

                        //获取要填充 ddll 的SQL.
                        var sql = myCtl[1].replace(/~/g, "'");
                        sql = sql.replace("@Key", txt);
                        //sql = BP.WF.Glo.DealExp(sql, en, null);  怎么办

                        //try
                        //{
                        //    dt = DBAccess.RunSQLReturnTable(sql);
                        //}
                        //catch (Exception ex)
                        //{
                        //    this.Clear();
                        //    this.AddFieldSet("配置错误");
                        //    this.Add(me.ToStringAtParas() + "<hr>错误信息:<br>" + ex#MessageDiv);
                        //    this.AddFieldSetEnd();
                        //    return;
                        //}

                        //if (dt.Rows.Count != 0)
                        //{
                        //    string valC1 = ddlC1.SelectedItemStringVal;
                        //    foreach (DataRow dr in dt.Rows)
                        //{
                        //        ListItem li = ddlC1.Items.FindByValue(dr[0].ToString());
                        //    if (li == null)
                        //    {
                        //        ddlC1.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                        //    }
                        //    else
                        //    {
                        //        li.Attributes["enable"] = "false";
                        //        li.Attributes["display"] = "false";

                        //    }
                        //}
                        //ddlC1.SetSelectItem(valC1);
                    }
                }

                break;
            case "ActiveDDL": /*自动初始化ddl的下拉框数据. 下拉框的级联操作 已经 OK*/
                var ddlPerant = $("#DDL_" + mapExt.AttrOfOper);
                var ddlChild = $("#DDL_" + mapExt.AttrsOfActive);
                if (ddlPerant == null || ddlChild == null)
                    continue;
                ddlPerant.attr("onchange", "DDLAnsc(this.value,\'" + "DDL_" + mapExt.AttrsOfActive + "\', \'" + mapExt.MyPK + "\')");
                // 处理默认选择。
                //string val = ddlPerant.SelectedItemStringVal;
                var valClient = ConvertDefVal(workNode, '', mapExt.AttrsOfActive); // ddlChild.SelectedItemStringVal;

                //ddlChild.select(valClient);  未写
                break;
            case "AutoFullDLL": // 自动填充下拉框.
                continue; //已经处理了。
            case "DDLFullCtrl": // 自动填充其他的控件..  先不做
                var ddlOper = $("#DDL_" + mapExt.AttrOfOper);
                if (ddlOper == null)
                    continue;

                ddlOper.attr("onchange", "Change('" + workNode.Sys_MapData[0].No + "');DDLFullCtrl(this.value,\'" + "DDL_" + mapExt.AttrOfOper + "\', \'" + mapExt.MyPK + "\')");
                if (mapExt.Tag != null && mapExt.Tag != "") {
                    /* 下拉框填充范围. */
                    var strs = mapExt.Tag.split('$');
                    for (var k = 0; k < strs.length; k++) {
                        var str = strs[k];
                        if (str == "")
                            continue;

                        var myCtl = str.split(':');
                        var ctlID = myCtl[0];
                        var ddlC1 = $("#DDL_" + ctlID);
                        if (ddlC1 == null) {
                            //me.Tag = "";
                            //me.Update();
                            continue;
                        }

                        //如果触发的dll 数据为空，则不处理.
                        if (ddlOper.val() == "")
                            continue;

                        var sql = myCtl[1].replace(/~/g, "'");
                        sql = sql.replace("@Key", ddlOper.val());

                        //需要执行SQL语句
                        //sql = BP.WF.Glo.DealExp(sql, en, null);

                        //dt = DBAccess.RunSQLReturnTable(sql);
                        //string valC1 = ddlC1.SelectedItemStringVal;
                        //if (dt.Rows.Count != 0)
                        //{
                        //    foreach (DataRow dr in dt.Rows)
                        //{
                        //        ListItem li = ddlC1.Items.FindByValue(dr[0].ToString());
                        //    if (li == null)
                        //    {
                        //        ddlC1.Items.Add(new ListItem(dr[1].ToString(), dr[0].ToString()));
                        //    }
                        //    else
                        //    {
                        //        li.Attributes["visable"] = "false";
                        //    }
                        //}

                        //var items = [{ No: 1, Name: '测试1' }, { No: 2, Name: '测试2' }, { No: 3, Name: '测试3' }, { No: 4, Name: '测试4' }, { No: 5, Name: '测试5'}];
                        var operations = '';
                        //                        $.each(items, function (i, item) {
                        //                            operations += "<option  value='" + item.No + "'>" + item.Name + "</option>";
                        //                        });
                        ddlC1.children().remove();
                        ddlC1.html(operations);
                        //ddlC1.SetSelectItem(valC1);
                    }
                }
                break;
        }
    }
}
//AtPara  @PopValSelectModel=0@PopValFormat=0@PopValWorkModel=0@PopValShowModel=0
function GepParaByName(name, atPara) {
    var params = atPara.split('@');
    var result = $.grep(params, function (value) {
        return value != '' && value.split('=').length == 2 && value.split('=')[0] == value;
    })
    return result;
}

//初始化下拉列表框的OPERATION
function InitDDLOperation(workNodeData, mapAttr, defVal) {
    var operations = '';
    //外键类型
    if (mapAttr.LGType == 2) {
        if (workNodeData[mapAttr.KeyOfEn] != undefined) {
            $.each(workNodeData[mapAttr.KeyOfEn], function (i, obj) {
                operations += "<option " + (obj.No == defVal ? " selected='selected' " : "") + " value='" + obj.No + "'>" + obj.Name + "</option>";
            });
        }
        else if (workNodeData[mapAttr.UIBindKey] != undefined) {
            $.each(workNodeData[mapAttr.UIBindKey], function (i, obj) {
                operations += "<option " + (obj.No == defVal ? " selected='selected' " : "") + " value='" + obj.No + "'>" + obj.Name + "</option>";
            });
        }
    } else {
        var enums = workNodeData.Sys_Enum;
        enums = $.grep(enums, function (value) {
            return value.EnumKey == mapAttr.UIBindKey;
        });


        $.each(enums, function (i, obj) {
            operations += "<option " + (obj.IntKey == defVal ? " selected='selected' " : "") + " value='" + obj.IntKey + "'>" + obj.Lab + "</option>";
        });

    }
    return operations;
}

//填充默认数据
function ConvertDefVal(workNodeData, defVal, keyOfEn) {
    //计算URL传过来的表单参数@TXB_Title=事件测试

    var pageParams = getQueryString();
    var pageParamObj = {};
    $.each(pageParams, function (i, pageParam) {
        if (pageParam.indexOf('@') == 0) {
            var pageParamArr = pageParam.split('=');
            pageParamObj[pageParamArr[0].substring(1, pageParamArr[0].length)] = pageParamArr[1];
        }
    });

    var result = defVal;

    //通过MAINTABLE返回的参数
    for (var ele in workNodeData.MainTable[0]) {
        if (keyOfEn == ele && workNodeData.MainTable[0] != '') {
            //console.info(ele + "==" + workNodeData.MainTable[0][ele]);
            result = workNodeData.MainTable[0][ele];
            break;
        }
    }

    //通过URL参数传过来的参数  后台处理到MainTable 里面
    //for (var pageParam in pageParamObj) {
    //    if (pageParam == keyOfEn) {
    //        result = pageParamObj[pageParam];
    //        break;
    //    }
    //}

    if (result != undefined && typeof (result) == 'string') {
        //result = result.replace(/｛/g, "{").replace(/｝/g, "}").replace(/：/g, ":").replace(/，/g, ",").replace(/【/g, "[").replace(/】/g, "]").replace(/；/g, ";").replace(/~/g, "'").replace(/‘/g, "'").replace(/‘/g, "'");
    }
    //console.info(defVal+"=="+keyOfEn+"=="+result);
    var result = unescape(result);

    if (result == "null")
        result = "";

    return result;
}

//获取表单数据
function getFormData(isCotainTextArea, isCotainUrlParam) {

    var formss = $('#divCCForm').serialize();
    var formArr = formss.split('&');
    var formArrResult = [];
    //获取CHECKBOX的值
    $.each(formArr, function (i, ele) {
        if (ele.split('=')[0].indexOf('CB_') == 0) {
            if ($('#' + ele.split('=')[0] + ':checked').length == 1) {
                ele = ele.split('=')[0] + '=1';
            } else {
                ele = ele.split('=')[0] + '=0';
            }
        }
        formArrResult.push(ele);
    });

    //获取表单中禁用的表单元素的值
    var disabledEles = $('#divCCForm :disabled');
    $.each(disabledEles, function (i, disabledEle) {
        var name = $(disabledEle).attr('name');
        switch (disabledEle.tagName.toUpperCase()) {
            case "INPUT":
                switch (disabledEle.type.toUpperCase()) {
                    case "CHECKBOX": //复选框
                        formArrResult.push(name + '=' + $(disabledEle).is(':checked') ? 1 : 0);
                        break;
                    case "TEXT": //文本框
                        formArrResult.push(name + '=' + $(disabledEle).val());
                        break;
                    case "RADIO": //单选钮
                        var eleResult = name + '=' + $('[name="' + name + ':checked"]').val();
                        if (!$.inArray(formArrResult, eleResult)) {
                            formArrResult.push();
                        }
                        break;
                }
                break;
            //下拉框  
            case "SELECT":
                formArrResult.push(name + '=' + $(disabledEle).children('option:checked').val());
                break;
            //formArrResult.push(name + '=' + $(disabledEle).children('option:checked').val()); 
            //对于复选下拉框获取值得方法 
            //                if ($('[data-id=' + name + ']').length > 0) { 
            //                    var val = $(disabledEle).val().join(','); 
            //                    formArrResult.push(name + '=' + val); 
            //                } else { 
            //                    formArrResult.push(name + '=' + $(disabledEle).children('option:checked').val()); 
            //                } 
            // break; 
            //文本区域  
            case "TEXTAREA":
                formArrResult.push(name + '=' + $(disabledEle).val());
                break;
        }
    });

    //获取表单中隐藏的表单元素的值
    var hiddens = $('input[type=hidden]');
    $.each(hiddens, function (i, hidden) {
        if ($(hidden).attr("name").indexOf('TB_') == 0) {
            //formArrResult.push($(hidden).attr("name") + '=' + $(hidden).val());
        }
    });

    if (!isCotainTextArea) {
        formArrResult = $.grep(formArrResult, function (value) {
            return value.split('=').length == 2 ? value.split('=')[1].length <= 50 : true;
        });
    }

    formss = formArrResult.join('&');
    var dataArr = [];
    //加上URL中的参数
    if (pageData != undefined && isCotainUrlParam) {
        var pageDataArr = [];
        for (var data in pageData) {
            pageDataArr.push(data + '=' + pageData[data]);
        }
        dataArr.push(pageDataArr.join('&'));
    }
    if (formss != '')
        dataArr.push(formss);
    var formData = dataArr.join('&');


    //为了复选框  合并一下值  复选框的值以  ，号分割
    //用& 符号截取数据
    var formDataArr = formData.split('&');
    var formDataResultObj = {};
    $.each(formDataArr, function (i, formDataObj) {
        //计算出等号的INDEX
        var indexOfEqual = formDataObj.indexOf('=');
        var objectKey = formDataObj.substr(0, indexOfEqual);
        var objectValue = formDataObj.substr(indexOfEqual + 1);
        if (formDataResultObj[objectKey] == undefined) {
            formDataResultObj[objectKey] = objectValue;
        } else {
            formDataResultObj[objectKey] = formDataResultObj[objectKey] + ',' + objectValue;
        }
    });

    var formdataResultStr = '';
    for (var ele in formDataResultObj) {
        formdataResultStr = formdataResultStr + ele + '=' + formDataResultObj[ele] + '&';
    }
    return formdataResultStr;
}
//发送
function Send() {

    //检查最小最大长度.
    var f = CheckMinMaxLength();
    if (f == false)
        return false;

    //必填项和正则表达式检查.
    if (checkBlanks() == false) {
        alert("检查必填项出现错误，边框变红颜色的是否填写完整？");
        return;
    }

    if (checkReg() == false) {
        alert("发送错误:请检查字段边框变红颜色的是否填写完整？");
        return;
    }

    var toNode = 0;
    //含有发送节点 且接收
    if ($('#DDL_ToNode').length > 0) {
        var selectToNode = $('#DDL_ToNode  option:selected').data();
        if (selectToNode.IsSelectEmps == "1") {//跳到选择接收人窗口

            Save();

            initModal("sendAccepter", selectToNode);

            $('#returnWorkModal').modal().show();
            return false;
        } else {
            toNode = selectToNode.No;
        }
    }

    execSend(toNode);
}

function execSend(toNode) {
    //先设置按钮等不可用
    setToobarDisiable();

    $.ajax({
        type: 'post',
        async: true,
        data: getFormData(true, true) + "&ToNode=" + toNode,
        url: MyFlow + "?DoType=Send",
        dataType: 'html',
        success: function (data) {

            if (data.indexOf('err@') == 0) {//发送时发生错误
                $('#Message').html(data.substring(4, data.length));
                $('#MessageDiv').modal().show();
                setToobarEnable();
                return;
            }

            if (data.indexOf('url@') == 0) { //发送成功时转到指定的URL 
                var url = data;
                url = url.replace('url@', '');
                window.location.href = url;
                return;
            }

            OptSuc(data);
            //  $('#Message').html(data);
            // $('#MessageDiv').modal().show();

            if (opener != null && opener.window != null && opener.window.parent != null
            && opener.window.parent.refSubSubFlowIframe != null && typeof (opener.window.parent.refSubSubFlowIframe) == "function") {
                opener.window.parent.refSubSubFlowIframe();
            }
            //if (window.opener != null && window.opener != undefined && window.opener)
            //    $('#Message').html(data);
            //$('#MessageDiv').modal().show();
            ////发送成功时
            //setAttachDisabled();
            //setToobarUnVisible();
            //setFormEleDisabled();
        }
    });
}

$(function () {

    $('#btnMsgModalOK').bind('click', function () {
        if (window.opener) {

            if (window.opener.name && window.opener.name == "main") {
                window.opener.location.href = window.opener.location.href;
                if (window.opener.top && window.opener.top.leftFrame) {
                    window.opener.top.leftFrame.location.href = window.opener.top.leftFrame.location.href;
                }
            } else if (window.opener.name && window.opener.name == "运行流程") {
                //测试运行流程，不进行刷新
            } else {
                //window.opener.location.href = window.opener.location.href;
            }
        }
        window.close();
    });

    setAttachDisabled();
    setToobarDisiable();
    setFormEleDisabled();

    $('#btnMsgModalOK1').bind('click', function () {
        window.close();
        opener.window.focus();
    });

})


//发送 退回 移交等执行成功后转到  指定页面
function OptSuc(msg) {
    // window.location.href = "/WF/MyFlowInfo.aspx";
    // $('#MessageDiv').modal().hide();
    if ($('#returnWorkModal:hidden').length == 0 && $('#returnWorkModal').length > 0) {
        $('#returnWorkModal').modal().hide()
    }
    msg = msg.replace("@查看<img src='/WF/Img/Btn/PrintWorkRpt.gif' >", '')

    $("#msgModalContent").html(msg.replace(/@/g, '<br/>'));
    var trackA = $('#msgModalContent a:contains("工作轨迹")');
    var trackImg = $('#msgModalContent img[src*="PrintWorkRpt.gif"]');
    trackA.remove();
    trackImg.remove();
    $("#msgModal").modal().show();
}
//移交
//初始化发送节点下拉框
function InitToNodeDDL(workNode) {


    if (workNode.ToNodes != undefined && workNode.ToNodes.length > 0) {
        // $('[value=发送]').
        var toNodeDDL = $('<select style="width:auto;" id="DDL_ToNode"></select>');
        $.each(workNode.ToNodes, function (i, toNode) {
            //IsSelectEmps: "1"
            //Name: "节点2"
            //No: "702"

            var opt = "";
            if (toNode.IsSelected == "1") {
                var opt = $("<option value='" + toNode.No + "' selected='true' >" + toNode.Name + "</option>");
                opt.data(toNode);
            } else {
                var opt = $("<option value='" + toNode.No + "'>" + toNode.Name + "</option>");
                opt.data(toNode);
            }

            toNodeDDL.append(opt);

        });


        $('[name=Send]').after(toNodeDDL);
    }
}

//根据下拉框选定的值，弹出提示信息  绑定那个元素显示，哪个元素不显示  
function showNoticeInfo() {
    var workNode = JSON.parse(jsonStr);
    var rbs = workNode.Sys_FrmRB;
    data = rbs;
    $("input[type=radio],select").bind('change', function (obj) {
        var needShowDDLids = [];
        var methodVal = obj.target.value;

        for (var j = 0; j < data.length; j++) {
            var value = data[j].IntKey;
            var noticeInfo = data[j].Tip;
            var drdlColName = data[j].KeyOfEn;

            if (obj.target.tagName == "SELECT") {
                drdlColName = 'DDL_' + drdlColName;
            } else {
                drdlColName = 'RB_' + drdlColName;
            }
            //if (methodVal == value &&  obj.target.name.indexOf(drdlColName) == (obj.target.name.length - drdlColName.length)) {
            if (methodVal == value && (obj.target.name == drdlColName)) {
                //高级JS设置;  设置表单字段的  可用 可见 不可用 
                var fieldConfig = data[j].FieldsCfg;
                var fieldConfigArr = fieldConfig.split('@');
                for (var k = 0; k < fieldConfigArr.length; k++) {
                    var fieldCon = fieldConfigArr[k];
                    if (fieldCon != '' && fieldCon.split('=').length == 2) {
                        var fieldConArr = fieldCon.split('=');
                        var ele = $('[name$=' + fieldConArr[0] + ']');
                        if (ele.length == 0) {
                            continue;
                        }
                        var labDiv = undefined;
                        var eleDiv = undefined;
                        if (ele.css('display').toUpperCase() == "NONE") {
                            continue;
                        }

                        if (ele.parent().attr('class').indexOf('input-group') >= 0) {
                            labDiv = ele.parent().parent().prev();
                            eleDiv = ele.parent().parent();
                        } else {
                            labDiv = ele.parent().prev();
                            eleDiv = ele.parent();
                        }
                        switch (fieldConArr[1]) {
                            case "1": //可用
                                if (labDiv.css('display').toUpperCase() == "NONE" && ele[0].id.indexOf('DDL_') == 0) {
                                    needShowDDLids.push(ele[0].id);
                                }

                                labDiv.css('display', 'block');
                                eleDiv.css('display', 'block');
                                ele.removeAttr('disabled');


                                break;
                            case "2": //可见
                                if (labDiv.css('display').toUpperCase() == "NONE" && ele[0].id.indexOf('DDL_') == 0) {
                                    needShowDDLids.push(ele[0].id);
                                }

                                labDiv.css('display', 'block');
                                eleDiv.css('display', 'block');
                                break;
                            case "3": //不可见
                                labDiv.css('display', 'none');
                                eleDiv.css('display', 'none');
                                break;
                        }
                    }
                }
                //根据下拉列表的值选择弹出提示信息
                if (noticeInfo == undefined || noticeInfo.trim() == '') {
                    break;
                }
                noticeInfo = noticeInfo.replace(/\\n/g, '<br/>')
                var selectText = '';
                if (obj.target.tagName.toUpperCase() == 'INPUT' && obj.target.type.toUpperCase() == 'RADIO') {//radio button
                    selectText = obj.target.nextSibling.textContent;
                } else {//select
                    selectText = $(obj.target).find("option:selected").text();
                }
                $($('#div_NoticeInfo .popover-title span')[0]).text(selectText);
                $('#div_NoticeInfo .popover-content').html(noticeInfo);

                var top = obj.target.offsetHeight;
                var left = obj.target.offsetLeft;
                var current = obj.target.offsetParent;
                while (current !== null) {
                    left += current.offsetLeft;
                    top += current.offsetTop;
                    current = current.offsetParent;
                }


                if (obj.target.tagName.toUpperCase() == 'INPUT' && obj.target.type.toUpperCase() == 'RADIO') {//radio button
                    left = left - 40;
                    top = top + 10;
                }
                if (top - $('#div_NoticeInfo').height() - 30 < 0) {
                    //让提示框在下方展示
                    $('#div_NoticeInfo').removeClass('top');
                    $('#div_NoticeInfo').addClass('bottom');
                    top = top;
                } else {
                    $('#div_NoticeInfo').removeClass('bottom');
                    $('#div_NoticeInfo').addClass('top');
                    top = top - $('#div_NoticeInfo').height() - 30;
                }
                $('#div_NoticeInfo').css('top', top);
                $('#div_NoticeInfo').css('left', left);
                $('#div_NoticeInfo').css('display', 'block');
                //$("#btnNoticeInfo").popover('show');
                //$('#btnNoticeInfo').trigger('click');
                break;
            }
        }

        $.each(needShowDDLids, function (i, ddlId) {
            $('#' + ddlId).change();
        });
    });


    $('#span_CloseNoticeInfo').bind('click', function () {
        $('#div_NoticeInfo').css('display', 'none');
    })

    $("input[type=radio]:checked,select").change();
    $('#span_CloseNoticeInfo').click();
}

//给出文本框输入提示信息
function showTbNoticeInfo() {
    var workNode = JSON.parse(jsonStr);
    var mapAttr = workNode.Sys_MapAttr;
    mapAttr = $.grep(mapAttr, function (attr) {
        var atParams = attr.AtPara;
        return atParams != undefined && AtParaToJson(atParams).Tip != undefined && AtParaToJson(atParams).Tip != '' && $('#TB_' + attr.KeyOfEn).length > 0 && $('#TB_' + attr.KeyOfEn).css('display') != 'none';
    })

    $.each(mapAttr, function (i, attr) {
        $('#TB_' + attr.KeyOfEn).bind('focus', function (obj) {
            var workNode = JSON.parse(jsonStr);
            var mapAttr = workNode.Sys_MapAttr;

            mapAttr = $.grep(mapAttr, function (attr) {
                return 'TB_' + attr.KeyOfEn == obj.target.id;
            })
            var atParams = AtParaToJson(mapAttr[0].AtPara);
            var noticeInfo = atParams.Tip;

            if (noticeInfo == undefined || noticeInfo == '')
                return;

            //noticeInfo = noticeInfo.replace(/\\n/g, '<br/>')

            $($('#div_NoticeInfo .popover-title span')[0]).text(mapAttr[0].Name);
            $('#div_NoticeInfo .popover-content').html(noticeInfo);

            var top = obj.target.offsetHeight;
            var left = obj.target.offsetLeft;
            var current = obj.target.offsetParent;
            while (current !== null) {
                left += current.offsetLeft;
                top += current.offsetTop;
                current = current.offsetParent;
            }

            if (top - $('#div_NoticeInfo').height() - 30 < 0) {
                //让提示框在下方展示
                $('#div_NoticeInfo').removeClass('top');
                $('#div_NoticeInfo').addClass('bottom');
                top = top;
            } else {
                $('#div_NoticeInfo').removeClass('bottom');
                $('#div_NoticeInfo').addClass('top');
                top = top - $('#div_NoticeInfo').height() - 30;
            }
            $('#div_NoticeInfo').css('top', top);
            $('#div_NoticeInfo').css('left', left);
            $('#div_NoticeInfo').css('display', 'block');
        });
    })
}

//必填项检查   名称最后是*号的必填  如果是选择框，值为'' 或者 显示值为 【*请选择】都算为未填 返回FALSE 检查必填项失败
function checkBlanks() {
    var checkBlankResult = true;
    //获取所有的列名 找到带* 的LABEL mustInput
    //var lbs = $('[class*=col-md-1] label:contains(*)');
    var lbs = $('.mustInput'); //获得所有的class=mustInput的元素.
    $.each(lbs, function (i, obj) {
        if ($(obj).parent().css('display') != 'none' && $(obj).parent().next().css('display')) {
            var keyofen = $(obj).data().keyofen;

            var ele = $('[id$=_' + keyofen + ']');
            if (ele.length == 1) {
                switch (ele[0].tagName.toUpperCase()) {
                    case "INPUT":
                        if (ele.attr('type') == "text") {
                            if (ele.val() == "") {
                                checkBlankResult = false;
                                ele.addClass('errorInput');
                            } else {
                                ele.removeClass('errorInput');
                            }
                        }
                        break;
                    case "SELECT":
                        if (ele.val() == "" || ele.children('option:checked').text() == "*请选择") {
                            checkBlankResult = false;
                            ele.addClass('errorInput');
                        } else {
                            ele.removeClass('errorInput');
                        }
                        break;
                    case "TEXTAREA":
                        if (ele.val() == "") {
                            checkBlankResult = false;
                            ele.addClass('errorInput');
                        } else {
                            ele.removeClass('errorInput');
                        }
                        break;
                }
            }
        }
    });

    //2.对 UMEditor 中的必填项检查
    window.UEs.forEach(function (item) {
        //如果是必填
        if (item.attr.UIIsInput == 1) {
            var ele = item.editor.$body;
            if (item.editor.getPlainTxt().trim() === "") {
                checkBlankResult = false;
                ele.addClass('errorInput');
            } else {
                ele.removeClass('errorInput');
            }
        }
    });

    return checkBlankResult;
}

//正则表达式检查
function checkReg() {
    var checkRegResult = true;
    var regInputs = $('.CheckRegInput');
    $.each(regInputs, function (i, obj) {
        var name = obj.name;
        var mapExtData = $(obj).data();
        if (mapExtData.Doc != undefined) {
            var regDoc = mapExtData.Doc.replace(/【/g, '[').replace(/】/g, ']').replace(/（/g, '(').replace(/）/g, ')').replace(/｛/g, '{').replace(/｝/g, '}').replace(/，/g, ',');
            var tag1 = mapExtData.Tag1;
            if ($(obj).val() != undefined && $(obj).val() != '') {

                var result = CheckRegInput(name, regDoc, tag1);
                if (!result) {
                    $(obj).addClass('errorInput');
                    checkRegResult = false;
                } else {
                    $(obj).removeClass('errorInput');
                }
            }
        }
    });

    //2.对 UMEditor 中的必填项检查
    window.UEs.forEach(function (item) {
        //如果是必填
        if (item.attr.UIIsInput == 1) {
            var ele = item.editor.$body;
            if (item.editor.getPlainTxt().trim() === "") {
                checkBlankResult = false;
                ele.addClass('errorInput');
            } else {
                ele.removeClass('errorInput');
            }
        }
    });

    return checkRegResult;
}

function SaveDtlAll() {
    return true;
}

// 生成表单.
function GenerWorkNode() {

    var href = window.location.href;
    var urlParam = href.substring(href.indexOf('?') + 1, href.length);
    urlParam = urlParam.replace('&DoType=', '&DoTypeDel=xx');

    $.ajax({
        type: 'post',
        async: true,
        data: pageData,
        url: MyFlow + "?DoType=GenerWorkNode&m=" + Math.random() + "&" + urlParam,
        dataType: 'html',
        success: function (data) {

            if (data.indexOf('err@') == 0) {
                alert(data);
                return;
            }

            //console.info(data);
            jsonStr = data;
            var gengerWorkNode = {};
            var flow_Data;

            try {

                flow_Data = JSON.parse(data);
                workNodeData = flow_Data;

            } catch (err) {
                alert("GenerWorkNode转换JSON失败:" + jsonStr);
                return;
            }

            var node = workNodeData.WF_Node[0];

            //设置标题.
            document.title = node.Name;
            document.title = "业务流程管理（BPM）平台";
            var Sys_GroupFields = workNodeData.Sys_GroupField;
            //初始化Sys_MapData
            var h = flow_Data.Sys_MapData[0].FrmH;
            var w = flow_Data.Sys_MapData[0].FrmW;

            $('#CCForm').html('');

            var tableWidth = w - 40;
            var html = "<table style='width:" + tableWidth + "px;' >";

            var frmName = workNodeData.Sys_MapData[0].Name;

            html += "<tr>";
            html += "<td colspan=4 ><div style='float:left' ><img src='../DataUser/ICON/LogBiger.png'  style='height:50px;' /></div><div style='float:right;padding:10px;bordder:none' ><h4><b>" + frmName + "</b></h4></div></td>";
            //  html += "<td colspan=2 ></td>";
            html += "</tr>";
            //遍历循环生成 listview
            for (var i = 0; i < Sys_GroupFields.length; i++) {

                var gf = Sys_GroupFields[i];

                //从表..
                if (gf.CtrlType == 'Dtl') {

                    html += "<tr>";
                    html += "  <th colspan=4>" + gf.Lab + "</th>";
                    html += "</tr>";

                    var dtls = workNodeData.Sys_MapDtl;

                    for (var k = 0; k < dtls.length; k++) {

                        var dtl = dtls[k];

                        if (dtl.No != gf.CtrlID)
                            continue;

                        html += "<tr>";
                        html += "  <td colspan='4' >";

                        html += figure_Template_Dtl(dtl);

                        html += "  </td>";
                        html += "</tr>";
                    }
                    continue;
                }


                //附件类的控件.
                if (gf.CtrlType == 'Ath') {

                    html += "<tr>";
                    html += "  <th colspan=4>" + gf.Lab + "</th>";
                    html += "</tr>";


                    html += "<tr>";
                    html += "  <td colspan='4' >";

                    html += figure_Template_Attachment(workNodeData, gf);

                    html += "  </td>";
                    html += "</tr>";

                    continue;
                }


                //审核组件..
                if (gf.CtrlType == 'FWC' && node.FWCSta != 0) {

                    html += "<tr>";
                    html += "  <th colspan=4>" + gf.Lab + "</th>";
                    html += "</tr>";

                    html += "<tr>";
                    html += "  <td colspan='4' >";

                    html += figure_Template_FigureFrmCheck(node);

                    html += "  </td>";
                    html += "</tr>";

                    continue;
                }

                //字段类的控件.
                if (gf.CtrlType == '' || gf.CtrlType == null) {

                    html += "<tr>";
                    html += "  <th colspan=4>" + gf.Lab + "</th>";
                    html += "</tr>";

                    html += InitMapAttr(workNodeData.Sys_MapAttr, workNodeData, gf.OID);
                    continue;
                }
            }

            html += "</table>";

            //加入隐藏控件.

            for (var attr in workNodeData.Sys_MapAttr) {
                if (attr.UIVisable == 0) {
                    var defval = ConvertDefVal(workNodeData, attr.DefVal, attr.KeyOfEn);
                    html += "<input type='hidden' id='TB_" + attr.KeyOfEn + "' name='TB_" + attr.KeyOfEn + "' value='" + defval + "' />";
                }
            }

            $('#CCForm').html("").append(html);

            //循环之前的提示信息.
            var info = "";
            for (var i in flow_Data.AlertMsg) {
                var alertMsg = flow_Data.AlertMsg[i];
                var alertMsgEle = figure_Template_MsgAlert(alertMsg, i);
                $('#Message').append(alertMsgEle);
                $('#Message').append($('<hr/>'));
            }

            if (flow_Data.AlertMsg.length != 0) {
                $('#MessageDiv').modal().show();
            }

            //循环组件 轨迹图 审核组件 子流程 子线程
            // $('#CCForm').append(figure_Template_FigureFlowChart(flow_Data["WF_FrmNodeComponent"][0]));
            // $('#CCForm').append(figure_Template_FigureFrmCheck(flow_Data["WF_FrmNodeComponent"][0]));
            // $('#CCForm').append(figure_Template_FigureSubFlowDtl(flow_Data["WF_FrmNodeComponent"][0]));
            // $('#CCForm').append(figure_Template_FigureThreadDtl(flow_Data["WF_FrmNodeComponent"][0]));

            // $('#topContentDiv').height(h);
            $('#topContentDiv').width(w);
            $('.Bar').width(w + 15);
            $('#lastOptMsg').width(w + 15);
            var marginLeft = $('#topContentDiv').css('margin-left');
            marginLeft = parseFloat(marginLeft.substr(0, marginLeft.length - 2)) + 50;
            $('#topContentDiv i').css('left', marginLeft.toString() + 'px');
            //原有的

            //为 DISABLED 的 TEXTAREA 加TITLE 
            var disabledTextAreas = $('#divCCForm textarea:disabled');
            $.each(disabledTextAreas, function (i, obj) {
                $(obj).attr('title', $(obj).val());
            })

            //根据NAME 设置ID的值
            var inputs = $('[name]');
            $.each(inputs, function (i, obj) {
                if ($(obj).attr("id") == undefined || $(obj).attr("id") == '') {
                    $(obj).attr("id", $(obj).attr("name"));
                }
            })


            ////加载JS文件 改变JS文件的加载方式 解决JS在资源中不显示的问题
            var enName = workNodeData.Sys_MapData[0].No;
            try {
                ////加载JS文件
                //jsSrc = "<script language='JavaScript' src='/DataUser/JSLibData/" + enName + "_Self.js' ></script>";
                //$('body').append($('<div>' + jsSrc + '</div>'));

                var s = document.createElement('script');
                s.type = 'text/javascript';
                s.src = "../DataUser/JSLibData/" + enName + "_Self.js";
                var tmp = document.getElementsByTagName('script')[0];
                tmp.parentNode.insertBefore(s, tmp);
            }
            catch (err) {

            }

            var jsSrc = '';
            try {
                //jsSrc = "<script language='JavaScript' src='/DataUser/JSLibData/" + enName + ".js' ></script>";
                //$('body').append($('<div>' + jsSrc + '</div>'));

                var s = document.createElement('script');
                s.type = 'text/javascript';
                s.src = "../DataUser/JSLibData/" + enName + "_Self.js";
                var tmp = document.getElementsByTagName('script')[0];
                tmp.parentNode.insertBefore(s, tmp);
            }
            catch (err) {

            }

            InitToNodeDDL(flow_Data);

            Common.MaxLengthError();

            //处理下拉框级联等扩展信息
            AfterBindEn_DealMapExt();

            //设置默认值
            for (var j = 0; j < workNodeData.Sys_MapAttr.length; j++) {
                var mapAttr = workNodeData.Sys_MapAttr[j];

                //添加 label
                //如果是整行的需要添加  style='clear:both'
                var defValue = ConvertDefVal(workNodeData, mapAttr.DefVal, mapAttr.KeyOfEn);

                if ($('#TB_' + mapAttr.KeyOfEn).length == 1) {
                    $('#TB_' + mapAttr.KeyOfEn).val(defValue);
                }

                if ($('#DDL_' + mapAttr.KeyOfEn).length == 1) {
                    $('#DDL_' + mapAttr.KeyOfEn).val(defValue);
                }
            }

            showNoticeInfo();

            showTbNoticeInfo();


            //初始化复选下拉框 
            var selectPicker = $('.selectpicker');
            $.each(selectPicker, function (i, selectObj) {
                var defVal = $(selectObj).attr('data-val');
                var defValArr = defVal.split(',');
                $(selectObj).selectpicker('val', defValArr);
            });

            //给富文本 创建编辑器
            window.UEs = [];
            if (document.UE_MapAttr) {
                document.UE_MapAttr.forEach(function (item) {
                    var obj = {};
                    //根据字段只读属性 调整外观
                    if (item.MapAttr.UIIsEnable == "0") {
                        obj.editor = UM.getEditor(item.id, {
                            'toolbar': [],
                            'readonly': true,
                            'autoHeightEnabled': false,
                            'fontsize': [10, 12, 14, 16, 18, 20, 24, 36]
                        });
                    } else {
                        document.activeEditor = obj.editor = UM.getEditor(item.id, {
                            'autoHeightEnabled': false,
                            'fontsize': [10, 12, 14, 16, 18, 20, 24, 36]
                        });
                        document.activeEditor.MaxLen = item.MapAttr.MaxLen;
                        document.activeEditor.MinLen = item.MapAttr.MinLen;
                        document.activeEditor.BindField = item.MapAttr.KeyOfEn;
                        document.activeEditor.BindFieldName = item.MapAttr.Name;
                    }
                    obj.attr = item.MapAttr;
                    window.UEs.push(obj);

                    //调整样式,让必选的红色 * 随后垂直居中
                    obj.editor.$container.css({ "display": "inline-block", "margin-right": "10px", "vertical-align": "middle" });
                });
            }
        }
    })
}

//解析表单字段 MapAttr.
function InitMapAttr(Sys_MapAttr, workNodeData, groupID) {

    var html = "";
    var isDropTR = true;
    for (var i = 0; i < Sys_MapAttr.length; i++) {

        var attr = Sys_MapAttr[i];

        if (attr.GroupID != groupID || attr.UIVisible == 0)
            continue;

        var enable = attr.UIIsEnable == "1" ? "" : " ui-state-disabled";
        var defval = ConvertDefVal(workNodeData, attr.DefVal, attr.KeyOfEn);

        var lab = "";
        if (attr.UIContralType == 0)
            lab = "<label for='TB_" + attr.KeyOfEn + "' class='" + (attr.UIIsInput == 1 ? "mustInput" : "") + "'>" + attr.Name + "</label>";

        if (attr.UIContralType == 1)
            lab = "<label for='DDL_" + attr.KeyOfEn + "' class='" + (attr.UIIsInput == 1 ? "mustInput" : "") + "'>" + attr.Name + "</label>";

        if (attr.UIIsInput == 1 && attr.UIIsEnable == 1) {
            lab += " <span style='color:red' class='mustInput' data-keyofen='" + attr.KeyOfEn + "' >*</span>";
        }

//        if (item.UIContralType == 2)
//            lab = "<label for='CB_" + item.KeyOfEn + "' >" + item.Name + "</label>";

        //线性展示并且colspan=3
        if (attr.ColSpan == 3) {
            isDropTR = true;
            html += "<tr>";
            html += "<td  class='FDesc' style='width:120px;'>" + lab + "</td>";
            html += "<td ColSpan=3>";
            html += InitMapAttrOfCtrl(attr, enable, defval);
            html += "</td>";
            html += "</tr>";
            continue;
        }

        //线性展示并且colspan=4
        if (attr.ColSpan == 4) {
            isDropTR = true;
            html += "<tr>";
            html += "<td ColSpan='4'>" + lab + "</br>";
            html += InitMapAttrOfCtrl(attr, enable, defval);
            html += "</td>";
            html += "</tr>";
            continue;
        }

        if (isDropTR == true) {
            html += "<tr>";
            html += "<td class='FDesc' style='width:120px;'>" + lab + "</td>";
            html += "<td class='FContext'  >";
            html += InitMapAttrOfCtrl(attr, enable, defval);
            html += "</td>";
            isDropTR = !isDropTR;
            continue;
        }

        if (isDropTR == false) {
            html += "<td class='FDesc' style='width:120px;'>" + lab + "</td>";
            html += "<td class='FContext'>";
            html += InitMapAttrOfCtrl(attr, enable, defval);
            html += "</td>";
            html += "</tr>";
            isDropTR = !isDropTR;
            continue;
        }
    }
    return html;
}

function InitMapAttrOfCtrl(mapAttr) {

    var str = '';
    var defValue = ConvertDefVal(workNodeData, mapAttr.DefVal, mapAttr.KeyOfEn);

    var isInOneRow = false; //是否占一整行
    var islabelIsInEle = false; //

    var eleHtml = '';

    //外键类型.
    if (mapAttr.LGType == 2) {
     //   return "<select data-val='" + ConvertDefVal(workNodeData, mapAttr.DefVal, mapAttr.KeyOfEn) + "' class='" + isMultiSeleClass + "' " + isMultiSele + " name='DDL_" + mapAttr.KeyOfEn + "' " + (mapAttr.UIIsEnable==1 ? '' : 'disabled="disabled"') + ">" + InitDDLOperation(workNodeData, mapAttr, defValue) + "</select>";
    }

    //添加文本框 ，日期控件等
    //AppString
    if (mapAttr.MyDataType == "1") {  //不是外键

        if (mapAttr.UIHeight <= 23) //普通的文本框.
            return "<input maxlength=" + mapAttr.MaxLen + "  name='TB_" + mapAttr.KeyOfEn + "' style='width:" + mapAttr.UIWidth + ";height:" + mapAttr.UIHeight + ";' type='text' " + (mapAttr.UIIsEnable==1 ? '' : ' disabled="disabled"') + " />";

        if (mapAttr.AtPara && mapAttr.AtPara.indexOf("@IsRichText=1") >= 0) {

            var eleHtml = "";
            //如果是富文本就使用百度 UEditor
            if (document.UE_MapAttr === undefined) {
                document.UE_MapAttr = [];
            }
            var editorPara = {};
            editorPara.id = "container" + document.UE_MapAttr.length;
            editorPara.MapAttr = mapAttr;
            document.UE_MapAttr.push(editorPara);

            //设置编辑器的默认样式
            var styleText = "text-align:left;font-size:12px;";
            styleText += "width:100%;";
            styleText += "height:" + mapAttr.UIHeight + "px;";

            if (mapAttr.UIIsEnable == "0") {
                //字段处于只读状态.注意这里 name 属性也是可以用来绑定字段名字的
                eleHtml += "<script id='" + editorPara.id + "' name='TB_" + mapAttr.KeyOfEn + "' type='text/plain' style='" + styleText + "'>" + defValue + "</script>";
            } else {
                eleHtml += "<script id='" + editorPara.id + "' name='TB_" + mapAttr.KeyOfEn + "' type='text/plain' style='" + styleText + "'></script>";
            }

            eleHtml = "<div style='white-space:normal;'>" + eleHtml + "</div>";
            return eleHtml
        }


        //普通的大块文本.
        return "<textarea maxlength=" + mapAttr.MaxLen + " style='height:" + mapAttr.UIHeight + "px;width:100%;' name='TB_" + mapAttr.KeyOfEn + "' type='text'  " + (mapAttr.UIIsEnable==1 ? '' : ' disabled="disabled"') + " />"
    }
     
    //日期类型.
    if (mapAttr.MyDataType == 6) {
        var enableAttr = '';
        if (mapAttr.UIIsEnable == 1)
            enableAttr = 'onfocus="WdatePicker({dateFmt:' + "'yyyy-MM-dd'})" + '";';
        else
            enableAttr = "disabled='disabled'";

        return "<input " + enableAttr + " style='width:80px;' name='TB_" + mapAttr.KeyOfEn + "' />";
    }

    if (mapAttr.MyDataType == 7) {

        var enableAttr = '';
        if (mapAttr.UIIsEnable == 1)
            enableAttr = 'onfocus="WdatePicker({dateFmt:' + "'yyyy-MM-dd HH:mm'})" + '";';
        else
            enableAttr = "disabled='disabled'";

        return "<input  type='text'  style='width:120px;' " + enableAttr + " name='TB_" + mapAttr.KeyOfEn + "' />";
    }


    if (mapAttr.MyDataType == 4) {  // AppBoolean = 7

        if (mapAttr.UIIsEnable == 1)
            enableAttr = "";
        else
            enableAttr = "disabled='disabled'";

        //CHECKBOX 默认值
        var checkedStr = '';
        if (checkedStr != "true" && checkedStr != '1') {
            checkedStr = ' checked="checked" ';
        }

        checkedStr = ConvertDefVal(workNodeData, '', mapAttr.KeyOfEn);

        return "<input " + enableAttr + " " + (defValue == 1 ? "checked='checked'" : "") + " type='checkbox' id='CB_" + mapAttr.KeyOfEn + "'  name='CB_" + mapAttr.KeyOfEn + "' " + checkedStr + " /><label for='CB_" + mapAttr.KeyOfEn + "' >" + mapAttr.Name + "</label>";
    }

    if (mapAttr.MyDataType == 2 && mapAttr.LGType == 1) { //AppInt Enum
        return "<select name='DDL_" + mapAttr.KeyOfEn + "' " + (mapAttr.UIIsEnable == 1 ? '' : 'disabled="disabled"') + ">" + InitDDLOperation(workNodeData, mapAttr, defValue) + "</select>";
    }

    // AppDouble  AppFloat
    if (mapAttr.MyDataType == 5 || mapAttr.MyDataType == 3) {

        var enableAttr = '';
        if (mapAttr.UIIsEnable != 1)
            enableAttr = "disabled='disabled'";

        return "<input style='text-align:right;width:80px;'  onkeyup=" + '"' + "if(isNaN(value))execCommand('undo')" + '"' + " onafterpaste=" + '"' + "if(isNaN(value))execCommand('undo')" + '"' + " maxlength=" + mapAttr.MaxLen / 2 + "   type='text'" + enableAttr + " name='TB_" + mapAttr.KeyOfEn + "'/>";
    }

    if ((mapAttr.MyDataType == 2)) {//AppInt
        var enableAttr = '';
        if (mapAttr.UIIsEnable != 1) {
            enableAttr = "disabled='disabled'";
        }
        return "<input style='text-align:right;width:80px;' onkeyup=" + '"' + "if(isNaN(value) || (value%1 !== 0))execCommand('undo')" + '"' + " onafterpaste=" + '"' + "if(isNaN(value) || (value%1 !== 0))execCommand('undo')" + '"' + " maxlength=" + mapAttr.MaxLen / 2 + "   type='text'" + enableAttr + " name='TB_" + mapAttr.KeyOfEn + "'/>";
    }

    //AppMoney  AppRate
    if (mapAttr.MyDataType == 8) {
        var enableAttr = '';
        if (mapAttr.UIIsEnable == 1) {

        } else {
            enableAttr = "disabled='disabled'";
        }
        return "<input style='text-align:right;width:80px;' onkeyup=" + '"' + "if(isNaN(value))execCommand('undo')" + '"' + " onafterpaste=" + '"' + "if(isNaN(value))execCommand('undo')" + '"' + " maxlength=" + mapAttr.MaxLen / 2 + "   type='text'" + enableAttr + " name='TB_" + mapAttr.KeyOfEn + "'/>";
    }

    alert(mapAttr.Name + "的类型没有判断.");
    return;
}

var workNodeData = {}; 
  

function ImgAth(url, athMyPK) {
    var v = window.showModalDialog(url, 'ddf', 'dialogHeight: 650px; dialogWidth: 950px;center: yes; help: no');
    if (v == null)
        return;
    document.getElementById('Img' + athMyPK).setAttribute('src', v);
}

//初始化 IMAGE附件
function figure_Template_ImageAth(frmImageAth) {
    var isEdit = frmImageAth.IsEdit;
    var eleHtml = $("<div></div>");
    var img = $("<img/>");

    var imgSrc = "/WF/Data/Img/LogH.PNG";
    //获取数据
    if (workNodeData.Sys_FrmImgAthDB) {
        $.each(workNodeData.Sys_FrmImgAthDB, function (i, obj) {
            if (obj.FK_FrmImgAth == frmImageAth.MyPK) {
                imgSrc = obj.FileFullName;
            }
        });
    }
    //设计属性
    img.attr('id', 'Img' + frmImageAth.MyPK).attr('name', 'Img' + frmImageAth.MyPK);
    img.attr("src", imgSrc).attr('onerror', "this.src='/WF/Data/Img/LogH.PNG'");
    img.css('width', frmImageAth.W).css('height', frmImageAth.H).css('padding', "0px").css('margin', "0px").css('border-width', "0px");
    //不可编辑
    if (isEdit == "1") {
        var fieldSet = $("<fieldset></fieldset>");
        var length = $("<legend></legend>");
        var a = $("<a></a>");
        var url = "/WF/CCForm/ImgAth.aspx?W=" + frmImageAth.W + "&H=" + frmImageAth.H + "&FK_MapData=ND" + pageData.FK_Node + "&MyPK=" + pageData.WorkID + "&ImgAth=" + frmImageAth.MyPK;

        a.attr('href', "javascript:ImgAth('" + url + "','" + frmImageAth.MyPK + "');").html("编辑");
        length.css('font-style', 'inherit').css('font-weight', 'bold').css('font-size', '12px');

        fieldSet.append(length);
        length.append(a);
        fieldSet.append(img);
        eleHtml.append(fieldSet);
    } else {
        eleHtml.append(img);
    }
    eleHtml.css('position', 'absolute').css('top', frmImageAth.Y).css('left', frmImageAth.X);
    return eleHtml;
}


//审核组件
function figure_Template_FigureFrmCheck(wf_node) {

    //审核组键FWCSta Sta,FWC_X X,FWC_Y Y,FWC_H H, FWC_W W from WF_Node
    var sta = wf_node.FWCSta;

    var h = wf_node.FWC_H;
    if (h == 0)
        h = 300;


    var src = "./WorkOpt/WorkCheck.htm?s=2";
    var fwcOnload = "";
    var paras = '';

    paras += "&FID=" + pageData["FID"];
    paras += "&OID=" + pageData["WorkID"];
    paras += '&FK_Flow=' + pageData.FK_Flow;
    paras += '&FK_Node=' + pageData.FK_Node;
    paras += '&WorkID=' + pageData.WorkID;
    if (sta == 2)//只读
    {
        src += "&DoType=View";
    }
    src += "&r=q" + paras;
    var eleHtml = "<iframe width='100%' height='" + h + "px' id='FWC' src='" + src + "' frameborder=0  leftMargin='0'  topMargin='0' scrolling=auto></iframe>";
    return eleHtml;
}

//初始化 附件
function figure_Template_Attachment(workNodeData, gf) {

    var ath = workNodeData.Sys_FrmAttachment[0];

    var eleHtml = '';
    if (ath.UploadType == 0) { //单附件上传 L4204
        return '';
    }
    var src = "";
    if (pageData.IsReadonly)
        src = "./CCForm/AttachmentUpload.htm?PKVal=" + pageData.WorkID + "&Ath=" + ath.NoOfObj + "&FK_MapData=" + ath.FK_MapData + "&FK_FrmAttachment=" + ath.MyPK + "&IsReadonly=1";
    else
        src = "./CCForm/AttachmentUpload.htm?PKVal=" + pageData.WorkID + "&Ath=" + ath.NoOfObj + "&FK_MapData=" + ath.FK_MapData + "&FK_FrmAttachment=" + ath.MyPK;

    eleHtml += "<iframe style='width:100%;height:" + ath.H + "px;' ID='Attach_" + ath.MyPK + "'    src='" + src + "' frameborder=0  leftMargin='0'  topMargin='0' scrolling=auto></iframe>" + '</div>';


    return eleHtml;
}

function addLoadFunction(id, eventName, method) {
    var js = "";
    js = "<script type='text/javascript' >";
    js += "function F" + id + "load() { ";
    js += "if (document.all) {";
    js += "document.getElementById('F" + id + "').attachEvent('on" + eventName + "',function(event){" + method + "('" + id + "');});";
    js += "} ";

    js += "else { ";
    js += "document.getElementById('F" + id + "').contentWindow.addEventListener('" + eventName + "',function(event){" + method + "('" + id + "');}, false); ";
    js += "} }";

    js += "</script>";
    return $(js);
}

var appPath = "../../";
var DtlsCount = " + dtlsCount + "; //应该加载的明细表数量

//初始化从表
function figure_Template_Dtl(frmDtl) {
    var eleHtml = $("<DIV id='Fd" + frmDtl.No + "' style='position:absolute; left:" + frmDtl.X + "px; top:" + frmDtl.Y + "px; width:" + frmDtl.W + "px; height:" + frmDtl.H + "px;text-align: left;' >");
    var paras = this.pageData;
    var strs = "";
    for (var str in paras) {
        if (str == "EnsName" || str == "RefPKVal" || str == "IsReadonly")
            continue
        else
            strs += "&" + str + "=" + paras[str];
    }
    var src = "";
    var href = window.location.href;
    var urlParam = href.substring(href.indexOf('?') + 1, href.length);
    urlParam = urlParam.replace('&DoType=', '&DoTypeDel=xx');
    if (frmDtl.DtlShowModel == "0") {
        if (pageData.IsReadOnly) {

            src = "./CCForm/Dtl.htm?EnsName=" + frmDtl.No + "&RefPKVal=" + this.pageData.WorkID + "&IsReadonly=1&" + urlParam + "&Version=" + load.Version;
        } else {
            src = "./CCForm/Dtl.htm?EnsName=" + frmDtl.No + "&RefPKVal=" + this.pageData.WorkID + "&IsReadonly=0&" + urlParam + "&Version=" + load.Version;
        }
    }
    else if (frmDtl.DtlShowModel == "1") {
        if (pageData.IsReadOnly)
            src = appPath + "WF/CCForm/DtlCard.htm?EnsName=" + frmDtl.No + "&RefPKVal=" + this.pageData.WorkID + "&IsReadonly=1" + strs;
        else
            src = appPath + "WF/CCForm/DtlCard.htm?EnsName=" + frmDtl.No + "&RefPKVal=" + this.pageData.WorkID + "&IsReadonly=0" + strs;

    }
    var eleIframe = '<iframe></iframe>';
    eleIframe = $("<iframe class='Fdtl' ID='F" + frmDtl.No + "' src='" + src +
                 "' frameborder=0  style='position:absolute;width:100%; height:" + frmDtl.H +
                 "px;text-align: left;'  leftMargin='0'  topMargin='0' scrolling=auto /></iframe>");
    if (pageData.IsReadOnly) {

    } else {
        if (frmDtl.DtlSaveModel == 0) {
            eleHtml.append(addLoadFunction(frmDtl.No, "blur", "SaveDtl"));
            eleIframe.attr('onload', frmDtl.No + "load()");
        }
    }
    eleHtml.append(eleIframe);
    //added by liuxc,2017-1-10,此处前台JS中增加变量DtlsLoadedCount记录明细表的数量，用于加载完全部明细表的判断
    var js = "";
    if (pageData.IsReadonly==false) {
        js = "<script type='text/javascript' >";
        js += " function SaveDtl(dtl) { ";
        js += "   GenerPageKVs(); //调用产生kvs ";
        js += "\n   var iframe = document.getElementById('F' + dtl );";
        js += "   if(iframe && iframe.contentWindow){ ";
        js += "      iframe.contentWindow.SaveDtlData(); ";
        js += "   } ";
        js += " } ";
        js += " function SaveM2M(dtl) { ";
        js += "   document.getElementById('F' + dtl ).contentWindow.SaveM2M();";
        js += "} ";
        js += "</script>";
        eleHtml.append($(js));
    }
    return eleHtml;
}

//初始化轨迹图
function figure_Template_FigureFlowChart(wf_node) {

    //轨迹图
    var sta = wf_node.FrmTrackSta;
    var x = wf_node.FrmTrack_X;
    var y = wf_node.FrmTrack_Y;
    var h = wf_node.FrmTrack_H;
    var w = wf_node.FrmTrack_W;

    if (sta == 0) {
        return $('');
    }

    if (sta == undefined) {
        return;
    }

    var src = "./WorkOpt/OneWork/OneWork.htm?CurrTab=Track";
    src += '&FK_Flow=' + pageData.FK_Flow;
    src += '&FK_Node=' + pageData.FK_Node;
    src += '&WorkID=' + pageData.WorkID;
    src += '&FID=' + pageData.FID;
    var eleHtml = '<div id="divtrack' + wf_node.NodeID + '">' + "<iframe id='track" + wf_node.NodeID + "' style='width:" + w + "px;height=" + h + "px;'    src='" + src + "' frameborder=0  leftMargin='0'  topMargin='0' scrolling=auto></iframe>" + '</div>';
    eleHtml = $(eleHtml);
    eleHtml.css('position', 'absolute').css('top', y).css('left', x).css('width', w).css('height', h);

    return eleHtml;
}
 

//子线程
function figure_Template_FigureThreadDtl(wf_node) {

    //FrmThreadSta Sta,FrmThread_X X,FrmThread_Y Y,FrmThread_H H,FrmThread_W
    var sta = wf_node.FrmThreadSta;
    var x = wf_node.FrmThread_X;
    var y = wf_node.FrmThread_Y;
    var h = wf_node.FrmThread_H;
    var w = wf_node.FrmThread_W;
    if (sta == 0 || sta == '0')
        return $('');

    var src = "./WorkOpt/Thread.htm?s=2";
    var fwcOnload = "";
    var paras = '';

    paras += "&FID=" + pageData["FID"];
    paras += "&OID=" + pageData["WorkID"];
    paras += '&FK_Flow=' + pageData.FK_Flow;
    paras += '&FK_Node=' + pageData.FK_Node;
    paras += '&WorkID=' + pageData.WorkID;

    if (sta == 2) //只读
    {
        src += "&DoType=View";
    }
    else {
        fwcOnload = "onload= 'WC" + wf_node.NodeID + "load();'";
        $('body').append(addLoadFunction("WC" + wf_node.NodeID, "blur", "SaveDtl"));
    }
    src += "&r=q" + paras;
    var eleHtml = '<div id=DIVFT' + wf_node.NodeID + '>' + "<iframe id=FFT" + wf_node.NodeID + " style='width:100%;height:" + h + "px;'    src='" + src + "' frameborder=0  leftMargin='0'  topMargin='0' scrolling=auto></iframe>" + '</div>';
    eleHtml = $(eleHtml);
    eleHtml.css('position', 'absolute').css('top', y).css('left', x).css('width', w).css('height', h);

    return eleHtml;
}

//子流程
function figure_Template_FigureSubFlowDtl(wf_node) {
    //SFSta Sta,SF_X X,SF_Y Y,SF_H H, SF_W W
    var sta = wf_node.SFSta;
    var x = wf_node.SF_X;
    var y = wf_node.SF_Y;
    var h = wf_node.SF_H;
    var w = wf_node.SF_W;
    if (sta == 0)
        return $('');

    var src = "./WorkOpt/SubFlow.htm?s=2";
    var fwcOnload = "";
    var paras = '';

    paras += "&FID=" + pageData["FID"];
    paras += "&OID=" + pageData["WorkID"];
    paras += '&FK_Flow=' + pageData.FK_Flow;
    paras += '&FK_Node=' + pageData.FK_Node;
    paras += '&WorkID=' + pageData.WorkID;
    if (sta == 2)//只读
    {
        src += "&DoType=View";
    }
    else {
        fwcOnload = "onload= 'WC" + wf_node.NodeID + "load();'";
        $('body').append(addLoadFunction("WC" + wf_node.NodeID, "blur", "SaveDtl"));
    }
    src += "&r=q" + paras;
    var eleHtml = '<div id=DIVWC' + wf_node.NodeID + '>' + "<iframe id=FSF" + wf_node.NodeID + " style='width:" + w + "px';height:" + h + "px'    src='" + src + "' frameborder=0  leftMargin='0'  topMargin='0' scrolling=auto></iframe>" + '</div>';
    eleHtml = $(eleHtml);
    eleHtml.css('position', 'absolute').css('top', y).css('left', x).css('width', w).css('height', h);

    return eleHtml;
}

//初始化框架
function figure_Template_IFrame(fram) {
    var eleHtml = '';
    var src = dealWithUrl(fram.src) + "IsReadOnly=0";
    eleHtml = $('<div id="iframe' + fram.MyPK + '">' + '</div>');
    var iframe = $(+"<iframe  style='width:" + fram.W + "px; height:" + fram.H + "'     src='" + src + "' frameborder=0  leftMargin='0'  topMargin='0' scrolling='no'></iframe>");

    eleHtml.css('position', 'absolute').css('top', fram.Y).css('left', fram.X).css('width', fram.W).css('height', fram.H);
    return frameHtml;
}

function figure_Template_MsgAlert(msgAlert, i) {
    var eleHtml = $('<div></div>');
    var titleSpan = $('<span class="titleAlertSpan"> ' + (parseInt(i) + 1) + "&nbsp;&nbsp;&nbsp;" + msgAlert.Title + '</span>');
    var msgDiv = $('<div>' + msgAlert.Msg + '</div>');
    eleHtml.append(titleSpan).append(msgDiv)
    return eleHtml;
}

//处理URL，MainTable URL 参数 替换问题
function dealWithUrl(src) {
    var src = fram.URL.replace(new RegExp(/(：)/g), ':');
    var params = '&FID=' + pageData.FID;
    params += '&WorkID=' + pageData.WorkID;
    if (src.indexOf("?") > 0) {
        var params = getQueryStringFromUrl(src);
        if (params != null && params.length > 0) {
            $.each(params, function (i, param) {
                if (param.indexOf('@') == 0) {//是需要替换的参数
                    paramArr = param.split('=');
                    if (paramArr.length == 2 && paramArr[1].indexOf('@') == 0) {
                        if (paramArr[1].indexOf('@WebUser.') == 0) {
                            params[i] = paramArr[0].substring(1) + "=" + workNodeData.MainTable[0][paramArr[1].substr('@WebUser.'.length)];
                        }
                        if (workNodeData.MainTable[0][paramArr[1].substr(1)] != undefined) {
                            params[i] = paramArr[0].substring(1) + "=" + workNodeData.MainTable[0][paramArr[1].substr(1)];
                        }

                        //使用URL中的参数
                        var pageParams = getQueryString();
                        var pageParamObj = {};
                        $.each(pageParams, function (i, pageParam) {
                            if (pageParam.indexOf('@') == 0) {
                                var pageParamArr = pageParam.split('=');
                                pageParamObj[pageParamArr[0].substring(1, pageParamArr[0].length)] = pageParamArr[1];
                            }
                        });
                        var result = "";
                        //通过MAINTABLE返回的参数
                        for (var ele in workNodeData.MainTable[0]) {
                            if (paramArr[0].substring(1) == ele) {
                                result = workNodeData.MainTable[0][ele];
                                break;
                            }
                        }
                        //通过URL参数传过来的参数
                        for (var pageParam in pageParamObj) {
                            if (pageParam == paramArr[0].substring(1)) {
                                result = pageParamObj[pageParam];
                                break;
                            }
                        }

                        if (result != '') {
                            params[i] = paramArr[0].substring(1) + "=" + unescape(result);
                        }
                    }
                }
            });
            src = src.substr(0, src.indexOf('?')) + "?" + params.join('&');
        }
    }
    else {
        src += "?q=1";
    }
    return src;
}

var colVisibleJsonStr = ''
var jsonStr = '';

//从MyFlowFree2017.htm 中拿过过的.

var pageData = {};
var globalVarList = {};

$(function () {

    var frm = document.forms["divCCForm"];

    if (plant == "CCFlow")
        frm.action = "MyFlow.ashx?method=login";
    else
        frm.action = MyFlow + "?method=login";

    initPageParam(); //初始化参数

    initBar(); //工具栏.ajax

    GenerWorkNode(); //表单数据.ajax


    if ($("#Message").html() == "") {
        $(".Message").hide();
    }

    if (parent != null && parent.document.getElementById('MainFrames') != undefined) {
        //计算高度，展示滚动条
        var height = $(parent.document.getElementById('MainFrames')).height() - 110;
        //$('#topContentDiv').height(height);

        $(window).resize(function () {
            $("#CCForm").height($(window).height() - 115 + "px").css("overflow-y", "auto").css("scrollbar-face-color", "#fff"); ;
        });
    }
    else {//新加
        //计算高度，展示滚动条
        var height = $("#CCForm").height($(window).height() - 115 + "px").css("overflow-y", "auto").css("scrollbar-face-color", "#fff");
        // $('#topContentDiv').height(height);

        $(window).resize(function () {
            $("#CCForm").height($(window).height() - 115 + "px").css("overflow-y", "auto").css("scrollbar-face-color", "#fff"); ;
        });
    }

    $('#btnCloseMsg').bind('click', function () {
        $('.Message').hide();
    });
})