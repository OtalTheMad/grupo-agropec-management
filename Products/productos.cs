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
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var row = dataGridView1.SelectedRows[0];
                txtNombre.Text = row.Cells["NombreProducto"].Value?.ToString();
                txtDescripcion.Text = row.Cells["Descripcion"].Value?.ToString();
                txtPrecio.Text = row.Cells["PrecioPorUnidad"].Value?.ToString();
                txtExistencia.Text = row.Cells["Existencias"].Value?.ToString();
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

        private ToolTip tooltip = new ToolTip();

        private bool RegistrarProducto()
        {
            btnAgregar.Enabled = false;

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                tooltip.Show("Campo obligatorio", txtNombre, 2000);
                txtNombre.BackColor = Color.MistyRose;
                txtNombre.Focus();
                btnAgregar.Enabled = true;
                return false;
            }
            else txtNombre.BackColor = SystemColors.Window;

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                tooltip.Show("Campo obligatorio", txtDescripcion, 2000);
                txtDescripcion.BackColor = Color.MistyRose;
                txtDescripcion.Focus();
                btnAgregar.Enabled = true;
                return false;
            }
            else txtDescripcion.BackColor = SystemColors.Window;

            if (!decimal.TryParse(txtPrecio.Text, out var precio))
            {
                tooltip.Show("Solo se permiten números", txtPrecio, 2000);
                txtPrecio.BackColor = Color.MistyRose;
                txtPrecio.Focus();
                btnAgregar.Enabled = true;
                return false;
            }
            else txtPrecio.BackColor = SystemColors.Window;

            if (!int.TryParse(txtExistencia.Text, out var existencias))
            {
                tooltip.Show("Solo se permiten números enteros", txtExistencia, 2000);
                txtExistencia.BackColor = Color.MistyRose;
                txtExistencia.Focus();
                btnAgregar.Enabled = true;
                return false;
            }
            else txtExistencia.BackColor = SystemColors.Window;

            using (var conn = dbConexion.ObtenerConexion())
            {
                if (conn == null)
                {
                    MessageBox.Show("No se pudo conectar a la base de datos.", "Error de Conexión");
                    btnAgregar.Enabled = true;
                    return false;
                }

                string query = @"
            INSERT INTO Productos (NombreProducto, Descripcion, PrecioPorUnidad, Existencias)
            VALUES (@nombre, @descripcion, @precio, @existencias);";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text.Trim());
                    cmd.Parameters.AddWithValue("@precio", precio);
                    cmd.Parameters.AddWithValue("@existencias", existencias);

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
                    finally
                    {
                        btnAgregar.Enabled = true;
                    }
                }
            }
            btnAgregar.Enabled = true;
            return false;
        }

        private void ActualizarProducto()
        {
            btnActualizar.Enabled = false;

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para actualizar.");
                btnActualizar.Enabled = true;
                return;
            }

            var row = dataGridView1.SelectedRows[0];
            var id = row.Cells["IDProducto"].Value;

            using (var conn = dbConexion.ObtenerConexion())
            {
                if (conn == null)
                {
                    MessageBox.Show("No se pudo conectar a la base de datos.", "Error de Conexión");
                    btnActualizar.Enabled = true;
                    return;
                }

                var updates = new List<string>();
                var cmd = new SQLiteCommand { Connection = conn };

                if (!string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    updates.Add("NombreProducto = @nombre");
                    cmd.Parameters.AddWithValue("@nombre", txtNombre.Text.Trim());
                    txtNombre.BackColor = SystemColors.Window;
                }

                if (!string.IsNullOrWhiteSpace(txtDescripcion.Text))
                {
                    updates.Add("Descripcion = @descripcion");
                    cmd.Parameters.AddWithValue("@descripcion", txtDescripcion.Text.Trim());
                    txtDescripcion.BackColor = SystemColors.Window;
                }

                if (!string.IsNullOrWhiteSpace(txtPrecio.Text))
                {
                    if (!decimal.TryParse(txtPrecio.Text, out var precio))
                    {
                        tooltip.Show("Solo se permiten números", txtPrecio, 2000);
                        txtPrecio.BackColor = Color.MistyRose;
                        txtPrecio.Focus();
                        btnActualizar.Enabled = true;
                        return;
                    }
                    updates.Add("PrecioPorUnidad = @precio");
                    cmd.Parameters.AddWithValue("@precio", precio);
                    txtPrecio.BackColor = SystemColors.Window;
                }

                if (!string.IsNullOrWhiteSpace(txtExistencia.Text))
                {
                    if (!int.TryParse(txtExistencia.Text, out var existencias))
                    {
                        tooltip.Show("Solo se permiten números enteros", txtExistencia, 2000);
                        txtExistencia.BackColor = Color.MistyRose;
                        txtExistencia.Focus();
                        btnActualizar.Enabled = true;
                        return;
                    }
                    updates.Add("Existencias = @existencias");
                    cmd.Parameters.AddWithValue("@existencias", existencias);
                    txtExistencia.BackColor = SystemColors.Window;
                }

                if (updates.Count == 0)
                {
                    MessageBox.Show("No hay cambios válidos para actualizar.");
                    btnActualizar.Enabled = true;
                    return;
                }

                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandText = $"UPDATE Productos SET {string.Join(", ", updates)} WHERE IDProducto = @id;";

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
                finally
                {
                    btnActualizar.Enabled = true;
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

                    // Elimina selección previa
                    dataGridView1.ClearSelection();
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
            btnEliminar.Enabled = false;

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para eliminar.");
                btnEliminar.Enabled = true;
                return;
            }

            var id = dataGridView1.SelectedRows[0].Cells["IDProducto"].Value;

            var confirm = MessageBox.Show("¿Está seguro que desea eliminar este producto?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes)
            {
                btnEliminar.Enabled = true;
                return;
            }

            using (var conn = dbConexion.ObtenerConexion())
            {
                if (conn == null)
                {
                    MessageBox.Show("No se pudo conectar a la base de datos.", "Error de Conexión");
                    btnEliminar.Enabled = true;
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
                    finally
                    {
                        btnEliminar.Enabled = true;
                    }
                }
            }
        }

    }
}
