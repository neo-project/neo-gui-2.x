using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Neo.Properties
{
    internal sealed partial class Settings
    {
        public string DataDirectoryPath { get; }
        public string CertCachePath { get; }
        public ushort NodePort { get; }
        public ushort WsPort { get; }
        public BrowserSettings Urls { get; }
        public string[] BootstrapMirrorUrls { get; }                            // a list of urls that chain.acc is stored on
        public string BootstrapFile { get; }                                    // mainnet and testnet chain.acc filenames
        public string BootstrapHashListURL { get; }                             // official neo file hash list

        public Settings()
        {
            if (NeedUpgrade)
            {
                Upgrade();
                NeedUpgrade = false;
                Save();
            }
            IConfigurationSection section = new ConfigurationBuilder().AddJsonFile("config.json").Build().GetSection("ApplicationConfiguration");
            this.DataDirectoryPath = section.GetSection("DataDirectoryPath").Value;
            this.CertCachePath = section.GetSection("CertCachePath").Value;
            this.NodePort = ushort.Parse(section.GetSection("NodePort").Value);
            this.WsPort = ushort.Parse(section.GetSection("WsPort").Value);
            this.Urls = new BrowserSettings(section.GetSection("Urls"));

            IConfigurationSection bootstrapData = section.GetSection("BootstrapDownload");
            this.BootstrapMirrorUrls = bootstrapData.GetSection("mirrors").GetChildren().Select(n => n.Value).ToArray();
            this.BootstrapFile = bootstrapData.GetSection("file").Value;
            this.BootstrapHashListURL = bootstrapData.GetSection("hashList").Value;
        }
    }

    internal class BrowserSettings
    {
        public string AddressUrl { get; }
        public string AssetUrl { get; }
        public string TransactionUrl { get; }

        public BrowserSettings(IConfigurationSection section)
        {
            this.AddressUrl = section.GetSection("AddressUrl").Value;
            this.AssetUrl = section.GetSection("AssetUrl").Value;
            this.TransactionUrl = section.GetSection("TransactionUrl").Value;
        }
    }
}
