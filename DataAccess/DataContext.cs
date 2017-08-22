using DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DataContext : DbContext
    {
        public DbSet<File> Files  { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Date> Dates {get; set;}

        public DataContext()
            : base(@"Data Source=donau.hiof.no; Initial Catalog=fredrha; Uid=fredrha; Password=Sommer15;")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }
    }
}
