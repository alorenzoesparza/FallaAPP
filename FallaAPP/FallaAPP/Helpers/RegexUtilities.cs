using System;
using System.Text.RegularExpressions;

namespace FallaAPP.Helpers
{
    public class RegexUtilities
    {
        public static Boolean ValidarEmail(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static Boolean ValidarPassword(String password)
        {
            String expresion;
            expresion = "(^(?=.*\\d)(?=.*[\u0021-\u002b\u003c-\u0040])(?=.*[A-Z])(?=.*[a-z])\\S{6,20}$)";
            if (Regex.IsMatch(password, expresion))
            {
                if (Regex.Replace(password, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
