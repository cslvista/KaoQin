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
        DataTable Staff_WorkShift_SQL = new DataTable();
        TimeSpan Timespan;
        DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit Shift=new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
        public string DepartmentID;
        public string DepartmentName;
        public string PBID;//排班ID
        public bool alter = false;
        public Schedual()
        {
            InitializeComponent();
        }

        private void Schedual_Load(object sender, EventArgs e)
        {
            SearchDepartment();

            if (alter == true)
            {
                comboBox1.Enabled = false;
                comboBox1.Text = DepartmentName;
                comboBoxMonth.Enabled = false;
                comboBoxYear.Enabled = false;
                simpleButton1.Enabled = false;
                string sql = string.Format("select * from KQ_PB_XB where PBID='{0}'",PBID);
                try
                {
                    Staff_WorkShift_SQL = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误1:" + ex.Message, "提示");
                    return;
                }
                
                //点击生成计划按钮
                simpleButton1_Click(null, null);

                for (int i = 0; i < Staff.Rows.Count; i++)
                {
                    for (int j=2;j<=Timespan.Days+2; j++)
                    {
                        Staff_WorkShift.Rows[i][j] = Staff_WorkShift_SQL.Rows[i][j+1].ToString();
                    }
                }
            }else
            {
                //如果是新增
                string TimeNow = GlobalHelper.IDBHelper.GetServerDateTime();
                comboBoxYear.Items.Add(Convert.ToDateTime(TimeNow).Year.ToString() + "年");
                comboBoxYear.Items.Add(Convert.ToDateTime(TimeNow).AddYears(-1).Year.ToString() + "年");
                comboBoxYear.Items.Add(Convert.ToDateTime(TimeNow).AddYears(-2).Year.ToString() + "年");
                comboBoxYear.Text = Convert.ToDateTime(TimeNow).Year.ToString() + "年";
                comboBoxMonth.Text= Convert.ToDateTime(TimeNow).Month.ToString() + "月";
            }
            
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
                string year = comboBoxYear.Text.Substring(0, 4);
                string month = comboBoxMonth.Text.Substring(0, comboBoxMonth.Text.IndexOf("月"));
                string startDate = Convert.ToDateTime(year + "-" + month + "-" + "1").ToString("yyyy-MM-dd");
                StartDate =Convert.ToDateTime(startDate);
                StopDate = StartDate.AddMonths(1).AddDays(-1);
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
            string sql2 = string.Format("select ID,NAME,COLOR from KQ_BC where LBID='{0}'", "0");
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
            string sql3 = string.Format("select BMLB from KQ_BM where BMID='{0}'",comboBox1.SelectedValue);
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
                string sql4 = string.Format("select ID,NAME,COLOR from KQ_BC where LBID='{0}'",DepartmentType.Rows[0][0].ToString());
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
            band.Fixed= DevExpress.XtraGrid.Columns.FixedStyle.Left;
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


            if (alter == true)
            {
                //写入排班主表
                string sql1 = string.Format("update KQ_PB set XGRID='{0}',XGR='{1}',XGSJ='{2}' where PBID='{3}'", GlobalHelper.UserHelper.User["U_ID"].ToString(),
                    GlobalHelper.UserHelper.User["U_NAME"].ToString(), Convert.ToDateTime(GlobalHelper.IDBHelper.GetServerDateTime()),PBID);

                try
                {
                    GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误1:" + ex.Message);
                    return;
                }

                //构建用于操作SQL语句的表，将表Staff_WorkShift的时间扩充为31天，没有排班的设为null
                Staff_WorkShift_SQL.Rows.Clear();
                Staff_WorkShift_SQL.Columns.Clear();
                for (int i = 0; i < Staff_WorkShift.Rows.Count; i++)
                {
                    Staff_WorkShift_SQL.Rows.Add(new object[] { });
                    for (int j = 0; j < 33; j++)
                    {
                        //第0列为考勤ID，第1列是姓名
                        if (i == 0)
                        {
                            Staff_WorkShift_SQL.Columns.Add();
                        }

                        if (j < Staff_WorkShift.Columns.Count && Staff_WorkShift.Rows[i][j].ToString() != "")
                        {
                            Staff_WorkShift_SQL.Rows[i][j] = "'" + Staff_WorkShift.Rows[i][j].ToString() + "'";
                        }
                        else
                        {
                            Staff_WorkShift_SQL.Rows[i][j] = "null";
                        }

                    }
                }

                StringBuilder sql = new StringBuilder();

                for (int i = 0; i < Staff.Rows.Count; i++)
                {
                    sql.Append(string.Format("update KQ_PB_XB set D1T={0},D2T={1},D3T={2},D4T={3},D5T={4},D6T={5},D7T={6},D8T={7},"
                        + "D9T={8},D10T={9},D11T={10},D12T={11},D13T={12},D14T={13},D15T={14},D16T={15},D17T={16},D18T={17},D19T={18},D20T={19},"
                        + "D21T={20},D22T={21},D23T={22},D24T={23},D25T={24},D26T={25},D27T={26},D28T={27},D29T={28},D30T={29},D31T={30} where PBID='{31}' and KQID={32};",
                        Staff_WorkShift_SQL.Rows[i][2].ToString(),
                        Staff_WorkShift_SQL.Rows[i][3].ToString(), Staff_WorkShift_SQL.Rows[i][4].ToString(),
                        Staff_WorkShift_SQL.Rows[i][5].ToString(), Staff_WorkShift_SQL.Rows[i][6].ToString(),
                        Staff_WorkShift_SQL.Rows[i][7].ToString(), Staff_WorkShift_SQL.Rows[i][8].ToString(),
                        Staff_WorkShift_SQL.Rows[i][9].ToString(), Staff_WorkShift_SQL.Rows[i][10].ToString(),
                        Staff_WorkShift_SQL.Rows[i][11].ToString(), Staff_WorkShift_SQL.Rows[i][12].ToString(),
                        Staff_WorkShift_SQL.Rows[i][13].ToString(), Staff_WorkShift_SQL.Rows[i][14].ToString(),
                        Staff_WorkShift_SQL.Rows[i][15].ToString(), Staff_WorkShift_SQL.Rows[i][16].ToString(),
                        Staff_WorkShift_SQL.Rows[i][17].ToString(), Staff_WorkShift_SQL.Rows[i][18].ToString(),
                        Staff_WorkShift_SQL.Rows[i][19].ToString(), Staff_WorkShift_SQL.Rows[i][20].ToString(),
                        Staff_WorkShift_SQL.Rows[i][21].ToString(), Staff_WorkShift_SQL.Rows[i][22].ToString(),
                        Staff_WorkShift_SQL.Rows[i][23].ToString(), Staff_WorkShift_SQL.Rows[i][24].ToString(),
                        Staff_WorkShift_SQL.Rows[i][25].ToString(), Staff_WorkShift_SQL.Rows[i][26].ToString(),
                        Staff_WorkShift_SQL.Rows[i][27].ToString(), Staff_WorkShift_SQL.Rows[i][28].ToString(),
                        Staff_WorkShift_SQL.Rows[i][29].ToString(), Staff_WorkShift_SQL.Rows[i][30].ToString(),
                        Staff_WorkShift_SQL.Rows[i][31].ToString(), Staff_WorkShift_SQL.Rows[i][32].ToString(),
                        PBID,Staff_WorkShift_SQL.Rows[i][0].ToString()
                        ));
                }

                try
                {
                    GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql.ToString());
                    MessageBox.Show("保存成功！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误2:" + ex.Message);
                    return;
                }

            }
            else
            {
                //新增排班的语句

                //查找最大ID，然后自增
                string sql1 = "select max(PBID) from KQ_PB";
                DataTable Max_ID = new DataTable();
                string ID = "";
                try
                {
                    Max_ID = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.ZYDB, sql1);
                    if (Max_ID.Rows[0][0].ToString() == "")
                    {
                        ID = "1";
                    }
                    else
                    {
                        ID = (Convert.ToInt32(Max_ID.Rows[0][0].ToString()) + 1).ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误1:" + ex.Message);
                    return;
                }

                //写入排班主表
                string sql2 = string.Format("insert into KQ_PB (PBID,BMID,YEAR,MONTH,CJRID,CJR,CJSJ) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                    ID,comboBox1.SelectedValue, comboBoxYear.Text,
                    comboBoxMonth.Text, GlobalHelper.UserHelper.User["U_ID"].ToString(),
                    GlobalHelper.UserHelper.User["U_NAME"].ToString(), Convert.ToDateTime(GlobalHelper.IDBHelper.GetServerDateTime()));

                try
                {
                    GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql2);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误2:" + ex.Message);
                    return;
                }

                //构建用于操作SQL语句的表，将表Staff_WorkShift的时间扩充为31天，没有排班的设为null
                Staff_WorkShift_SQL.Rows.Clear();
                Staff_WorkShift_SQL.Columns.Clear();
                for (int i = 0; i < Staff_WorkShift.Rows.Count; i++)
                {
                    Staff_WorkShift_SQL.Rows.Add(new object[] {});
                    for (int j = 0; j < 33; j++)
                    {
                        if (i == 0)
                        {
                            Staff_WorkShift_SQL.Columns.Add();
                        }
                        
                        if (j< Staff_WorkShift.Columns.Count && Staff_WorkShift.Rows[i][j].ToString()!="")
                        {
                            Staff_WorkShift_SQL.Rows[i][j] = "'"+Staff_WorkShift.Rows[i][j].ToString()+ "'";
                        }else
                        {
                            Staff_WorkShift_SQL.Rows[i][j] = "null";
                        }
                        
                    }
                }
                
                //写入排班细表
                StringBuilder sql = new StringBuilder();

                for (int i = 0; i < Staff.Rows.Count; i++)
                {
                    sql.Append("insert into KQ_PB_XB (PBID,BMID,KQID,"
                        + "D1T,D2T,D3T,D4T,D5T,D6T,D7T,D8T,D9T,"
                        + "D10T,D11T,D12T,D13T,D14T,D15T,D16T,D17T,D18T,D19T,"
                        + "D20T,D21T,D22T,D23T,D24T,D25T,D26T,D27T,D28T,D29T,D30T,D31T)"
                        + string.Format(" values ('{0}','{1}',{2},{3},{4},{5},{6},{7},{8},{9},{10},"
                        + "{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},"
                        + "{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33});",
                        ID, comboBox1.SelectedValue, Staff_WorkShift_SQL.Rows[i][0].ToString(), 
                        Staff_WorkShift_SQL.Rows[i][2].ToString(),
                        Staff_WorkShift_SQL.Rows[i][3].ToString(), Staff_WorkShift_SQL.Rows[i][4].ToString(),
                        Staff_WorkShift_SQL.Rows[i][5].ToString(), Staff_WorkShift_SQL.Rows[i][6].ToString(),
                        Staff_WorkShift_SQL.Rows[i][7].ToString(), Staff_WorkShift_SQL.Rows[i][8].ToString(),
                        Staff_WorkShift_SQL.Rows[i][9].ToString(), Staff_WorkShift_SQL.Rows[i][10].ToString(),
                        Staff_WorkShift_SQL.Rows[i][11].ToString(), Staff_WorkShift_SQL.Rows[i][12].ToString(),
                        Staff_WorkShift_SQL.Rows[i][13].ToString(), Staff_WorkShift_SQL.Rows[i][14].ToString(),
                        Staff_WorkShift_SQL.Rows[i][15].ToString(), Staff_WorkShift_SQL.Rows[i][16].ToString(),
                        Staff_WorkShift_SQL.Rows[i][17].ToString(), Staff_WorkShift_SQL.Rows[i][18].ToString(),
                        Staff_WorkShift_SQL.Rows[i][19].ToString(), Staff_WorkShift_SQL.Rows[i][20].ToString(),
                        Staff_WorkShift_SQL.Rows[i][21].ToString(), Staff_WorkShift_SQL.Rows[i][22].ToString(),
                        Staff_WorkShift_SQL.Rows[i][23].ToString(), Staff_WorkShift_SQL.Rows[i][24].ToString(),
                        Staff_WorkShift_SQL.Rows[i][25].ToString(), Staff_WorkShift_SQL.Rows[i][26].ToString(),
                        Staff_WorkShift_SQL.Rows[i][27].ToString(), Staff_WorkShift_SQL.Rows[i][28].ToString(),
                        Staff_WorkShift_SQL.Rows[i][29].ToString(), Staff_WorkShift_SQL.Rows[i][30].ToString(),
                        Staff_WorkShift_SQL.Rows[i][31].ToString(), Staff_WorkShift_SQL.Rows[i][32].ToString()
                        ));

                }

                try
                {
                    GlobalHelper.IDBHelper.ExecuteNonQuery(GlobalHelper.GloValue.ZYDB, sql.ToString());
                    alter = true;
                    MessageBox.Show("保存成功！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误3:" + ex.Message);
                    return;
                }
            }
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

            for (int i=0;i< WorkShift.Rows.Count; i++)
            {
                if (e.CellValue.ToString() == WorkShift.Rows[i]["ID"].ToString())
                {
                    e.Appearance.ForeColor = ColorTranslator.FromHtml(WorkShift.Rows[i]["COLOR"].ToString());
                    break;
                }
            }
            
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
