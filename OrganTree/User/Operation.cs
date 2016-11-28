

using System;

namespace Energy.Entity
{
	/// <summary>
	/// 模块权限表 实体类
	/// </summary>
    [Serializable]
	public class Operation
	{
        /// <summary>
        /// 操作编号
        /// </summary>
        public string OperationID { get; set; }

        /// <summary>
        /// 记录ID
        /// </summary>

        public string RecordID { get; set; }

		/// <summary>
		/// 功能模块GUID
		/// </summary>
        public string FunctionModuleRecordID { get; set; }		

		/// <summary>
        /// 操作名称，主要用来action打标记用，保证唯一
		/// </summary>
        public string OperationName { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Action
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 界面显示的操作名称
        /// </summary>
        public string OperationFullName { get; set; }
	}
}
