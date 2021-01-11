using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KAPatients.Models
{
    public partial class PatientsContext : DbContext
    {
        public PatientsContext()
        {
        }

        public PatientsContext(DbContextOptions<PatientsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ConcentrationUnit> ConcentrationUnit { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Diagnosis> Diagnosis { get; set; }
        public virtual DbSet<DiagnosisCategory> DiagnosisCategory { get; set; }
        public virtual DbSet<DispensingUnit> DispensingUnit { get; set; }
        public virtual DbSet<Medication> Medication { get; set; }
        public virtual DbSet<MedicationType> MedicationType { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<PatientDiagnosis> PatientDiagnosis { get; set; }
        public virtual DbSet<PatientMedication> PatientMedication { get; set; }
        public virtual DbSet<PatientTreatment> PatientTreatment { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<Treatment> Treatment { get; set; }
        public virtual DbSet<TreatmentMedication> TreatmentMedication { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localDB)\\MsSqlLocalDB;DataBase=Patients;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<ConcentrationUnit>(entity =>
            {
                entity.HasKey(e => e.ConcentrationCode);

                entity.ToTable("concentrationUnit");

                entity.Property(e => e.ConcentrationCode)
                    .HasColumnName("concentrationCode")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryCode);

                entity.ToTable("country");

                entity.Property(e => e.CountryCode)
                    .HasColumnName("countryCode")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.FederalSalesTax).HasColumnName("federalSalesTax");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhonePattern)
                    .HasColumnName("phonePattern")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PostalPattern)
                    .HasColumnName("postalPattern")
                    .HasMaxLength(120)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Diagnosis>(entity =>
            {
                entity.HasKey(e => e.DiagnosisId)
                    .HasName("aaaaadiagnosis_PK")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("diagnosis");

                entity.HasIndex(e => e.DiagnosisCategoryId)
                    .HasName("diseasecategoryId");

                entity.HasIndex(e => e.DiagnosisId)
                    .HasName("ailmentId");

                entity.Property(e => e.DiagnosisId).HasColumnName("diagnosisId");

                entity.Property(e => e.DiagnosisCategoryId).HasColumnName("diagnosisCategoryId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.DiagnosisCategory)
                    .WithMany(p => p.Diagnosis)
                    .HasForeignKey(d => d.DiagnosisCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_diagnosis_diagnosisCategory");
            });

            modelBuilder.Entity<DiagnosisCategory>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("aaaaacategory_PK")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("diagnosisCategory");

                entity.HasIndex(e => e.Id)
                    .HasName("categoryId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DispensingUnit>(entity =>
            {
                entity.HasKey(e => e.DispensingCode);

                entity.ToTable("dispensingUnit");

                entity.Property(e => e.DispensingCode)
                    .HasColumnName("dispensingCode")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<Medication>(entity =>
            {
                entity.HasKey(e => e.Din)
                    .HasName("aaaaamedication_PK")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("medication");

                entity.Property(e => e.Din)
                    .HasColumnName("din")
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Concentration).HasColumnName("concentration");

                entity.Property(e => e.ConcentrationCode)
                    .IsRequired()
                    .HasColumnName("concentrationCode")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DispensingCode)
                    .IsRequired()
                    .HasColumnName("dispensingCode")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasColumnName("image")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MedicationTypeId).HasColumnName("medicationTypeId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(20);

                entity.HasOne(d => d.ConcentrationCodeNavigation)
                    .WithMany(p => p.Medication)
                    .HasForeignKey(d => d.ConcentrationCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_medication_concentrationUnit");

                entity.HasOne(d => d.DispensingCodeNavigation)
                    .WithMany(p => p.Medication)
                    .HasForeignKey(d => d.DispensingCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_medication_dispensingUnit");

                entity.HasOne(d => d.MedicationType)
                    .WithMany(p => p.Medication)
                    .HasForeignKey(d => d.MedicationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_medication_medicationType");
            });

            modelBuilder.Entity<MedicationType>(entity =>
            {
                entity.ToTable("medicationType");

                entity.Property(e => e.MedicationTypeId).HasColumnName("medicationTypeId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.PatientId)
                    .HasName("aaaaapatient_PK")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("patient");

                entity.HasIndex(e => e.HomePhone)
                    .HasName("homePhone");

                entity.HasIndex(e => e.PatientId)
                    .HasName("patientId");

                entity.HasIndex(e => e.PostalCode)
                    .HasName("postalCode");

                entity.HasIndex(e => e.ProvinceCode)
                    .HasName("provincepatient");

                entity.Property(e => e.PatientId).HasColumnName("patientId");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfBirth)
                    .HasColumnName("dateOfBirth")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateOfDeath)
                    .HasColumnName("dateOfDeath")
                    .HasColumnType("datetime");

                entity.Property(e => e.Deceased).HasColumnName("deceased");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("firstName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.HomePhone)
                    .HasColumnName("homePhone")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("lastName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ohip)
                    .HasColumnName("OHIP")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PostalCode)
                    .HasColumnName("postalCode")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ProvinceCode)
                    .HasColumnName("provinceCode")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.HasOne(d => d.ProvinceCodeNavigation)
                    .WithMany(p => p.Patient)
                    .HasForeignKey(d => d.ProvinceCode)
                    .HasConstraintName("FK_patient_province");
            });

            modelBuilder.Entity<PatientDiagnosis>(entity =>
            {
                entity.ToTable("patientDiagnosis");

                entity.Property(e => e.PatientDiagnosisId).HasColumnName("patientDiagnosisId");

                entity.Property(e => e.Comments).HasColumnName("comments");

                entity.Property(e => e.DiagnosisId).HasColumnName("diagnosisId");

                entity.Property(e => e.PatientId).HasColumnName("patientId");

                entity.HasOne(d => d.Diagnosis)
                    .WithMany(p => p.PatientDiagnosis)
                    .HasForeignKey(d => d.DiagnosisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_patientDiagnosis_diagnosis");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientDiagnosis)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_patientDiagnosis_patient");
            });

            modelBuilder.Entity<PatientMedication>(entity =>
            {
                entity.HasKey(e => e.PatientMedicationId)
                    .HasName("aaaaapatientMedication_PK")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("patientMedication");

                entity.HasIndex(e => e.Din)
                    .HasName("medicationpatientMedication");

                entity.HasIndex(e => e.PatientTreatmentId)
                    .HasName("patientTreatmentpatientMedication");

                entity.Property(e => e.PatientMedicationId).HasColumnName("patientMedicationId");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .IsUnicode(false);

                entity.Property(e => e.Din)
                    .IsRequired()
                    .HasColumnName("din")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Dose)
                    .HasColumnName("dose")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ExactMinMax)
                    .HasColumnName("exactMinMax")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(N'exact')");

                entity.Property(e => e.Frequency)
                    .HasColumnName("frequency")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FrequencyPeriod)
                    .HasColumnName("frequencyPeriod")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PatientTreatmentId).HasColumnName("patientTreatmentId");

                entity.HasOne(d => d.DinNavigation)
                    .WithMany(p => p.PatientMedication)
                    .HasForeignKey(d => d.Din)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_patientMedication_medication");

                entity.HasOne(d => d.PatientTreatment)
                    .WithMany(p => p.PatientMedication)
                    .HasForeignKey(d => d.PatientTreatmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_patientMedication_patientTreatment");
            });

            modelBuilder.Entity<PatientTreatment>(entity =>
            {
                entity.HasKey(e => e.PatientTreatmentId)
                    .HasName("aaaaapatientTreatment_PK")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("patientTreatment");

                entity.HasIndex(e => e.PatientTreatmentId)
                    .HasName("patientProcedureId");

                entity.HasIndex(e => e.TreatmentId)
                    .HasName("procedurepatientProcedure");

                entity.Property(e => e.PatientTreatmentId).HasColumnName("patientTreatmentId");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .IsUnicode(false);

                entity.Property(e => e.DatePrescribed)
                    .HasColumnName("datePrescribed")
                    .HasColumnType("datetime");

                entity.Property(e => e.PatientDiagnosisId).HasColumnName("patientDiagnosisId");

                entity.Property(e => e.TreatmentId).HasColumnName("treatmentId");

                entity.HasOne(d => d.PatientDiagnosis)
                    .WithMany(p => p.PatientTreatment)
                    .HasForeignKey(d => d.PatientDiagnosisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_patientTreatment_patientDiagnosis");

                entity.HasOne(d => d.Treatment)
                    .WithMany(p => p.PatientTreatment)
                    .HasForeignKey(d => d.TreatmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("patientTreatment_FK01");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.HasKey(e => e.ProvinceCode)
                    .HasName("aaaaaprovince_PK")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("province");

                entity.HasIndex(e => e.ProvinceCode)
                    .HasName("ProvinceCode");

                entity.Property(e => e.ProvinceCode)
                    .HasColumnName("provinceCode")
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasColumnName("countryCode")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.FirstPostalLetter)
                    .HasColumnName("firstPostalLetter")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IncludesFederalTax).HasColumnName("includesFederalTax");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SalesTax).HasColumnName("salesTax");

                entity.Property(e => e.SalesTaxCode)
                    .IsRequired()
                    .HasColumnName("salesTaxCode")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.CountryCodeNavigation)
                    .WithMany(p => p.Province)
                    .HasForeignKey(d => d.CountryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_province_country");
            });

            modelBuilder.Entity<Treatment>(entity =>
            {
                entity.HasKey(e => e.TreatmentId)
                    .HasName("aaaaatreatment_PK")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("treatment");

                entity.HasIndex(e => e.DiagnosisId)
                    .HasName("diagnosisprocedure");

                entity.HasIndex(e => e.TreatmentId)
                    .HasName("procedureId");

                entity.Property(e => e.TreatmentId).HasColumnName("treatmentId");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .IsUnicode(false);

                entity.Property(e => e.DiagnosisId).HasColumnName("diagnosisId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Diagnosis)
                    .WithMany(p => p.Treatment)
                    .HasForeignKey(d => d.DiagnosisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_treatment_diagnosis");
            });

            modelBuilder.Entity<TreatmentMedication>(entity =>
            {
                entity.HasKey(e => new { e.TreatmentId, e.Din })
                    .HasName("aaaaatreatmentMedication_PK")
                    .ForSqlServerIsClustered(false);

                entity.ToTable("treatmentMedication");

                entity.HasIndex(e => e.Din)
                    .HasName("medicationtreatmentMedication");

                entity.HasIndex(e => e.TreatmentId)
                    .HasName("treatmenttreatmentMedication");

                entity.Property(e => e.TreatmentId).HasColumnName("treatmentId");

                entity.Property(e => e.Din)
                    .HasColumnName("din")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.HasOne(d => d.DinNavigation)
                    .WithMany(p => p.TreatmentMedication)
                    .HasForeignKey(d => d.Din)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_treatmentMedication_medication");

                entity.HasOne(d => d.Treatment)
                    .WithMany(p => p.TreatmentMedication)
                    .HasForeignKey(d => d.TreatmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_treatmentMedication_treatment");
            });
        }
    }
}
