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

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void add_alter_authority_Load(object sender, EventArgs e)
        {
            treeView1.CheckBoxes = true;
            //treeView1.ShowLines = false;

            //第一级权限
            TreeNode mainControl = new TreeNode();
            mainControl.Text = "权限管理";
            //第二级权限
            TreeNode PB = new TreeNode();
            PB.Text = "排班管理";
            TreeNode Dep = new TreeNode();
            Dep.Text = "部门与员工管理";
            TreeNode Device = new TreeNode();
            Device.Text = "设备管理";
            TreeNode Arrange = new TreeNode();
            Arrange.Text = "班次管理";
            TreeNode Attendance = new TreeNode();
            Attendance.Text = "考勤管理";
            TreeNode Autority = new TreeNode();
            Autority.Text = "授权管理";

            mainControl.Nodes.Add(PB);
            mainControl.Nodes.Add(Dep);
            mainControl.Nodes.Add(Device);
            mainControl.Nodes.Add(Arrange);
            mainControl.Nodes.Add(Attendance);
            mainControl.Nodes.Add(Autority);
            //第三级权限
            //排班
            TreeNode PB_Read = new TreeNode();
            PB_Read.Text = "查看";
            TreeNode PB_Edit = new TreeNode();                   
            PB_Edit.Text = "修改";
            TreeNode PB_Del = new TreeNode();
            PB_Del.Text = "删除";
            //部门与员工
            TreeNode Dep_Read = new TreeNode();
            Dep_Read.Text = "查看";
            TreeNode Dep_Edit = new TreeNode();
            Dep_Edit.Text = "修改";
            //设备管理
            TreeNode Device_Read = new TreeNode();
            Device_Read.Text = "查看";
            TreeNode Device_Edit = new TreeNode();
            Device_Edit.Text = "修改";
            //班次管理
            TreeNode Arrange_Read = new TreeNode();
            Arrange_Read.Text = "查看";
            TreeNode Arrange_Edit = new TreeNode();
            Arrange_Edit.Text = "修改";
            //考勤管理
            TreeNode Attendance_Read = new TreeNode();
            Attendance_Read.Text = "查看";
            //授权管理
            TreeNode Autority_Read = new TreeNode();
            Autority_Read.Text = "查看";
            TreeNode Autority_Edit = new TreeNode();
            Autority_Edit.Text = "修改";

            PB.Nodes.Add(PB_Read);
            PB.Nodes.Add(PB_Edit);
            PB.Nodes.Add(PB_Del);
            Dep.Nodes.Add(Dep_Read);
            Dep.Nodes.Add(Dep_Edit);
            Device.Nodes.Add(Device_Read);
            Device.Nodes.Add(Device_Edit);
            Arrange.Nodes.Add(Arrange_Read);
            Arrange.Nodes.Add(Arrange_Edit);
            Attendance.Nodes.Add(Attendance_Read);
            Autority.Nodes.Add(Autority_Read);
            Autority.Nodes.Add(Autority_Edit);

            treeView1.Nodes.Add(mainControl);
            treeView1.ExpandAll();
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Checked == true)
                {
                    //选中节点之后，选中该节点所有的子节点
                    setChildNodeCheckedState(e.Node, true);
                }
                else if (e.Node.Checked == false)
                {
                    //取消节点选中状态之后，取消该节点所有子节点选中状态
                    setChildNodeCheckedState(e.Node, false);
                    //如果节点存在父节点，取消父节点的选中状态
                    if (e.Node.Parent != null)
                    {
                        setParentNodeCheckedState(e.Node, false);
                    }
                }
            }
        }

        //取消节点选中状态之后，取消所有父节点的选中状态
        private void setParentNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNode parentNode = currNode.Parent;
            parentNode.Checked = state;
            if (currNode.Parent.Parent != null)
            {
                setParentNodeCheckedState(currNode.Parent, state);
            }
        }
        //选中节点之后，选中节点的所有子节点
        private void setChildNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNodeCollection nodes = currNode.Nodes;
            if (nodes.Count > 0)
            {
                foreach (TreeNode tn in nodes)
                {
                    tn.Checked = state;
                    setChildNodeCheckedState(tn, state);
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //if (e.Action == TreeViewAction.ByMouse)
            //{
            //    if (e.Node.Checked == true)
            //    {
            //        //选中节点之后，选中该节点所有的子节点
            //        setChildNodeCheckedState(e.Node, true);
            //    }
            //    else if (e.Node.Checked == false)
            //    {
            //        //取消节点选中状态之后，取消该节点所有子节点选中状态
            //        setChildNodeCheckedState(e.Node, false);
            //        //如果节点存在父节点，取消父节点的选中状态
            //        if (e.Node.Parent != null)
            //        {
            //            setParentNodeCheckedState(e.Node, false);
            //        }
            //    }
            //}
        }
    }
}
