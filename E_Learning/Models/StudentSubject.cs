//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace E_Learning.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class StudentSubject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StudentSubject()
        {
            this.StuSubTeas = new HashSet<StuSubTea>();
        }
    
        public int StuSubID { get; set; }
        public int SubLevelID { get; set; }
        public string StuId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    
        public virtual Student Student { get; set; }
        public virtual SubjectClass SubjectClass { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StuSubTea> StuSubTeas { get; set; }
    }
}
