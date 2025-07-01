﻿using ProyectoIsis.Data;
using ProyectoIsis.Products;
using ProyectoIsis.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PdfiumViewer;
using System.Drawing.Printing;

namespace ProyectoIsis.Modules
{
    public partial class Ventas : Form
    {
        Efectos efectos = new Efectos();
        public Ventas()
        {
            InitializeComponent();
            
        }

        public class ItemVenta
        {
            public int IDProducto { get; set; }
            public string Nombre { get; set; }
            public decimal Precio { get; set; }
            public int Cantidad { get; set; }
            public decimal Subtotal => Precio * Cantidad;
            public int Impuesto { get; set; }
            }


        #region queries
        public List<Producto> ObtenerProductos()
        {
            var productos = new List<Producto>();

            using (var conn = dbConexion.ObtenerConexion())
            {
                string query = "SELECT IDProducto, Nombre, PrecioVenta, CantidadStock, ISV FROM Productos ORDER BY Nombre";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        
                        productos.Add(new Producto
                        {
                            IDProducto = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Precio = Convert.ToDecimal(reader.GetValue(2)),
                            CantidadStock = reader.GetInt32(3),
                            Impuesto = reader.GetInt32(4)
                        });
                    }
                }
            }

            return productos;
        }

        private void RestarStock(int idProducto, int cantidad)
        {
            using (var conn = dbConexion.ObtenerConexion())
            {
                string query = "UPDATE Productos SET CantidadStock = CantidadStock - @cantidad WHERE IDProducto = @idProducto";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@idProducto", idProducto);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void ActualizarTotal()
        {
            decimal total = listaVenta.Sum(p => p.Subtotal);
            lblTotal.Text = $"Total: L. {total:N2}";
        }

        private void LimpiarVenta()
        {
            listaVenta.Clear();
            dgvVenta.Rows.Clear();
            txtCliente.Clear();
            nUpCantidad.Value = 0;
            lblTotal.Text = "Total: L. 0.00";
        }

        #endregion

        #region metodos

        private void CargarProductos()
        {
            List<Producto> productos = ObtenerProductos();
            cbProducto.DataSource = productos;
            cbProducto.DisplayMember = "Nombre";
            cbProducto.ValueMember = "IDProducto";
            cbProducto.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private bool ValidarVenta()
        {
            if (listaVenta.Count == 0)
            {
                MessageBox.Show("No hay productos en la venta.", "Error de Venta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        public void ImprimirFacturaPDF(string rutaArchivoPDF)
        {
            using (var document = PdfDocument.Load(rutaArchivoPDF))
            {
                using (PrintDialog printDialog = new PrintDialog())
                {
                    printDialog.AllowSomePages = true;
                    printDialog.AllowSelection = false;
                    printDialog.UseEXDialog = true;
                    printDialog.PrinterSettings = new PrinterSettings();

                    if (printDialog.ShowDialog() == DialogResult.OK)
                    {
                        using (var printDoc = document.CreatePrintDocument())
                        {
                            printDoc.PrinterSettings = printDialog.PrinterSettings;
                            printDoc.PrintController = new StandardPrintController();
                            printDoc.Print();
                        }
                    }
                }
            }
        }

        public int InsertarRecibo(string nombreCliente, string creadoPor, int cantidadTotal)
        {
            using (var conn = dbConexion.ObtenerConexion())
            {
                string query = "INSERT INTO Recibos (NombreCliente, CreadoPor, CreadoEn, CantidadTotal) VALUES (@nombreCliente, @creadoPor, @creadoEn, @cantidad)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombreCliente", nombreCliente);
                    cmd.Parameters.AddWithValue("@creadoPor", creadoPor);
                    cmd.Parameters.AddWithValue("@creadoEn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@cantidad", cantidadTotal);
                    cmd.ExecuteNonQuery();

                    return (int)conn.LastInsertRowId;
                }
            }
        }
        private void InsertarDetalleRecibo(int idRecibo, int idProducto, int cantidad, decimal precioUnidad, decimal subtotal)
        {
            using (var conn = dbConexion.ObtenerConexion())
            {
                string query = "INSERT INTO DetalleRecibos (IDRecibo, IDProducto, Cantidad, PrecioPorUnidad, Subtotal) " +
                               "VALUES (@idRecibo, @idProducto, @cantidad, @precio, @subtotal)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idRecibo", idRecibo);
                    cmd.Parameters.AddWithValue("@idProducto", idProducto);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@precio", precioUnidad);
                    cmd.Parameters.AddWithValue("@subtotal", subtotal);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion

        private List<ItemVenta> listaVenta = new List<ItemVenta>();
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombreCliente;
            Producto producto = (Producto)cbProducto.SelectedItem;

            if (cbProducto.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecciona un producto.", "Error de Selección", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtCliente == null || string.IsNullOrWhiteSpace(txtCliente.Text))
            {
                txtCliente.Text = "CLIENTE";
            }

            nombreCliente = txtCliente.Text.Trim();
            int cantidad = (int)nUpCantidad.Value;
            if (cantidad <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor a cero.", "Error de Cantidad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cantidad > producto.CantidadStock)
            {
                MessageBox.Show("Cantidad insuficiente en existencias.", "Error de Existencias", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var item = new ItemVenta
            {
                IDProducto = producto.IDProducto,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Cantidad = cantidad,
                Impuesto = producto.Impuesto
            };

            listaVenta.Add(item);

            dgvVenta.Rows.Add(item.Nombre, item.Precio.ToString("N2"), item.Cantidad, item.Subtotal.ToString("N2"), item.Impuesto.ToString("N2"));
            ActualizarTotal();
        }
        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (dgvVenta.CurrentRow == null || dgvVenta.CurrentRow.Index < 0)
            {
                MessageBox.Show("Selecciona un producto para quitar.", "Quitar Producto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            string nombreProducto = dgvVenta.CurrentRow.Cells[0].Value.ToString();

            ItemVenta itemAEliminar = listaVenta.FirstOrDefault(i => i.Nombre == nombreProducto);
            if (itemAEliminar != null)
            {
                listaVenta.Remove(itemAEliminar);
            }

            dgvVenta.Rows.RemoveAt(dgvVenta.CurrentRow.Index);

            ActualizarTotal();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (ValidarVenta() == false)
            {
                return;
            }

            FacturaPDF facturaPDF = new FacturaPDF();
            string nombreArchivo = $"Factura_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string rutaPDF = Path.Combine(Application.StartupPath, nombreArchivo);

            try
            {
                facturaPDF.GenerarFactura(txtCliente.Text, DateTime.Now, listaVenta, listaVenta.Sum(p => p.Subtotal), rutaPDF);
                System.Diagnostics.Process.Start("explorer.exe", rutaPDF);
                foreach (var item in listaVenta)
                {
                    RestarStock(item.IDProducto, item.Cantidad);
                }

                MessageBox.Show("Venta confirmada con éxito.", "Venta Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ImprimirFacturaPDF(rutaPDF);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar la factura: {ex.Message}", "Error de Factura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int _cantidadTotal = listaVenta.Sum(p => p.Cantidad);
                int idRecibo = InsertarRecibo(txtCliente.Text, ObtenerUsuarioLogueado.Usuario, _cantidadTotal);
                
                foreach (var item in listaVenta)
                {
                    InsertarDetalleRecibo(idRecibo, item.IDProducto, item.Cantidad, item.Precio, item.Subtotal);
                }
            }
            catch (Exception facturaException)
            {
                MessageBox.Show($"Error al insertar el recibo: {facturaException.Message}", "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

            LimpiarVenta();
        }

        private void Ventas_Load(object sender, EventArgs e)
        {
            CargarProductos();
            efectos.AplicarHover(this.btnAgregar, "#A8D5BA", "#95C8A3", "#3c3c3c");
            efectos.AplicarHover(this.btnQuitar, "#A8D5BA", "#95C8A3", "#3c3c3c");
            efectos.AplicarHover(this.btnConfirmar, "#A8D5BA", "#95C8A3", "#3c3c3c");
            efectos.AplicarHover(this.btnCancelar, "#A8D5BA", "#95C8A3", "#3c3c3c");
            efectos.AplicarFormatoBoton(this.btnAgregar, "#A8D5BA", "#3c3c3c");
            efectos.AplicarFormatoBoton(this.btnQuitar, "#A8D5BA", "#3c3c3c");
            efectos.AplicarFormatoBoton(this.btnConfirmar, "#A8D5BA", "#3c3c3c");
            efectos.AplicarFormatoBoton(this.btnCancelar, "#A8D5BA", "#3c3c3c");
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbProducto.SelectedValue is int idProducto)
            {
                using (var conn = dbConexion.ObtenerConexion())
                {
                    string query = "SELECT CantidadStock FROM Productos WHERE IDProducto = @idProducto";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idProducto", idProducto);
                        var cantidadStock = cmd.ExecuteScalar();
                        if (cantidadStock != null)
                        {
                            nUpCantidad.Maximum = Convert.ToInt32(cantidadStock);
                        }
                        else
                        {
                            nUpCantidad.Maximum = 0;
                        }
                    }
                }
            }
        }
    }
}
