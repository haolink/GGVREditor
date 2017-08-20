using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace GGVREditor
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            EditSettings settings = new EditSettings();
            settings.UsePAKFile = false;
            settings.GameDirectory = @"F:\Steam\SteamApps\common\GalGun VR";

            Application.Run(new MainForm(settings));
        }
    }
}
