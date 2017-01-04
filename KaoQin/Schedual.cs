using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.BandedGrid;

namespace KaoQin
{
    public partial class Schedual : Form
    {
        DataTable Department = new DataTable();
        DataTable Staff = new DataTable();
        DataTable WorkShift = new DataTable();
        DataTable WorkShift_Common = new DataTable();
        DataTable Staff_WorkShift = new DataTable();
        TimeSpan Timespan;
        DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit Shift=new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
        public string DepartmentID;
        public string DepartmentName;
        public bool alter = false;
        public Schedual()
        {
            InitializeComponent();
        }

        private void Schedual_Load(object sender, EventArgs e)
        {
            dateEdit1.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit1.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit2.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit2.Properties.Mask.EditMask = "yyyy-MM-dd";
            dateEdit1.Text = Convert.ToDateTime(DateTime.Today).ToString("yyyy-MM-01");
            dateEdit2.Text = Convert.ToDateTime(DateTime.Today).ToString("yyyy-MM-dd");
            if (alter == true)
            {
                comboBox1.Enabled = false;
                simpleButton1.Enabled = false;
            }
            SearchDepartment();
        }

        private void SearchDepartment()
        {
            Department.Columns.Add("BMID");
            Department.Columns.Add("BMMC");

            string sql = "select BMID,BMMC from KQ_BM where BMID>0";

            try
            {
                Department = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                comboBox1.DataSource = Department;
                comboBox1.DisplayMember = "BMMC";
                comboBox1.ValueMember = "BMID";
                comboBox1.Text = DepartmentName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }

        }

        private string Week(DateTime Day)
        {
            string[] weekdays = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            return weekdays[Convert.ToInt32(Day.DayOfWeek)];
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //校验日期
            DateTime StartDate;
            DateTime StopDate;
            
            try
            {
                StartDate =Convert.ToDateTime(dateEdit1.Text);
                StopDate = Convert.ToDateTime(dateEdit2.Text);
                Timespan = StopDate-StartDate;

                if (Timespan.Days < 0)
                {
                    MessageBox.Show("结束时间不得小于开始时间！");
                    return;
                }

                if (Timespan.Days > 62)
                {
                    MessageBox.Show("不能生成超过两个月的计划！");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:"+ex.Message);
                return;                   
            }

            //读取员工信息
            string sql1 = string.Format("select KQID,YGXM from KQ_YG where BMID='{0}' and ZT='0'",comboBox1.SelectedValue);

            try
            {
                Staff=GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误2:" + ex.Message);
                return;
            }

            //读取部门共用班次信息
            string sql2 = string.Format("select ID,NAME from KQ_BC where LBID='{0}'", "0");
            try
            {
                WorkShift_Common = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误3:" + ex.Message);
                return;
            }
            //读取指定部门班次信息
            string sql3 = string.Format("select BMLX from KQ_BM where BMID='{0}'",comboBox1.SelectedValue);
            DataTable DepartmentType = new DataTable();
            try
            {
                DepartmentType = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql3);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误4:" + ex.Message);
                return;
            }

            if (DepartmentType.Rows[0][0].ToString()!="")
            {
                string sql4 = string.Format("select ID,NAME from KQ_BC where LBID='{0}'",DepartmentType.Rows[0][0].ToString());
                try
                {
                    WorkShift = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql4);
                    WorkShift.Merge(WorkShift_Common);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误5:" + ex.Message);
                    return;
                }
            }else
            {
                WorkShift = WorkShift_Common.Clone();
            }
            
            bandedGridView1.Columns.Clear();
            bandedGridView1.Bands.Clear();
            Staff_WorkShift.Columns.Clear();
            Staff_WorkShift.Clear();

            GridBand band = new GridBand();
            band.Caption = " ";
            band.Width = 30;
            bandedGridView1.Bands.Add(band);

            //生成列
            BandedGridColumn Staff_ID = new BandedGridColumn();
            Staff_ID.Caption = "考勤号";
            Staff_ID.Name = "Staff_ID";
            Staff_ID.FieldName = "KQID";
            Staff_ID.Visible = false;
            Staff_ID.OptionsColumn.AllowEdit = false;
            band.Columns.Add(Staff_ID);
            bandedGridView1.Columns.Add(Staff_ID);
            Staff_WorkShift.Columns.Add("KQID");

            BandedGridColumn Staff_Name = new BandedGridColumn();
            Staff_Name.Caption = "姓名";
            Staff_Name.Name = "Staff_Name";
            Staff_Name.Visible = true;
            Staff_Name.FieldName = "YGXM";
            Staff_Name.OptionsColumn.AllowEdit = false;
            band.Columns.Add(Staff_Name);
            bandedGridView1.Columns.Add(Staff_Name);
            Staff_WorkShift.Columns.Add("YGXM");

