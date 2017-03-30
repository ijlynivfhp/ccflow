﻿/** 
* An 'interface' for undoable actions, implemented by classes that specify 
* how to handle action
* 
* 
* @this {FigureCreateCommand} 
* @constructor
* @param {Function} factoryFunction - the function that will create the {Figure}. It will be local copy (of original pointer)
* @param {Number} x - the x coordinates
* @param {Number} y - the x coordinates
* @author Alex <alex@scriptoid.com>
*  @author Artyom Pokatilov <artyom.pokatilov@gmail.com>
*/
function FigureCreateCommand(factoryFunction, x, y) {
    this.oType = 'FigureCreateCommand';

    /**Any sequence of many mergeable actions can be packed by the history*/
    this.mergeable = false;
    this.factoryFunction = factoryFunction;
    this.x = x;
    this.y = y;
    this.firstExecute = true;
    this.figureId = null;
}


FigureCreateCommand.prototype = {
    /**This method got called every time the Command must execute*/
    execute: function () {
        if (createFigureName == "FlowChart" ) {
            var funIsExist = this.IsExist;
            var isExit = funIsExist(createFigureName);
            if (isExit == true) {
                $.messager.alert("错误", "@已存在ID为(" + createFigureName + ")的元素，不允许添加同名元素！", "error");
                return false;
            }
        }

        if (this.firstExecute) {
            var canAddFigure = true;  //是否拖上去之后，就立刻创建？
            //create figure
            var createdFigure = this.factoryFunction(this.x, this.y);
            //CCForm private Property
            createdFigure.CCForm_Shape = createFigureName;
            //move it into position
            createdFigure.transform(Matrix.translationMatrix(this.x - createdFigure.rotationCoords[0].x, this.y - createdFigure.rotationCoords[0].y))
            createdFigure.style.lineWidth = defaultLineWidth;
            //ccflow business logic
            switch (createFigureName) {

                case CCForm_Controls.Label:
                case CCForm_Controls.Button:
                case CCForm_Controls.HyperLink:
                case CCForm_Controls.Image:
                    createdFigure.CCForm_MyPK = Util.NewGUID();
                    break;
                case CCForm_Controls.TextBox:
                case CCForm_Controls.TextBoxInt:
                case CCForm_Controls.TextBoxFloat:
                case CCForm_Controls.TextBoxMoney:
                case CCForm_Controls.Date:
                case CCForm_Controls.DateTime:
                case CCForm_Controls.CheckBox:
                    canAddFigure = false;
                    this.DataFieldCreate(createdFigure, this.x, this.y);
                    break;
                case CCForm_Controls.ListBox:
                case CCForm_Controls.HiddendField:
                    alert('目前还没有对{' + createFigureName + '}提供该控件的支持.');
                    return;
                case CCForm_Controls.DropDownListEnum: //枚举类型.
                    canAddFigure = false; // 需要弹出对话框创建.
                    this.RadioButtonCreate(createdFigure, this.x, this.y, 'DDL');
                    break;
                case CCForm_Controls.RadioButton:
                    canAddFigure = false;  // 需要弹出对话框创建.
                    this.RadioButtonCreate(createdFigure, this.x, this.y, 'RB');
                    break;
                case CCForm_Controls.DropDownListTable: //枚举类型.
                    canAddFigure = false; // 需要弹出对话框创建.
                    this.DropDownListTableCreate(createdFigure, this.x, this.y);
                    break;
                case "Fieldset":
                case "HandSiganture":
                case "iFrame":
                case CCForm_Controls.Dtl: //明细表.
                case CCForm_Controls.AthMulti: //多附件.
                case CCForm_Controls.AthSingle: //单附件.
                case CCForm_Controls.AthImg: //图片附件.  以上控件都是用通用的 No,Name 数据框.
                    canAddFigure = false; // 需要弹出对话框创建.
                    this.PublicNoNameCtrlCreate(createdFigure, this.x, this.y, createFigureName);
                    break;
                case "CheckGroup":
                    alert('该功能没有实现' + createFigureName + ' 需要连续创建三个字段.');
                    break;
                //case CCForm_Controls.FrmCheck: //审核组件
                //case CCForm_Controls.FrmCheck: // 审核组件.
                //case CCForm_Controls.FlowChart: //轨迹图.
                //case CCForm_Controls.SubFlowDtl: //子流程.
                    //case CCForm_Controls.ThreadDtl: //子线城.
                case "FrmCheck": // 审核组件.
                case "FlowChart": //轨迹图.
                case "SubFlowDtl": //子流程.
                case "ThreadDtl": //子线城.

                    // alert(createFigureName);
                    //                    if (funIsExist(createFigureName) == true) {
                    //                        //$.messager.alert("错误", "已存在ID为(" + frmVal.KeyOfEn + ")的元素，不允许添加同名元素！", "error");
                    //                        //  alert('该控件已经存在' + createFigureName);
                    //                        return;
                    //                    }

                    //名称都是独立的.
                    createdFigure.CCForm_MyPK = createFigureName;
                    this.FlowFieldCreate(createdFigure, this.x, this.y, createFigureName);
                    break;

                default: //按照通用的接受编号，名称的方式来创建.
                    //canAddFigure = false; // 需要弹出对话框创建.
                    //this.PublicNoNameCtrlCreate(createdFigure, this.x, this.y, createFigureName);
                    alert('没有判断的控件类型{' + createFigureName + '}，或者该功能为实现。');
                    return;
            }

            if (canAddFigure == true) {
                //store id for later use
                //TODO: maybe we should try to recreate it with same ID (in case further undo will recreate objects linked to this)
                this.figureId = createdFigure.id;

                //add to STACK
                STACK.figureAdd(createdFigure);

                //make this the selected figure
                selectedFigureId = createdFigure.id;

                //set up it's editor
                setUpEditPanel(createdFigure);

                //move to figure selected state
                state = STATE_FIGURE_SELECTED;
            }
            this.firstExecute = false;
        }
        else { //redo
            throw "Not implemented";
        }
    },
    /**创建数据字段**/
    DataFieldCreate: function (createdFigure, x, y) {

        var dgId = "iframeTextBox";
        var url = "DialogCtr/FrmTextBox.htm?DataType=" + createFigureName + "&s=" + Math.random();
        var funIsExist = this.IsExist;

        OpenEasyUiDialog(url, dgId, '新建文本字段', 600, 394, 'icon-new', true, function (HidenFieldFun) {
            var win = document.getElementById(dgId).contentWindow;
            var frmVal = win.GetFrmInfo();

            if (frmVal.Name == null || frmVal.Name.length == 0) {
                $.messager.alert('错误', '字段名称不能为空。', 'error');
                return false;
            }
            if (frmVal.KeyOfEn == null || frmVal.KeyOfEn.length == 0) {
                $.messager.alert('错误', '英文字段不能为空。', 'error');
                return false;
            }
            //判断主键是否存在
            var isExit = funIsExist(frmVal.KeyOfEn);
            if (isExit == true) {
                $.messager.alert("错误", "已存在ID为(" + frmVal.KeyOfEn + ")的元素，不允许添加同名元素！", "error");
                return false;
            }

            //控件数据类型
            if (frmVal.FieldType == "1") {
                createdFigure.CCForm_Shape = "TextBoxStr";
            } else if (frmVal.FieldType == "2") {
                createdFigure.CCForm_Shape = "TextBoxInt";
            } else if (frmVal.FieldType == "3") {
                createdFigure.CCForm_Shape = "TextBoxFloat";
            } else if (frmVal.FieldType == "4") {
                createdFigure.CCForm_Shape = "TextBoxBoolean";
            } else if (frmVal.FieldType == "5") {
                createdFigure.CCForm_Shape = "TextBoxDouble";
            } else if (frmVal.FieldType == "6") {
                createdFigure.CCForm_Shape = "TextBoxDate";
            } else if (frmVal.FieldType == "7") {
                createdFigure.CCForm_Shape = "TextBoxDateTime";
            } else if (frmVal.FieldType == "8") {
                createdFigure.CCForm_Shape = "TextBoxMoney";
            }

            //如果为隐藏字段
            if (frmVal.IsHidenField == true) {
                HidenFieldFun(frmVal);
            } else {


                //根据信息创建不同类型的数字控件
                var transField = new TransFormDataField(createdFigure, frmVal, x, y);

                // 定义参数，让其保存到数据库里。
                var param = {
                    action: "DoType",
                    DoType: "NewField",
                    FrmID: CCForm_FK_MapData,
                    KeyOfEn: frmVal.KeyOfEn,
                    Name: frmVal.Name,
                    FieldType: frmVal.FieldType,
                    x: x,
                    y: y
                };

                ajaxService(param, function (json) {
                    if (json == "true") {
                        //开始画这个-元素.
                        transField.paint();

                    } else {
                        Designer_ShowMsg(json);
                    }
                }, this);
            }
        }, this.HidenFieldCreate);

        return false;
    },
    /**创建单选按钮**/
    RadioButtonCreate: function (createdFigure, x, y, dotype) {

        var dgId = "iframeRadioButton";
        var url = "DialogCtr/FrmEnumeration.htm?DataType=&s=" + Math.random();
        var funIsExist = this.IsExist;

        var lab = '单选按钮枚举值';
        if (dotype == 'DDL')
            lab = '下拉框枚举值';

        OpenEasyUiDialog(url, dgId, lab, 650, 394, 'icon-new', true, function () {

            var win = document.getElementById(dgId).contentWindow;
            var frmVal = win.GetFrmInfo();

            if (frmVal.Name == null || frmVal.Name.length == 0) {
                $.messager.alert('错误', '字段名称不能为空。', 'error');
                return false;
            }

            if (frmVal.KeyOfEn == null || frmVal.KeyOfEn.length == 0) {
                $.messager.alert('错误', '英文字段不能为空。', 'error');
                return false;
            }

            //判断主键是否存在
            var isExit = funIsExist(frmVal.KeyOfEn);
            if (isExit == true) {
                $.messager.alert("错误", "@已存在ID为(" + frmVal.KeyOfEn + ")的元素，不允许添加同名元素！", "error");
                return false;
            }

            if (dotype == "DDL")
                createdFigure.CCForm_Shape = "DropDownListEnum";
            else
                createdFigure.CCForm_Shape = "RadioButton";


            //根据信息创建不同类型的数字控件.
            var transField = new TransFormDataField(createdFigure, frmVal, x, y);

            // 定义参数，让其保存到数据库里。
            var param = {
                action: "DoType",
                DoType: "FrmEnumeration_NewEnumField",
                FK_MapData: CCForm_FK_MapData,
                Name: frmVal.Name,
                KeyOfEn: frmVal.KeyOfEn,
                UIBindKey: frmVal.UIBindKey,
                CtrlDoType: dotype,
                x: x,
                y: y
            };
            ajaxService(param, function (json) {
                if (json == "true") {

                    try {

                        //开始画这个 - 元素.
                        transField.paint();

                    } catch (e) {
                        alert('画元素错误：' + e);
                    }

                } else {
                    Designer_ShowMsg(json);
                }
            }, this);


        }, null);

        return false;
    },
    /**通用的No,Name 明细表，多附件，单附件 存储**/
    PublicNoNameCtrlCreate: function (createdFigure, x, y, ctrlType) {

        var dgId = "iframeRadioButton";
        var url = "DialogCtr/PublicNoNameCtrlCreate.htm?FrmID="+ CCForm_FK_MapData +"&CtrlType=" + ctrlType + "&s=" + Math.random();
        var funIsExist = this.IsExist;

        var lab = '创建从表';

        var note = '<ul>';

        switch (ctrlType) {
            case "Dtl":
                lab = "创建从表";
                break;
            case "Fieldset":
                lab = "创建分组";
                break;
            case "AthMulti":
                lab = "创建多附件";
                break;
            case "AthSingle":
                lab = "创建单附件";
                break;
            case "AthImg":
                lab = "创建图片附件";
                break;
            case "HandSiganture":
                lab = "签字板";
                break;
            case "iFrame":
                lab = "框架";
                break;
            default:
                alert('没有判断的控件类型PublicNoNameCtrlCreate:' + ctrlType);
                return;
        }

        OpenEasyUiDialog(url, dgId, lab, 650, 394, 'icon-new', true, function () {
            var win = document.getElementById(dgId).contentWindow;
            var frmVal = win.GetFrmInfo();

            if (frmVal.Name == null || frmVal.Name.length == 0) {
                $.messager.alert('错误', '名称不能为空。', 'error');
                return false;
            }

            if (frmVal.No == null || frmVal.No.length == 0) {
                $.messager.alert('错误', '编号不能为空。', 'error');
                return false;
            }

            //判断主键是否存在?
            var isExit = funIsExist(frmVal.No);
            if (isExit == true) {
                $.messager.alert("错误", "@已存在ID为(" + frmVal.No + ")的元素，不允许添加同名元素！", "error");
                return false;
            }

            createdFigure.CCForm_Shape = ctrlType;

            //根据信息创建不同类型的数字控件.
            var transField = new TransFormDataField(createdFigure, frmVal, x, y);

            // 定义参数，让其保存到数据库里。
            var param = {
                action: "DoType",
                DoType: "PublicNoNameCtrlCreate",
                CtrlType: ctrlType,
                FK_MapData: CCForm_FK_MapData,
                Name: frmVal.Name,
                No: frmVal.No,
                x: x,
                y: y
            };
            ajaxService(param, function (json) {
                if (json == "true") {
                    try {
                        //开始画这个 - 元素.
                        transField.paint();
                    } catch (e) {
                        alert(e);
                    }
                } else {
                    Designer_ShowMsg(json);
                }
            }, this);

        }, null);

        return false;
    },
    /**创建外部数据源下拉框**/
    DropDownListTableCreate: function (createdFigure, x, y) {

        var dgId = "iframeRadioButton";
        var url = "DialogCtr/FrmTable.htm?DataType=&s=" + Math.random();
        var funIsExist = this.IsExist;

        var lab = '外键表字段';

        OpenEasyUiDialog(url, dgId, lab, 850, 494, 'icon-new', true, function () {
            var win = document.getElementById(dgId).contentWindow;
            var frmVal = win.GetFrmInfo();

            if (frmVal.Name == null || frmVal.Name.length == 0) {
                $.messager.alert('错误', '字段名称不能为空。', 'error');
                return false;
            }

            if (frmVal.KeyOfEn == null || frmVal.KeyOfEn.length == 0) {
                $.messager.alert('错误', '英文字段不能为空。', 'error');
                return false;
            }

            //判断主键是否存在
            var isExit = funIsExist(frmVal.KeyOfEn);

            if (isExit == true) {
                $.messager.alert("错误", "@已存在ID为(" + frmVal.KeyOfEn + ")的元素，不允许添加同名元素！", "error");
                return false;
            }

            createdFigure.CCForm_Shape = "DropDownListTable";

            //根据信息创建不同类型的数字控件.
            var transField = new TransFormDataField(createdFigure, frmVal, x, y);

            // 定义参数，让其保存到数据库里。
            var param = {
                action: "DoType",
                DoType: "NewSFTableField",
                FK_MapData: CCForm_FK_MapData,
                Name: frmVal.Name,
                KeyOfEn: frmVal.KeyOfEn,
                UIBindKey: frmVal.UIBindKey,
                x: x,
                y: y
            };
            ajaxService(param, function (json) {
                if (json == "true") {

                    try {
                        //开始画这个 - 元素.
                        transField.paint();
                    } catch (e) {
                        alert('画元素失败');
                        alert(e);
                    }

                } else {
                    Designer_ShowMsg(json);
                }
            }, this);


        }, null);

        return false;
    },
    /**创建隐藏字段**/
    HidenFieldCreate: function (frmVal) {
        var param = {
            action: "DoType",
            DoType: "NewHidF",
            v1: CCForm_FK_MapData,
            v2: frmVal.KeyOfEn,
            v3: frmVal.Name,
            v4: frmVal.FieldType
        };
        ajaxService(param, function (json) {
            if (json == "true") {
            } else {
                Designer_ShowMsg(json);
            }
        }, this);
    },
    IsExist: function (MyPK) {
        var flag = false;
        for (f in STACK.figures) {
            if (STACK.figures[f].CCForm_MyPK == MyPK) {
                flag = true;
                break;
            }
        }
        return flag;
    },
    /**This method should be called every time the Command should be undone*/
    undo: function () {

        // if current figure is in text editing state
        if (state == STATE_TEXT_EDITING) {
            // remove current text editor
            currentTextEditor.destroy();
            currentTextEditor = null;
        }

        //remove it from container (if belongs to one)
        var containerId = CONTAINER_MANAGER.getContainerForFigure(this.figureId);
        if (containerId !== -1) {
            CONTAINER_MANAGER.removeFigure(containerId, this.figureId);
        }

        //remove figure
        STACK.figureRemoveById(this.figureId);

        //change state
        state = STATE_NONE;

        // set properties panel to canvas because current figure doesn't exist anymore
        setUpEditPanel(canvasProps);
    },

    /**创建流程控件 杨玉慧**/
    FlowFieldCreate: function (createdFigure, x, y, createFigureName) {
        //判断主键是否存在?
        createdFigure.CCForm_Shape = createFigureName;
        var frmVal = { Name: createFigureName, No: createFigureName }

        switch (createFigureName) {
            case "FrmCheck": // 审核组件.
                frmVal.Name = "审核组件";
                break;
            case "FlowChart": //轨迹图.
                frmVal.Name = "轨迹图";
                break;
            case "SubFlowDtl": //子流程.
                frmVal.Name = "子流程";
                break;
            case "ThreadDtl": //子流程.
                frmVal.Name = "子线程组件";
                break;
            case "FrmTransferCustom": //子流程.
                frmVal.Name = "流转自定义";
                break;
            default:
                alert('未定义类型:' + createFigureName);
                return; 
        }
        //根据信息创建不同类型的数字控件.
        var transField = new TransFormDataField(createdFigure, frmVal, x, y);

        // 定义参数，让其保存到数据库里。
        var param = {
            action: "DoType",
            DoType: "PublicNoNameCtrlCreate",
            CtrlType: createFigureName,
            FK_MapData: CCForm_FK_MapData,
            Name: frmVal.Name,
            No: frmVal.No,
            x: x,
            y: y
        };

        transField.paint();
        return false;
    }
}

