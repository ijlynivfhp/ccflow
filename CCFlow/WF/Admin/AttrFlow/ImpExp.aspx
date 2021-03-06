<%@ Page Title="模版导入导出" Language="C#" MasterPageFile="../WinOpen.master" AutoEventWireup="true" CodeBehind="ImpExp.aspx.cs" Inherits="CCFlow.WF.Admin.AttrFlow.ImpExp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <link href="../../Scripts/easyUI/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../Scripts/easyUI/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/easyUI/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../Scripts/CommonUnite.js" type="text/javascript"></script>
    <script src="../../Comm/JScript.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- editer by xushuhao -->
<%
    BP.WF.Flow fl = new BP.WF.Flow(this.Request.QueryString["FK_Flow"]);
  %>
<table style=" width:100%">
<caption>流程[ <%=fl.Name%> ]模版导入导出 </caption>

<tr>
<td valign="top" style="width:30%;"> 

<fieldset>
<legend><img src="../../Img/Btn/Help.gif" />关于流程模版</legend>
<ol>
<li>ccbpm生成的流程模版是一个特定格式的xml文件。</li>
<li>它是流程引擎模版与表单引擎模版的完整的组合体。</li>
<li>ccbpm的jflow与ccflow的流程引擎导出的流程模版通用。</li>
<li>流程模版用于流程设计者的作品交换。</li>
<li>在实施的过程中，我们可以把一个系统上的流程模版导入到另外一个系统中去。</li>
<ol>
</fieldset>

<fieldset>
<legend><img src="../../Img/Btn/Help.gif" />关于流程模版云</legend>
<ol>
<li>ccbpm团队为各位爱好者提供了云储存</li>
<li>它是流程引擎模版与表单引擎模版的完整的组合体。</li>
<li>ccbpm的jflow与ccflow的流程引擎导出的流程模版通用。</li>
<li>流程模版用于流程设计者的作品交换。</li>
<li>在实施的过程中，我们可以把一个系统上的流程模版导入到另外一个系统中去。</li>
</ol>
</fieldset>



 </td>

<td  valign="top">

<fieldset>
<legend> 模版导出 </legend>
<ul>
<li>我们已经为您生成了流程模版文件，<a href="javascript:WinOpen('/WF/Admin/XAP/DoPort.aspx?DoType=ExpFlowTemplete&FK_Flow=<%=fl.No%>&Lang=CH');">请点击这里下载到本机</a>。 </li>
<li>您可以把此模版放入到您的私有云里，我们会好好的为您永久的保管，点击这里<a href="" target="_blank" >上传到到私有云服务器</a> 。</li>
<li>如果您想支持ccbpm的工作，分享自己的设计成果为更多的ccbpm的爱好者，点击这里<a href="" target="_blank" >上传到到共有云服务器</a>。 </li>
</ul>
</fieldset>


<fieldset>
<legend> 从本机导入 </legend>
<ul>
<li>从本机导入：请您选择本机的一个xml格式文件 点击导入按钮完成导入。</li>
<li> 请选择文件: <asp:FileUpload ID="FU_Upload" runat="server" />  </li>
<li>导入的方式: 
<br />
<asp:RadioButton ID="Import_1" Text="作为新流程导入(由ccbpm自动生成新的流程编号)" GroupName="Import_mode" runat="server" Checked="true"/>
<br />
<asp:RadioButton ID="Import_2" Text="作为新流程导入(使用流程模版里面的流程编号，如果该编号已经存在系统则会提示错误)" GroupName="Import_mode" runat="server" />
<br />
<asp:RadioButton ID="Import_3" Text="作为新流程导入(使用流程模版里面的流程编号，如果该编号已经存在系统则会覆盖此流程)" GroupName="Import_mode" runat="server" />
<br />
<asp:RadioButton ID="Import_4" Text="按指定流程编号导入" GroupName="Import_mode" runat="server" />
指定的流程编号:<asp:TextBox ID="SpecifiedNumber" runat="server" ></asp:TextBox>
<br />
 </li>
</ul>
<div style=" text-align:center; padding:5px;">
    <asp:Button ID="Button1" runat="server" Text="执行导入" onclick="Button1_Click" />  
 </div>
</fieldset>


<fieldset>
<legend> 从云服务器导入 </legend>
<ul>
<li>从私有云导入</li>
<li>从共有云导入</li>
</ul>
 
</fieldset>


 </td>

</tr>
</table>

</asp:Content>
