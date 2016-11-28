window.Areca= {
    AjaxPorcess: function (data, OKCallback, NoCallBack, NoJurisdiction) {
        var ajaxObj = data;
        if (typeof data == "string") {
            ajaxObj = $.parseJSON(data);
        }

        if (ajaxObj.State == 0) {
            //成功状态处理
            if (OKCallback) {
                OKCallback(ajaxObj);
            }
        }
        else if (ajaxObj.State == 1) {
            if (NoCallBack) {
                NoCallBack(ajaxObj);
            }
            if (ajaxObj.Message != "" && ajaxObj.Message) {
                Areca.ErrorMessage("温馨提示", ajaxObj.Message);
            }


        } else if (ajaxObj.State == 2) {
            if (NoJurisdiction) {
                NoJurisdiction(ajaxObj);
            }
            if (ajaxObj.Message != "" && ajaxObj.Message) {
                Areca.ErrorMessage("温馨提示", ajaxObj.Message);
            }
        }
    },
    ErrorMessage: function (title, message) {
        // $("#ErrorMessageModal .modal-body").html(message);
        // $("#ErrorMessageModal .modal-header h3").html(title);
        //var model= $("#ErrorMessageModal").modal({
        //     backdrop: "static",
        //     keyboard: true
        //}).modal('show');
        //setTimeout(function () {

        //    model.modal("hide");
        // },1500)
        layer.alert(message, {
            shadeClose: true,
            title: title,
        });
    },  //初始化一个模态框加载指定div的id  left="距离左侧百分比 默认50%"
    InitializeModel: function (title, id, width, height, left, top) {
        // if (!left) {
        //     left = "50";
        // }
        // if (!top) {
        //     top = "50";
        // }
        // ModelStateIndex++;
        // var guid = "myModal" + ModelStateIndex;
        // var contId = "content" + ModelStateIndex;
        // var html = "<div id=\"" + guid + "\" class=\"modal hide fade\" tabindex=\"-1\" role=\"dialog\" aria-labelledby=\"myModalLabel\" aria-hidden=\"true\">" +
        //           "<div class=\"modal-header\">" +
        //           "<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">×</button>" +
        //           "<h3 id=\"myModalLabel\">" + title + "</h3>" +
        //           "</div>" +
        //           "<div class=\"modal-body\" id=\"" + contId + "\"></div>" +
        //           "</div>";
        // var mode = $(html).appendTo($("body"));
        //$("#" + guid).css("min-height", height);
        // $("#" + contId).css("min-height", height);
        // //$("#" + guid).css("width", width);
        // //$("#" + guid).css("left", left + "%");
        // $("#" + guid).css("top", top + "%");

        // $("#" + contId).html($("#" + id).children("div").children("div"));
        // $("#" + guid).on("hidden", function () {
        //     //初始化所有表单信息
        //     $("#" + guid).find("input[type='text']").val("");
        //     $("#" + guid).find("textarea").val("");
        //     $("#" + guid).find("select").find("option").first().attr("selected", true);
        //     $("#" + guid).find("input[type='checkbox']").prop("checked", false);
        // });

        // //模态框可拖动
        // mode.draggable({
        //     cursor: 'move',
        //     refreshPositions: false
        // }).css({ width: width, 'min-height': height, 'margin-left': function () { return -($(this).width() / 2); } });
        // return mode;
        //页面层
        var model = new Object();
        //如果是字符串则当id处理 如果不是则当dom元素处理
        if (typeof id == "string" && id.constructor == String) {
            model.demo = $("#" + id);
        } else {
            model.demo = id;
        }
        model.width = width;
        model.height = height;

        model.demo.hide();
        model.on = function (type, callback) {

            if (type == "hide") {
                model.closeCallback = callback;
            }
            if (type == "show") {

                model.success = callback;
            }
            return this;
        }
        model.modal = function (type) {
            var th = this;
            if (type == "show") {
                model.demo.show();
                var showdeom = model.demo;//.children("div").children("div");
                this.index = layer.open({
                    title: title,
                    type: 1,
                    skin: 'layui-layer-rim', //加上边框
                    area: [th.width + "px", th.height + "px"], //宽高
                    content: showdeom,
                    end: th.closeCallback,
                    success: model.success || null
                });

            } else {
                layer.close(this.index);
            }
            return this;
        }
        model.title = function (title) {
            layer.title(title, this.index);
        }
        return model;


    }
}

