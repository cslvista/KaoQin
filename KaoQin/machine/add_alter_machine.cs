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
        bool success = false;
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


            if (alter == true)
            {
                success = Alter();
            }
            else
            {
                success = Add();
            }

            //更新主界面
            if (success == true)
            {
                machine form = (machine)this.Owner;
                form.simpleButton1_Click(null, null);
                this.Close();
            }         

        }

        private bool Alter()
        {

           string sql = string.Format("update KQ_Machine set Machine='{0}',IP='{1}',Port='{2}',Password='{3}' where ID='{4}'", textBox1.Text.Trim(), textBox2.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim(),ID);

            try
            {
                GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return false;
            }
        }

        private bool Add()
        {
            string sql = "select max(ID) from KQ_Machine";

            DataTable Max_ID = new DataTable();
            string ID="";
            try
            {
                Max_ID=GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                if (Max_ID.Rows[0][0].ToString() =="")
                {
                    ID = "1";
                }else
                {
                    ID = (Convert.ToInt32(Max_ID.Rows[0][0].ToString()) + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return false;
            }

           string sql1=string.Format("insert into KQ_Machine (ID,Machine,IP,Port,Password) values ('{0}','{1}','{2}','{3}','{4}')", ID,textBox1.Text.Trim(), textBox2.Text.Trim(), textBox3.Text.Trim(), textBox4.Text.Trim());

            try
            {
                GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql1);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误2:" + ex.Message, "提示");
                return false;
            }
        }

        private void add_alter_machine_Load(object sender, EventArgs e)
        {
            if (alter == true)
            {
                this.Text = "修改设备信息";
            }else
            {
                this.Text = "添加设备信息";
            }
            UILocation();

        }

        private void UILocation()
        {
            int x = 3;
            int y = 2;
            label3.Location = new Point(textBox1.Location.X - label3.Width - x, textBox1.Location.Y + y);
            label1.Location = new Point(textBox2.Location.X - label1.Width - x, textBox2.Location.Y + y);
            label2.Location = new Point(textBox3.Location.X - label2.Width - x, textBox3.Location.Y + y);
            label4.Location = new Point(textBox4.Location.X - label4.Width - x, textBox4.Location.Y + y);

        }
    }
}
