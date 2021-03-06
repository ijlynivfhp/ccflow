<%@ Page Title="" Language="C#" MasterPageFile="../WinOpen.master" AutoEventWireup="true" CodeBehind="APICode.aspx.cs" Inherits="CCFlow.WF.Admin.AttrFlow.APICode" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<style type="text/css">
 
</style>
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
<caption >开发API <div style=" float:right" > <a href="API.aspx">URL调用接口</a> |  <a href="APICode.aspx">代码开发API</a> |  <a href="APICodeFEE.aspx?FK_Flow=<%=flowNo %>">FEE开发API</a>  </div>  </caption> 

<tr>
<th  colspan="2" style="width:30%" > 登录与门户API </th>
</tr>

<tr>
<td valign="top" >
<ol>
<li>首先要进行代码集成与组织机构的集成 </li>
<li>其次在自己的系统登录界面，登录成功后要执行ccbpm的框架登录。</li>
<li>所谓的登录就是调用ccbpm的登录接口，如左边的代码所示。</li>
</ol>
</td>

<td valign="top"  >
<font color=green>
 // 如下代码需要写入您的系统校验密码与用户名之后。</font>
<br />
<%
    //string userNo = "zhangsan";
    //BP.WF.Dev2Interface.Port_Login(userNo);
 %>

  <font color=blue> string</font>  userNo = <font color=red>"zhangsan"; </font>
  <br />
    BP.WF.<font color=blue>Dev2Interface</font>.Port_Login(userNo);

</td>
</tr>

<tr>
<th  colspan="2" > 菜单API </th>
</tr>

<tr>
<td valign="top" >

<ol>
<li>发起：一个操作员可以发起的工作 </li>
<li>待办：等待处理的工作。</li>
<li>在途：我参与的，但是这条流程还没有结束的流程。</li>
<li>抄送：不需要我处理，但是需要我知晓的工作。</li>
</ol>
</td>

<td>

<fieldset>
<legend>发起:</legend>
    //获得指定人员的可以发起的流程列表,调用这个接口返回一个datatable, 可以参考一个demo实现发起列表的输出。
   <br>

   <%
    //string userNo = "zhangsan";
    //BP.WF.Dev2Interface.Port_Login(userNo);
    // System.Data.DataTable dtStart = BP.WF.Dev2Interface.DB_GenerCanStartFlowsOfDataTable("zhangsan");
 %>
 <br />
 <b>
  System.Data.<font color=green>DataTable</font> dtStart = BP.WF.<font color=green>Dev2Interface</font>.DB_GenerCanStartFlowsOfDataTable(<font color=red>"zhangsan"</font>);
 <br />
 </b>

   <%
       string url = BP.Sys.SystemConfig.PathOfWebApp + "\\WF\\App\\Simple\\Start.aspx";
        %>
   <br>实现列表输出代码，请参考:  <br>
   <font color=gree><b> <%=url %></b></font>
   <br />
   运行Demo: <a href="./../../App/Simple/Start.aspx" target="_blank">发起流程</a>
</fieldset>


<fieldset>
<legend>待办：</legend>
    //获得指定人员的待办,调用这个接口返回一个datatable, 可以参考一个demo实现发起列表的输出。
   <br>
   <font color=green><b>  <font color=green>DataTable</font> dtTodolist = BP.WF.<font color=green>Dev2Interface</font>.DB_GenerEmpWorksOfDataTable();</b></font>
   <%
       url = BP.Sys.SystemConfig.PathOfWebApp + "\\WF\\App\\Simple\\ToDoList.aspx";
        %>
   <br>实现列表输出代码，请参考:  <br>
   <font color=gree><b> <%=url %></b></font>
   <br /> 
   运行Demo: <a href="./../../App/Simple/ToDoList.aspx" target="_blank">工作待办</a>
</fieldset>

<fieldset>
<legend>在途：</legend>
    //获得指定人员的在途,调用这个接口返回一个datatable ，代码参考：。
   <br>
    <font color=green>DataTable</font> dtRuning = BP.WF.<font color=green>Dev2Interface</font>.DB_GenerRuning();</b> 
   <%
       url = BP.Sys.SystemConfig.PathOfWebApp + "\\WF\\App\\Simple\\Runing.aspx";
    %>
   <br>实现列表输出代码，请参考:  <br>
   <font color=gree><b> <%=url %></b></font>
   <br /> 
   运行Demo: <a href="./../../App/Simple/Runing.aspx" target="_blank">在途工作</a>
