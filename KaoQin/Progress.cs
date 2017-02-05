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
    public partial class Progress : Form
    {
        public Progress()
        {
            InitializeComponent();
        }

        private void Progress_Load(object sender, EventArgs e)
        {
        }
        public void CloseProgress()
        {
            this.Close();
        }

        public void ShowData(int i)
        {
            progressBar1.Value = i;
        }
    }
}
