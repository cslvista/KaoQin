using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin
{
    public partial class unkownShift : Form
    {
        public string unkownshift = "";
        public string LBID = "";
        public unkownShift()
        {
            InitializeComponent();
        }

        private void unkownShift_Load(object sender, EventArgs e)
        {
            textBox1.Text = unkownshift;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            arrangement.arrange form = new arrangement.arrange();
            form.Authority_Shift_Edit = true;
            form.PB = true;
            form.PB_LBID = LBID;
            form.Show();
            Schedual form1 = (Schedual)this.Owner;
            form1.Close();
        }
    }
}
