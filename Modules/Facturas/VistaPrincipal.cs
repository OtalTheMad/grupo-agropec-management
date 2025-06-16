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

        private void CargarRecibos()
        {
            using (var conn = dbConexion.ObtenerConexion())
            {
                string query =
                        @"SELECT
                           IDRecibo AS 'ID Recibo',
                           NombreCliente AS 'Cliente',
                           CreadoEn AS 'Fecha de Creación',
                           CantidadTotal AS 'Total'
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

            int idRecibo = Convert.ToInt32(dgvRecibos.CurrentRow.Cells["ID Recibo"].Value);

            DetalleFactura detalle = new DetalleFactura(idRecibo);
            detalle.ShowDialog();
        }

        private void VistaPrincipal_Load(object sender, EventArgs e)
        {
            CargarRecibos();
            efectos.AplicarFormatoBoton(this.btnVerDetalle, "#A8D5BA", "#3c3c3c");
        }
    }
}