//嵌入指定容器的机构树信息
/**
 * 依赖文件(按顺序引入
 *<link rel="stylesheet" href="@BaseHelper.PlugUrlContent("/LQTree/css/tree.css")" /> 
 * <script src="@BaseHelper.PlugUrlContent("/LQTree/js/tree.js")"></script>
 *<script src="@BaseHelper.AreasUrlContent("/Js/SelOrgan.js")"></script>
 * 快速使用例子
     var test = new OrganTree({ container: "test"//你的容器id, parentid: "", SelDBCallBack: function (d) {
                  console.log(d);//双击选中后的回调函数d为数据
            } })
Options 参数：
SelDBCallBack: 选择回调函数
parentid:指定加载父级id(如果没有传空字符串 默认加载整个机构树，比如需要加载a公司 以下子公司以及部门信息 则传a公司的recoredid)
validateOpractionCodes 权限验证 第一个查看所有 查看同级 查看部门以下
tokenCreateTime token的生成日期
token 验证和日期+服务器密码 算出的md5散列值
常用方法：

 调用ShrinkAll 函数 收缩整个树结构
 */
var OrganTree = (function () {
    var getOrganDataUrl = "/Home/GetOrganByParentId";//获得机构数据的url
    var getDeparmentUrl = "/Home/GetDeparmentByParentId";//获得部门数据的url
 
    function OrganTree(options) {
      
        var th = this;

         this.parameer = options;
         //构建一个树结构
         this.tree = new lqTree({ container: options.container, shrinkCallBack: LoadTreeByParent, SelDBCallBack: options.SelDBCallBack, multiSel: options.multiSel||"" });
         if (options.hasOwnProperty("parentid")) {
            LoadTreeByParent({ RecordID: options.parentid || "", Type: options.type || "organ" });
         }
         function LoadTreeByParent(data) {
             var loadDataUrl = data.Type === "organ" ? getOrganDataUrl : getDeparmentUrl;//根据类型判断 加载数据的url
             $.ajax({
                 type: "POST",
                 url: loadDataUrl,
                 data: { parentid: data.RecordID, validateOpractionCodes: th.parameer.validateOpractionCodes, tokenCreateTime: th.parameer.tokenCreateTime, token: th.parameer.token },
                 success: function (ajadata) {
                     Areca.AjaxPorcess(ajadata,
                         function (ajaxObj) {
                             if (ajaxObj.Data.length <= 0 || (ajaxObj.Data[0].RecordID == data.RecordID && th.tree.GetNodeById(data.RecordID).length > 0)) {
                                 Areca.ErrorMessage("温馨提示", "没有了");
                                 return;
                             }
                             ajaxObj.Data.forEach(function (c) {
                                 var $parentNode = th.tree.GetNodeById(data.RecordID);
                                 th.tree.AddNode(c, $parentNode.length == 0 ? null : $parentNode);
                                 th.tree.Open($parentNode);
                             })


                         });
                 }
             });
         }
    }
    OrganTree.prototype.ShrinkAll=function() {
        this.tree.ShrinkAll();
    }
    OrganTree.prototype.AddNode=function(data,$parentDom) {
        this.tree.AddNode(data, $parentDom);
    }
    //根据RecoredId会的节点对象
    OrganTree.prototype.GetNodeById=function(recoredid) {
        return this.tree.GetNodeById(recoredid);
    }
 

    return OrganTree;
})();
//可搜索的机构树形组件
/**
 * 依赖文件(按顺序引入
 *<link rel="stylesheet" href="@BaseHelper.PlugUrlContent("/LQTree/css/tree.css")" /> 
 * <script src="@BaseHelper.PlugUrlContent("/LQTree/js/tree.js")"></script>
 *<script src="@BaseHelper.AreasUrlContent("/Js/SelOrgan.js")"></script>
 * 快速使用例子
     var test = new OrganSerachTree({ container: "test"//你的容器id, parentid: "", SelDBCallBack: function (d) {
                  console.log(d);//双击选中后的回调函数d为数据
            } })

Options 参数
validateOpractionCodes 权限验证 第一个查看所有 查看同级 查看部门以下
tokenCreateTime token的生成日期
token 验证和日期+服务器密码 算出的md5散列值
SelDBCallBack: 选择回调函数
parentid:指定加载父级id(如果传空字符串默认加载整个机构树，比如需要加载a公司 以下子公司以及部门信息 则传a公司的recoredid)

 */

