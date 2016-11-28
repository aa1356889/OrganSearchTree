
using Energy.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Energy.Share
{
    [Serializable]
    public class UserTicket 
    {
        public string OrganId { get; set; }
        public string UserId { get; set; }
        public string Guid { get; set; }
        public string UserName { get; set; }
        public string AuthCode { get; set; }
        public string RealName { get; set; }
        public string DeptId { get; set; }
        public string Sign { get; set; }
        public List<Operation> OperationTable { get; set; }

      
    }
}
