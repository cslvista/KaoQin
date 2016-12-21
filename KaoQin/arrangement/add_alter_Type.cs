using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin.arrangement
{
    public partial class add_alter_Type : Form
    {
        public add_alter_Type()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string sql = "select max(ID) from KQ_PBType";



        }

        private void add_alter_Class_Load(object sender, EventArgs e)
        {

        }
    }
}
