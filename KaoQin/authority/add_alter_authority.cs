﻿using System;
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
        public bool alter = false;
        DataTable users = new DataTable();
        public string ID = "";
        public string name = "";
        DataTable Authority = new DataTable();
        public add_alter_authority()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (gridLookUpEdit1.Text == "")
            {
                MessageBox.Show("请输入姓名！" );
                return;
            }

            StringBuilder PB = new StringBuilder();
            StringBuilder Dep = new StringBuilder();
            StringBuilder Device = new StringBuilder();
            StringBuilder Shift = new StringBuilder();
            StringBuilder Attendance = new StringBuilder();
            StringBuilder Authority_s = new StringBuilder();

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
                            }else 
                            if (ThirdNode.Text == "新增")
                            {
                                PB.Append(ThirdNode.Checked == true ? "1" : "0");
                            }
                            else
                            if (ThirdNode.Text == "修改")
                            {
                                PB.Append(ThirdNode.Checked == true ? "1" : "0");
                            }
                            else 
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
                            }else
                            if (ThirdNode.Text == "新增与修改")
                            {
                                Dep.Append(ThirdNode.Checked == true ? "1" : "0");
                            }else
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
                            }else
                            if (ThirdNode.Text == "新增与修改")
                            {
                                Device.Append(ThirdNode.Checked == true ? "1" : "0");
                            }else 
                            if (ThirdNode.Text == "删除")
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
                            }else
                            if (ThirdNode.Text == "新增与修改")
                            {
                                Shift.Append(ThirdNode.Checked == true ? "1" : "0");
                            }
                            continue;
                        }

                        if (SecondNode.Name == "考勤查询")
                        {
                            if (ThirdNode.Text == "查看")
                            {
                                Attendance.Append(ThirdNode.Checked == true ? "1" : "0");
                            }
                            if (ThirdNode.Text == "读取考勤机")
                            {
                                Attendance.Append(ThirdNode.Checked == true ? "1" : "0");
                            }
                            if (ThirdNode.Text == "删除数据库")
                            {
                                Attendance.Append(ThirdNode.Checked == true ? "1" : "0");
                            }
                            continue;
                        }

                        if (SecondNode.Name == "授权管理")
                        {
                            if (ThirdNode.Text == "查看")
                            {
                                Authority_s.Append(ThirdNode.Checked == true ? "1" : "0");
                            }else
                            if (ThirdNode.Text == "新增与修改")
                            {
                                Authority_s.Append(ThirdNode.Checked == true ? "1" : "0");
                            }else 
                            if (ThirdNode.Text == "删除")
                            {
                                Authority_s.Append(ThirdNode.Checked == true ? "1" : "0");
                            }
                            continue;
                        }
                    }
                }
            }

            string sql = "";

            if (alter)
            {
                sql =string.Format("update KQ_SQ set PBGL='{0}',YGGL='{1}',SBGL='{2}',BCGL='{3}',KQGL='{4}',SQGL='{5}' where ID='{6}'", PB.ToString(), Dep.ToString(), Device.ToString(), Shift.ToString(), Attendance.ToString(), Authority_s.ToString(),ID);
            }            
            else
            {
                sql = string.Format("insert into KQ_SQ (ID,Name,PBGL,YGGL,SBGL,BCGL,KQGL,SQGL) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')"
                , gridLookUpEdit1.EditValue.ToString(), gridLookUpEdit1.Text, PB.ToString(), Dep.ToString(), Device.ToString(), Shift.ToString(), Attendance.ToString(), Authority_s.ToString());
            }
            try
            {
                
                GlobalHelper.IDBHelper.ExecuteNonQuery(DBLink.key, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            Authority form = (Authority)this.Owner;
            form.ButtonRefresh_Click(null, null);
            this.Close();
        }

        private void add_alter_authority_Load(object sender, EventArgs e)
        {
            gridLookUpEdit1.Properties.DisplayMember = "U_NAME";
            gridLookUpEdit1.Properties.ValueMember = "U_ACCOUNT";
            gridLookUpEdit1.Properties.AutoComplete = false;
            gridLookUpEdit1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            gridLookUpEdit1.Properties.ImmediatePopup = true;
            gridLookUpEdit1.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            treeView1.CheckBoxes = true;

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
            Attendance.Text = "考勤查询";
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
            TreeNode PB_Add = new TreeNode();
            PB_Add.Text = "新增";
            TreeNode PB_Edit = new TreeNode();                   
            PB_Edit.Text = "修改";
            TreeNode PB_Del = new TreeNode();
            PB_Del.Text = "删除";
            //部门与员工
            TreeNode Dep_Read = new TreeNode();
            Dep_Read.Text = "查看";
            TreeNode Dep_Edit = new TreeNode();
            Dep_Edit.Text = "新增与修改";
            TreeNode Dep_Del = new TreeNode();
            Dep_Del.Text = "删除";
            //设备管理
            TreeNode Device_Read = new TreeNode();
            Device_Read.Text = "查看";
            TreeNode Device_Edit = new TreeNode();
            Device_Edit.Text = "新增与修改";
            TreeNode Device_Del = new TreeNode();
            Device_Del.Text = "删除";
            //班次管理
            TreeNode Shift_Read = new TreeNode();
            Shift_Read.Text = "查看";
            TreeNode Shift_Edit = new TreeNode();
            Shift_Edit.Text = "新增与修改";
            //考勤查询
            TreeNode Attendance_Read = new TreeNode();
            Attendance_Read.Text = "查看";
            TreeNode Attendance_VisitMachine = new TreeNode();
            Attendance_VisitMachine.Text = "读取考勤机";
            TreeNode Attendance_DelDB = new TreeNode();
            Attendance_DelDB.Text = "删除数据库";
            //授权管理
            TreeNode Autority_Read = new TreeNode();
            Autority_Read.Text = "查看";
            TreeNode Autority_Edit = new TreeNode();
            Autority_Edit.Text = "新增与修改";
            TreeNode Autority_Del = new TreeNode();
            Autority_Del.Text = "删除";

            PB.Nodes.Add(PB_Read);
            PB.Nodes.Add(PB_Add);
            PB.Nodes.Add(PB_Edit);
            PB.Nodes.Add(PB_Del);
            Dep.Nodes.Add(Dep_Read);
            Dep.Nodes.Add(Dep_Edit);
            Dep.Nodes.Add(Dep_Del);
            Device.Nodes.Add(Device_Read);
            Device.Nodes.Add(Device_Edit);
            Device.Nodes.Add(Device_Del);
            Shift.Nodes.Add(Shift_Read);
            Shift.Nodes.Add(Shift_Edit);
            Attendance.Nodes.Add(Attendance_Read);
            Attendance.Nodes.Add(Attendance_VisitMachine);
            Attendance.Nodes.Add(Attendance_DelDB);
            Autority.Nodes.Add(Autority_Read);
            Autority.Nodes.Add(Autority_Edit);
            Autority.Nodes.Add(Autority_Del);

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

            try
            {
                string sql = "select U_ACCOUNT,U_NAME from USERS";
                users = GlobalHelper.IDBHelper.ExecuteDataTable(GlobalHelper.GloValue.USER, sql);
                gridLookUpEdit1.Properties.DataSource = users;
                gridLookUpEdit1.EditValue = ID;                
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误1:" + ex.Message, "提示");
                return;
            }

            if (alter)
            {
                gridLookUpEdit1.Enabled = false;
                try
                {
                    string sql = string.Format("select * from KQ_SQ where ID='{0}'", ID);
                    Authority = GlobalHelper.IDBHelper.ExecuteDataTable(DBLink.key, sql);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误1:" + ex.Message, "提示");
                    return;
                }

                //排班
                if (Authority.Rows[0]["PBGL"].ToString() == "1111")
                {
                    treeView1.Nodes[0].Nodes[0].Checked = true;
                    treeView1.Nodes[0].Nodes[0].Nodes[0].Checked = true;
                    treeView1.Nodes[0].Nodes[0].Nodes[1].Checked = true;
                    treeView1.Nodes[0].Nodes[0].Nodes[2].Checked = true;
                    treeView1.Nodes[0].Nodes[0].Nodes[3].Checked = true;
                }
                else
                {
                    string s1 = Authority.Rows[0]["PBGL"].ToString().Substring(0, 1);
                    string s2 = Authority.Rows[0]["PBGL"].ToString().Substring(1, 1);
                    string s3 = Authority.Rows[0]["PBGL"].ToString().Substring(2, 1);
                    string s4 = Authority.Rows[0]["PBGL"].ToString().Substring(3, 1);
                    treeView1.Nodes[0].Nodes[0].Nodes[0].Checked = s1 == "1" ? true : false;
                    treeView1.Nodes[0].Nodes[0].Nodes[1].Checked = s2 == "1" ? true : false;
                    treeView1.Nodes[0].Nodes[0].Nodes[2].Checked = s3 == "1" ? true : false;
                    treeView1.Nodes[0].Nodes[0].Nodes[3].Checked = s4 == "1" ? true : false;
                }
                //员工
                if (Authority.Rows[0]["YGGL"].ToString() == "111")
                {
                    treeView1.Nodes[0].Nodes[1].Checked = true;
                    treeView1.Nodes[0].Nodes[1].Nodes[0].Checked = true;
                    treeView1.Nodes[0].Nodes[1].Nodes[1].Checked = true;
                    treeView1.Nodes[0].Nodes[1].Nodes[2].Checked = true;
                }
                else
                {
                    string s1 = Authority.Rows[0]["YGGL"].ToString().Substring(0, 1);
                    string s2 = Authority.Rows[0]["YGGL"].ToString().Substring(1, 1);
                    string s3 = Authority.Rows[0]["YGGL"].ToString().Substring(2, 1);
                    treeView1.Nodes[0].Nodes[1].Nodes[0].Checked = s1 == "1" ? true : false;
                    treeView1.Nodes[0].Nodes[1].Nodes[1].Checked = s2 == "1" ? true : false;
                    treeView1.Nodes[0].Nodes[1].Nodes[2].Checked = s3 == "1" ? true : false;
                }
                //设备
                if (Authority.Rows[0]["SBGL"].ToString() == "111")
                {
                    treeView1.Nodes[0].Nodes[2].Checked = true;
                    treeView1.Nodes[0].Nodes[2].Nodes[0].Checked = true;
                    treeView1.Nodes[0].Nodes[2].Nodes[1].Checked = true;
                    treeView1.Nodes[0].Nodes[2].Nodes[2].Checked = true;
                }
                else
                {
                    string s1 = Authority.Rows[0]["SBGL"].ToString().Substring(0, 1);
                    string s2 = Authority.Rows[0]["SBGL"].ToString().Substring(1, 1);
                    string s3 = Authority.Rows[0]["SBGL"].ToString().Substring(2, 1);
                    treeView1.Nodes[0].Nodes[2].Nodes[0].Checked = s1 == "1" ? true : false;
                    treeView1.Nodes[0].Nodes[2].Nodes[1].Checked = s2 == "1" ? true : false;
                    treeView1.Nodes[0].Nodes[2].Nodes[2].Checked = s3 == "1" ? true : false;
                }
                //班次
                if (Authority.Rows[0]["BCGL"].ToString() == "11")
                {
                    treeView1.Nodes[0].Nodes[3].Checked = true;
                    treeView1.Nodes[0].Nodes[3].Nodes[0].Checked = true;
                    treeView1.Nodes[0].Nodes[3].Nodes[1].Checked = true;
                }
                else
                {
                    string s1 = Authority.Rows[0]["BCGL"].ToString().Substring(0, 1);
                    string s2 = Authority.Rows[0]["BCGL"].ToString().Substring(1, 1);
                    treeView1.Nodes[0].Nodes[3].Nodes[0].Checked = s1 == "1" ? true : false;
                    treeView1.Nodes[0].Nodes[3].Nodes[1].Checked = s2 == "1" ? true : false;
                }

                //考勤
                if (Authority.Rows[0]["KQGL"].ToString() == "111")
                {
                    treeView1.Nodes[0].Nodes[4].Checked = true;
                    treeView1.Nodes[0].Nodes[4].Nodes[0].Checked = true;
                    treeView1.Nodes[0].Nodes[4].Nodes[1].Checked = true;
                    treeView1.Nodes[0].Nodes[4].Nodes[2].Checked = true;
                }
                else
                {
                    string s1 = Authority.Rows[0]["KQGL"].ToString().Substring(0, 1);
                    string s2 = Authority.Rows[0]["KQGL"].ToString().Substring(1, 1);
                    string s3 = Authority.Rows[0]["KQGL"].ToString().Substring(2, 1);
                    treeView1.Nodes[0].Nodes[4].Nodes[0].Checked = s1 == "1" ? true : false;
                    treeView1.Nodes[0].Nodes[4].Nodes[1].Checked = s2 == "1" ? true : false;
                    treeView1.Nodes[0].Nodes[4].Nodes[2].Checked = s3 == "1" ? true : false;
                }

                //授权
                if (Authority.Rows[0]["SQGL"].ToString() == "111")
                {
                    treeView1.Nodes[0].Nodes[5].Checked = true;
                    treeView1.Nodes[0].Nodes[5].Nodes[0].Checked = true;
                    treeView1.Nodes[0].Nodes[5].Nodes[1].Checked = true;
                    treeView1.Nodes[0].Nodes[5].Nodes[2].Checked = true;
                }
                else
                {
                    string s1 = Authority.Rows[0]["SQGL"].ToString().Substring(0, 1);
                    string s2 = Authority.Rows[0]["SQGL"].ToString().Substring(1, 1);
                    string s3 = Authority.Rows[0]["SQGL"].ToString().Substring(2, 1);
                    treeView1.Nodes[0].Nodes[5].Nodes[0].Checked = s1 == "1" ? true : false;
                    treeView1.Nodes[0].Nodes[5].Nodes[1].Checked = s2 == "1" ? true : false;
                    treeView1.Nodes[0].Nodes[5].Nodes[2].Checked = s3 == "1" ? true : false;
                }

                if (Authority.Rows[0]["PBGL"].ToString() == "1111" && Authority.Rows[0]["YGGL"].ToString() == "111")
                {
                    if (Authority.Rows[0]["SBGL"].ToString() == "111" && Authority.Rows[0]["BCGL"].ToString() == "11")
                    {
                        if (Authority.Rows[0]["KQGL"].ToString() == "111" && Authority.Rows[0]["SQGL"].ToString() == "111")
                        {
                            treeView1.Nodes[0].Checked = true;
                        }
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
        }

        private void gridLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
