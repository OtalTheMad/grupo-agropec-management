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
            this.btnBalance = new System.Windows.Forms.Button();
            this.btnFacturas = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGestionProductos
            // 
            this.btnGestionProductos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(250)))), ((int)(((byte)(240)))));
            this.btnGestionProductos.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGestionProductos.Location = new System.Drawing.Point(89, 54);
            this.btnGestionProductos.Name = "btnGestionProductos";
            this.btnGestionProductos.Size = new System.Drawing.Size(279, 140);
            this.btnGestionProductos.TabIndex = 0;
            this.btnGestionProductos.Text = "Gestión de Productos";
            this.btnGestionProductos.UseVisualStyleBackColor = false;
            this.btnGestionProductos.Click += new System.EventHandler(this.btnVerProductos_Click);
            // 
            // btnVerProductos
            // 
            this.btnVerProductos.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVerProductos.Location = new System.Drawing.Point(439, 54);
            this.btnVerProductos.Name = "btnVerProductos";
            this.btnVerProductos.Size = new System.Drawing.Size(279, 140);
            this.btnVerProductos.TabIndex = 0;
            this.btnVerProductos.Text = "Ver Productos";
            this.btnVerProductos.Click += new System.EventHandler(this.btnVerProductos_Click);
            // 
            // btnBalance
            // 
            this.btnBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBalance.Location = new System.Drawing.Point(89, 230);
            this.btnBalance.Name = "btnBalance";
            this.btnBalance.Size = new System.Drawing.Size(279, 140);
            this.btnBalance.TabIndex = 0;
            this.btnBalance.Text = "Balance";
            this.btnBalance.Click += new System.EventHandler(this.btnVerProductos_Click);
            // 
            // btnFacturas
            // 
            this.btnFacturas.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFacturas.Location = new System.Drawing.Point(439, 230);
            this.btnFacturas.Name = "btnFacturas";
            this.btnFacturas.Size = new System.Drawing.Size(279, 140);
            this.btnFacturas.TabIndex = 0;
            this.btnFacturas.Text = "Facturas";
            this.btnFacturas.Click += new System.EventHandler(this.btnVerProductos_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(89, 401);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(279, 140);
            this.button1.TabIndex = 0;
            this.button1.Text = "Ventas";
            this.button1.Click += new System.EventHandler(this.btnVerProductos_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalir.Location = new System.Drawing.Point(439, 401);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(279, 140);
            this.btnSalir.TabIndex = 0;
            this.btnSalir.Text = "Salir";
            this.btnSalir.Click += new System.EventHandler(this.btnVerProductos_Click);
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(250)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(800, 590);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnFacturas);
            this.Controls.Add(this.btnBalance);
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
        private System.Windows.Forms.Button btnBalance;
        private System.Windows.Forms.Button btnFacturas;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnSalir;
    }
}

