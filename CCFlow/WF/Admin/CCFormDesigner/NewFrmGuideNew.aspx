<%@ Page Title="创建表单向导" Language="C#" MasterPageFile="~/WF/Admin/CCFormDesigner/Site.Master" AutoEventWireup="true" CodeBehind="NewFrmGuideNew.aspx.cs" Inherits="CCFlow.WF.Admin.CCFormDesigner.NewFrmGuide1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
    .title{
	text-decoration: none;
  font-size: 16px;
  color: #ffffff;
  background: cornflowerblue;
  padding: 5px 10px;
  margin: 0 5px;
  border-radius: 3px;
  box-shadow: 0px 1px 2px;
}
.con-list{
  line-height: 30px;
  font-size: 13px;
}
fieldset{
	  border: 1px solid #c7ced3;
	    margin-bottom: 20px;
}
.link-img{
	  float: right;
  padding-bottom: 10px;
  margin-right: 10px;
}

table caption{
	  border: 1px solid #C2D5E3;
  border-bottom: none;
line-height: 30px !important;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


 <table  style='width:100%;'  >
<caption >表单创建向导:请选择要创建的表单类型。</caption>

<TR>
<TD valign=top nowrap >
<fieldset width='100%' >

<legend><a class='title' href='?Step=1&FrmType=0&FK_FrmSort=&DBSrc=local' >创建自由表单</a>&nbsp;</legend>
<div class='con-list' style='float:left'>
<ul>
<li>自由表单是ccbpm推荐使用的表单.</li> 	
<li>他有丰富的界面元素,可以满足不同的应用需求.</li> 	
<li>采用了关系数据库存储格式，可以导出到xml存储，可以运行在任何设备上,实现与平台无关.</li> 	
<li>可以导入导出，格式不受影响，java , .net 移动终端都可以使用.</li>


<li>从ccform云端的表单库里导入, 从本机导入表单模版.</li>
<li><a href="NewFrmGuideEntity.aspx?DoType=FreeFrm" >创建空白的自由表单.</a></li>
</ul>	
</div>

<div style='float:right'>
<a class='link-img' href='http://blog.csdn.net/jflows/article/details/50034329'  target='_blank'>
<img src='./Img/ziyouForm.png' width='400px' ></a></div>
</fieldset>

<fieldset width='100%' >
<legend>&nbsp;<a class='title' href='?Step=1&FrmType=1&FK_FrmSort=&DBSrc=local' >创建傻瓜表单</a>&nbsp;</legend>
<div class='con-list' style='float:left'>
<ul>
<li>傻瓜表单与自由表单就是展示格式不同,其他的与自由表单一样.</li> 	
<li>傻瓜表单有固定的列与行，格式简洁、清新、实用.</li>
</ul>	

</div>

<div style='float:right'><a class='link-img' href='http://ccbpm.mydoc.io/?v=5404&t=17922' target='_blank' ><img src='./Img/shaguaForm.png' width='400px' ></a></div></fieldset><fieldset width='100%' ><legend>&nbsp;<a class='title' href='?Step=1&FrmType=5&FK_FrmSort=&DBSrc=local' >创建Excel表单</a>&nbsp;</legend><div class='con-list' style='float:left'><ul><li>Excel表单以excel表单模版为基础展现给用户，数据的展现与采集以excel文件为基础。</li> 	
<li>您可以设置每个excel的单元格对应一个表的一个字段,</li> 	
<li>用户在保存数据的时候可以保存到excel文件背后的数据表里。</li> 	
<li>数据表可以用于综合分析，而excel文件用于数据展现。</li> 	
<li>使用excel表单必须运行在IE浏览器上，需要支持activeX插件。</li> 	
</ul>	
</div><div style='float:right'><a class='link-img' href='http://ccbpm.mydoc.io/?v=5404&t=17922' target='_blank' ><img src='./Img/excelForm.jpg' width='400px' ></a></div></fieldset><fieldset width='100%' ><legend>&nbsp;<a class='title' href='?Step=1&FrmType=4&FK_FrmSort=&DBSrc=local' >创建Word表单</a>&nbsp;</legend><div class='con-list' style='float:left'><ul><li>Word表单以Word表单模版为基础展现给用户，数据的展现与采集以Word文件为基础。</li> 	
<li>您可以设置每个Word的标签对应一个表的一个字段,</li> 	
<li>用户在保存数据的时候可以保存到Word文件背后的数据表里。</li> 	
<li>数据表可以用于综合分析，而excel文件用于数据展现。</li> 	
<li>使用Word表单必须运行在IE浏览器上，需要支持activeX插件。</li> 	
<li>Word表单多用于公文。</li> 	
</ul>	
</div><div style='float:right'><a class='link-img' href='http://ccbpm.mydoc.io/?v=5404&t=17922' target='_blank' ><img src='./Img/wordForm.jpg' width='400px' ></a></div></fieldset><fieldset width='100%' ><legend>&nbsp;<a class='title' href='?Step=1&FrmType=3&FK_FrmSort=&DBSrc=local' >嵌入式表单</a>&nbsp;</legend><div class='con-list' style='float:left'><ul><li>自己编写一个表单jsp,aspx,php .... </li> 	
<li>通过本功能把他注册到ccbpm的表单系统中,可以被其他的程序引用，比如流程引擎。</li> 	
<li>如果要被驰骋工作流程引擎调用，自己定义的表单需要有一个约定的 Save javascript 函数,</li> 	
<li>用于保存整个表单的数据，如果保存的时候异常，就抛出错误。</li> 	
<li>驰骋工作流引擎在调用您的表单的时候，就需要传入一个数值类型的主键，参数名称为OID,</li> 	
<li>该嵌入式表单获取这个参数做为主键处理。</li> 	
<li>该表单用于特殊用户格式要求的表单，或者客户现在已有的表单。</li> 	
</ul>	
</div><div style='float:right'><a class='link-img' href='http://blog.csdn.net/jflows/article/details/50150457' target='_blank' ><img src='./Img/selfForm.png' width='400px' ></a></div></fieldset>
</TD>
</TR></Table>

</asp:Content>
