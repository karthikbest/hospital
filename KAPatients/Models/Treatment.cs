using System;
using System.Collections.Generic;

namespace KAPatients.Models
{
    public partial class Treatment
    {
        public Treatment()
        {
            PatientTreatment = new HashSet<PatientTreatment>();
            TreatmentMedication = new HashSet<TreatmentMedication>();
        }

        public int? TreatmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DiagnosisId { get; set; }

        public virtual Diagnosis Diagnosis { get; set; }
        public virtual ICollection<PatientTreatment> PatientTreatment { get; set; }
        public virtual ICollection<TreatmentMedication> TreatmentMedication { get; set; }
    }
}
