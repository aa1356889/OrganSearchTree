

using System;

namespace Energy.Entity
{
	/// <summary>
	/// ģ��Ȩ�ޱ� ʵ����
	/// </summary>
    [Serializable]
	public class Operation
	{
        /// <summary>
        /// �������
        /// </summary>
        public string OperationID { get; set; }

        /// <summary>
        /// ��¼ID
        /// </summary>

        public string RecordID { get; set; }

		/// <summary>
		/// ����ģ��GUID
		/// </summary>
        public string FunctionModuleRecordID { get; set; }		

		/// <summary>
        /// �������ƣ���Ҫ����action�����ã���֤Ψһ
		/// </summary>
        public string OperationName { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// Action
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// ������ʾ�Ĳ�������
        /// </summary>
        public string OperationFullName { get; set; }
	}
}
