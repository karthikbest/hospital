using System;
using System.Collections.Generic;

namespace KAPatients.Models
{
    public partial class TreatmentMedication
    {
        public int TreatmentId { get; set; }
        public string Din { get; set; }

        public virtual Medication DinNavigation { get; set; }
        public virtual Treatment Treatment { get; set; }
    }
}
