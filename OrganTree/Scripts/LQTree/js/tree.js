
/**
 * 参数{ container: "contarner", shrinkCallBack: shrinkCallBack, SelDBCallBack: window.parent["@(Request.QueryString["callback"])"] }
 *  container容器id   shrinkCallBack展开收缩回调函数 SelDBCallBack选择节点回调函数
 */

var lqTree = (function () {
    //节点模板
    var treeNodetemp = "<li  class='parent_li'><span title='Expand this branch' recoreid='{{id}}'><i class='splashy-add_small' style=\"width:23px;\"></i><i title=\"log\" class=\"{{icon}}\"></i>{{name}}</span> <a href=''></a></li>";

    function lQTree(parameter) {
        var th = this;
        th.selDatas = [];
        $("#" + parameter.container + " .tree").remove();
        this.$container = parameter.container ? $("#" + parameter.container) : null;
        this.loadParamer(parameter);
        this.$container.append("<div class=\"tree well2\"><ul></ul></div>");//加载树的初始结构
        this.$treeContraine = this.$container.children().children("ul");
    }
    //将初始化参数加载到当前对象
    lQTree.prototype.loadParamer = function (parameter) {
        for (var parameterName in parameter) {
            this[parameterName] = parameter[parameterName];
        }
    }
    //在指定节点下添加子节点。如果没有传则默认在父容器下  data 结构 必须包含这2个属性{RecordID:"",Name:""}
    lQTree.prototype.AddNode = function (data, $parentnode) {
        var treeNodehtml = treeNodetemp.replace("{{id}}", data.RecordID).replace("{{name}}", data.Name).replace("{{icon}}", data.Icon || "");
        var $node = $(treeNodehtml);
        if ($parentnode) {
            if ($parentnode[0].data.RecordID == "-1"||($parentnode[0].data.RecordID != "-1"&&!$parentnode[0].data.DetailText)) {
                data.DetailText = data.Name;
            } else {
                data.DetailText = $parentnode[0].data.DetailText+ "=>" + data.Name;
            }
        }
        $node[0].data = data;
        BindNodeShrinkEvent.call(this,$node); //为当前节点绑定收缩事件
        BindNodeSelctEvent.call(this,$node);
        if (!$parentnode) {
            this.$treeContraine.append($node);
            return $node;
        } else {
            $node.css("display", "none");
            var children = $parentnode.find(' > ul > li');;
            if (children.length == 0 && $(this).parent().attr["title"] != "Collapse this branch") {
                var $ul = $("<ul></ul>");
              $ul.append($node);
              $parentnode.append($ul);
            } else {

                $parentnode.children("ul").append($node);
            }


        }
        return $node;

    }
    //展开指定节点
    lQTree.prototype.Open = function($node) {
             var children = $node.find(' > ul > li');
             children.show('fast');
             $node.find("> span i[title!='log']").addClass('splashy-remove_minus_sign_small').removeClass('splashy-add_small');
    }
    //删除指定节点
    lQTree.prototype.RemoveNode = function ($node)
    {
        $node.siblings("ul").remove();
        $node.parent().remove();
    }
   //根据RecoredId会的节点对象
    lQTree.prototype.GetNodeById=function(recoredid) {
      return  this.$treeContraine.find("span[recoreid='" + recoredid + "']").parent();
    }
    //绑定收缩事件
    function BindNodeShrinkEvent($node) {
        var th = this;
        $node.children().children("i").click(function(e) {
            shrink.call(this,th,e);
        });
    }
    //绑定指定节点的选中事件
    function BindNodeSelctEvent($node) {
        var th = this;
        $node.children("span").click(function(e) {
            selectNode.call(this,th,e);
        });
    }

    //收缩所有节点
    lQTree.prototype.ShrinkAll = function () {
        $(".tree li > span[title='Collapse this branch'] ~ ul li").hide('fast').parent().prevAll("span").children("i").addClass('splashy-add_small').removeClass('splashy-remove_minus_sign_small');
        this.selDatas = [];
    }
    lQTree.prototype.GetSelDatas=function() {
        return this.selDatas;
    }
    //收缩节点事件
    function shrink(th,e) {
        var children = $(this).parent().parent('li.parent_li').find(' > ul > li');
        //如果是最后一个节点则通过ajax请求对应数据
        if (children.length == 0 && $(this).parent().attr["title"] != "Collapse this branch") {
            th.shrinkCallBack($(this).parent().parent()[0].data);
            e.stopPropagation();
            return;
        }
        if (children.is(":visible")) {
            children.hide('fast');
            $(this).parent().attr('title', 'Expand this branch').find(" > i[title!='log']").addClass('splashy-add_small').removeClass('splashy-remove_minus_sign_small');
        } else {
            children.show('fast');
            $(this).parent().attr('title', 'Collapse this branch').find(" > i[title!='log']").addClass('splashy-remove_minus_sign_small').removeClass('splashy-add_small');
        }
        e.stopPropagation();
    }
    //选中节点事件
    function selectNode(th, e) {

        if (th.multiSel) {
            th.$selectNode = $(this);
            var data = th.$selectNode.parent()[0].data;
            if ($(this).hasClass("selected")) {
                $(this).removeClass("selected");
                for(var i=0;i<th.selDatas.length;i++) {
                    if (th.selDatas[i].RecordID == data.RecordID) {
                        th.selDatas.splice(i, 1);
                        return;
                    }
                }
               
            } else {
              
               
     
                if (th.selDatas.length > 0 && th.selDatas.every(function (c) { return c.ParentId == data.ParentId })) {
                    th.selDatas[th.selDatas.length] = data;
                  
                } else {
                    //把原来选中的清空选中
                    $("#" +th.container).find(".selected").removeClass("selected");
                    th.selDatas = [];
                    th.selDatas[th.selDatas.length] = data;
                }
                $(this).addClass("selected");
            }

        } else {
            if (th.$selectNode) {
                th.$selectNode.removeClass("selected");
            }
            th.$selectNode = $(this);
            $(this).addClass("selected");
            th.dbnum++;
            if (th.dbnum >= 2) {
                //双击事件
                if (th.SelDBCallBack) {
                    th.SelDBCallBack(th.$selectNode.parent()[0].data);
                }
            }
          
            setTimeout(function() {
                th.dbnum = 0;
            }, 500);
        }
        e.stopPropagation();
    }
   
    return lQTree;
})();