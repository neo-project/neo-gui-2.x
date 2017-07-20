using Microsoft.Extensions.Configuration;

namespace Neo.Properties
{
    internal sealed partial class Settings
    {
        public string DataDirectoryPath { get; }
        public string CertCachePath { get; }
        public ushort NodePort { get; }
        public ushort WsPort { get; }
        public BrowserSettings Urls { get; }

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
