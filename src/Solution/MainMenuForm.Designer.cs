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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenuForm));
            btnMulai = new Button();
            btnKeluar = new Button();
            btnTimer = new Button();
            textSeconds = new TextBox();
            lbCounter = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            btnStart = new Button();
            btnTimerKembali = new Button();
            secondLabel = new PictureBox();
            counterBg = new PictureBox();
            textSecondBg = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)secondLabel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)counterBg).BeginInit();
            ((System.ComponentModel.ISupportInitialize)textSecondBg).BeginInit();
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
            // textSeconds
            // 
            textSeconds.BackColor = SystemColors.Window;
            textSeconds.BorderStyle = BorderStyle.None;
            textSeconds.Font = new Font("Minecraft", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            textSeconds.Location = new Point(90, 362);
            textSeconds.Name = "textSeconds";
            textSeconds.Size = new Size(63, 26);
            textSeconds.TabIndex = 4;
            textSeconds.Text = "10";
            // 
            // lbCounter
            // 
            lbCounter.AutoSize = true;
            lbCounter.BackColor = Color.White;
            lbCounter.FlatStyle = FlatStyle.Flat;
            lbCounter.Font = new Font("Minecraft", 28.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbCounter.ForeColor = SystemColors.InfoText;
            lbCounter.Location = new Point(97, 278);
            lbCounter.Name = "lbCounter";
            lbCounter.Size = new Size(160, 47);
            lbCounter.TabIndex = 7;
            lbCounter.Text = "Timer";
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // btnStart
            // 
            btnStart.BackgroundImage = (Image)resources.GetObject("btnStart.BackgroundImage");
            btnStart.BackgroundImageLayout = ImageLayout.Center;
            btnStart.Font = new Font("Minecraft", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStart.Location = new Point(59, 425);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(133, 50);
            btnStart.TabIndex = 8;
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnTimerKembali
            // 
            btnTimerKembali.BackgroundImage = (Image)resources.GetObject("btnTimerKembali.BackgroundImage");
            btnTimerKembali.BackgroundImageLayout = ImageLayout.Center;
            btnTimerKembali.Location = new Point(198, 425);
            btnTimerKembali.Name = "btnTimerKembali";
            btnTimerKembali.Size = new Size(133, 50);
            btnTimerKembali.TabIndex = 9;
            btnTimerKembali.UseVisualStyleBackColor = true;
            btnTimerKembali.Click += btnTimerKembali_Click;
            // 
            // secondLabel
            // 
            secondLabel.BackColor = SystemColors.Window;
            secondLabel.BackgroundImage = (Image)resources.GetObject("secondLabel.BackgroundImage");
            secondLabel.BackgroundImageLayout = ImageLayout.Center;
            secondLabel.Location = new Point(159, 346);
            secondLabel.Name = "secondLabel";
            secondLabel.Size = new Size(150, 55);
            secondLabel.TabIndex = 10;
            secondLabel.TabStop = false;
            // 
            // counterBg
            // 
            counterBg.BackgroundImage = (Image)resources.GetObject("counterBg.BackgroundImage");
            counterBg.BackgroundImageLayout = ImageLayout.Center;
            counterBg.Location = new Point(79, 252);
            counterBg.Name = "counterBg";
            counterBg.Size = new Size(235, 88);
            counterBg.TabIndex = 11;
            counterBg.TabStop = false;
            // 
            // textSecondBg
            // 
            textSecondBg.BackgroundImage = (Image)resources.GetObject("textSecondBg.BackgroundImage");
            textSecondBg.BackgroundImageLayout = ImageLayout.Center;
            textSecondBg.Location = new Point(79, 346);
            textSecondBg.Name = "textSecondBg";
            textSecondBg.Size = new Size(81, 55);
            textSecondBg.TabIndex = 12;
            textSecondBg.TabStop = false;
            // 
            // MainMenuForm
            // 
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(800, 600);
            Controls.Add(textSeconds);
            Controls.Add(textSecondBg);
            Controls.Add(lbCounter);
            Controls.Add(counterBg);
            Controls.Add(secondLabel);
            Controls.Add(btnTimerKembali);
            Controls.Add(btnStart);
            Controls.Add(btnMulai);
            Controls.Add(btnKeluar);
            Controls.Add(btnTimer);
            Name = "MainMenuForm";
            Text = "Menu Utama";
            ((System.ComponentModel.ISupportInitialize)secondLabel).EndInit();
            ((System.ComponentModel.ISupportInitialize)counterBg).EndInit();
            ((System.ComponentModel.ISupportInitialize)textSecondBg).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox textSeconds;
        private Label lbCounter;
        private System.Windows.Forms.Timer timer1;
        private Button btnStart;
        private Button btnTimerKembali;
        private PictureBox secondLabel;
        private PictureBox counterBg;
        private PictureBox textSecondBg;
    }
}