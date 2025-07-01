﻿using System;
using System.Reflection;
using System.Windows.Forms;
using System.Data.SQLite;
using ProyectoIsis.Data;
using ProyectoIsis.Utilities;

namespace ProyectoIsis
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private string _user;
        private string _password;
        private int _idUsuario;

        private bool ValidarCredenciales(string _user, string _password)
        {
            using (var conn = dbConexion.ObtenerConexion())
            {
                if (conn == null)
                {
                    MessageBox.Show("No se pudo conectar a la base de datos. Verifique su conexión.", "Error de Conexión");
                    return false;
                }

                string query = @"SELECT esActivo from Usuarios WHERE Usuario = @usuario AND PasswordHash = @password";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@_idUsuario", _idUsuario);
                    cmd.Parameters.AddWithValue("@usuario", _user);
                    cmd.Parameters.AddWithValue("@password", _password);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bool activo = Convert.ToBoolean(reader["esActivo"]);
                            return activo;
                        }
                    }
                }
            }
            return false;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(dbConexion.ObtenerRutaDB()); // Imprimir la ruta de la base de datos para depuración
            _user = txtUser.Text;
            _password = txtPassword.Text;

            if (ValidarCredenciales(_user, _password))
            {
                // Obtener el ID del usuario para la sesión
                ObtenerUsuarioLogueado.IDUsuario = _idUsuario;
                ObtenerUsuarioLogueado.Usuario = _user;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("El usuario o la contraseña son incorrectos.");
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            string versionString = version.ToString();

            lblDev.Text = $"Desarrollado por Oscar Marcía - V{versionString}";
        }
    }
}