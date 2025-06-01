using System;
using System.Windows.Forms;
using ProyectoIsis.Utilities;

namespace ProyectoIsis
{
    public partial class Home : Form
    {
        Efectos efectos = new Efectos();
        public Home()
        {
            InitializeComponent();
            efectos.AplicarFormatoBoton(this.btnGestionProductos, "#A8D5BA", "#3c3c3c");
            efectos.AplicarHover(this.btnGestionProductos, "#A8D5BA", "#95C8A3", "#3c3c3c");
            efectos.AplicarFormatoBoton(this.btnVerProductos, "#A8D5BA", "#3c3c3c");
            efectos.AplicarHover(this.btnVerProductos, "#A8D5BA", "#95C8A3", "#3c3c3c");
        }


        private void btnVerProductos_Click(object sender, EventArgs e)
        {
            var formProductos = new ProyectoIsis.Products.Productos();
            formProductos.ShowDialog();
        }
    }
}
