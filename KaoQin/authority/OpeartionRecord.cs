﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin.authority
{
    public partial class OpeartionRecord : Form
    {
        DataTable Record = new DataTable();
        public OpeartionRecord()
        {
            InitializeComponent();
        }

        private void OpeartionRecord_Load(object sender, EventArgs e)
        {
            searchControl1.Properties.NullValuePrompt = "搜索操作记录";
            dateEdit1.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit1.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit2.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit2.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit1.Text = Convert.ToDateTime(GlobalHelper.IDBHelper.GetServerDateTime()).AddDays(-30).ToString("yyyy-MM-dd");
            dateEdit2.Text = Convert.ToDateTime(GlobalHelper.IDBHelper.GetServerDateTime()).ToString("yyyy-MM-dd"); ;           
            SearchDB();
        }

        private void SearchDB()
        {
            try
            {
                string sql = string.Format("select ID,Record,Time from KQ_LOG where Time between '{0} 00:00:00' and '{1} 23:59:59' order by ID desc", dateEdit1.Text, dateEdit2.Text);
                Record = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
                gridControl1.DataSource = Record;
                gridView1.BestFitColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
            }
        }

        private void SearchInfo()
        {
            if (Record.Rows.Count>0)
            {
                try
                {
                    Record.DefaultView.RowFilter = string.Format("Record like '%{0}%'",searchControl1.Text);
                }
                catch { }                
            }

        }

        private void searchControl1_TextChanged(object sender, EventArgs e)
        {
            SearchInfo();
        }


        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (Record.Rows.Count == 0)
                {
                    return;
                }

                string sql = string.Format("select Details from KQ_LOG where ID='{0}'", gridView1.GetFocusedRowCellDisplayText("ID").ToString());
                DataTable Details = new DataTable();
                Details=GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
                if (Details.Rows.Count == 0)
                {
                    MessageBox.Show("该项操作没有记录！");
                    return;
                }
                OpeartionDetails form = new OpeartionDetails();
                form.details = Details.Rows[0][0].ToString();
                form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void 查看详细信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridControl1_DoubleClick(sender, e);
        }

        private void dateEdit1_TextChanged(object sender, EventArgs e)
        {
            SearchDB();
        }

        private void dateEdit2_TextChanged(object sender, EventArgs e)
        {
            SearchDB();
        }
    }
}
