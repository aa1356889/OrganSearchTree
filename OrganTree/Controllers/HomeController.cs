using Areca.Public.Binder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Mvc;
using Areca.Areas.Admin.Controllers;
using Energy.Business;
using Energy.Entity;
using Hammer.Common;
using Energy.Enum;
using Energy.Share;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using StackExchange.Profiling.EntityFramework6;

namespace OrganTree.Controllers
{
    public class HomeController : MyControllerBase
    {

        public HomeController()
        {
            UserInfo = new UserTicket();
            UserInfo.OrganId = "D8A162EF62D94D4B8B7DFAF2CC98AAA9";//机构id
            UserInfo.DeptId = "65D88F4807164FC5B6AF86805E7FD2FB";//部门id
            UserInfo.OperationTable = new List<Operation>()
            {
                new Operation() {OperationName = "khgl_accreditA_001"},//默认当前用户有最大权限
            };
        }
        // GET: Home
        public ActionResult Index()
        {
          
               //组织结构验证权限所需数据
               var operactionCode = new string[]
            {
                "khgl_accreditA_001",//最大权限
                "khgl_accreditC_001",//同级权限
                "khgl_accreditB_001",//部门以及部门以下
            };
            var datettime = DateTime.Now.ToString();
            var operactionCodeStr = JSONHelp.SerializeObject(operactionCode);
            var token = Hammer.Encrypt.MD5.Encode(operactionCodeStr + datettime + TokenKeysEnum.TokenPassword);
            ViewData["tokenCreateTime"] = datettime;
            ViewData["code"] = operactionCodeStr;
            ViewData["token"] = token;
            var profiler = MiniProfiler.Current;
            return View();
        }
     
