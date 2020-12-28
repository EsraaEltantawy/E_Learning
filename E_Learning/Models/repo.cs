using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Learning.Models
{
    public class repo
    {
        public IEnumerable<SubjectClass> SubjectClasses { get; set; }
        public IEnumerable<Teacher> Teachers { get; set; }

    }
}