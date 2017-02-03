namespace KaoQin.DataOpeation
{
    partial class OutportData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutportData));
            this.ButtonRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonDelete = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // ButtonRefresh
            // 
            this.ButtonRefresh.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ButtonRefresh.Appearance.Options.UseFont = true;
            this.ButtonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("ButtonRefresh.Image")));
            this.ButtonRefresh.Location = new System.Drawing.Point(39, 102);
            this.ButtonRefresh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ButtonRefresh.Name = "ButtonRefresh";
            this.ButtonRefresh.Size = new System.Drawing.Size(148, 30);
            this.ButtonRefresh.TabIndex = 38;
            this.ButtonRefresh.Text = "保存到Excel";
            this.ButtonRefresh.Click += new System.EventHandler(this.ButtonRefresh_Click);
            // 
            // ButtonDelete
            // 
            this.ButtonDelete.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ButtonDelete.Appearance.Options.UseFont = true;
            this.ButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("ButtonDelete.Image")));
            this.ButtonDelete.Location = new System.Drawing.Point(38, 41);
            this.ButtonDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ButtonDelete.Name = "ButtonDelete";
            this.ButtonDelete.Size = new System.Drawing.Size(148, 30);
            this.ButtonDelete.TabIndex = 37;
            this.ButtonDelete.Text = "保存到数据库";
            this.ButtonDelete.Click += new System.EventHandler(this.ButtonDelete_Click);
            // 
            // OutportData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 172);
            this.Controls.Add(this.ButtonRefresh);
            this.Controls.Add(this.ButtonDelete);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OutportData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "保存数据";
            this.Load += new System.EventHandler(this.OutportData_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton ButtonDelete;
        private DevExpress.XtraEditors.SimpleButton ButtonRefresh;
    }
}