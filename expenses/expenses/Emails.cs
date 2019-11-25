using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpensesEF;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Globalization;

namespace expenses
{
    public class Emails
    {
        int _idIdioma;
        //Retorna mes en letras
        private string _GetMesLetras(int iMes)
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

        //Retorna Mes i any dels processos a executar...
        private Dates _GetMesAny()
        {
            Dates _dates = new Dates();

            if (DateTime.Now.Month == 1)
            {
                _dates.Mes = 12;
                _dates.Any = DateTime.Now.Year - 1;
            }
            else
            {
                _dates.Mes = DateTime.Now.Month - 1;
                _dates.Any = DateTime.Now.Year;
            }
            return _dates;
        }

        //Retorna Mes i any dels processos a executar...
        private Dates _GetMesAnterior()
        {
            Dates _dates = new Dates();

            if (DateTime.Now.Month == 1)
            {
                _dates.Mes = 11;
                _dates.Any = DateTime.Now.Year - 1;
            }

            if (DateTime.Now.Month == 2)
            {
                _dates.Mes = 12;
                _dates.Any = DateTime.Now.Year - 1;
            }
            else
            {
                _dates.Mes = DateTime.Now.Month - 2;
                _dates.Any = DateTime.Now.Year;
            }
            return _dates;
        }


        private string _VerMaximoGastoAnual(SummaryTotal_Mensual _GastoMesAnterior, decimal _MaxGasto)
        {
            string _Res = "";
            if (_GastoMesAnterior.GastoTotal == _MaxGasto && _GastoMesAnterior.Mes > 1)
            {
                _Res = String.Format(Resources.Descripciones.mailMaximoGastadoporMes, _GetMesLetras(_GastoMesAnterior.Mes));
            }
            return _Res;
        }

        //Mirem els gastos dels dos darrers mesos i veiem si hi ha hahut millora
        private string _MejoraGastoRespecto2MesesAtras(SummaryTotal_Mensual _GastoMesAnterior, SummaryTotal_Mensual _Gasto2MesesAtras)
        {
            string _Res = "";
            decimal valorDivActualMayor;
            decimal valorDivActualMenor;
            decimal valorDiferencia;

            if (_Gasto2MesesAtras != null)
            {
                //_GastoMesAnterior.GastoTotal = 15000;
                //_Gasto2MesesAtras.GastoTotal = 5000;

                valorDiferencia = Math.Abs(_GastoMesAnterior.GastoTotal - _Gasto2MesesAtras.GastoTotal);
                valorDivActualMayor = Math.Abs((Math.Round((_GastoMesAnterior.GastoTotal / _Gasto2MesesAtras.GastoTotal) * 100)-100));
                valorDivActualMenor = 100-(Math.Round((_Gasto2MesesAtras.GastoTotal / _GastoMesAnterior.GastoTotal) * 100));


                if (_GastoMesAnterior.GastoTotal > _Gasto2MesesAtras.GastoTotal)
                {
                    _Res = string.Format(Resources.Descripciones.mailGastoRespecto2MesesAtrasMas, valorDiferencia, valorDivActualMenor, _GetMesLetras(_Gasto2MesesAtras.Mes));
                }

                if (_GastoMesAnterior.GastoTotal == _Gasto2MesesAtras.GastoTotal)
                {
                    _Res = Resources.Descripciones.mailGastoRespecto2MesesAtrasIgual;
                }

                if (_GastoMesAnterior.GastoTotal < _Gasto2MesesAtras.GastoTotal)
                {
                    _Res = string.Format(Resources.Descripciones.mailGastoRespecto2MesesAtrasMenos, valorDiferencia, valorDivActualMayor, _GetMesLetras(_Gasto2MesesAtras.Mes));
                }
            }
            return _Res;
        }

