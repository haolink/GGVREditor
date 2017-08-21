using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GGVREditor
{
    public partial class PackedOrUnpacked : Form
    {
        public POUResult Result { get; private set; }

        public PackedOrUnpacked()
        {
            InitializeComponent();            
        }

        public enum POUResult
        {
            PAK,
            Unpacked,
            Undecided
        }

        private void PackedOrUnpacked_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = SystemIcons.Question.ToBitmap();
            Result = POUResult.Undecided;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Result = POUResult.PAK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Result = POUResult.Unpacked;
            Close();
        }

        private void PackedOrUnpacked_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (Result == POUResult.Undecided);
        }
    }
}
