﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="CCFlow.WF.MapDef.Comm_MapDef_EditSQL" Codebehind="EditSQL.aspx.cs" %>
<%@ Register src="Pub.ascx" tagname="Pub" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>编辑外键表字段</title>
	<script language="JavaScript" src="../Comm/JScript.js" ></script>
    <script language=javascript>
    /* ESC Key Down  */
    function Esc()
    {
        if (event.keyCode == 27)    
            window.close();
       return true;
   }
   function RSize() {
       return;
       if (document.body.scrollWidth > (window.screen.availWidth - 100)) {
           window.dialogWidth = (window.screen.availWidth - 100).toString() + "px"
       } else {
           window.dialogWidth = (document.body.scrollWidth + 50).toString() + "px"
       }

       if (document.body.scrollHeight > (window.screen.availHeight - 70)) {
           window.dialogHeight = (window.screen.availHeight - 50).toString() + "px"
       } else {
           window.dialogHeight = (document.body.scrollHeight + 115).toString() + "px"
       }
       window.dialogLeft = ((window.screen.availWidth - document.body.clientWidth) / 2).toString() + "px"
       window.dialogTop = ((window.screen.availHeight - document.body.clientHeight) / 2).toString() + "px"
   }
    </script>
    <base target=_self /> 
    <link href="../Comm/Style/Table0.css" rel="stylesheet" type="text/css" />
</head>
<body    topmargin="0" leftmargin="0" onkeypress="Esc()" onload="RSize()"  >
    <form id="form1" runat="server">
    <div align=center width='90%'>
        <uc2:Pub ID="Pub1" runat="server" />
    </div>
    </form>
</body>
</html>
