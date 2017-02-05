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
        public bool Authority_Dep_Edit = false;
        public bool Authority_Dep_Del = false;
        bool searchAllUsers = false;
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
            searchControl2.Properties.NullValuePrompt = "请输入部门名称";
            gridView2.OptionsBehavior.AutoExpandAllGroups = true;
            UILocation();
            toolStripButtonRefresh_Click(null, null);
        }

        private void UILocation()
        {
            tableLayoutPanel1.RowStyles[0].Height = searchControl2.Height + toolStrip1.Height + 15;
            int height =(panelControl2.Height-ButtonAdd.Height)/2;
            ButtonAdd.Location=new Point(ButtonAdd.Location.X, height);
            ButtonAlter.Location = new Point(ButtonAlter.Location.X, height);
            ButtonDelete.Location = new Point(ButtonDelete.Location.X, height);
            ButtonRefresh.Location = new Point(ButtonRefresh.Location.X, height);
            ButtonSearch.Location = new Point(ButtonSearch.Location.X, height);
            ButtonAll.Location = new Point(ButtonAll.Location.X, height);
            searchControl1.Location = new Point(searchControl1.Location.X, (panelControl2.Height - searchControl1.Height) / 2);
        }


        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (Authority_Dep_Edit == false)
            {
                MessageBox.Show("您没有操作的权限！");
                return;
            }

            try
            {
                add_alter_Users form = new add_alter_Users();
                if (gridView1.GetFocusedRowCellDisplayText("BMID") == "0")
                {
                    form.department = "";
                }
                else
                {
                    form.department = gridView1.GetFocusedRowCellDisplayText("BMMC");
                }                
                form.Show(this);
                form.textBoxID.Focus();
                
            }
            catch { }
            
        }

        public void simpleButton3_Click(object sender, EventArgs e)
        {
            gridControl1_Click(null, null);
        }

        private void ButtonAlter_Click(object sender, EventArgs e)
        {
            if (Authority_Dep_Edit == false)
            {
                MessageBox.Show("您没有操作的权限！");
                return;
            }

            try
            {
                add_alter_Users form = new add_alter_Users();
                form.alter = true;                
                form.comboBoxState.Text =  gridView2.GetFocusedRowCellDisplayText("ZT").ToString();
                form.department = gridView2.GetFocusedRowCellDisplayText("BMMC").ToString();
                form.KQID = gridView2.GetFocusedRowCellDisplayText("KQID").ToString();
                form.textBoxID.Text= gridView2.GetFocusedRowCellDisplayText("KQID").ToString();
                form.textBoxName.Text= gridView2.GetFocusedRowCellDisplayText("YGXM").ToString();
                form.dateEdit1.Text= gridView2.GetFocusedRowCellDisplayText("RZSJ").ToString();
                form.dateEdit2.Text = gridView2.GetFocusedRowCellDisplayText("LZSJ").ToString();
                form.textBoxRemark.Text = gridView2.GetFocusedRowCellDisplayText("SM").ToString();
                form.searchAllUsers = searchAllUsers;
                form.Show(this);
            }
            catch { }
            
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (Authority_Dep_Del == false)
            {
                MessageBox.Show("您没有操作的权限！");
                return;
            }

            try
            {
                if (MessageBox.Show(string.Format("是否删除员工 '{0}'？", gridView2.GetFocusedRowCellDisplayText("YGXM").ToString()), "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            catch { }
            
            try
            {
                string sql = string.Format("delete from KQ_YG where KQID='{0}';", gridView2.GetFocusedRowCellValue("KQID").ToString())
                           + string.Format("delete from KQ_PB_XB where KQID='{0}';", gridView2.GetFocusedRowCellValue("KQID").ToString())
                           + string.Format("delete from KQ_PB_LD where KQID='{0}';", gridView2.GetFocusedRowCellValue("KQID").ToString());
                GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql);
                gridControl1_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message);
                return;
            }
        }

        public void simpleButton2_Click(object sender, EventArgs e)
        {
            string sql = "select a.KQID,b.BMMC,a.YGXM,a.BMID,a.RZSJ,a.LZSJ,a.ZT,a.SM from KQ_YG a left join KQ_BM b on a.BMID=b.BMID";

            try
            {
                Staff = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
                gridControl2.DataSource = Staff;
                gridView2.BestFitColumns();
                searchAllUsers = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:" + ex.Message);
                return;
            }
        }

        private void searchControl1_TextChanged(object sender, EventArgs e)
        {
            if (Staff.Rows.Count>0)
            {
                Staff.DefaultView.RowFilter = string.Format("YGXM like '%{0}%' or KQID like '%{0}%'", searchControl1.Text);
            }
            
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            SearchMachine form = new SearchMachine();
            form.Show();
        }

        private void searchControl2_TextChanged(object sender, EventArgs e)
        {
            if (Department.Rows.Count > 0)
            {
                Department.DefaultView.RowFilter = string.Format("BMMC like '%{0}%'", searchControl2.Text);
            }
                
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            if (Authority_Dep_Edit == false)
            {
                MessageBox.Show("您没有操作的权限！");
                return;
            }

            add_alter_Dep form = new add_alter_Dep();
            form.Show(this);
        }

        public void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            string sql = "select a.BMID,a.BMMC,b.BMLB from KQ_BM a left join KQ_BMLB b on a.BMLB=b.ID";

            try
            {
                Department = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
                gridControl1.DataSource = Department;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
            }
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (Authority_Dep_Edit == false)
            {
                MessageBox.Show("您没有操作的权限！");
                return;
            }


            try
            {
                if (gridView1.GetFocusedRowCellDisplayText("BMID").ToString() == "0")
                {
                    MessageBox.Show("该项不可修改！");
                    return;
                }

                add_alter_Dep form = new add_alter_Dep();
                form.alter = true;
                form.textBox1.Text = gridView1.GetFocusedRowCellDisplayText("BMMC").ToString();
                form.type= gridView1.GetFocusedRowCellDisplayText("BMLB").ToString();
                form.BMID= gridView1.GetFocusedRowCellDisplayText("BMID").ToString();
                form.Show(this);
            }
            catch { }
            
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount == 0)
            {
                return;
            }
            else
            {
                Thread t1 = new Thread(SearchStaff);
                t1.IsBackground = true;
                t1.Start();
                searchAllUsers = false;
            }            
        }

        private void SearchStaff()
        {
            
            this.BeginInvoke(new UpdateUI(delegate ()
            {
                try
                {
                    string sql = string.Format("select a.KQID,b.BMMC,a.YGXM,a.BMID,a.RZSJ,a.LZSJ,a.ZT,a.SM from KQ_YG a left join KQ_BM b on a.BMID=b.BMID where a.BMID='{0}'", gridView1.GetFocusedRowCellValue("BMID").ToString());
                    Staff = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql.ToString());
                    gridControl2.DataSource = Staff;
                    gridView2.BestFitColumns();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误1:" + ex.Message, "提示");
                    return;
                }

            }));
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (Authority_Dep_Del == false)
            {
                MessageBox.Show("您没有操作的权限！");
                return;
            }


            if (gridView1.GetFocusedRowCellDisplayText("BMID").ToString() == "0")
            {
                MessageBox.Show("该项不可删除！");
                return;
            }

            try
            {
                if (MessageBox.Show(string.Format("是否删除'{0}'？", gridView1.GetFocusedRowCellDisplayText("BMMC").ToString()), "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            catch
            {
                return;
            }
                     
            try
            {
                string sql = string.Format("delete from KQ_BM where BMID='{0}';", gridView1.GetFocusedRowCellValue("BMID").ToString())
                       + string.Format("delete from KQ_PB where BMID='{0}';", gridView1.GetFocusedRowCellValue("BMID").ToString())
                       + string.Format("delete from KQ_PB_XB where BMID='{0}';", gridView1.GetFocusedRowCellValue("BMID").ToString())
                       + string.Format("delete from KQ_PB_LD where BMID='{0}';", gridView1.GetFocusedRowCellValue("BMID").ToString())
                       + string.Format("update KQ_YG set BMID='0' where BMID='{0}';", gridView1.GetFocusedRowCellValue("BMID").ToString());
                GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql);
                toolStripButtonRefresh_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
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

        private void gridView2_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "ZT")
            {               
              switch (e.Value.ToString())
              {
                 case "0": e.DisplayText = "在职"; break;
                 case "1": e.DisplayText = "离职"; break;
              }
            }
        }

        private void gridControl2_DoubleClick(object sender, EventArgs e)
        {
            ButtonAlter_Click(null, null);
        }

        private void gridView2_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {

        }
    }
}
