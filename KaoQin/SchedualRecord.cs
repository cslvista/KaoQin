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
    public partial class SchedualRecord : Form
    {
        public string details="";
        public SchedualRecord()
        {
            InitializeComponent();
        }

        private void SchedualRecord_Load(object sender, EventArgs e)
        {
            textBox1.AppendText(details);
            textBox1.ReadOnly = true;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {           
            Schedual form = (Schedual)this.Owner;
            form.ChangeAccept = true;
            this.Close();
        }
    }
}
