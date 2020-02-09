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

        public static string GetMesLetras(int iMes)
        {
            string _Res;
            switch (iMes)
            {
                case 1:
                    _Res = Resources.Descripciones.Enero;
                    break;
                case 2:
                    _Res = Resources.Descripciones.Febrero;
                    break;
                case 3:
                    _Res = Resources.Descripciones.Marzo;
                    break;
                case 4:
                    _Res = Resources.Descripciones.Abril;
                    break;
                case 5:
                    _Res = Resources.Descripciones.Mayo;
                    break;
                case 6:
                    _Res = Resources.Descripciones.Junio;
                    break;
                case 7:
                    _Res = Resources.Descripciones.Julio;
                    break;
                case 8:
                    _Res = Resources.Descripciones.Agosto;
                    break;
                case 9:
                    _Res = Resources.Descripciones.Septiembre;
                    break;
                case 10:
                    _Res = Resources.Descripciones.Octubre;
                    break;
                case 11:
                    _Res = Resources.Descripciones.Noviembre;
                    break;
                default:
                    _Res = Resources.Descripciones.Diciembre;
                    break;
            }
            return _Res;
        }

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