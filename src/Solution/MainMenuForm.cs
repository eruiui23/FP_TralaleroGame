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
        public MainMenuForm()
        {
            InitializeComponent();
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
