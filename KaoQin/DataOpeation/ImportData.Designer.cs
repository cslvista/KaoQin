namespace KaoQin.DataOpeation
{
    partial class ImportData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportData));
            this.FromExcel = new DevExpress.XtraEditors.SimpleButton();
            this.FromDB = new DevExpress.XtraEditors.SimpleButton();
            this.FromMachine = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // FromExcel
            // 
            this.FromExcel.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FromExcel.Appearance.Options.UseFont = true;
            this.FromExcel.Image = ((System.Drawing.Image)(resources.GetObject("FromExcel.Image")));
            this.FromExcel.Location = new System.Drawing.Point(37, 154);
            this.FromExcel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.FromExcel.Name = "FromExcel";
            this.FromExcel.Size = new System.Drawing.Size(169, 30);
            this.FromExcel.TabIndex = 37;
            this.FromExcel.Text = "从Excel文件读取";
            this.FromExcel.Click += new System.EventHandler(this.FromExcel_Click);
            // 
            // FromDB
            // 
            this.FromDB.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FromDB.Appearance.Options.UseFont = true;
            this.FromDB.Image = ((System.Drawing.Image)(resources.GetObject("FromDB.Image")));
            this.FromDB.Location = new System.Drawing.Point(37, 94);
            this.FromDB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.FromDB.Name = "FromDB";
            this.FromDB.Size = new System.Drawing.Size(169, 30);
            this.FromDB.TabIndex = 36;
            this.FromDB.Text = "从数据库下载";
            this.FromDB.Click += new System.EventHandler(this.FromDB_Click);
            // 
            // FromMachine
            // 
            this.FromMachine.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FromMachine.Appearance.Options.UseFont = true;
            this.FromMachine.Image = ((System.Drawing.Image)(resources.GetObject("FromMachine.Image")));
            this.FromMachine.Location = new System.Drawing.Point(37, 34);
            this.FromMachine.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.FromMachine.Name = "FromMachine";
            this.FromMachine.Size = new System.Drawing.Size(169, 30);
            this.FromMachine.TabIndex = 35;
            this.FromMachine.Text = "从考勤机下载";
            this.FromMachine.Click += new System.EventHandler(this.FromMachine_Click);
            // 
            // ImportData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(241, 221);
            this.Controls.Add(this.FromExcel);
            this.Controls.Add(this.FromDB);
            this.Controls.Add(this.FromMachine);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导入数据";
            this.Load += new System.EventHandler(this.ImportData_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton FromExcel;
        private DevExpress.XtraEditors.SimpleButton FromDB;
        private DevExpress.XtraEditors.SimpleButton FromMachine;
    }
}