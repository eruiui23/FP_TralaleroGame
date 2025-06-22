namespace TralalaGame
{
    partial class MainMenuForm
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
        //private void InitializeComponent()
        //{
        //    this.components = new System.ComponentModel.Container();
        //    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //    this.ClientSize = new System.Drawing.Size(800, 450);
        //    this.Text = "MainMenuForm";
        //}
        private System.Windows.Forms.Button btnMulai;
        private System.Windows.Forms.Button btnKeluar;

        private void InitializeComponent()
        {
            this.btnMulai = new System.Windows.Forms.Button();
            this.btnKeluar = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // btnMulai
            this.btnMulai.Location = new System.Drawing.Point(100, 50);
            this.btnMulai.Size = new System.Drawing.Size(100, 30);
            this.btnMulai.Text = "Mulai";
            this.btnMulai.Click += new System.EventHandler(this.btnMulai_Click);

            // btnKeluar
            this.btnKeluar.Location = new System.Drawing.Point(100, 100);
            this.btnKeluar.Size = new System.Drawing.Size(100, 30);
            this.btnKeluar.Text = "Keluar";
            this.btnKeluar.Click += new System.EventHandler(this.btnKeluar_Click);

            // MainMenuForm
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.btnMulai);
            this.Controls.Add(this.btnKeluar);
            this.Text = "Menu Utama";
            this.ResumeLayout(false);
        }


        #endregion
    }
}