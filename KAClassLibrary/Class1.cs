using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace KAClassLibrary
{
    public static class KAValidations
    {

        public  static string KACapitalize(string inputString)
        {
            if (String.IsNullOrEmpty(inputString))
                return ("");

            else
            {
                inputString = inputString.ToLower();
                inputString = inputString.Trim();
              
                //To capitalize each word
                TextInfo ti = new CultureInfo("en-US", false).TextInfo;
                return (ti.ToTitleCase(inputString));



            }

        }

        public static string KAExtractDigits(string inputString)
        {
            string output="";          

            for (int i = 0; i < inputString.Length; i++)
            {

                if (char.IsDigit(inputString[i]))
                {
                    output += inputString[i];
                }

            }
            return output;          
        }

        public static Boolean KAPostalCodeValidation (string inputString)
        {
            //inputString = KAPostalCodeFormat(inputString);
            if (Regex.IsMatch(inputString.ToUpper(), @"^[ABCEGHJ-NPRSTVXY][0-9][ABCEGHJ-NPRSTV-Z] [0-9][ABCEGHJ-NPRSTV-Z][0-9]$"))
            {
                return true;

            }

            else if (String.IsNullOrEmpty(inputString))
                return true;

            else
                return false;

        }


        public static Boolean KAOhipValidation(string inputString)
        {
            if (Regex.IsMatch(inputString.ToUpper(), @"^[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9]-[0-9][0-9][0-9]-[A-Za-z][A-Za-z]$"))
            {
                return true;

            }

            else if (String.IsNullOrEmpty(inputString))
                return true;

            else
                return false;


        }


     
        public static string KAPostalCodeFormat( string inputString)

        {
            if (String.IsNullOrEmpty(inputString))
            {
                return "";

            }

            else
            {
                inputString = inputString.ToUpper();
                inputString.Insert(3, " ");
                
               
             


                return inputString;


            }

        }


        public static Boolean KAZipCodeValidation(string inputString)
        {

            if (String.IsNullOrEmpty(inputString))
            {
                inputString = "";
                return true;
            }

            else
            {
                string digits = "";

                for (int i = 0; i <= inputString.Length; i++)
                {

                    if (char.IsDigit(inputString[i]))
                    {
                        digits += inputString[i];
                    }

                }
                if (digits.Length == 5)
                {
                    inputString = digits;
                    return true;

                }
                else if (digits.Length == 9)
                {
                    inputString.Insert(5, " ");
                    return true;
                }
                else
                    return false;



            }




        }

  



    }
}
