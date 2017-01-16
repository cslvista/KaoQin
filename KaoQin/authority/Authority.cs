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
    public partial class Authority : Form
    {
        public Authority()
        {
            InitializeComponent();
        }

        private void Authority_Load(object sender, EventArgs e)
        {
            ButtonRefresh_Click(null,null);
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            add_alter_authority form = new add_alter_authority();
            form.Show();
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            string sql = "select * from KQ_QX";

        }
    }
}
