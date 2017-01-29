using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace KaoQin.arrangement
{
    public partial class arrange : Form
    {
        DataTable Type = new DataTable();
        DataTable WorkShift = new DataTable();
        public bool Authority_Shift_Edit = false;
        delegate void UpdateUI();

        public arrange()
        {
            InitializeComponent();
        }

        private void arrange_Load(object sender, EventArgs e)
        {
            searchControl1.Properties.NullValuePrompt = "请输入时段名称";
            searchControl2.Properties.NullValuePrompt = "请输入部门类别";
            gridView2.OptionsBehavior.AutoExpandAllGroups = true;
            toolStripButtonRefresh_Click(null, null);
        }


        public void ButtonRefresh_Click(object sender, EventArgs e)
        {
            SearchWorkShift();
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(string.Format("是否删除'{0}'？", gridView2.GetFocusedRowCellDisplayText("NAME").ToString()), "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            catch { }

            string sql = string.Format("select top 1 PBID from KQ_PB_XB where D1T='{0}' or D2T='{0}' or D3T='{0}' or D4T='{0}' or D5T='{0}' or D6T='{0}' or D7T='{0}' or D8T='{0}' or D9T='{0}' or D10T='{0}'", gridView2.GetFocusedRowCellValue("ID").ToString());
            DataTable isExists = new DataTable();
            try
            {
                isExists=GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                if (isExists.Rows.Count > 0)
                {
                    MessageBox.Show("该班次不可删除！");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message);
                return;
            }

            string sql1 = string.Format("delete from KQ_BC where ID='{0}'", gridView2.GetFocusedRowCellValue("ID").ToString());

            try
            {
                GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误2:" + ex.Message);
                return;
            }

            SearchWorkShift();
        }

        private void ButtonAlter_Click(object sender, EventArgs e)
        {
            if (Authority_Shift_Edit == false)
            {
                MessageBox.Show("您没有操作的权限！");
                return;
            }

            try
            {
                add_alter_Item form = new add_alter_Item();
                form.alter = true;
                form.LBID= gridView1.GetFocusedRowCellValue("ID").ToString();
                form.ID = gridView2.GetFocusedRowCellValue("ID").ToString();
                form.comboBox2.Text= gridView2.GetFocusedRowCellDisplayText("ZT").ToString();
                form.comboBox1.Text = gridView2.GetFocusedRowCellDisplayText("KT").ToString();
                form.textBox1.Text = gridView1.GetFocusedRowCellDisplayText("BMLB").ToString();
                form.textBox2.Text = gridView2.GetFocusedRowCellDisplayText("GZR").ToString();
                form.textBox3.Text = gridView2.GetFocusedRowCellDisplayText("NAME").ToString(); 
                form.textBox4.Text = gridView2.GetFocusedRowCellDisplayText("SM").ToString();
                form.colorPickEdit1.Text= gridView2.GetFocusedRowCellDisplayText("COLOR").ToString();
                if (gridView2.GetFocusedRowCellDisplayText("SBSJ").ToString() == "")
                {
                    form.checkBox1.Checked = true;
                }
                else
                {
                    form.timeEdit1.EditValue = gridView2.GetFocusedRowCellDisplayText("SBSJ").ToString();
                }

                if (gridView2.GetFocusedRowCellDisplayText("XBSJ").ToString() == "")
                {
                    form.checkBox2.Checked = true;
                }else
                {
                    form.timeEdit2.EditValue = gridView2.GetFocusedRowCellDisplayText("XBSJ").ToString();
                }
                form.Show(this);
            }
            catch { }
            
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (Authority_Shift_Edit == false)
            {
                MessageBox.Show("您没有操作的权限！");
                return;
            }

            try
            {
                add_alter_Item form = new add_alter_Item();
                form.LBID= gridView1.GetFocusedRowCellDisplayText("ID").ToString();
                form.textBox1.Text = gridView1.GetFocusedRowCellDisplayText("BMLB").ToString();
                form.Show(this);
            }
            catch { }
            
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            if (Authority_Shift_Edit == false)
            {
                MessageBox.Show("您没有操作的权限！");
                return;
            }

            add_alter_Type form = new add_alter_Type();            
            form.Show(this);
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (Authority_Shift_Edit == false)
            {
                MessageBox.Show("您没有操作的权限！");
                return;
            }

            try
            {
                if (gridView1.GetFocusedRowCellDisplayText("ID").ToString()=="0")
                {
                    MessageBox.Show("'部门共用'不可修改！");
                    return;
                }

                add_alter_Type form = new add_alter_Type();
                form.alter = true;
                form.textBox1.Text = gridView1.GetFocusedRowCellDisplayText("BMLB").ToString();
                form.ID = gridView1.GetFocusedRowCellDisplayText("ID").ToString();
                form.Show(this);
            }
            catch { }
            
        }

        public void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            string sql = "select ID,BMLB from KQ_BMLB";

            try
            {
                Type = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                gridControl1.DataSource = Type;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误:" + ex.Message);
                return;
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (Authority_Shift_Edit == false)
            {
                MessageBox.Show("您没有操作的权限！");
                return;
            }

            if (gridView1.GetFocusedRowCellDisplayText("ID").ToString() == "0")
            {
                MessageBox.Show("'部门共用'不可删除！");
                return;
            }

            if (MessageBox.Show(string.Format("是否删除 '{0}'？", gridView1.GetFocusedRowCellDisplayText("BMLB").ToString()), "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }


            //查询是否有子项
            string sql = string.Format("select ID from KQ_BC where LBID='{0}'", gridView1.GetFocusedRowCellValue("ID").ToString());
            DataTable isExists = new DataTable();
            try
            {
                isExists = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                if (isExists.Rows.Count > 0)
                {
                    MessageBox.Show("该类别不允许被删除！");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message);
                return;
            }

            //查找部门表中该类别是否已经使用
            string sql1 = string.Format("select BMLB from KQ_BM where BMLB='{0}'",gridView1.GetFocusedRowCellDisplayText("ID").ToString());
            DataTable isExists_LB = new DataTable();
            try
            {
                isExists_LB=GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql1);
                if (isExists_LB.Rows.Count > 0)
                {
                    MessageBox.Show("该类别仍被使用，不允许删除!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误2:" + ex.Message);
                return;
            }
            
            string sql2 = string.Format("delete from KQ_BMLB where ID='{0}'", gridView1.GetFocusedRowCellDisplayText("ID").ToString());
               
            try
            {
                GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误3:" + ex.Message);
                return;
            }
            //刷新界面
            toolStripButtonRefresh_Click(null, null);

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            Thread t1 = new Thread(SearchWorkShift);
            t1.IsBackground = true;
            t1.Start();
        }

        private void SearchWorkShift()
        {
            this.BeginInvoke(new UpdateUI(delegate ()
            {
                string sql = string.Format("select ID,LBID,ZT,KT,NAME,SBSJ,XBSJ,GZR,SM,CJR,XGR,COLOR from KQ_BC where LBID='{0}'",gridView1.GetFocusedRowCellValue("ID").ToString());
                try
                {
                    WorkShift = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql.ToString());
                    gridControl2.DataSource = WorkShift;
                    gridView2.BestFitColumns();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误:" + ex.Message, "提示");
                    return;
                }

            }));
        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "BMLB")
            {
                string ZYZT = gridView1.GetRowCellDisplayText(e.RowHandle, gridView1.Columns["BMLB"]);

                if (ZYZT == "部门共用")
                {
                    e.Appearance.ForeColor = Color.Red;
                }
            }
        }


        private void gridControl2_DoubleClick(object sender, EventArgs e)
        {
            ButtonAlter_Click(null, null);
        }

        private void searchControl1_TextChanged(object sender, EventArgs e)
        {
            if (WorkShift.Rows.Count>0)
            {
                WorkShift.DefaultView.RowFilter = string.Format("NAME like '%{0}%'", searchControl1.Text);
            }
            
        }

        private void gridView2_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "KT")
            {
                switch (e.Value.ToString())
                {
                    case "0": e.DisplayText = "否"; break;
                    case "1": e.DisplayText = "是"; break;
                }
            }

            if (e.Column.FieldName == "ZT")
            {
                switch (e.Value.ToString())
                {
                    case "0": e.DisplayText = "在用"; break;
                    case "1": e.DisplayText = "停用"; break;
                }
            }

        }

        private void gridView2_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "KT")
            {
                string KT = gridView2.GetRowCellDisplayText(e.RowHandle, gridView2.Columns["KT"]);

                if (KT == "是")
                {
                    e.Appearance.ForeColor = Color.Red;
                }
                else if (KT == "否")
                {
                    e.Appearance.ForeColor = Color.Blue;
                }
            }

        }

        private void searchControl2_TextChanged(object sender, EventArgs e)
        {
            if (Type.Rows.Count > 0)
            {
                Type.DefaultView.RowFilter = string.Format("BMLB like '%{0}%'", searchControl2.Text);
            }
        }
    }
}
