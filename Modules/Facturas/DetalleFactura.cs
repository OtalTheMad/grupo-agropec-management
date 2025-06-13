using ProyectoIsis.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoIsis.Modules.Facturas
{
    public partial class DetalleFactura : Form
    {
        private int idRecibo;

        public DetalleFactura(int idRecibo)
        {
            InitializeComponent();
            this.idRecibo = idRecibo;
        }

        private void DetalleFactura_Load(object sender, EventArgs e)
        {
            CargarDetalle();
        }
        private void CargarDetalle()
        {
            using (var conn = dbConexion.ObtenerConexion())
            {
                // Cargar productos
                string query = @"
                    SELECT P.Nombre AS Producto, D.Cantidad, D.PrecioPorUnidad AS Precio, D.Subtotal
                    FROM DetalleRecibos D
                    INNER JOIN Productos P ON D.IDProducto = P.IDProducto
                    WHERE D.IDRecibo = @idRecibo";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idRecibo", idRecibo);
                    using (var adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvDetalle.DataSource = dt;
                    }
                }

                // Cargar cliente y fecha
                string queryCliente = "SELECT NombreCliente, CreadoEn FROM Recibos WHERE IDRecibo = @idRecibo";
                using (var cmd = new SQLiteCommand(queryCliente, conn))
                {
                    cmd.Parameters.AddWithValue("@idRecibo", idRecibo);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblCliente.Text = "Cliente: " + reader.GetString(0);
                            lblFecha.Text = "Fecha: " + reader.GetDateTime(1).ToString("dd/MM/yyyy HH:mm");
                        }
                    }
                }
            }
        }
    }
}
