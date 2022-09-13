using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankAccount.Helpers
{
    public class AgeCalculator
    {
        public static int CalculateAge(DateTime dateOfBirth)
        {
            int age = DateTime.Now.Year-dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age-1;
            return age;
        }

    }
}