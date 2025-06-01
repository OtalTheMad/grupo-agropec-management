namespace ProyectoIsis
{
    partial class Home
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGestionProductos = new System.Windows.Forms.Button();
            this.btnVerProductos = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGestionProductos
            // 
            this.btnGestionProductos.Location = new System.Drawing.Point(88, 54);
            this.btnGestionProductos.Name = "btnGestionProductos";
            this.btnGestionProductos.Size = new System.Drawing.Size(279, 140);
            this.btnGestionProductos.TabIndex = 0;
            this.btnGestionProductos.Text = "Gestión de Productos";
            this.btnGestionProductos.UseVisualStyleBackColor = true;
            this.btnGestionProductos.Click += new System.EventHandler(this.btnVerProductos_Click);
            // 
            // btnVerProductos
            // 
            this.btnVerProductos.Location = new System.Drawing.Point(439, 54);
            this.btnVerProductos.Name = "btnVerProductos";
            this.btnVerProductos.Size = new System.Drawing.Size(279, 140);
            this.btnVerProductos.TabIndex = 0;
            this.btnVerProductos.Text = "Ver Productos";
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(250)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(800, 510);
            this.Controls.Add(this.btnVerProductos);
            this.Controls.Add(this.btnGestionProductos);
            this.Name = "Home";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inicio";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGestionProductos;
        private System.Windows.Forms.Button btnVerProductos;
    }
}

