using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Energy.Entity;

namespace OrganTree.OperationServer
{
    public class OrganAdapter
    {
        public static OrganOperaction GetOrganOperaction(IEnumerable<Operation> userOperactions,string[] validateOpractionCodes,string userOrganId,string userDepid)
        {
            OrganOperaction organOperaction = null;
            if (userOperactions.Any(c => c.OperationName == validateOpractionCodes[0]))
            {
                organOperaction = new OrganAllOpraction(userOrganId, userDepid);
            }
            else if (userOperactions.Any(c => c.OperationName == validateOpractionCodes[1]))
            {

                organOperaction = new BrotherOperaction(userOrganId, userDepid);

            }
            else if (userOperactions.Any(c => c.OperationName == validateOpractionCodes[2]))
            {
                organOperaction = new OrganSonOraction(userOrganId, userDepid);
            }
            else
            {
                organOperaction = new OrganCurrent(userOrganId, userDepid);

            }
            return organOperaction;
        }
    }
}