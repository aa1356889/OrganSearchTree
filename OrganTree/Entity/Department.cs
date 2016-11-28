

using System;
using System.ComponentModel.DataAnnotations;

namespace Energy.Entity
{
	/// <summary>
	/// 部门表 实体类
	/// </summary>
    [Serializable]
	public class Department
	{
        /// <summary>
        /// 组织机构编号
        /// </summary>      
        public string OrganId { get; set; }

        /// <summary>
        /// 记录ID
        /// </summary>
  
        public string RecordID { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        [Key]
        public int DeptId { get; set; }

        /// <summary>
        /// 层次代码
        /// </summary>

        public string LevelCode { get; set; }

        /// <summary>
        /// 部门自编号
        /// </summary>
    
        public string DeptCode { get; set; }

		/// <summary>
		/// 部门名称
		/// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 上级部门
        /// </summary>
    
        public string ParentId { get; set; }

        /// <summary>
        /// 顺序号
        /// </summary>

        public double SortNum { get; set; }

        /// <summary>
        /// 部门电话
        /// </summary>
        [System.ComponentModel.DataAnnotations.StringLength(50)]
        public string Tel { get; set; }

        /// <summary>
        /// 传真电话
        /// </summary>

        public string Fax { get; set; }

        /// <summary>
        /// QQ
        /// </summary>

        public string QQ { get; set; }

        public string Province { get; set; }
      
        public string City { get; set; }
    
        public string District { get; set; }

        public string Address { get; set; }

        /// <summary>
        /// 部门主管
        /// </summary>

        public string Master { get; set; }

        /// <summary>
        /// 部门副主管
        /// </summary>
  
        public string DeputyMaster { get; set; }

        /// <summary>
        /// 上级主管
        /// </summary>
 
        public string Leader { get; set; }

        /// <summary>
        /// 部门职能
        /// </summary>
      
        public string Intro { get; set; }

        /// <summary>
        /// 虚拟标志
        /// </summary>

        public string Virtual { get; set; }

		/// <summary>
		/// 状态
		/// </summary>
   
        public int Status { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>

        public string IsDelete { get; set; }

        /// <summary>
        /// 点赞数
        /// </summary>
        public int TotalFavories { get; set; }

        /// <summary>
        /// 浏览数
        /// </summary>
        public int TotalViews { get; set; }


        public string Area { get; set; }
     
        public int TotalScheme { get; set; }
   
        public int TotalComment { get; set; }

        public int Recommended { get; set; }
        /// <summary>
        /// 当前记录在树机构中的路径
        /// </summary>

        public string Path { get; set; }

	}
}
