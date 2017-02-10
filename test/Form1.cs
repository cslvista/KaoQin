using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;


namespace test
{
    public partial class Form1 : Form
    {
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "MZSYS";
            textBox3.Text = "1";
            textBox2.Text = "800";
            textBox4.Text = "450";

            button1_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Func<bool> method = GlobalHelper.UserHelper.UserLog;
            method.BeginInvoke(null, null);

            IntPtr ParenthWnd = new IntPtr(0);
            AutoLogin.AutoLogin AL = new AutoLogin.AutoLogin();
            if (AL.autoLogin(textBox1.Text, textBox3.Text, Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox4.Text), "") == true)
            {
                Thread.Sleep(50);
                ParenthWnd = FindWindow(null, "用户登录");
                if (ParenthWnd.Equals(IntPtr.Zero))
                {
                    KaoQin.Main form = new KaoQin.Main();
                    form.Show();

                }
            }

        }
    }
}
