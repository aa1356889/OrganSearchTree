
using Energy.Enum;
using Energy.Share;
using Hammer.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Areca.Areas.Admin.Controllers
{
    public class MyControllerBase : Controller
    {

        public UserTicket UserInfo { get; set; }

        /// <summary>
        /// 检查用户权限
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool CheckRight(string code)
        {
            if (UserInfo == null || string.IsNullOrEmpty(code))
            {
                return false;
            }
            return UserInfo.OperationTable.Any(c => c.OperationName == code.Trim());
        }

        /*
        /// <summary>
        ///  客户施工微信入口令牌
        /// </summary>
        public CustomerTicket CustomerInfo { get; set; }
        */

        /// <summary>
        /// 处理成功返回数据
        /// </summary>
        /// <param name="obje">数据</param>
        /// <returns></returns>
        public JsonResult Succeed(object data, string msg = "处理成功")
        {
            return Json(new { Datas = data, State = ProcessAjaxStateEnum.OK, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 处理成功返回提示信息
        /// </summary>
        /// <param name="msg">成功提示信息</param>
        /// <returns></returns>
        public JsonResult Succeed(string msg)
        {
            return Json(new { Message = msg, State = ProcessAjaxStateEnum.OK }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 处理失败返回提示
        /// </summary>
        /// <param name="msg">失败提示信息</param>
        /// <returns></returns>
        public JsonResult NoSucceed(string msg)
        {
            return Json(new { Message = msg, State = ProcessAjaxStateEnum.Failure }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ajax请求返回值
        /// </summary>
        /// <param name="result"></param>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ActionResult AjaxResult(ProcessAjaxStateEnum result, object data, string msg)
        {
            return Content(JSONHelp.SerializeObject(new { State = result, Data = data, Message = msg }));
        }

        /// <summary>
        /// ajax请求返回值
        /// </summary>
        /// <param name="result"></param>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ActionResult AjaxResult(ProcessAjaxStateEnum result, string msg)
        {
            return Content(JSONHelp.SerializeObject(new { State = result, Message = msg }));
        }

        /// <summary>
        /// ajax请求返回值，主要是列表使用
        /// </summary>
        /// <param name="result"></param>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ActionResult AjaxResult(object data)
        {
            return Content(JSONHelp.SerializeObject(data));
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ActionResult AjaxSucceedResult(object data, string msg = "操作成功")
        {
            return AjaxResult(ProcessAjaxStateEnum.OK, data, msg);
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ActionResult AjaxSucceedResult(string msg = "操作成功")
        {
            return AjaxResult(ProcessAjaxStateEnum.OK, msg);
        }
        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ActionResult AjaxFailureResult(object data, string msg = "操作失败")
        {
            return AjaxResult(ProcessAjaxStateEnum.Failure, data, msg);
        }
        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ActionResult AjaxFailureResult(string msg = "操作失败")
        {
            return AjaxResult(ProcessAjaxStateEnum.Failure, msg);
        }
        /// <summary>
        /// 无权限
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ActionResult AjaxForbidResult(string msg = "对不起，您无权访问此页面或操作此功能！如您确实需要访问或操作，请联系管理员")
        {
            return AjaxResult(ProcessAjaxStateEnum.Forbid, msg);
        }




        /// <summary>
        /// 验证当前用户是否有指定权限编码权限
        /// </summary>
        /// <param name="orcoding">权限编码</param>
        /// <returns></returns>
        public bool IsOperation(string orcoding)
        {
            if (!UserInfo.OperationTable.Any(c => c.OperationName == orcoding.Trim()))
            {
                return false;
            }
            return true;
        }

    }
}