using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin.DataOpeation
{
    public partial class ImportData : Form
    {
        public ImportData()
        {
            InitializeComponent();
        }

        private void ImportData_Load(object sender, EventArgs e)
        {

        }

        private void FromExcel_Click(object sender, EventArgs e)
        {
            Attendance form = (Attendance)this.Owner;
            form.ButtonImport_Click(sender, e);
        }

        private void FromDB_Click(object sender, EventArgs e)
        {
            SaveToDB form = new SaveToDB();
            form.Extract = true;
            form.Text = "提取考勤记录";
            form.Show(this);
        }

        private void FromMachine_Click(object sender, EventArgs e)
        {
            LoadingForm form = new LoadingForm();
            form.Show(this);
        }
    }
}
