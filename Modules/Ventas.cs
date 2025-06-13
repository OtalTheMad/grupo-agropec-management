using ProyectoIsis.Data;
using ProyectoIsis.Products;
using ProyectoIsis.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ProyectoIsis.Modules
{
    public partial class Ventas : Form
    {
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
        }


        #region queries
        public List<Producto> ObtenerProductos()
        {
            var productos = new List<Producto>();

            using (var conn = dbConexion.ObtenerConexion())
            {
                string query = "SELECT IDProducto, Nombre, Precio, CantidadStock FROM Productos ORDER BY Nombre";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productos.Add(new Producto
                        {
                            IDProducto = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Precio = reader.GetDecimal(2),
                            CantidadStock = reader.GetInt32(3)
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

        public int InsertarRecibo(string nombreCliente, string creadoPor, int cantidadTotal)
        {
            using (var conn = dbConexion.ObtenerConexion())
            {
                string query = "INSERT INTO Recibos (NombreCliente, CreadoPor, CreadoEn, CantidadTotal) VALUES (@nombreCliente, @creadoPor, datetime('now'), @cantidad)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombreCliente", nombreCliente);
                    cmd.Parameters.AddWithValue("@creadoPor", creadoPor);
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
                Cantidad = cantidad
            };

            listaVenta.Add(item);

            dgvVenta.Rows.Add(item.Nombre, item.Precio.ToString("N2"), item.Cantidad, item.Subtotal.ToString("N2"));
            ActualizarTotal();
        }
        #endregion

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
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
