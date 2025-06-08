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
                    SELECT SUM(CantidadTotal) 
                    FROM Recibos 
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

        private void btnCalcularBalance_Click(object sender, EventArgs e)
        {
            CalcularBalance();
        }
    }
}
