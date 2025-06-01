using System;
using System.Windows.Forms;
using ProyectoIsis.Utilities;

namespace ProyectoIsis.Products
{
    public partial class Productos : Form
    {
        Efectos efectos = new Efectos();

        public Productos()
        {
            InitializeComponent();
            cargarEfectos();
        }

        private void cargarEfectos()
        {
            efectos.AplicarHover(btnAgregar, "#007bff", "#95C8A3", "#3C3C3C");
            efectos.AplicarHover(btnEliminar, "#dc3545", "#95C8A3", "#3C3C3C");
            efectos.AplicarHover(btnActualizar, "#ffc107", "#95C8A3", "#3C3C3C");
            efectos.AplicarHover(btnRegresar, "#04feff", "#95C8A3", "#3C3C3C");
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            var home = new Home();
            home.Show();
            this.Dispose();
        }
    }
}
