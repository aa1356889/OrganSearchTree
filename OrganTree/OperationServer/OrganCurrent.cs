using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Energy.Entity;

namespace OrganTree.OperationServer
{
    /// <summary>
    /// 查看自己的
    /// </summary>
    public class OrganCurrent:OrganOperaction
    {
        public OrganCurrent(string userOrganId, string userDeparmentId):base(userOrganId,userDeparmentId)
        {
        }
        public override IEnumerable<object> ProcessOractionData(string parentid)
        {
            List<object> datas = new List<object>();
            var data = organBusiness.GetByRecordId(_userOrganId);
            var dep = departmentBusiness.GetByRecordId(_userDeparmentId);
            //如果机构没有找到加载部门信息
            datas = dep == null || string.IsNullOrEmpty(dep.RecordID) ? new List<object>() { new { data.RecordID, Name = data.OrganName, Icon = "splashy-zoom_out6", Type = "organ" } } : new List<object>()
                        {
                            new
                            {

                                dep.RecordID,
                                Name = dep.DeptName,
                                Organid = dep.OrganId,
                                ParentId=dep.ParentId,
                                Type = "deparment",
                                Icon="splashy-zoom_out5"
                            }
                        };
            return datas;
        }
        public override IEnumerable<object> ProcessDeparmentData(string organid, string parentid)
        {
           var list = new List<Department>()
                    {
                        departmentBusiness.GetByRecordId(_userDeparmentId)
                    };
            return FormatDeparmentData(list);
        }
        public override object ProcessOrganSerach(string name)
        {
            string organPath = string.Empty;
            string deparmentPath = string.Empty;
            string deparmetOrgan = string.Empty;
            string deparementid = string.Empty;
            string organid = string.Empty;
            var organModel = string.IsNullOrEmpty(_userOrganId) ? null : organBusiness.GetByRecordId(_userOrganId);
            var deparmentModel = string.IsNullOrEmpty(_userDeparmentId) ? null : departmentBusiness.GetByRecordId(_userDeparmentId);
            if (!string.IsNullOrEmpty(_userDeparmentId))
            {
                deparementid =_userDeparmentId;
            }
            else if (string.IsNullOrEmpty(_userOrganId) && !string.IsNullOrEmpty(_userOrganId))
            {
                organid = _userOrganId;
                deparmentPath = "-1";
            }
            else
            {
                //用户没有机构和部门 表示什么都搜不到
                organPath = "-1";
                deparmentPath = "-1";
            }
            var organDatas = organBusiness.GetOrgansByName(name, organPath, organid);
            var deparments = departmentBusiness.GetDepartmentsByName(name, deparmetOrgan, deparmentPath, deparementid);
            return FormateSerachOrganData(deparments, organDatas);
        }
    }
}