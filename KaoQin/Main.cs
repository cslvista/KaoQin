using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KaoQin
{
    public partial class Main : Form
    {
        DataTable Department = new DataTable();
        DataTable Attendance = new DataTable();
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            searchControl1.Properties.NullValuePrompt = " ";
            SearchDepartment();//查找部门
        }

        private void SearchDepartment()
        {
            string sql = "select BMID,BMMC,BMLX from KQ_BM";

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
            Schedual form = new Schedual();
            form.Show(this);
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
            string sql = string.Format("select * from a");

            try
            {
                Attendance = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                gridControl2.DataSource = Attendance;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }
        }

        private void 排班维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            arrangement.arrange form = new arrangement.arrange();
            form.Show();
        }
    }
}
