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
    public partial class RestMain : Form
    {
        public RestMain()
        {
            InitializeComponent();
        }


        private void btnRestRules_Click(object sender, EventArgs e)
        {
            RestRules form = new RestRules();
            form.Show();
        }

        private void btnPersonRest_Click(object sender, EventArgs e)
        {
            PersonRest form = new PersonRest();
            form.Show();
        }

        private void btnRestMain_Click(object sender, EventArgs e)
        {
            MonthRestCollect form = new MonthRestCollect();
            form.Show();
        }
    }
}
