namespace KaoQin.rest
{
    partial class RestMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RestMain));
            this.btnRestMain = new DevExpress.XtraEditors.SimpleButton();
            this.btnPersonRest = new DevExpress.XtraEditors.SimpleButton();
            this.btnRestRules = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // btnRestMain
            // 
            this.btnRestMain.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRestMain.Appearance.Options.UseFont = true;
            this.btnRestMain.Image = ((System.Drawing.Image)(resources.GetObject("btnRestMain.Image")));
            this.btnRestMain.Location = new System.Drawing.Point(42, 137);
            this.btnRestMain.Margin = new System.Windows.Forms.Padding(2);
            this.btnRestMain.Name = "btnRestMain";
            this.btnRestMain.Size = new System.Drawing.Size(108, 26);
            this.btnRestMain.TabIndex = 49;
            this.btnRestMain.Text = "每月余休记录";
            this.btnRestMain.Click += new System.EventHandler(this.btnRestMain_Click);
            // 
            // btnPersonRest
            // 
            this.btnPersonRest.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPersonRest.Appearance.Options.UseFont = true;
            this.btnPersonRest.Image = ((System.Drawing.Image)(resources.GetObject("btnPersonRest.Image")));
            this.btnPersonRest.Location = new System.Drawing.Point(42, 85);
            this.btnPersonRest.Margin = new System.Windows.Forms.Padding(2);
            this.btnPersonRest.Name = "btnPersonRest";
            this.btnPersonRest.Size = new System.Drawing.Size(108, 26);
            this.btnPersonRest.TabIndex = 48;
            this.btnPersonRest.Text = "个人休假记录";
            this.btnPersonRest.Click += new System.EventHandler(this.btnPersonRest_Click);
            // 
            // btnRestRules
            // 
            this.btnRestRules.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRestRules.Appearance.Options.UseFont = true;
            this.btnRestRules.Image = ((System.Drawing.Image)(resources.GetObject("btnRestRules.Image")));
            this.btnRestRules.Location = new System.Drawing.Point(42, 33);
            this.btnRestRules.Margin = new System.Windows.Forms.Padding(2);
            this.btnRestRules.Name = "btnRestRules";
            this.btnRestRules.Size = new System.Drawing.Size(108, 26);
            this.btnRestRules.TabIndex = 47;
            this.btnRestRules.Text = "休假规则设定";
            this.btnRestRules.Click += new System.EventHandler(this.btnRestRules_Click);
            // 
            // RestMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(193, 202);
            this.Controls.Add(this.btnRestMain);
            this.Controls.Add(this.btnPersonRest);
            this.Controls.Add(this.btnRestRules);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RestMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "休假管理";
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnRestRules;
        private DevExpress.XtraEditors.SimpleButton btnPersonRest;
        private DevExpress.XtraEditors.SimpleButton btnRestMain;
    }
}