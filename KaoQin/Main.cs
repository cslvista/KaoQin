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
        bool Authority_Device = false;
        bool Authority_Arrangement = false;
        bool Authority_Mangement = false;
        delegate void UpdateUI();
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            searchControl1.Properties.NullValuePrompt = "请输入部门名称";
            SearchAuthority();//授权管理
            SearchDepartment();//查找部门
        }


        private void SearchAuthority()
        {
            if (GlobalHelper.UserHelper.User["U_NAME"].ToString() == "MZSYS")
            {
                Authority_Dep = true;
                Authority_Device = true;
                Authority_Arrangement = true;
                Authority_Mangement = true;
            }

            string sql = "select * from KQ_SQ";

            try
            {
                Authority = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
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
               + string.Format("delete from KQ_PB_XB  where PBID='{0}'", gridView2.GetFocusedRowCellValue("PBID").ToString());
                GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }

            string Record = string.Format("{0}删除了{1}{2}{3}的排班记录", GlobalHelper.UserHelper.User["U_NAME"].ToString(), gridView1.GetFocusedRowCellValue("BMMC").ToString(), gridView2.GetFocusedRowCellValue("YEAR").ToString(), gridView2.GetFocusedRowCellValue("MONTH").ToString());
            string sql1 = string.Format("insert into KQ_LOG (Record,Time) values ('{0}','{1}')", Record, GlobalHelper.IDBHelper.GetServerDateTime());
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
