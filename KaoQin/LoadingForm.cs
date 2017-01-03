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
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();
        }

        private void Loading_Load(object sender, EventArgs e)
        {
            this.Height = pictureBox1.Height;
            this.Width = pictureBox1.Width;
            
        }
    }
}
