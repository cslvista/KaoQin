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
        public int Row;
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
            if (AlterColumn == true)
            {
                Attendance form = (Attendance)this.Owner;
                for (int i = 0; i < form.AttendanceResult.Rows.Count; i++)
                {
                    form.AttendanceResult.Rows[i][Date] = comboBox1.Text;
                }                
                form.DataCollect(StartDate, Timespan);                
            }
            else
            {
                Attendance form = (Attendance)this.Owner;
                form.AttendanceResult.Rows[Row][Date] = comboBox1.Text;
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
                label5.Text = Convert.ToDateTime(Date).Year + "年" + Convert.ToDateTime(Date).Month + "月" + Convert.ToDateTime(Date).Day + "日";
            }
            else
            {
                label3.Text = Name;
                label5.Text = Convert.ToDateTime(Date).Year + "年" + Convert.ToDateTime(Date).Month + "月" + Convert.ToDateTime(Date).Day + "日";
                comboBox1.Text = Result;
            }
            
        }
    }
}
