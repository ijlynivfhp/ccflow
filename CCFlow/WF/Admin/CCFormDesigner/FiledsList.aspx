<%@ Page Title="表单字段列表" Language="C#" MasterPageFile="~/WF/Admin/CCFormDesigner/Site.Master" AutoEventWireup="true" CodeBehind="FiledsList.aspx.cs" Inherits="CCFlow.WF.Admin.CCFormDesigner.FiledsList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<!-- editer: liuhui -->

<table style="width:100%" >
<caption> 字段列表 </caption>
<tr>
<th>序</th>
<th>字段名</th>
<th>中文名</th>
<th>类型</th>
<th>长度</th>
<th>关联</th>
<th>扩展设置</th>
<th>数据质量</th>
<th>编辑</th>
</tr>

<tr>
<th colspan=9 >普通字段  </th>
</tr>


<tr>
<td class=Idx >1</td>
<td>No</td>
<td>编号</td>
<td>String</td>
<td>min=1,max=20</td>
<td> - </td>
<td> - </td>
<td>[<a href="javascript:WinOpen('../EditF.aspx?')">编辑]</td>
</tr>

<tr>
<td class=Idx >2</td>
<td>Name</td>
<td>名称</td>
<td>String</td>
<td>min=1,max=20</td>
<td> - </td>
<td> - </td>
<td> - </td>
<td>[<a href="javascript:WinOpen('../EditF.aspx?')">编辑]</td>
</tr>


<tr>
<th colspan=9 >枚举字段  </th>
</tr>

<tr>
<td class=Idx >2</td>
<td>XB</td>
<td>性别</td>
<td>Int</td>
<td> - </td>
<td><a href="javascript:EditEum('')">@0=女@1=男</a></td>

<td> - </td>

<td> - </td>

<td>[<a href="javascript:WinOpen('../EditF.aspx?')">编辑]</td>
</tr>

<tr>
<th colspan=9 >外键字段  </th>
</tr>
</table>

</asp:Content>
