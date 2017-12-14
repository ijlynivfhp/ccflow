﻿
function InitPage() {

    //加载标签页
    $.ajax({
        type: 'post',
        async: true,
        url: Handler + "?DoType=TimeBase_Init&FK_Node=" + fk_node + "&FK_Flow=" + fk_flow + "&WorkID=" + workid + "&FID=" + fid + "&t=" + Math.random(),
        dataType: 'html',
        success: function (data) {

            if (data.indexOf('err@') == 0) {
                alert(data);
                return;
            }

            if (data == "[]")
                return;

            data = eval('(' + data + ')');

            //日志列表.
            var tracks = data["Track"];

            //工作列表.
            var timeDay = "";
            var checkStr = "";
            var dotColor = -1;
            var idx = 1;

            //获得流程引擎注册表信息.
            var gwf = data["WF_GenerWorkFlow"][0];

            //获得工作人员列表.
            var gwls = data["WF_GenerWorkerList"];

            //前进的 track. 用于获取当前节点的上一个节点的track.
            var trackDotOfForward = null;

            //是否有审核组件?
            var isHaveCheck = false;
            for (var i = 0; i < tracks.length; i++) {

                var track = tracks[i];

                // 记录审核节点。
                if (track.ActionType == ActionType.WorkCheck) {
                    isHaveCheck = true;
                    break;
                }
            }

            //输出列表.
            for (var i = 0; i < tracks.length; i++) {

                var track = tracks[i];
                if (track.FID != 0)
                    continue;

                //如果是协作发送，就不输出他. edit 2016.02.20 .BBS也不显示，added by liuxc,2016-12-15
                if (track.ActionType == ActionType.TeampUp
                || track.ActionType == ActionType.UnSend //撤销的不现实.
                || track.ActionType == ActionType.FlowBBS)
                    continue;

                //如果有审核组件, 发送信息都不显示,可以显示会签，流程完成信息.
                if (isHaveCheck == true && idx > 1) {

                    //所有前进的信息，都不显示.
                    if (track.ActionType == ActionType.Forward
                                    || track.ActionType == ActionType.ForwardAskfor
                                    || track.ActionType == ActionType.ForwardFL
                                    || track.ActionType == ActionType.ForwardHL) {

                        if (trackDotOfForward && trackDotOfForward.NDFrom != track.NFrom) {
                            trackDotOfForward = track;
                        }
                        continue; //都不显示他.
                    }
                }

                // 记录审核节点。
                if (track.ActionType == ActionType.WorkCheck)
                    checkStr = track.NDFrom; //记录当前的审核节点id.

                //审核信息过滤,如果到达了该节点, 1.当前节点没有完成.  2.当前流程没有走完.  3.
                if (track.ActionType == ActionType.WorkCheck
                && gwf.FK_Node == track.NDFrom
                && gwf.WFState != 3 && gwls != undefined) {
                    //如果当前节点与审核信息节点一致，就说明当前人员的审核意见已经保存，但是工作还没有发送,就不让他显示。

                    var isChecked = false;
                    for (var idxOfGWL = 0; idxOfGWL < gwls.length; idxOfGWL++) {
                        var gwl = gwls[idxOfGWL];

                        if (gwl.FK_Node != track.NDFrom)
                            continue;

                        if (gwl.FK_Emp != track.EmpFrom)
                            continue;

                        if (gwl.IsPass == "1") {
                            isChecked = true;
                            break;
                        }
                    }

                    //如果他没有审核，就不让他显示.
                    if (isChecked == false)
                        continue;
                } // end 审核信息过滤 , 排除在审批中，并且没有发送的节点.


                //前进.
                if (track.ActionType == ActionType.Forward) {
                    if (checkStr == track.NDFrom)
                        continue;
                }

                var rdt = track.RDT;
                if (timeDay == "") {

                    timeDay = rdt.substring(0, 10);
                    dotColor = dotColor + 1;
                    if (dotColor == 3)
                        dotColor = 1;

                    var rowDay = "<tr>";
                    rowDay += "<td colspan=3 class=TDDay ><b>" + timeDay + "</b></td>";
                    rowDay += "</tr>";
                    $("#Table1 tr:last").after(rowDay);

                } else if (rdt.indexOf(timeDay) != 0) {

                    timeDay = rdt.substring(0, 10);

                    dotColor = dotColor + 1;
                    if (dotColor == 3)
                        dotColor = 1;

                    var rowDay = "<tr>";
                    rowDay += "<td colspan=3 class=TDDay ><b>" + timeDay + "</b></td>"
                    rowDay += "</tr>";
                    $("#Table1 tr:last").after(rowDay);
                }

                //左边的日期点.
                var left = "";
                left = rdt.substring(10, 16);
                left = left.replace(':', '时');
                left = left + "分";

                left += "<br><img src='../../../DataUser/UserIcon/" + track.EmpFrom + ".png'  onerror=\"src='../../../DataUser/UserIcon/Default.png'\" style='width:60px;' /><br>" + track.EmpFromT;

                //时间轴.
                var timeBase = "";
                var imgUrl = ActionTypeStr(track.ActionType);
                timeBase = "<img src='" + imgUrl + "' width='10px;' class='ImgOfAC' alt='" + track.ActionTypeText + "'  />";

                //内容.
                var doc = "";
               // doc += "<img src='../../Img/TolistSta/" + dotColor + ".png' />" + track.NDFromT + " - " + timeBase + track.ActionTypeText;
                doc += timeBase + track.NDFromT + " - " +track.ActionTypeText;


                var at = track.ActionType;

                //如果是第一节点，就把他设置为到达节点.
                if (idx == 1 || idx == 0) {
                    trackDotOfForward = track;
                }

                //增加两列，到达时间、用时 added by liuxc,2014-12-4
                if (idx > 1 && 1 == 2) {

                    //求时间点track.

                    //求当前节点执行会签的tack, 如果没有就去当前节点上一个节点发送track.
                    if (track.ActionType == ActionType.Forward
                                    || track.ActionType == ActionType.ForwardAskfor
                                    || track.ActionType == ActionType.ForwardFL
                                    || track.ActionType == ActionType.ForwardHL) {
                        trackDotOfForward = track;
                    }

                    if (trackDotOfForward) {
                        //到达时间.
                        var toTime = trackDotOfForward.RDT;
                        var toTimeDot = toTime.replace(/\-/g, "/");
                        toTimeDot = new Date(toTimeDot);

                        //当前发生日期.
                        var timeDot = track.RDT.replace(/\-/g, "/");
                        timeDot = new Date(timeDot);

                        doc += "<br>到达时间:<font color=green>" + toTime + "</font>用时：<font color=green>" + GetSpanTime(toTimeDot, timeDot) + "</font>";

                    }
                }

                var tag = track.Tag;
                if (tag != null)
                    tag = tag.replace("~", "'");

                var msg = track.Msg;
                if (msg != "") {

                    while (msg.indexOf('\t\n') >= 0) {
                        msg = msg.replace('\t\n', '<br>');
                    }

                    doc += "<br><p>";
                    doc += "<font color=green><br>" + msg + "</font><br>";
                    doc += "</p>";
                }

                var newRow = "";
                newRow = "<tr  title='" + track.ActionTypeText + "' >";
                newRow += "<td class='TDTime' >" + left + "</td>";
                newRow += "<td class='TDBase' ></td>";
                newRow += "<td class='TDDoc' >" + doc+"</td>";
                newRow += "</tr>";

                $("#Table1 tr:last").after(newRow);

                idx++;
            }


            //增加等待审核的人员, 在所有的人员循环以后.
            if (gwls) {
                var isHaveNoChecker = false;
                for (var i = 0; i < gwls.length; i++) {

                    var gwl = gwls[i];
                    if (gwl.IsPass == 1)
                        continue;

                    isHaveNoChecker = true;
                }

                //如果有尚未审核的人员，就
                if (isHaveNoChecker == true) {


                    var rowDay = "<tr>";
                    rowDay += "<td colspan=3 class=TDDay ><b>等待审批</b></td>";
                    rowDay += "</tr>";
                    $("#Table1 tr:last").after(rowDay);

                    for (var i = 0; i < gwls.length; i++) {
                        var html = "";

                        var gwl = gwls[i];
                        if (gwl.IsPass == 1)
                            continue;

                        var doc = "";
                        doc += "<table border=0>";

                        doc += "<tr>";
                        doc += "<td>审批人</td>";
                        doc += "<td>" + gwl.FK_EmpText + "</td>";
                        doc += "</tr>";

                        doc += "<tr>";
                        doc += "<td>阅读状态</td>";

                        if (gwl.IsRead == "1")
                            doc += "<td><font color=green>阅读（打开）.</font></td>";
                        else
                            doc += "<td><font color=red>尚未阅读（打开）.</font></td>";
                        doc += "</tr>";



                        doc += "<tr>";
                        doc += "<td>工作到达日期</td>";
                        doc += "<td>" + gwl.RDT + "</td>";
                        doc += "</tr>";


                        //到达时间.
                        var toTime = gwl.RDT;
                        var toTimeDot = toTime.replace(/\-/g, "/");
                        toTimeDot = new Date(toTimeDot);

                        //当前发生日期.
                        timeDot = new Date();

                        doc += "<tr>";
                        doc += "<td>已经耗时</td>";
                        doc += "<td>" + GetSpanTime(toTimeDot, timeDot) + "</td>";
                        doc += "</tr>";


                        doc += "<tr>";
                        doc += "<td>应完成日期</td>";
                        doc += "<td>" + gwl.SDT + "</td>";
                        doc += "</tr>";


                        //应该完成日期.
                        toTime = gwl.SDT;
                        toTimeDot = toTime.replace(/\-/g, "/");
                        toTimeDot = new Date(toTimeDot);

                        //当前发生日期.
                        timeDot = new Date();

                        doc += "<tr>";
                        doc += "<td>还剩余</td>";
                        doc += "<td>" + GetSpanTime(timeDot, toTimeDot) + "</td>";
                        doc += "</tr>";

                        doc += "</table>";

                        var left = "";
                        left += "<br><img src='../../../DataUser/UserIcon/" + gwl.FK_Emp + ".png'  onerror=\"src='../../../DataUser/UserIcon/Default.png'\" style='width:60px;' />";
                        left += "" + gwl.FK_EmpText;

                        var newRow = "";
                        newRow = "<tr  title='等待审批人员' >";
                        newRow += "<td class='TDTime' >" + left + "</td>";
                        newRow += "<td class='TDBase' ></td>";
                        newRow += "<td class='TDDoc' >" + doc + "</td>";
                        newRow += "</tr>";
                        $("#Table1 tr:last").after(newRow);
                    }
                }
            }


            //调整大小.
            if (window.screen) {
                var w = screen.availWidth;
                var h = screen.availHeight;
                window.moveTo(0, 0);
                window.resizeTo(w, h);
            }
        }
    });
}

