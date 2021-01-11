using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace KAPatients.Models
{
    public partial class Diagnosis
    {
        public Diagnosis()
        {
            PatientDiagnosis = new HashSet<PatientDiagnosis>();
            Treatment = new HashSet<Treatment>();
        }

        public int DiagnosisId { get; set; }

        [DisplayName("Diagnosis Name")]
        public string Name { get; set; }
        public int DiagnosisCategoryId { get; set; }

        public virtual DiagnosisCategory DiagnosisCategory { get; set; }
        public virtual ICollection<PatientDiagnosis> PatientDiagnosis { get; set; }
        public virtual ICollection<Treatment> Treatment { get; set; }
    }
}
