using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Collections.Generic;
using System.IO;

namespace MS.AFORO255.Sale.Services
{
    public class PdfService
    {
        public byte[] GenerateSalesReportPdf(IEnumerable<dynamic> sales)
        {
            using var stream = new MemoryStream();
            var document = new Document(PageSize.A4, 25, 25, 30, 30);
            var writer = PdfWriter.GetInstance(document, stream);

            document.Open();

            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

            // Título del PDF
            var title = new Paragraph("Reporte de Ventas", titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20
            };
            document.Add(title);

            // Tabla de datos
            var table = new PdfPTable(3)
            {
                WidthPercentage = 100
            };

            table.AddCell(new Phrase("Cliente", regularFont));
            table.AddCell(new Phrase("Fecha de Venta", regularFont));
            table.AddCell(new Phrase("Monto Total", regularFont));

            foreach (var sale in sales)
            {
                table.AddCell(new Phrase(sale.CustomerName, regularFont));
                table.AddCell(new Phrase(sale.SaleDate.ToString("yyyy-MM-dd"), regularFont));
                table.AddCell(new Phrase($"{sale.TotalAmount:C}", regularFont));
            }

            document.Add(table);
            document.Close();

            return stream.ToArray();
        }
    }
}
