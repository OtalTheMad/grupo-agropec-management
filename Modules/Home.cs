using ProyectoIsis.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoIsis
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        Productos userProducts = new Productos();

        private void btnGestionProductos_Click(object sender, EventArgs e)
        {
            userProducts.Show();
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void btnVerProductos_Click(object sender, EventArgs e)
        {

        }
    }
}
