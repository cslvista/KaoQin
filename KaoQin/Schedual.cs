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
        DataTable Staff_WorkShift_SQL_Copy = new DataTable();
        DateTime StartDate;
        DateTime StopDate;
        TimeSpan Timespan;
        DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit Shift=new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
        public string DepartmentID;
        public string DepartmentName;
        public string PBID;//排班ID
        public string tableName = "";
        public int PBColumn;//Excel中排班的起始列
        public int NameColumn;//Excel中的姓名列
        public bool ColumnLocation = false;
        public bool alter = false;
        public bool TableNameHasChoosed = false;
        public bool ChangeAccept = false;
        public bool Authority_Arrangement_Edit = false;
        public Schedual()
        {
            InitializeComponent();
        }

        private void Schedual_Load(object sender, EventArgs e)
        {
            UILocation();
            SearchDepartment();
            searchControl1.Properties.NullValuePrompt = "请输入姓名";
            bandedGridView1.IndicatorWidth = 35;
            if (alter == true)
            {
                comboBox1.Enabled = false;
                comboBox1.Text = DepartmentName;
                comboBoxMonth.Enabled = false;
                comboBoxYear.Enabled = false;
                ButtonCreate.Enabled = false;
                
                string sql = string.Format("select * from KQ_PB_XB where PBID='{0}'",PBID);
                try
                {
                    Staff_WorkShift_SQL = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误1:" + ex.Message, "提示");
                    return;
                }
                
                //点击生成计划按钮
                simpleButton1_Click(null, null);

                //将排班内容填充
                for (int i = 0; i < Staff.Rows.Count; i++)
                {
                    for (int j=2;j<=Timespan.Days+2; j++)
                    {
                        Staff_WorkShift.Rows[i][j] = Staff_WorkShift_SQL.Rows[i][j+1].ToString();
                    }
                }
                Staff_WorkShift_SQL_Copy = Staff_WorkShift_SQL.Copy();

                //从排班中保留停用中还存在的班次
                for (int k = 0; k < WorkShift.Rows.Count; k++)
                {
                    //还在启用的班次保留
                    if (WorkShift.Rows[k]["ZT"].ToString()=="0")
                    {
                        continue;
                    }
                    //检查停用的班次是否在使用
                    for (int i = 0; i < Staff_WorkShift_SQL.Rows.Count; i++)
                    {
                        bool hasFound = false;
                        for (int j = 0; j <= Timespan.Days; j++)
                        {
                            if (WorkShift.Rows[k]["ID"].ToString()== Staff_WorkShift_SQL.Rows[i][j + 3].ToString())
                            {
                                hasFound = true;
                                break;
                            }
                            //如果还没找到，就删除
                            if (i== Staff_WorkShift_SQL.Rows.Count-1 && j== Timespan.Days)
                            {
                                WorkShift.Rows.RemoveAt(k);
                            }
                        }
                        if (hasFound == true)
                        {
                            break;
                        }
                    }
                }
                if (Authority_Arrangement_Edit == false)
                {
                    ButtonSave.Enabled = false;
                    ButtonImport.Enabled = false;
                }

            }
            else
            {
                //如果是新增
                string TimeNow = GlobalHelper.IDBHelper.GetServerDateTime();
                comboBoxYear.Items.Add(Convert.ToDateTime(TimeNow).Year.ToString() + "年");
                comboBoxYear.Items.Add(Convert.ToDateTime(TimeNow).AddYears(-1).Year.ToString() + "年");
                comboBoxYear.Items.Add(Convert.ToDateTime(TimeNow).AddYears(-2).Year.ToString() + "年");
                comboBoxYear.Text = Convert.ToDateTime(TimeNow).Year.ToString() + "年";
                comboBoxMonth.Text= Convert.ToDateTime(TimeNow).Month.ToString() + "月";
                ButtonSave.Enabled = false;
                ButtonImport.Enabled = false;
                ButtonExport.Enabled = false;
            }
            
        }

        private void UILocation()
        {
            int height = (panel1.Height - ButtonCreate.Height) / 2;
            int x = 3;
            int y = 2;
            ButtonCreate.Location = new Point(ButtonCreate.Location.X, height);
            ButtonSave.Location = new Point(ButtonSave.Location.X, height);
            ButtonImport.Location = new Point(ButtonImport.Location.X, height);
            ButtonExport.Location = new Point(ButtonExport.Location.X, height);
            searchControl1.Location = new Point(searchControl1.Location.X, (panel1.Height - searchControl1.Height) / 2);
            label3.Location = new Point(searchControl1.Location.X - label3.Width - x, searchControl1.Location.Y + y);
            comboBox1.Location=new Point(comboBox1.Location.X, (panel1.Height - comboBox1.Height) / 2);
            label1.Location = new Point(comboBox1.Location.X - label1.Width - x, comboBox1.Location.Y + y);
            comboBoxYear.Location = new Point(comboBoxYear.Location.X, (panel1.Height - comboBoxYear.Height) / 2);
            label4.Location = new Point(comboBoxYear.Location.X - label4.Width - x, comboBoxYear.Location.Y + y);
            comboBoxMonth.Location = new Point(comboBoxMonth.Location.X, (panel1.Height - comboBoxMonth.Height) / 2);
        }
        private void SearchDepartment()
        {
            Department.Columns.Add("BMID");
            Department.Columns.Add("BMMC");

            string sql = "select BMID,BMMC from KQ_BM where BMID>0";

            try
            {
                Department = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
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
            try
            {
                string year = comboBoxYear.Text.Substring(0, 4);
                string month = comboBoxMonth.Text.Substring(0, comboBoxMonth.Text.IndexOf("月"));
                string startDate = Convert.ToDateTime(year + "-" + month + "-" + "1").ToString("yyyy-MM-dd");
                StartDate =Convert.ToDateTime(startDate);
                StopDate = StartDate.AddMonths(1).AddDays(-1);
                Timespan = StopDate-StartDate;
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:"+ex.Message);
                return;                   
            }

            //读取员工信息
            string sql1 = "";
            if (alter == true)
            {
                sql1 = string.Format("select a.KQID,a.YGXM from KQ_YG a inner join KQ_PB_XB b on a.KQID=b.KQID where b.PBID='{0}'",PBID);
            }else
            {
                sql1 = string.Format("select KQID,YGXM from KQ_YG where BMID='{0}' and ZT='0'", comboBox1.SelectedValue);
            }
            

            try
            {
                Staff=GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误2:" + ex.Message);
                return;
            }

            //读取部门共用班次信息
            string sql2 = "";
            if (alter == true)
            {
                sql2 = string.Format("select ID,NAME,COLOR,ZT from KQ_BC where LBID='{0}'", "0");
            }else
            {
                sql2 = string.Format("select ID,NAME,COLOR from KQ_BC where LBID='{0}' and ZT='0'", "0");
            }
            
            try
            {
                WorkShift_Common = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误3:" + ex.Message);
                return;
            }

            //读取指定部门班次信息
            //先获取此部门的类别
            string sql3 = string.Format("select BMLB from KQ_BM where BMID='{0}'",comboBox1.SelectedValue);
            DataTable DepartmentType = new DataTable();
            try
            {
                DepartmentType = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql3);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误4:" + ex.Message);
                return;
            }

            if (DepartmentType.Rows[0][0].ToString()!="")
            {
                string sql4 = "";
                if (alter == true)
                {
                    sql4 = string.Format("select ID,NAME,COLOR,ZT from KQ_BC where LBID='{0}'", DepartmentType.Rows[0][0].ToString());
                }else
                {
                    sql4 = string.Format("select ID,NAME,COLOR from KQ_BC where LBID='{0}' and ZT='0'", DepartmentType.Rows[0][0].ToString());
                }
                
                try
                {
                    WorkShift = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql4);
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
            Staff_Name.Width = 30;
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
            Shift.DropDownRows = WorkShift.Rows.Count;
            Shift.AutoHeight = false;
            Shift.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ID", "ID", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("NAME", "班次")}
            );


            for (int i = 0; i <= Timespan.Days; i++)
            {
                GridBand Day_band = new GridBand();
                Day_band.Caption = Week(StartDate);
                bandedGridView1.Bands.Add(Day_band);
                BandedGridColumn Day_Column = new BandedGridColumn();
                Day_Column.Caption = StartDate.ToString("MM-dd");
                Day_Column.FieldName = StartDate.ToString("yyyy-MM-dd");
                Day_Column.Name= Day_Column.FieldName;
                Day_Column.Visible = true;
                Day_Column.Width = 61;
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
            //禁止再更改
            comboBox1.Enabled = false;
            comboBoxMonth.Enabled = false;
            comboBoxYear.Enabled = false;
            ButtonCreate.Enabled = false;
            ButtonSave.Enabled = true;
            ButtonImport.Enabled = true;
            ButtonExport.Enabled = true;
        }


        private void simpleButton4_Click(object sender, EventArgs e)
        {

            if (alter == true)
            {
                //对比更改了什么记录
                StringBuilder details = new StringBuilder();

                //第一层循环是员工人数
                for (int i = 0; i < Staff_WorkShift.Rows.Count; i++)
                {
                    //第二层循环是排班天数
                    for (int j = 0; j <= Timespan.Days; j++)
                    {
                        if (Staff_WorkShift.Rows[i][j + 2].ToString() != Staff_WorkShift_SQL_Copy.Rows[i][j + 3].ToString())
                        {
                            DateTime startDate = StartDate.AddMonths(-1);
                            string name = Staff_WorkShift.Rows[i]["YGXM"].ToString();
                            string date = startDate.AddDays(j).Month.ToString() + "月" + startDate.AddDays(j).Day.ToString() + "日";
                            string PB_num_origin = Staff_WorkShift_SQL_Copy.Rows[i][j + 3].ToString();
                            string PB_num_change = Staff_WorkShift.Rows[i][j + 2].ToString();
                            string PB_origin = "空";
                            string PB_change = "";
                            bool PB_origin_found = false;
                            bool PB_change_found = false;
                            //第三层循环是班次的名称
                            for (int k = 0; k < WorkShift.Rows.Count; k++)
                            {
                                if (PB_origin_found == false && PB_num_origin == WorkShift.Rows[k]["ID"].ToString())
                                {
                                    PB_origin = WorkShift.Rows[k]["NAME"].ToString();
                                    PB_origin_found = true;
                                }

                                if (PB_change_found == false && PB_num_change == WorkShift.Rows[k]["ID"].ToString())
                                {
                                    PB_change = WorkShift.Rows[k]["NAME"].ToString();
                                    PB_change_found = true;
                                }

                                if (PB_origin_found == true && PB_change_found == true)
                                {
                                    break;
                                }
                            }
                            details.Append(string.Format("{0} {1} 从 {2} 变更为 {3}  \r\n", name, date, PB_origin, PB_change));
                        }
                    }
                }
                if (details.ToString() == "")
                {
                    MessageBox.Show("本次操作没有做任何修改！");
                    return;
                }

                SchedualRecord form = new SchedualRecord();
                form.details = details.ToString();
                form.ShowDialog(this);

                if (ChangeAccept == true)
                {
                    ChangeAccept = false;
                }else
                {
                    return;
                }

                //写入排班主表
                string sql1 = string.Format("update KQ_PB set XGRID='{0}',XGR='{1}',XGSJ='{2}' where PBID='{3}'", GlobalHelper.UserHelper.User["U_ID"].ToString(),
                    GlobalHelper.UserHelper.User["U_NAME"].ToString(), Convert.ToDateTime(GlobalHelper.IDBHelper.GetServerDateTime()),PBID);

                try
                {
                    GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql1);
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
                StringBuilder sql_lastday = new StringBuilder();
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
                    sql_lastday.Append(string.Format("update KQ_PB_LD set LastDay={0} where PBID='{1}' and KQID={2};", Staff_WorkShift_SQL.Rows[i][2+Timespan.Days].ToString(), PBID, Staff_WorkShift_SQL.Rows[i][0].ToString()));
                }

                try
                {
                    GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql.ToString());
                    GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql_lastday.ToString());
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误2:" + ex.Message);
                    return;
                }
                //写日志
                if (WriteLog(details.ToString()))
                {
                    MessageBox.Show("保存成功！");
                }
               
            }
            else
            {
                //新增排班的语句

                //查找主表中是否已经添加过
                string search_PB =string.Format("select PBID from  KQ_PB where Year='{0}' and Month='{1}' and BMID='{2}'",comboBoxYear.Text,comboBoxMonth.Text,comboBox1.SelectedValue);
                DataTable isExists = new DataTable();
                try
                {
                    isExists=GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, search_PB);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误0:" + ex.Message);
                    return;
                }

                if (isExists.Rows.Count > 0)
                {
                    MessageBox.Show("已经添加过该月排班记录，无需重复添加！" );
                    return;
                }

                //查找最大ID，然后自增
                string sql1 = "select max(PBID) from KQ_PB";
                DataTable Max_ID = new DataTable();
                string ID = "";
                try
                {
                    Max_ID = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql1);
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
                    GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql2);
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
                StringBuilder sql_lastday = new StringBuilder();
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
                    sql_lastday.Append(string.Format("insert into KQ_PB_LD (PBID,BMID,KQID,YEAR,MONTH,LastDay) values ('{0}','{1}',{2},'{3}','{4}',{5});", ID, comboBox1.SelectedValue, Staff_WorkShift_SQL.Rows[i][0].ToString(),comboBoxYear.Text,comboBoxMonth.Text, Staff_WorkShift_SQL.Rows[i][2+Timespan.Days].ToString()));
                }

                try
                {
                    GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql.ToString());
                    GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql_lastday.ToString());
                    alter = true;
                    MessageBox.Show("保存成功！");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误3:" + ex.Message);
                    return;
                }

            }
        }

        private bool WriteLog(string details)
        {
            string sql_del = string.Format("select max(ID) from KQ_LOG");
            string ID = "";
            DataTable MaxID = new DataTable();
            try
            {
                MaxID = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql_del);
                if (MaxID.Rows[0][0].ToString() == "")
                {
                    ID = "1";
                }
                else
                {
                    ID = (Convert.ToInt32(MaxID.Rows[0][0].ToString()) + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误3:" + ex.Message);
                return false;
            }
          

            string Record = string.Format("{0}更改了{1}{2}{3}的排班记录", GlobalHelper.UserHelper.User["U_NAME"].ToString(), comboBox1.Text, comboBoxYear.Text, comboBoxMonth.Text);
            string sql = string.Format("insert into KQ_LOG (ID,Record,Time,Details) values ('{0}','{1}','{2}','{3}')",ID, Record, GlobalHelper.IDBHelper.GetServerDateTime(),details);

            try
            {
                GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误4:" + ex.Message);
                return false;
            }

            return true;
        }
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //SaveFileDialog sf = new SaveFileDialog();
            //sf.Filter = "Excel文件(*.xlsx)|*.xlsx";
            //if (sf.ShowDialog() == DialogResult.OK)
            //{
            //    try
            //    {
            //        string path = sf.FileName.ToString();
            //        gridControl1.ExportToXlsx(path);
            //        MessageBox.Show("导出成功！");
            //    }
            //    catch { }
            //    if (MessageBox.Show("是否打开？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            //        return;
            //    try
            //    {
            //        System.Diagnostics.Process.Start(sf.FileName);
            //    }
            //    catch { }
            //}
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "电子表格(*.xls)|*.xls";
            if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DevExpress.XtraExport.IExportProvider provider = new DevExpress.XtraExport.ExportXlsProvider(sf.FileName);
                DevExpress.XtraGrid.Export.BaseExportLink link = bandedGridView1.CreateExportLink(provider);
                link.ExportCellsAsDisplayText = true;
                try { link.ExportTo(true); }
                catch { }
                if (MessageBox.Show("是否打开？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
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

            MessageBox.Show("提示：在导入之前，请先关闭要读取的Excel文件!");

            //用于定位姓名列与排班第一列
            SchedualLocation form = new SchedualLocation();
            form.ShowDialog(this);

            if (ColumnLocation == false)
            {
                return;
            }else
            {
                ColumnLocation = false;
            }
           
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel文件(*.xls; *.xlsx)| *.xls; *.xlsx";//过滤文件类型
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
                //获取Excel的Sheet表名
                if (schemaTable.Rows.Count == 1)
                {
                    tableName = schemaTable.Rows[0][2].ToString().Trim();
                    TableNameHasChoosed = true;
                }
                else
                {
                    //多表时要选择
                    SchedualTableName form1 = new SchedualTableName();
                    form1.tableName = schemaTable.Copy();
                    form1.ShowDialog(this);
                }

                if (TableNameHasChoosed == true)
                {
                    TableNameHasChoosed = false;
                }else
                {
                    return;
                }
                string strSql = "select * from [" + tableName + "]";
                //获取Excel指定Sheet表中的信息
                OleDbDataAdapter myData = new OleDbDataAdapter(strSql, ExcelConn);
                myData.Fill(ds, tableName);//填充数据
                dtExcel = ds.Tables[tableName];
                ExcelConn.Close();
            }
            catch (Exception ex)
            {
                ExcelConn.Close();
                MessageBox.Show("错误2:" + ex.Message);                
                return;
            }

            //检查Excel中的列数是否存在错误
            if ((Timespan.Days+PBColumn)> dtExcel.Columns.Count)
            {
                MessageBox.Show("Excel文件或导入设置存在错误！");
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
                    string Name = dtExcel.Rows[j][NameColumn].ToString().Replace(" ", ""); //去掉空格
                    if (Name.IndexOf(Staff.Rows[i][1].ToString())>=0)
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
                            if (dtExcel.Rows[row][k + PBColumn].ToString().Trim()== WorkShift.Rows[n][1].ToString())
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
                string name = Week(DateTime.Parse(e.Column.FieldName));
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

        private void searchControl1_TextChanged(object sender, EventArgs e)
        {
            if (Staff_WorkShift.Rows.Count > 0)
            {
                Staff_WorkShift.DefaultView.RowFilter = string.Format("YGXM like '%{0}%'", searchControl1.Text);
            }
        }

        private void bandedGridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Staff_WorkShift.Rows.Count == 0)
            {
                return;
            }

            try
            {
                if (bandedGridView1.FocusedColumn.FieldName=="YGXM")
                {
                    return;
                }

                int row = 0;
                //找到焦点行
                for (int i = 0; i < Staff_WorkShift.Rows.Count;i++)
                {
                    if (Staff_WorkShift.Rows[i]["KQID"].ToString() == bandedGridView1.GetFocusedRowCellValue("KQID").ToString())
                    {
                        row = i;
                        break;
                    }
                }

                Staff_WorkShift.Rows[row][bandedGridView1.FocusedColumn.FieldName] = "";
            }
            catch { }

        }
    }
}
