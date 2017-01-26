using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace KaoQin
{
    public partial class Main : Form
    {
        DataTable Department = new DataTable();
        DataTable Attendance = new DataTable();
        DataTable Authority = new DataTable();
        bool Authority_Dep = false;
        bool Authority_Dep_Edit = false;
        bool Authority_Dep_Del = false;
        bool Authority_Device = false;
        bool Authority_Device_Edit = false;
        bool Authority_Device_Del = false;
        bool Authority_Shift = false;
        bool Authority_Shift_Edit = false;
        bool Authority_Shift_Del = false;
        bool Authority_Arrangement = false;
        bool Authority_Arrangement_Edit = false;
        bool Authority_Arrangement_Del = false;
        bool Authority_Mangement = false;
        bool Authority_Mangement_Edit = false;
        bool Authority_Mangement_Del = false;
        bool Authority_Attendance = false;
        delegate void UpdateUI();
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            searchControl1.Properties.NullValuePrompt = "请输入部门名称";
            UILocation();
            SearchAuthority();//授权管理
            if (Authority_Arrangement==true)
            {
                SearchDepartment();//查找部门
            }
           
        }

        private void UILocation()
        {
            int height = (panelControl2.Height - ButtonAdd.Height) / 2;
            ButtonAdd.Location = new Point(ButtonAdd.Location.X, height);
            ButtonAlter.Location = new Point(ButtonAlter.Location.X, height);
            ButtonDelete.Location = new Point(ButtonDelete.Location.X, height);
            ButtonRefresh.Location = new Point(ButtonRefresh.Location.X, height);

            ButtonRefresh1.Location = new Point(ButtonRefresh1.Location.X, height);
            searchControl1.Location= new Point(searchControl1.Location.X, (panelControl2.Height - searchControl1.Height) / 2);
        }


        private void SearchAuthority()
        {
            if (GlobalHelper.UserHelper.User["U_NAME"].ToString() == "MZSYS")
            {
                Authority_Dep = true;
                Authority_Dep_Edit = true;
                Authority_Dep_Del = true;
                Authority_Device = true;
                Authority_Device_Edit = true;
                Authority_Device_Del = true;
                Authority_Shift = true;
                Authority_Shift_Edit = true;
                Authority_Shift_Del = true;
                Authority_Arrangement = true;
                Authority_Arrangement_Edit = true;
                Authority_Arrangement_Del = true;
                Authority_Mangement = true;
                Authority_Mangement_Edit = true;
                Authority_Mangement_Del = true;
                Authority_Attendance = true;
                return;
            }
            
            try
            {
                string sql =string.Format("select * from KQ_SQ where ID='{0}'", GlobalHelper.UserHelper.User["U_ID"].ToString());
                Authority = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }

            if (Authority.Rows.Count == 0)
            {
                return;
            }else
            {
                //排班
                if (Authority.Rows[0]["PBGL"].ToString() == "111")
                {
                    Authority_Arrangement = true;
                    Authority_Arrangement_Edit = true;
                    Authority_Arrangement_Del = true;
                }
                else
                {
                    string s1 = Authority.Rows[0]["PBGL"].ToString().Substring(0, 1);
                    string s2 = Authority.Rows[0]["PBGL"].ToString().Substring(1, 1);
                    string s3 = Authority.Rows[0]["PBGL"].ToString().Substring(2, 1);
                    Authority_Arrangement = s1 == "1" ? true : false;
                    Authority_Arrangement_Edit = s2 == "1" ? true : false;
                    Authority_Arrangement_Del = s3 == "1" ? true : false;
                }
                //员工
                if (Authority.Rows[0]["YGGL"].ToString() == "111")
                {
                    Authority_Dep = true;
                    Authority_Dep_Edit = true;
                    Authority_Dep_Del = true;
                }
                else
                {
                    string s1 = Authority.Rows[0]["YGGL"].ToString().Substring(0, 1);
                    string s2 = Authority.Rows[0]["YGGL"].ToString().Substring(1, 1);
                    string s3 = Authority.Rows[0]["YGGL"].ToString().Substring(2, 1);
                    Authority_Dep = s1 == "1" ? true : false;
                    Authority_Dep_Edit = s2 == "1" ? true : false;
                    Authority_Dep_Del = s3 == "1" ? true : false;
                }
                //设备
                if (Authority.Rows[0]["SBGL"].ToString() == "111")
                {
                    Authority_Device = true;
                    Authority_Device_Edit = true;
                    Authority_Device_Del = true;
                }
                else
                {
                    string s1 = Authority.Rows[0]["SBGL"].ToString().Substring(0, 1);
                    string s2 = Authority.Rows[0]["SBGL"].ToString().Substring(1, 1);
                    string s3 = Authority.Rows[0]["SBGL"].ToString().Substring(2, 1);
                    Authority_Device = s1 == "1" ? true : false;
                    Authority_Device_Edit = s2 == "1" ? true : false;
                    Authority_Device_Del = s3 == "1" ? true : false;
                }
                //班次
                if (Authority.Rows[0]["BCGL"].ToString() == "111")
                {
                    Authority_Shift = true;
                    Authority_Shift_Edit = true;
                    Authority_Shift_Del = true;
                }
                else
                {
                    string s1 = Authority.Rows[0]["BCGL"].ToString().Substring(0, 1);
                    string s2 = Authority.Rows[0]["BCGL"].ToString().Substring(1, 1);
                    string s3 = Authority.Rows[0]["BCGL"].ToString().Substring(2, 1);
                    Authority_Shift = s1 == "1" ? true : false;
                    Authority_Shift_Edit = s2 == "1" ? true : false;
                    Authority_Shift_Del = s3 == "1" ? true : false;
                }

                //考勤
                if (Authority.Rows[0]["KQGL"].ToString() == "1")
                {
                    Authority_Attendance = true;
                }
                else
                {
                    Authority_Attendance = false;
                }

                //授权
                if (Authority.Rows[0]["SQGL"].ToString() == "111")
                {
                    Authority_Mangement = true;
                    Authority_Mangement_Edit = true;
                    Authority_Mangement_Del = true;
                }
                else
                {
                    string s1 = Authority.Rows[0]["SQGL"].ToString().Substring(0, 1);
                    string s2 = Authority.Rows[0]["SQGL"].ToString().Substring(1, 1);
                    string s3 = Authority.Rows[0]["SQGL"].ToString().Substring(2, 1);
                    Authority_Mangement = s1 == "1" ? true : false;
                    Authority_Mangement_Edit = s2 == "1" ? true : false;
                    Authority_Mangement_Del = s3 == "1" ? true : false;
                }
            }
        }
        private void SearchDepartment()
        {
            string sql = "select BMID,BMMC,BMLB from KQ_BM where BMID>0";

            try
            {
                Department = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                gridControl1.DataSource = Department;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }

        }
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Schedual form = new Schedual();
                form.DepartmentID = gridView1.GetFocusedRowCellValue("BMID").ToString();
                form.DepartmentName = gridView1.GetFocusedRowCellValue("BMMC").ToString();
                form.Show(this);
            }
            catch { }

        }

        private void 机器设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Authority_Device == false)
            {
                MessageBox.Show("您没有修改设备信息的权利！");
                return;
            }

            machine.machine form = new machine.machine();
            form.Show();
        }

        private void 员工设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Authority_Dep == false)
            {
                MessageBox.Show("您没有修改部门与员工信息的权利！");
                return;
            }

            users.MainUsers form = new users.MainUsers();
            form.Show();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SearchDepartment();
        }

        private void searchControl1_TextChanged(object sender, EventArgs e)
        {
            if (Department.Rows.Count > 0)
            {
                Department.DefaultView.RowFilter = string.Format("BMMC like '%{0}%'", searchControl1.Text);
            }

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                Thread t1 = new Thread(SearchAttendance);
                t1.IsBackground = true;
                t1.Start();
            }

        }

        private void SearchAttendance()
        {
            string sql = string.Format("select PBID,BMID,YEAR,MONTH,CJR,CJSJ,XGR,XGSJ from KQ_PB where BMID='{0}'", gridView1.GetFocusedRowCellValue("BMID").ToString());

            this.BeginInvoke(new UpdateUI(delegate ()
            {
                try
                {
                    Attendance = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                    gridControl2.DataSource = Attendance;
                    gridView2.BestFitColumns();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误1:" + ex.Message, "提示");
                    return;
                }
            }));
        }

        private void 排班维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            arrangement.arrange form = new arrangement.arrange();
            form.Show();
        }

        private void ButtonAlter_Click(object sender, EventArgs e)
        {
            try
            {
                Schedual form = new Schedual();
                form.DepartmentID = gridView1.GetFocusedRowCellValue("BMID").ToString();
                form.DepartmentName = gridView1.GetFocusedRowCellValue("BMMC").ToString();
                form.comboBoxYear.Items.Add(gridView2.GetFocusedRowCellValue("YEAR").ToString());
                form.comboBoxYear.Text = gridView2.GetFocusedRowCellValue("YEAR").ToString();
                form.comboBoxMonth.Text = gridView2.GetFocusedRowCellValue("MONTH").ToString();
                form.PBID = gridView2.GetFocusedRowCellValue("PBID").ToString();
                form.alter = true;
                form.Show(this);
            }
            catch { }
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            gridControl1_Click(null, null);
        }

        private void 考勤管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Attendance form = new Attendance();
            form.Show();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            LoadingForm form = new LoadingForm();
            form.Show();
        }

        private void gridControl2_DoubleClick(object sender, EventArgs e)
        {
            ButtonAlter_Click(null, null);
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount == 0 || gridView2.RowCount == 0)
            {
                return;
            }

            if (MessageBox.Show(string.Format("是否删除 {0}{1}{2}的排班计划？", gridView1.GetFocusedRowCellDisplayText("BMMC").ToString(), gridView2.GetFocusedRowCellDisplayText("YEAR").ToString(), gridView2.GetFocusedRowCellDisplayText("MONTH").ToString()), "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            try
            {
                string sql = string.Format("delete from KQ_PB  where PBID='{0}';", gridView2.GetFocusedRowCellValue("PBID").ToString())
               + string.Format("delete from KQ_PB_XB  where PBID='{0}'", gridView2.GetFocusedRowCellValue("PBID").ToString())
               + string.Format("delete from KQ_PB_LD  where PBID='{0}'", gridView2.GetFocusedRowCellValue("PBID").ToString());
                GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }

            string sql_del = string.Format("select max(ID) from KQ_LOG");
            string ID = "";
            DataTable MaxID = new DataTable();
            try
            {
                MaxID = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql_del);
                if (MaxID.Rows[0][0].ToString() == "")
                {
                    ID = "1";
                }
                else
                {
                    ID = (Convert.ToInt32(MaxID.Rows[0][0].ToString()) + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误2:"+ex.Message);
                return;
            }

            string Record = string.Format("{0}删除了{1}{2}{3}的排班记录", GlobalHelper.UserHelper.User["U_NAME"].ToString(), gridView1.GetFocusedRowCellValue("BMMC").ToString(), gridView2.GetFocusedRowCellValue("YEAR").ToString(), gridView2.GetFocusedRowCellValue("MONTH").ToString());
            string sql1 = string.Format("insert into KQ_LOG (ID,Record,Time) values ('{0}','{1}','{2}')", ID, Record, GlobalHelper.IDBHelper.GetServerDateTime());
            try
            {
               GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误2:" + ex.Message, "提示");
                return;
            }

            //刷新
            gridControl1_Click(null, null);
        }

        private void 授权管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            authority.Authority form = new authority.Authority();
            form.Show();
        }
    }
}
