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
    public partial class machine : Form
    {
        public zkemkeeper.CZKEMClass DKJ = new zkemkeeper.CZKEMClass();//打卡机

        DataTable Machine = new DataTable();
        public machine()
        {
            InitializeComponent();
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            add_alter_machine form = new add_alter_machine();
            form.Show(this);
        }

        private void ButtonAlter_Click(object sender, EventArgs e)
        {
            add_alter_machine form = new add_alter_machine();
            try
            {
                form.ID = gridView1.GetFocusedRowCellDisplayText("ID").ToString();
                form.textBox1.Text = gridView1.GetFocusedRowCellDisplayText("Machine").ToString();
                form.textBox2.Text = gridView1.GetFocusedRowCellDisplayText("IP").ToString();
                form.textBox3.Text = gridView1.GetFocusedRowCellDisplayText("Port").ToString();
                form.textBox4.Text = gridView1.GetFocusedRowCellDisplayText("Password").ToString();
                form.alter = true;
                form.Show(this);
            }
            catch { }
            
        }

        public void simpleButton1_Click(object sender, EventArgs e)
        {
            string sql = "select ID,Machine,IP,Port,Password from KQ_Machine";
            
            try
            {
                Machine = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                gridControl1.DataSource = Machine;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }
        }

        private void machine_Load(object sender, EventArgs e)
        {
            simpleButton1_Click(null, null);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            bool bIsConnected = false;//判断设备是否可连接

            for (int i = 0; i < Machine.Rows.Count; i++)
            {
                try
                {
                    DKJ.SetCommPassword(Convert.ToInt32(Machine.Rows[i]["Password"].ToString()));
                    bIsConnected = DKJ.Connect_Net(Machine.Rows[i]["IP"].ToString(), Convert.ToInt32(Machine.Rows[i]["Port"].ToString()));
                    if (bIsConnected==false)
                    {

                    }else
                    {

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;                    
                }
            }
            

        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否删除？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            string sql = string.Format("Delete from KQ_Machine");

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
    }
}
