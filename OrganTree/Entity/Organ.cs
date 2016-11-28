#region License Statement

//======================================================================
//
//        Copyright (C) 2013 ������ά(LV)  
//        All rights reserved
//
//        filename : Role.cs
//        description : ��ɫ�� ʵ����
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
	/// ��֯���� ʵ����
	/// </summary>
    [Serializable]
	public class Organ
	{

		/// <summary>
		/// ��֯�������
		/// </summary>
		[Key]
        public int OrganId { get; set; }

        /// <summary>
        /// ��¼ID
        /// </summary>

        public string RecordID { get; set; }

        /// <summary>
        /// ���������
        /// </summary>

        public string RootOrganId { get; set; }

        /// <summary>
        /// �ϼ��������
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// ��δ���
        /// </summary>

        public string LevelCode { get; set; }

        /// <summary>
        /// ����ȫ��
        /// </summary>
 
        public string OrganName { get; set; }

        /// <summary>
        /// �������
        /// </summary>

        public string ShortName { get; set; }

		/// <summary>
		/// ״̬
		/// </summary>
        public int Status { get; set; }

        /// <summary>
        /// ɾ����־
        /// </summary>
    
        public string IsDelete { get; set; }

        /// <summary>
        /// ������ĸ��д
        /// </summary>

        public string ShortChar { get; set; }

        /// <summary>
        /// �Ƿ񱾲�
        /// 1Ϊ����������Ϊ�Ǳ���
        /// </summary>
       
        public string IsHeadquarters { get; set; }

        /// <summary>
        /// �Ƿ�����
        /// 0Ĭ�ϲ�����
        /// </summary>
 
        public int IsEnable { get; set; }


        public string isgroup { get; set; }

        public string isreset { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>

        public DateTime CreateDate { get; set; }

        /// <summary>
        ///  ���ÿ�ʼʱ��
        /// </summary>
        public DateTime? EnableDateStart { get; set; }

        /// <summary>
        /// ���ý���ʱ��
        /// </summary>
        public DateTime? EnableDateEnd { get; set; }

        /// <summary>
        /// ��Ӧ�̱�־
        /// </summary>
        public string IsSupplier { get; set; }

        public string Province{ get; set; }

        public string City { get; set; }

        public string District { get; set; }

        /// <summary>
        /// ��ҵ����
        /// </summary>

        public string OfficeWebsite { get; set; }

        /// <summary>
        /// ��˾��վ
        /// </summary>

        public string Website { get; set; }

        /// <summary>
        /// ��ǰ��¼���������е�·��
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///  �����������(������)
        /// </summary>

        public string OrganCode { get; set; }

        /// <summary>
        /// ���ڱ�����ϸ��ַ��������������������һ�������ĵ�ַ
        /// </summary>

        public string Address { get; set; }
	}
}
