﻿<%@ Page Title="" Language="C#" MasterPageFile="~/WF/Admin/CCFormDesigner/Site.Master"
    AutoEventWireup="true" CodeBehind="Tables.aspx.cs" Inherits="CCFlow.WF.Admin.CCFormDesigner.Tables" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Comm/JScript.js" type="text/javascript"></script>
    <link href="../../Scripts/easyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/easyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/easyUI/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../../Scripts/easyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Del(refNo) {

            if (window.confirm('您确定要删除吗？') == false)
                return;
            window.location.href = '?DoType=Del&RefNo=' + refNo;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="easyui-layout" style="width: 100%;">
        <div style="float: left">
            数据源表： <a href="javascript:WinOpen('/WF/Admin/FoolFormDesigner/CreateSFGuide.htm?DoType=New&FromApp=SL')">
                新建</a>
        </div>
        <tr>
            <th width="3%">
                序
            </th>
            <th>
                数据源
            </th>
            <th>
                表名
            </th>
            <th>
                中文名
            </th>
            <th>
                数据结构
            </th>
            <th>
                查看引用
            </th>
            <th>
                编辑
            </th>
            <th>
                数据
            </th>
            <th>
                删除
            </th>
        </tr>
        <%
    
            //删除数据.
            if (this.Request.QueryString["DoType"] == "Del")
            {
                BP.Sys.SFTable mytab = new BP.Sys.SFTable();
                mytab.No = this.Request.QueryString["RefNo"];
                mytab.Delete();
            }


            BP.Sys.SFTables tabs = new BP.Sys.SFTables();
            tabs.RetrieveAll();

            int idx = 0;
            string icon = "";
            foreach (BP.Sys.SFTable tab in tabs)
            {
                idx++;

                if (tab.No.Contains("BP.") == false)
                {
                    icon = "./Img/DBSrcTable.png";
                }
                else
                {
                    icon = "./Img/Form.png";
                }
        
        %>
        <tr onmouseover='TROver(this)' onmouseout='TROut(this)'>
            <td class="Idx">
                <%=idx %>
            </td>
            <td>
                <%=tab.FK_SFDBSrcT %>
            </td>
            <td>
                <%=tab.No %>
            </td>
            <td>
                <img src='<%=icon %>' height="17" width="17" />
                <%=tab.Name %>
            </td>
            <td>
                <%=tab.CodeStructT%>
            </td>
            <%
                int refNum = BP.DA.DBAccess.RunSQLReturnValInt("SELECT COUNT(KeyOfEn) FROM Sys_MapAttr WHERE UIBindKey='" + tab.No + "'", 0);
                string delLink = "";
                if (refNum == 0)
                    delLink = "<a href=\"javascript:Del('" + tab.No + "')\">删除</a>";

                string editDBLink = "无";
                //edited by liuxc,2016-12-29,只显示创建表类型数据源可以编辑数据，SQL查询、外键表/视图，一律使用直接查询方式，不再创建表
                if (tab.SrcType == BP.Sys.SrcType.CreateTable)
                {
                    int dbNum = new BP.Sys.SFDBSrc(tab.FK_SFDBSrc).RunSQLReturnInt("SELECT COUNT(*) FROM " + tab.No + " ", 0);
                    editDBLink = "<a href=\"javascript:WinOpen('/WF/Admin/FoolFormDesigner/SFTableEditData.aspx?FK_SFTable=" + tab.No + "')\">编辑(" + dbNum + ")</a>";
                }
        
    //int refNum = tab.db("SELECT COUNT(KeyOfEn) FROM Sys_MapAttr WHERE UIBindKey='" + tab.No + "'", 0);
    //int dataNum=
        
            %>
            <td>
                <a href="javascript:WinOpen('/WF/Admin/CCFormDesigner/TableRef.aspx?RefNo=<%=tab.No %>&FromApp=SL')">
                    引用(<%=refNum %>)</a>
            </td>
            <td>
                <a href="javascript:WinOpen('/WF/Admin/FoolFormDesigner/SFTable.aspx?RefNo=<%=tab.No %>&FromApp=SL')">
                    编辑属性</a>
            </td>
            <td>
                <%=editDBLink %>
            </td>
            <td>
                <%=delLink %>
            </td>
        </tr>
        <%
            }
        %>
    </table>
</asp:Content>
