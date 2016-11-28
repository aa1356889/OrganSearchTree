using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Energy.Entity;

namespace OrganTree.OperationServer
{
    /// <summary>
    /// 查看所有数据权限
    /// </summary>
    public class OrganAllOpraction : OrganOperaction
    {
        public OrganAllOpraction(string userOrganId, string userDeparmentId) : base(userOrganId, userDeparmentId)
        {
        }

        public override IEnumerable<object> ProcessOractionData(string parentid)
        {
            List<object> datas = new List<object>();
            var list = string.IsNullOrEmpty(parentid)
                ? organBusiness.GetChildrenList(string.Empty, string.Empty)
                : organBusiness.GetChildrenList(parentid, string.Empty);
            if (list == null || list.Count <= 0)
            {
               return  ProcessDeparmentData(parentid, string.Empty);

                //return GetDeparmentByParentId(parentid, string.Empty, validateOpractionCodes,
                //    tokenCreateTime, token);


            }
            else
            {
                return FormatData(list, parentid);
            }


        }
        public override IEnumerable<object> ProcessDeparmentData(string organid, string parentid)
        {
            List<Department> list = departmentBusiness.GetDetpList(organid, parentid);
            return  FormatDeparmentData(list);
        }
        public override object ProcessOrganSerach(string name)
        {
            var organDatas = organBusiness.GetOrgansByName(name,string.Empty, string.Empty);
            var deparments = departmentBusiness.GetDepartmentsByName(name, string.Empty, string.Empty, string.Empty);
            return FormateSerachOrganData(deparments, organDatas);
        }
    }
}