using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;



namespace TralalaGame
{
    public partial class MainMenuForm : Form
    {
        int seconds;


        PictureBox picShark = new PictureBox();
        System.Windows.Forms.Timer animTimer = new System.Windows.Forms.Timer();
        Image[] sharkFrames;
        int currentFrame = 0;

        SoundPlayer MainMenuPlayer = new SoundPlayer("Resources/JitenshaWav.wav");
        SoundPlayer LevelPlayer = new SoundPlayer("Resources/HummingWordWav.wav");

        public MainMenuForm()
        {
            InitializeComponent();

            // ngeset timer diawal ga keliatan
            lbCounter.Visible = false;
            textSeconds.Visible = false;
            btnStart.Visible = false;
            secondLabel.Visible = false;
            btnTimerKembali.Visible = false;
            counterBg.Visible = false;
            textSecondBg.Visible = false;

            MainMenuPlayer.PlayLooping();


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
            animTimer.Interval = 750; // ubah frame setiap tiap ... ms
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
            MasukLevel();
        }

        private void btnKeluar_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void btnTimer_Click(object sender, EventArgs e)
        {
            lbCounter.Visible = true;
            textSeconds.Visible = true;
            btnStart.Visible = true;
            secondLabel.Visible = true;
            btnTimerKembali.Visible = true;
            counterBg.Visible = true;
            textSecondBg.Visible = true;

            btnMulai.Visible = false;
            btnKeluar.Visible = false;
            btnTimer.Visible = false;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            lbCounter.Text = seconds--.ToString();
            if (seconds < 0)
            {
                MasukLevel();
                stopModeTimer();
            }
        }





        private void btnStart_Click(object sender, EventArgs e)
        {
            seconds = Convert.ToInt32(textSeconds.Text);
            textSeconds.Enabled = false;
            timer1.Start();
        }

        private void btnTimerKembali_Click(object sender, EventArgs e)
        {
            stopModeTimer();
        }

        private void stopModeTimer()
        {
            timer1.Stop();
            textSeconds.Enabled = true;
            lbCounter.Text = "Timer";

            lbCounter.Visible = false;
            textSeconds.Visible = false;
            btnStart.Visible = false;
            secondLabel.Visible = false;
            btnTimerKembali.Visible = false;
            counterBg.Visible = false;
            textSecondBg.Visible = false;

            btnMulai.Visible = true;
            btnKeluar.Visible = true;
            btnTimer.Visible = true;
        }

        private void MasukLevel()
        {
            timer1.Stop();
            textSeconds.Enabled = true;
            MainMenuPlayer.Stop();
            LevelPlayer.PlayLooping();
            this.Hide();
            LevelForm levelForm = new LevelForm();


            levelForm.FormClosed += (s, args) => // buka menu pas game di tutup
            {
                this.Show();
                MainMenuPlayer.PlayLooping();
            };

            levelForm.Show();
        }
    }
}
