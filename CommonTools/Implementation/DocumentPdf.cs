using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CommonTools.Implementation
{


    public static class DocumentPdf
    {
        
        public static byte[] DocumetPdf(string filename, byte[] arrayData) 
        {

            byte[] all;

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document();

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                doc.SetPageSize(PageSize.Letter);
                doc.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;

                PdfReader reader;
                //foreach (byte[] p in pdf)
                //{
                    reader = new PdfReader(arrayData);
                    int pages = reader.NumberOfPages;

                    // loop over document pages
                    for (int i = 1; i <= pages; i++)
                    {
                        doc.SetPageSize(PageSize.Letter);
                        doc.NewPage();
                        page = writer.GetImportedPage(reader, i);
                        cb.AddTemplate(page, 0, 0);
                    }
                //}

                doc.Close();
                all = ms.GetBuffer();
                ms.Flush();
                ms.Dispose();

            }
            return all;
        }

    }
}
