using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using OrganTree.Dal;

namespace Energy.Business
{
    public class BussinesBase
    {
        public TreeDbContext Context
        {
            get
            {
                #region 避免额外创建连接的开销 同一线程内共享数据上下文
                var context = CallContext.GetData("dbContext") as TreeDbContext;
                if (context == null)
                {
                    context = new TreeDbContext();
                    //关闭实体跟踪
                    context.Configuration.ValidateOnSaveEnabled = false;
                    CallContext.SetData("dbContext", context);
                }
                #endregion

                return context;
            }
        }
    }
}