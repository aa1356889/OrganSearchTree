using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energy.Enum
{
    //用于表示处理ajax请求状态的方法
    public enum ProcessAjaxStateEnum
    {
        /// <summary>
        /// 处理成功
        /// </summary>
        OK = 0,
        /// <summary>
        /// 处理失败
        /// </summary>
        Failure = 1,
        /// <summary>
        /// 无权限
        /// </summary>
        Forbid = 2,
        /// <summary>
        /// 未登陆
        /// </summary>
        NoLogin=3
    }
}
