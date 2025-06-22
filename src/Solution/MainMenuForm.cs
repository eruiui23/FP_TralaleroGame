using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace TralalaGame
{
    public partial class MainMenuForm : Form
    {

        PictureBox picShark = new PictureBox();
        System.Windows.Forms.Timer animTimer = new System.Windows.Forms.Timer();
        Image[] sharkFrames;
        int currentFrame = 0;
        public MainMenuForm()
        {
            InitializeComponent();

            // Load semua frame
            sharkFrames = new Image[]
            {
                Image.FromFile("Resources/idleAnimation2/idle1.png"),
                Image.FromFile("Resources/idleAnimation2/idle2.png"),
                Image.FromFile("Resources/idleAnimation2/idle3.png"),
                Image.FromFile("Resources/idleAnimation2/idle4.png"),
            };
            // Atur PictureBox untuk hiu
            picShark.Size = new Size(638, 338);
            picShark.Location = new Point(380, 200); // posisi bebas sesuai background
            picShark.BackColor = Color.Transparent;
            picShark.Image = sharkFrames[0];
            picShark.SizeMode = PictureBoxSizeMode.Normal;
            this.Controls.Add(picShark);

            // Timer animasi
            animTimer.Interval = 350; // ubah frame setiap 150ms
            animTimer.Tick += AnimTimer_Tick;
            animTimer.Start();
        }
        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            currentFrame = (currentFrame + 1) % sharkFrames.Length;
            picShark.Image = sharkFrames[currentFrame];
        }
        private void btnMulai_Click(object sender, EventArgs e)
        {
            this.Hide();
            LevelForm levelForm = new LevelForm();
            levelForm.FormClosed += (s, args) => this.Show(); // Menutup menu saat game ditutup
            levelForm.Show();
        }

        private void btnKeluar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnTimer_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
