using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin.rest
{
    public partial class PersonRest : Form
    {
        DataTable personRest = new DataTable();
        public PersonRest()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {

        }

        private void PersonRest_Load(object sender, EventArgs e)
        {
            searchPersonRecord.Properties.NullValuePrompt = "请输入姓名";
            string TimeNow = GlobalHelper.IDBHelper.GetServerDateTime();
            comboBoxYear.Items.Add(Convert.ToDateTime(TimeNow).AddYears(1).Year.ToString() + "年");
            comboBoxYear.Items.Add(Convert.ToDateTime(TimeNow).Year.ToString() + "年");
            comboBoxYear.Items.Add(Convert.ToDateTime(TimeNow).AddYears(-1).Year.ToString() + "年");
            comboBoxYear.Items.Add(Convert.ToDateTime(TimeNow).AddYears(-2).Year.ToString() + "年");
            comboBoxYear.Text = Convert.ToDateTime(TimeNow).Year.ToString() + "年";
            comboBoxMonth.Text = Convert.ToDateTime(TimeNow).Month.ToString() + "月";
            SearchPersonRecord();
        }

        private void SearchPersonRecord()
        {

        }

        private void comboBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchPersonRecord();
        }

        private void comboBoxMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchPersonRecord();
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            SearchPersonRecord();
        }
    }
}
