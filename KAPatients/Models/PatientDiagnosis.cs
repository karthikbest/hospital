using System;
using System.Collections.Generic;

namespace KAPatients.Models
{
    public partial class PatientDiagnosis
    {
        public PatientDiagnosis()
        {
            PatientTreatment = new HashSet<PatientTreatment>();
        }

        public int PatientDiagnosisId { get; set; }
        public int PatientId { get; set; }
        public int DiagnosisId { get; set; }
        public string Comments { get; set; }

        public virtual Diagnosis Diagnosis { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual ICollection<PatientTreatment> PatientTreatment { get; set; }

        //public virtual PatientTreatment PatientTreatment { get; set; }


    }
}
