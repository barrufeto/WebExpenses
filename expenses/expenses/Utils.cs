using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace expenses
{
    public  class Dates
    {
        private  int _Mes;
        private  int _Any;

        public  int Mes
        {

            get
            {
                return _Mes;
            }

            set
            {
                _Mes = value;
            }
        }

        public  int Any
        {

            get
            {
                return _Any;
            }

            set
            {
                _Any = value;
            }
        }

    }

    public static class Utils
    {
        public static decimal parseDecimal(string value)
        {

            value = value.Replace(" ", "");
            if (value != "")
            {
                if (System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",")
                {
                    value = value.Replace(".", ",");
                }
                else
                {
                    value = value.Replace(",", ".");
                }
                string[] splited = value.Split(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]);
                if (splited.Length > 2)
                {
                    string r = "";
                    for (int i = 0; i < splited.Length; i++)
                    {
                        if (i == splited.Length - 1)
                            r += System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                        r += splited[i];
                    }
                    value = r;
                }
                return decimal.Parse(value);
            }
            else return decimal.Parse("0");
        }

    }
}