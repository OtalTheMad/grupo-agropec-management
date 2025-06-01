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

        private void button1_Click(object sender, EventArgs e)
        {
            var formProductos = new ProyectoIsis.Products.productos();
            formProductos.ShowDialog();

        }
    }
}
