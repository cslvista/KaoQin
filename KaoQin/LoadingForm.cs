using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;


namespace KaoQin
{
    public partial class LoadingForm : Form
    {
        Thread t1;
        delegate void UpdateUI();
        public LoadingForm()
        {
            InitializeComponent();
        }

        
        private void Loading_Load(object sender, EventArgs e)
        {            
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Width = 300;
            pictureBox1.Height = 243;
            simpleButton1.Location = new Point(0, pictureBox1.Height-3);
            simpleButton1.Width = 300;

            this.Width = 300;
            this.Height = pictureBox1.Height+simpleButton1.Height;

            t1 = new Thread(DownloadData);
            t1.IsBackground = true;
            t1.Start();
        }

        private void DownloadData()
        {
            //读取考勤机数据
            zkemkeeper.CZKEMClass DKJ = new zkemkeeper.CZKEMClass();//打卡机
            DataTable Machine = new DataTable();//机器信息
            DataTable Staff_Orign = new DataTable();//打卡机的原始员工数据，包括考勤号和姓名
            DataTable Record_DKJ = new DataTable();//考勤机原始数据

            Staff_Orign.Columns.Add("ID", typeof(string));
            Staff_Orign.Columns.Add("Name", typeof(string));

            Record_DKJ.Columns.Add("ID", typeof(string));
            Record_DKJ.Columns.Add("Time", typeof(string));
            Record_DKJ.Columns.Add("Source", typeof(string));

            string sql = "select ID,Machine,IP,Port,Password from KQ_Machine";

            try
            {
                Machine = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message);
                return;
            }

            if (Machine.Rows.Count == 0)
            {
                MessageBox.Show("没有可用设备！");
                return;
            }

            DKJ.SetCommPassword(Convert.ToInt32(Machine.Rows[0]["Password"].ToString()));
            DKJ.Connect_Net(Machine.Rows[0]["IP"].ToString(), Convert.ToInt32(Machine.Rows[0]["Port"].ToString()));
            bool bIsConnected = false;//判断设备是否可连接   
            string sdwEnrollNumber = "";
            string sName = "";
            string sPassword = "";
            int iPrivilege = 0;
            bool bEnabled = false;
            
            DKJ.ReadAllUserID(0);
            
            while (DKJ.SSR_GetAllUserInfo(0, out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))//get all the users' information from the memory
            {
                int position = sName.IndexOf("\0");
                string name = sName.Substring(0, position);//过滤sName中多余字符
                Staff_Orign.Rows.Add(new object[] { sdwEnrollNumber, name });                
            }

            int iMachineNumber = 0;
            int VerifyMode = 0;
            int InOutMode = 0;
            int Year = 0;
            int Month = 0;
            int Day = 0;
            int Hour = 0;
            int Minute = 0;
            int Second = 0;
            int Workcode = 0;
            string dwEnrollNumber = "";
            for (int i = 1; i < Machine.Rows.Count; i++)
            {
                DKJ.SetCommPassword(Convert.ToInt32(Machine.Rows[i]["Password"].ToString()));
                bIsConnected = DKJ.Connect_Net(Machine.Rows[i]["IP"].ToString(), Convert.ToInt32(Machine.Rows[i]["Port"].ToString()));
                if (bIsConnected == false)
                {
                    MessageBox.Show(string.Format("'{0}'无法连接！", Machine.Rows[i]["Machine"].ToString()));
                    return;
                }

                DKJ.ReadAllGLogData(iMachineNumber);//read all the user information to the memory
                while (DKJ.SSR_GetGeneralLogData(iMachineNumber, out dwEnrollNumber, out VerifyMode, out InOutMode, out Year, out Month, out Day, out Hour, out Minute, out Second, ref Workcode))
                {
                    string time = string.Format("{0}-{1}-{2} {3}:{4}:{5}", Year, Month, Day, Hour, Minute, Second);
                    string time1 = Convert.ToDateTime(time).ToString("yyyy-MM-dd  HH:mm:ss");
                    Record_DKJ.Rows.Add(new object[] { dwEnrollNumber, time1, Machine.Rows[i]["Machine"].ToString() });
                }
            }
            //下载数据完毕
            this.BeginInvoke(new UpdateUI(delegate ()
            {
                Attendance form = (Attendance)this.Owner;
                form.Staff_Orign = Staff_Orign.Copy();
                form.Record_DKJ.Merge(Record_DKJ);
                form.HasDownload = true;
                form.ButtonCal.Enabled = true;
                form.ButtonOrignData.Enabled = true;
                MessageBox.Show("数据已经下载完成，请选择相应部门并点击'查询计算'按钮查看考勤结果！");
                this.Close();                
            }));            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DownloadData();
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("注意：强制停止会关闭整个程序，是否继续？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            Application.Exit();
        }

        public void CloseWin()
        {
            this.Close();
        }
    }
}