        private string _GastoMedioMensual(SummaryTotal_Mensual _GastoMesAnterior, string _Usuari,int MesActual, Dates _Data2MesosEnrera)
        {
            string _Res = "";
            decimal valorDiferencia, valorDivActualMayor, valorDivActualMenor, _MediaGastoMensualAnual;
            var context = new ExpensesEF.Entities();

            try
            {
                _MediaGastoMensualAnual = Math.Round((context.SummaryTotal_Mensual.Where(x => x.idUser == _Usuari && x.Año == _Data2MesosEnrera.Any).Sum(x => x.GastoTotal)) / MesActual);

                if (_GastoMesAnterior.Mes > 1)
                {
                 
                    valorDiferencia = Math.Abs(_GastoMesAnterior.GastoTotal - _MediaGastoMensualAnual);
                                        
                    valorDivActualMenor = Math.Abs((Math.Round((_GastoMesAnterior.GastoTotal / _MediaGastoMensualAnual) * 100) - 100));
                    valorDivActualMayor = 100 - (Math.Round((_MediaGastoMensualAnual / _GastoMesAnterior.GastoTotal) * 100));

                    if (_GastoMesAnterior.GastoTotal > _MediaGastoMensualAnual)
                    {
                        _Res = string.Format(Resources.Descripciones.mailGastoMedioMensualMayorMedia, _MediaGastoMensualAnual, valorDiferencia, valorDivActualMayor);
                    }

                    if (_GastoMesAnterior.GastoTotal == _MediaGastoMensualAnual)
                    {
                        _Res = string.Format(Resources.Descripciones.mailGastoMedioMensualIgualMedia, _MediaGastoMensualAnual);
                    }

                    if (_GastoMesAnterior.GastoTotal < _MediaGastoMensualAnual)
                    {
                        _Res = string.Format(Resources.Descripciones.mailGastoMedioMensualMenorMedia, _MediaGastoMensualAnual, valorDiferencia, valorDivActualMenor);
                    }
                }
            }
            catch (Exception _ex) { }

            return _Res;
        }

