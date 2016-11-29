using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Energy.Business;
using Energy.Entity;
using Energy.Enum;
using Hammer.Common;

namespace OrganTree.OperationServer
{
    public abstract class OrganOperaction
    {
        public OrganOperaction( string userOrganId, string userDeparmentId)
        {
            this._userOrganId = userOrganId;
            this._userDeparmentId = userDeparmentId;
        }


        protected OrganBusiness organBusiness = new OrganBusiness();
        protected DepartmentBusiness departmentBusiness = new DepartmentBusiness();
        protected string _userOrganId;
        protected string _userDeparmentId;
      
        /// <summary>
        ///验证参数的有效性
        /// </summary>
        /// <param name="validateOpractionCodes">权限验证code</param>
        /// <param name="tokenCreateTime">token创建日期</param>
        /// <param name="token">验证token</param>
        /// <returns>返回false 表示失效 或者参数被篡改</returns>
        protected bool ValidateToken(string[] validateOpractionCodes, DateTime tokenCreateTime, string token)
        {
            var index = DateTime.Now.Subtract(tokenCreateTime);
            //一天后token过期
            if (index.Days >= 1) return false;
            //验证参数是否有被篡改
            var a = JSONHelp.SerializeObject(validateOpractionCodes);
            var b = tokenCreateTime.ToString();
            var servertoken = Hammer.Encrypt.MD5.Encode(a + b + TokenKeysEnum.TokenPassword);
            return servertoken.Equals(token);


        }
        public IEnumerable<object> GetOrganData(string parentid,string[] validateOpractionCodes, DateTime tokenCreateTime, string token)
        {
            if (!ValidateToken(validateOpractionCodes, tokenCreateTime, token))
               throw new Exception("参数失效，请刷新页面重试");
            if (validateOpractionCodes == null || validateOpractionCodes.Length < 3)
                throw new Exception("权限代码必须为3个长度数组");
            List<Organ> list = null;
            List<object> datas = new List<object>();
            return ProcessOractionData(parentid);
        }

        public IEnumerable<object> GetDeparentData(string organid, string parentid, string[] validateOpractionCodes, DateTime tokenCreateTime, string token)
        {
            if (!ValidateToken(validateOpractionCodes, tokenCreateTime, token))
                throw new Exception("参数失效，请刷新页面重试");
            if (validateOpractionCodes == null || validateOpractionCodes.Length < 3)
                throw new Exception("权限代码必须为3个长度数组");
            List<Organ> list = null;
            List<object> datas = new List<object>();
            return ProcessDeparmentData(organid, parentid);
        }

        public object GetOrganSerachData(string name, string[] validateOpractionCodes, DateTime tokenCreateTime, string token)
        {
            if (!ValidateToken(validateOpractionCodes, tokenCreateTime, token))
                throw new Exception("参数失效，请刷新页面重试");
            if (validateOpractionCodes == null || validateOpractionCodes.Length < 3)
                throw new Exception("权限代码必须为3个长度数组");
            return ProcessOrganSerach(name);
        }


        public abstract IEnumerable<object> ProcessOractionData(string parentid);

        public abstract IEnumerable<object> ProcessDeparmentData(string organid, string parentid);

        public abstract object ProcessOrganSerach(string name);

        protected virtual IEnumerable<object> FormatData(List<Organ> list,string parentid)
        {
            var datas = new List<object>();
             if(list!=null&&list.Count>0)
            datas.AddRange(list.Select(c => new { c.RecordID, Name = c.OrganName, c.ParentId, Icon = "splashy-zoom_out6", Type = "organ" }));
            //再尝试加载这个机构下部门信息
            var deparemts = departmentBusiness.GetDetpList(parentid, string.Empty);
            if (deparemts != null && deparemts.Count > 0)
                datas.AddRange(
                    deparemts.Select(
                        c =>
                            new
                            {
                                c.RecordID,
                                Icon = "splashy-zoom_out5",
                                Name = c.DeptName,
                                c.ParentId,
                                Organid = c.OrganId,
                                Type = "deparment"
                            }));
            return datas;
        }

        protected virtual IEnumerable<object> FormatDeparmentData(List<Department> list)
        {
            return list.Select(c => new { c.RecordID, Icon = "splashy-zoom_out5", Name = c.DeptName, c.ParentId, Organid = c.OrganId, Type = "deparment" });
        }

        protected virtual object FormateSerachOrganData(List<Department> deparments, List<Organ> organDatas)
        {
            deparments.ForEach(c =>
            {
                if (!organDatas.Any(d => d.RecordID == c.OrganId))
                {
                    organDatas.Add(organBusiness.GetByRecordId(c.OrganId));
                }
            });
            return new { organs = organDatas.Select(c => new { c.OrganName, c.RecordID, c.ParentId, Icon = "splashy-zoom_out6" }), deparments = deparments.Select(c => new { c.DeptName, Icon = "splashy-zoom_out5", c.OrganId, c.RecordID, c.ParentId }) };
        }
    }
}