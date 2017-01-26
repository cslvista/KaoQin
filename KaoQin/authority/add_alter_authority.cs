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
            StringBuilder PB = new StringBuilder();
            StringBuilder Dep = new StringBuilder();
            StringBuilder Device = new StringBuilder();
            StringBuilder Shift = new StringBuilder();
            StringBuilder Attendance = new StringBuilder();
            StringBuilder autority = new StringBuilder();

            foreach(TreeNode FirstNode in treeView1.Nodes)
            {
                foreach (TreeNode SecondNode in FirstNode.Nodes)
                {
                    foreach (TreeNode ThirdNode in SecondNode.Nodes)
                    {
                        if (SecondNode.Name== "排班管理")
                        {
                            if (ThirdNode.Text == "查看")
                            {
                                PB.Append(ThirdNode.Checked == true ? "1" :"0");
                            }
                            if (ThirdNode.Text == "修改")
                            {
                                PB.Append(ThirdNode.Checked == true ? "1" : "0");
                            }
                            if (ThirdNode.Text == "删除")
                            {
                                PB.Append(ThirdNode.Checked == true ? "1" : "0");
                            }
                            continue;
                        }

                        if (SecondNode.Name == "部门与员工")
                        {
                            if (ThirdNode.Text == "查看")
                            {
                                Dep.Append(ThirdNode.Checked == true ? "1" : "0");
                            }
                            if (ThirdNode.Text == "修改")
                            {
                                Dep.Append(ThirdNode.Checked == true ? "1" : "0");
                            }
                            if (ThirdNode.Text == "删除")
                            {
                                Dep.Append(ThirdNode.Checked == true ? "1" : "0");
                            }
                            continue;
                        }

                        if (SecondNode.Name == "设备管理")
                        {
                            if (ThirdNode.Text == "查看")
                            {
                                Device.Append(ThirdNode.Checked == true ? "1" : "0");
                            }
                            if (ThirdNode.Text == "修改与删除")
                            {
                                Device.Append(ThirdNode.Checked == true ? "1" : "0");
                            }
                            continue;
                        }

                        if (SecondNode.Name == "班次管理")
                        {
                            if (ThirdNode.Text == "查看")
                            {
                                Shift.Append(ThirdNode.Checked == true ? "1" : "0");
                            }
                            if (ThirdNode.Text == "修改")
                            {
                                Shift.Append(ThirdNode.Checked == true ? "1" : "0");
                            }
                            continue;
                        }

                        if (SecondNode.Name == "考勤管理")
                        {
                            if (ThirdNode.Text == "查看")
                            {
                                Attendance.Append(ThirdNode.Checked == true ? "1" : "0");
                            }
                            continue;
                        }

                        if (SecondNode.Name == "授权管理")
                        {
                            if (ThirdNode.Text == "查看")
                            {
                                autority.Append(ThirdNode.Checked == true ? "1" : "0");
                            }
                            if (ThirdNode.Text == "修改与删除")
                            {
                                autority.Append(ThirdNode.Checked == true ? "1" : "0");
                            }
                            continue;
                        }
                    }
                }
            }
           

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
            Dep.Text = "部门与员工";
            TreeNode Device = new TreeNode();
            Device.Text = "设备管理";
            TreeNode Shift = new TreeNode();
            Shift.Text = "班次管理";
            TreeNode Attendance = new TreeNode();
            Attendance.Text = "考勤管理";
            TreeNode Autority = new TreeNode();
            Autority.Text = "授权管理";

            mainControl.Nodes.Add(PB);
            mainControl.Nodes.Add(Dep);
            mainControl.Nodes.Add(Device);
            mainControl.Nodes.Add(Shift);
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
            TreeNode Dep_Del = new TreeNode();
            Dep_Del.Text = "删除";
            //设备管理
            TreeNode Device_Read = new TreeNode();
            Device_Read.Text = "查看";
            TreeNode Device_Edit = new TreeNode();
            Device_Edit.Text = "修改与删除";
            //班次管理
            TreeNode Shift_Read = new TreeNode();
            Shift_Read.Text = "查看";
            TreeNode Shift_Edit = new TreeNode();
            Shift_Edit.Text = "修改";
            //考勤管理
            TreeNode Attendance_Read = new TreeNode();
            Attendance_Read.Text = "查看";
            //授权管理
            TreeNode Autority_Read = new TreeNode();
            Autority_Read.Text = "查看";
            TreeNode Autority_Edit = new TreeNode();
            Autority_Edit.Text = "修改与删除";

            PB.Nodes.Add(PB_Read);
            PB.Nodes.Add(PB_Edit);
            PB.Nodes.Add(PB_Del);
            Dep.Nodes.Add(Dep_Read);
            Dep.Nodes.Add(Dep_Edit);
            Dep.Nodes.Add(Dep_Del);
            Device.Nodes.Add(Device_Read);
            Device.Nodes.Add(Device_Edit);
            Shift.Nodes.Add(Shift_Read);
            Shift.Nodes.Add(Shift_Edit);
            Attendance.Nodes.Add(Attendance_Read);
            Autority.Nodes.Add(Autority_Read);
            Autority.Nodes.Add(Autority_Edit);

            treeView1.Nodes.Add(mainControl);
            treeView1.ExpandAll();

            foreach (TreeNode FirstNode in treeView1.Nodes)
            {
                foreach (TreeNode SecondNode in FirstNode.Nodes)
                {
                    SecondNode.Name = SecondNode.Text;
                    foreach (TreeNode ThirdNode in SecondNode.Nodes)
                    {
                        ThirdNode.Name = SecondNode.Text+"_"+ThirdNode.Text;                        
                    }
                }
            }
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
