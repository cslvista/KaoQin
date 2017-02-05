using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraPrinting;

namespace KaoQin
{
    public partial class OrignData : Form
    {
        public DataTable Record_DKJ = new DataTable();
        public DataTable Staff = new DataTable();//打卡机上的员工数据
        DataTable Machine = new DataTable();//考勤机数据
        DataTable Record_DKJ_new = new DataTable();//连接Record_DKJ和Staff数据后的表
        bool allowVisit = false;//是否允许访问Record_DKJ_new
        public OrignData()
        {
            InitializeComponent();
        }

        private void OrignData_Load(object sender, EventArgs e)
        {
            searchControl1.Properties.NullValuePrompt = "请输入考勤号或者姓名";
            dateEdit1.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit1.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit2.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit2.Properties.Mask.EditMask = "yyyy-MM-dd";
            Record_DKJ_new.Columns.Add("ID", typeof(string));
            Record_DKJ_new.Columns.Add("Name", typeof(string));
            Record_DKJ_new.Columns.Add("Time", typeof(DateTime));
            Record_DKJ_new.Columns.Add("Source", typeof(string));
            if (Staff.Rows.Count == 0)
            {
                if (MessageBox.Show(string.Format("员工姓名为空，是否需要下载员工信息？"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    SearchStaff();
                }
            }
            //联结两个表
            var query = from rec in Record_DKJ.AsEnumerable()
                        join staff in Staff.AsEnumerable()
                        on rec.Field<string>("ID") equals staff.Field<string>("ID") into AllInfo
                        from allInfo in AllInfo.DefaultIfEmpty()
                        select new
                        {
                            ID = rec.Field<string>("ID"),
                            Name = allInfo == null ? "" : allInfo["Name"],
                            Time = rec.Field<string>("Time"),
                            Source = rec.Field<string>("Source"),
                        };
            try
            {                
                foreach (var obj in query)
                {
                    Record_DKJ_new.Rows.Add(obj.ID, obj.Name, Convert.ToDateTime(obj.Time), obj.Source);
                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Record_DKJ_new.DefaultView.Sort = "Time";
            gridControl1.DataSource = Record_DKJ_new;
            allowVisit = false;
            dateEdit1.Text = Convert.ToDateTime(Record_DKJ_new.Rows[0]["Time"]).ToString("yyyy-MM-dd");
            allowVisit = true;
            dateEdit2.Text = Convert.ToDateTime(Record_DKJ_new.Rows[Record_DKJ_new.Rows.Count-1]["Time"]).ToString("yyyy-MM-dd");
        }

        private void SearchStaff()
        {
            zkemkeeper.CZKEMClass DKJ = new zkemkeeper.CZKEMClass();//打卡机
            
            try
            {
                string sql = "select ID,Machine,IP,Port,Password from KQ_Machine";
                Machine = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
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
            bool isConnected = false;
            DKJ.SetCommPassword(Convert.ToInt32(Machine.Rows[0]["Password"].ToString()));
            isConnected=DKJ.Connect_Net(Machine.Rows[0]["IP"].ToString(), Convert.ToInt32(Machine.Rows[0]["Port"].ToString()));
            if (isConnected == false)
            {
                MessageBox.Show(string.Format("'{0}'无法连接！", Machine.Rows[0]["Machine"].ToString()));
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
                int position = sName.IndexOf("\0");
                string name = sName.Substring(0, position);//过滤sName中多余字符
                Staff.Rows.Add(new object[] {sdwEnrollNumber, name});
            }
            Attendance form =  (Attendance)this.Owner;
            form.Staff_Orign = Staff.Copy();
           
        }

        private void SearchInfo()
        {
            if (allowVisit == true)
            {
                string StopTime = Convert.ToDateTime(dateEdit2.Text).AddDays(1).ToString("yyyy-MM-dd");
                Record_DKJ_new.DefaultView.RowFilter = string.Format("Time>='{0}' and Time<='{1}' and  ( ID like '%{2}%' or Name like '%{2}%' )", dateEdit1.Text, StopTime, searchControl1.Text);
            }
            
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel文件(*.xlsx)|*.xlsx|所有文件(*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName.ToString();
                try
                {
                    XlsxExportOptions options = new XlsxExportOptions();
                    options.ExportMode = XlsxExportMode.SingleFile;
                    options.TextExportMode = TextExportMode.Text;
                    options.RawDataMode = false;
                    gridControl1.ExportToXlsx(path, options);
                    if (MessageBox.Show("是否打开？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
                        return;
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
                catch
                {
                    MessageBox.Show("导出失败！");
                }
            }
        }

        private void dateEdit1_TextChanged(object sender, EventArgs e)
        {
            SearchInfo();
        }

        private void searchControl1_TextChanged(object sender, EventArgs e)
        {
            SearchInfo();
        }


        /// <summary>
        /// 保存到数据库
        /// </summary>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

                
            var query= from rec in Record_DKJ.AsEnumerable()
                       where Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(Convert.ToDateTime(dateEdit1.Text))>= 0 && Convert.ToDateTime(rec.Field<string>("Time")).CompareTo(Convert.ToDateTime(dateEdit2.Text).AddDays(1)) < 0
                       select new
                       {
                           ID = rec.Field<string>("ID"),
                           Time = rec.Field<string>("Time"),
                           Source = rec.Field<string>("Source"),
                       };

            DataTable SaveData = new DataTable();
            SaveData.Columns.Add("ID", typeof(string));
            SaveData.Columns.Add("Time", typeof(DateTime));
            SaveData.Columns.Add("Source", typeof(string));

            foreach (var obj in query)
            {
                SaveData.Rows.Add(obj.ID,Convert.ToDateTime(obj.Time),obj.Source);
            }
            SaveData.DefaultView.Sort = "Time";

            string startTime = Convert.ToDateTime(SaveData.Rows[0]["Time"]).ToString("yyyy-MM-dd HH:mm:ss");
            string stopTime =  Convert.ToDateTime(SaveData.Rows[SaveData.Rows.Count - 1]["Time"]).ToString("yyyy-MM-dd HH:mm:ss");

            if (MessageBox.Show(string.Format(" 开始时间:{0} \r\n 结束时间:{1} \r\n 记录条数:{2} \r\n 是否保存？", startTime,stopTime, SaveData.Rows.Count), "",
            MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            //求主表的ID

            string ID = "";            
            DataTable Max_ID = new DataTable();
            try
            {
                string sql = "select max(ID) from KQ_JL";
                Max_ID = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                if (Max_ID.Rows[0][0].ToString() == "")
                {
                    ID = "1";
                }
                else
                {
                    ID = (Convert.ToInt32(Max_ID.Rows[0][0].ToString()) + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }
            

            //写入主表
            try
            {
                string sql = string.Format("insert into KQ_JL (ID,KSSJ,JSSJ,JLTS,BCRID,BCR,BCSJ) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                    ID,startTime,stopTime, SaveData.Rows.Count, GlobalHelper.UserHelper.User["U_ID"].ToString(), GlobalHelper.UserHelper.User["U_NAME"].ToString(), GlobalHelper.IDBHelper.GetServerDateTime());
                GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误2:" + ex.Message, "提示");
                return;
            }

            //写入细表           

            Application.DoEvents();
            try
            {
                StringBuilder sql = new StringBuilder();
                for (int i = 0; i < SaveData.Rows.Count; i++)
                {
                    sql.Append(string.Format("insert into KQ_JL_XB (ZBID,ID,KQSJ,LY) values ('{0}','{1}','{2}','{3}');", ID,SaveData.Rows[i][0], SaveData.Rows[i][1], SaveData.Rows[i][2]));
                    if (i.ToString().Length == 4 || i==SaveData.Rows.Count-1)
                    {
                        GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql.ToString());                       
                        
                        if (i == SaveData.Rows.Count - 1)
                        {
                            
                        }
                        Application.DoEvents();
                        sql.Clear();
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误3:" + ex.Message, "提示");
                return;
            }

            MessageBox.Show("保存成功！");
        }

        private void dateEdit2_TextChanged(object sender, EventArgs e)
        {
            SearchInfo();
        }
    }
}
