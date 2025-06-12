using ProyectoIsis.Data;
using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace ProyectoIsis.Modules
{
    public partial class VistaBalance : Form
    {
        public VistaBalance()
        {
            InitializeComponent();
        }

        private void CalcularBalance()
        {
            using (var conn = dbConexion.ObtenerConexion())
            {
                if (conn == null)
                {
                    MessageBox.Show("No se pudo conectar a la base de datos.", "Error de Conexión");
                    return;
                }
                string query = @"
                    SELECT IDRecibo, CreadoEn, NombreCliente, Ingreso, TotalVenta, CostoEstimado, Ganancia
                    FROM VistaBalance;
                    WHERE DATE(CreadoEn) BETWEEN @Inicio AND @Fin;";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Inicio", dtpInicio.Value.Date);
                    cmd.Parameters.AddWithValue("@Fin", dtpFin.Value.Date.AddDays(1).AddTicks(-1)); // Incluye todo el día final

                    using (var adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvBalance.DataSource = dt;

                        dgvBalance.ClearSelection(); // Limpia selección
                    }
                }
            }
        }
        private void FormatoDGV()
        {
            try
            {
                dgvBalance.Columns[0].HeaderText = "ID Recibo";
                dgvBalance.Columns[1].HeaderText = "Creado En";
                dgvBalance.Columns[2].HeaderText = "Nombre del Cliente";
                dgvBalance.Columns[3].HeaderText = "Ingreso";
                dgvBalance.Columns[4].HeaderText = "Total Venta";
                dgvBalance.Columns[5].HeaderText = "Costo Estimado";
                dgvBalance.Columns[6].HeaderText = "Ganancia";
                // Ajustar el ancho de las columnas
                dgvBalance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvBalance.AllowUserToAddRows = false;
                dgvBalance.AllowUserToDeleteRows = false;
                dgvBalance.AllowUserToResizeColumns = true;
                dgvBalance.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al dar formato a el DataGridView: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCalcularBalance_Click(object sender, EventArgs e)
        {
            CalcularBalance();
            FormatoDGV();
        }

    }
}