function GetSpanTime(date1, date2) {
    ///<summary>计算date2-date1的时间差，返回使用“x天x小时x分x秒”形式的字符串表示</summary>
    var date3 = date2.getTime() - date1.getTime();  //时间差秒
    var str = '';
    //计算出相差天数
    var days = Math.floor(date3 / (24 * 3600 * 1000));
    if (days > 0) {
        str += days + '天';
    }

    //计算出小时数
    var leave1 = date3 % (24 * 3600 * 1000);   //计算天数后剩余的毫秒数
    var hours = Math.floor(leave1 / (3600 * 1000));
    if (hours > 0) {
        str += hours + '小时';
    }

    //计算相差分钟数
    var leave2 = leave1 % (3600 * 1000);         //计算小时数后剩余的毫秒数
    var minutes = Math.floor(leave2 / (60 * 1000));
    if (minutes > 0) {
        str += minutes + '分';
    }

    if (str.length == 0) {
        var leave3 = leave2 % (60 * 1000);
        var seconds = Math.floor(leave3 / 1000);
        str += seconds + '秒';
    }

    return str;
}

/**Begin - Form Controls ***************/
var ActionType = {
    /// <summary>
    /// 发起
    /// </summary>
    Start: 0,
    /// <summary>
    /// 前进(发送)
    /// </summary>
    Forward: 1,
    /// <summary>
    /// 退回
    /// </summary>
    Return: 2,
    /// <summary>
    /// 退回并原路返回.
    /// </summary>
    ReturnAndBackWay: 201,
    /// <summary>
    /// 移交
    /// </summary>
    Shift: 3,
    /// <summary>
    /// 撤消移交
    /// </summary>
    UnShift: 4,
    /// <summary>
    /// 撤消发送
    /// </summary>
    UnSend: 5,
    /// <summary>
    /// 分流前进
    /// </summary>
    ForwardFL: 6,
    /// <summary>
    /// 合流前进
    /// </summary>
    ForwardHL: 7,
    /// <summary>
    /// 流程正常结束
    /// </summary>
    FlowOver: 8,
    /// <summary>
    /// 调用起子流程
    /// </summary>
    CallChildenFlow: 9,
    /// <summary>
    /// 启动子流程
    /// </summary>
    StartChildenFlow: 10,
    /// <summary>
    /// 子线程前进
    /// </summary>
    SubFlowForward: 11,
    /// <summary>
    /// 取回
    /// </summary>
    Tackback: 12,
    /// <summary>
    /// 恢复已完成的流程
    /// </summary>
    RebackOverFlow: 13,
    /// <summary>
    /// 强制终止流程 For lijian:2012-10-24.
    /// </summary>
    FlowOverByCoercion: 14,
    /// <summary>
    /// 挂起
    /// </summary>
    HungUp: 15,
    /// <summary>
    /// 取消挂起
    /// </summary>
    UnHungUp: 16,
    /// <summary>
    /// 强制移交
    /// </summary>
    ShiftByCoercion: 17,
    /// <summary>
    /// 催办
    /// </summary>
    Press: 18,
    /// <summary>
    /// 逻辑删除流程(撤销流程)
    /// </summary>
    DeleteFlowByFlag: 19,
    /// <summary>
    /// 恢复删除流程(撤销流程)
    /// </summary>
    UnDeleteFlowByFlag: 20,
    /// <summary>
    /// 抄送
    /// </summary>
    CC: 21,
    /// <summary>
    /// 工作审核(日志)
    /// </summary>
    WorkCheck: 22,
    /// <summary>
    /// 删除子线程
    /// </summary>
    DeleteSubThread: 23,
    /// <summary>
    /// 请求加签
    /// </summary>
    AskforHelp: 24,
    /// <summary>
    /// 加签向下发送
    /// </summary>
    ForwardAskfor: 25,
    /// <summary>
    /// 自动条转的方式向下发送
    /// </summary>
    Skip: 26,
    /// <summary>
    /// 队列发送
    /// </summary>
    Order: 27,
    /// <summary>
    /// 协作发送
    /// </summary>
    TeampUp: 28,
    /// <summary>
    /// 流程评论
    /// </summary>
    FlowBBS: 29,
    /// <summary>
    /// 信息
    /// </summary>
    Info: 100
};