</fieldset>

<fieldset>
<legend>查询：</legend>
    //ccbpm给你提供了一个link ，您可以调用这个link ,也可以自己去根据代码实现。
   <br>
   <%
       url = BP.Sys.SystemConfig.PathOfWebApp + "\\WF\\App\\Simple\\Search.aspx";
    %>
   <br>实现列表输出代码，请参考:  <br>
   <font color=gree><b> <%=url %></b></font>
   <br /> 
   运行Demo: <a href="./../../App/Simple/Search.aspx" target="_blank">查询</a>
</fieldset>
</td>
</tr>




<tr><th  colspan="2"> 创建WorkID </th> </tr>
<tr>
<td valign="top" >
<ol>
<li>创建工作ID是启动流程的开始。 </li>
<li>ccbpm的工作ID是一个Int64位的整数，始终是按照顺序号+1产生的。</li>
<li>该workid全局唯一，并且没有重复性，该信息记录到Sys_Serial，WorkID的生成从100开始。</li>
<li>该workid全局唯一，并且没有重复性，该信息记录到Sys_Serial，WorkID的生成从100开始。</li>

</ol>
</td>

<td valign="top">
<%
   // Int64 workid = BP.WF.Dev2Interface.Node_CreateBlankWork("001");
 %>
 //传入流程编号，调用创建一个工作ID。
 <br />
    <font color=blue>Int64</font> workid = BP.WF. <font color=blue>Dev2Interface</font>.Node_CreateBlankWork(<font color=red>"001"</font>);
</td>
</tr>



<tr><th  colspan="2"> 发送  -   简单发送</th> </tr>
<tr>
<td valign="top" >
<ol>
<li>工作发送就是让节点向下运动。 </li>
<li>调用接口执行发送后，返回一个执行结果的对象，该对象是流程引擎执行过程中的变量。</li>
<li>解析该变量，可以检查出流程是否完成，运行到那一个节点上去了，下一个节点谁可以处理工作？</li>
<li>它的流向，是根据流程设计的规则执行的。</li>
<li>它的接收人，是根据接受人的规则确定的。</li>
</ol>
</td>

<td valign="top">
<%
   ////传入流程编号, WorkID执行发送.
   //BP.WF.SendReturnObjs objs= BP.WF.Dev2Interface.Node_SendWork("001",workid);

   //// 检查流程是否结束？
   //bool isFlowOver = objs.IsStopFlow;
    
   //// 获得发送到那个节点上去了？
   //int toNodeID = objs.VarToNodeID;
   //string  toNodeName = objs.VarToNodeName;

   //// 获得发送给谁了？ 注意：这里如果是多个接受人员就会使用逗号分开。
   //string toEmpID   = objs.VarAcceptersID;
   //string toEmpName = objs.VarAcceptersName;
    
   //// 输出提示信息, 这个信息可以提示给操作员.
   //string infoMsg = objs.ToMsgOfHtml(); 
 %>

  <font color=green>//传入流程编号, WorkID执行发送. </font><br />
   BP.WF.<font color=blue>SendReturnObjs</font> objs= BP.WF.<font color=blue>Dev2Interface</font>.Node_SendWork(<font color=red>"001"</font>,workid);<br />
   <br />

  <font color=green> // 检查流程是否结束？ </font><br />
   <font color=blue>bool</font> isFlowOver = objs.IsStopFlow;<br />
   <br />
    
  <font color=green> // 获得发送到那个节点上去了？ </font><br />
   <font color=blue>int</font> toNodeID = objs.VarToNodeID;<br />
   <font color=blue>string</font>  toNodeName = objs.VarToNodeName;<br />
   <br />

   <font color=green>// 获得发送给谁了？ 注意：这里如果是多个接受人员就会使用逗号分开。 </font><br />
   <font color=blue>string</font> toEmpID   = objs.VarAcceptersID;<br />
   <font color=blue>string</font> toEmpName = objs.VarAcceptersName;<br />
   <br />
    
   <font color=green>// 输出提示信息, 这个信息可以提示给操作员. </font><br />
   <font color=blue>string</font> infoMsg = objs.ToMsgOfHtml(); <br />
   <br />
