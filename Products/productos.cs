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
            efectos.AplicarColor(btnAgregar, "#A2D2FF", "#3C3C3C");
            efectos.AplicarColor(btnEliminar, "#FCE38A", "#3C3C3C");
            efectos.AplicarColor(btnActualizar, "#F8A5A5", "#3C3C3C");
            efectos.AplicarColor(btnRegresar, "#A8D5BA", "#3C3C3C");

            efectos.AplicarHover(btnAgregar, "#A8D5BA", "#95C8A3", "#3C3C3C");
            efectos.AplicarHover(btnEliminar, "#A8D5BA", "#95C8A3", "#3C3C3C");
            efectos.AplicarHover(btnActualizar, "#A8D5BA", "#95C8A3", "#3C3C3C");
            efectos.AplicarHover(btnRegresar, "#A8D5BA", "#95C8A3", "#3C3C3C");
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
