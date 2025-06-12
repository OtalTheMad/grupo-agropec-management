using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using System;
using System.Collections.Generic;
using iText.Layout;
using static ProyectoIsis.Modules.Ventas;
using System.IO;
using System.Windows.Forms;

namespace ProyectoIsis.Utilities
{
    public  class FacturaPDF
    {
        public void GenerarFactura(string cliente, DateTime fecha, List<ItemVenta> items, decimal total, string rutaArchivo)
        {
            PdfWriter writer = new PdfWriter(rutaArchivo, new WriterProperties());
            PdfDocument pdf = new PdfDocument(writer);
            Document doc = new Document(pdf);
            string rutaLogo = Path.Combine(Application.StartupPath, "Resources", "Logos", "logo_serviciosAgropec.jpeg");

            if (File.Exists(rutaLogo))
            {
                ImageData imageData = ImageDataFactory.Create(rutaLogo);
                Image logo = new Image(imageData);
                logo.ScaleToFit(100, 100);
                logo.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                doc.Add(logo);
            }

            Paragraph titulo = new Paragraph("FACTURA")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(20);
            doc.Add(titulo);

            Paragraph parCliente = new Paragraph("Cliente: " + cliente);
            Paragraph parFecha = new Paragraph("Fecha: " + fecha.ToString("dd/MM/yyyy HH:mm"));
            doc.Add(parCliente);
            doc.Add(parFecha);

            Table tabla = new Table(4).UseAllAvailableWidth();
            tabla.AddHeaderCell("Producto");
            tabla.AddHeaderCell("Precio");
            tabla.AddHeaderCell("Cantidad");
            tabla.AddHeaderCell("Subtotal");

            for (int i = 0; i < items.Count; i++)
            {
                ItemVenta item = items[i];
                tabla.AddCell(item.Nombre);
                tabla.AddCell("L. " + item.Precio.ToString("N2"));
                tabla.AddCell(item.Cantidad.ToString());
                tabla.AddCell("L. " + item.Subtotal.ToString("N2"));
            }

            doc.Add(tabla);

            Paragraph parTotal = new Paragraph("TOTAL: L. " + total.ToString("N2"))
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontSize(14);

            doc.Add(parTotal);
            doc.Close();
        }
    }
}