        //Total gasto Anual
        private string _TotalGastosAnual(SummaryTotal_Mensual _GastoMesAnterior, string _Usuari)
        {
            string _Res = "";
            var context = new ExpensesEF.Entities();
            decimal _TotalAnualAcumulado, _TotalAnualAcumuladoAnyoAnterior, diferencia, valorDivActualMayor, valorDivActualMenor;

            try
            {
                _TotalAnualAcumulado = context.SummaryTotal_Mensual.Where(x => x.idUser == _Usuari && x.Año == _GastoMesAnterior.Año).Sum(x => x.GastoTotal);
                _Res += string.Format(Resources.Descripciones.mailTotalGastado, _TotalAnualAcumulado);

                _TotalAnualAcumuladoAnyoAnterior = context.SummaryTotal_Mensual.Where(x => x.idUser == _Usuari && x.Año == _GastoMesAnterior.Año - 1).Sum(x => x.GastoTotal);
                diferencia = Math.Abs(_TotalAnualAcumulado - _TotalAnualAcumuladoAnyoAnterior);
            
                if (_TotalAnualAcumuladoAnyoAnterior < _TotalAnualAcumulado)
                {
                    _Res = string.Format(Resources.Descripciones.mailTotalGastadoAnyoAnteriorMenos, _TotalAnualAcumulado, _TotalAnualAcumuladoAnyoAnterior, diferencia);
                }

                if (_TotalAnualAcumuladoAnyoAnterior == _TotalAnualAcumulado)
                { _Res = string.Format(Resources.Descripciones.mailTotalGastadoAnyoAnteriorIgual, _TotalAnualAcumulado);}

                if (_TotalAnualAcumuladoAnyoAnterior > _TotalAnualAcumulado)
                {
                    _Res = string.Format(Resources.Descripciones.mailTotalGastadoAnyoAnteriorMas, _TotalAnualAcumulado, _TotalAnualAcumuladoAnyoAnterior, diferencia);
                }

            }
            catch (Exception _ex)
            {}
            return _Res;

        }
        //lògica de l'estalvi a fi de mes
        private string _AhorroDelMes(SummaryTotal_Mensual _GastoMesAnterior, SummaryTotal_Mensual _Gasto2MesosEnrera, string _Usuari)
        {
            string _Res = "";
            decimal _AhorroMes;
            var context = new ExpensesEF.Entities();
            
            try
            {
                _AhorroMes = context.SummaryTotal_Mensual.Where(x => x.idUser == _Usuari && x.Año == _GastoMesAnterior.Año && x.Mes == _GastoMesAnterior.Mes).FirstOrDefault().AhorroFinDeMes.GetValueOrDefault();
                

                if (_AhorroMes > 0)
                   {
                        _Res = string.Format(Resources.Descripciones.mailAhorroMesMayor, _AhorroMes);

                        if (_Gasto2MesosEnrera.AhorroFinDeMes > 0)
                            {
                                _Res += string.Format(Resources.Descripciones.mailAhorroMesMayorY2MesesMayor, _GetMesLetras(_Gasto2MesosEnrera.Mes),_Gasto2MesosEnrera.AhorroFinDeMes);
                            }
                        else {
                            _Res += string.Format(Resources.Descripciones.mailAhorroMesMayorY2MesesMenorOIgual, _GetMesLetras(_Gasto2MesosEnrera.Mes), Math.Abs(_Gasto2MesosEnrera.AhorroFinDeMes.GetValueOrDefault()));
                            }
                   }
                if (_AhorroMes == 0)
                {
                    _Res = string.Format(Resources.Descripciones.mailAhorroMesIgual);
                }
                if (_AhorroMes < 0)
                {
                    _Res = string.Format(Resources.Descripciones.mailAhorroMesMenor, Math.Abs(_AhorroMes));

                    if (_Gasto2MesosEnrera.AhorroFinDeMes > 0)
                    {
                        _Res += string.Format(Resources.Descripciones.mailAhorroMesMenorY2MesesMayor);
                    }
                    else
                    {
                        _Res += string.Format(Resources.Descripciones.mailAhorroMesMenorY2MesesMenorOIgual);
                    }
                }


            }
            catch (Exception _ex)
            {

            }
            return _Res;
        }
        //Per a cada tipo de gasto, ens diu el que ens hem gastat
        private string _TiposGastos(SummaryTotal_Mensual _GastoMesAnterior, string _Usuari)
        {
            string _Res = "";
            var context = new ExpensesEF.Entities();
            decimal _TotalGastoMensual;
            List<SummaryPorTipoGasto_Mensual> _TiposGastosMensual;

            try
            {
                _TotalGastoMensual = Math.Round(context.SummaryPorTipoGasto_Mensual.Where(x => x.idUser == _Usuari && x.Año == _GastoMesAnterior.Año && x.Mes == _GastoMesAnterior.Mes).Sum(x => x.Valor));
                _TiposGastosMensual = context.SummaryPorTipoGasto_Mensual.Where(x => x.idUser == _Usuari && x.Año == _GastoMesAnterior.Año && x.Mes == _GastoMesAnterior.Mes).ToList();

                //Miramos si ha gastado algo este mes...
                if (_TiposGastosMensual.Count>0)
                {
                    _Res = string.Format(Resources.Descripciones.mailTipoGastoCabecera, _GetMesLetras(_GastoMesAnterior.Mes));
                    _Res += "<table>";
                    foreach (SummaryPorTipoGasto_Mensual TipoGasto_Mensual in _TiposGastosMensual)
                        {
                        _Res += "<tr>";
                        _Res += "<td>";
                        _Res += context.TipoGastoTextosTraduccion.Where(x => x.idTipoGasto == TipoGasto_Mensual.idTipoGasto && x.idIdioma == _idIdioma).FirstOrDefault().Descripcion + ":&nbsp;";
                        _Res += "</td>";
                        _Res += "<td>";
                        _Res += "<b>" + (TipoGasto_Mensual.Valor) + "</b>" + "€&nbsp;";
                        _Res += "</td>";
                        _Res += "<td>";
                        _Res += "(" + Math.Round((TipoGasto_Mensual.Valor / _TotalGastoMensual) * 100) + "%)";
                        _Res += "</td>";
                        _Res += "</tr>";
                    }
                    _Res += "</table>";
                }
            }
            catch (Exception _ex)
            {
            }

            return _Res;

        }

