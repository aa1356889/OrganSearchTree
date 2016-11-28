
using Energy.Entity;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace Energy.Business
{
    public  class OrganBusiness:BussinesBase
    {


        /// <summary>
        /// 通过recordid得到organ
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public  Organ GetByRecordId(string recordId)
        {
            if (string.IsNullOrWhiteSpace(recordId))
            {
                return new Organ();
            }
            return base.Context.Organ.ToList().FirstOrDefault(c => c.RecordID == recordId);
        }

        /// <summary>
        /// 获得指定节点下面的所有子节点以及孙子节点
        /// </summary>
        /// <param name="recordid"></param>
        /// <returns></returns>
        public  List<Organ> GetALLSonByRecordid(string recordid)
        {
            if (string.IsNullOrEmpty(recordid))
            {
              return  new List<Organ>();
            }
            return base.Context.Organ.Where(c => c.Path.IndexOf(recordid) > 0&&c.IsDelete=="0").ToList<Organ>();
           // return idba.Where("select * from Organ o where instr(o.path,:p1)>0 and o.isdelete=0 order by o.organid", recordid).ToList<Organ>();
        }



        /// <summary>
        /// 获取父ID下的子组织机构（儿子这一层）同时增加path搜索限制
        /// 2016-11-22 新增 李强
        /// </summary>
        /// <param name="parentId">组织机构父id</param>
        /// <param name="path">path限制</param>
        /// <returns></returns>
        public  List<Organ> GetChildrenList(string parentId,string path)
        {
            if (parentId == null)
            {
                return new List<Organ>();
            }
            Expression<Func<Organ, bool>> eps = c => c.IsDelete == "0";

            if (!string.IsNullOrEmpty(parentId))
            {

                eps= eps.And(c => c.ParentId == parentId);

            }
            else
            {
                eps= eps.And(c => c.ParentId == null);
            }
            if (!string.IsNullOrEmpty(path))
            {
                eps= eps.And(c => c.Path.IndexOf(path) > 0);

            }
            return base.Context.Organ.Where(eps).ToList<Organ>();
        }












        /// <summary>
        /// 根据名称 获得机构信息以及父级机构信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public  List<Organ> GetOrgansByName(string name,string path,string organid)
        {
            string sql=@"select * from organs o where o.recordid in(select o.parentid from Organs o
                         where o.organname like @name {0} and isdelete='0')
                         union 
                         select * from Organs o
                         where o.organname like @name {1} and isdelete='0'";
            string where = string.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@name", "%"+name+"%")
            };
            if (!string.IsNullOrEmpty(path))
            {
                where = " and charindex(path,@path)>0 ";
                parameters.Add(new SqlParameter("@path", path));
            }
            if (!string.IsNullOrEmpty(organid))
            {
                where += " and recordid=:p5";
                parameters.Add(new SqlParameter("p5", organid));
            }
            return base.Context.Database.SqlQuery<Organ>(string.Format(sql,where, where), parameters.ToArray()).ToList();
        }
        
    }
}