        private OrganBusiness organBusiness = new OrganBusiness();
        private DepartmentBusiness departmentBusiness = new DepartmentBusiness();
        /// <summary>
        /// 根据机构父id获得机构信息 如果没有 尝试加载部门（调用此接口 请在下方注明）
        /// 2016-11-21 新增 李强
        /// 2016-11-22 修改  改为涉及权限验证的组织机构树
        /// 用于SelOrgan.js
        /// </summary>
        /// <param name="parentid">机构父guid</param>
        /// <param name="validateOpractionCodes">验证权限数组长度必须为3个 权限逐级递减(0查看所有  1同级  2部门以及部门以下 3 当前部门)</param>
        /// <param name="tokenCreateTime">token的创建时间</param>
        /// <param name="token">验证权限是否又被篡改</param>
        /// <returns></returns>
        public ActionResult GetOrganByParentId(string parentid, [ModelBinder(typeof(MyArrayBind))]string[] validateOpractionCodes, DateTime tokenCreateTime, string token)
        {
            if (!ValidateToken(validateOpractionCodes, tokenCreateTime, token))
                return AjaxFailureResult("参数失效，请刷新页面重试");
            if (validateOpractionCodes == null || validateOpractionCodes.Length < 3)
                return AjaxFailureResult("权限代码必须为3个长度数组");
            List<Organ> list = null;
            List<object> datas = new List<object>();
            if (UserInfo.OperationTable.Any(c => c.OperationName == validateOpractionCodes[0]))
            {
                list = string.IsNullOrEmpty(parentid) ? organBusiness.GetChildrenList(string.Empty, string.Empty) : organBusiness.GetChildrenList(parentid, string.Empty);
                if (list == null || list.Count <= 0)
                {

                    return GetDeparmentByParentId(parentid, string.Empty, validateOpractionCodes,
                        tokenCreateTime, token);


                }
            }
            else if (UserInfo.OperationTable.Any(c => c.OperationName == validateOpractionCodes[1]))
            {

                var data = organBusiness.GetByRecordId(UserInfo.OrganId);
                var dep = departmentBusiness.GetByRecordId(UserInfo.DeptId);

                var userparentDep = string.IsNullOrEmpty(dep.ParentId)
                    ? dep
                    : departmentBusiness.GetByRecordId(dep.ParentId);
                var userParentData = string.IsNullOrEmpty(data.ParentId) ? data : organBusiness.GetByRecordId(data.ParentId);
                if (string.IsNullOrEmpty(parentid))
                {


                    if (string.IsNullOrEmpty(UserInfo.DeptId) && !string.IsNullOrEmpty(UserInfo.OrganId))
                    {
                        //这段代码就是如果是同级没有部门情况下 机构为最后一级 也不按机构同级加载
                        if (data != null && !string.IsNullOrEmpty(data.RecordID))
                        {
                            list = new List<Organ>() { userParentData };
                        }


                    }
                }
                else
                {
                    list = organBusiness.GetChildrenList(parentid, userParentData.RecordID);
                }
                if (list == null || list.Count <= 0)
                {
                    if (string.IsNullOrEmpty(parentid))
                    {

                        //如果机构没有找到加载部门信息
                        datas = string.IsNullOrEmpty(dep.ParentId) ? new List<object>() { new { data.RecordID, Name = data.OrganName, Type = "organ" } } : new List<object>()
                        {
                            new
                            {

                                userparentDep.RecordID,
                                Name = userparentDep.DeptName,
                                Organid = userparentDep.OrganId,
                                ParentId=userparentDep.ParentId,
                                Type = "deparment",
                                  Icon="splashy-zoom_out5"
                            }
                        };
                    }
                    else
                    {
                        return GetDeparmentByParentId(parentid, string.Empty, validateOpractionCodes,
                            tokenCreateTime, token);
                    }


                }

            }
            else if (UserInfo.OperationTable.Any(c => c.OperationName == validateOpractionCodes[2]))
            {
                var dep = departmentBusiness.GetByRecordId(UserInfo.DeptId);
                var data = organBusiness.GetByRecordId(UserInfo.OrganId);
                if (string.IsNullOrEmpty(parentid))
                {
                    if (string.IsNullOrEmpty(UserInfo.DeptId) && !string.IsNullOrEmpty(UserInfo.OrganId))
                    {
                        list = new List<Organ>()
                    {
                        organBusiness.GetByRecordId(UserInfo.OrganId)
                    };
                    }

                }
                else
                {
                    list = organBusiness.GetChildrenList(parentid, UserInfo.OrganId);
                }
                if (list == null || list.Count <= 0)
                {
                    if (string.IsNullOrEmpty(parentid))
                    {
                        //如果机构没有找到加载部门信息
                        datas = dep == null || string.IsNullOrEmpty(dep.RecordID) ? new List<object>() { new { data.RecordID, Name = data.OrganName, Type = "organ" } } : new List<object>()
                        {
                            new
                            {

                                dep.RecordID,
                                Name = dep.DeptName,
                                Organid = dep.OrganId,
                               ParentId=dep.ParentId,
                                Type = "deparment",
                                  Icon="splashy-zoom_out5"
                            }
                        };
                    }
                    else
                    {
                        return GetDeparmentByParentId(parentid, string.Empty, validateOpractionCodes,
                            tokenCreateTime, token);
                    }
                }
            }
            else
            {
                var data = organBusiness.GetByRecordId(UserInfo.OrganId);
                var dep = departmentBusiness.GetByRecordId(UserInfo.DeptId);
                //如果机构没有找到加载部门信息
                datas = dep == null || string.IsNullOrEmpty(dep.RecordID) ? new List<object>() { new { data.RecordID, Name = data.OrganName, Icon = "splashy-zoom_out6", Type = "organ" } } : new List<object>()
                        {
                            new
                            {

                                dep.RecordID,
                                Name = dep.DeptName,
                                Organid = dep.OrganId,
                                ParentId=dep.ParentId,
                                Type = "deparment",
                                Icon="splashy-zoom_out5"
                            }
                        };


            }


            if (list != null && list.Count > 0)
            {
                datas.AddRange(list.Select(c => new { c.RecordID, Name = c.OrganName, c.ParentId, Icon = "splashy-zoom_out6", Type = "organ" }));
                //再尝试加载这个机构下部门信息
                var deparemts = departmentBusiness.GetDetpList(parentid, string.Empty);
                if (deparemts != null && deparemts.Count > 0)
                    datas.AddRange(
                        deparemts.Select(
                            c =>
                                new
                                {
                                    c.RecordID,
                                    Icon = "splashy-zoom_out5",
                                    Name = c.DeptName,
                                    c.ParentId,
                                    Organid = c.OrganId,
                                    Type = "deparment"
                                }));
            }
            return AjaxSucceedResult(datas);
        }

