using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin.machine
{
    public partial class add_alter_machine : Form
    {
        public bool alter = false;
        public string ID = "";
        public add_alter_machine()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("请输入设备名称");
                return;
            }

            if (textBox2.Text.Length == 0)
            {
                MessageBox.Show("请输入IP地址");
                return;
            }

            if (textBox3.Text.Length == 0)
            {
                MessageBox.Show("请输入端口号");
                return;
            }

            if (textBox4.Text.Length == 0)
            {
                MessageBox.Show("请输入密码");
                return;
            }

            string sql = "";

            if (alter == true)
            {
                sql = string.Format("insert into KQ_Machine (Machine,IP,Port,Password) values ('{0}','{1}','{2}','{3}')", textBox1.Text.Trim(), textBox2.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim());
            }else
            {
                sql = string.Format("update from  KQ_Machine set Machine='{0}',IP='{1}',Port='{2}',Password='{3}' where ID='{4}'", textBox1.Text.Trim(), textBox2.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim());
            }
            
            try
            {
                GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }

        }

        private void add_alter_machine_Load(object sender, EventArgs e)
        {
            if (alter == true)
            {
                this.Text = "修改设备信息";
                simpleButton1.Text = "修改";
            }else
            {
                this.Text = "添加设备";
            }
        }
    }
}
