using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DTSaveManager.Services
{
    public static class RegexHelperService
    {
        public static bool GetStringHasNumber(string s)
        {
            return Regex.Match(s, @"\d+").Success;
        }

        public static string GetNumberFromString(string s)
        {
            return Regex.Match(s, @"\d+").Value;
        }
    }
}
