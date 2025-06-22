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
        private System.Windows.Forms.Button btnMulai;
        private System.Windows.Forms.Button btnKeluar;
        private System.Windows.Forms.Button btnTimer;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenuForm));
            btnMulai = new Button();
            btnKeluar = new Button();
            btnTimer = new Button();
            SuspendLayout();
            // 
            // btnMulai
            // 
            btnMulai.Image = (Image)resources.GetObject("btnMulai.Image");
            btnMulai.Location = new Point(116, 255);
            btnMulai.Name = "btnMulai";
            btnMulai.Size = new Size(180, 70);
            btnMulai.TabIndex = 0;
            btnMulai.UseVisualStyleBackColor = false;
            btnMulai.Click += btnMulai_Click;
            // 
            // btnKeluar
            // 
            btnKeluar.Image = (Image)resources.GetObject("btnKeluar.Image");
            btnKeluar.Location = new Point(116, 405);
            btnKeluar.Name = "btnKeluar";
            btnKeluar.Size = new Size(180, 70);
            btnKeluar.TabIndex = 1;
            btnKeluar.Click += btnKeluar_Click;
            // 
            // btnTimer
            // 
            btnTimer.Image = (Image)resources.GetObject("btnTimer.Image");
            btnTimer.Location = new Point(116, 331);
            btnTimer.Name = "btnTimer";
            btnTimer.Size = new Size(180, 70);
            btnTimer.TabIndex = 2;
            btnTimer.Click += btnTimer_Click;
            // 
            // MainMenuForm
            // 
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(800, 600);
            Controls.Add(btnMulai);
            Controls.Add(btnKeluar);
            Controls.Add(btnTimer);
            Name = "MainMenuForm";
            Text = "Menu Utama";
            ResumeLayout(false);
        }


        #endregion
    }
}