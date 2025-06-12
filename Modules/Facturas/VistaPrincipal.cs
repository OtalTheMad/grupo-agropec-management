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

        //public void GenerarPDF()
        //{
        //    if (dgvRecibos.CurrentRow == null)
        //    {
        //        MessageBox.Show("Selecciona una factura primero.");
        //        return;
        //    }

        //    int idRecibo = Convert.ToInt32(dgvRecibos.CurrentRow.Cells["IDRecibo"].Value);
        //    var recibo = ObtenerReciboPorID(idRecibo);
        //    var detalles = ObtenerDetallesPorRecibo(idRecibo);

        //    if (recibo != null && detalles.Any())
        //        GenerarPDF(recibo, detalles);
        //    else
        //        MessageBox.Show("No se pudo generar el PDF. Verifica los datos del recibo.");
        //}

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

        //private void btnVerDetalle_Click(object sender, EventArgs e)
        //{
        //    if (dgvRecibos.SelectedRows.Count > 0)
        //    {
        //        int idRecibo = Convert.ToInt32(dgvRecibos.SelectedRows[0].Cells["IDRecibo"].Value);
        //        //VistaDetalle detalleForm = new VistaDetalle(idRecibo);
        //        detalleForm.ShowDialog();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Por favor, seleccione un recibo para ver los detalles.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}

        //private void btnGenerarFactura_Click(object sender, EventArgs e)
        //{

        //}
    }
}
