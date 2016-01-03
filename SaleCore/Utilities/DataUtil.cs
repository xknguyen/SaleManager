using System;
using System.Globalization;

namespace SaleCore.Utilities
{
    public class DataUtil
    {
        public static long ToLong(object value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch
            {

                return 0;
            }
        }

        public static int ToInt(object value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {

                return 0;
            }
        }


        public static string ChangeMoneyToString(long money)
        {
            string s = "";
            string t = money.ToString();
            int i, j;
            j = t.Length % 3;
            for (i = 0; i < j; i++)
            {
                s += t[i];
            }
            if (j != 0 && t.Length>3)
                s += ".";
            int z = 0;
            while (i < t.Length)
            {
                if (z == 3)
                { s += "."; z = 0; }
                s += t[i++];
                z++;
            }
            s += " VNĐ";
            return s;
        }

        public static long ChangeStringToMoney(string money)
        {
            try
            {
                int i=0;
                string t = "";
                while (i < money.Length)
                {
                    if (money[i] != '.' && money[i] != 'V' && money[i] != 'N' && money[i] != 'Đ' && money[i] != ' ')
                    {
                        t += money[i];
                    }
                    i++;
                }
                return ToLong(t);
            }
            catch
            {
                return 0;
            }
        }

        public static DateTime ToDateTime(object value)
        {
            try
            {
                return Convert.ToDateTime(value);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static short ToShort(object value)
        {
            try
            {
                return Convert.ToInt16(value);
            }
            catch
            {

                return 0;
            }
        }

        public static string ToString(object value)
        {
            try
            {
                return Convert.ToString(value);
            }
            catch
            {
                return "";
            }
        }

        public static bool ToBool(object value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }

        public static double ToDouble(object value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {

                return 0;
            }
        }

        public static float ToFloat(object value)
        {
            try
            {
                return (float)Convert.ToDecimal(value);
            }
            catch
            {

                return 0;
            }
        }

        public static DateTime ToDateTime(string dateString, string format)
        {
            try
            {
                return DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}
