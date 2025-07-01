using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using static ProyectoIsis.Modules.Ventas;

namespace ProyectoIsis.Utilities
{
    public static class StreamExtensions
    {
        public static byte[] ReadAllBytes(this Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }

    public class FacturaPDF
    {
        string nombreEmpresa = "Servicios Agropec";
        string mottoEmpresa = "Sembramos Confianza, Cosechamos Progreso";
        string direccionEmpresa = "Barrio Tatumbla, El Negrito, Yoro";
        string telefonoEmpresa = "Tel: 9805-9036 - RTN: 18031999003659";
        string redesEmpresa = "Gmail: serviciosagropec@gmail.com - Facebook: Servicios Agropec";
        string usuario = ObtenerUsuarioLogueado.Usuario;
        public void GenerarFactura(string cliente, DateTime fecha, List<ItemVenta> items, decimal total, string rutaArchivo)
        {
            PdfWriter writer = new PdfWriter(rutaArchivo, new WriterProperties());
            PdfDocument pdf = new PdfDocument(writer);
            Document doc = new Document(pdf);
            Table tablaPrincipal = new Table(2).UseAllAvailableWidth();
            tablaPrincipal.SetBorder(iText.Layout.Borders.Border.NO_BORDER);

            string resourceName = "ProyectoIsis.Resources.Logos.serviciosAgropec_mainlogo.png";
            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    ImageData imageData = ImageDataFactory.Create(stream.ReadAllBytes());
                    Image logo = new Image(imageData);
                    logo.ScaleToFit(100, 100);
                    logo.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.LEFT);
                    Cell cellLogo = new Cell().Add(logo)
                        .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE);
                    tablaPrincipal.AddCell(cellLogo);
                }
                else
                {
                    tablaPrincipal.AddCell(new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                }
            }

            Table tablaInfo = new Table(1);
            tablaInfo.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
            tablaInfo.SetWidth(UnitValue.CreatePercentValue(100));
            tablaInfo.AddCell(new Cell().Add(new Paragraph(nombreEmpresa).SetFontSize(9)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
            tablaInfo.AddCell(new Cell().Add(new Paragraph(mottoEmpresa).SetFontSize(8)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
            tablaInfo.AddCell(new Cell().Add(new Paragraph(direccionEmpresa).SetFontSize(8)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
            tablaInfo.AddCell(new Cell().Add(new Paragraph(telefonoEmpresa).SetFontSize(8)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
            tablaInfo.AddCell(new Cell().Add(new Paragraph(redesEmpresa).SetFontSize(8)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));

            Cell cellInfo = new Cell().Add(tablaInfo)
                .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE);
            tablaPrincipal.AddCell(cellInfo);

            doc.Add(tablaPrincipal);


            //Cuerpo de la factura

            doc.Add(new Paragraph("\n\n"));
            Paragraph parCliente = new Paragraph("Cliente: " + cliente);
            Paragraph vendedor = new Paragraph("Vendedor: " + usuario);
            Paragraph parFecha = new Paragraph("Fecha: " + fecha.ToString("dd/MM/yyyy HH:mm"));

            Table tablaPrincipalCuerpo = new Table(3).UseAllAvailableWidth();
            tablaPrincipalCuerpo.SetBorder(iText.Layout.Borders.Border.NO_BORDER);

            tablaPrincipalCuerpo.AddCell(new Cell().Add(parCliente.SetFontSize(9)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
            tablaPrincipalCuerpo.AddCell(new Cell().Add(vendedor.SetFontSize(9)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
            tablaPrincipalCuerpo.AddCell(new Cell().Add(parFecha.SetFontSize(9)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));

            doc.Add(tablaPrincipalCuerpo);


            Table tabla = new Table(5).UseAllAvailableWidth();
            tabla.SetBorder(iText.Layout.Borders.Border.NO_BORDER);

            tabla.AddHeaderCell(new Cell().Add(new Paragraph("Producto").SetFontSize(9).SimulateBold()).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
            tabla.AddHeaderCell(new Cell().Add(new Paragraph("Precio").SetFontSize(9).SimulateBold()).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
            tabla.AddHeaderCell(new Cell().Add(new Paragraph("Cantidad").SetFontSize(9).SimulateBold()).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
            tabla.AddHeaderCell(new Cell().Add(new Paragraph("Subtotal").SetFontSize(9).SimulateBold()).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
            tabla.AddHeaderCell(new Cell().Add(new Paragraph("ISV").SetFontSize(9).SimulateBold()).SetBorder(iText.Layout.Borders.Border.NO_BORDER));

            foreach (var item in items)
            {
                tabla.AddCell(new Cell().Add(new Paragraph(item.Nombre).SetFontSize(9)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                tabla.AddCell(new Cell().Add(new Paragraph("L. " + item.Precio.ToString("N2")).SetFontSize(9)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                tabla.AddCell(new Cell().Add(new Paragraph(item.Cantidad.ToString()).SetFontSize(9)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                tabla.AddCell(new Cell().Add(new Paragraph("L. " + item.Subtotal.ToString("N2")).SetFontSize(9)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                tabla.AddCell(new Cell().Add(new Paragraph(item.Impuesto.ToString()).SetFontSize(9)).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
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
