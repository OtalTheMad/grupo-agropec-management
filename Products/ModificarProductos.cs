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

namespace ProyectoIsis.Products
{
    public partial class ModificarProductos : Form
    {
        Efectos efectos = new Efectos();

        public bool Successful = false;
        private int idProducto;
        public ModificarProductos(int idProducto)
        {
            InitializeComponent();
            this.idProducto = idProducto;
        }

        #region MetodosDB
        
        private void CargarDatosProducto()
        {
            using (var conn = dbConexion.ObtenerConexion())
            {
                if (conn == null)
                {
                    MessageBox.Show("No se pudo conectar a la base de datos.", "Error de Conexión");
                    return;
                }
                try
                {
                    string query = @"
                    SELECT 
                        CantidadStock
                    FROM Productos
                    WHERE IDProducto = @idProducto";
                    
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idProducto", idProducto);
                        using (var adapter = new SQLiteDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                var cantidad = dt.Rows[0]["CantidadStock"];
                                if (cantidad != DBNull.Value)
                                {
                                    int cantidadStock = Convert.ToInt32(cantidad);
                                    if (cantidadStock > numericUpDown1.Maximum)
                                    {
                                        numericUpDown1.Maximum = cantidadStock;
                                    }
                                    if (cantidadStock < numericUpDown1.Minimum)
                                    {
                                        numericUpDown1.Value = numericUpDown1.Minimum;
                                    }
                                    else
                                    {
                                        numericUpDown1.Value = cantidadStock;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("El valor de CantidadStock es nulo. Por favor, verifique los datos del producto.", "Datos no válidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    numericUpDown1.Value = 0;
                                }
                            }
                            else
                            {
                                MessageBox.Show("No se encontraron datos para el producto seleccionado.", "Datos no encontrados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                // Si no hay datos, puedes decidir si dejar el NumericUpDown en 0 o en un valor predeterminadoq
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Hubo un error procesando los datos. Informe al desarrollador del producto.", "Cargar Datos de Productos", MessageBoxButtons.OKCancel);
                    throw;
                }
            }
        }

        private void ActualizarCantidadStock()
        {
            int filasAfectadas = 0;

            using (var conn = dbConexion.ObtenerConexion())
            {
                if (conn == null)
                {
                    MessageBox.Show("No se pudo conectar a la base de datos.", "Error de Conexión");
                    return;
                }
                try
                {
                    string query = @"
                    UPDATE Productos
                    SET CantidadStock = CantidadStock + @cantidadStock
                    WHERE IDProducto = @idProducto";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cantidadStock", numericUpDown1.Value);
                        cmd.Parameters.AddWithValue("@idProducto", idProducto);
                        filasAfectadas = cmd.ExecuteNonQuery();
                        Successful = filasAfectadas > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hubo un error actualizando los datos. Informe al desarrollador del producto.\n" + ex.Message, "Actualizar Cantidad Stock", MessageBoxButtons.OKCancel);
                }
            }
            
            if (Successful)
            {
                MessageBox.Show("Existencias actualizadas exitosamente.", "Existencias Actualizadas", MessageBoxButtons.OK);
                ReestablecerCampos();
            }            
        }

        #endregion
        #region Metodos de Formulario

        private void ReestablecerCampos()
        {
            numericUpDown1.Value = 0;
        }

        #endregion

        private void ModificarProductos_Load(object sender, EventArgs e)
        {
            efectos.AplicarFormatoBoton(this.btnAgregar, "#A8D5BA", "#3c3c3c");
            efectos.AplicarFormatoBoton(this.btnCancelar, "#A8D5BA", "#3c3c3c");
            efectos.AplicarHover(this.btnAgregar, "#A8D5BA", "#95C8A3", "#3c3c3c");
            efectos.AplicarHover(this.btnCancelar, "#A8D5BA", "#95C8A3", "#3c3c3c");
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            ActualizarCantidadStock();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
