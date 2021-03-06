<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DesignerSL.aspx.cs" Inherits="CCFlow.WF.Admin.CCBPMDesigner.DesignerSL" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>流程设计器</title>
     <style type="text/css">
    html, body {
	    height: 100%;
	    width : 100%;
	    overflow:hidden;
    }
    body {
	    padding: 0;
	    margin: 0;
    }
    #silverlightControlHost {
	    height: 100%;
	    width: 100%;
	    text-align:center; 
    }
   </style>

    <script src="../../../Silverlight.js" type="text/javascript"></script>
    <script type="text/javascript">

        function maximizeWindow() {
            window.moveTo(0, 0)
            window.resizeTo(screen.width, window.screen.availHeight)
        }
        function GetBrowserWidth() {
            return window.screen.width;
        }
        function GetBrowserHeight() {
            return window.screen.height;
        }

        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }

            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
                return;
            }

            var errMsg = "Silverlight 应用程序中未处理的错误 " + appSource + "\n";

            errMsg += "代码: " + iErrorCode + "    \n";
            errMsg += "类别: " + errorType + "       \n";
            errMsg += "消息: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "文件: " + args.xamlFile + "     \n";
                errMsg += "行: " + args.lineNumber + "     \n";
                errMsg += "位置: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "行: " + args.lineNumber + "     \n";
                    errMsg += "位置: " + args.charPosition + "     \n";
                }
                errMsg += "方法名称: " + args.methodName + "     \n";
            }
            alert(errMsg);
        }

</script>
</head>
<body onload="javascript:maximizeWindow()">
    <form id="form1" runat="server" style="height:100%">
    <div id="silverlightControlHost">
		<object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
			<param name="source" value="../ClientBin/CCBPMDesignerSingle.xap"/>
			<param name="onerror" value="onSilverlightError" />
			<param name="background" value="white" />
			<param name="minRuntimeVersion" value="2.0.31005.0" />
            <param name="initParams" value="platForm=.NET,appName=" />
			<param name="autoUpgrade" value="false" />
			<a href="<%=BP.WF.Glo.SilverlightDownloadUrl %>" > 下载微软的插件Silverlight
     			<img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="下载微软的插件Silverlight" style="border-style: none"/>
			</a>
		</object>
    </div>
  
    <iframe style='visibility:hidden;height:0;width:0;border:0px'></iframe>
    </form>
</body>
</html>
