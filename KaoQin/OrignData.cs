using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraPrinting;

namespace KaoQin
{
    public partial class OrignData : Form
    {
        public DataTable Record_DKJ = new DataTable();
        public DataTable Machine = new DataTable();
        DataTable Record_DKJ_new = new DataTable();
        DataTable Staff = new DataTable();
        
        zkemkeeper.CZKEMClass DKJ = new zkemkeeper.CZKEMClass();
        public OrignData()
        {
            InitializeComponent();
        }

        private void OrignData_Load(object sender, EventArgs e)
        {
            searchControl1.Properties.NullValuePrompt = "请输入考勤号或者姓名";
            dateEdit1.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit1.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit2.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit2.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit1.Text = Convert.ToDateTime(DateTime.Today).ToString("yyyy-MM-01");
            dateEdit2.Text = Convert.ToDateTime(DateTime.Today).ToString("yyyy-MM-dd");
            Record_DKJ_new.Columns.Add("ID", typeof(string));
            Record_DKJ_new.Columns.Add("Name", typeof(string));
            Record_DKJ_new.Columns.Add("Time", typeof(string));
            Record_DKJ_new.Columns.Add("Source", typeof(string));

            //读取员工信息
            string sdwEnrollNumber = "";
            string sName = "";
            string sPassword = "";
            int iPrivilege = 0;
            bool bEnabled = false;

            DKJ.ReadAllUserID(0);
            while (DKJ.SSR_GetAllUserInfo(0, out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))
            {
                int a;
                a = sName.IndexOf("\0");
                string name = sName.Substring(0, a);//过滤sName中多余字符
                Staff.Rows.Add(new object[] { sdwEnrollNumber, name });
            }

            //联结两个表
            var query = from rec in Record_DKJ.AsEnumerable()
                        join staff in Staff.AsEnumerable()
                        on rec.Field<string>("ID") equals staff.Field<string>("ID")
                        select new
                        {
                            ID = staff.Field<string>("ID"),
                            NAME = staff.Field<string>("NAME"),
                            Time = rec.Field<string>("Time"),
                            LY = rec.Field<string>("LY"),
                        };


            foreach (var obj in query)
            {
                Record_DKJ_new.Rows.Add(obj.ID, obj.NAME, obj.Time, obj.LY);
            }

            gridControl1.DataSource = Record_DKJ_new;
        }

        private void SearchInfo()
        {
            string StopTime = Convert.ToDateTime(DateTime.Today).AddDays(1).ToString("yyyy-MM-dd");
            Record_DKJ.DefaultView.RowFilter = string.Format("Time>='{0}' and Time<='{1}' and  ( ID like '%{2}%' or NAME like '%{2}%' )", dateEdit1.Text, StopTime, searchControl1.Text);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel文件(*.xlsx)|*.xlsx|所有文件(*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName.ToString();
                try
                {
                    XlsxExportOptions options = new XlsxExportOptions();
                    options.ExportMode = XlsxExportMode.SingleFile;
                    options.TextExportMode = TextExportMode.Text;
                    options.RawDataMode = false;
                    gridControl1.ExportToXlsx(path, options);
                    if (MessageBox.Show("是否打开？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
                        return;
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
                catch
                {
                    MessageBox.Show("导出失败！");
                }
            }
        }

        private void dateEdit1_TextChanged(object sender, EventArgs e)
        {
            SearchInfo();
        }

        private void searchControl1_TextChanged(object sender, EventArgs e)
        {
            SearchInfo();
        }
    }
}
