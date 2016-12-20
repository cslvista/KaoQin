using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KaoQin
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            searchControl1.Properties.NullValuePrompt = " ";
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            Schedual form = new Schedual();
            form.Show(this);
        }

        private void 机器设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            machine.machine form = new machine.machine();
            form.Show();
        }

        private void 员工设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            users.MainUsers form = new users.MainUsers();
            form.Show();
        }
    }
}