            Shift.Columns.Clear();
            Shift.DataSource = WorkShift;
            Shift.DisplayMember = "NAME";
            Shift.ValueMember = "ID";
            Shift.Name = "Shift";
            Shift.NullText = "";
            Shift.AutoHeight = false;
            Shift.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ID", "ID", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "类型")});

            for (int i = 0; i <= Timespan.Days; i++)
            {
                GridBand Day_band = new GridBand();
                Day_band.Caption = Week(StartDate);
                bandedGridView1.Bands.Add(Day_band);
                BandedGridColumn Day_Column = new BandedGridColumn();
                Day_Column.Caption = StartDate.ToString("yyyy-MM-dd");
                Day_Column.FieldName = StartDate.ToString("yyyy-MM-dd");
                Day_Column.Name= Day_Column.FieldName;
                Day_Column.Visible = true;
                Day_Column.ColumnEdit = Shift;
                //Day_Column.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
                Day_Column.OptionsColumn.AllowEdit = true;
                Day_band.Columns.Add(Day_Column);
                bandedGridView1.Columns.Add(Day_Column);
                Staff_WorkShift.Columns.Add(Day_Column.FieldName);
                StartDate =StartDate.AddDays(1);
            }

            foreach (DataRow Row in Staff.Rows)
            {
                Staff_WorkShift.Rows.Add(new object[] {Row["KQID"],Row["YGXM"]});
            }

            gridControl1.DataSource = Staff_WorkShift;
            bandedGridView1.BestFitColumns();

        }


        private void simpleButton4_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "电子表格(*.xls)|*.xls";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string path = sf.FileName.ToString();
                    gridControl1.ExportToXlsx(path);
                    MessageBox.Show("导出成功！");
                }
                catch { }
                if (MessageBox.Show("是否打开？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                    return;
                try
                {
                    System.Diagnostics.Process.Start(sf.FileName);
                }
                catch { }
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (bandedGridView1.RowCount == 0)
            {
                MessageBox.Show("请先生成计划！");
                return;
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel文件(*.xls)|*.xls|Excel文件(*.xlsx)|*.xlsx";//过滤文件类型
            ofd.RestoreDirectory = true; //记忆上次浏览路径
            string FileName = "";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileName = ofd.FileName;
            }else
            {
                return;
            }

            string FileExtension = "";
            DataTable dtExcel = new DataTable(); //数据表   
            DataSet ds = new DataSet();
            OleDbConnection ExcelConn = null;

            try
            {
                FileExtension = System.IO.Path.GetExtension(FileName);
                switch (FileExtension)
                {
                    case ".xls": ExcelConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";" + "Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1;\""); break;
                    case ".xlsx": ExcelConn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";" + "Extended Properties=\"Excel 12.0;HDR=NO;IMEX=1;\""); break;
                    default: ExcelConn = null; break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message);
                return;
            }
                       
            try
            {
                ExcelConn.Open();
                //获取Excel中所有Sheet表的信息
                DataTable schemaTable = ExcelConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //获取Excel的第一个Sheet表名
                string tableName = schemaTable.Rows[0][2].ToString().Trim();
                string strSql = "select * from [" + tableName + "]";
                //获取Excel指定Sheet表中的信息
                OleDbDataAdapter myData = new OleDbDataAdapter(strSql, ExcelConn);
                myData.Fill(ds, tableName);//填充数据
                dtExcel = ds.Tables[tableName];
                ExcelConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误2:" + ex.Message);
                return;
            }

            //将Excel文件内容插入DataTable
            int row;
            for (int i = 0; i < Staff.Rows.Count; i++)
            {
                //找出Excel表格中同名者所在行
                row = -1;
                for (int j = 0; j < dtExcel.Rows.Count; j++)
                {
                    string Name = dtExcel.Rows[j][0].ToString().Replace(" ", ""); //去掉空格
                    if (Name== Staff.Rows[i][1].ToString())
                    {
                        row = j;
                        break;
                    }
                }

                //将Excel表格中的排班信息插入DataTable
                if (row >= 0)
                {
                    //指定的时间段
                    for (int k = 0; k <= Timespan.Days; k++)
                    {
                        //检验Excel中的排班是否和系统中的排班相同
                        for (int n = 0; n < WorkShift.Rows.Count; n++)
                        {
                            if (dtExcel.Rows[row][k + 3].ToString()== WorkShift.Rows[n][1].ToString())
                            {
                                Staff_WorkShift.Rows[i][k+2] = WorkShift.Rows[n][0].ToString();
                                break;
                            }                             
                        }                     
                    }
                }
            }
            gridControl1.DataSource = Staff_WorkShift;
        }

        private void bandedGridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                string name = Week(DateTime.Parse(e.Column.Caption));
                if (name == "周六" || name == "周日")
                {
                    e.Appearance.BackColor = Color.AliceBlue;
                }
            }
            catch { }

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
