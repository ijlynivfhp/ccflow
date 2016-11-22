﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="CCFlow.WF.MapDef.Comm_MapDef_MapDtlDe"
    CodeBehind="MapDtlDe.aspx.cs" %>

<%@ Register Assembly="BP.Web.Controls" Namespace="BP.Web.Controls" TagPrefix="cc1" %>
<%@ Register Src="Pub.ascx" TagName="Pub" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>表单设计</title>
    <script language="JavaScript" src="../../Comm/JScript.js" type="text/javascript" ></script>
    <script language="JavaScript" src="../../Comm/JS/Calendar/WdatePicker.js" defer="defer" type="text/javascript" ></script>
    <base target="_self" />
    <script language="javascript">
        function Insert(fk_mapdata, IDX) {
            var url = 'FieldTypeList.htm?FK_MapData=' + fk_mapdata + '&IDX=' + IDX;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 400px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = window.location.href;
        }
        function AddF(fk_mapdata) {
            var url = 'FieldTypeList.htm?FK_MapData=' + fk_mapdata;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = window.location.href;
        }
        function AddFGroup(fk_mapdata) {
            var url = 'Do.aspx?DoType=AddFGroup&FK_MapData=' + fk_mapdata;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = window.location.href;
        }
        function CopyF(fk_mapdata) {
            var url = 'CopyDtlField.htm?FK_MapData=' + fk_mapdata + '&s=' + Math.random();
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 600px; dialogWidth: 800px;center: yes; help: no');
            window.location.href = window.location.href;
        }

        function HidAttr(mypk) {
            var url = 'HidAttr.htm?FK_MapData=' + mypk + '&s=' + Math.random();
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 600px; dialogWidth: 800px;center: yes; help: no');
            //  window.location.href = window.location.href;
        }
        function Edit(fk_mapdata, mypk, ftype) {
            var url = 'EditF.htm?DoType=Edit&FK_MapData=' + fk_mapdata + '&MyPK=' + mypk + '&FType=' + ftype + '&s=' + Math.random();
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = window.location.href;
        }
        function EditEnum(fk_mapdata, mypk, enumKey) {
            var url = 'EditEnum.htm?DoType=Edit&FK_MapData=' + fk_mapdata + '&MyPK=' + mypk + '&EnumKey=' + enumKey + '&s=' + Math.random();
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = window.location.href;
        }
        function EditTable(fk_mapdata, mypk,sfTable) {
            var url = 'EditTableField.htm?DoType=Edit&FK_MapData=' + fk_mapdata + '&MyPK=' + mypk + '&FK_SFTable=' + sfTable + '&s=' + Math.random();
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 500px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = window.location.href;
        }

        function Up(fk_mapdata, mypk, toidx) {
            var url = 'Do.aspx?DoType=Up&FK_MapData=' + fk_mapdata + '&MyPK=' + mypk + "&IsDtl=1&ToIdx=" + toidx;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 400px; dialogWidth: 600px;center: yes; help: no');
            //window.location.href ='Designer.aspx?PK='+mypk+'&IsOpen=1';
            window.location.href = window.location.href;
        }
        function Down(fk_mapdata, mypk, toidx) {
            var url = 'Do.aspx?DoType=Down&FK_MapData=' + fk_mapdata + '&MyPK=' + mypk + "&IsDtl=1&Ds=" + toidx;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 400px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = window.location.href;
        }
        function Del(fk_mapdata, refNo) {
            if (window.confirm('您确定要删除吗？') == false)
                return;

            var url = 'Do.aspx?DoType=Del&MyPK=' + fk_mapdata + '&RefNo=' + refNo;
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 400px; dialogWidth: 600px;center: yes; help: no');
            window.location.href = window.location.href;
        }
        function DtlMTR(MyPK) {
            var url = 'MapDtlMTR.aspx?MyPK=' + MyPK + '&s=' + Math.random();
            var b = window.showModalDialog(url, 'ass', 'dialogHeight: 350px; dialogWidth: 550px;center: yes; help: no');
            window.location.href = window.location.href;
        }
        function Esc() {
            if (event.keyCode == 27)
                window.close();
            return true;
        }
        function Attachment(fk_mapdtl) {
            var url = 'Attachment.aspx?IsBTitle=1&PKVal=0&FK_MapData=' + fk_mapdtl + '&FK_FrmAttachment=' + fk_mapdtl + '_AthMDtl&Ath=AthMDtl&s=' + Math.random();
            window.showModalDialog(url, 'xx','dialogWidth=750px;dialogHeight=700px');
        }
        function MapM2M(fk_mapdtl) {
            window.showModalDialog('MapM2M.aspx?NoOfObj=M2M&PKVal=0&FK_MapData=' + fk_mapdtl + '&FK_FrmAttachment=' + fk_mapdtl + '_AthMDtl&Ath=AthMDtl&s=' + Math.random());
        }
    </script>
    <script language="javascript" for="document" event="onkeydown" type="text/javascript">
//    if(event.keyCode==13)
//       event.keyCode=9;
    </script>
    <script language="javascript" type="text/javascript">
        // row主键信息 .
        var rowPK = null;
        // ccform 为开发者提供的内置函数.
        // 获取DDL值.
        function ReqDDL(ddlID) {
            var v = document.getElementById('Pub1_DDL_' + ddlID + "_" + rowPK).value;
            if (v == null) {
                alert('没有找到ID=' + ddlID + '的下拉框控件.');
            }
            return v;
        }
        // 获取TB值
        function ReqTB(tbID) {
            var v = document.getElementById('Pub1_TB_' + tbID + "_" + rowPK).value;
            if (v == null) {
                alert('没有找到ID=' + tbID + '的文本框控件.');
            }
            return v;
        }
        // 获取CheckBox值
        function ReqCB(cbID) {
            var v = document.getElementById('Pub1_CB_' + cbID + "_" + rowPK).value;
            if (v == null) {
                alert('没有找到ID=' + cbID + '的单选控件.');
            }
            return v;
        }

        /// 获取DDL Obj
        function ReqDDLObj(ddlID) {
            var v = document.getElementById('Pub1_DDL_' + ddlID + "_" + rowPK);
            if (v == null) {
                alert('没有找到ID=' + ddlID + '的下拉框控件.');
            }
            return v;
        }
        // 获取TB Obj
        function ReqTBObj(tbID) {
            var v = document.getElementById('Pub1_TB_' + tbID + "_" + rowPK);
            if (v == null) {
                alert('没有找到ID=' + tbID + '的文本框控件.');
            }
            return v;
        }
        // 获取CheckBox Obj值
        function ReqCBObj(cbID) {
            var v = document.getElementById('Pub1_CB_' + cbID + "_" + rowPK);
            if (v == null) {
                alert('没有找到ID=' + cbID + '的单选控件.');
            }
            return v;
        }

        // 设置控件值.
        function SetCtrlVal(ctrlID, val) {
            document.getElementById('Pub1_TB_' + ctrlID + "_" + rowPK).value = val;
            document.getElementById('Pub1_DDL_' + ctrlID + "_" + rowPK).value = val;
            document.getElementById('Pub1_CB_' + ctrlID + "_" + rowPK).value = val;
        }
    </script>
    <link href="../../Comm/Style/Table0.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/easyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/easyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/easyUI/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../../Scripts/easyUI/jquery.easyui.min.js" type="text/javascript"></script>
</head>
<body  onkeypress="Esc()">
    <form id="form1" runat="server">
        <uc1:Pub ID="Pub1" runat="server" />
    </form>
</body>
</html>
