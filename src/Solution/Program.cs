using System;
using System.Windows.Forms;

namespace TralalaGame
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainMenuForm());

            Application.Run(new LevelForm());
        }
    }
}