</td>
</tr>



<tr><th  colspan="2"> 发送  -  要指定发送给谁？发送到那个节点？(万能发送接口)</th> </tr>
<tr>
<td valign="top" >
<ol>
<li>如果程序员知道下一步要发送给谁，发送到那一个节点的情况下，就可以调用这个接口。</li>
<li>该接口就会摆脱流程引擎设计的方向条件规则与接受人规则。</li>
</ol>
</td>

<td valign="top">
<%
    ////如果确定了（或者自己计算好了）下一步要达到的节点，下一步的接受人，就可以按照如下格式调用。
    //BP.WF.SendReturnObjs objs = null;
    //objs = BP.WF.Dev2Interface.Node_SendWork("001", workid, 103, "zhangsan"); //发送给一个人,如果发送给多个人用逗号分开比如: zhangsan,lisi,wangwu

    ////下面调用方式，是知道要发送到那一个节点，但是不知道要发送给谁，让当前的节点定义的接受人规则来确定。
    //objs = BP.WF.Dev2Interface.Node_SendWork("001", workid, "103", null);

    ////下面调用方式，是知道要发送到那些人，但是不知道要发送到那个节点，让当前的节点定义的方向条件来确定。
    //objs = BP.WF.Dev2Interface.Node_SendWork("001", workid, "zhangsan", 103);

    //// 输出提示信息, 这个信息可以提示给操作员.
    //string infoMsg = objs.ToMsgOfHtml(); 
 %>

   <font color=green> //如果确定了（或者自己计算好了）下一步要达到的节点，下一步的接受人，就可以按照如下格式调用。</font><br />
    BP.WF.SendReturnObjs objs = null;
    objs = BP.WF.<font color=blue>Dev2Interface</font>.Node_SendWork(<font color=red>"001"</font>, workid, 103,  <font color=red>"zhangsan"</font> );
    <br />
    <font color=green>//发送给一个人,如果发送给多个人用逗号分开比如: zhangsan,lisi,wangwu </font>
    <br />
    <br />

    <font color=green>//下面调用方式，是知道要发送到那一个节点，但是不知道要发送给谁，让当前的节点定义的接受人规则来确定。</font><br />
    objs = BP.WF.<font color=blue>Dev2Interface</font>.Node_SendWork(<font color=red>"001"</font>, workid, 103, null);
    <br />
    <br />


   <font color=green> //下面调用方式，是知道要发送到那些人，但是不知道要发送到那个节点，让当前的节点定义的方向条件来确定。</font><br />
    objs = BP.WF.<font color=blue>Dev2Interface</font>.Node_SendWork(<font color=red>"001"</font>, workid, 103,<font color=red>"zhangsan"</font>);
    <br />
    <br />

   <font color=green> // 输出提示信息, 这个信息可以提示给操作员.</font><br />
    <font color=blue>string</font> infoMsg = objs.ToMsgOfHtml(); </font>
    <br />
</td>
</tr>





<tr><th  colspan="2"> 撤销</th> </tr>
<tr>
<td valign="top" >
<ul>
<li>撤销是发送的逆向操作。</li>
<li>撤销可以调用ccbpm提供的撤销窗口完成，这是最简单的方式。</li>
<li>地址为：/WF/WorkOpt/UnSend.aspx 参数为: FK_Flow,FK_Node,WorkID,FID，当前流程的4大参数。</li>
<li>如果需要在其他设备上工作，或者要自己写一个移交界面，请参考。</li>
<li>能否被撤销，是有当前活动节点的撤销规则所决定的。</li>
<li>撤销的功能显示在，在途的流程列表里，只有在途的工作才能被撤销。</li>
<li>在途工作：顾名思义，就是我参与的工作，并且工作尚未完成。</li>
<li>回滚流程，是在流程结束后需要重新在指定的节点，让指定的人员从新向下走。</li>
</ul>
</td>

