using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaoQin.authority
{
    public partial class add_alter_authority : Form
    {
        public add_alter_authority()
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

        private void add_alter_authority_Load(object sender, EventArgs e)
        {
            treeView1.CheckBoxes = true;
            //treeView1.ShowLines = false;
            //treeView1.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            TreeNode ZB = new TreeNode();            
            TreeNode PB = new TreeNode();
            TreeNode PB_Read = new TreeNode();
            TreeNode PB_Edit = new TreeNode();
            PB.Text = "排班管理";
            PB_Read.Text = "查看";
            PB_Edit.Text = "编辑";
            PB.Nodes.Add(PB_Read);
            PB.Nodes.Add(PB_Edit);


            treeView1.Nodes.Add(PB);
            
        }
    }
}
