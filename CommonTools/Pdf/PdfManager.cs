using System;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Text;
using System.Collections.Generic;
using CommonTools.Pdf;
using System.Reflection;
using CommonTools.DTOs;
using CommonTools.DTOs.Register;
using System.Runtime.InteropServices.ComTypes;
using CommonTools.DTOs.Query;
using SkiaSharp;
using System.Globalization;

namespace CommonTools.Pdf
{
    public class PdfManager
    {
        private const float _1_CM_EN_PUNTO = 28.34f;
        private const float MARGEN_SUPERIOR_OCESA_PRESENTA = 7.937008f;
        private const float MARGEN_IZQUIERDO_OCESA_PRESENTA = 17.0079f;
        private const float MARGEN_DERECHO_OCESA_PRESENTA = 17.0079f;
        private const float MARGEN_INFERIOR_OCESA_PRESENTA = 13.88976f;
        private const float ALTO_IMAGEN_OCESA = 50.45669f;
        private const float ANCHO_IMAGEN_OCESA = 66.61417f;

        private const float ALTO_IMAGEN_FIRMA = 60.45669f;
        private const float ANCHO_IMAGEN_FIRMA = 86.61417f;


        private int MaxcountLines = 0;


        private BaseFont Courier = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, true);
        private Font FuenteCourier8 = new Font(BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, true), 8f);
        private Font FuenteArial8 = new Font(FontFactory.GetFont("Arial MT", 8, Font.NORMAL));
        private Font FuenteArial8Roja = new Font(FontFactory.GetFont("Arial MT", 8, Font.NORMAL, BaseColor.Red));
        private Font FuenteArial8Negrita = new Font(FontFactory.GetFont("Arial MT", 8, Font.BOLD));
        private Font FuenteArial10 = new Font(FontFactory.GetFont("Arial MT", 11, Font.NORMAL));
        private Font FuenteArial11 = new Font(FontFactory.GetFont("Arial MT", 11, Font.NORMAL));
        private Font FuenteArial11Roja = new Font(FontFactory.GetFont("Arial MT", 11, Font.NORMAL, BaseColor.Red));
        private Font FuenteArial11RojaNegrita = new Font(FontFactory.GetFont("Arial MT", 11, Font.BOLD, BaseColor.Red));
        private Font FuenteArial11Negrita = new Font(FontFactory.GetFont("Arial MT", 11, Font.BOLD));


        Document doc;
        MemoryStream bufferDoc;
        PdfWriter writer;

        private string pathAssets = "Assets";

        public PdfManager()
        {
        }

        #region Carta Invitacion
        //public AttachmentFileDto GeneraDocumento(string codigo, string tipoReporte, IReporteInfo reporteInfo)
        //{
        //    AttachmentFileDto result = new AttachmentFileDto();
        //    result.FileName = $"{reporteInfo.TipoArchivoGenerado}.pdf";
        //    //string attachment = $"attachment; filename={nombreArchivo}\"{DateTime.Now.ToString()}.pdf";



        //    //Creacion del documento
        //    doc = new Document();
        //    //Configuraciones de estructura del documento
        //    doc.SetPageSize(PageSize.Letter);
        //    //28.34f son los puntos que equivalen a un cm
        //    doc.SetMargins(MARGEN_IZQUIERDO_OCESA_PRESENTA, MARGEN_DERECHO_OCESA_PRESENTA, MARGEN_SUPERIOR_OCESA_PRESENTA, MARGEN_IZQUIERDO_OCESA_PRESENTA);


        //    // Indicamos donde vamos a guardar el documento
        //    bufferDoc = new MemoryStream();
        //    writer = PdfWriter.GetInstance(doc, bufferDoc);
        //    // Le colocamos el título y el autor
        //    // **Nota: Esto no será visible en el documento
        //    doc.AddTitle("OCESA");
        //    doc.AddCreator("OCESA");
        //    doc.AddAuthor("OCESA");

        //    doc.Open();
        //    GeneraDocumentoOCESA(reporteInfo);

        //    doc.Close();

        //    result.File = bufferDoc.ToArray();
        //    return result;

        //}
        //public static Stream GetImage(string imagen)
        //{
        //    var assembly = typeof(CommonTools.Pdf.PdfManager).GetTypeInfo().Assembly;
        //    Stream stream = assembly.GetManifestResourceStream($"CommonTools.Assets.{imagen}");
        //    return stream;
        //}
        //private void GeneraDocumentoOCESA(IReporteInfo reporteInfo)
        //{
        //    BaseFont Courier = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, true);
        //    Font FuenteCourier8 = new Font(Courier, 8f);

        //    FontFactory.RegisterDirectories();
        //    Font FuenteArial8 = new Font(FontFactory.GetFont("Arial MT", 8, Font.NORMAL));

        //    FontFactory.RegisterDirectories();
        //    Font FuenteArial10 = new Font(FontFactory.GetFont("Arial MT", 11, Font.NORMAL));

        //    FontFactory.RegisterDirectories();
        //    Font FuenteArial11 = new Font(FontFactory.GetFont("Arial MT", 11, Font.NORMAL));

        //    FontFactory.RegisterDirectories();
        //    Font FuenteArial11Negrita = new Font(FontFactory.GetFont("Arial MT", 11, Font.BOLD));


        //    Image logo = Image.GetInstance(GetImage($"logo_ocesa.png"));
        //    logo.ScaleAbsoluteHeight(ALTO_IMAGEN_OCESA);
        //    logo.ScaleAbsoluteWidth(ANCHO_IMAGEN_OCESA);

        //    Image logoOcesaPresenta = Image.GetInstance(GetImage($"ocesa_presenta.png"));
        //    logoOcesaPresenta.ScaleAbsoluteHeight(ALTO_IMAGEN_OCESA);
        //    logoOcesaPresenta.ScaleAbsoluteWidth(ANCHO_IMAGEN_OCESA);

        //    // ENCABEZADO
        //    var encabezado = new Paragraph() { Alignment = Element.ALIGN_CENTER, Font = FuenteArial11Negrita };
        //    encabezado.Add(new Chunk(logo, -100, -35));
        //    //encabezado.Add(new Chunk("CARTA INVITACIÓN/INVITATION LETTER"));
        //    encabezado.Add(new Chunk("CARTA INVITACIÓN/INVITATION LETTER"));
        //    encabezado.Add(new Chunk(logoOcesaPresenta, 100, -35));

        //    //Se asigna las columnas
        //    var generalTable = new PdfPTable(new float[] { 50f, 50f });
        //    generalTable.WidthPercentage = 100;

        //    //Phrase test = new Phrase("texto",FuenteArial8) ;


        //    StringBuilder celdaEspanol = GeneraContenidoCeldaEspanOCESAPresenta(reporteInfo);
        //    StringBuilder celdaIngles = GeneraContenidoCeldaEngOCESAPresenta(reporteInfo);

        //    //var lengthTextEsp = celdaEspanol.Length;
        //    //var lengthTextIng = celdaIngles.Length;

        //    //if(lengthTextEsp < lengthTextIng)
        //    //{
        //    //    var diferencia = lengthTextIng - lengthTextEsp;
        //    //    string caracteresParaAgregar = "";
        //    //    int caracterActual = 0;
        //    //    while (caracterActual < diferencia)
        //    //    {
        //    //        caracteresParaAgregar += "  ";
        //    //        caracterActual++;
        //    //    }
        //    //    caracteresParaAgregar += ".";
        //    //    celdaEspanol.Append(caracteresParaAgregar);
        //    //}
        //    //else
        //    //{
        //    //    var diferencia = lengthTextEsp - lengthTextIng;
        //    //    string caracteresParaAgregar = "";
        //    //    int caracterActual = 0;
        //    //    while (caracterActual < diferencia)
        //    //    {
        //    //        caracteresParaAgregar += "  ";
        //    //        caracterActual++;
        //    //    }
        //    //    caracteresParaAgregar += ".";
        //    //    celdaIngles.Append(caracteresParaAgregar);

        //    //}





        //    celdaEspanol.AppendLine("\n");
        //    celdaIngles.AppendLine("\n");


        //    //var celdaEspanText = new Paragraph(celdaEspanol.ToString(), FuenteArial8) { Alignment = Element.ALIGN_JUSTIFIED, Leading=10f };

        //    //MultiColumnText columns= new MultiColumnText(600f);
        //    //columns.AddRegularColumns(MARGEN_IZQUIERDO_OCESA_PRESENTA, doc.PageSize.Width-MARGEN_DERECHO_OCESA_PRESENTA, 10f, 2);
        //    //columns.AddElement(celdaEspanText);


        //    //columns.AddElement(celdaEspanText);
        //    var phraseEsp = new Phrase(celdaEspanol.ToString(), FuenteArial8);
        //    var cellEsp = new PdfPCell(phraseEsp);
        //    cellEsp.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
        //    cellEsp.Padding = 10;
        //    cellEsp.BorderColorBottom = BaseColor.White;

        //    var phraseIng = new Phrase(celdaIngles.ToString(), FuenteArial8);
        //    var cellIng = new PdfPCell(phraseIng);
        //    cellIng.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
        //    cellIng.Padding = 10;
        //    cellIng.BorderColorBottom = BaseColor.White;

        //    //Agregar firmas 
        //    string seccionFirma = "\n\n\n\n" + GeneraSeccionFirma().ToString() + "\n";
        //    var phraseFirmaEsp = new Phrase(seccionFirma, FuenteArial11);
        //    var cellFirmaEsp = new PdfPCell(phraseFirmaEsp);
        //    cellFirmaEsp.PaddingTop = 0f;
        //    cellFirmaEsp.BorderColorTop = BaseColor.White;

        //    var phraseFirmaIng = new Phrase(seccionFirma, FuenteArial11);
        //    var cellFirmaIng = new PdfPCell(phraseFirmaIng);
        //    cellFirmaIng.BorderColorTop = BaseColor.White;



        //    generalTable.AddCell(cellEsp);
        //    generalTable.AddCell(cellIng);
        //    generalTable.AddCell(cellFirmaEsp);
        //    generalTable.AddCell(cellFirmaIng);

        //    //PdfContentByte columna1 = new PdfContentByte(writer);

        //    //PdfContentByte cb = writer.DirectContent;
        //    //ColumnText ct = new ColumnText(writer.DirectContent);
        //    //float columnWidth = 260f;
        //    //float[] left1 = { MARGEN_IZQUIERDO_OCESA_PRESENTA, doc.Top - 80f, MARGEN_IZQUIERDO_OCESA_PRESENTA, doc.Bottom - 80 };
        //    //float[] right1 = { MARGEN_IZQUIERDO_OCESA_PRESENTA + columnWidth, doc.Top - 80f, MARGEN_IZQUIERDO_OCESA_PRESENTA + columnWidth, doc.Bottom - 80 };
        //    //float[] left2 = { doc.Right - columnWidth, doc.Top - 80f, doc.Right - columnWidth, doc.Bottom };
        //    //float[] right2 = { doc.Right, doc.Top - 80f, doc.Right, doc.Bottom };


        //    //ct.Leading = -11f;
        //    //ct.SetLeading(9f,1f);
        //    //ct.Alignment = Element.ALIGN_JUSTIFIED;


        //    //var phraseCelda1 = new Phrase(celdaEspanol.ToString(), FuenteArial8);
        //    //phraseCelda1.Leading = 9f;

        //    // Add content for left column.
        //    //ct.SetColumns(left1, right1);
        //    //ct.AddText(new Phrase( "texto1\n", FuenteArial8));
        //    //ct.AddText(new Phrase( "texto2", FuenteArial8));
        //    //ct.AddText(phraseCelda1);
        //    //ct.Go();

        //    //// Add content for right column.
        //    //ct.SetColumns(left2, right2);
        //    //ct.AddText(new Paragraph(celdaEspanol.ToString(), FuenteArial8));
        //    //ct.Go();

        //    var footer = new Paragraph(GeneraFooter().ToString(), FuenteArial11) { Alignment = Element.ALIGN_CENTER };


        //    doc.Add(encabezado);
        //    doc.Add(new Paragraph("\n\n\n\n"));
        //    //doc.Add(columns);
        //    doc.Add(generalTable);
        //    doc.Add(footer);



        //}

        //private StringBuilder GeneraContenidoCeldaEspanOCESAPresenta(IReporteInfo infoEventosList)
        //{
        //    //Parrafo 1
        //    string NombreInvitado = infoEventosList.NombreInvitado;//"MIGUEL ANGEL VILLEGAS";
        //    string Nacionalidad = infoEventosList.Nacionalidad;// "USA";
        //    string NumPasaporte = infoEventosList.NumPasaporte;// "213123x122342";

        //    //Punto 6
        //    string puestoParteStaff = "STAFF";


        //    //infoEventosList.Add(new InfoEventoModel()
        //    //{
        //    //    FechaEvento = "2023/01/01",
        //    //    InmuebleEvento = "Foro Sol",
        //    //    NombreEvento = "Vive Latino",
        //    //    UbicacionInmueble = "Av. Canal de tezontle"
        //    //});

        //    //infoEventosList.Add(new InfoEventoModel()
        //    //{
        //    //    FechaEvento = "2023/01/11",
        //    //    InmuebleEvento = "Arena Mty",
        //    //    NombreEvento = "Pal norte",
        //    //    UbicacionInmueble = "Av. Mty"
        //    //});

        //    //Punto 7
        //    string fechaEntradaAlPais = infoEventosList.FechaEntradaAlPais;// "2023/03/28";
        //    string fechaSalidaAlPais = infoEventosList.FechaSalidaAlPais;// "2023/04/28";

        //    StringBuilder result = new StringBuilder();

        //    result.AppendLine(NombreInvitado);
        //    result.AppendLine("");
        //    string parrafo1 = $"De conformidad con el artículo 26 de los “Lineamientos para Trámites y Procedimientos Migratorios” y Trámite 1 de los “Lineamientos generales para la expedición de visas” que emiten las Secretarías de Gobernación y de Relaciones Exteriores se extiende la presente CARTA INVITACION a favor de {NombreInvitado}, de la nacionalidad de {Nacionalidad}, con número de pasaporte {NumPasaporte}, en los siguientes términos:";
        //    result.AppendLine(parrafo1);
        //    result.AppendLine("");
        //    string punto1 = "1.- NOMBRE COMPLETO DEL APODERADO LEGAL Y NACIONALIDAD:\nLic. Alfonso David Aragon Buendia, apoderado legal de OCESA PRESENTA, S.A. de C.V. de nacionalidad mexicana.";
        //    result.AppendLine(punto1);
        //    result.AppendLine("\n");
        //    string punto2 = "2.- DENOMINACION O RAZON SOCIAL DE LA ORGANIZACIÓN: OCESA PRESENTA, S.A. de C.V. (en adelante “OCESA”)";
        //    result.Append(punto2);
        //    result.AppendLine("\n");
        //    string punto3 = "3.-NUMERO DE REGISTRO Y OBJETO DE LA ORGANIZACIÓN: OCESA PRESENTA, S.A. de C.V. se constituyó ante el Notario Público de la Ciudad de México Lic. Ponciano López Juárez mediante la Escritura Pública 97,765 de fecha 18 de noviembre de 2010, siendo el objeto de la sociedad entre otros la contratación, promoción y puesta en escena de todo tipo de espectáculos musicales, artísticos, cinematográficos, teatrales, deportivos y comerciales, así como la contratación de artistas, músicos, grupos musicales, y corales de danza y deportistas.";
        //    result.Append(punto3);
        //    result.AppendLine("\n");
        //    string punto4 = "4.- NUMERO DE CONSTANCIA DE INSCRIPCIÓN Y FECHA DE REGISTRO ANTE EL INSTITUTO NACIONAL DE MIGRACIÓN: Número 1002695236.";
        //    result.Append(punto4);
        //    result.AppendLine("\n");
        //    string punto5 = "5.- DOMICILIO COMPLETO Y DATOS DE CONTACTO DE LA ORGANIZACIÓN:\nLa empresa tiene su domicilio ubicado en calle Independencia No. 90, Colonia Centro (área 5), Alcaldía en Cuauhtémoc, C.P. 06050, Ciudad de México, teléfono: (55) 26296900. Nombre del Contacto: Lic. Alfonso David Aragon Buendia.";
        //    result.Append(punto5);
        //    result.AppendLine("\n");
        //    string punto6 = $"6.-INFORMACION SOBRE LA ACTIVIDAD QUE REALIZARA LA PERSONA EXTRANJERA INVITADA:\nParticipar como {puestoParteStaff} en el(os) evento(s) denominado(s):";
        //    int numEventos = infoEventosList.InfoEventosList.Count;
        //    int eventosRecorridos = 0;
        //    foreach (IInfoEvento evento in infoEventosList.InfoEventosList)
        //    {
        //        eventosRecorridos++;
        //        punto6 += $" “{evento.NombreEvento}” que se llevará acabo el día {evento.FechaEvento} en el “{evento.InmuebleEvento}” en la {evento.UbicacionInmueble}";
        //        if (eventosRecorridos < numEventos)
        //            punto6 += $",";
        //        else
        //            punto6 += $".";
        //    }
        //    result.Append(punto6);
        //    result.AppendLine("\n");
        //    string punto7 = $"7.- FECHA DE ENTRADA Y SALIDA DEL INVITADO AL PAÍS:\nEntrada el día {fechaEntradaAlPais}.\nSalida el día {fechaSalidaAlPais}.";
        //    result.Append(punto7);
        //    result.AppendLine("\n");
        //    string punto8 = $"8.- SE ADJUNTA COPIA DE LA IDENTIFICACION OFICIAL DE LA\r\nPERSONA QUE SUSCRIBE LA CARTA INVITACION. Es importante señalar que {NombreInvitado} que se mencionan en es invitado por OCESA para participar en el evento antes señalado bajo la condición de estancia de VISITANTE SIN PERMISO PARA REALIZAR ACTIVIDADES REMUNERADAS, toda vez los honorarios, sueldos y gastos de estos extranjeros son pagados íntegramente fuera del territorio nacional por empresas distintas a OCESA. Se expide la presente CARTA INVITACION únicamente para los efectos de internación al país en los términos antes señalados.";
        //    result.Append(punto8);

        //    return result;
        //}

        //private StringBuilder GeneraContenidoCeldaEngOCESAPresenta(IReporteInfo infoEventosList)
        //{
        //    //Parrafo 1
        //    string NombreInvitado = infoEventosList.NombreInvitado;//"MIGUEL ANGEL VILLEGAS";
        //    string Nacionalidad = infoEventosList.Nacionalidad;// "USA";
        //    string NumPasaporte = infoEventosList.NumPasaporte;// "213123x122342";

        //    //Punto 6
        //    string puestoParteStaff = "STAFF";

        //    //Punto 7
        //    string fechaEntradaAlPais = infoEventosList.FechaEntradaAlPais;// "2023/03/28";
        //    string fechaSalidaAlPais = infoEventosList.FechaSalidaAlPais;// "2023/04/28";

        //    StringBuilder result = new StringBuilder();

        //    result.AppendLine(NombreInvitado);
        //    result.AppendLine("");
        //    string parrafo1 = $"In accordance with Article 26 of the \"Guidelines and Procedures for Immigration Proceedings\" and Procedure 1 of the \"General Guidelines for issuing visas\" emitted by the Ministries of Interior and Foreign Affairs extends this INVITATION LETTER for {NombreInvitado}, of {Nacionalidad} nationality, passport number {NumPasaporte}, as follows:";
        //    result.AppendLine(parrafo1);
        //    result.AppendLine("");
        //    string punto1 = "1. FULL NAME OF LEGAL GUARDIAN AND NATIONALITY: Lic. Alfonso David Aragon Buendia, legal representative of OCESA PRESENTA, S.A. de C.V. of Mexican nationality.";
        //    result.AppendLine(punto1);
        //    result.AppendLine("\n");
        //    string punto2 = "2. CORPORATE NAME/ORGANIZATION: OCESA PRESENTA, S.A. de C.V. (hereinafter \"OCESA\")";
        //    result.Append(punto2);
        //    result.AppendLine("\n");
        //    string punto3 = "3. REGISTRATION NUMBER AND PURPOSE OF THE ORGANIZATION\nOCESA PRESENTA, S.A. de C.V. was constituted before the Public Notary No. 70 of Mexico City Lic Ponciano López Juárez through public deed 97,765 dated November 18th, 2010, being the object of the company among others the contracting, promotion and staging of all kinds of musical, artistic, cinematographic, theatrical, sports and commercial shows, as well as the hiring of artists, musicians, musical groups, and dance choirs and athletes.\r\n";
        //    result.Append(punto3);
        //    result.AppendLine("\n");
        //    string punto4 = "4. PROOF OF REGISTRATION NUMBER AND DATE OF REGISTRATION TO THE NATIONAL INSTITUTE OF MIGRATION: Number 1002695236.";
        //    result.Append(punto4);
        //    result.AppendLine("\n");
        //    string punto5 = "5. COMPLETE ADDRESS AND CONTACT DETAILS OF THE ORGANIZATION:\nThe company is located at Independencia No. 90, Colonia Centro (área 5), Alcaldía en Cuauhtémoc, C.P. 06050, Ciudad de México, Phone number: (55) 26296900 Contact person: Lic. Alfonso David Aragon Buendia.";
        //    result.Append(punto5);
        //    result.AppendLine("\n");
        //    string punto6 = $"6. INFORMATION ON THE ACTIVITY MADE BY THE FOREIGN GUEST:\nParticipate as {puestoParteStaff} in the event called:";
        //    int numEventos = infoEventosList.InfoEventosList.Count;
        //    int eventosRecorridos = 0;
        //    foreach (IInfoEvento evento in infoEventosList.InfoEventosList)
        //    {
        //        eventosRecorridos++;
        //        punto6 += $" “{evento.NombreEvento}” which will take place on  {evento.FechaEvento} at the “{evento.InmuebleEvento}” , located in {evento.UbicacionInmueble}";
        //        if (eventosRecorridos < numEventos)
        //            punto6 += $",";
        //        else
        //            punto6 += $".";
        //    }
        //    result.Append(punto6);
        //    result.AppendLine("\n");
        //    string punto7 = $"7. DATE OF ENTRY AND DEPARTURE OF THE FOREIGN GUEST: \nEntry {fechaEntradaAlPais}.\nDeparture {fechaSalidaAlPais}.";
        //    result.Append(punto7);
        //    result.AppendLine("\n");
        //    string punto8 = $"8. ATTACHED COPY OF THE OFFICIAL IDENTIFICATION OF THE PERSON WHO SIGNED THE INVITATION LETTER. It is important to note that {NombreInvitado} mentioned in is invited by OCESA to participate in the aforementioned event under the condition of stay of VISITOR WITHOUT PERMISSION TO PERFORM REMUNERATED ACTIVITIES, since the fees, salaries and expenses of these foreigners are paid entirely outside the national territory by companies other than OCESA. This LETTER OF INVITATION is issued only for the purpose of internment in the country in the terms indicated above.";
        //    result.Append(punto8);

        //    return result;
        //}

        //public StringBuilder GeneraSeccionFirma()
        //{
        //    StringBuilder result = new StringBuilder();

        //    result.AppendLine("             _________________________________");
        //    result.AppendLine("             Lic. Alfonso David Aragon Buendia");
        //    result.AppendLine("             Apoderado Legal OCESA");

        //    return result;
        //}

        //public StringBuilder GeneraFooter()
        //{
        //    StringBuilder result = new StringBuilder();

        //    result.AppendLine("CALLE INDEPENDENCIA N° 90, COL.CENTRO, ALCALDIA EN. CUAUHTEMOC, C.P. 06050");
        //    result.AppendLine("                           CIUDAD DE MEXICO");

        //    return result;
        //}
        #endregion


        #region Carta Invitacion
        public AttachmentFileDto GenerateDocument(InviteDto regInvite,IReporteInfo reporteInfo)
        {
            AttachmentFileDto result = new AttachmentFileDto();
            result.FileName = $"{regInvite.FILE_NAME}";
            //string attachment = $"attachment; filename={nombreArchivo}\"{DateTime.Now.ToString()}.pdf";



            //Creacion del documento
            doc = new Document();
            //Configuraciones de estructura del documento
            doc.SetPageSize(PageSize.Letter);
            //28.34f son los puntos que equivalen a un cm
            doc.SetMargins(MARGEN_IZQUIERDO_OCESA_PRESENTA, MARGEN_DERECHO_OCESA_PRESENTA, MARGEN_SUPERIOR_OCESA_PRESENTA, MARGEN_IZQUIERDO_OCESA_PRESENTA);


            // Indicamos donde vamos a guardar el documento
            bufferDoc = new MemoryStream();
            writer = PdfWriter.GetInstance(doc, bufferDoc);
            // Le colocamos el título y el autor
            // **Nota: Esto no será visible en el documento
            doc.AddTitle("OCESA");
            doc.AddCreator("OCESA");
            doc.AddAuthor("OCESA");

            doc.Open();
            plantillaOCESA(regInvite, reporteInfo);

            doc.Close();

            result.File = bufferDoc.ToArray();
            return result;

        }
        public static Stream GetImage(string imagen)
        {
            var assembly = typeof(CommonTools.Pdf.PdfManager).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream($"CommonTools.Assets.{imagen}");
            return stream;
        }
        private string ObtenerNombreImagenLogoDerecho(string nombre)
        {
            string result = "";
            switch(nombre)
            {
                case "OPROM":
                    result = "oprom.png";
                    break;
                case "Promotodo":
                    result = "promotodo.png";
                    break;
                case "OCESA Presenta":
                    result = "ocesa_presenta.png";
                    break;
                    default:
                    result = "empty_logo.png";
                    break;
            }
            return result;
        }

        private void plantillaOCESA(InviteDto regInvite, IReporteInfo reporteInfo)
        {
            FontFactory.RegisterDirectories();

            string imagen = ObtenerNombreImagenLogoDerecho(reporteInfo.TipoArchivoGenerado);

            Image logo = Image.GetInstance(GetImage($"logo_ocesa.png"));
            logo.ScaleAbsoluteHeight(ALTO_IMAGEN_OCESA);
            logo.ScaleAbsoluteWidth(ANCHO_IMAGEN_OCESA);

            Image logoOcesaPresenta = Image.GetInstance(GetImage(imagen));
            logoOcesaPresenta.ScaleAbsoluteHeight(ALTO_IMAGEN_OCESA);
            logoOcesaPresenta.ScaleAbsoluteWidth(ANCHO_IMAGEN_OCESA);

            // ENCABEZADO
            //var encabezado = new Paragraph() { Alignment = Element.ALIGN_CENTER, Font = FuenteArial11Negrita };
            //encabezado.Add(new Chunk(logo, -100, -35));
            ////encabezado.Add(new Chunk("CARTA INVITACIÓN/INVITATION LETTER"));
            //encabezado.Add(new Chunk(regInvite.DES_TITLE));
            //encabezado.Add(new Chunk(logoOcesaPresenta, 100, -35));

            var TableHeader = new PdfPTable(new float[] { 15f, 70f, 15f });
            TableHeader.WidthPercentage = 100;

            var cellLogoOCESA = new PdfPCell(logo);
            cellLogoOCESA.HorizontalAlignment = Element.ALIGN_CENTER;
            cellLogoOCESA.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellLogoOCESA.Border = Rectangle.NO_BORDER;


            var phraseTituloDocumento = new Phrase(regInvite.DES_TITLE, FuenteArial11Negrita);
            var cellTitulo = new PdfPCell(phraseTituloDocumento);
            cellTitulo.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTitulo.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellTitulo.Border = Rectangle.NO_BORDER;

            var cellLogoTipoEvento = new PdfPCell(logoOcesaPresenta);
            cellLogoTipoEvento.HorizontalAlignment = Element.ALIGN_CENTER;
            cellLogoTipoEvento.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellLogoTipoEvento.Border = Rectangle.NO_BORDER;


            TableHeader.AddCell(cellLogoOCESA);
            TableHeader.AddCell(cellTitulo);
            TableHeader.AddCell(cellLogoTipoEvento);



            //Se asigna las columnas
            var generalTable = new PdfPTable(new float[] { 50f, 50f});
            generalTable.WidthPercentage = 100;
            generalTable.SplitLate = false;
            

            StringBuilder celdaEspanol = GeneraContenidoCeldaEspanOCESA(regInvite, reporteInfo);
            StringBuilder celdaIngles = GeneraContenidoCeldaEngOCESA(regInvite, reporteInfo);

            string celdaEspanolStr = celdaEspanol.ToString();
            Phrase phraseEsp = GeneraParrafoCeldaConEstilos(celdaEspanolStr);
            //columns.AddElement(celdaEspanText);
            //var phraseEsp = new Phrase(celdaEspanol.ToString(), FuenteArial8);
            var cellEsp = new PdfPCell(phraseEsp);
            cellEsp.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cellEsp.Padding = 10;
            cellEsp.BorderColorBottom = BaseColor.White;

            string celdaInglesStr = celdaIngles.ToString();
            Phrase phraseIng = GeneraParrafoCeldaConEstilos(celdaInglesStr);
            var cellIng = new PdfPCell(phraseIng);
            cellIng.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cellIng.Padding = 10;
            cellIng.BorderColorBottom = BaseColor.White;

            //Agregar celdas de los cuerpos principales
            generalTable.AddCell(cellEsp);
            generalTable.AddCell(cellIng);


            //Agregar firmas 
            string seccionFirma = regInvite.SIGN_1;
            if (!string.IsNullOrEmpty(seccionFirma))
            {
                //Agregar imagenes de firmas
                byte[] bytesImage = Convert.FromBase64String(regInvite.SIGN_BLOB);

                Image signImageEsp;
                using (MemoryStream ms = new MemoryStream(bytesImage))
                {
                    signImageEsp = Image.GetInstance(ms);
                    signImageEsp.ScaleAbsoluteHeight(ALTO_IMAGEN_FIRMA);
                    signImageEsp.ScaleAbsoluteWidth(ANCHO_IMAGEN_FIRMA);

                    var cellFirmaImagenEsp = new PdfPCell(signImageEsp);
                    cellFirmaImagenEsp.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellFirmaImagenEsp.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //cellFirmaImagenEsp.Border = Rectangle.NO_BORDER;
                    cellFirmaImagenEsp.PaddingTop = 10;
                    cellFirmaImagenEsp.PaddingBottom = 10;
                    generalTable.AddCell(cellFirmaImagenEsp);
                }

                Image signImageEng;
                using (MemoryStream ms = new MemoryStream(bytesImage))
                {
                    signImageEng = Image.GetInstance(ms);
                    signImageEng.ScaleAbsoluteHeight(ALTO_IMAGEN_FIRMA);
                    signImageEng.ScaleAbsoluteWidth(ANCHO_IMAGEN_FIRMA);

                    var cellFirmaImagenIng = new PdfPCell(signImageEng);
                    cellFirmaImagenIng.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellFirmaImagenIng.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //cellFirmaImagenIng.Border = Rectangle.NO_BORDER;
                    cellFirmaImagenIng.PaddingTop = 10;
                    cellFirmaImagenIng.PaddingBottom = 10;

                    generalTable.AddCell(cellFirmaImagenIng);
                }


                var phraseFirmaEsp = new Phrase(seccionFirma, FuenteArial11);
                var cellFirmaEsp = new PdfPCell(phraseFirmaEsp);
                cellFirmaEsp.PaddingTop = 0f;
                cellFirmaEsp.BorderColorTop = BaseColor.White;
                cellFirmaEsp.HorizontalAlignment = Element.ALIGN_CENTER;
                generalTable.AddCell(cellFirmaEsp);

                var phraseFirmaIng = new Phrase(seccionFirma, FuenteArial11);
                var cellFirmaIng = new PdfPCell(phraseFirmaIng);
                cellFirmaIng.BorderColorTop = BaseColor.White;
                cellFirmaIng.HorizontalAlignment = Element.ALIGN_CENTER;
                generalTable.AddCell(cellFirmaIng);
            }

            var footerList = new List<Paragraph>();
            if(!string.IsNullOrEmpty(regInvite.FOOT_PAGE))
            {
                footerList = GeneraParrafoFooterConEstilos(regInvite.FOOT_PAGE);
            }
            


            doc.Add(TableHeader);
            //doc.Add(new Paragraph("\n"));
            //doc.Add(columns);
            doc.Add(generalTable);
            foreach (var footer in footerList)
            {
                doc.Add(footer);
            }

        }

        private Phrase GeneraParrafoCeldaConEstilos(string texto)
        {
            FontFactory.RegisterDirectories();

            string[] arregloParrafos = texto.Split('\n');
            var phraseResult = new Phrase();
            string textoAuxiliar = "";
            foreach (string parrafoActual in arregloParrafos)
            {
                textoAuxiliar = "\n" + parrafoActual;
                if (parrafoActual.StartsWith("[RED]"))
                {
                    phraseResult.Add(new Chunk(textoAuxiliar.Replace("[RED]", ""), FuenteArial8Roja));
                }
                else if (parrafoActual.StartsWith("[BOLD]"))
                {
                    phraseResult.Add(new Chunk(textoAuxiliar.Replace("[BOLD]", ""), FuenteArial8Negrita));
                }
                else
                {
                    phraseResult.Add(new Chunk(textoAuxiliar, FuenteArial8));
                }
            }
            return phraseResult;
        }
        private List<Paragraph> GeneraParrafoFooterConEstilos(string texto)
        {
            FontFactory.RegisterDirectories();

            string[] arregloParrafos = texto.Split('\n');
            var result = new List<Paragraph>();
            

            foreach (string parrafoActual in arregloParrafos)
            {
                if (parrafoActual.StartsWith("[RED]"))
                {
                    var phraseResult = new Phrase(new Chunk("\n" + parrafoActual.Replace("[RED]", ""), FuenteArial11RojaNegrita));
                    result.Add(new Paragraph(phraseResult) { Alignment = Element.ALIGN_CENTER});
                }
                else if (parrafoActual.StartsWith("[BOLD]"))
                {
                    var phraseResult = new Phrase(new Chunk("\n" + parrafoActual.Replace("[BOLD]", ""), FuenteArial11Negrita));
                    result.Add(new Paragraph(phraseResult) { Alignment = Element.ALIGN_JUSTIFIED });
                }
                else if (parrafoActual.StartsWith("[CENTER_BLACK]"))
                {
                    var phraseResult = new Phrase(new Chunk("\n" + parrafoActual.Replace("[CENTER_BLACK]", ""), FuenteArial11Negrita));
                    result.Add(new Paragraph(phraseResult) { Alignment = Element.ALIGN_CENTER });
                }
                else
                {
                    var phraseResult = new Phrase(new Chunk("\n" + parrafoActual, FuenteArial11));
                    result.Add(new Paragraph(phraseResult) { Alignment = Element.ALIGN_JUSTIFIED });
                }
            }
            return result;
        }

        private StringBuilder GeneraContenidoCeldaEspanOCESA(InviteDto regInvite, IReporteInfo infoEventosList)
        {


            //Parrafo 1
            //string NombreInvitado = infoEventosList.NombreInvitado;//"MIGUEL ANGEL VILLEGAS";
            //string Nacionalidad = infoEventosList.Nacionalidad;// "USA";
            //string NumPasaporte = infoEventosList.NumPasaporte;// "213123x122342";

            ////Punto 6
            //string puestoParteStaff = infoEventosList.PuestoParteStaff; //Staff

            ////Punto 7
            //string fechaEntradaAlPais = infoEventosList.FechaEntradaAlPais;// "2023/03/28";
            //string fechaSalidaAlPais = infoEventosList.FechaSalidaAlPais;// "2023/04/28";

            //string[] dataDocumentInsert = { infoEventosList.NombreInvitado,
            //    infoEventosList.Nacionalidad,
            //    infoEventosList.NumPasaporte,
            //    infoEventosList.PuestoParteStaff,
            //    infoEventosList.FechaEntradaAlPais,
            //    infoEventosList.FechaSalidaAlPais};

            StringBuilder result = new StringBuilder();
            //var content = string.Format($"{regInvite.DESC_SPANISH}",  dataDocumentInsert);
            var content = ReemplazaBanderasPorContenidoPrincipalCeldasESP(infoEventosList,regInvite.DESC_SPANISH);
            result.AppendLine(content);

            //string parrafo1 = $"De conformidad con el artículo 26 de los “Lineamientos para Trámites y Procedimientos Migratorios” y Trámite 1 de los “Lineamientos generales para la expedición de visas” que emiten las Secretarías de Gobernación y de Relaciones Exteriores se extiende la presente CARTA INVITACION a favor de {NombreInvitado}, de la nacionalidad de {Nacionalidad}, con número de pasaporte {NumPasaporte}, en los siguientes términos:";
            ////result.AppendLine(parrafo1);
            ////result.AppendLine("");
            //string punto1 = "1.- NOMBRE COMPLETO DEL APODERADO LEGAL Y NACIONALIDAD:\nLic. Alfonso David Aragon Buendia, apoderado legal de OCESA PRESENTA, S.A. de C.V. de nacionalidad mexicana.";
            ////result.AppendLine(punto1);
            ////result.AppendLine("\n");
            ////string punto2 = "2.- DENOMINACION O RAZON SOCIAL DE LA ORGANIZACIÓN: OCESA PRESENTA, S.A. de C.V. (en adelante “OCESA”)";
            ////result.Append(punto2);
            ////result.AppendLine("\n");
            //string punto3 = "3.-NUMERO DE REGISTRO Y OBJETO DE LA ORGANIZACIÓN: OCESA PRESENTA, S.A. de C.V. se constituyó ante el Notario Público de la Ciudad de México Lic. Ponciano López Juárez mediante la Escritura Pública 97,765 de fecha 18 de noviembre de 2010, siendo el objeto de la sociedad entre otros la contratación, promoción y puesta en escena de todo tipo de espectáculos musicales, artísticos, cinematográficos, teatrales, deportivos y comerciales, así como la contratación de artistas, músicos, grupos musicales, y corales de danza y deportistas.";
            ////result.Append(punto3);
            ////result.AppendLine("\n");
            //string punto4 = "4.- NUMERO DE CONSTANCIA DE INSCRIPCIÓN Y FECHA DE REGISTRO ANTE EL INSTITUTO NACIONAL DE MIGRACIÓN: Número 1002695236.";
            ////result.Append(punto4);
            ////result.AppendLine("\n");
            //string punto5 = "5.- DOMICILIO COMPLETO Y DATOS DE CONTACTO DE LA ORGANIZACIÓN:\nLa empresa tiene su domicilio ubicado en calle Independencia No. 90, Colonia Centro (área 5), Alcaldía en Cuauhtémoc, C.P. 06050, Ciudad de México, teléfono: (55) 26296900. Nombre del Contacto: Lic. Alfonso David Aragon Buendia.";
            ////result.Append(punto5);
            ////result.AppendLine("\n");
            //string punto6 = $"6.-INFORMACION SOBRE LA ACTIVIDAD QUE REALIZARA LA PERSONA EXTRANJERA INVITADA:\nParticipar como {puestoParteStaff} en el(os) evento(s) denominado(s):";
            //int numEventos = infoEventosList.InfoEventosList.Count;
            //int eventosRecorridos = 0;
            //foreach (IInfoEvento evento in infoEventosList.InfoEventosList)
            //{
            //    eventosRecorridos++;
            //    punto6 += $" “{evento.NombreEvento}” que se llevará acabo el día {evento.FechaEvento} en el “{evento.InmuebleEvento}” en la {evento.UbicacionInmueble}";
            //    if (eventosRecorridos < numEventos)
            //        punto6 += $",";
            //    else
            //        punto6 += $".";
            //}
            //result.Append(punto6);
            //result.AppendLine("\n");
            //string punto7 = $"7.- FECHA DE ENTRADA Y SALIDA DEL INVITADO AL PAÍS:\nEntrada el día {fechaEntradaAlPais}.\nSalida el día {fechaSalidaAlPais}.";
            //result.Append(punto7);
            //result.AppendLine("\n");
            //string punto8 = $"8.- SE ADJUNTA COPIA DE LA IDENTIFICACION OFICIAL DE LA\r\nPERSONA QUE SUSCRIBE LA CARTA INVITACION. Es importante señalar que {NombreInvitado} que se mencionan en es invitado por OCESA para participar en el evento antes señalado bajo la condición de estancia de VISITANTE SIN PERMISO PARA REALIZAR ACTIVIDADES REMUNERADAS, toda vez los honorarios, sueldos y gastos de estos extranjeros son pagados íntegramente fuera del territorio nacional por empresas distintas a OCESA. Se expide la presente CARTA INVITACION únicamente para los efectos de internación al país en los términos antes señalados.";
            //result.Append(punto8);

            return result;
        }

  
        private StringBuilder GeneraContenidoCeldaEngOCESA(InviteDto regInvite, IReporteInfo infoEventosList)
        {
            ////Parrafo 1
            //string NombreInvitado = infoEventosList.NombreInvitado;//"MIGUEL ANGEL VILLEGAS";
            //string Nacionalidad = infoEventosList.Nacionalidad;// "USA";
            //string NumPasaporte = infoEventosList.NumPasaporte;// "213123x122342";

            ////Punto 6
            //string puestoParteStaff = infoEventosList.PuestoParteStaff; //Staff

            ////Punto 7
            //string fechaEntradaAlPais = infoEventosList.FechaEntradaAlPais;// "2023/03/28";
            //string fechaSalidaAlPais = infoEventosList.FechaSalidaAlPais;// "2023/04/28";

            string[] dataDocumentInsert = { infoEventosList.NombreInvitado,
                infoEventosList.Nacionalidad,
                infoEventosList.NumPasaporte,
                infoEventosList.PuestoParteStaff,
                infoEventosList.FechaEntradaAlPais,
                infoEventosList.FechaSalidaAlPais};

            StringBuilder result = new StringBuilder();
            var content = ReemplazaBanderasPorContenidoPrincipalCeldasENG(infoEventosList, regInvite.DESC_ENGLISH);
            result.AppendLine(content);


            //result.AppendLine(NombreInvitado);
            //result.AppendLine("");
            //string parrafo1 = $"In accordance with Article 26 of the \"Guidelines and Procedures for Immigration Proceedings\" and Procedure 1 of the \"General Guidelines for issuing visas\" emitted by the Ministries of Interior and Foreign Affairs extends this INVITATION LETTER for {NombreInvitado}, of {Nacionalidad} nationality, passport number {NumPasaporte}, as follows:";
            //result.AppendLine(parrafo1);
            //result.AppendLine("");
            //string punto1 = "1. FULL NAME OF LEGAL GUARDIAN AND NATIONALITY: Lic. Alfonso David Aragon Buendia, legal representative of OCESA PRESENTA, S.A. de C.V. of Mexican nationality.";
            //result.AppendLine(punto1);
            //result.AppendLine("\n");
            //string punto2 = "2. CORPORATE NAME/ORGANIZATION: OCESA PRESENTA, S.A. de C.V. (hereinafter \"OCESA\")";
            //result.Append(punto2);
            //result.AppendLine("\n");
            //string punto3 = "3. REGISTRATION NUMBER AND PURPOSE OF THE ORGANIZATION\nOCESA PRESENTA, S.A. de C.V. was constituted before the Public Notary No. 70 of Mexico City Lic Ponciano López Juárez through public deed 97,765 dated November 18th, 2010, being the object of the company among others the contracting, promotion and staging of all kinds of musical, artistic, cinematographic, theatrical, sports and commercial shows, as well as the hiring of artists, musicians, musical groups, and dance choirs and athletes.\r\n";
            //result.Append(punto3);
            //result.AppendLine("\n");
            //string punto4 = "4. PROOF OF REGISTRATION NUMBER AND DATE OF REGISTRATION TO THE NATIONAL INSTITUTE OF MIGRATION: Number 1002695236.";
            //result.Append(punto4);
            //result.AppendLine("\n");
            //string punto5 = "5. COMPLETE ADDRESS AND CONTACT DETAILS OF THE ORGANIZATION:\nThe company is located at Independencia No. 90, Colonia Centro (área 5), Alcaldía en Cuauhtémoc, C.P. 06050, Ciudad de México, Phone number: (55) 26296900 Contact person: Lic. Alfonso David Aragon Buendia.";
            //result.Append(punto5);
            //result.AppendLine("\n");
            //string punto6 = $"6. INFORMATION ON THE ACTIVITY MADE BY THE FOREIGN GUEST:\nParticipate as {puestoParteStaff} in the event called:";
            //int numEventos = infoEventosList.InfoEventosList.Count;
            //int eventosRecorridos = 0;
            //foreach (IInfoEvento evento in infoEventosList.InfoEventosList)
            //{
            //    eventosRecorridos++;
            //    punto6 += $" “{evento.NombreEvento}” which will take place on  {evento.FechaEvento} at the “{evento.InmuebleEvento}” , located in {evento.UbicacionInmueble}";
            //    if (eventosRecorridos < numEventos)
            //        punto6 += $",";
            //    else
            //        punto6 += $".";
            //}
            //result.Append(punto6);
            //result.AppendLine("\n");
            //string punto7 = $"7. DATE OF ENTRY AND DEPARTURE OF THE FOREIGN GUEST: \nEntry {fechaEntradaAlPais}.\nDeparture {fechaSalidaAlPais}.";
            //result.Append(punto7);
            //result.AppendLine("\n");
            //string punto8 = $"8. ATTACHED COPY OF THE OFFICIAL IDENTIFICATION OF THE PERSON WHO SIGNED THE INVITATION LETTER. It is important to note that {NombreInvitado} mentioned in is invited by OCESA to participate in the aforementioned event under the condition of stay of VISITOR WITHOUT PERMISSION TO PERFORM REMUNERATED ACTIVITIES, since the fees, salaries and expenses of these foreigners are paid entirely outside the national territory by companies other than OCESA. This LETTER OF INVITATION is issued only for the purpose of internment in the country in the terms indicated above.";
            //result.Append(punto8);

            return result;
        }
        private string ReemplazaBanderasPorContenidoPrincipalCeldasESP(IReporteInfo infoEvento, string texto = "")
        {
            //[NOMBRE_INVITADO]
            //[NACIONALIDAD]
            //[NUMERO_PASAPORTE]
            //[PUESTO_STAFF]
            //[FECHA_INGRESO_AL_PAIS]
            //[FECHA_SALIDA_DEL_PAIS]
            //[LISTA_DE_EVENTOS]

            texto = texto.Replace("[NOMBRE_INVITADO]", infoEvento.NombreInvitado);
            texto = texto.Replace("[NACIONALIDAD]", infoEvento.Nacionalidad);
            texto = texto.Replace("[NUMERO_PASAPORTE]", infoEvento.NumPasaporte);
            texto = texto.Replace("[PUESTO_STAFF]", infoEvento.PuestoParteStaff);
            texto = texto.Replace("[FECHA_INGRESO_AL_PAIS]", infoEvento.FechaEntradaAlPais);
            texto = texto.Replace("[FECHA_SALIDA_DEL_PAIS]", infoEvento.FechaSalidaAlPais);


            int numEventos = infoEvento.InfoEventosList.Count;
            int eventosRecorridos = 0;
            string eventosConcatenados = "";
            foreach (IInfoEvento evento in infoEvento.InfoEventosList)
            {
                eventosRecorridos++;
                eventosConcatenados += $" “{evento.NombreEvento}” que se llevará acabo el(los) día(s) {RemplazaIntervaloDeFechasEventoPorTexto(evento.FechaInicioEvento, evento.FechaFinEvento, true)} en el “{evento.InmuebleEvento}” en la {evento.UbicacionInmueble}";
                if (eventosRecorridos < numEventos)
                    eventosConcatenados += $",";
                else
                    eventosConcatenados += $".";
            }

            texto = texto.Replace("[LISTA_DE_EVENTOS]", eventosConcatenados);

            return texto;
        }

        private string RemplazaIntervaloDeFechasEventoPorTexto(string fechaInicioStr, string fechaFinStr, bool lenguajeES = true)
        {
            DateTime fechaInicio = DateTime.ParseExact(fechaInicioStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(fechaFinStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            StringBuilder resultado = new StringBuilder();

            DateTime fechaActual = fechaInicio;
            int mesActual = fechaActual.Month;
            bool primero = true;

            while (fechaActual <= fechaFin)
            {
                if (fechaActual.Month != mesActual) // Cambio de mes detectado
                {
                    if (lenguajeES)
                    {
                        resultado.Append($"/{mesActual:00}/{fechaActual.Year} y ");
                    }
                    else
                    {
                        resultado.Append($"/{mesActual:00}/{fechaActual.Year} and ");
                    }
                    mesActual = fechaActual.Month;
                    primero = true; // Reiniciar el primer día del nuevo mes
                }

                if (!primero)
                {
                    resultado.Append(", ");
                }

                resultado.Append($"{fechaActual.Day}");

                fechaActual = fechaActual.AddDays(1);
                primero = false;
            }

            resultado.Append($"/{mesActual:00}/{fechaFin.Year}");

            return resultado.ToString();
        }

        private string ReemplazaBanderasPorContenidoPrincipalCeldasENG(IReporteInfo infoEvento, string texto = "")
        {
            //[NOMBRE_INVITADO]
            //[NACIONALIDAD]
            //[NUMERO_PASAPORTE]
            //[PUESTO_STAFF]
            //[FECHA_INGRESO_AL_PAIS]
            //[FECHA_SALIDA_DEL_PAIS]
            //[LISTA_DE_EVENTOS]

            texto = texto.Replace("[NOMBRE_INVITADO]", infoEvento.NombreInvitado);
            texto = texto.Replace("[NACIONALIDAD]", infoEvento.Nacionalidad);
            texto = texto.Replace("[NUMERO_PASAPORTE]", infoEvento.NumPasaporte);
            texto = texto.Replace("[PUESTO_STAFF]", infoEvento.PuestoParteStaff);
            texto = texto.Replace("[FECHA_INGRESO_AL_PAIS]", infoEvento.FechaEntradaAlPais);
            texto = texto.Replace("[FECHA_SALIDA_DEL_PAIS]", infoEvento.FechaSalidaAlPais);


            int numEventos = infoEvento.InfoEventosList.Count;
            int eventosRecorridos = 0;
            string eventosConcatenados = "";
            foreach (IInfoEvento evento in infoEvento.InfoEventosList)
            {
                eventosRecorridos++;
                eventosConcatenados += $" “{evento.NombreEvento}” which will take place on {RemplazaIntervaloDeFechasEventoPorTexto(evento.FechaInicioEvento, evento.FechaFinEvento, false)} at the “{evento.InmuebleEvento}” located in  {evento.UbicacionInmueble}";
                if (eventosRecorridos < numEventos)
                    eventosConcatenados += $",";
                else
                    eventosConcatenados += $".";
            }

            texto = texto.Replace("[LISTA_DE_EVENTOS]", eventosConcatenados);

            return texto;
        }
        #endregion
    }
}
