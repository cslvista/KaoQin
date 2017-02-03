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
    public partial class SchedualLocation : Form
    {
        public SchedualLocation()
        {
            InitializeComponent();
        }

        private void SchedualLocation_Load(object sender, EventArgs e)
        {
            comboBoxName.Text = "B";
            comboBoxPB.Text = "E";
            UILocation();
        }

        private void UILocation()
        {
            int x = 2;
            int y = 2;
            label4.Location = new Point(comboBoxName.Location.X - label4.Width - x, comboBoxName.Location.Y + y);
            label1.Location = new Point(comboBoxPB.Location.X - label1.Width - x, comboBoxPB.Location.Y + y);
            label3.Location = new Point(comboBoxName.Location.X + comboBoxName.Width + x, comboBoxName.Location.Y + y);
            label2.Location = new Point(comboBoxPB.Location.X + comboBoxPB.Width  + x, comboBoxPB.Location.Y + y);
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
                case "D": name = 3; break;
                case "E": name = 4; break;
                case "F": name = 5; break;
            }

            switch (comboBoxPB.Text)
            {
                case "B": PB = 1; break;
                case "C": PB = 2; break;
                case "D": PB = 3; break;
                case "E": PB = 4; break;
                case "F": PB = 5; break;
                case "G": PB = 6; break;
                case "H": PB = 7; break;
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
