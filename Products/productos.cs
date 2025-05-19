        using System;
        using System.Collections.Generic;
        using System.ComponentModel;
        using System.Data;
        using System.Drawing;
        using System.Linq;
        using System.Text;
        using System.Threading.Tasks;
        using System.Windows.Forms;
        using System.Data.SQLite;
        using System.Windows.Forms;
        using ProyectoIsis.Data;


namespace ProyectoIsis.Products
{
    public partial class productos : Form
    {
        public productos()
        {
            InitializeComponent();
            CargarProductos();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.SelectedRows.Count > 0)
            {
                var row = dataGridView1.SelectedRows[0];
                txtNombre.Text = row.Cells["NombreProducto"].Value.ToString();
                txtDescripcion.Text = row.Cells["Descripcion"].Value.ToString();
                txtPrecio.Text = row.Cells["PrecioPorUnidad"].Value.ToString();
                txtExistencia.Text = row.Cells["Existencias"].Value.ToString();
            }
        }

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            if (RegistrarProducto())
                CargarProductos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            EliminarProducto();
            CargarProductos();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            ActualizarProducto();
            CargarProductos();
        }

        private void btnVolver_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtPrecio_TextChanged_1(object sender, EventArgs e)
        {
        }

        private void txtDescripcion_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtExistencia_TextChanged(object sender, EventArgs e)
        {
        }

        private bool RegistrarProducto()
        {
            using (var conn = dbConexion.ObtenerConexion())
            {
                if (conn == null)
                {
                    MessageBox.Show("No se pudo conectar a la base de datos.", "Error de Conexión");
                    return false;
                }

                string query = @"
                    INSERT INTO Productos (NombreProducto, Descripcion, PrecioPorUnidad, Existencias)
                    VALUES (@nombre, @descripcion, @precio, @existencias);";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text.Trim());
                    cmd.Parameters.AddWithValue("@precio", decimal.TryParse(txtPrecio.Text, out var precio) ? precio : 0);
                    cmd.Parameters.AddWithValue("@existencias", int.TryParse(txtExistencia.Text, out var existencias) ? existencias : 0);

                    try
                    {
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("✅ Producto registrado correctamente.");
                            LimpiarCampos();
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("⚠️ No se registró ningún producto.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ Error al registrar el producto:\n{ex.Message}");
                    }
                }
            }
            return false;
        }

        private void ActualizarProducto()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para actualizar.");
                return;
            }

            var id = dataGridView1.SelectedRows[0].Cells["IDProducto"].Value;

            using (var conn = dbConexion.ObtenerConexion())
            {
                if (conn == null)
                {
                    MessageBox.Show("No se pudo conectar a la base de datos.", "Error de Conexión");
                    return;
                }

                string query = @"
                    UPDATE Productos
                    SET NombreProducto = @nombre,
                        Descripcion = @descripcion,
                        PrecioPorUnidad = @precio,
                        Existencias = @existencias
                    WHERE IDProducto = @id;";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text.Trim());
                    cmd.Parameters.AddWithValue("@precio", decimal.TryParse(txtPrecio.Text, out var precio) ? precio : 0);
                    cmd.Parameters.AddWithValue("@existencias", int.TryParse(txtExistencia.Text, out var existencias) ? existencias : 0);
                    cmd.Parameters.AddWithValue("@id", id);

                    try
                    {
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("✏️ Producto actualizado correctamente.");
                            LimpiarCampos();
                        }
                        else
                        {
                            MessageBox.Show("⚠️ No se actualizó ningún producto.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ Error al actualizar el producto:\n{ex.Message}");
                    }
                }
            }
        }

        private void CargarProductos()
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
                    dataGridView1.DataSource = dt;
                }
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();
            txtExistencia.Clear();
        }

        private void EliminarProducto()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para eliminar.");
                return;
            }

            var id = dataGridView1.SelectedRows[0].Cells["IDProducto"].Value;

            var confirm = MessageBox.Show("¿Está seguro que desea eliminar este producto?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            using (var conn = dbConexion.ObtenerConexion())
            {
                if (conn == null)
                {
                    MessageBox.Show("No se pudo conectar a la base de datos.", "Error de Conexión");
                    return;
                }

                string query = "DELETE FROM Productos WHERE IDProducto = @id";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    try
                    {
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("🗑 Producto eliminado correctamente.");
                        }
                        else
                        {
                            MessageBox.Show("⚠️ No se eliminó ningún producto.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"❌ Error al eliminar el producto:\n{ex.Message}");
                    }
                }
            }
        }
    }
}
