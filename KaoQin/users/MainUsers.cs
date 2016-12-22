using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin.users
{
    public partial class MainUsers : Form
    {
        DataTable Staff = new DataTable();
        DataTable Department = new DataTable();
        public MainUsers()
        {
            InitializeComponent();
        }

        private void gridControl2_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void MainUsers_Load(object sender, EventArgs e)
        {
            searchControl1.Properties.NullValuePrompt = "请输入考勤号或姓名";
            searchControl2.Properties.NullValuePrompt = " ";
            SearchDepartment();
        }

        private void SearchDepartment()
        {
            string sql = "select BMID,BMMC,BMLX from KQ_BM";

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
            add_alter_Users form = new add_alter_Users();
            form.Show(this);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            string sql = "select KQID,YGXM,BMID,RZSJ,ZT,SM from KQ_YG where BMID ";

            try
            {
                Staff = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                gridControl2.DataSource = Staff;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void ButtonAlter_Click(object sender, EventArgs e)
        {
            try
            {
                add_alter_Users form = new add_alter_Users();
                form.alter = true;
                form.comboBox1.Text =  gridView1.GetFocusedRowCellDisplayText("").ToString();
                form.comboBox2.Text= gridView1.GetFocusedRowCellDisplayText("").ToString();
                form.textBox2.Text= gridView1.GetFocusedRowCellDisplayText("").ToString();
                form.textBox3.Text= gridView1.GetFocusedRowCellDisplayText("").ToString();
                form.dateEdit1.Text= gridView1.GetFocusedRowCellDisplayText("").ToString();
                form.textBox1.Text = gridView1.GetFocusedRowCellDisplayText("").ToString();
                form.Show(this);
            }
            catch { }
            
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string sql = "select a.KQID,a.YGXM,a.BMID,a.RZSJ,a.ZT,a.SM from KQ_YG a left join KQ_BM b on a.BMID=b.BMID";

            try
            {
                Staff = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                gridControl2.DataSource = Staff;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:" + ex.Message);
                return;
            }
        }

        private void searchControl1_TextChanged(object sender, EventArgs e)
        {
            Staff.DefaultView.RowFilter = string.Format("YGXM like '%{0}%' or KQID like '%{0}%'", searchControl1.Text);
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            Search form = new Search();
            form.Show();
        }
    }
}
