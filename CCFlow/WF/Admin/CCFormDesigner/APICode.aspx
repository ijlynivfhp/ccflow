<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="APICode.aspx.cs" Inherits="CCFlow.WF.Admin.CCFormDesigner.APICode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="../../../DataUser/Style/Table0.css" rel="stylesheet"  type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
     
     <table style="width:100%">
     <caption >代码API</caption>
     
     <tr>
     <td  style="width:30%;"   valign=top >
     
     <ul>
     <li>本代码演示了如何创建一个实体</li>
     <li>对一个实体的查询、修改、删除</li>
     </ul>
      </td>
     <td>

     <%
         ////获得表单ID.
         //string fk_mapdata = this.Request.QueryString["FK_MapData"];
         
         ////根据表单ID,创建表单实体， 执行新建操作.
         //BP.Sys.GEEntity en = new BP.Sys.GEEntity(fk_mapdata);
         //en.SetValByKey("DiZhi", "山东省济南市");
         //en.SetValByKey("DianHua", "0531-82374939");
         //en.SetValByKey("NianLIng", "33");  // 这里省下10000字..
         
         ////执行Insert 操作, 系统就会自动创建一个新的int类型的OID 插入数据库里.
         //en.Insert();


         ////根据表单ID,创建表单实体， 执行执行查询、更新操作.
         //string oid = this.Request.QueryString["OID"];
         //BP.Sys.GEEntity myen = new BP.Sys.GEEntity(fk_mapdata, oid);

         ////从实体里获取数据.
         //string diZhi = myen.GetValStringByKey("DiZhi");
         //string dianHua = myen.GetValStringByKey("DianHua");
         //int nianLing = myen.GetValIntByKey("NianLing");
         //// 这里省下10000字....... :)

         ////跟字段赋值。
         //en.SetValByKey("DiZhi", "山东省济南市高新区奥体中心");
         //en.SetValByKey("DianHua", "0531-82374939");
         //en.SetValByKey("NianLIng", "24"); 
         ////执行 Update 操作,系统就会根据主键执行更新.
         //myen.Update();

         ////根据表单ID,创建表单实体， 执行执行删除操作方法1.
         //string myOID = this.Request.QueryString["OID"];
         //BP.Sys.GEEntity myenTest = new BP.Sys.GEEntity(fk_mapdata, myOID);
         //myenTest.Delete(); //执行删除.

         //// 执行执行删除操作方法2.
         //BP.Sys.GEEntity myenTest1 = new BP.Sys.GEEntity(fk_mapdata);
         //myenTest1.PKVal = myOID;
         //myenTest1.Delete(); //执行删除.
       %>


         <font color="green"> //获得表单ID.</font><br />
         <font color="blue">string</font> fk_mapdata = this.Request.QueryString[<font color="red">"FK_MapData"</font>];
         <br /><br />
         
        <font color="green"> //根据表单ID,创建表单实体， 执行新建操作.</font><br />
         BP.Sys.<font color="blue">GEEntity</font> en = new BP.Sys.GEEntity(fk_mapdata);<br />
         en.SetValByKey(<font color="red">"DiZhi", "山东省济南市"</font>);<br />
         en.SetValByKey(<font color="red">"DianHua", "0531-82374939"</font>);<br />
         en.SetValByKey(<font color="red">"NianLIng", "33"</font>);  <font color="green"> // 这里省下10000字..</font>
         <br />
         <font color="green">//执行Insert 操作, 系统就会自动创建一个新的int类型的OID 插入数据库里.</font><br />
         en.Insert();<br />

         <br /><br />
         <font color="green">//根据表单ID,创建表单实体， 执行执行查询、更新操作.</font><br />
         <font color="blue">string</font> oid = this.Request.QueryString[<font color="red">"OID"</font>];<br />
         BP.Sys.<font color="blue">GEEntity</font> myen = new BP.Sys.GEEntity(fk_mapdata, oid);<br /><br />

        <font color="green"> //从实体里获取数据.</font><br />
         <font color="blue">string</font> diZhi = myen.GetValStringByKey(<font color="red">"DiZhi"</font>);<br />
         <font color="blue">string</font> dianHua = myen.GetValStringByKey(<font color="red">"DianHua"</font>);<br />
         <font color="blue">int</font> nianLing = myen.GetValIntByKey(<font color="red">"NianLing"</font>);<br />
         <font color="green">// 这里省下10000字..</font><br /><br />

         <font color="green">//跟字段赋值。</font><br />
         en.SetValByKey(<font color="red">"DiZhi", "山东省济南市高新区奥体中心"</font>);<br />
         en.SetValByKey(<font color="red">"DianHua", "0531-82374939"</font>);<br />
         en.SetValByKey(<font color="red">"NianLIng", "24"</font>); <br />
         <font color="green">//执行 Update 操作,系统就会根据主键执行更新.</font><br />
         myen.Update();<br /><br />

         <font color="green">//根据表单ID,创建表单实体， 执行执行删除操作方法1.</font><br />
         <font color="blue">string</font> myOID = this.Request.QueryString[<font color="red">"OID"</font>];<br />
         BP.Sys.<font color="blue">GEEntity</font> myenTest = new BP.Sys.GEEntity(fk_mapdata, myOID);<br />
         myenTest.Delete(); <font color="green">//执行删除.</font><br /><br />

         <font color="green">// 执行执行删除操作方法2.</font><br />
         BP.Sys.<font color="blue">GEEntity</font> myenTest1 = new BP.Sys.<font color="blue">GEEntity</font>(fk_mapdata);<br />
         myenTest1.PKVal = myOID;<br />
         myenTest1.Delete(); <font color="green">//执行删除.</font><br />


      </td>

     </tr>


     </table>

    </form>
</body>
</html>
