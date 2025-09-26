using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CommonTools.Pdf
{
    public class PdfFooter : PdfPageEventHelper
    {
        public string TextoFooter { get; set; } 
        // Fuente para el footer
        Font font = FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.ITALIC, BaseColor.Gray);

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PdfPTable footerTbl = new PdfPTable(1);
            footerTbl.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            // Texto del footer
            string texto = string.IsNullOrEmpty(TextoFooter)
            ? "Footer por defecto"
            : TextoFooter;
            texto += $"\n{writer.PageNumber}";
            PdfPCell cell = new PdfPCell(new Phrase(texto, font));
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;

            footerTbl.AddCell(cell);

            // Posicionar el footer
            footerTbl.WriteSelectedRows(
                0, -1,
                document.LeftMargin,
                document.BottomMargin -5,  // posición Y
                writer.DirectContent
            );
        }
    }
}
