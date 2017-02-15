using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin
{
    public partial class AttendanceAlter : Form
    {
        public string Name="";
        public string Date = "";
        public string Result = "";
        public double WorkDay = 0;
        public int Row=0;
        public int Column=0;
        public DateTime StartDate;
        public int Timespan;
        public bool AlterColumn;
        public AttendanceAlter()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            double day=0;
            try
            {
                day=Convert.ToDouble(textBox1.Text);
            }
            catch
            {
                MessageBox.Show("出勤天数必须是数字！");
                return;
            }

            if (comboBox1.Text == "")
            {
                MessageBox.Show("请输入考勤结果！");
                return;
            }

            if (AlterColumn == true)
            {
                Attendance form = (Attendance)this.Owner;
                for (int i = 0; i < form.AttendanceResult.Rows.Count; i++)
                {
                    form.AttendanceResult.Rows[i][Date] = comboBox1.Text;
                    form.WorkDayCount[i][Column] = day;
                }                
                form.DataCollect(StartDate, Timespan);

            }
            else
            {
                Attendance form = (Attendance)this.Owner;
                form.AttendanceResult.Rows[Row][Date] = comboBox1.Text;
                form.WorkDayCount[Row][Column] = day;
                form.DataCollect(StartDate, Timespan);
            }

            this.Close();        
        }

        private void AttendanceAlter_Load(object sender, EventArgs e)
        {
            if (AlterColumn == true)
            {
                label4.Text = "部门名称：";
                label3.Text = Name;
                textBox1.Text = "1";
                label5.Text = Convert.ToDateTime(Date).Year + "年" + Convert.ToDateTime(Date).Month + "月" + Convert.ToDateTime(Date).Day + "日";
            }
            else
            {
                label3.Text = Name;
                label5.Text = Convert.ToDateTime(Date).Year + "年" + Convert.ToDateTime(Date).Month + "月" + Convert.ToDateTime(Date).Day + "日";
                comboBox1.Text = Result;
                textBox1.Text = WorkDay.ToString();
            }
            
            
        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {

        }
    }
}
