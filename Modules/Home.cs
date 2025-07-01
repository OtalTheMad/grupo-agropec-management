using System;
using System.Windows.Forms;
using ProyectoIsis.Utilities;
using ProyectoIsis.Products;
using ProyectoIsis.Modules;
using ProyectoIsis.Modules.Facturas;

namespace ProyectoIsis
{
    public partial class Home : Form
    {
        Efectos efectos = new Efectos();

        public Home()
        {
            InitializeComponent();
        }


        private void btnVerProductos_Click(object sender, EventArgs e)
        {
            var vistaProductos = new VistaProducto();
            vistaProductos.ShowDialog();
        }

        private void btnBalance_Click(object sender, EventArgs e)
        {
            var formProductos = new VistaBalance();
            formProductos.ShowDialog();
        }

        private void btnGestionProductos_Click(object sender, EventArgs e)
        {
            var formProductos = new Productos();
            formProductos.ShowDialog();
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            var formVentas =   new Ventas();
            formVentas.ShowDialog();
        }
        private void btnFacturas_Click(object sender, EventArgs e)
        {
            var formFacturas = new VistaPrincipal();
            formFacturas.ShowDialog();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            efectos.AplicarFormatoBoton(this.btnGestionProductos, "#A8D5BA", "#3c3c3c");
            efectos.AplicarHover(this.btnGestionProductos, "#A8D5BA", "#95C8A3", "#3c3c3c");
            efectos.AplicarFormatoBoton(this.btnVerProductos, "#A8D5BA", "#3c3c3c");
            efectos.AplicarHover(this.btnVerProductos, "#A8D5BA", "#95C8A3", "#3c3c3c");
            efectos.AplicarFormatoBoton(this.btnBalance, "#A8D5BA", "#3c3c3c");
            efectos.AplicarHover(this.btnBalance, "#A8D5BA", "#95C8A3", "#3c3c3c");
            efectos.AplicarFormatoBoton(this.btnFacturas, "#A8D5BA", "#3c3c3c");
            efectos.AplicarHover(this.btnFacturas, "#A8D5BA", "#95C8A3", "#3c3c3c");
            efectos.AplicarFormatoBoton(this.btnVentas, "#A8D5BA", "#3c3c3c");
            efectos.AplicarHover(this.btnVentas, "#A8D5BA", "#95C8A3", "#3c3c3c");
            efectos.AplicarFormatoBoton(this.btnSalir, "#A8D5BA", "#3c3c3c");
            efectos.AplicarHover(this.btnSalir, "#A8D5BA", "#95C8A3", "#3c3c3c");
        }
        private void usuariosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var formUsuarios = new ControlDeUsuario();
            formUsuarios.ShowDialog();
        }
    }
}