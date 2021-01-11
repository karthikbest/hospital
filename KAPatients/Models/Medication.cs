using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KAPatients.Models
{
    public partial class Medication
    {
        public Medication()
        {
            PatientMedication = new HashSet<PatientMedication>();
            TreatmentMedication = new HashSet<TreatmentMedication>();
        }

        [StringLength(8)]
        [Required]
        public string Din { get; set; }

        [Required]
        public string Name { get; set; }
        public string Image { get; set; }
        public int MedicationTypeId { get; set; }
        public string DispensingCode { get; set; }
        public double Concentration { get; set; }
        public string ConcentrationCode { get; set; }

        [Display(Name="ConcentrationCode")]

        public virtual ConcentrationUnit ConcentrationCodeNavigation { get; set; }

        [Display(Name = "DispensingCode")]
        public virtual DispensingUnit DispensingCodeNavigation { get; set; }

        
        public virtual MedicationType MedicationType { get; set; }
        public virtual ICollection<PatientMedication> PatientMedication { get; set; }
        public virtual ICollection<TreatmentMedication> TreatmentMedication { get; set; }
        
    }
}
