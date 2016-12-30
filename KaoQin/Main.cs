﻿using System;
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
            searchControl1.Properties.NullValuePrompt = " ";
            SearchDepartment();//查找部门
        }

        private void SearchDepartment()
        {
            string sql = "select BMID,BMMC,BMLX from KQ_BM where BMID>0";

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
            Thread t1 = new Thread(SearchAttendance);
            t1.IsBackground = true;
            t1.Start();
        }

        private void SearchAttendance()
        {
            string sql = string.Format("select ID,BMID,KSSJ,JSSJ,CJRID,CJSJ,XGRID,XGR,XGSJ from KQ_PB where BMID='{0}'", gridView1.GetFocusedRowCellValue("BMID").ToString());

            this.BeginInvoke(new UpdateUI(delegate ()
            {
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
                form.alter = true;
                form.Show(this);
            }
            catch { }
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {

        }

        private void 考勤管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Attendance form = new Attendance();
            form.Show();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Loading form = new Loading();
            form.Show();
        }
    }
}
