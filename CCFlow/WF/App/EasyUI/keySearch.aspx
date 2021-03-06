<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="keySearch.aspx.cs" Inherits="CCFlow.AppDemoLigerUI.keySearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="jquery/lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="jquery/tablestyle.css" rel="stylesheet" type="text/css" />
    <link href="jquery/lib/ligerUI/skins/ligerui-icons.css" rel="stylesheet" type="text/css" />
    <script src="jquery/lib/jquery/jquery-1.5.2.min.js" type="text/javascript"></script>
    <script src="jquery/lib/ligerUI/js/core/base.js" type="text/javascript"></script>
    <script src="jquery/lib/ligerUI/js/ligerui.all.js" type="text/javascript"></script>
    <script src="jquery/lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="jquery/lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="jquery/lib/ligerUI/js/plugins/ligerButton.js" type="text/javascript"></script>
    <script src="jquery/lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>
    <script src="jquery/lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="js/AppData.js" type="text/javascript"></script>
    <script src="js/KeySearch.js" type="text/javascript"></script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div id="pageloading">
    </div>
    <div id="toptoolbar">
    </div>
    <div id="maingrid" style="margin: 0; padding: 0;">
    </div>
    <div id="showKey" style="display:block">
        <b>输入关键字:</b>
        <input type="text" id="txtKey"/>
        <input type="checkbox" id="cbkQueryType"/>仅查询我参与的流程
    </div>
    </form>
</body>
</html>
