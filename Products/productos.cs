using iText.Layout.Element;
using ProyectoIsis.Data;
using ProyectoIsis.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;


namespace ProyectoIsis.Products
{
    public partial class Productos : Form
    {
        Efectos efectos = new Efectos();

        public Productos()
        {
            InitializeComponent();
            CargarProductos();
        }

        private void FormatoDGV()
        {
            try
            {
                dataGridView1.Columns[0].HeaderText = "ID Producto";
                dataGridView1.Columns[1].HeaderText = "Nombre";
                dataGridView1.Columns[2].HeaderText = "Descripcion";
                dataGridView1.Columns[3].HeaderText = "Precio de Compra";
                dataGridView1.Columns[4].HeaderText = "Precio de Venta";
                dataGridView1.Columns[5].HeaderText = "Existencias";
                dataGridView1.Columns[6].HeaderText = "Creado En";
                // Ajustar el ancho de las columnas
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.AllowUserToResizeColumns = true;
                dataGridView1.AllowUserToResizeRows = false;
                dataGridView1.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al dar formato a el DataGridView: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (!int.TryParse(txtExistencia.Text, out var CantidadStock))
            {
                tooltip.Show("Solo se permiten números enteros", txtExistencia, 2000);
                txtExistencia.BackColor = Color.MistyRose;
                txtExistencia.Focus();
                btnAgregar.Enabled = true;
                return false;
            }
            else txtExistencia.BackColor = SystemColors.Window;
            
            if (!int.TryParse(txtPrecioCompra.Text, out var precioCompra))
            {
                tooltip.Show("Solo se permiten números enteros", txtPrecioCompra, 2000);
                txtPrecioCompra.BackColor = Color.MistyRose;
                txtPrecioCompra.Focus();
                btnAgregar.Enabled = true;
                return false;
            }
            else txtPrecioCompra.BackColor = SystemColors.Window;

            using (var conn = dbConexion.ObtenerConexion())
            {
                if (conn == null)
                {
                    MessageBox.Show("No se pudo conectar a la base de datos.", "Error de Conexión");
                    btnAgregar.Enabled = true;
                    return false;
                }

                string query = @"
            INSERT INTO Productos (Nombre, Descripcion, PrecioVenta, CantidadStock, CreadoEn, PrecioCompra)
            VALUES (@Nombre, @Descripcion, @PrecioVenta, @CantidadStock, @creadoEn, @PrecioCompra);";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@Descripcion", txtDescripcion.Text.Trim());
                    cmd.Parameters.AddWithValue("@PrecioVenta", precio);
                    cmd.Parameters.AddWithValue("@PrecioCompra", precioCompra);
                    cmd.Parameters.AddWithValue("@CantidadStock", CantidadStock);
                    cmd.Parameters.AddWithValue("@creadoEn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

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
            var id = row.Cells["Id"].Value;  // ← usa el alias personalizado del DataGridView

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
                    updates.Add("Nombre = @Nombre");
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                    txtNombre.BackColor = SystemColors.Window;
                }

                if (!string.IsNullOrWhiteSpace(txtDescripcion.Text))
                {
                    updates.Add("Descripcion = @Descripcion");
                    cmd.Parameters.AddWithValue("@Descripcion", txtDescripcion.Text.Trim());
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
                    updates.Add("PrecioVenta = @PrecioVenta");
                    cmd.Parameters.AddWithValue("@PrecioVenta", precio);
                    txtPrecio.BackColor = SystemColors.Window;
                }

                if (!string.IsNullOrWhiteSpace(txtPrecioCompra.Text))
                {
                    if (!decimal.TryParse(txtPrecioCompra.Text, out var precio))
                    {
                        tooltip.Show("Solo se permiten números", txtPrecioCompra, 2000);
                        txtPrecioCompra.BackColor = Color.MistyRose;
                        txtPrecioCompra.Focus();
                        btnActualizar.Enabled = true;
                        return;
                    }
                    updates.Add("PrecioVenta = @PrecioCompra");
                    cmd.Parameters.AddWithValue("@PrecioCompra", precio);
                    txtPrecioCompra.BackColor = SystemColors.Window;
                }

                if (!string.IsNullOrWhiteSpace(txtExistencia.Text))
                {
                    if (!int.TryParse(txtExistencia.Text, out var CantidadStock))
                    {
                        tooltip.Show("Solo se permiten números enteros", txtExistencia, 2000);
                        txtExistencia.BackColor = Color.MistyRose;
                        txtExistencia.Focus();
                        btnActualizar.Enabled = true;
                        return;
                    }
                    updates.Add("CantidadStock = @CantidadStock");
                    cmd.Parameters.AddWithValue("@CantidadStock", CantidadStock);
                    txtExistencia.BackColor = SystemColors.Window;
                }

                if (updates.Count == 0)
                {
                    MessageBox.Show("No hay cambios válidos para actualizar.");
                    btnActualizar.Enabled = true;
                    return;
                }

                cmd.Parameters.AddWithValue("@IDProducto", id);
                cmd.CommandText = $"UPDATE Productos SET {string.Join(", ", updates)} WHERE IDProducto = @IDProducto;";

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

                string query = @"
            SELECT 
                IDProducto AS 'Id',
                Nombre AS 'Nombre',
                Descripcion AS 'Descripcion',
                PrecioCompra AS 'Precio de Compra',
                PrecioVenta AS 'Precio de Venta',
                CantidadStock AS 'Existencias',
                CreadoEn AS 'Fecha de Creación'
            FROM Productos";

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
            txtPrecioCompra.Clear();
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

            var id = dataGridView1.SelectedRows[0].Cells["Id"].Value;

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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                DataGridViewRow fila = dataGridView1.CurrentRow;

                txtNombre.Text = fila.Cells["Nombre"].Value?.ToString();
                txtDescripcion.Text = fila.Cells["Descripcion"].Value?.ToString();
                txtPrecioCompra.Text = fila.Cells["Precio de Compra"].Value?.ToString();
                txtPrecio.Text = fila.Cells["Precio de Venta"].Value?.ToString();
                txtExistencia.Text = fila.Cells["Existencias"].Value?.ToString();
            }
        }

        private void Productos_Load(object sender, EventArgs e)
        {
            FormatoDGV();
            dataGridView1.ClearSelection();
            LimpiarCampos();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un producto primero.", "Modificar Producto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var idCell = dataGridView1.CurrentRow.Cells["Id"].Value;

            if (idCell == null || !int.TryParse(idCell.ToString(), out int idProducto))
            {
                MessageBox.Show("El producto seleccionado no tiene un ID válido.", "Modificar Producto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ModificarProductos modProductos = new ModificarProductos(idProducto);
            modProductos.ShowDialog();

            if (modProductos.Successful)
            {
                CargarProductos();
            }
        }
    }
}
