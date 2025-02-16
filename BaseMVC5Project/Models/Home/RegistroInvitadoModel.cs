using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BaseMVC5Project.Models.Utils;
using CommonTools.Pdf;
using MigracionTalentoExtranjero.Models.Enum;
using MigracionTalentoExtranjero.Models.Utils;
using Org.BouncyCastle.Crypto.Operators;

namespace MigracionTalentoExtranjero.Models.Home
{
    public class RegistroInvitadoModel
    {
        private ComboBoxHelper CB = new ComboBoxHelper();

        #region Campos y Atributos
        // PASAPORTE
        public int? Id { get; set; }
        public string CodigoSecreto { get; set; }
        public int IdStatusActual { get; set; }


        public string NumeroPasaporte { get; set; }
        public string DiaExpPas { get; set; }
        public string MesExpPas { get; set; }
        public string AnioExpPas { get; set; }
        public string DiaVenPas { get; set; }
        public string MesVenPas { get; set; }
        public string AnioVenPas { get; set; }
        public string Nacionalidad { get; set; }

        // DATOS DEL EXTRANJERO VISITANTE
        public string Empresa { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Sexo { get; set; }
        public string Correo { get; set; }
        public string ConfirmacionCorreo { get; set; }

        // PAIS DE NACIMIENTO
        public string PaisNacimiento { get; set; }

        //INFORMACION ADICIONAL DEL EXTRANJERO QUE NOS VISITA
        public string ActividadPaisResidencia { get; set; }
        public string FueExpulsadoDeMexico { get; set; }
        public string AntecedentesPenalesEnMexico { get; set; }
        public string ExplicacionAntecedentesExpulsion { get; set; }
        public string ActividadEnMexico { get; set; }
        public string DiaEntrada{ get; set; }
        public string MesEntrada { get; set; }
        public string AnioEntrada { get; set; }
        public string DiaSalida { get; set; }
        public string MesSalida { get; set; }
        public string AnioSalida { get; set; }

        public List<InfoEventoModel> Eventos { get; set; } = new List<InfoEventoModel>() { new InfoEventoModel()};

        // INFORMACION DEL VUELO DE LLEGADA
        public string AeropuertoLlegada { get; set; }
        public string NuevoAeropuerto { get; set; }
        public string Aerolinea { get; set; }
        public string NuevaAerolinea { get; set; }
        public string NumeroVuelo { get; set; }

        // MODAL CONFIRMACION
        public bool AceptaPoliticaPrivacidad { get; set; }
        
        
        public bool CHECK_VERIFY { get; set; }



        public string PaisNacimientoDesc { get; set; }
        public string NuevaEmpresa { get; set; }


        public List<List<SelectListItem>> SelectListEvents { get; set; }
        public List<List<SelectListItem>> SelectListInmuebles { get; set; }
        public List<List<SelectListItem>> SelectListDiaInicioEvento { get; set; }
        public List<List<SelectListItem>> SelectListMesInicioEvento { get; set; }
        public List<List<SelectListItem>> SelectListAnioInicioEvento { get; set; }
        public List<List<SelectListItem>> SelectListDiaFinEvento { get; set; }
        public List<List<SelectListItem>> SelectListMesFinEvento { get; set; }
        public List<List<SelectListItem>> SelectListAnioFinEvento { get; set; }
        public List<List<SelectListItem>> SelectListNacionalidad { get; set; }

        public List<SelectListItem> SetSelectedItem(string value, List<SelectListItem> items)
        {
            return items.Select(s => new SelectListItem
            {
                Value = s.Value,
                Text = s.Text,
                Selected = s.Value == value
            }).ToList();
        }




        #region Labels
        public string IdiomaActual { get; set; } = "ES";
        public string LabelNumPasaporte {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Número de Pasaporte";
                else
                    return "Passport Number";
            }
            set {  }
        }