        /// <summary>
        ///验证参数的有效性
        /// </summary>
        /// <param name="validateOpractionCodes">权限验证code</param>
        /// <param name="tokenCreateTime">token创建日期</param>
        /// <param name="token">验证token</param>
        /// <returns>返回false 表示失效 或者参数被篡改</returns>
        private bool ValidateToken(string[] validateOpractionCodes, DateTime tokenCreateTime, string token)
        {
            var index = DateTime.Now.Subtract(tokenCreateTime);
            //一天后token过期
            if (index.Days >= 1) return false;
            //验证参数是否有被篡改
            var a = JSONHelp.SerializeObject(validateOpractionCodes);
            var b = tokenCreateTime.ToString();
            var servertoken = Hammer.Encrypt.MD5.Encode(a + b + TokenKeysEnum.TokenPassword);
            return servertoken.Equals(token);


        }
        /// <summary>
        /// 根据部门父id
        /// 2016-11-21 新增 李强
        /// 用于SelOrgan.js
        /// </summary>
        /// <param name="validateOpractionCodes">验证权限数组长度必须为3个 权限逐级递减(0查看所有  1同级  2部门以及部门以下 3 当前部门)</param>
        /// <param name="tokenCreateTime">token的创建时间</param>
        /// <param name="token">验证权限是否又被篡改</param>
        /// <param name="organid">机构id</param>
        /// <param name="parentid">部门父guid</param>
        /// <returns></returns>
        public ActionResult GetDeparmentByParentId(string organid, string parentid, [ModelBinder(typeof(MyArrayBind))] string[] validateOpractionCodes, DateTime tokenCreateTime, string token)
        {
            if (!ValidateToken(validateOpractionCodes, tokenCreateTime, token))
                return AjaxFailureResult("参数失效，请刷新页面重试");
            if (validateOpractionCodes == null || validateOpractionCodes.Length < 3)
                return AjaxFailureResult("权限代码必须为3个长度数组");
            List<Department> list = null;
            if (UserInfo.OperationTable.Any(c => c.OperationName == validateOpractionCodes[0]))
            {
                list = departmentBusiness.GetDetpList(organid, parentid);
            }
            else if (UserInfo.OperationTable.Any(c => c.OperationName == validateOpractionCodes[1]))
            {
                list = departmentBusiness.GetDetpList(organid, parentid);

            }
            else if (UserInfo.OperationTable.Any(c => c.OperationName == validateOpractionCodes[2]))
            {
                list = departmentBusiness.GetDetpList(organid, parentid, UserInfo.DeptId);
            }
            else
            {
                list = new List<Department>()
                    {
                        departmentBusiness.GetByRecordId(UserInfo.DeptId)
                    };
            }


            var seledata = list.Select(c => new { c.RecordID, Icon = "splashy-zoom_out5", Name = c.DeptName, c.ParentId, Organid = c.OrganId, Type = "deparment" });
            return AjaxSucceedResult(seledata);
        }

