using Microsoft.Extensions.Configuration;
using Neo.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Neo.UI
{
    public partial class DownloadChainACC : Form
    {
        private string chainDataFilePath;
        private string hashDataFilePath;
        private string fullURL;
        private bool doneInit = false;
        private int measurements = 0;
        private int maxDataPoints = 5;
        private int lastBytesReceived = 0;
        private double[] dataPoints;
        private Stopwatch downloadTimer;
        private WebClient downloadClient;

        public DownloadChainACC()
        {
            InitializeComponent();
            dataPoints = new double[maxDataPoints];

            comboMirrorLocation.Items.AddRange(Settings.Default.BootstrapMirrorUrls);
            comboMirrorLocation.SelectedIndex = 0;

            doneInit = true;
            UpdateTargetURL();
        }

        /**
         * download the md5 hash data for chain files from neo.org 
         */
        private void DownloadBootstrapHashData()
        {
            UpdateStatusText(Strings.BoostrapNeoBlockchainDownloadSigs);
            hashDataFilePath = Application.StartupPath + "\\" + URIFilename(Settings.Default.BootstrapHashListURL);

            using (WebClient wc = new WebClient())
            {
                wc.DownloadFileCompleted += ChainHashDataDownloadCompleted;
                wc.DownloadFileAsync(new Uri(Settings.Default.BootstrapHashListURL), hashDataFilePath);
            }
        }

        /**
         * user has clicked download, retrieve the chain.acc data defined in config
         */
        private void DownloadBootstrap_Click(object sender, EventArgs e)
        {
            if (btnDownload.Tag.Equals("download"))
            {
                SetButtonToCancel();
                downloadClient = new WebClient();
                downloadClient.DownloadProgressChanged += DownloadProgressChanged;
                downloadClient.DownloadFileCompleted += ChainDataDownloadCompleted;
                downloadClient.DownloadFileAsync(new Uri(fullURL), chainDataFilePath);
                downloadTimer = Stopwatch.StartNew();
            } else
            {
                SetButtonToDownload();
                downloadClient.CancelAsync();
            }
        }

        /**
         * calculate md5 hash for filePath
         */
        private byte[] MD5Sum(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    return md5.ComputeHash(stream);
                }
            }
        }

        /**
         * update the URL we will download from - called on form init and combobox changes
         */
        private void UpdateTargetURL()
        {
            if (!doneInit)
            {
                // don't want this to fire when form is initialising
                return;
            }

            string chainDataFileName = Settings.Default.BootstrapFile;
            chainDataFilePath = Application.StartupPath + "\\" + chainDataFileName;
            fullURL = $"{comboMirrorLocation.SelectedItem}/{chainDataFileName}";
        }

        /**
         * update the progress bar and label describing how much data has been downloaded
         */
        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            string mbTotal = (e.TotalBytesToReceive / 1024 / 1024).ToString() + "MB";
            string mbRecv = (e.BytesReceived / 1024 / 1024).ToString() + "MB";

            if (downloadTimer.ElapsedMilliseconds > 500)
            {
                downloadTimer.Stop();
                double msElapsed = downloadTimer.Elapsed.TotalMilliseconds;
                int bytesDownloaded = (int)e.BytesReceived - lastBytesReceived;
                lastBytesReceived = (int)e.BytesReceived;
                double dataPoint = bytesDownloaded / (msElapsed / 1000);
                dataPoints[measurements++ % maxDataPoints] = dataPoint;

                double downloadSpeed = dataPoints.Average();
                string downloadSpeedStr = (Math.Round(downloadSpeed / 1024)).ToString();
                downloadTimer.Restart();
                UpdateStatusText($"{mbRecv} of {mbTotal} {downloadSpeedStr}KB/s");
            }

            progressDownload.Value = e.ProgressPercentage;
        }

        /**
         * update the label value with messages to the user
         */
        private void UpdateStatusText(string message)
        {
            lblFileSize.Text = message;
        }

        /**
         * set the download button to read download
         */
        private void SetButtonToDownload()
        {
            btnDownload.Text = Strings.BoostrapNeoBlockchainDownload;
            btnDownload.Tag = "download";
        }

        /**
         *  set the download button to read cancel
         */
        private void SetButtonToCancel()
        {
            btnDownload.Text = Strings.BoostrapNeoBlockchainCancel;
            btnDownload.Tag = "cancel";
        }
        /**
         * file hash data has finished downloading, verify our download
         */
        private void ChainHashDataDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                UpdateStatusText(Strings.BoostrapNeoBlockchainDownloadCancelled);
                SetButtonToDownload();
                return;
            }

            if (e.Error != null)
            {
                UpdateStatusText(Strings.BoostrapNeoBlockchainDownloadFailed + e.Error.Message);
                SetButtonToDownload();
                return;
            }

            // file md5 sum will be stored in chain.acc.zip or chain.acc.test.zip
            string requiredFileHash = new ConfigurationBuilder().AddJsonFile(hashDataFilePath).Build().GetSection(Settings.Default.BootstrapFile).Value;
            string downloadedFileHash = MD5Sum(chainDataFilePath).ToHexString();

            if (requiredFileHash.Equals(downloadedFileHash))
            {
                UpdateStatusText(Strings.BoostrapNeoBlockchainDownloadSuccessful);
                DialogResult = DialogResult.OK;
            }
            else
            {
                UpdateStatusText(Strings.BoostrapNeoBlockchainSignatureInvalid);
                if (File.Exists("invalid_" + Settings.Default.BootstrapFile))
                {
                    File.Delete("invalid_" + Settings.Default.BootstrapFile);
                }
                File.Move(chainDataFilePath, "invalid_" + Settings.Default.BootstrapFile);
                SetButtonToDownload();
            }
        }

        /**
         * chain data has finished downloading
         */
        private void ChainDataDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            progressDownload.Value = 0;

            if (e.Cancelled)
            {
                UpdateStatusText(Strings.BoostrapNeoBlockchainDownloadCancelled);
                SetButtonToDownload();
                return;
            }

            if (e.Error != null)
            {
                UpdateStatusText(Strings.BoostrapNeoBlockchainDownloadFailed + e.Error.Message);
                SetButtonToDownload();
                return;
            }

            // download of chain data finished, retrieve file hash data from neo.org
            DownloadBootstrapHashData();
        }

        /**
         * get the basename of a url
         */
        private string URIFilename(string downloadLink)
        {
            Uri uri = new Uri(downloadLink);

            string filename = Path.GetFileName(uri.LocalPath);

            return filename;
        }

        /**
         * mirro combobox has changed, update target url
         */
        private void MirrorLocation_Changed(object sender, EventArgs e)
        {
            UpdateTargetURL();
        }

        private void DownloadChainACC_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(downloadClient != null && downloadClient.IsBusy)
            {
                downloadClient.CancelAsync();
            }
        }
    }
}
