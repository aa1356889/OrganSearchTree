#region License Statement

//======================================================================
//
//        Copyright (C) 2013 广州量维(LV)  
//        All rights reserved
//
//        filename : Role.cs
//        description : 角色表 实体类
//
//        created by jiangjun at 2015/7/1
//
//======================================================================

#endregion

using System;
using System.ComponentModel.DataAnnotations;

namespace Energy.Entity
{
	/// <summary>
	/// 组织机构 实体类
	/// </summary>
    [Serializable]
	public class Organ
	{

		/// <summary>
		/// 组织机构编号
		/// </summary>
		[Key]
        public int OrganId { get; set; }

        /// <summary>
        /// 记录ID
        /// </summary>

        public string RecordID { get; set; }

        /// <summary>
        /// 根机构编号
        /// </summary>

        public string RootOrganId { get; set; }

        /// <summary>
        /// 上级机构编号
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 层次代码
        /// </summary>

        public string LevelCode { get; set; }

        /// <summary>
        /// 机构全称
        /// </summary>
 
        public string OrganName { get; set; }

        /// <summary>
        /// 机构简称
        /// </summary>

        public string ShortName { get; set; }

		/// <summary>
		/// 状态
		/// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
    
        public string IsDelete { get; set; }

        /// <summary>
        /// 机构字母缩写
        /// </summary>

        public string ShortChar { get; set; }

        /// <summary>
        /// 是否本部
        /// 1为本部，其他为非本部
        /// </summary>
       
        public string IsHeadquarters { get; set; }

        /// <summary>
        /// 是否启用
        /// 0默认不启用
        /// </summary>
 
        public int IsEnable { get; set; }


        public string isgroup { get; set; }

        public string isreset { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>

        public DateTime CreateDate { get; set; }

        /// <summary>
        ///  启用开始时间
        /// </summary>
        public DateTime? EnableDateStart { get; set; }

        /// <summary>
        /// 启用结束时间
        /// </summary>
        public DateTime? EnableDateEnd { get; set; }

        /// <summary>
        /// 供应商标志
        /// </summary>
        public string IsSupplier { get; set; }

        public string Province{ get; set; }

        public string City { get; set; }

        public string District { get; set; }

        /// <summary>
        /// 企业官网
        /// </summary>

        public string OfficeWebsite { get; set; }

        /// <summary>
        /// 公司网站
        /// </summary>

        public string Website { get; set; }

        /// <summary>
        /// 当前记录在树机构中的路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///  机构短名编号(订单用)
        /// </summary>

        public string OrganCode { get; set; }

        /// <summary>
        /// 用于保存详细地址，和区域联合起来才是一个完整的地址
        /// </summary>

        public string Address { get; set; }
	}
}
