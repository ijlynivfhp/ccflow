<%@ Page Title="" Language="C#" MasterPageFile="../WinOpen.master" AutoEventWireup="true" CodeBehind="API.aspx.cs" Inherits="CCFlow.WF.Admin.AttrFlow.API" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%
   string flowNo = this.Request.QueryString["FK_Flow"];
   if (flowNo == null)
        flowNo = "001";
    BP.WF.Flow fl = new BP.WF.Flow(flowNo);
    int flowID = int.Parse(flowNo);
     %>

<table style="width:100%;">
<caption >开发API <div style=" float:right" > <a href="API.aspx?FK_Flow=<%=flowNo %>">URL调用接口</a> |  <a href="APICode.aspx?FK_Flow=<%=flowNo %>">代码开发API</a> |  <a href="APICodeFEE.aspx?FK_Flow=<%=flowNo %>">FEE开发API</a>  </div> </caption> 

<tr>
<th colspan="2"> URL调用接口 </th>
</tr>
<tr>

<td valign="top"  style="width:20%;" >

<ol>
<li>ccbpm提供页面级的功能组件，这些功能组件在/WF/下面。</li>
<li>比如：发起、待办、抄送、查询。</li>
<li>这些功能可以以明文的方式传输调用。</li>
<li>他的安全性是需要系统调用登录方法才可以，调用以上的URL。</li>
<li>如何调用登录方法，请参考下面的API。</li>
</ol>
</td>

<td>


<fieldset>
<legend>菜单列表</legend>
<ul>
<li>流程发起: <font color="Blue"><b> /WF/Start.aspx </b></font> 获得当前操作员的流程发起的列表，每个操作员的权限不同能发起的流程列表也不同。</li>
<li>工作待办: <font color="Blue"><b>/WF/EmpWorks.aspx  </b></font> 获得当前操作员的所有的待办列表，<font color="Blue"><b>/WF/EmpWorks.aspx?FK_Flow=<%=flowNo %>  </b></font>当前操作员的指定流程的待办。 </li>
<li>在途: <font color="Blue"><b>/WF/Runing.aspx  </b></font>  当前工作人员的所有在途，<font color="Blue"><b>/WF/Runing.aspx?FK_Flow=<%=flowNo %>  </b></font>当前操作员的指定流程的在途。在途定义：一个操作员的参与的流程，但是流程还没有完成，就叫在途。</li>
<li>抄送: <font color="Blue"><b>/WF/CC.aspx  </b></font>  抄送来的工作，当前人员没有处理权限，但是可以查看。</li>
<li>查询: <font color="Blue"><b>/WF/Search.aspx  </b></font>  对完成或者未完成的流程进行查询。</li>

</ul>
</fieldset>


<fieldset>
<legend>流程【<%=fl.Name %>】发起</legend>
<ul>
<li>发起URL: <font color="Blue"><b>/WF/MyFlow.aspx?FK_Flow=<%=flowNo %> </b></font>  ，您可以把该URL 放入到自己的系统菜单里，或者列表里。</li>
<li>该页面组件名叫：“工作处理器”，该处理器可以接受很多参数，可以向工作处理器里传入很多参数，格式与约定请参考说明书。</li>
</ul>
</fieldset>
 
<fieldset>
<legend>工作待办</legend>
<ul>
<li> 当前流程工作待办:<font color="Blue"><b> /WF/EmpWorks.aspx?FK_Flow=<%=flowNo %> </b></font></li>
<li> 所有流程工作待办:<font color="Blue"><b> /WF/EmpWorks.aspx </b></font> </li>
</ul>
</fieldset>
 
<fieldset>
<legend>在途工作(也成为未完成)</legend>
<ul>
 <li>当前流程工作在途:<font color="Blue"><b> /WF/Runing.aspx?FK_Flow=<%=flowNo %> </b> </font></li>
 <li>所有流程工作在途: <font color="Blue"><b> /WF/Runing.aspx </b></font></li>
</ul>
 </fieldset>

 
<fieldset>
<legend>查询相关接口</legend>

<ul>
 <li> 查询:   <font color="Blue"><b> /WF/Rpt/Search.aspx?FK_Flow=<%=flowNo %>&RptNo=ND<%= flowID%>MyRpt</b> </font>  </li> 
 <li>高级查询: <font color="Blue"><b> /WF/Rpt/SearchAdv.aspx?FK_Flow=<%=flowNo %>&RptNo=ND<%= flowID%>MyRpt</b> </font>  </li> 
 <li>分组分析:<font color="Blue"><b> /WF/Rpt/Group.aspx?FK_Flow=<%=flowNo %>&RptNo=ND<%= flowID%>MyRpt  </b> </font>  </li> 
 <li>交叉报表:<font color="Blue"><b> /WF/Rpt/D3.aspx?FK_Flow=<%=flowNo %>&RptNo=ND<%= flowID%>MyRpt  </b> </font>  </li> 
 <li>对比分析: <font color="Blue"><b> /WF/Rpt/Contrast.aspx?FK_Flow=<%=flowNo %>&RptNo=ND<%= flowID%>MyRpt </b> </font> </li> 
</ul>

 </fieldset>
</td>
</tr>


</table>
</asp:Content>
