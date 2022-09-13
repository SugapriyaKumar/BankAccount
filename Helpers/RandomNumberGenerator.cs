using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankAccount.Helpers
{
    public class RandomNumberGenerator
    {
        private static readonly Random _rdm = new Random();
        public static int PinGenerator(int digits)
        {
            if (digits <= 1) return 0;

            var _min = (int)Math.Pow(10, digits - 1);
            var _max = (int)Math.Pow(10, digits) - 1;
            return _rdm.Next(_min, _max);
        }
    }
}