<td valign="top">
<%
   // 执行撤销，返回撤销是否成功信息，如果抛出异常就说明撤销失败，撤销失败的原因多种，最有可能的是因为当前活动节点不允许撤销规则决定的。
   // string msg= BP.WF.Dev2Interface.Flow_DoUnSend("001", workID);
   // BP.WF.Dev2Interface.Node_ShiftUn("001", workid);
 %>
    <font color="green">
    /*<br />
    *执行撤销，返回撤销是否成功信息，如果抛出异常就说明撤销失败。<br /> 
    *撤销失败的原因多种，最有可能的是因为当前活动节点不允许撤销规则决定的。<br /> 
     */<br />
    <br />
    </font>
    <br />

    <font color=blue>string</font> msg= BP.WF.<font color=blue>Dev2Interface</font>.Flow_DoUnSend(<font color=red>"001"</font>, workID);
     
</td>
</tr>




<tr><th  colspan="2"> 回滚</th> </tr>
<tr>
<td valign="top" >
<ul>
<li>回滚与撤销不同的是回滚是在流程完成以后的操作，并且回滚是由管理员操作的。</li>
<li>回滚流程，是在流程结束后需要重新在指定的节点，让指定的人员从新向下走。</li>
</ul>
</td>

<td valign="top">
<%
    //执行回滚。
    //string msg= BP.WF.Dev2Interface.Flow_DoRebackWorkFlow("001", workID, 103, "因为审批错误，需要回滚，从节点103重新开始审批。");
 %>
    <font color="green">//执行回滚，返回的是回滚执行信息，如果回滚失败，则会抛出异常。</font>
    <br />
    <font color=blue>string</font> msg= BP.WF.<font color=blue>Dev2Interface</font>.Flow_DoRebackWorkFlow(<font color=red>"001"</font>, workID, 103, <font color=red>"因为审批错误，需要回滚，从节点103重新开始审批。"</font>);
     
</td>
</tr>










<tr><th  colspan="2">退回</th> </tr>
<tr>
<td valign="top" >
<ol>
<li>退回可以调用ccbpm提供的退回窗口完成，这是最简单的方式。</li>
<li>地址为：/WF/WorkOpt/ReturnWork.aspx 参数为: FK_Flow,FK_Node,WorkID,FID，当前流程的4大参数。</li>
<li>如果需要在其他设备上工作，或者要自己写一个退回界面，请参考。</li>
</ol>
</td>

<td valign="top">
<%
    ///*
    // * 1, 获得当前节点可以退回的节点，该接口返回一个datatable。
    // * 2, 一个节点能够退回到那写节点是由当前节点的退回规则确定的。
    // * 3, 调用退回需要三个参数：节点编号，工作ID, 流程ID, 对于线性流程FID始终等于0.
    // */
    //System.Data.DataTable dtCanReturnNodes = BP.WF.Dev2Interface.DB_GenerWillReturnNodes(103, workid, 0);
    ////执行退回，当前的节点是103，要退回的节点是105，
    //string msg = BP.WF.Dev2Interface.Node_ReturnWork("001", workid, 0, 103, 105, "您的申请信息不完整，请修改后重新发送。", false);
 %>
    
    <font color="green"> /*
    <br />
     * 1, 获得当前节点可以退回的节点，该接口返回一个datatable。
    <br />
     * 2, 一个节点能够退回到那写节点是由当前节点的退回规则确定的。
    <br />
     * 3, 调用退回需要三个参数：节点编号，工作ID, 流程ID, 对于线性流程FID始终等于0.
    <br />
     */</font>
    <br />
    System.Data.<font color=blue>DataTable</font> dtCanReturnNodes = BP.WF.<font color=blue>Dev2Interface</font>.DB_GenerWillReturnNodes(103, workid, 0);
    <br />
    <font color=green> // 返回的是可以退回的节点。</font>

    <br />
    <br />
    <font color=green> //执行退回，当前的节点是103，要退回的节点是105，</font>
    <br />
    <font color=blue>string</font> msg = BP.WF.<font color=blue>Dev2Interface</font>.Node_ReturnWork("001", workid, 0, 103, 105, <font color=red>"您的申请信息不完整，请修改后重新发送。"</font>, <font color=blue>false</font>);
    <br />