        //mirem els grups de gastos
        private string _GruposGastos(SummaryTotal_Mensual _GastoMesAnterior, string _Usuari)
        {
            string _Res = "";
            string _GrupoGastoNombre="";
            var context = new ExpensesEF.Entities();
            decimal _TotalGastoMensual;
            decimal _ValorResto;
            List<SummaryPorGrupoGasto_Mensual> _GrupoGastosMensual;

            try
            {

                _TotalGastoMensual = (context.SummaryPorTipoGasto_Mensual.Where(x => x.idUser == _Usuari && x.Año == _GastoMesAnterior.Año && x.Mes == _GastoMesAnterior.Mes).Sum(x => x.Valor));
               _GrupoGastosMensual = context.SummaryPorGrupoGasto_Mensual.Where(x => x.idUser == _Usuari && x.Año == _GastoMesAnterior.Año && x.Mes == _GastoMesAnterior.Mes).ToList();

                //Miramos si ha habido algún grupo de gasto este mes...
                if (_GrupoGastosMensual.Count>0)
                {
                    _Res += string.Format(Resources.Descripciones.mailGrupoGastos, _GrupoGastosMensual.Count+1);
                    _ValorResto = _TotalGastoMensual;
                    foreach (SummaryPorGrupoGasto_Mensual GrupoGasto_Mensual in _GrupoGastosMensual)
                    {
                        _GrupoGastoNombre = context.GrupoGastoTextosTraduccion.Where(x => x.idGrupoGasto == GrupoGasto_Mensual.idGrupoGasto && x.idIdioma == _idIdioma).FirstOrDefault().Descripcion + "&nbsp;";
                        _Res += _GrupoGastoNombre + "(<b>" + GrupoGasto_Mensual.Valor + "€</b>), ";
                        _ValorResto -= GrupoGasto_Mensual.Valor;
                    }
                    _Res = _Res.Substring(0, _Res.Length - 2);
                   _Res += string.Format(Resources.Descripciones.mailGrupoGastosResto, _ValorResto);
                }
                else
                {
                    _Res = Resources.Descripciones.mailNoGrupoGasto;
                }
            }
            catch (Exception _ex)
            {
            }

            return _Res;


        }


