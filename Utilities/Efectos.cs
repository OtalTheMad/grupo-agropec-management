using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoIsis.Utilities
{
    public class Efectos
    {
        #region Design
        public void AplicarHover(Button boton, string colorBaseHex, string colorHoverHex, string colorTextoHex)
        {
            Color colorBase = ColorTranslator.FromHtml(colorBaseHex);
            Color colorHover = ColorTranslator.FromHtml(colorHoverHex);
            Color colorTexto = ColorTranslator.FromHtml(colorTextoHex);

            boton.BackColor = colorBase;
            boton.ForeColor = colorTexto;

            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;

            boton.MouseEnter += (s, e) => boton.BackColor = colorHover;
            boton.MouseLeave += (s, e) => boton.BackColor = colorBase;
        }
        public void AplicarHover(Button boton, string colorBaseHex, string colorTextoHex)
        {
            Color colorBase = ColorTranslator.FromHtml(colorBaseHex);
            Color colorTexto = ColorTranslator.FromHtml(colorTextoHex);

            boton.BackColor = colorBase;
            boton.ForeColor = colorTexto;

            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
        }
        #endregion
    }
}
