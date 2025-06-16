using ProyectoIsis.Data;
using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using ProyectoIsis.Utilities;

namespace ProyectoIsis.Modules
{
    public partial class VistaProducto : Form
    {
        public VistaProducto()
        {
            InitializeComponent();
        }

        public void CargarProductos()
        {
            using (var conn = dbConexion.ObtenerConexion())
            {
                if (conn == null)
                {
                    MessageBox.Show("No se pudo conectar a la base de datos.", "Error de Conexión");
                    return;
                }

                string query = "SELECT * FROM Productos";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var adapter = new SQLiteDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvProductos.DataSource = dt;

                    // Elimina selección previa
                    dgvProductos.ClearSelection();

                    lblFilas.Text = $"Total de productos: {dt.Rows.Count}";
                }
            }
        }

        private void FormatoDGV()
        {
            dgvProductos.Columns[0].Visible = false; // Ocultar columna Id
            dgvProductos.Columns[1].HeaderText = "Nombre";
            dgvProductos.Columns[2].HeaderText = "Descripcion";
            dgvProductos.Columns[3].HeaderText = "Precio";
            dgvProductos.Columns[4].HeaderText = "Existencias";
            dgvProductos.Columns[5].HeaderText = "Creado En";
            // Ajustar el ancho de las columnas
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.AllowUserToDeleteRows = false;
            dgvProductos.ReadOnly = true;
        }

        private void VistaProducto_Load(object sender, EventArgs e)
        {
            CargarProductos();
            FormatoDGV();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {

            string searchText = txtBuscar.Text;

            using (var conn = dbConexion.ObtenerConexion())
            {
                string query = "SELECT * FROM Productos WHERE Nombre LIKE @search";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvProductos.DataSource = dt;
                    lblFilas.Text = $"Total de productos: {dt.Rows.Count}";
                }
            }
        }
    }
}