        private string _CuerpoMesaje(ValuesEnvioEmail _Email)
        {
            string _Res = "";
            var context = new ExpensesEF.Entities();
            var _DataMesAnterior = new Dates();
            var _Data2MesosEnrera = new Dates();
            AspNetUsers _Usuari;
            SummaryTotal_Mensual _SummaryMensual;
            SummaryTotal_Mensual _SummaryMensual2MesosEnrera;
            SummaryRegalo_Mensual _SummaryRegaloMensual;
            decimal _MaxGastoMensual;
            string _Header;

            try
            {
                _DataMesAnterior = _GetMesAny();
                _Data2MesosEnrera = _GetMesAnterior();
                
                _Usuari = context.AspNetUsers.Where(x => x.Id == _Email.idUser).FirstOrDefault();
                _Header = context.ValuesEnvioEmail.Where(x => x.idUser == _Usuari.Id).FirstOrDefault().Header;

                _SummaryMensual = context.SummaryTotal_Mensual.Where(x => x.idUser == _Usuari.Id && x.Mes == _DataMesAnterior.Mes && x.Año == _DataMesAnterior.Any).FirstOrDefault();
                _SummaryMensual2MesosEnrera = context.SummaryTotal_Mensual.Where(x => x.idUser == _Usuari.Id && x.Mes == _Data2MesosEnrera.Mes && x.Año == _Data2MesosEnrera.Any).FirstOrDefault();
                _SummaryRegaloMensual = context.SummaryRegalo_Mensual.Where(x => x.idUser == _Usuari.Id && x.Mes == _DataMesAnterior.Mes && x.Año == _DataMesAnterior.Any).FirstOrDefault();
                _MaxGastoMensual = context.SummaryTotal_Mensual.Where(x => x.idUser == _Usuari.Id && x.Año == _Data2MesosEnrera.Any).Max(x => x.GastoTotal);

                //Primera linea del mail
                _Res = String.Format(Resources.Descripciones.mailLinea1, _Header);

                //Linea on enviem el total de gasto del mes anterior
                if (_Email.TotalGastadoMesAnterior == true )
                {
                    if (_SummaryMensual != null)
                    {
                        

                        _Res += String.Format(Resources.Descripciones.mailTotalGastadoMesAnterior, _GetMesLetras(_SummaryMensual.Mes), _SummaryMensual.GastoTotal);
                        _Res += _MejoraGastoRespecto2MesesAtras(_SummaryMensual, _SummaryMensual2MesosEnrera);
                        _Res += _VerMaximoGastoAnual(_SummaryMensual, _MaxGastoMensual);
                        _Res += _GastoMedioMensual(_SummaryMensual, _Usuari.Id, _SummaryMensual.Mes,_Data2MesosEnrera);
                        _Res += _AhorroDelMes(_SummaryMensual, _SummaryMensual2MesosEnrera, _Usuari.Id);
                        _Res += _TiposGastos(_SummaryMensual, _Usuari.Id);
                        _Res += _GruposGastos(_SummaryMensual, _Usuari.Id);
                        _Res += _TotalGastosAnual(_SummaryMensual, _Usuari.Id);
                    }
                    else
                    {
                        _Res = Resources.Descripciones.mailNoGasto;
                    }

                }
                /*
                //Linea d'estalvi respecte gastos
                if (_Email.AhorroMesAnterior == true && _SummaryMensual != null && _SummaryMensual.AhorroFinDeMes != 0)
                {
                    _Res += String.Format(Resources.Descripciones.mailAhorroMesAnterior, _SummaryMensual.AhorroFinDeMes);
                }

                //Linea de regals el mes anterior
                if (_Email.RegalosMesAnterior == true && _SummaryRegaloMensual != null)
                {
                    _Res += String.Format(Resources.Descripciones.mailRegaloMesAnterior, _SummaryRegaloMensual.Valor);
                }
                */
                _Res += Resources.Descripciones.mailBottom;

            }
            catch (Exception _ex)
            {

            }
            return _Res;
        }

        private System.Net.Mail.MailMessage _GenerarEmail(ValuesEnvioEmail _Email)
        {
            var context = new ExpensesEF.Entities();

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.To.Add(context.AspNetUsers.Where(x => x.Id == _Email.idUser).FirstOrDefault().Email.ToString());
            //msg.To.Add("oscar.redondo@gmail.com");
            msg.From = new MailAddress("info@expenses.com", "expenses", System.Text.Encoding.UTF8);
            msg.Subject = Resources.Descripciones.mailSubject;
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Body = _CuerpoMesaje(_Email);
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true;

            return msg;
        }

        private void _EnviarEmail(System.Net.Mail.MailMessage msg)
        {
            //Aquí es donde se hace lo especial
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("oscar.redondo", "rkwsbapsruxhrass");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true; //Esto es para que vaya a través de SSL que es obligatorio con GMail
            try
            {
                client.Send(msg);
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                //Console.WriteLine(ex.Message);
                //Console.ReadLine();
            }
        }

        //Fixem l'idioma de l'email
        private void SetCultura(int _idId)
        {
            var context = new ExpensesEF.Entities();
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo(context.Idiomas.Where(x => x.idIdioma == _idId).FirstOrDefault().culture);
            _idIdioma = _idId;
        }

        public void Send()
        {
            try
            {
                //Per a cada usuari...
                var context = new ExpensesEF.Entities();
                System.Net.Mail.MailMessage _email;
                List<ValuesEnvioEmail> _EmailsAEnviar;

                //Busquem usuaris que vulguin rebre l'email...
                _EmailsAEnviar = context.ValuesEnvioEmail.Where(x => x.EnvioMail == true).ToList();


                foreach (ValuesEnvioEmail _Email in _EmailsAEnviar)
                {
                    SetCultura(_Email.idIdiomaEMail.GetValueOrDefault());
                    _email = _GenerarEmail(_Email);
                    _EnviarEmail(_email);
                }
            }
            catch (Exception _ex)
            {

            }
        }
    }
}