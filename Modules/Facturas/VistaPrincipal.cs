using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using ProyectoIsis.Data;
using ProyectoIsis.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows.Forms;


namespace ProyectoIsis.Modules.Facturas
{
    public partial class VistaPrincipal : Form
    {
        Efectos efectos = new Efectos();
        public VistaPrincipal()
        {
            InitializeComponent();
        }

        private void FormatoDGV()
        {
            try
            {
                dgvRecibos.Columns[0].HeaderText = "ID Recibo";
                dgvRecibos.Columns[1].HeaderText = "Cliente";
                dgvRecibos.Columns[2].HeaderText = "Creado En";
                dgvRecibos.Columns[3].HeaderText = "Cantidad Vendida";
                // Ajustar el ancho de las columnas
                dgvRecibos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvRecibos.AllowUserToAddRows = false;
                dgvRecibos.AllowUserToDeleteRows = false;
                dgvRecibos.AllowUserToResizeColumns = true;
                dgvRecibos.AllowUserToResizeRows = false;
                dgvRecibos.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al dar formato a el DataGridView: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarRecibos()
        {
            using (var conn = dbConexion.ObtenerConexion())
            {
                string query =
                        @"SELECT
                           IDRecibo,
                           NombreCliente,
                           CreadoEn,
                           CantidadTotal
                        FROM Recibos";

                using (var cmd = new SQLiteCommand(query, conn))
                using (var adapter = new SQLiteDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvRecibos.DataSource = dt;
                    dgvRecibos.ClearSelection();
                }
            }
        }

        private void btnVerDetalle_Click(object sender, EventArgs e)
        {
            if (dgvRecibos.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un recibo primero.", "Ver Detalle", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idRecibo = Convert.ToInt32(dgvRecibos.CurrentRow.Cells["IDRecibo"].Value);

            DetalleFactura detalle = new DetalleFactura(idRecibo);
            detalle.ShowDialog();
        }

        private void VistaPrincipal_Load(object sender, EventArgs e)
        {
            CargarRecibos();
            FormatoDGV();
            efectos.AplicarFormatoBoton(this.btnVerDetalle, "#A8D5BA", "#3c3c3c");
        }
    }
}
