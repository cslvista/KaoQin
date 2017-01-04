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
    public partial class PersonData : Form
    {
        public DataTable PersonRecord = new DataTable();
        public PersonData()
        {
            InitializeComponent();
        }

        private void SearchDetail_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = PersonRecord;
        }
    }
}