/**数据字段处理
**图片：Image
**生成Label
**/
function TransFormDataField(newfigure, frmVal, x, y) {
    this.figure = newfigure;
    this.dataArrary = frmVal;
    this.x = x;
    this.y = y;
}


/*
* 绘制图形.
*/
TransFormDataField.prototype = {
    /** 画输出控件 **/
    paint: function () {
        var createdFigure = this.figure;
        //把主键给他.
        if (this.dataArrary.KeyOfEn != null)
            createdFigure.CCForm_MyPK = this.dataArrary.KeyOfEn;
        if (this.dataArrary.No != null)
            createdFigure.CCForm_MyPK = this.dataArrary.No;
        //添加到Figures
        //add to STACK
        STACK.figureAdd(createdFigure);
        //add property  增加属性.
        createdFigure = this.Transform();
        //change text  //设置控件上的ID文本.
        var figureText = STACK.figuresTextPrimitiveGetByFigureId(createdFigure.id);
        if (figureText != null && createdFigure.CCForm_Shape == "TextBoxBoolean") {//除了复选框，其余的都不写TEXT
           if (this.dataArrary.Name != null)
                figureText.setTextStr(this.dataArrary.Name);
            //if (this.dataArrary.No != null)
            //    figureText.setTextStr(this.dataArrary.No);
        } //创建标签
        this.LabelCreateForFigure();
        draw();
        if (createdFigure.CCForm_Shape == "RadioButton") {
            var rbArr = this.dataArrary.Vals.slice(1).split("@");
            var s = [];
            for (var i = 0; i < rbArr.length; i++) {
                s.push("RB_" + this.dataArrary.UIBindKey + "_" + rbArr[i]);
            }
            for (var k = 0; k < s.length; k++) {
                if (this.dataArrary.UIBindKey != null) {
                    createdFigure.CCForm_MyPK = s[k];
                }
                this.y += 24;
                STACK.figureAdd(createdFigure);
                if (createdFigure.name == "Label") {
                    createdFigure = this.Transform();
                }
                var figureText = STACK.figuresTextPrimitiveGetByFigureId(createdFigure.id);
                if (figureText != null) {
                    if (this.dataArrary.KeyOfEn != null)
                        figureText.setTextStr(this.dataArrary.KeyOfEn);
                    this.LabelCreateForFigure();
                    draw();
                }
            }
        }
    },
    /**根据控件类型，生成不同控件描述 and propertys**/
    Transform: function () {
        var createdFigure = this.figure;
        var propertys = CCForm_Control_Propertys.TextBox_Str;
        var shap_src = null;

        shap_src = "/DataView/" + createdFigure.CCForm_Shape + ".png";

        //  alert(shap_src);
        //  alert(shap_src);
        //  alert(figureSetsURL);
        //  alert(figureSetsURL + shap_src);

        propertys = CCForm_Control_Propertys[createdFigure.CCForm_Shape];
        //shap image
        if (createdFigure.CCForm_Shape != "RadioButton") {
            var imageFrame = STACK.figuresImagePrimitiveGetByFigureId(createdFigure.id);
            if (imageFrame != null) {
                //alert(figureSetsURL + shap_src);
                imageFrame.setUrl(figureSetsURL + shap_src);
            }
        }

        // alert(figureSetsURL + shap_src);

        var ctrlLab = '控件属性';
        switch (createdFigure.CCForm_Shape) {
            case "Dtl":
                ctrlLab = '从表/明细表属性';
                break;
            case "AthMulti":
                ctrlLab = '多附件属性';
                break;
            case "AthSingle":
                ctrlLab = '单附件属性';
                break;
            case "TextBoxStr":
                ctrlLab = '控件属性-文本框';
                break;
            case "FlowChart":
                ctrlLab = '控件属性-轨迹图';
            default:
                ctrlLab = '控件属性' + createdFigure.CCForm_Shape;
                break;
        }

        //push property
        createdFigure.properties.push(new BuilderProperty(ctrlLab, 'group', BuilderProperty.TYPE_GROUP_LABEL));
        createdFigure.properties.push(new BuilderProperty(BuilderProperty.SEPARATOR));

        for (var i = 0; i < propertys.length; i++) {

            var defVal = propertys[i].DefVal ? propertys[i].DefVal : "";

            switch (defVal) {
                case "No":  // 编号
                    defVal = this.dataArrary.No;
                    break;
                case "Name":  // 名称
                    defVal = this.dataArrary.Name;
                    break;
                case "FieldText":  // 字段中文名
                    defVal = this.dataArrary.Name;
                    break;
                case "KeyOfEn":    // 字段名.
                    if (createdFigure.CCForm_Shape == "RadioButton") {
                        this.dataArrary.KeyOfEn = "";
                    }
                    defVal = this.dataArrary.KeyOfEn;
                    break;
                case "UIBindKey":  // 绑定的外键.
                    defVal = this.dataArrary.UIBindKey;
                    break;
                default:
                    break;
            }

            //替换系统值
            defVal = this.DealExp(defVal);

            //增加一个属性, 放到属性面板里.
            createdFigure.properties.push(new BuilderProperty(propertys[i].ProText, propertys[i].proName, propertys[i].ProType, defVal));
        }

        return createdFigure;
    },
    /**创建控件对应的标签**/
    LabelCreateForFigure: function () {
        var x = this.x;
        var y = this.y;
        //计算位移
        var moveX = (this.dataArrary.Name.length * 12) + 60;
        moveX = moveX < 90 ? 90 : moveX;
        x = x - moveX;
        y = y - 15;
        //假如X,Y <5px 会靠边看不到，设置为5px;
        if (x < 0) {
            x = 5;
        }
        if (y < 0) {
            y = 5;
        }
        //create
        var createdFigure = figure_Label(x, y);
        createdFigure.CCForm_MyPK = Util.NewGUID();
        createdFigure.CCForm_Shape = CCForm_Controls.Label;

        //store id for later use
        //TODO: maybe we should try to recreate it with same ID (in case further undo will recreate objects linked to this)
        this.figureId = createdFigure.id;

        //add to STACK
        STACK.figureAdd(createdFigure);

        //make this the selected figure
        selectedFigureId = createdFigure.id;

        //set up it's editor
        setUpEditPanel(createdFigure);

        //move to figure selected state
        state = STATE_FIGURE_SELECTED;

        //change text
        figureText = STACK.figuresTextPrimitiveGetByFigureId(selectedFigureId);
        if (figureText != null) {
            if (this.figure.CCForm_Shape == "RadioButton") {
                figureText.setTextStr(" * " + this.figure.CCForm_MyPK.split("=")[1]);
                createdFigure.CCForm_MyPK = this.figure.CCForm_MyPK.split("=")[0];
                if (figureText.str == " * undefined") {
                    figureText.setTextStr(this.dataArrary.Name);
                } else {
                    createdFigure.CCForm_Shape = this.figure.CCForm_Shape;
                    var propertys = CCForm_Control_Propertys.TextBox_Str;
                    propertys = CCForm_Control_Propertys[createdFigure.CCForm_Shape];
                    var ctrlLab = '控件属性';
                    switch (createdFigure.CCForm_Shape) {
                    case "Dtl":
                        ctrlLab = '从表/明细表属性';
                        break;
                    case "AthMulti":
                        ctrlLab = '多附件属性';
                        break;
                    case "AthSingle":
                        ctrlLab = '单附件属性';
                        break;
                    case "TextBoxStr":
                        ctrlLab = '控件属性-文本框';
                        break;
                    default:
                        ctrlLab = '控件属性' + createdFigure.CCForm_Shape;
                        break;
                    }

                    //push property
                    createdFigure.properties
                        .push(new BuilderProperty(ctrlLab, 'group', BuilderProperty.TYPE_GROUP_LABEL));
                    createdFigure.properties.push(new BuilderProperty(BuilderProperty.SEPARATOR));

                    for (var i = 0; i < propertys.length; i++) {

                        var defVal = propertys[i].DefVal ? propertys[i].DefVal : "";

                        switch (defVal) {
                        case "No": // 编号
                            defVal = this.dataArrary.No;
                            break;
                        case "Name": // 名称
                            defVal = this.dataArrary.Name;
                            break;
                        case "FieldText": // 字段中文名
                            defVal = this.dataArrary.Name;
                            break;
                        case "KeyOfEn": // 字段名.
                            if (createdFigure.CCForm_Shape == "RadioButton") {
                                this.dataArrary.KeyOfEn = "";
                            }
                            defVal = this.dataArrary.KeyOfEn;
                            break;
                        case "UIBindKey": // 绑定的外键.
                            defVal = this.dataArrary.UIBindKey;
                            break;
                        default:
                            break;
                        }

                        //替换系统值
                        defVal = this.DealExp(defVal);

                        //增加一个属性, 放到属性面板里.
                        createdFigure.properties
                            .push(new BuilderProperty(propertys[i].ProText,
                                propertys[i].proName,
                                propertys[i].ProType,
                                defVal));
                    }
                }
            } else {
                figureText.setTextStr(this.dataArrary.Name);
            }
        }
    },
    /**替换系统表达式值**/
    DealExp: function (expString) {
        try {
            expString = expString.replace(/@FrmID@/g, CCForm_FK_MapData);

            if (this.dataArrary.No != null) {
                expString = expString.replace(/@KeyOfEn@/g, this.dataArrary.No);
                expString = expString.replace(/@No@/g, this.dataArrary.No);
            }

            if (this.dataArrary.KeyOfEn != null) {
                expString = expString.replace(/@KeyOfEn@/g, this.dataArrary.KeyOfEn);
                expString = expString.replace(/@No@/g, this.dataArrary.KeyOfEn);
            }

        } catch (e) {
        }
        return expString;
    }
}

function CrateRB(createdFigure, dataArrary) {
   //把主键给他.
    if (this.dataArrary.KeyOfEn != null)
        createdFigure.CCForm_MyPK = this.dataArrary.KeyOfEn;
    if (this.dataArrary.No != null)
        createdFigure.CCForm_MyPK = this.dataArrary.No;

    //添加到Figures
    //add to STACK
    STACK.figureAdd(createdFigure);

    //add property  增加属性.
    createdFigure = this.Transform();

    //change text  //设置控件上的ID文本.
    var figureText = STACK.figuresTextPrimitiveGetByFigureId(createdFigure.id);
    if (figureText != null) {

        if (this.dataArrary.KeyOfEn != null)
            figureText.setTextStr(this.dataArrary.KeyOfEn);

        if (this.dataArrary.No != null)
            figureText.setTextStr(this.dataArrary.No);
    }

    //创建标签
    this.LabelCreateForFigure();
    draw();
}