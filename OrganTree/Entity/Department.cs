

using System;
using System.ComponentModel.DataAnnotations;

namespace Energy.Entity
{
	/// <summary>
	/// ���ű� ʵ����
	/// </summary>
    [Serializable]
	public class Department
	{
        /// <summary>
        /// ��֯�������
        /// </summary>      
        public string OrganId { get; set; }

        /// <summary>
        /// ��¼ID
        /// </summary>
  
        public string RecordID { get; set; }

        /// <summary>
        /// ���ű��
        /// </summary>
        [Key]
        public int DeptId { get; set; }

        /// <summary>
        /// ��δ���
        /// </summary>

        public string LevelCode { get; set; }

        /// <summary>
        /// �����Ա��
        /// </summary>
    
        public string DeptCode { get; set; }

		/// <summary>
		/// ��������
		/// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// �ϼ�����
        /// </summary>
    
        public string ParentId { get; set; }

        /// <summary>
        /// ˳���
        /// </summary>

        public double SortNum { get; set; }

        /// <summary>
        /// ���ŵ绰
        /// </summary>
        [System.ComponentModel.DataAnnotations.StringLength(50)]
        public string Tel { get; set; }

        /// <summary>
        /// ����绰
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
        /// ��������
        /// </summary>

        public string Master { get; set; }

        /// <summary>
        /// ���Ÿ�����
        /// </summary>
  
        public string DeputyMaster { get; set; }

        /// <summary>
        /// �ϼ�����
        /// </summary>
 
        public string Leader { get; set; }

        /// <summary>
        /// ����ְ��
        /// </summary>
      
        public string Intro { get; set; }

        /// <summary>
        /// �����־
        /// </summary>

        public string Virtual { get; set; }

		/// <summary>
		/// ״̬
		/// </summary>
   
        public int Status { get; set; }

        /// <summary>
        /// ɾ����־
        /// </summary>

        public string IsDelete { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public int TotalFavories { get; set; }

        /// <summary>
        /// �����
        /// </summary>
        public int TotalViews { get; set; }


        public string Area { get; set; }
     
        public int TotalScheme { get; set; }
   
        public int TotalComment { get; set; }

        public int Recommended { get; set; }
        /// <summary>
        /// ��ǰ��¼���������е�·��
        /// </summary>

        public string Path { get; set; }

	}
}
