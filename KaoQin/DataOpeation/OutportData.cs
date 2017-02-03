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
    public partial class OutportData : Form
    {
        public DataTable Record_DKJ = new DataTable();
        public OutportData()
        {
            InitializeComponent();
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            Attendance form = (Attendance)this.Owner;
            form.ButtonExport_Click(sender,e);
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {

        }

        private void OutportData_Load(object sender, EventArgs e)
        {

        }
    }
}
