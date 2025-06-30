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

        private void FormatoDGV()
        {
            try
            {
                dgvDetalle.Columns[0].HeaderText = "Nombre";
                dgvDetalle.Columns[1].HeaderText = "Cantidad";
                dgvDetalle.Columns[2].HeaderText = "Precio de Venta";
                // Ajustar el ancho de las columnas
                dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvDetalle.AllowUserToAddRows = false;
                dgvDetalle.AllowUserToDeleteRows = false;
                dgvDetalle.AllowUserToResizeColumns = true;
                dgvDetalle.AllowUserToResizeRows = false;
                dgvDetalle.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al dar formato a el DataGridView: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DetalleFactura_Load(object sender, EventArgs e)
        {
            CargarDetalle();
            FormatoDGV();
        }
        private void CargarDetalle()
        {
            using (var conn = dbConexion.ObtenerConexion())
            {
                // Cargar productos
                string query = @"
                    SELECT P.Nombre, D.Cantidad, D.PrecioPorUnidad, D.Subtotal
                    FROM DetalleRecibos D
                    INNER JOIN Productos P ON D.IDProducto = P.IDProducto
                    WHERE D.IDRecibo = @idRecibo
                    GROUP BY P.Nombre
                    ORDER BY D.Cantidad";

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

        private void dgvDetalle_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string colName = dgvDetalle.Columns[e.ColumnIndex].Name;

            switch (colName)
            {
                case "PrecioPorUnidad":
                case "Subtotal":
                    if (e.Value != null && e.Value != DBNull.Value)
                    {
                        decimal valor;
                        if (e.Value is decimal dec)
                            valor = dec;
                        else if (e.Value is double dbl)
                            valor = Convert.ToDecimal(dbl);
                        else if (decimal.TryParse(e.Value.ToString(), out var parsed))
                            valor = parsed;
                        else
                            return;

                        e.Value = $"L. {valor:N2}";
                        e.FormattingApplied = true;
                    }
                    break;
                case "Cantidad":
                    if (e.Value != null && e.Value != DBNull.Value)
                    {
                        if (int.TryParse(e.Value.ToString(), out int cantidad))
                        {
                            e.Value = cantidad.ToString("N0");
                            e.FormattingApplied = true;
                        }
                    }
                    break;
            }
        }
    }
}
