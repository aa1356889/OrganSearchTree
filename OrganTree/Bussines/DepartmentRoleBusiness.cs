
using Energy.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using OrganTree.Dal;

namespace Energy.Business
{
    public  class DepartmentBusiness:BussinesBase
    {
      
        /// <summary>
        /// 获取某个部门的子级部门(两个参数至少传一个)
        /// </summary>
        /// <param name="organId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public  List<Department> GetDetpList(string organId, string parentId,string udepid="",string path="")
        {
            if (string.IsNullOrWhiteSpace(parentId) && string.IsNullOrWhiteSpace(organId))
            {
                return null;
            }
            Expression<Func<Department, bool>> sqlExpression = t => t.IsDelete == "0";

            if (!string.IsNullOrWhiteSpace(organId))
            {
                sqlExpression = sqlExpression.And(c => c.OrganId == organId);
            }
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                sqlExpression = sqlExpression.And(c => c.ParentId == parentId);
            }
            else
            {
                sqlExpression = sqlExpression.And(c => c.ParentId == null);
            }
            if (!string.IsNullOrEmpty(path))
            {
                sqlExpression = sqlExpression.And(c => c.Path.IndexOf(path) > 0);
            }

            if (!string.IsNullOrEmpty(udepid))
            {
                sqlExpression = sqlExpression.And(c => c.Path.IndexOf(udepid) > 0);
            }
            return base.Context.Department.Where(sqlExpression).ToList<Department>();

        }





        /// <summary>
        /// 通过recordid得到Department
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public  Department GetByRecordId(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId))
            {
                return new Department();
            }
            return base.Context.Department.Where(c => c.RecordID == recordId).FirstOrDefault();
                //idba.Where("select * from Department where recordId=:recordid", new OracleParameter(":recordid", recordId)).To<Department>();
        }


        /// <summary>
        /// 模糊查询 根据地区名字获得相关信息
        /// </summary>
        /// <param name="depName"></param>
        /// <returns></returns>
        public  List<Department> GetDepartmentsByName(string depName,string organId,string path,string depid)
        {

        
            string sql = "select * from Departments d where d.deptname like @name and isdelete='0'";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@name","%"+depName+"%")
            };
            if (!string.IsNullOrEmpty(path))
            {
                sql += " and instr(path,@path)>0 ";
                parameters.Add(new SqlParameter("@path", path));
            }
            if (!string.IsNullOrEmpty(organId))
            {
                sql += " and exists(select o.organid from organ o where o.recordid=d.organid and instr(o.path,@orgid)>0)";
                parameters.Add(new SqlParameter("@orgid", organId));
            }
            if (!string.IsNullOrEmpty(depid))
            {
                sql += " and recordid=@p5";
                parameters.Add(new SqlParameter("@p5", depid));
            }
            return base.Context.Database.SqlQuery<Department>(sql,parameters.ToArray()).ToList();
        }

    }
}
