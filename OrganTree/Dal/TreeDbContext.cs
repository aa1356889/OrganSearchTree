using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Energy.Entity;

namespace OrganTree.Dal
{
    public class TreeDbContext: DbContext
    {


        public TreeDbContext()
            : base("EFDbContext")
        {

        }
        public DbSet<Department> Department { get; set; }
        public DbSet<Organ> Organ { get; set; }
    }
}