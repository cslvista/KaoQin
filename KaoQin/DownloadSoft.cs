using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace KaoQin
{
    public partial class DownloadSoft : Form
    {
        string ftpServerIP = "";
        string ftpUser = "";
        string ftpPwd = "";
        public DownloadSoft()
        {
            InitializeComponent();
        }

        private void ButtonDownSDK_Click(object sender, EventArgs e)
        {
        }

        private void ButtonDownExcel_Click(object sender, EventArgs e)
        {
            
            string fileName = @"FTPData/FunctionModule/MZSYS/妇产科床位预约/BCYY.dll";
            string DownfilePath = @"E:\";
            
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + fileName));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUser, ftpPwd);
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                WebResponse response = reqFTP.GetResponse();
                FileStream outputStream = new FileStream(DownfilePath, FileMode.Create);
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DownloadSoft_Load(object sender, EventArgs e)
        {
            ftpServerIP = "192.168.8.16";
            ftpUser = "administrator";
            ftpPwd = "123";
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            FontsClearup.FixRegistryFonts();
        }
    }
}
