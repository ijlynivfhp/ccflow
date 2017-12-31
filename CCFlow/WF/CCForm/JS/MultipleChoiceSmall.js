﻿//小范围的多选,不需要搜索.
function MultipleChoiceSmall(mapExt) {


    var data = [];
    var valueField = "No";
    var textField = "Name";
    switch (mapExt.DoWay) {
        case 1:
            var tag1 = mapExt.Tag1;
            tag1 = tag1.replace(';', ',');

            $.each(tag1.split(","), function (i, o) {
                data.push({ No: i, Name: o })
            });
            break;
        case 2:
            valueField = "IntKey"
            textField = "Lab";
            var enums = new Entities("BP.Sys.SysEnums");
            enums.Retrieve("EnumKey", mapExt.Tag2);
            data = enums;
            break;
        case 3:
            var en = new Entity("BP.Sys.SFTable", mapExt.Tag3);
            data = en.DoMethodReturnJSON("GenerDataOfJson");
            break;
        case 4:
            var tag4SQL = mapExt.Tag4;
            tag4SQL = tag4SQL.replace('@WebUser.No', webUser.No);
            tag4SQL = tag4SQL.replace('@WebUser.Name', webUser.Name);
            tag4SQL = tag4SQL.replace('@WebUser.FK_Dept', webUser.FK_Dept);
            tag4SQL = tag4SQL.replace('@WebUser.FK_DeptName', webUser.FK_DeptName);
            if (tag4SQL.indexOf('@') == 0) {
                alert('约定的变量错误:' + tag4SQL + ", 没有替换下来.");
                return;
            }
            data = DBAccess.RunSQLReturnTable(tag4SQL);
            break;
    }

    (function (AttrOfOper, data, FK_MapData) {

        //如果是checkbox 多选.
        if (mapExt.Tag == "1" || mapExt.Tag == "2") {
            return MakeCheckBoxsModel(mapExt, data);
        }

        var tb = $("#TB_" + AttrOfOper);
        //tb.attr("visible", true); //把他隐藏起来.
        tb.css("visibility", "hidden");

        var cbx = $('<input type="text" />');
        cbx.attr("id", AttrOfOper + "_combobox");
        cbx.attr("name", AttrOfOper + "_combobox");
        tb.before(cbx);
        cbx.attr("class", "easyui-combobox");
        cbx.css("width", tb.width());

        cbx.combobox({
            "editable": false,
            "valueField": valueField,
            "textField": textField,
            "multiple": true,
            "onSelect": function (p) {
                $("#TB_" + AttrOfOper).val(cbx.combobox("getText"));
                (function sel(n, KeyOfEn, FK_MapData) {
                    //保存选择的值.
                    SaveVal(FK_MapData, KeyOfEn, n);

                })(p[valueField], AttrOfOper, FK_MapData);
            },
            "onUnselect": function (p) {
                $("#TB_" + AttrOfOper).val(cbx.combobox("getText"));
                (function unsel(n, KeyOfEn) {

                    //删除选择的值.
                    Delete(KeyOfEn, n);

                })(p[valueField], AttrOfOper);
            }
        });
        cbx.combobox("loadData", data);
    })(mapExt.AttrOfOper, data, mapExt.FK_MapData);
}


//checkbox 模式.
function MakeCheckBoxsModel(mapExt, data) {

    var textbox = $("#TB_" + mapExt.AttrOfOper);
    textbox.css("visibility", "hidden");
    var tbVal = textbox.val();
    for (var i = 0; i < data.length; i++) {

        var en = data[i];

        var eleHtml = "";
        var id = "CB_" + mapExt.AttrOfOper + "_" + en.No;
        var cb = $('<input type="checkbox" />');
        cb.attr("id", id);
        cb.attr("name", id);

        if (tbVal.indexOf(en.Name + ',') == 0)
            cb.attr("checked", false);
        else
            cb.attr("checked", true);

        //开始绑定事件.


        //end 绑定checkbox事件.

        textbox.before(cb);

        if (mapExt.Tag == "1")
            var lab = $('<label class="labRb align_cbl" for=' + id + ' >&nbsp;' + en.Name + '</label>');
        else
            var lab = $('<label class="labRb align_cbl" for=' + id + ' >&nbsp;' + en.Name + '</label><br>');

        textbox.before(lab);
    }
}

//删除数据.
function Delete(keyOfEn, n) {

    var oid = (pageData.WorkID || pageData.OID || "");
    var frmEleDB = new Entity("BP.Sys.FrmEleDB");
    frmEleDB.MyPK = KeyOfEn + "_" + oid + "_" + n;
    frmEleDB.Delete();
}

//设置值.
function SaveVal(fk_mapdata, keyOfEn, val) {

    var oid = (pageData.WorkID || pageData.OID || "");

    var frmEleDB = new Entity("BP.Sys.FrmEleDB");

    frmEleDB.MyPK = keyOfEn + "_" + oid + "_" + n;
    frmEleDB.FK_MapData = fk_mapdata;
    frmEleDB.EleID = KeyOfEn;
    frmEleDB.RefPKVal = oid;
    frmEleDB.Tag1 = n;
    if (frmEleDB.Update() == 0) {
        frmEleDB.Insert();
    }
}
