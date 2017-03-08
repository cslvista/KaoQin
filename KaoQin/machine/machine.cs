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
        public bool Authority_Device_Edit = false;
        public bool Authority_Device_Del = false;
        public machine()
        {
            InitializeComponent();

        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (Authority_Device_Edit == false)
            {
                MessageBox.Show("您没有操作的权限！");
                return;
            }

            add_alter_machine form = new add_alter_machine();
            form.Show(this);
        }

        private void ButtonAlter_Click(object sender, EventArgs e)
        {
            if (Authority_Device_Edit == false)
            {
                MessageBox.Show("您没有操作的权限！");
                return;
            }

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
            
            
            try
            {
                string sql = "select ID,Machine,IP,Port,Password from KQ_Machine";
                Machine = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
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
            Machine.Columns.Add("ID", typeof(string));
            Machine.Columns.Add("Machine", typeof(string));
            Machine.Columns.Add("IP", typeof(string));
            Machine.Columns.Add("Port", typeof(string));
            Machine.Columns.Add("Password", typeof(string));            
            simpleButton1_Click(null, null);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (Machine.Rows.Count == 0)
            {
                MessageBox.Show("没有可用设备！");
                return;
            }

            bool bIsConnected = false;//判断设备是否可连接

            try
            {
                //判断存不存在这一列
                string test = Machine.Rows[0]["Status"].ToString();
            }
            catch
            {
                Machine.Columns.Add("Status", typeof(string));
            }
            
            for (int i = 0; i < Machine.Rows.Count; i++)
            {
                try
                {
                    if (Machine.Rows[i]["Password"].ToString() == "")
                    {
                        
                    }else
                    {
                        DKJ.SetCommPassword(Convert.ToInt32(Machine.Rows[i]["Password"].ToString()));
                    }
                    
                    bIsConnected = DKJ.Connect_Net(Machine.Rows[i]["IP"].ToString(), Convert.ToInt32(Machine.Rows[i]["Port"].ToString()));
                    if (bIsConnected==false)
                    {
                        Machine.Rows[i]["Status"] = "连接失败";
                    }else
                    {
                        Machine.Rows[i]["Status"] = "连接成功";
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
            if (Authority_Device_Del == false)
            {
                MessageBox.Show("您没有操作的权限！");
                return;
            }

            string sql = "";
            try
            {
                if (MessageBox.Show(string.Format("是否删除设备'{0}'?", gridView1.GetFocusedRowCellValue("Machine").ToString()), "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)                    
                {
                    return;
                }

                sql = string.Format("Delete from KQ_Machine where ID='{0}'", gridView1.GetFocusedRowCellDisplayText("ID").ToString());
            }
            catch
            {

            }
            
            try
            {
                GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }

            simpleButton1_Click(null, null);

        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "Status")
            {
                string Status = gridView1.GetRowCellDisplayText(e.RowHandle, gridView1.Columns["Status"]);

                if (Status == "连接成功")
                {
                    e.Appearance.ForeColor = Color.Blue;
                }
                else if (Status == "连接失败")
                {
                    e.Appearance.ForeColor = Color.Red;
                }
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount == 0)
            {
                return;
            }

            if (MessageBox.Show(string.Format("是否重启设备 '{0}'?", gridView1.GetFocusedRowCellValue("Machine").ToString()), "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            try
            {
                DKJ.SetCommPassword(Convert.ToInt32(gridView1.GetFocusedRowCellDisplayText("Password").ToString()));
                if (DKJ.Connect_Net(gridView1.GetFocusedRowCellDisplayText("IP").ToString(), Convert.ToInt32(gridView1.GetFocusedRowCellDisplayText("Port").ToString())))
                {
                    DKJ.RestartDevice(0);
                    MessageBox.Show("重启成功！");
                }else
                {
                    MessageBox.Show("重启失败！");
                }
            }
            catch 
            {
                MessageBox.Show("重启失败！");
                return;
            }
            
           
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            ButtonAlter_Click(sender, e);
        }
    }
}
