using Neo.Persistence.LevelDB;
using Neo.Properties;
using Neo.UI;
using Neo.Wallets;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Neo
{
    internal static class Program
    {
        public static NeoSystem NeoSystem;
        public static Wallet CurrentWallet;
        public static MainForm MainForm;

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            using (FileStream fs = new FileStream("error.log", FileMode.Create, FileAccess.Write, FileShare.None))
            using (StreamWriter w = new StreamWriter(fs))
                if (e.ExceptionObject is Exception ex)
                {
                    PrintErrorLogs(w, ex);
                }
                else
                {
                    w.WriteLine(e.ExceptionObject.GetType());
                    w.WriteLine(e.ExceptionObject);
                }
        }

        private static bool InstallCertificate()
        {
            if (!Settings.Default.InstallCertificate) return true;
            using (X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine))
            using (X509Certificate2 cert = new X509Certificate2(Resources.OnchainCertificate))
            {
                store.Open(OpenFlags.ReadOnly);
                if (store.Certificates.Contains(cert)) return true;
            }
            using (X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine))
            using (X509Certificate2 cert = new X509Certificate2(Resources.OnchainCertificate))
            {
                try
                {
                    store.Open(OpenFlags.ReadWrite);
                    store.Add(cert);
                    return true;
                }
                catch (CryptographicException) { }
                if (MessageBox.Show(Strings.InstallCertificateText, Strings.InstallCertificateCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                {
                    Settings.Default.InstallCertificate = false;
                    Settings.Default.Save();
                    return true;
                }
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = Application.ExecutablePath,
                        UseShellExecute = true,
                        Verb = "runas",
                        WorkingDirectory = Environment.CurrentDirectory
                    });
                    return false;
                }
                catch (Win32Exception) { }
                MessageBox.Show(Strings.InstallCertificateCancel);
                return true;
            }
        }

        [STAThread]
        public static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            XDocument xdoc = null;
            try
            {
                xdoc = XDocument.Load("https://raw.githubusercontent.com/neo-project/neo-gui/master/update.xml");
            }
            catch { }
            if (xdoc != null)
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                Version minimum = Version.Parse(xdoc.Element("update").Attribute("minimum").Value);
                if (version < minimum)
                {
                    using (UpdateDialog dialog = new UpdateDialog(xdoc))
                    {
                        dialog.ShowDialog();
                    }
                    return;
                }
            }
            if (!InstallCertificate()) return;
            using (LevelDBStore store = new LevelDBStore(Settings.Default.Paths.Chain))
            using (NeoSystem = new NeoSystem(store))
            {
                Application.Run(MainForm = new MainForm(xdoc));
            }
        }

        private static void PrintErrorLogs(StreamWriter writer, Exception ex)
        {
            writer.WriteLine(ex.GetType());
            writer.WriteLine(ex.Message);
            writer.WriteLine(ex.StackTrace);
            if (ex is AggregateException ex2)
            {
                foreach (Exception inner in ex2.InnerExceptions)
                {
                    writer.WriteLine();
                    PrintErrorLogs(writer, inner);
                }
            }
            else if (ex.InnerException != null)
            {
                writer.WriteLine();
                PrintErrorLogs(writer, ex.InnerException);
            }
        }
    }
}
