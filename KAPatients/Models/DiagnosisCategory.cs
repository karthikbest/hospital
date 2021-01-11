using System;
using System.Collections.Generic;

namespace KAPatients.Models
{
    public partial class DiagnosisCategory
    {
        public DiagnosisCategory()
        {
            Diagnosis = new HashSet<Diagnosis>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Diagnosis> Diagnosis { get; set; }
    }
}
