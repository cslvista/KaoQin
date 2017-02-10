namespace KaoQin
{
    partial class DownloadSoft
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadSoft));
            this.ButtonDownSDK = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonDownExcel = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // ButtonDownSDK
            // 
            this.ButtonDownSDK.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ButtonDownSDK.Appearance.Options.UseFont = true;
            this.ButtonDownSDK.Image = ((System.Drawing.Image)(resources.GetObject("ButtonDownSDK.Image")));
            this.ButtonDownSDK.Location = new System.Drawing.Point(32, 23);
            this.ButtonDownSDK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ButtonDownSDK.Name = "ButtonDownSDK";
            this.ButtonDownSDK.Size = new System.Drawing.Size(184, 31);
            this.ButtonDownSDK.TabIndex = 53;
            this.ButtonDownSDK.Text = "下载SDK";
            this.ButtonDownSDK.Click += new System.EventHandler(this.ButtonDownSDK_Click);
            // 
            // ButtonDownExcel
            // 
            this.ButtonDownExcel.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ButtonDownExcel.Appearance.Options.UseFont = true;
            this.ButtonDownExcel.Image = ((System.Drawing.Image)(resources.GetObject("ButtonDownExcel.Image")));
            this.ButtonDownExcel.Location = new System.Drawing.Point(32, 76);
            this.ButtonDownExcel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ButtonDownExcel.Name = "ButtonDownExcel";
            this.ButtonDownExcel.Size = new System.Drawing.Size(184, 31);
            this.ButtonDownExcel.TabIndex = 54;
            this.ButtonDownExcel.Text = "下载Excel库";
            this.ButtonDownExcel.Click += new System.EventHandler(this.ButtonDownExcel_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(32, 129);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(184, 31);
            this.simpleButton1.TabIndex = 55;
            this.simpleButton1.Text = "处理非法字符";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // DownloadSoft
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(255, 181);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.ButtonDownExcel);
            this.Controls.Add(this.ButtonDownSDK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DownloadSoft";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "下载软件";
            this.Load += new System.EventHandler(this.DownloadSoft_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraEditors.SimpleButton ButtonDownSDK;
        public DevExpress.XtraEditors.SimpleButton ButtonDownExcel;
        public DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}