</td>
</tr>




<tr><th  colspan="2"> 移交</th> </tr>
<tr>
<td valign="top" >
<ol>
<li>移交也可以调用ccbpm提供的移交窗口完成，这是最简单的方式。</li>
<li>地址为：/WF/WorkOpt/Forward.aspx 参数为: FK_Flow,FK_Node,WorkID,FID，当前流程的4大参数。</li>
<li>移交就是把自己所要做的工作交给其他人处理。</li>
<li>如果需要在其他设备上工作，或者要自己写一个移交界面，请参考。</li>
</ol>
</td>

<td valign="top">
<%

    /*
     * 调用移交接口，传入必要的参数执行移交.
     * FID 在线性流程上始终等于0.
     */
    
  //  BP.WF.Dev2Interface.Node_Shift("001", 103, workid, 0, "zhangsan", "因我需要出差，所以特把工作移交给您。");

    /*
     * 撤销移交
     * 如果在移交之后，发现不需要移交，就需要撤销回来，调用撤销移交接口。
     */
 //   BP.WF.Dev2Interface.Node_ShiftUn("001", workid);
    
 %>
    
    <font color="green">
    <br />
    /* <br />
     * 调用移交接口，传入必要的参数执行移交.<br />
     * FID 在线性流程上始终等于0.<br />
     */<br />
     </font>
    <br />
    BP.WF.<font color=blue>Dev2Interface</font>.Node_Shift(<font color=red>"001"</font>, 103, workid, 0, <font color=red>"zhangsan", "因我需要出差，所以特把工作移交给您。"</font>);
    <br />
     
    <font color="green">
    <br />
       /*<br />
     * 撤销移交 <br />
     * 如果在移交之后，发现不需要移交，就需要撤销回来，调用撤销移交接口。 <br />
     */ <br />
     </font>
    BP.WF.<font color=blue>Dev2Interface</font>.Node_ShiftUn(<font color=red>"001"</font>, workid);
</td>
</tr>




<tr><th  colspan="2">加签</th> </tr>
<tr>
<td valign="top" >
<ul>
<li>加签也可以调用ccbpm提供的加签窗口完成，这是最简单的方式。</li>
<li>地址为：/WF/WorkOpt/Forward.aspx 参数为: FK_Flow,FK_Node,WorkID,FID，当前流程的4大参数。</li>
<li>加签就是把自己所要做的工作参考其他人意见，或者让其他人处理。</li>
<li>加签有两种模式：1，加签后由加签人发送到下一个节点。2，加签后由让加签人发送给当前人，由当前人发送给下一个节点。</li>
<li>如果需要在其他设备上工作，或者要自己写一个加签界面，请参考。</li>
</ul>
</td>

<td valign="top">
<%

    /*
     * 调用加签接口，传入必要的参数执行.
     * FID 在线性流程上始终等于0.
     */

    ////技术人员zhangsan接受工作后，点击发送还会发送给当前人员，由当前人员发送给下一步骤。
    //string info1= BP.WF.Dev2Interface.Node_Askfor(workid, BP.WF.AskforHelpSta.AfterDealSendByWorker, "zhangsan", "这里需要您出具技术鉴定意见.");

    ////技术人员填写后，直接就发送了下一步骤.
    //string info2 = BP.WF.Dev2Interface.Node_Askfor(workid, BP.WF.AskforHelpSta.AfterDealSend, "zhangsan", "这里需要您出具技术鉴定意见.");

    ////技术人员回复加签.
    //string infoReply = BP.WF.Dev2Interface.Node_AskforReply("001", 103, workid,0,  "我已经出具了技术鉴定意见，请参考.");
    
 %>
    
    <font color=green>
    <br />
     /*<br />
     * 调用加签接口，传入必要的参数执行.<br />
     * FID 在线性流程上始终等于0.<br />
     */<br />
     </font>
     <br />

    <font color=green>//技术人员zhangsan接受工作后，点击发送还会发送给当前人员，由当前人员发送给下一节点。</font><br />
    <font color=blue>string</font> info1= BP.WF.<font color=blue>Dev2Interface</font>.Node_Askfor(workid, BP.WF.AskforHelpSta.AfterDealSendByWorker, <font color=red>"zhangsan"</font>, <font color=red>"这里需要您出具技术鉴定意见."</font>);
    <br />
    <br />

    <font color=green>//技术人员填写后，直接就发送了下一节点.</font><br />
    <font color=blue>string</font> info2 = BP.WF.<font color=blue>Dev2Interface</font>.Node_Askfor(workid, BP.WF.AskforHelpSta.AfterDealSend, <font color=red>"zhangsan"</font>, <font color=red>"这里需要您出具技术鉴定意见."</font>);
    <br />
    <br />
    <font color=green>//技术人员回复加签，在由当前人发送到下一个节点。</font><br />
    <font color=blue>string</font> infoReply = BP.WF.<font color=blue>Dev2Interface</font>.Node_AskforReply(<font color=red>"001"</font>, 103, workid,0,  <font color=red>"我已经出具了技术鉴定意见，请参考."</font>);
