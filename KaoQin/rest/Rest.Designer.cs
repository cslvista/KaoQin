namespace KaoQin.rest
{
    partial class Rest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Rest));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.searchDep = new DevExpress.XtraEditors.SearchControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.ButtonDelete = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonAdd = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonAlter = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonCal = new DevExpress.XtraEditors.SimpleButton();
            this.gridDep = new DevExpress.XtraGrid.GridControl();
            this.gridViewDep = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchDep.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelControl2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.gridDep, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.gridControl2, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(860, 635);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnRefresh);
            this.panelControl1.Controls.Add(this.searchDep);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(3, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(214, 50);
            this.panelControl1.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRefresh.Appearance.Options.UseFont = true;
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.Location = new System.Drawing.Point(152, 14);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(51, 24);
            this.btnRefresh.TabIndex = 17;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // searchDep
            // 
            this.searchDep.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.True;
            this.searchDep.Location = new System.Drawing.Point(14, 14);
            this.searchDep.Margin = new System.Windows.Forms.Padding(2);
            this.searchDep.Name = "searchDep";
            this.searchDep.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.searchDep.Properties.Appearance.Options.UseFont = true;
            this.searchDep.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Repository.ClearButton(),
            new DevExpress.XtraEditors.Repository.SearchButton()});
            this.searchDep.Properties.FindDelay = 100;
            this.searchDep.Properties.NullValuePrompt = "E";
            this.searchDep.Size = new System.Drawing.Size(127, 24);
            this.searchDep.TabIndex = 16;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.ButtonDelete);
            this.panelControl2.Controls.Add(this.ButtonRefresh);
            this.panelControl2.Controls.Add(this.ButtonAdd);
            this.panelControl2.Controls.Add(this.ButtonAlter);
            this.panelControl2.Controls.Add(this.ButtonCal);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(223, 3);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(634, 50);
            this.panelControl2.TabIndex = 4;
            // 
            // ButtonDelete
            // 
            this.ButtonDelete.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ButtonDelete.Appearance.Options.UseFont = true;
            this.ButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("ButtonDelete.Image")));
            this.ButtonDelete.Location = new System.Drawing.Point(159, 14);
            this.ButtonDelete.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonDelete.Name = "ButtonDelete";
            this.ButtonDelete.Size = new System.Drawing.Size(52, 24);
            this.ButtonDelete.TabIndex = 67;
            this.ButtonDelete.Text = "删除";
            // 
            // ButtonRefresh
            // 
            this.ButtonRefresh.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ButtonRefresh.Appearance.Options.UseFont = true;
            this.ButtonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("ButtonRefresh.Image")));
            this.ButtonRefresh.Location = new System.Drawing.Point(226, 14);
            this.ButtonRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonRefresh.Name = "ButtonRefresh";
            this.ButtonRefresh.Size = new System.Drawing.Size(52, 24);
            this.ButtonRefresh.TabIndex = 64;
            this.ButtonRefresh.Text = "刷新";
            // 
            // ButtonAdd
            // 
            this.ButtonAdd.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ButtonAdd.Appearance.Options.UseFont = true;
            this.ButtonAdd.Image = ((System.Drawing.Image)(resources.GetObject("ButtonAdd.Image")));
            this.ButtonAdd.Location = new System.Drawing.Point(24, 14);
            this.ButtonAdd.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonAdd.Name = "ButtonAdd";
            this.ButtonAdd.Size = new System.Drawing.Size(52, 24);
            this.ButtonAdd.TabIndex = 65;
            this.ButtonAdd.Text = "新增";
            // 
            // ButtonAlter
            // 
            this.ButtonAlter.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ButtonAlter.Appearance.Options.UseFont = true;
            this.ButtonAlter.Image = ((System.Drawing.Image)(resources.GetObject("ButtonAlter.Image")));
            this.ButtonAlter.Location = new System.Drawing.Point(91, 14);
            this.ButtonAlter.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonAlter.Name = "ButtonAlter";
            this.ButtonAlter.Size = new System.Drawing.Size(52, 24);
            this.ButtonAlter.TabIndex = 66;
            this.ButtonAlter.Text = "修改";
            // 
            // ButtonCal
            // 
            this.ButtonCal.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ButtonCal.Appearance.Options.UseFont = true;
            this.ButtonCal.Image = ((System.Drawing.Image)(resources.GetObject("ButtonCal.Image")));
            this.ButtonCal.Location = new System.Drawing.Point(296, 14);
            this.ButtonCal.Margin = new System.Windows.Forms.Padding(2);
            this.ButtonCal.Name = "ButtonCal";
            this.ButtonCal.Size = new System.Drawing.Size(78, 24);
            this.ButtonCal.TabIndex = 63;
            this.ButtonCal.Text = "休假规则";
            this.ButtonCal.Click += new System.EventHandler(this.ButtonCal_Click);
            // 
            // gridDep
            // 
            this.gridDep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDep.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            this.gridDep.Location = new System.Drawing.Point(2, 58);
            this.gridDep.MainView = this.gridViewDep;
            this.gridDep.Margin = new System.Windows.Forms.Padding(2);
            this.gridDep.Name = "gridDep";
            this.gridDep.Size = new System.Drawing.Size(216, 575);
            this.gridDep.TabIndex = 3;
            this.gridDep.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDep});
            // 
            // gridViewDep
            // 
            this.gridViewDep.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Blue;
            this.gridViewDep.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gridViewDep.Appearance.FocusedRow.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridViewDep.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gridViewDep.Appearance.FocusedRow.Options.UseFont = true;
            this.gridViewDep.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gridViewDep.Appearance.HeaderPanel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridViewDep.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridViewDep.Appearance.Row.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridViewDep.Appearance.Row.Options.UseFont = true;
            this.gridViewDep.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn8});
            this.gridViewDep.GridControl = this.gridDep;
            this.gridViewDep.Name = "gridViewDep";
            this.gridViewDep.OptionsBehavior.Editable = false;
            this.gridViewDep.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "部门ID";
            this.gridColumn1.FieldName = "BMID";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "部门名称";
            this.gridColumn2.FieldName = "BMMC";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "部门类型";
            this.gridColumn8.FieldName = "BMLB";
            this.gridColumn8.Name = "gridColumn8";
            // 
            // gridControl2
            // 
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            this.gridControl2.Location = new System.Drawing.Point(222, 58);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Margin = new System.Windows.Forms.Padding(2);
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(636, 575);
            this.gridControl2.TabIndex = 5;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Appearance.HeaderPanel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView2.Appearance.HeaderPanel.Options.UseFont = true;
            this.gridView2.Appearance.Row.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridView2.Appearance.Row.Options.UseFont = true;
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn9,
            this.gridColumn10});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn3, DevExpress.Data.ColumnSortOrder.Descending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn4, DevExpress.Data.ColumnSortOrder.Descending)});
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "年份";
            this.gridColumn3.FieldName = "YEAR";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "月份";
            this.gridColumn4.FieldName = "MONTH";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 1;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "创建人";
            this.gridColumn5.FieldName = "CJR";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 2;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "创建时间";
            this.gridColumn6.DisplayFormat.FormatString = "yyyy-MM-dd  HH:mm";
            this.gridColumn6.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn6.FieldName = "CJSJ";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 3;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "修改人";
            this.gridColumn7.FieldName = "XGR";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 4;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "修改时间";
            this.gridColumn9.DisplayFormat.FormatString = "yyyy-MM-dd  HH:mm";
            this.gridColumn9.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn9.FieldName = "XGSJ";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 5;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "排班ID";
            this.gridColumn10.FieldName = "PBID";
            this.gridColumn10.Name = "gridColumn10";
            // 
            // Rest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 635);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Rest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "休假设置";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Rest_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.searchDep.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnRefresh;
        private DevExpress.XtraEditors.SearchControl searchDep;
        private DevExpress.XtraGrid.GridControl gridDep;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDep;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        public DevExpress.XtraEditors.SimpleButton ButtonCal;
        private DevExpress.XtraEditors.SimpleButton ButtonDelete;
        private DevExpress.XtraEditors.SimpleButton ButtonRefresh;
        private DevExpress.XtraEditors.SimpleButton ButtonAdd;
        private DevExpress.XtraEditors.SimpleButton ButtonAlter;
    }
}