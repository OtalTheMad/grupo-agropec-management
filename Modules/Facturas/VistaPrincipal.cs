using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using ProyectoIsis.Data;
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
        public VistaPrincipal()
        {
            InitializeComponent();
        }

        private void CargarRecibos()
        {
            using (var conn = dbConexion.ObtenerConexion())
            {
                string query = "SELECT IDRecibo, NombreCliente, CreadoEn, CantidadTotal FROM Recibos";

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

            // Suponiendo que tenés una columna "IDRecibo" visible o accesible
            int idRecibo = Convert.ToInt32(dgvRecibos.CurrentRow.Cells["IDRecibo"].Value);

            DetalleFactura detalle = new DetalleFactura(idRecibo);
            detalle.ShowDialog();
        }

        private void VistaPrincipal_Load(object sender, EventArgs e)
        {
            CargarRecibos();
        }
    }
}
