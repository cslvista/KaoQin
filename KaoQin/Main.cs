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
        delegate void UpdateUI();
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            searchControl1.Properties.NullValuePrompt = "请输入部门名称";
            SearchDepartment();//查找部门
        }

        private void SearchDepartment()
        {
            string sql = "select BMID,BMMC,BMLB from KQ_BM where BMID>0";

            try
            {
                Department=GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
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
            machine.machine form = new machine.machine();
            form.Show();
        }

        private void 员工设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            users.MainUsers form = new users.MainUsers();
            form.Show();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SearchDepartment();
        }

        private void searchControl1_TextChanged(object sender, EventArgs e)
        {
            Department.DefaultView.RowFilter = string.Format("BMMC like '%{0}%'", searchControl1.Text);
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
                form.comboBoxYear.Text= gridView2.GetFocusedRowCellValue("YEAR").ToString();
                form.comboBoxMonth.Text = gridView2.GetFocusedRowCellValue("MONTH").ToString();
                form.PBID= gridView2.GetFocusedRowCellValue("PBID").ToString();
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

        }
    }
}