</td>
</tr>




 
 

<tr><th  colspan="2"> 结束流程</th> </tr>
<tr>
<td valign="top" >
<ul>
<li>流程结束有三种方式</li>
<li>第一种走到最后一个节点正常结束。</li>
<li>第二种在特定的节点上，用户需要终止流程向下运动(与删除流程不同)。</li>
<li>第三种在特定的节点上，用户需要删除流程。</li>
</ul>
</td>

<td valign="top">
<%
    /*
     * 手工的结束流程,这种方式会记录日志.
     */
   //  string  overInfo = BP.WF.Dev2Interface.Flow_DoFlowOver("001", workID, "该供应商找不到了，要结束掉该流程。");

    /*
     * 删除流程, 
     * 删除流程有多种方式，用户可以根据自己的需求，调用不同的方式.
     * 最后一个参数是是否删除子流程. 
     */

     //按照标记删除流程
    // string delInfo0 = BP.WF.Dev2Interface.Flow_DoDeleteFlowByFlag("001", workID, "我不需要请假了", true);

     //彻底的删除流程，无日志记录.
    // string delInfo1 = BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal("001", workID, "我不需要请假了", true);

     //彻底的删除流程,有日志记录.
    // string delInfo2 = BP.WF.Dev2Interface.Flow_DoDeleteFlowByWriteLog("001", workID, "我不需要请假了", true); 
    
 %>
    <font color="green">
    <br />
     /*
     * 手工的结束流程,这种方式会记录日志.<br />
     */<br /></font>

     <font color="blue">string</font>  overInfo = BP.WF.<font color="blue">Dev2Interface</font> .Flow_DoFlowOver(<font color="red">"001" </font>, workID, <font color="red">"该供应商找不到了，要结束掉该流程。"</font>);


     <br />
    /*
     * 删除流程, <br />
     * 删除流程有多种方式，用户可以根据自己的需求，调用不同的方式.<br />
     * 最后一个参数是是否删除子流程. <br />
     */<br />
     <br />

     <font color="green">//按照标记删除流程</font><br />
     <font color="blue">string</font> delInfo0 = BP.WF.<font color="blue">Dev2Interface</font> .Flow_DoDeleteFlowByFlag(<font color="red">"001"</font>, workID, <font color="red">"我不需要请假了"</font>, <font color="blue">true</font>);<br /><br />

     <font color="green">//彻底的删除流程，无日志记录.</font><br />
     <font color="blue">string</font> delInfo1 = BP.WF.<font color="blue">Dev2Interface</font> .Flow_DoDeleteFlowByReal(<font color="red">"001"</font>, workID, <font color="red">"我不需要请假了"</font>, <font color="blue">true</font>);<br />


     <br />
     <font color="green">//彻底的删除流程,有日志记录.</font><br />
     <font color="blue">string</font> delInfo2 = BP.WF.<font color="blue">Dev2Interface</font> .Flow_DoDeleteFlowByWriteLog(<font color="red">"001"</font>, workID, <font color="red">"我不需要请假了"</font>, <font color="blue">true</font>); <br />
     
</td>
</tr>



</table>
</asp:Content>
