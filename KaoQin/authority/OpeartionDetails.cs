using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin.authority
{
    
    public partial class OpeartionDetails : Form
    {
        public string Details = "";
        public OpeartionDetails()
        {
            InitializeComponent();
        }

        private void OpeartionDetails_Load(object sender, EventArgs e)
        {
            textBox1.AppendText( Details);
            textBox1.ReadOnly = true;
            this.Text = "操作详细信息";
        }
    }
}
