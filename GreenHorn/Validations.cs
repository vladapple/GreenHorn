using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GreenHorn
{
    public class Validations
    {
        //COMPANY TABLE BELOW
        public static bool IsNameValid(string name, out string error)
        {
            if ((Regex.IsMatch(name, @"(^[a-zA-Z][a-zA-Z\s]{0,20}[a-zA-Z]$)")) && name.Length > 1 && name.Length < 100)
            {
                error = null;
                return true;
            }
            error = "Invalid name!";
            return false;
        }
        public static bool IsCompanyNameValid(string name, out string error)
        {
            if ((Regex.IsMatch(name, @"\b([A-Za-zÀ-ÿ][-, a-z. ']+[ ]*)+")) && name.Length > 1 && name.Length < 100)
            {
                error = null;
                return true;
            }
            error = "Invalid company name!";
            return false;
        }

        public static bool IsAddressValid(string address, out string error)
        {
            if (address.Length >= 5 && address.Length <= 250)
            {
                error = null;
                return true;
            }
            error = "Address has to be 5-250 characters";
            return false;
        }

        public static bool IsEmailValid(string email, out string error)
        {
            if (Regex.Match(email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$").Success && email.Length >= 6 && email.Length <= 100)
            {
                error = null;
                return true;
            }
            error = "Wrong email format! Format email@email.com is needed!";
            return false;
        }

        public static bool IsPhoneValid(string phone, out string error)
        {
            if (Regex.Match(phone, @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$").Success && phone.Length >= 6 && phone.Length <= 15)
            {
                error = null;
                return true;
            }
            error = "Invalid phone format!";
            return false;
        }
        //POSITION AND INDUSTRY TABLES BELOW
        public static bool IsIndustryNameValid(string name, out string error)
        {
            if((Regex.IsMatch(name, "([a-zA-Z0-9-. *]")) &&
                name.Length > 1 && name.Length < 75)
            {
                error = null;
                return true;
            }
            error = "Invalid Industry format, must be between 1 and 75 characters, only letters, numbers, dashes, and apostrophes allowed.";
            return false;
        }

        public static bool IsPositionTypeValid(string name, out string error)
        {
            if (name.Length > 1 && name.Length < 75)
            {
                error = null;
                return true;
            }
            error = "Invalid position type, must be between 1 and 75 characters";
            return false;
        }

        public static bool IsPositionNameValid(string name, out string error)
        {
            if ((Regex.IsMatch(name, @"\b([A-Za-z][-,a-z. ']+[ ]*)+")) && name.Length > 1 && name.Length < 100)
            {
                error = null;
                return true;
            }
            error = "Invalid name, only letters and digits between 1 and 100 characters allowed.";
                return false;
        }

        public static bool IsYearValid(DateTime date, out string error)
        {
            if (date.Year > 1900 && date.Year <2100)
            {
                error = null;
                return true;
            }
            error = "Date must be in (1901 - 2099)";
            return false;
        }
    }
}
