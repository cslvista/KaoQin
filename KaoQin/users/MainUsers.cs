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
    public partial class MainUsers : Form
    {
        DataTable Staff = new DataTable();
        DataTable Department = new DataTable();
        delegate void UpdateUI();
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
            toolStripButtonRefresh_Click(null, null);
        }


        private void ButtonAdd_Click(object sender, EventArgs e)
        {           
            try
            {
                if (gridView1.GetFocusedRowCellDisplayText("BMID") == "0")
                {
                    return;
                }else
                {
                    add_alter_Users form = new add_alter_Users();
                    form.department = gridView1.GetFocusedRowCellDisplayText("BMMC");
                    form.Show(this);
                    form.textBox2.Focus();
                }
                
            }
            catch { }
            
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
            SearchMachine form = new SearchMachine();
            form.Show();
        }

        private void searchControl2_TextChanged(object sender, EventArgs e)
        {
            Department.DefaultView.RowFilter = string.Format("BMMC like '%{0}%'", searchControl2.Text);
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            add_alter_Dep form = new add_alter_Dep();
            form.Show(this);
        }

        public void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            string sql = "select a.BMID,a.BMMC,b.BMLB from KQ_BM a left join KQ_BMLB b on a.BMLX=b.ID";

            try
            {
                Department = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                gridControl1.DataSource = Department;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
            }
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.GetFocusedRowCellDisplayText("BMID").ToString() == "0")
                {
                    MessageBox.Show("该项不可修改！");
                    return;
                }

                add_alter_Dep form = new add_alter_Dep();
                form.Show(this);
            }
            catch { }
            
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            Thread t1 = new Thread(SearchUsers);
            t1.IsBackground = true;
            t1.Start();
        }

        private void SearchUsers()
        {
            string sql =string.Format( "");
            this.BeginInvoke(new UpdateUI(delegate ()
            {
                try
                {
                    //Reservation = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql.ToString());
                    //gridControl1.DataSource = Reservation;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误:" + ex.Message, "提示");
                    return;
                }

            }));
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (gridView1.GetFocusedRowCellDisplayText("BMID").ToString() == "0")
            {
                MessageBox.Show("该项不可删除！");
                return;
            }


        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "BMMC")
            {
                string BMMC = gridView1.GetRowCellDisplayText(e.RowHandle, gridView1.Columns["BMMC"]);

                if (BMMC == "未分类员工")
                {
                    e.Appearance.ForeColor = Color.Red;
                }
            }
        }
    }
}
