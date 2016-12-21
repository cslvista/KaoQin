using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin.users
{
    public partial class add_alter_Users : Form
    {
        public bool alter = false;
        public add_alter_Users()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void AddUsers_Load(object sender, EventArgs e)
        {
            dateEdit1.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
            dateEdit1.Properties.Mask.EditMask = "yyyy-MM-dd";

            if (alter == true)
            {

            }else
            {

            }
        }
    }
}