var OrganSerachTree = (function () {
    var serchurl = "/Home/SerchOragnInfoByName";//搜索部门机构信息查询参数
    //基础模板
    var htmlTemp = "<div class='treetool'><span>搜索：</span><input type=\"text\" class=\"search\" style='width:150px;'></div><div class='content'></div>";

    function OrganSerachTree(options) {
        var  th = this;
        this.options = options;
        $("#" + options.container).append(htmlTemp);
        //树形搜索框继承机构树类
        OrganTree.call(this, { container: th.options.container, parentid: th.options.parentid, SelDBCallBack: th.options.SelDBCallBack, validateOpractionCodes: th.options.validateOpractionCodes, tokenCreateTime: th.options.tokenCreateTime, token: th.options.token, multiSel: th.options.multiSel || "" });
        this.Model = Areca.InitializeModel("请选择", options.container, 400, 650).on("hide", function() {
            th.tree.ShrinkAll(); //每次分类窗口关闭 收缩所有节点
            $("#" + th.options.container + " .tree").remove();
            $("#" + th.options.container).find(".search").val("");
            OrganTree.call(th, { container: th.options.container, parentid: th.options.parentid, SelDBCallBack: th.options.SelDBCallBack, validateOpractionCodes: th.options.validateOpractionCodes, tokenCreateTime: th.options.tokenCreateTime, token: th.options.token, multiSel: th.options.multiSel || "" });
        });
       
        BindEvent();
        //绑定事件
        function BindEvent() {
            var topCode;
            $("#" + th.options.container).find(".search").keyup(function () {
                var text = $(this).val();
                if (topCode) window.clearTimeout(topCode);//每次清空上一次的检查
                topCode = setTimeout(function () {

                    SeachOragan(text);
                }, 1000);
            })
        }
        function SeachOragan(text) {
            if (!text) {
                $("#" + th.options.container + " .tree").remove();
                $("#" + th.options.container).find(".search").val("");
                //重新构建机构树
                OrganTree.call(th, { container: th.options.container, parentid: th.options.parentid, SelDBCallBack: th.options.SelDBCallBack, validateOpractionCodes: th.options.validateOpractionCodes, tokenCreateTime: th.options.tokenCreateTime, token: th.options.token, multiSel: th.options.multiSel || "" });
                return;
            }
          
       
            $("#" + th.options.container + " .tree").text("正在查询，请稍后...");
         
            $.ajax({
                type: "POST",
                url: serchurl,
                data: { name: text, validateOpractionCodes: th.options.validateOpractionCodes, tokenCreateTime: th.options.tokenCreateTime, token: th.options.token, multiSel: th.options.multiSel || "" },
                success: function (ajadata) {
                  
                    Areca.AjaxPorcess(ajadata,
                        function (ajaxObj) {
                            if ((!ajaxObj.Data.organs || ajaxObj.Data.organs.length <= 0) && (!ajaxObj.Data.deparments || ajaxObj.Data.deparments.length <= 0)) {
                                $("#" + th.options.container + " .tree").text("没有找到到相关记录");
                                return;
                            }
                            //构建一个基础的树结构
                            OrganTree.call(th,{ container: th.options.container, SelDBCallBack: th.options.SelDBCallBack, validateOpractionCodes: th.options.validateOpractionCodes, tokenCreateTime: th.options.tokenCreateTime, token: th.options.token, multiSel: th.options.multiSel || "" });
                            LoadOrganTree(null, ajaxObj.Data);
                            LoadDeparmentTree(null, ajaxObj.Data);
                        });
                }
            });
        }
        //递归调用所有父机构信息
        function LoadOrganTree(parentData, datas) {
            if (!parentData) {
  
                datas.organs.forEach(function (c) {
                    //机构数据 没有父类数据的开始递归加载
                    if (datas.organs.filter(function (d) { return c.ParentId == d.RecordID }).length <= 0) {
                        var data = { RecordID: c.RecordID, Name: c.OrganName, Type: "organ", Icon: c.Icon };
                        var node = th.tree.AddNode(data);
                        LoadOrganTree(data, datas);
                    }
                });
                //开始加载子机构
            } else {

                var sons = datas.organs.filter(function (c) { return c.ParentId == parentData.RecordID});
                sons.forEach(function (c) {
                    var data = { RecordID: c.RecordID, Name: c.OrganName, Type: "organ", Icon: c.Icon };
                    var $parentNode = th.tree.GetNodeById(parentData.RecordID);
                    var node = th.tree.AddNode(data, $parentNode);
                    console.log(data);
                    LoadOrganTree(data, datas);
                    th.tree.Open($parentNode);
                });
            }
        }
        //递归加载所有子节点信息
        function LoadDeparmentTree(parentData, datas) {
            if (!parentData) {
                datas.deparments.forEach(function (c) {
                    //机构数据 没有父类数据的开始递归加载
                    if (datas.deparments.filter(function (d) { return c.ParentId == d.RecordID }).length <= 0) {

                        var data = { RecordID: c.RecordID, Name: c.DeptName, Type: "deparment", OrganId: c.OrganId, Icon: c.Icon };
                        var $parentiNode = th.tree.GetNodeById(data.OrganId);
                        var node = th.tree.AddNode(data, $parentiNode);
                        LoadDeparmentTree(data, datas);
                        th.tree.Open($parentiNode);
                    }
                });
            } else {

                var sons = datas.deparments.filter(function (c) { return c.ParentId == parentData.RecordID });
                sons.forEach(function (c) {
                    var data = { RecordID: c.RecordID, Name: c.DeptName, Type: "deparment", OrganId: c.OrganId,Icon:c.Icon };
                    var $parentNode = th.tree.GetNodeById(parentData.RecordID);
                    var node = th.tree.AddNode(data, $parentNode);
                    LoadOrganTree(data, datas);
                    th.tree.Open($parentNode);
                });
            }
        }
    }
    //继承机构树的原型对象
    OrganSerachTree.prototype = Object.create(OrganTree.prototype);
  

    
    return OrganSerachTree;
})();
//可搜索的机构树形组件
/**
 * 依赖文件(按顺序引入
 *<link rel="stylesheet" href="@BaseHelper.PlugUrlContent("/LQTree/css/tree.css")" /> 
 * <script src="@BaseHelper.PlugUrlContent("/LQTree/js/tree.js")"></script>
 *<script src="@BaseHelper.AreasUrlContent("/Js/SelOrgan.js")"></script>
 * 快速使用例子
     var test = new OrganSerachTreeModel({ container: "test"//你的容器id, parentid: "",validateOpractionCodes:[], tokenCreateTime: "", token:""), SelDBCallBack: function (d) {
                  console.log(d);//双击选中后的回调函数d为数据
            } })
Options 参数
SelDBCallBack: 选择回调函数
validateOpractionCodes 权限验证 第一个查看所有 查看同级 查看部门以下
tokenCreateTime token的生成日期
token 验证和日期+服务器密码 算出的md5散列值
parentid:指定加载父级id(如果传空字符串默认加载整个机构树，比如需要加载a公司 以下子公司以及部门信息 则传a公司的recoredid)

方法
ShowModel();//打开模态框
HideModel();//隐藏模态框
 */
