<%@ Page Language="C#" AutoEventWireup="true" Inherits="CCFlow.WF.MapDef.Comm_MapDef_EditEnum" Codebehind="EditEnum.aspx.cs" %>
<%@ Register src="Pub.ascx" tagname="Pub" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>编辑枚举</title>
	<script language="JavaScript" src="../Comm/JScript.js" ></script>
    <script language=javascript>
    /* ESC Key Down  */
    function Esc()
    {
        if (event.keyCode == 27)    
            window.close();
       return true;
    }
    </script>
    <base target=_self /> 
    <link href="../Comm/Style/Table0.css" rel="stylesheet" type="text/css" />
</head>
<body    topmargin="0" leftmargin="0" onkeypress="Esc()"   >
    <form id="form1" runat="server">
    <div align=left width='90%'>
        <uc1:Pub ID="Pub1" runat="server" />
    </div>
    </form>
</body>
</html>