        /// <summary>
        /// 根据机构部门名称查询机构信息以及父机构信息
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns></returns>
        public ActionResult SerchOragnInfoByName(string name, [ModelBinder(typeof(MyArrayBind))] string[] validateOpractionCodes, DateTime tokenCreateTime, string token)
        {
            if (!ValidateToken(validateOpractionCodes, tokenCreateTime, token))
                return AjaxFailureResult("参数失效，请刷新页面重试");
            if (validateOpractionCodes == null || validateOpractionCodes.Length < 3)
                return AjaxFailureResult("权限代码必须为3个长度数组");
            string organPath = string.Empty;
            string deparmentPath = string.Empty;
            string deparmetOrgan = string.Empty;
            string deparementid = string.Empty;
            string organid = string.Empty;
            var organModel = string.IsNullOrEmpty(UserInfo.OrganId) ? null : organBusiness.GetByRecordId(UserInfo.OrganId);
            var deparmentModel = string.IsNullOrEmpty(UserInfo.DeptId) ? null : departmentBusiness.GetByRecordId(UserInfo.DeptId);
            if (UserInfo.OperationTable.Any(c => c.OperationName == validateOpractionCodes[0]))
            {
                //可以查看所有

            }
            else if (UserInfo.OperationTable.Any(c => c.OperationName == validateOpractionCodes[1]))
            {
                //查看同级
                if (!string.IsNullOrEmpty(UserInfo.DeptId))
                {
                    organPath = UserInfo.OrganId;
                    deparmentPath = deparmentModel == null ? string.Empty : deparmentModel.ParentId;
                    deparmetOrgan = string.IsNullOrEmpty(organModel.ParentId)
                        ? organModel.RecordID
                        : organModel.ParentId;
                }
                else if (string.IsNullOrEmpty(UserInfo.DeptId) && !string.IsNullOrEmpty(UserInfo.OrganId))
                {
                    if (organModel != null) organPath = organModel.ParentId;
                    deparmetOrgan = string.IsNullOrEmpty(organModel.ParentId)
                      ? organModel.RecordID
                      : organModel.ParentId;
                }
                else
                {
                    //用户没有机构和部门 表示什么都搜不到
                    organPath = "-1";
                    deparmentPath = "-1";
                }

            }
            else if (UserInfo.OperationTable.Any(c => c.OperationName == validateOpractionCodes[2]))
            {
                //查看部门以及部分以下
                if (!string.IsNullOrEmpty(UserInfo.DeptId))
                {
                    organPath = UserInfo.OrganId;
                    deparmentPath = UserInfo.DeptId;
                }
                else if (string.IsNullOrEmpty(UserInfo.DeptId) && !string.IsNullOrEmpty(UserInfo.OrganId))
                {
                    deparmetOrgan = organPath = UserInfo.OrganId;

                }
                else
                {
                    //用户没有机构和部门 表示什么都搜不到
                    organPath = "-1";
                    deparmentPath = "-1";
                }

            }
            else
            {
                //查看部门以及部分以下
                if (!string.IsNullOrEmpty(UserInfo.DeptId))
                {
                    deparementid = UserInfo.DeptId;
                }
                else if (string.IsNullOrEmpty(UserInfo.DeptId) && !string.IsNullOrEmpty(UserInfo.OrganId))
                {
                    organid = UserInfo.OrganId;
                    deparmentPath = "-1";
                }
                else
                {
                    //用户没有机构和部门 表示什么都搜不到
                    organPath = "-1";
                    deparmentPath = "-1";
                }

            }
            var organDatas = organBusiness.GetOrgansByName(name, organPath, organid);
            var deparments = departmentBusiness.GetDepartmentsByName(name, deparmetOrgan, deparmentPath, deparementid);
            deparments.ForEach(c =>
            {
                if (!organDatas.Any(d => d.RecordID == c.OrganId))
                {
                    organDatas.Add(organBusiness.GetByRecordId(c.OrganId));
                }
            });
            return AjaxSucceedResult(new { organs = organDatas.Select(c => new { c.OrganName, c.RecordID, c.ParentId, Icon = "splashy-zoom_out6" }), deparments = deparments.Select(c => new { c.DeptName, Icon = "splashy-zoom_out5", c.OrganId, c.RecordID, c.ParentId }) });
        }
    }
}