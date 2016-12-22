using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace KaoQin.users
{
    public partial class Search : Form
    {
        zkemkeeper.CZKEMClass DKJ = new zkemkeeper.CZKEMClass();//打卡机
        delegate void UpdateUI();
        string sql = "";
        DataTable User = new DataTable();
        public Search()
        {
            InitializeComponent();
        }

        private void Search_Load(object sender, EventArgs e)
        {
            searchControl1.Properties.NullValuePrompt = "请输入考勤号或姓名";

            string sql = "select ID,Machine from KQ_Machine";

            DataTable Machine = new DataTable();
            Machine.Columns.Add("ID", typeof(string));
            Machine.Columns.Add("Machine", typeof(string));

            User.Columns.Add("ID", typeof(string));
            User.Columns.Add("NAME", typeof(string));

            try
            {
                Machine = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }
            comboBox1.DataSource = Machine;
            comboBox1.DisplayMember = "Machine";
            comboBox1.ValueMember = "ID";
            comboBox1.Text = null;
        }

        private void SearchStaff()
        {                    
            DataTable ConnectInfo = new DataTable();
            try
            {
                ConnectInfo = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }

            DKJ.SetCommPassword(Convert.ToInt32(ConnectInfo.Rows[0]["Password"].ToString()));
            bool bIsConnected = DKJ.Connect_Net(ConnectInfo.Rows[0]["IP"].ToString(), Convert.ToInt32(ConnectInfo.Rows[0]["Port"].ToString()));

            if (bIsConnected == false)
            {
                MessageBox.Show("连接失败！");
                return;
            }

            string sdwEnrollNumber = "";
            string sName = "";
            string sPassword = "";
            int iPrivilege = 0;
            bool bEnabled = false;

            DKJ.ReadAllUserID(0);
            while (DKJ.SSR_GetAllUserInfo(0, out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))//get all the users' information from the memory
            {
                int a;
                a = sName.IndexOf("\0");
                string name = sName.Substring(0, a);//过滤sName中多余字符
                User.Rows.Add(new object[] { sdwEnrollNumber, name });
            }
            this.BeginInvoke(new UpdateUI(delegate ()
            {
                gridControl1.DataSource = User;
            }));
            
        }

        private void searchControl1_TextChanged(object sender, EventArgs e)
        {
            User.DefaultView.RowFilter = string.Format("ID like '%{0}%' or NAME like '%{0}%'", searchControl1.Text);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            sql = string.Format("select IP,Port,Password from KQ_Machine where ID='{0}'", comboBox1.SelectedValue);
            User.Clear();
            Thread t1 = new Thread(SearchStaff);
            t1.IsBackground = true;
            t1.Start();
        }
    }
}
