using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin.users
{
    public partial class add_alter_Dep : Form
    {
        public add_alter_Dep()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请输入部门名称！");
                return;
            }

            string sql = "";



        }

        private void AddDep_Load(object sender, EventArgs e)
        {
            string sql = "";

            DataTable Department = new DataTable();


        }
    }
}
