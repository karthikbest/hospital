using KAClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace KAPatients.Models
{

    [ModelMetadataType(typeof(KAPatientMetadata))]
    public partial class Patient : IValidatableObject
    {
        PatientsContext localPatientContext;
        string firstLetterPostalCode;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //throw new NotImplementedException();

            if (FirstName == null || FirstName.Trim() == "")
            {
                yield return new ValidationResult("First name cannot be empty or just blanks",
                                                    new[] { nameof(FirstName) });
            }

            if (!(FirstName == null || FirstName.Trim() == ""))
            {
                FirstName = FirstName.Trim();
            }

            if (LastName == null || LastName.Trim() == "")
            {
                yield return new ValidationResult("Last name cannot be empty or just blanks",
                                                new[] { nameof(LastName) });


            }

            if (!(LastName == null || LastName.Trim() == ""))
            {

                LastName = LastName.Trim();

            }


            FirstName = KAValidations.KACapitalize(FirstName);
            LastName = KAValidations.KACapitalize(LastName);
            Address = KAValidations.KACapitalize(Address);
            City = KAValidations.KACapitalize(City);
            Gender = KAValidations.KACapitalize(Gender);


            if (!String.IsNullOrEmpty(ProvinceCode))
            {
                ProvinceCode = ProvinceCode.ToUpper();






            }


            if (!String.IsNullOrEmpty(Ohip))
            {

                Ohip = KAValidations.KACapitalize(Ohip);

                if (!KAValidations.KAOhipValidation(Ohip))
                {
                    yield return new ValidationResult("Ohip format not accepted. Accepted format is 1234-123-123-XX",
                                               new[] { nameof(Ohip) });
                }


            }

            if (!String.IsNullOrEmpty(HomePhone))
            {

                string phoneDigits = KAValidations.KAExtractDigits(HomePhone);

                if (Regex.IsMatch(phoneDigits, @"^[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]$"))
                {

                    phoneDigits = phoneDigits.Insert(3, "-");
                    phoneDigits = phoneDigits.Insert(7, "-");



                    HomePhone = phoneDigits;



                }

                else
                {
                    yield return new ValidationResult("Home Phone number format is wrong. Home phone number must contain exactly 10 digits",
                                             new[] { nameof(HomePhone) });

                }

            }


            if (!String.IsNullOrEmpty(DateOfBirth.ToString()))
            {
                if (DateOfBirth > DateTime.Now)
                {

                    yield return new ValidationResult("Date of birth cannot be in future",
                                           new[] { nameof(DateOfBirth) });

                }



            }


            if (Deceased)
            {
                if (String.IsNullOrWhiteSpace(DateOfDeath.ToString()))
                {
                    yield return new ValidationResult("Date of Death is compulsary for deceased patients",
                                            new[] { nameof(DateOfDeath) });
                }

            }

            if (!Deceased)
            {
                if (!String.IsNullOrWhiteSpace(DateOfDeath.ToString()))
                {
                    yield return new ValidationResult("Cannot add date of death for patient who is not dead (deceased must be checked)",
                                            new[] { nameof(DateOfDeath) });
                }

            }

            if (!String.IsNullOrWhiteSpace(DateOfDeath.ToString()))
            {

                if (DateOfDeath < DateOfBirth)
                {
                    yield return new ValidationResult("Date of death cannot be before date of birth.",
                                            new[] { nameof(DateOfDeath) });
                }

                if (DateOfDeath > DateTime.Now)
                {

                    yield return new ValidationResult("Date of death cannot be in future",
                                           new[] { nameof(DateOfDeath) });

                }

            }

            if (String.IsNullOrWhiteSpace(Gender))
            {

                yield return new ValidationResult("Gender is required",
                                       new[] { nameof(Gender) });


            }

            if (!(Gender == "M" || Gender == "F" || Gender == "X"))
            {


                yield return new ValidationResult("Gender must be M or F or X",
                                       new[] { nameof(Gender) });
            }



            localPatientContext = (PatientsContext)validationContext.GetService(typeof(PatientsContext));        



             if (!String.IsNullOrEmpty(ProvinceCode))
            {
                ProvinceCode = KAValidations.KACapitalize(ProvinceCode);
                //try
                //{
                    if (!(localPatientContext.Province.Any(x => x.ProvinceCode == ProvinceCode)))
                    {
                        yield return new ValidationResult("This province name do not exists",
                                          new[] { nameof(ProvinceCode) });
                    }
                //}

                //catch (Exception ex) {

                //}
            }


            if (!String.IsNullOrEmpty(PostalCode))
            {
                if (String.IsNullOrEmpty(ProvinceCode))
                {
                    yield return new ValidationResult("In order to add/edit postal code, You need to add valid Province code ",
                                          new[] { nameof(PostalCode), nameof(ProvinceCode) });
                }

                PostalCode = PostalCode.ToUpper();
                firstLetterPostalCode = PostalCode[0].ToString();

            }

            if (localPatientContext.Province.Where(x => x.ProvinceCode == ProvinceCode)
                .Select(x => x.CountryCode).FirstOrDefault() == "CA")
            {

                if (ProvinceCode.ToUpper() == "ON")
                {
                    if (!(firstLetterPostalCode == "K" || firstLetterPostalCode == "L" || firstLetterPostalCode == "M" || firstLetterPostalCode == "N" || firstLetterPostalCode == "P"))
                    {
                        yield return new ValidationResult("Postal Code and Province Name are not matching ",
                                         new[] { nameof(PostalCode), nameof(ProvinceCode) });

                    }
                }

                else if (ProvinceCode.ToUpper() == "QC")

                {
                    if (!(firstLetterPostalCode == "G" || firstLetterPostalCode == "H" || firstLetterPostalCode == "J"))
                    {
                        yield return new ValidationResult("Postal Code and Province Name are not matching ",
                                         new[] { nameof(PostalCode), nameof(ProvinceCode) });

                    }


                }

                else
                {

                    if (!(localPatientContext.Province.Where(x => x.ProvinceCode == ProvinceCode)
                .Select(x => x.FirstPostalLetter).FirstOrDefault() == firstLetterPostalCode))
                    {
                        yield return new ValidationResult("Postal Code and Province Name are not matching ",
                                       new[] { nameof(PostalCode), nameof(ProvinceCode) });

                    }

                }

                if (!KAValidations.KAPostalCodeValidation(PostalCode))
                {

                    yield return new ValidationResult("The postal code is not as per the format",
                                       new[] { nameof(PostalCode) });

                }

                if (KAValidations.KAPostalCodeValidation(PostalCode))
                {
                    PostalCode = KAValidations.KAPostalCodeFormat(PostalCode);

                }

            }

            if (localPatientContext.Province.Where(x => x.ProvinceCode == ProvinceCode)
                  .Select(x => x.CountryCode).FirstOrDefault() == "US")

            {

                if (! KAValidations.KAZipCodeValidation(PostalCode))
                {
                    yield return new ValidationResult("The ZIP code is not as per the format",
                                      new[] { nameof(PostalCode) });

                    

                }

            }


                yield return ValidationResult.Success;
        }
    }
    public class KAPatientMetadata
    {
        //public Patient()
        //{
        //    PatientDiagnosis = new HashSet<PatientDiagnosis>();
        //}

        public int PatientId { get; set; }




        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Street Address")]

        public string Address { get; set; }
        public string City { get; set; }

        [Display(Name = "Province Code")]
        public string ProvinceCode { get; set; }



        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "OHIP")]
        public string Ohip { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]

        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }
        public bool Deceased { get; set; }

        [Display(Name = "Date of Death")]

        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}")]
        public DateTime? DateOfDeath { get; set; }

        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }

      
        public string Gender { get; set; }




    }


}
