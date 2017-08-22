using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel
{
    public class File
    {
        public int FileId { get; set; }
        public String FileName { get; set; }
        public virtual ICollection<File> Files { get; set; }



    }
    public class Assignment
    {
        public int AssignmentId { get; set; }
        public String AssignmentName { get; set; }
        public virtual ICollection<Date> Dates { get; set; }
        

        public Assignment()
        {
            Dates = new HashSet<Date>();
        }
    
    }
    public class Date {
        public int DateId {get; set;}
        public String DateName  {get; set;}
        public virtual ICollection<Assignment> Assignments {get; set;}
        public Date()
        {
            Assignments = new HashSet<Assignment>();
        }
    }
}
