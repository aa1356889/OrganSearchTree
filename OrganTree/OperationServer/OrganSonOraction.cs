using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Energy.Entity;

namespace OrganTree.OperationServer
{
    /// <summary>
    /// 查看部门以及部门以下
    /// </summary>
    public class OrganSonOraction: OrganOperaction
    {
        public OrganSonOraction(string userOrganId, string userDeparmentId):base(userOrganId,userDeparmentId)
        {

        }
        public override IEnumerable<object> ProcessOractionData(string parentid)
        {
            var dep = departmentBusiness.GetByRecordId(_userDeparmentId);
            var data = organBusiness.GetByRecordId(_userOrganId);
            List<Organ> list = null;
            List<object> datas = null;
            if (string.IsNullOrEmpty(parentid))
            {
                if (string.IsNullOrEmpty(_userDeparmentId) && !string.IsNullOrEmpty(_userOrganId))
                {
                    list = new List<Organ>()
                    {
                        organBusiness.GetByRecordId(_userOrganId)
                    };
                }

            }
            else
            {
                list = organBusiness.GetChildrenList(parentid, _userOrganId);
            }
            if (list == null || list.Count <= 0)
            {
                if (string.IsNullOrEmpty(parentid))
                {
                    //如果机构没有找到加载部门信息
                    datas = dep == null || string.IsNullOrEmpty(dep.RecordID)
                        ? new List<object>() {new {data.RecordID, Name = data.OrganName, Type = "organ"}}
                        : new List<object>()
                        {
                            new
                            {

                                dep.RecordID,
                                Name = dep.DeptName,
                                Organid = dep.OrganId,
                                ParentId = dep.ParentId,
                                Type = "deparment",
                                Icon = "splashy-zoom_out5"
                            }
                        };
                }
                else
                {
                    return ProcessDeparmentData(parentid,string.Empty);
                   
                }
            }
            else
            {
                datas.AddRange(FormatData(list,parentid));
                
            }
            return datas;
        }
        public override IEnumerable<object> ProcessDeparmentData(string organid, string parentid)
        {
            var list = departmentBusiness.GetDetpList(organid, parentid, _userDeparmentId);
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
            //查看部门以及部分以下
            if (!string.IsNullOrEmpty(_userDeparmentId))
            {
                organPath = _userOrganId;
                deparmentPath = _userDeparmentId;
            }
            else if (string.IsNullOrEmpty(_userDeparmentId) && !string.IsNullOrEmpty(_userOrganId))
            {
                deparmetOrgan = organPath = _userOrganId;

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