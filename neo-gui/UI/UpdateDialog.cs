using Neo.Properties;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Neo.UI
{
    internal partial class UpdateDialog : Form
    {
        private readonly WebClient web = new WebClient();
        private readonly string download_url;
        private string download_path;

        public UpdateDialog(XDocument xdoc)
        {
            InitializeComponent();
            Version latest = Version.Parse(xdoc.Element("update").Attribute("latest").Value);
            textBox1.Text = latest.ToString();
            XElement release = xdoc.Element("update").Elements("release").First(p => p.Attribute("version").Value == latest.ToString());
            textBox2.Text = release.Element("changes").Value.Replace("\n", Environment.NewLine);
            download_url = release.Attribute("file").Value;
            web.DownloadProgressChanged += Web_DownloadProgressChanged;
            web.DownloadFileCompleted += Web_DownloadFileCompleted;
        }

        private void Web_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Web_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null) return;
            DirectoryInfo di = new DirectoryInfo("update");
            if (di.Exists) di.Delete(true);
            di.Create();
            ZipFile.ExtractToDirectory(download_path, di.Name);
            FileSystemInfo[] fs = di.GetFileSystemInfos();
            if (fs.Length == 1 && fs[0] is DirectoryInfo)
            {
                ((DirectoryInfo)fs[0]).MoveTo("update2");
                di.Delete();
                Directory.Move("update2", di.Name);
            }
            File.WriteAllBytes("update.bat", Resources.UpdateBat);
            Close();
            if (Program.MainForm != null) Program.MainForm.Close();
            Process.Start("update.bat");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://neo.org/");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(download_url);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            download_path = "update.zip";
            web.DownloadFileAsync(new Uri(download_url), download_path);
        }
    }
}
