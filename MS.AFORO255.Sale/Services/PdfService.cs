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

            // Definir BaseFont y Font
            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            var titleFont = new Font(baseFont, 16, Font.BOLD, new BaseColor(0, 0, 0)); // Negro
            var regularFont = new Font(baseFont, 12, Font.NORMAL, new BaseColor(0, 0, 0)); // Negro

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

            // Añadir encabezado con color de fondo
            var cellStyle = new Font(baseFont, 12, Font.BOLD, new BaseColor(255, 255, 255)); // Blanco
            var headerCell = new PdfPCell(new Phrase("Cliente", cellStyle))
            {
                BackgroundColor = new BaseColor(169, 169, 169), // Gris
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(headerCell);

            headerCell = new PdfPCell(new Phrase("Fecha de Venta", cellStyle))
            {
                BackgroundColor = new BaseColor(169, 169, 169), // Gris
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(headerCell);

            headerCell = new PdfPCell(new Phrase("Monto Total", cellStyle))
            {
                BackgroundColor = new BaseColor(169, 169, 169), // Gris
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(headerCell);

            // Añadir filas con los datos
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
