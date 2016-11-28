using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Energy.Entity;

namespace OrganTree.OperationServer
{
    /// <summary>
    /// 浏览同级
    /// </summary>
    public class BrotherOperaction: OrganOperaction
    {
        public BrotherOperaction(string userOrganId, string userDeparmentId) : base(userOrganId, userDeparmentId)
        {
        }
        public override IEnumerable<object> ProcessOractionData(string parentid)
        {
            var data = organBusiness.GetByRecordId(_userOrganId);
            var dep = departmentBusiness.GetByRecordId(_userDeparmentId);

            var userparentDep = string.IsNullOrEmpty(dep.ParentId)
                ? dep
                : departmentBusiness.GetByRecordId(dep.ParentId);
            List<Organ> list=null;
            List<object> datas = new List<object>();
            var userParentData = string.IsNullOrEmpty(data.ParentId) ? data : organBusiness.GetByRecordId(data.ParentId);
            if (string.IsNullOrEmpty(parentid))
            {


                if (string.IsNullOrEmpty(_userOrganId) && !string.IsNullOrEmpty(_userDeparmentId))
                {
                    //这段代码就是如果是同级没有部门情况下 机构为最后一级 也不按机构同级加载
                    if (data != null && !string.IsNullOrEmpty(data.RecordID))
                    {
                        list = new List<Organ>() { userParentData };
                    }


                }
            }
            else
            {
                list = organBusiness.GetChildrenList(parentid, userParentData.RecordID);
            }
            if (list == null || list.Count <= 0)
            {
                if (string.IsNullOrEmpty(parentid))
                {

                    //如果机构没有找到加载部门信息
                    datas = string.IsNullOrEmpty(dep.ParentId) ? new List<object>() { new { data.RecordID, Name = data.OrganName, Type = "organ" } } : new List<object>()
                        {
                            new
                            {

                                userparentDep.RecordID,
                                Name = userparentDep.DeptName,
                                Organid = userparentDep.OrganId,
                                ParentId=userparentDep.ParentId,
                                Type = "deparment",
                                  Icon="splashy-zoom_out5"
                            }
                        };
                }
                else
                {
                    return ProcessDeparmentData(parentid,string.Empty);
                 
                }


            }
            datas.AddRange(FormatData(list,parentid));
            return datas;
        }

        public override IEnumerable<object> ProcessDeparmentData(string organid, string parentid)
        {
           var list = departmentBusiness.GetDetpList(organid, parentid);
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
            //查看同级
            if (!string.IsNullOrEmpty(_userDeparmentId))
            {
                organPath = _userOrganId;
                deparmentPath = deparmentModel == null ? string.Empty : deparmentModel.ParentId;
                deparmetOrgan = string.IsNullOrEmpty(organModel.ParentId)
                    ? organModel.RecordID
                    : organModel.ParentId;
            }
            else if (string.IsNullOrEmpty(_userDeparmentId) && !string.IsNullOrEmpty(_userOrganId))
            {
                if (organModel != null) organPath = organModel.ParentId;
                deparmetOrgan = string.IsNullOrEmpty(organModel.ParentId)
                  ? organModel.RecordID
                  : organModel.ParentId;
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