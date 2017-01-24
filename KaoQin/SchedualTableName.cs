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
    public partial class SchedualTableName : Form
    {
        public DataTable tableName = new DataTable();
        public SchedualTableName()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SchedualTableName_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < tableName.Rows.Count; i++)
            {
                string str = tableName.Rows[i]["TABLE_NAME"].ToString().Replace("'", "");
                string str1= str.Replace("$", "");
                string str2= str1.Replace("#", ".");
                tableName.Rows[i]["DESCRIPTION"] = str2;
            }

            comboBox1.DataSource = tableName;
            comboBox1.DisplayMember = "DESCRIPTION";
            comboBox1.ValueMember = "TABLE_NAME";
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Schedual form = (Schedual)this.Owner;
            form.tableName = comboBox1.SelectedValue.ToString();
            form.TableNameHasChoosed = true;
            this.Close();
        }
    }
}
