﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin
{
    public partial class PersonData : Form
    {
        public DataTable PersonRecord = new DataTable();
        public DataTable PersonShift = new DataTable();
        public DataTable WorkShift = new DataTable();
        public double workDay = 0;
        public string name = "";
        public string Date="";
        public PersonData()
        {
            InitializeComponent();
        }

        private void SearchDetail_Load(object sender, EventArgs e)
        {
            int month = Convert.ToDateTime(Date).Month;
            int day= Convert.ToDateTime(Date).Day;
            string weekdays = Week(Convert.ToDateTime(Date));
            string date = month + "月" + day + "日";
            this.Text = name + " 排班与签到 "+string.Format("({0} {1})",date,weekdays);
            gridControl1.DataSource = PersonRecord;
            gridView1.BestFitColumns();
            txtCQ.Text = "记为出勤：" + workDay.ToString()+"天";

            for (int i = 0; i < PersonShift.Rows.Count; i++)
            {
                //判断昨日的排班是否为跨天，如果不是，就删除昨日排班
                if (PersonShift.Rows[i]["PD"].ToString() == "0")
                {
                    if (PersonShift.Rows[i]["KT"].ToString() == "0")
                    {
                        PersonShift.Rows.RemoveAt(i);
                    }
                }
            }

            gridControl2.DataSource = PersonShift;
            gridView2.BestFitColumns();
            
        }

        private string Week(DateTime Day)
        {
            string[] weekdays = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            return weekdays[Convert.ToInt32(Day.DayOfWeek)];
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

            if (e.Column.FieldName == "PD")
            {
                switch (e.Value.ToString())
                {
                    case "0": e.DisplayText = "昨日"; break;
                    case "1": e.DisplayText = "今日"; break;
                }
            }

            if (e.Column.FieldName == "ID")
            {
                for (int i = 0; i < WorkShift.Rows.Count; i++)
                {
                    if (e.Value.ToString() == WorkShift.Rows[i]["ID"].ToString())
                    {
                        e.DisplayText = WorkShift.Rows[i]["NAME"].ToString();
                        break;
                    }
                }
            }

        }

        private void gridView2_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "KT")
            {
                string KT = gridView2.GetRowCellDisplayText(e.RowHandle, gridView2.Columns["KT"]);

                if (KT == "否")
                {
                    e.Appearance.ForeColor = Color.Blue;
                }
                else 
                {
                    e.Appearance.ForeColor = Color.Red;
                }
            }

        }
    }
}