        public string LabelFechaExpedicion
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Fecha de Expedición";
                else
                    return "Date of issue";
            }
            set { }
        }

        public string LabelFechaDia
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Día";
                else
                    return "Day";
            }
            set { }
        }

        public string LabelFechaMes
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Mes";
                else
                    return "Month";
            }
            set { }
        }

        public string LabelFechaAnio
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Año";
                else
                    return "Year";
            }
            set { }
        }


        public string LabelFechaVencimiento
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Fecha de Vencimiento";
                else
                    return "Due Date";
            }
            set { }
        }
        public string LabelNacionalidad
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Nacionalidad";
                else
                    return "Nationality";
            }
            set { }
        }
        public string LabelEmpresa
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Empresa";
                else
                    return "Company";
            }
            set { }
        }

         public string LabelNombrePasaporte
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Nombre como en el Pasaporte";
                else
                    return "Name in Passport";
            }
            set { }
        }
         public string LabelNombre
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Nombre(s)";
                else
                    return "Name";
            }
            set { }
        }

        public string LabelApellidos
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Apellidos";
                else
                    return "Last Name";
            }
            set { }
        }

        public string LabelSexo
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Sexo";
                else
                    return "Gender";
            }
            set { }
        }

        public string LabelCorreoElectronico
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Correo Electrónico";
                else
                    return "Email";
            }
            set { }
        }
          public string LabelConfirmaCorreoElectronico
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Confirme Correo Electrónico";
                else
                    return "Confirm Email";
            }
            set { }
        }

        public string LabelPaisNacimiento
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "País de Nacimiento";
                else
                    return "Country of Birth";
            }
            set { }
        }

           public string LabelActividadPaisResidencia
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Actividad en su País de Residencia";
                else
                    return "Activity in your Country of Residence";
            }
            set { }
        }
           public string LabelExpulsadoDeMexico
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "¿Has sido expulsado de México?";
                else
                    return "Have you been expelled from México?";
            }
            set { }
        }
        public string LabelAntecedentesPenalesMexico
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "¿Tiene antecedentes penales en México?";
                else
                    return "Do you have a criminal record in México?";
            }
            set { }
        }        
        public string LabelExplicacionAntecedentesExpulsion
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Explicación";
                else
                    return "Explanation";
            }
            set { }
        }
        public string LabelActividadEnMexico
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Actividad a realizar en México";
                else
                    return "Activity you will to do in México";
            }
            set { }
        }
        public string LabelNombreEvento
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Nombre del Evento";
                else
                    return "Name of the Event";
            }
            set { }
        }

          public string LabelInmueble
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Inmueble del Evento";
                else
                    return "Event Property";
            }
            set { }
        }

          public string LabelLocation
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Ubicacion del Inmueble";
                else
                    return "Event Venue";
            }
            set { }
        }

             public string LabelFechaInicioEvento
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Fecha de Inicio del Evento";
                else
                    return "Event Start Date";
            }
            set { }
        }

               public string LabelFechaFinEvento
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Fecha de Fin del Evento";
                else
                    return "Event End Date";
            }
            set { }
        }

        public string LabelAgregarOtroEvento
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Agregar Otro Evento";
                else
                    return "Add Event";
            }
            set { }
        }

         public string LabelFechaLlegada
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Fecha de Llegada";
                else
                    return "Arrive Date";
            }
            set { }
        }

        
         public string LabelFechaSalida
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Fecha de Salida";
                else
                    return "Departure Date";
            }
            set { }
        }

           public string LabelMensajeVuelo
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Solo si cuenta con la información, por favor proporcione su fecha de llegada y salida de México";
                else
                    return "Only if you have the information, please provide your date of arrival and departure from México";
            }
            set { }
        }
           public string LabelAeropuertoLlegada
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Aeropuerto de Llegada";
                else
                    return "Arrival Airport";
            }
            set { }
        }
        
           public string LabelVuelo
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Vuelo";
                else
                    return "Flight";
            }
            set { }
        }
           public string LabelAerolinea
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Aerolinea";
                else
                    return "Airline";
            }
            set { }
        }
         public string LabelNumeroVuelo
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Número";
                else
                    return "Number";
            }
            set { }
        }

        
           public string LabelEnviarBtn
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Enviar";
                else
                    return "Send";
            }
            set { }
        }

           public string LabelTituloRegistro
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Formulario de Registro";
                else
                    return "Registration Form";
            }
            set { }
        }
        
           public string LabelSeccionPasaporte
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "PASAPORTE";
                else
                    return "PASSPORT";
            }
            set { }
        }

        
           public string LabelSeccionDatosExtranjeroVisitante
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "DATOS DEL EXTRANJERO VISITANTE";
                else
                    return "VISITING FOREIGNER INFORMATION";
            }
            set { }
        }

        
           public string LabelSeccionPaisNacimiento
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "PAÍS DE NACIMIENTO";
                else
                    return "COUNTRY OF BIRTH";
            }
            set { }
        }


        
           public string LabelSeccionInformacionAdicional
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "INFORMACIÓN ADICIONAL DEL EXTRANJERO QUE NOS VISITA";
                else
                    return "ADDITIONAL INFORMATION ABOUT FOREIGN VISITORS";
            }
            set { }
        }

        
        
           public string LabelSeccionInformacionIngresoAlPais
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "INFORMACIÓN DE INGRESO AL PAÍS";
                else
                    return "COUNTRY ENTRY INFORMATION";
            }
            set { }
        }

        
           public string LabelSeccionAvisoPrivacidad
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Operadora de Centros de Espectáculos, S.A. de C.V. (en adelante OCESA) y/o sus empresas filiares y subsidiarias con domicilio fiscal en Avenida Rio Churubusco y Añil S/N, Colonia Granjas México, Alcaldía Iztacalco, C.P. 08400, Ciudad de México, es responsable del tratamiento de los datos personales recabados en este formato tales como nombre completo, domicilio, teléfono, correo electrónico e información contenida en la identificación oficial presentada, dicha información única y exclusivamente será utilizada por OCESA y/o sus empresas filiales y subsidiarias, para la aceptación de los términos y condiciones establecidas en este párrafo, así como para identificarlo como participante acreditado, la información se conservara por un periodo indeterminado, lo que resulte de los siguientes periodos (i) el tiempo que sea necesario para la actividad (ii) cualquier periodo de retención requerido por la ley, (iii) el final del periodo en el cual pueden surgir litigios o investigaciones con respecto a los servicios. El texto completo del aviso de privacidad integral se pondrá a su disposición cuando así se requiera por escrito el cual debe presentarse en el domicilio indicado en el presente párrafo o enviado al correo electrónico talentoextranjero@ocesa.mx";
                else
                    return "Operadora de Centros de Espectáculos, S.A. de C.V. (hereinafter OCESA) and/or its affiliates and subsidiaries with fiscal domicile at Avenida Rio Churubusco y Añil S/N, Colonia Granjas México, Alcaldía Iztacalco, C.P. 08400, Mexico City, is responsible for the treatment of personal data collected in this form such as full name, address, telephone, e-mail and information contained in the official identification submitted, such information will be used exclusively by OCESA. 08400, Mexico City, is responsible for the treatment of personal data collected in this form such as full name, address, telephone, email and information contained in the official identification presented, such information will only and exclusively be used by OCESA and/or its affiliates and subsidiaries, for the acceptance of the terms and conditions set forth in this paragraph, as well as to identify you as an accredited participant, the information will be kept for an undetermined period, resulting from the following periods (i) the time necessary for the activity (ii) any retention period required by law, (iii) the end of the period in which litigation or investigations may arise with respect to the services. The full text of the comprehensive privacy notice will be made available to you upon request in writing which must be submitted to the address indicated in this paragraph or sent to the e-mail address: talentoextranjero@ocesa.mx";
            }
            set { }
        }
        
           public string LabelCheckAvisoPrivacidad
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Acepto la política de privacidad";
                else
                    return "I accept the privacy policy";
            }
            set { }
        }
        
        public string LabelBotonAceptaAvisoPrivacidad
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Aceptar";
                else
                    return "Accept";
            }
            set { }
        }
    public string LabelBotonCancelaAvisoPrivacidad
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Cancelar";
                else
                    return "Cancel";
            }
            set { }
        }


    public string LabelTituloAvisoPrivacidad
        {
            get
            {
                if (IdiomaActual.Equals("ES"))
                    return "Importante";
                else
                    return "Important";
            }
            set { }
        }




        #endregion

        #endregion


        #region Metodos

        public static List<string> CatSiNo()
        {
            List<string> result = new List<string>
            {
                "SI",
                "NO"
            };
            return result;
        }
        public static List<string> CatYesNo()
        {
            List<string> result = new List<string>
            {
                "YES",
                "NO"
            };
            return result;
        }
        public static List<string> CatDias()
        {
            List<string> result = new List<string>
            {
                "01",
                "02",
                "03",
                "04",
                "05",
                "06",
                "07",
                "08",
                "09",
                "10",
                "11",
                "12",
                "13",
                "14",
                "15",
                "16",
                "17",
                "18",
                "19",
                "20",
                "21",
                "22",
                "23",
                "24",
                "25",
                "26",
                "27",
                "28",
                "29",
                "30",
                "31"
            };
            return result;
        }
        public static List<string> CatMes()
        {
            List<string> result = new List<string>
            {
                "01",
                "02",
                "03",
                "04",
                "05",
                "06",
                "07",
                "08",
                "09",
                "10",
                "11",
                "12"
            };
            return result;
        }

        public static List<string> CatAnios()
        {
            List<string> result = new List<string>();
            int inicial = DateTime.Now.Year - 15;
            int final = DateTime.Now.Year + 10;

            for (int index =inicial; index< final; index++)
            {
                result.Add(index.ToString());
            }

            return result;
        }


        public static List<string> Cat10AniosFuturo()
        {
            List<string> result = new List<string>();
            int inicial = DateTime.Now.Year;
            int final = DateTime.Now.Year + 10;

            for (int index = inicial; index <= final; index++)
            {
                result.Add(index.ToString());
            }

            return result;
        }

        
        public static List<string> Cat10AniosPasado()
        {
            List<string> result = new List<string>();
            int inicial = DateTime.Now.Year - 10;
            int final = DateTime.Now.Year;

            for (int index = inicial; index <= final; index++)
            {
                result.Add(index.ToString());
            }

            return result;
        }


        public static List<string> CatAeropuerto()
        {
            //HttpManager httpManager = new HttpManager(HttpHostEnum.HOST.GetString());
            //object objectTemp = new object();

            List<string> result = new List<string>();
            int inicial = 1950;
            int final = DateTime.Now.Year + 10;
            result.Add("");

            for (int index =inicial; index< final; index++)
            {
                result.Add(index.ToString());
            }

            return result;
        }


        internal string ValidateLanguage(string language)
        {
            string result = "";

            if(language.ToUpper().Equals(LanguageEnum.EN.GetString()))
            {
                result = LanguageEnum.EN.GetString();
            }
            else
                result = LanguageEnum.ES.GetString();

            return result;
        }

        internal string ValidaCamposRequeridos()
        {
            string result = "";
            if (string.IsNullOrEmpty(NumeroPasaporte))
            {
                result += "\nNúmero de Pasaporte, ";
            }
            if (string.IsNullOrEmpty(DiaExpPas))
            {
                result += "\nFecha de Expedición de Pasaporte (Día), ";
            }
            if (string.IsNullOrEmpty(MesExpPas))
            {
                result += "\nFecha de Expedición de Pasaporte (Mes), ";
            }
            if (string.IsNullOrEmpty(AnioExpPas))
            {
                result += "\nFecha de Expedición de Pasaporte (Año), ";
            }
            if (string.IsNullOrEmpty(DiaVenPas))
            {
                result += "\nFecha de Vencimiento de Pasaporte (Día), ";
            }
            if (string.IsNullOrEmpty(MesVenPas))
            {
                result += "\nFecha de Vencimiento de Pasaporte (Mes), ";
            }
            if (string.IsNullOrEmpty(AnioVenPas))
            {
                result += "\nFecha de Vencimiento de Pasaporte (Año), ";
            }
            if (string.IsNullOrEmpty(Nacionalidad))
            {
                result += "\nNacionalidad (En Pasaporte), ";
            }
            if (string.IsNullOrEmpty(Empresa))
            {
                result += "\nEmpresa, ";
            }
            if (string.IsNullOrEmpty(Nombre))
            {
                result += "\nNombre, ";
            }
            if (string.IsNullOrEmpty(Apellidos))
            {
                result += "\nApellidos, ";
            }
            //if (string.IsNullOrEmpty(Sexo))
            //{
            //    result += "\nSexo, ";
            //}
            if (string.IsNullOrEmpty(Correo))
            {
                result += "\nCorreo, ";
            }
            if (string.IsNullOrEmpty(ConfirmacionCorreo))
            {
                result += "\nConfirmación de Correo, ";
            }
            if (string.IsNullOrEmpty(PaisNacimiento))
            {
                result += "\nPaís de Nacimiento, ";
            }

            if (string.IsNullOrEmpty(ActividadEnMexico))
            {
                result += "\nActividad en el GP de México, ";
            }
            if (Eventos != null && Eventos.Count > 0)
            {
                if (string.IsNullOrEmpty(Eventos[0].NombreEvento))
                {
                    result += "\nNombre del Evento, ";
                }
                if (string.IsNullOrEmpty(Eventos[0].DiaInicioEvento))
                {
                    result += "\nFecha de Inicio del Evento (Día), ";
                }
                if (string.IsNullOrEmpty(Eventos[0].MesInicioEvento))
                {
                    result += "\nFecha de Inicio del Evento (Mes), ";
                }
                if (string.IsNullOrEmpty(Eventos[0].AnioInicioEvento))
                {
                    result += "\nFecha de Inicio del Evento (Año), ";
                }
                if (string.IsNullOrEmpty(Eventos[0].InmuebleEvento))
                {
                    result += "\nInmueble del Evento, ";
                }
                if (string.IsNullOrEmpty(Eventos[0].UbicacionInmueble))
                {
                    result += "\nUbicacion del Inmueble, ";
                }
            }

            return result;
        }
        #endregion


    }
}