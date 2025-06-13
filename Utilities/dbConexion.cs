using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace ProyectoIsis.Data
{
    internal class dbConexion
    {
        private static readonly string dbFileName = "dbGrupoAgropec.sqlite";
        private static readonly string dbFolder = "Data";
        private static readonly string dbPath = Path.Combine(Application.StartupPath, dbFolder, dbFileName);
        private static readonly string connectionString = $"Data Source={dbPath};Version=3;";

        public static void ImprimirConexion()
        {
            MessageBox.Show(dbPath);
        }

        public static SQLiteConnection ObtenerConexion()
        {
            if(!File.Exists(dbPath))
            {
                MessageBox.Show("La base de datos no se encuentra en la ruta especificada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            var conn = new SQLiteConnection(connectionString);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos:\n", "Error" + ex.Message);
                return null;
            }

            return conn;
        }

        public static bool ProbarConexion()
        {
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string ObtenerRutaDB()
        {
            return dbPath;
        }
    }
}