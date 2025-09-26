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
        private Font FuenteArial14Negrita = new Font(FontFactory.GetFont("Arial MT", 14, Font.BOLD));


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


        /* Se agrega nueva version para administrador */
        public AttachmentFileDto GenerateOtherAssistantsDocument(InviteDto regInvite, IReporteInfo reporteInfo, List<IOtroInvitadoModel> otrosInvitados, List<IOtroInvitadoModel> otrosArtistas, string lenguaje)
        {
            AttachmentFileDto result = new AttachmentFileDto();
            result.FileName = $"{regInvite.FILE_NAME}";
            //string attachment = $"attachment; filename={nombreArchivo}\"{DateTime.Now.ToString()}.pdf";



            //Creacion del documento
            doc = new Document();
            //Configuraciones de estructura del documento
            doc.SetPageSize(PageSize.Letter);
            //28.34f son los puntos que equivalen a un cm
            doc.SetMargins(MARGEN_IZQUIERDO_OCESA_PRESENTA*4, MARGEN_DERECHO_OCESA_PRESENTA*4, MARGEN_SUPERIOR_OCESA_PRESENTA, MARGEN_INFERIOR_OCESA_PRESENTA*4);


            // Indicamos donde vamos a guardar el documento
            bufferDoc = new MemoryStream();
            writer = PdfWriter.GetInstance(doc, bufferDoc);

            // Asignar evento para footer
            writer.PageEvent = new PdfFooter
            {
                TextoFooter = regInvite.FOOT_PAGE
            };

            // Le colocamos el título y el autor
            // **Nota: Esto no será visible en el documento
            doc.AddTitle("OCESA");
            doc.AddCreator("OCESA");
            doc.AddAuthor("OCESA");

            doc.Open();
            /* Obtener el nombre del primer evento */
            string nombrePrimerEvento = reporteInfo.InfoEventosList[0].NombreEvento;
            plantillaOCESAOtrosAsistentes(regInvite, reporteInfo, otrosInvitados, otrosArtistas, nombrePrimerEvento, lenguaje);

            doc.Close();

            result.File = bufferDoc.ToArray();
            return result;

        }


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

        private void plantillaOCESAOtrosAsistentes(InviteDto regInvite, IReporteInfo reporteInfo, List<IOtroInvitadoModel> otrosInvitados, List<IOtroInvitadoModel> otrosArtistas, string nombrePrimerEvento, string language = "ES")
        {
            FontFactory.RegisterDirectories();

            string imagen = ObtenerNombreImagenLogoDerecho(reporteInfo.TipoArchivoGenerado);

            Image logo = Image.GetInstance(GetImage($"logo_ocesa.png"));
            logo.ScaleAbsoluteHeight(ALTO_IMAGEN_OCESA);
            logo.ScaleAbsoluteWidth(ANCHO_IMAGEN_OCESA);

            Image logoOcesaPresenta = Image.GetInstance(GetImage(imagen));
            logoOcesaPresenta.ScaleAbsoluteHeight(ALTO_IMAGEN_OCESA);
            logoOcesaPresenta.ScaleAbsoluteWidth(ANCHO_IMAGEN_OCESA);

            var TableHeader = new PdfPTable(new float[] { 15f, 70f, 15f });
            TableHeader.WidthPercentage = 100;

            var cellLogoOCESA = new PdfPCell(logo);
            cellLogoOCESA.HorizontalAlignment = Element.ALIGN_CENTER;
            cellLogoOCESA.VerticalAlignment = Element.ALIGN_MIDDLE;
            cellLogoOCESA.Border = Rectangle.NO_BORDER;


            var phraseTituloDocumento = new Phrase(regInvite.DES_TITLE, FuenteArial14Negrita);
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
            var generalTable = new PdfPTable(new float[] { 100f });
            generalTable.WidthPercentage = 100;
            generalTable.SplitLate = false;


            /* Agregar la fecha a la derecha*/
            string celdaFechaStr = $"Ciudad de México, a {DateTime.Now.Day} de {DateTime.Now.ToString("MMMM", new CultureInfo("es-ES"))} de {DateTime.Now.Year}";
            Phrase phraseFecha = new Phrase(celdaFechaStr, FuenteArial11);
            var cellFecha = new PdfPCell(phraseFecha);
            cellFecha.HorizontalAlignment = Element.ALIGN_RIGHT;
            cellFecha.Padding = 10;
            cellFecha.Border = Rectangle.NO_BORDER;
            generalTable.AddCell(cellFecha);

            StringBuilder celdaGen;
            if (language.Equals("EN"))
            {
                celdaGen = GeneraContenidoCeldaEngOCESA(regInvite, reporteInfo, otrosArtistas);
            }
            else {
                celdaGen = GeneraContenidoCeldaEspanOCESA(regInvite, reporteInfo, otrosArtistas, nombrePrimerEvento);
            }

            string celdaStr = celdaGen.ToString();
            Phrase phrase = GeneraParrafoCeldaConEstilos(celdaStr, 11);
            var cellGen = new PdfPCell(phrase);
            cellGen.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cellGen.Padding = 10;
            cellGen.BorderColorBottom = BaseColor.White;
            cellGen.Border = Rectangle.NO_BORDER;

            
            //Agregar celdas de los cuerpos principales
            generalTable.AddCell(cellGen);
            //generalTable.AddCell(cellIng);

            Image signImageAnexo = null;
            //Agregar firmas 
            string seccionFirma = regInvite.SIGN_1;
            if (!string.IsNullOrEmpty(seccionFirma))
            {
                //Agregar imagenes de firmas
                string firma = string.IsNullOrEmpty(regInvite.SIGN_BLOB) ? "" : regInvite.SIGN_BLOB;
                byte[] bytesImage = Convert.FromBase64String(firma);

                Image signImageEsp;
                using (MemoryStream ms = new MemoryStream(bytesImage))
                {
                    if (firma.Equals(""))
                    {
                        // Dimensiones de la imagen en blanco
                        int width = (int)ANCHO_IMAGEN_FIRMA;  // Ancho de la imagen
                        int height = (int)ALTO_IMAGEN_FIRMA; // Alto de la imagen
                        byte[] whiteImageData = new byte[width * height * 3]; // Imagen RGB en blanco
                        // Rellenar con blanco (255 en cada canal)
                        for (int i = 0; i < whiteImageData.Length; i++)
                        {
                            whiteImageData[i] = 255;
                        }
                        signImageEsp = Image.GetInstance(width, height, 3, 8, whiteImageData);
                    }
                    else
                    {
                        signImageEsp = Image.GetInstance(ms);
                    }
                    signImageEsp.ScaleAbsoluteHeight(ALTO_IMAGEN_FIRMA);
                    signImageEsp.ScaleAbsoluteWidth(ANCHO_IMAGEN_FIRMA);

                    var cellFirmaImagenEsp = new PdfPCell(signImageEsp);
                    cellFirmaImagenEsp.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellFirmaImagenEsp.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cellFirmaImagenEsp.Border = Rectangle.NO_BORDER;
                    cellFirmaImagenEsp.PaddingTop = 10;
                    cellFirmaImagenEsp.PaddingBottom = 10;
                    generalTable.AddCell(cellFirmaImagenEsp);


                    signImageAnexo = Image.GetInstance(signImageEsp);

                }

                var phraseFirmaEsp = new Phrase(seccionFirma, FuenteArial11);
                var cellFirmaEsp = new PdfPCell(phraseFirmaEsp);
                cellFirmaEsp.PaddingTop = 2f;
                cellFirmaEsp.PaddingBottom = 5f;
                cellFirmaEsp.Border = Rectangle.TOP_BORDER;
                cellFirmaEsp.BorderWidthTop = 1f;
                cellFirmaEsp.HorizontalAlignment = Element.ALIGN_CENTER;
                cellFirmaEsp.VerticalAlignment = Element.ALIGN_MIDDLE;

                generalTable.AddCell(cellFirmaEsp);

            }



            //var footerList = new List<Paragraph>();
            //if (!string.IsNullOrEmpty(regInvite.FOOT_PAGE))
            //{
            //    footerList = GeneraParrafoFooterConEstilos(regInvite.FOOT_PAGE);
            //}



            doc.Add(TableHeader);
            //doc.Add(new Paragraph("\n"));
            //doc.Add(columns);
            doc.Add(generalTable);


            #region Seccion Anexo 1

            doc.NewPage();
            doc.Add(new Paragraph("\n\n")); // dos saltos de línea
            // 3) Tabla de 5 columnas
            var tableTituloAnexo = new PdfPTable(1)
            {
                WidthPercentage = 100f,
                SpacingBefore = 5f,
                SpacingAfter = 5f
            };
            tableTituloAnexo.SetWidths(new float[] { 10 });

            var cellTituloAnexo = new PdfPCell(new Phrase("ANEXO 1", FuenteArial11Negrita))
            {
                Border = Rectangle.NO_BORDER,
                Padding = 5f,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            //
            //
            string tituloAnexo1 = "LISTA DE INVITADOS EVENTO [NOMBRE_PRIMER_EVENTO] [LISTA_DE_OTROS_ARTISTAS]";
            tituloAnexo1 = GeneraTituloAnexo1OtrosInvitados(tituloAnexo1, otrosArtistas, nombrePrimerEvento);
            
            var cellTituloAnexolista = new PdfPCell(new Phrase(tituloAnexo1, FuenteArial11Negrita))
            {
                Border = Rectangle.NO_BORDER,
                Padding = 5f,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            tableTituloAnexo.AddCell(cellTituloAnexo);
            tableTituloAnexo.AddCell(cellTituloAnexolista);

            doc.Add(tableTituloAnexo);

            /* Agregar extra de la lista */
            // 2) Fuentes
            var fontHeader = FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD);

            // 3) Tabla de 5 columnas
            var table = new PdfPTable(5)
            {
                WidthPercentage = 100f,
                SpacingBefore = 5f,
                SpacingAfter = 5f
            };
            // (Opcional) Proporciones de columnas
            table.SetWidths(new float[] { 2, 2, 2, 2, 2 });

            // Bordes por defecto visibles
            table.DefaultCell.Border = Rectangle.BOX;
            table.DefaultCell.BorderWidth = 1f;
            table.DefaultCell.Padding = 5f;

            // 4) Encabezados (primera fila)
            string[] headers = { "APELLIDO", "NOMBRE", "ACTIVIDAD", "NACIONALIDAD", "PASAPORTE No." };
            foreach (var h in headers)
            {
                var th = new PdfPCell(new Phrase(h, fontHeader))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    BackgroundColor = BaseColor.LightGray,
                    Border = Rectangle.BOX,
                    BorderWidth = 1f,
                    Padding = 6f
                };
                table.AddCell(th);
            }
            // Indicar que la primera fila es encabezado
            table.HeaderRows = 1;
            // 5) Filas de datos (N filas)
            for (int i = 0; i < otrosInvitados.Count; i++)
            {
                table.AddCell(CreaNuevaCeldaTablaOtrosInvitados(otrosInvitados[i].Apellidos));
                table.AddCell(CreaNuevaCeldaTablaOtrosInvitados(otrosInvitados[i].Nombre));
                table.AddCell(CreaNuevaCeldaTablaOtrosInvitados(otrosInvitados[i].ActvidadEnMexico));
                table.AddCell(CreaNuevaCeldaTablaOtrosInvitados(otrosInvitados[i].Nacionalidad));
                table.AddCell(CreaNuevaCeldaTablaOtrosInvitados(otrosInvitados[i].NumPasaporte));
            }

            // 6) Agregar la tabla al documento
            doc.Add(table);
            #endregion


            #region FirmaAnexo
            if (signImageAnexo != null)
            {
                PdfPCell cellFirmaAnexo = null;
                
                var tableFirmaAnexo = new PdfPTable(new float[] { 100f });
                tableFirmaAnexo.WidthPercentage = 100;
                tableFirmaAnexo.SplitLate = false;

                var cellFirmaImagenEsp = new PdfPCell(signImageAnexo);
                cellFirmaImagenEsp.HorizontalAlignment = Element.ALIGN_CENTER;
                cellFirmaImagenEsp.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellFirmaImagenEsp.Border = Rectangle.NO_BORDER;
                cellFirmaImagenEsp.PaddingTop = 10;
                cellFirmaImagenEsp.PaddingBottom = 10;
                tableFirmaAnexo.AddCell(cellFirmaImagenEsp);



                var phraseFirmaEsp = new Phrase(seccionFirma, FuenteArial11);
                var cellFirmaEsp = new PdfPCell(phraseFirmaEsp);
                cellFirmaEsp.PaddingTop = 2f;
                cellFirmaEsp.PaddingBottom = 5f;
                cellFirmaEsp.Border = Rectangle.TOP_BORDER;
                cellFirmaEsp.BorderWidthTop = 1f;
                cellFirmaEsp.HorizontalAlignment = Element.ALIGN_CENTER;
                cellFirmaEsp.VerticalAlignment = Element.ALIGN_MIDDLE;
                
                tableFirmaAnexo.AddCell(cellFirmaEsp);

                doc.Add(tableFirmaAnexo);
            }
            #endregion


            //foreach (var footer in footerList)
            //{
            //    doc.Add(footer);
            //}
        }

        private PdfPCell CreaNuevaCeldaTablaOtrosInvitados(string phraseStr) {
            var fontCell = FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL);
            var cell = new PdfPCell(new Phrase(phraseStr, fontCell))
            {
                Border = Rectangle.BOX,
                BorderWidth = 1f,
                Padding = 5f,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            return cell;
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
                string firma = string.IsNullOrEmpty(regInvite.SIGN_BLOB) ? "" : regInvite.SIGN_BLOB;
                byte[] bytesImage = Convert.FromBase64String(firma);

                Image signImageEsp;
                using (MemoryStream ms = new MemoryStream(bytesImage))
                {
                    if (firma.Equals(""))
                    {
                        // Dimensiones de la imagen en blanco
                        int width = (int)ANCHO_IMAGEN_FIRMA;  // Ancho de la imagen
                        int height = (int)ALTO_IMAGEN_FIRMA; // Alto de la imagen
                        byte[] whiteImageData = new byte[width * height * 3]; // Imagen RGB en blanco
                        // Rellenar con blanco (255 en cada canal)
                        for (int i = 0; i < whiteImageData.Length; i++)
                        {
                            whiteImageData[i] = 255;
                        }
                        signImageEsp = Image.GetInstance(width, height, 3, 8, whiteImageData);
                    }
                    else
                    {
                        signImageEsp = Image.GetInstance(ms);
                    }
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
                    if (firma.Equals(""))
                    {
                        // Dimensiones de la imagen en blanco
                        int width = (int)ANCHO_IMAGEN_FIRMA;  // Ancho de la imagen
                        int height = (int)ALTO_IMAGEN_FIRMA; // Alto de la imagen
                        byte[] whiteImageData = new byte[width * height * 3]; // Imagen RGB en blanco
                        // Rellenar con blanco (255 en cada canal)
                        for (int i = 0; i < whiteImageData.Length; i++)
                        {
                            whiteImageData[i] = 255;
                        }
                        signImageEng = Image.GetInstance(width, height, 3, 8, whiteImageData);
                    }
                    else
                    {
                        signImageEng = Image.GetInstance(ms);
                    }
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

        private Phrase GeneraParrafoCeldaConEstilos(string texto, int tamanoFuente)
        {
            FontFactory.RegisterDirectories();

            string[] arregloParrafos = texto.Split('\n');
            var phraseResult = new Phrase();
            string textoAuxiliar = "";
            foreach (string parrafoActual in arregloParrafos)
            {
                textoAuxiliar = "\n" + parrafoActual;
                if (tamanoFuente == 11)
                {
                    if (parrafoActual.StartsWith("[RED]"))
                    {
                        phraseResult.Add(new Chunk(textoAuxiliar.Replace("[RED]", ""), FuenteArial11Roja));
                    }
                    else if (parrafoActual.StartsWith("[BOLD]"))
                    {
                        phraseResult.Add(new Chunk(textoAuxiliar.Replace("[BOLD]", ""), FuenteArial11Negrita));
                    }
                    else
                    {
                        phraseResult.Add(new Chunk(textoAuxiliar, FuenteArial11));
                    }
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

        private StringBuilder GeneraContenidoCeldaEspanOCESA(InviteDto regInvite, IReporteInfo infoEventosList, List<IOtroInvitadoModel> otrosArtistas = null, string nombrePrimerEvento="")
        {

            StringBuilder result = new StringBuilder();
            //var content = string.Format($"{regInvite.DESC_SPANISH}",  dataDocumentInsert);
            var content = ReemplazaBanderasPorContenidoPrincipalCeldasESP(infoEventosList,regInvite.DESC_SPANISH, otrosArtistas, nombrePrimerEvento);
            result.AppendLine(content);


            return result;
        }

  
        private StringBuilder GeneraContenidoCeldaEngOCESA(InviteDto regInvite, IReporteInfo infoEventosList, List<IOtroInvitadoModel> otrosArtistas = null)
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
                infoEventosList.NacionalidadEsp,
                infoEventosList.NacionalidadIng,
                infoEventosList.NumPasaporte,
                infoEventosList.PuestoParteStaff,
                infoEventosList.FechaEntradaAlPais,
                infoEventosList.FechaSalidaAlPais};

            StringBuilder result = new StringBuilder();
            var content = ReemplazaBanderasPorContenidoPrincipalCeldasENG(infoEventosList, regInvite.DESC_ENGLISH, otrosArtistas);
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
        private string ReemplazaBanderasPorContenidoPrincipalCeldasESP(IReporteInfo infoEvento, string texto = "", List<IOtroInvitadoModel> otrosArtistas = null, string nombrePrimerEvento = "")
        {
            //[NOMBRE_INVITADO]
            //[NACIONALIDAD]
            //[NUMERO_PASAPORTE]
            //[PUESTO_STAFF]
            //[FECHA_INGRESO_AL_PAIS]
            //[FECHA_SALIDA_DEL_PAIS]
            //[LISTA_DE_EVENTOS]
            //[LISTA_DE_OTROS_ARTISTAS]
            //[NOMBRE_PRIMER_EVENTO]

            if (otrosArtistas != null && otrosArtistas.Count > 0) {
                int artistasRecorridos = 0;
                int numArtistas = otrosArtistas.Count;
                string artistasConcatenados = " ARTISTA INVITADO";
                foreach (IOtroInvitadoModel evento in otrosArtistas)
                {
                    artistasRecorridos++;
                    artistasConcatenados += $" “{evento.Nombre}”";
                    if (artistasRecorridos < numArtistas)
                        artistasConcatenados += $",";
                    else
                        artistasConcatenados += $".";
                }
                texto = texto.Replace("[LISTA_DE_OTROS_ARTISTAS]", artistasConcatenados);
            }

            texto = texto.Replace("[NOMBRE_INVITADO]", infoEvento.NombreInvitado);
            texto = texto.Replace("[NACIONALIDAD]", infoEvento.NacionalidadEsp);
            texto = texto.Replace("[NUMERO_PASAPORTE]", infoEvento.NumPasaporte);
            texto = texto.Replace("[PUESTO_STAFF]", infoEvento.PuestoParteStaff);
            texto = texto.Replace("[FECHA_INGRESO_AL_PAIS]", infoEvento.FechaEntradaAlPais);
            texto = texto.Replace("[FECHA_SALIDA_DEL_PAIS]", infoEvento.FechaSalidaAlPais);
            texto = texto.Replace("[NOMBRE_PRIMER_EVENTO]", $"“{nombrePrimerEvento}”");


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

        private string GeneraTituloAnexo1OtrosInvitados(string texto = "", List<IOtroInvitadoModel> otrosArtistas = null, string nombrePrimerEvento="")
        {
            //[LISTA_DE_OTROS_ARTISTAS]
            //[NOMBRE_PRIMER_EVENTO]

            if (otrosArtistas != null && otrosArtistas.Count > 0)
            {
                int artistasRecorridos = 0;
                int numArtistas = otrosArtistas.Count;
                string artistasConcatenados = " ARTISTA INVITADO";
                foreach (IOtroInvitadoModel evento in otrosArtistas)
                {
                    artistasRecorridos++;
                    artistasConcatenados += $" “{evento.Nombre}”";
                    if (artistasRecorridos < numArtistas)
                        artistasConcatenados += $",";
                    else
                        artistasConcatenados += $".";
                }
                texto = texto.Replace("[LISTA_DE_OTROS_ARTISTAS]", artistasConcatenados);
            }

            texto = texto.Replace("[NOMBRE_PRIMER_EVENTO]", $"“{nombrePrimerEvento}”");

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

        private string ReemplazaBanderasPorContenidoPrincipalCeldasENG(IReporteInfo infoEvento, string texto = "", List<IOtroInvitadoModel> otrosArtistas = null)
        {
            //[NOMBRE_INVITADO]
            //[NACIONALIDAD]
            //[NUMERO_PASAPORTE]
            //[PUESTO_STAFF]
            //[FECHA_INGRESO_AL_PAIS]
            //[FECHA_SALIDA_DEL_PAIS]
            //[LISTA_DE_EVENTOS]
            //[LISTA_DE_OTROS_ARTISTAS]

            if (otrosArtistas != null && otrosArtistas.Count > 0)
            {
                int artistasRecorridos = 0;
                int numArtistas = otrosArtistas.Count;
                string artistasConcatenados = "INVITED ARTIST";
                foreach (IOtroInvitadoModel evento in otrosArtistas)
                {
                    artistasRecorridos++;
                    artistasConcatenados += $" “{evento.Nombre} {evento.Apellidos}”";
                    if (artistasRecorridos < numArtistas)
                        artistasConcatenados += $",";
                    else
                        artistasConcatenados += $".";
                }
                texto = texto.Replace("[LISTA_DE_OTROS_ARTISTAS]", artistasConcatenados);
            }

            texto = texto.Replace("[NOMBRE_INVITADO]", infoEvento.NombreInvitado);
            texto = texto.Replace("[NACIONALIDAD]", infoEvento.NacionalidadIng);
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