var OrganSerachTreeModel = (function() {
    function OrganSerachTreeModel(options) {
        var th = this;
        //继承可搜索的树
        OrganSerachTree.call(this, options);
        this.Model = Areca.InitializeModel("请选择", options.container, 400, 650).on("hide", function() {
            th.tree.ShrinkAll(); //每次分类窗口关闭 收缩所有节点
            $("#" + th.options.container + " .tree").remove();
            $("#" + th.options.container).find(".search").val("");
            OrganTree.call(th, { container: th.options.container, parentid: th.options.parentid, SelDBCallBack: th.options.SelDBCallBack, validateOpractionCodes: th.options.validateOpractionCodes, tokenCreateTime: th.options.tokenCreateTime, token: th.options.token, multiSel: th.options.multiSel || "" });
        });

    }

    //继承可搜索树的构造方法的原形对象
    OrganSerachTreeModel.prototype = Object.create(OrganSerachTree.prototype);
    OrganSerachTreeModel.prototype.ShowModel = function() {

        this.Model.modal("show");

    }
    OrganSerachTreeModel.prototype.HideModel = function() {
        //每次关闭重新加载数据

        this.Model.modal("hide");
    }
    return OrganSerachTreeModel;
})();

/**
 * 多选可权限控制的组织机构树组件
 * 依赖文件(按顺序引入
 *<link rel="stylesheet" href="@BaseHelper.PlugUrlContent("/LQTree/css/tree.css")" /> 
 * <script src="@BaseHelper.PlugUrlContent("/LQTree/js/tree.js")"></script>
 *<script src="@BaseHelper.AreasUrlContent("/Js/SelOrgan.js")"></script>
 * 快速使用例子
     var test = new OrganSerachTreeModel({ container: "test"//你的容器id, parentid: "",validateOpractionCodes:[], tokenCreateTime: "", token:""), SelDBCallBack: function (d) {
                  console.log(d);//双击选中后的回调函数d为数据
            } })
Options 参数
SelDBCallBack: 选择回调函数
validateOpractionCodes 权限验证 第一个查看所有 查看同级 查看部门以下
tokenCreateTime token的生成日期
token 验证和日期+服务器密码 算出的md5散列值
parentid:指定加载父级id(如果传空字符串默认加载整个机构树，比如需要加载a公司 以下子公司以及部门信息 则传a公司的recoredid)

方法
ShowModel();//打开模态框
HideModel();//隐藏模态框
**/
var MultiOrganSerachTreeModel = (function() {
    function MultiOrganSerachTreeModel(options) {
        var th = this;
        options.multiSel = true;
        OrganSerachTreeModel.call(this, options);
       
        $("#" + options.container).find(".treetool").append("<div style='display:inline-block; margin-left:43px;'><a style='margin-top: -13px;' class='okSel btn'>确认选择</a></div>");
        BindeEvent();
        function BindeEvent() {
            $("#" + options.container).find("[class*='okSel']").click(function () {
                var datas = th.tree.GetSelDatas();
                if (datas.length <= 0) {
                    Areca.ErrorMessage("温馨提示", "请先选择项");
                    return;
                }
                th.options.SelDBCallBack(datas);
            });
        }
    }

    MultiOrganSerachTreeModel.prototype = OrganSerachTreeModel.prototype;
    return MultiOrganSerachTreeModel;
})();