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
    public partial class SchedualLocation : Form
    {
        public SchedualLocation()
        {
            InitializeComponent();
        }

        private void SchedualLocation_Load(object sender, EventArgs e)
        {
            comboBoxName.Text = "A";
            comboBoxPB.Text = "C";
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            int name = 0;
            int PB = 0;

            switch (comboBoxName.Text)
            {
                case "A": name = 0; break;
                case "B": name = 1; break;
                case "C": name = 2; break;
            }

            switch (comboBoxPB.Text)
            {
                case "B": PB = 1; break;
                case "C": PB = 2; break;
                case "D": PB = 3; break;
                case "E": PB = 4; break;
                case "F": PB = 5; break;
            }

            if (PB <= name)
            {
                MessageBox.Show("排班列必须大于姓名列！");
                return;
            }

            Schedual form = (Schedual)this.Owner;
            form.PBColumn = PB;
            form.NameColumn = name;
            form.ColumnLocation = true;
            this.Close();
        }
    }
}