using ProyectoIsis.Data;
using ProyectoIsis.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoIsis.Modules
{
    public partial class ControlDeUsuario : Form
    {
        Efectos efectos = new Efectos();
        
        public ControlDeUsuario()
        {
            InitializeComponent();
        }

        private void CargarUsuarios()
        {
            using (var conn = dbConexion.ObtenerConexion())
            {
                if (conn == null)
                {
                    MessageBox.Show("No se pudo conectar a la base de datos.", "Error de Conexión");
                    return;
                }
                string query = "SELECT IDUsuario, Usuario, EsAdmin, CreadoEn FROM Usuarios WHERE esActivo = 1";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    using (var adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvUsuarios.DataSource = dt;
                        dgvUsuarios.ClearSelection(); // Limpia selección
                    }
                }
            }
        }

        private void AgregarUsuario()
        {
            string usuario = txtUser.Text.Trim();
            string password = txtPassword.Text.Trim();

            using (var conn = dbConexion.ObtenerConexion())
            {
                if (conn == null)
                {
                    MessageBox.Show("No se pudo conectar a la base de datos.", "Error de Conexión");
                    return;
                }
                if (usuario == string.Empty || password == string.Empty)
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Campos Vacíos");
                    return;
                }
                if (password.Length < 8)
                {
                    MessageBox.Show("La contraseña debe tener al menos 8 caracteres.", "Contraseña Débil");
                    return;
                }

                // Verificar si el usuario ya existe
                string checkQuery = "SELECT COUNT(*) FROM Usuarios WHERE Usuario = @Usuario";
                using (var checkCmd = new SQLiteCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Usuario", usuario);
                    long count = (long)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("El usuario ya existe. Elija otro nombre de usuario.", "Usuario Existente");
                        return;
                    }
                }

                string query = "INSERT INTO Usuarios (Usuario, PasswordHash, EsAdmin, CreadoEn) VALUES (@Usuario, @Password, @EsAdmin, @CreadoEn)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Usuario", txtUser.Text);
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@EsAdmin", cbEsAdmin.Checked);
                    cmd.Parameters.AddWithValue("@CreadoEn", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Usuario agregado exitosamente.", "Éxito");
                        btnLimpiar.PerformClick();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al agregar usuario: " + ex.Message, "Error");
                    }
                }
            }
        }

        private void EliminarUsuario()
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario para eliminar.");
                btnEliminar.Enabled = false;
                return;
            }

            var row = dgvUsuarios.SelectedRows[0];
            var id = row.Cells["IDUsuario"].Value;

            using (var conn = dbConexion.ObtenerConexion())
            {
                if (conn == null)
                {
                    MessageBox.Show("No se pudo conectar a la base de datos.", "Error de Conexión");
                    return;
                }

                string query = "UPDATE Usuarios SET (esActivo) = 0 WHERE IDUsuario = @IDProducto";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@IDProducto", id);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Usuario eliminado exitosamente.", "Éxito");
                        CargarUsuarios();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar al usuario: " + ex.Message, "Error");
                    }

                }

            }
        }


        private void FormatoDGV()
        {
            try
            {
                dgvUsuarios.Columns[0].HeaderText = "ID Usuario";
                dgvUsuarios.Columns[1].HeaderText = "Usuario";
                dgvUsuarios.Columns[2].HeaderText = "Es Admin";
                dgvUsuarios.Columns[3].HeaderText = "Creado En";
                // Ajustar el ancho de las columnas
                dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvUsuarios.AllowUserToAddRows = false;
                dgvUsuarios.AllowUserToDeleteRows = false;
                dgvUsuarios.AllowUserToResizeColumns = true;
                dgvUsuarios.AllowUserToResizeRows = false;
                dgvUsuarios.ReadOnly = true;
                dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvUsuarios.MultiSelect = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al dar formato a el DataGridView: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtUser.Clear();
            txtPassword.Clear();
            cbEsAdmin.Checked = false;
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AgregarUsuario();
            CargarUsuarios();
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            EliminarUsuario();
        }

        private void ControlDeUsuario_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
            FormatoDGV();
            efectos.AplicarFormatoBoton(this.btnAgregar, "#A8D5BA", "#3c3c3c");
            efectos.AplicarHover(this.btnAgregar, "#A8D5BA", "#95C8A3", "#3c3c3c");
            efectos.AplicarFormatoBoton(this.btnLimpiar, "#A8D5BA", "#3c3c3c");
            efectos.AplicarHover(this.btnLimpiar, "#A8D5BA", "#95C8A3", "#3c3c3c");
            efectos.AplicarFormatoBoton(this.btnCancelar, "#A8D5BA", "#3c3c3c");
            efectos.AplicarHover(this.btnCancelar, "#A8D5BA", "#95C8A3", "#3c3c3c");
            efectos.AplicarFormatoBoton(this.btnEliminar, "#A8D5BA", "#3c3c3c");
            efectos.AplicarHover(this.btnEliminar, "#A8D5BA", "#95C8A3", "#3c3c3c");
        }
    }
}
