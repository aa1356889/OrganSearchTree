using Areca.Public.Binder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Mvc;
using Areca.Areas.Admin.Controllers;
using Energy.Business;
using Energy.Entity;
using Hammer.Common;
using Energy.Enum;
using Energy.Share;
using OrganTree.OperationServer;
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

            
            OrganOperaction organOperaction =OrganAdapter.GetOrganOperaction(UserInfo.OperationTable, validateOpractionCodes,UserInfo.OrganId, UserInfo.DeptId);
            return AjaxSucceedResult(organOperaction.GetOrganData(parentid,validateOpractionCodes,tokenCreateTime,token));
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
            OrganOperaction organOperaction = OrganAdapter.GetOrganOperaction(UserInfo.OperationTable, validateOpractionCodes, UserInfo.OrganId, UserInfo.DeptId);
            return AjaxSucceedResult(organOperaction.GetDeparentData(organid,parentid,validateOpractionCodes,tokenCreateTime,token));
        }

        /// <summary>
        /// 根据机构部门名称查询机构信息以及父机构信息
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns></returns>
        public ActionResult SerchOragnInfoByName(string name, [ModelBinder(typeof(MyArrayBind))] string[] validateOpractionCodes, DateTime tokenCreateTime, string token)
        {
            OrganOperaction organOperaction = OrganAdapter.GetOrganOperaction(UserInfo.OperationTable, validateOpractionCodes, UserInfo.OrganId, UserInfo.DeptId);
            return AjaxSucceedResult(organOperaction.GetOrganSerachData(name,validateOpractionCodes,tokenCreateTime,token));
        }
    }
    
}