function ActionTypeStr(at) {

    switch (at) {
        case ActionType.Start:
            return "../../Img/Action/Start.png";
        case ActionType.Forward:
            return "../../Img/Action/Forward.png";
        case ActionType.Return:
            return "../../Img/Action/Return.png";
        case ActionType.ReturnAndBackWay:
            return "../../Img/Action/ReturnAndBackWay.png";
        case ActionType.Shift:
            return "../../Img/Action/Shift.png";
        case ActionType.UnShift:
            return "../../Img/Action/UnShift.png";
        case ActionType.UnSend:
            return "../../Img/Action/UnSend.png";
        case ActionType.ForwardFL:
            return "../../Img/Action/ForwardFL.png";
        case ActionType.ForwardHL:
            return "../../Img/Action/ForwardHL.png";
        case ActionType.CallChildenFlow:
            return "../../Img/Action/CallChildenFlow.png";
        case ActionType.StartChildenFlow:
            return "../../Img/Action/StartChildenFlow.png";
        case ActionType.SubFlowForward:
            return "../../Img/Action/SubFlowForward.png";
        case ActionType.RebackOverFlow:
            return "../../Img/Action/RebackOverFlow.png";
        case ActionType.FlowOverByCoercion:
            return "../../Img/Action/FlowOverByCoercion.png";
        case ActionType.HungUp:
            return "../../Img/Action/HungUp.png";
        case ActionType.UnHungUp:
            return "../../Img/Action/UnHungUp.png";
        case ActionType.ShiftByCoercion:
            return "../../Img/Action/ShiftByCoercion.png";
        case ActionType.Press:
            return "../../Img/Action/Press.png";
        case ActionType.DeleteFlowByFlag:
            return "../../Img/Action/DeleteFlowByFlag.png";
        case ActionType.UnDeleteFlowByFlag:
            return "../../Img/Action/UnDeleteFlowByFlag.png";
        case ActionType.CC:
            return "../../Img/Action/CC.png";
        case ActionType.WorkCheck:
            return "../../Img/Action/WorkCheck.png";
        case ActionType.AskforHelp:
            return "../../Img/Action/AskforHelp.png";
        case ActionType.Skip:
            return "../../Img/Action/Skip.png";
        case ActionType.Order:
            return "../../Img/Action/Order.png";
        case ActionType.TeampUp:
            return "../../Img/Action/TeampUp.png";
        case ActionType.FlowBBS:
            return "../../Img/Action/FlowBBS.png";
        case ActionType.Info:
            return "../../Img/Action/Info.png";
        default:
            return "../../Img/Dot.png";
    }
}

/* 打开表单. */
function OpenFrm(workid, nodeID, flowNo) {

    //执行催办.
    $.ajax({
        type: 'post',
        async: true,
        url: Handler + "?DoType=TimeBase_OpenFrm&FK_Node=" + nodeID + "&FK_Flow=" + flowNo + "&WorkID=" + workid + "&m=" + Math.random(),
        dataType: 'html',
        success: function (data) {

            if (data.indexOf('err@') == 0) {
                alert(data);
                return;
            }

            if (data.indexOf('url@') == 0) {
                data = data.replace('url@', '');
                data = "../../"+data;
                window.open(data);
                return;
            }
            alert(data);
        }
    });
}
 