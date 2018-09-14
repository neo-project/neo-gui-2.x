using Neo.Properties;
using Neo.SmartContract;
using Neo.Wallets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using ECCurve = Neo.Cryptography.ECC.ECCurve;
using ECPoint = Neo.Cryptography.ECC.ECPoint;

namespace Neo.Cryptography
{
    internal static class CertificateQueryService
    {
        private static Dictionary<UInt160, CertificateQueryResult> results = new Dictionary<UInt160, CertificateQueryResult>();

        static CertificateQueryService()
        {
            Directory.CreateDirectory(Settings.Default.Paths.CertCache);
        }

        private static void Web_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            using ((WebClient)sender)
            {
                UInt160 hash = (UInt160)e.UserState;
                if (e.Cancelled || e.Error != null)
                {
                    lock (results)
                    {
                        results[hash].Type = CertificateQueryResultType.Missing;
                    }
                }
                else
                {
                    string address = hash.ToAddress();
                    string path = Path.Combine(Settings.Default.Paths.CertCache, $"{address}.cer");
                    File.WriteAllBytes(path, e.Result);
                    lock (results)
                    {
                        UpdateResultFromFile(hash);
                    }
                }
            }
        }

        public static CertificateQueryResult Query(ECPoint pubkey)
        {
            return Query(Contract.CreateSignatureRedeemScript(pubkey).ToScriptHash());
        }

        public static CertificateQueryResult Query(UInt160 hash)
        {
            lock (results)
            {
                if (results.ContainsKey(hash)) return results[hash];
                results[hash] = new CertificateQueryResult { Type = CertificateQueryResultType.Querying };
            }
            string address = hash.ToAddress();
            string path = Path.Combine(Settings.Default.Paths.CertCache, $"{address}.cer");
            if (File.Exists(path))
            {
                lock (results)
                {
                    UpdateResultFromFile(hash);
                }
            }
            else
            {
                string url = $"http://cert.onchain.com/antshares/{address}.cer";
                WebClient web = new WebClient();
                web.DownloadDataCompleted += Web_DownloadDataCompleted;
                web.DownloadDataAsync(new Uri(url), hash);
            }
            return results[hash];
        }

        private static void UpdateResultFromFile(UInt160 hash)
        {
            string address = hash.ToAddress();
            X509Certificate2 cert;
            try
            {
                cert = new X509Certificate2(Path.Combine(Settings.Default.Paths.CertCache, $"{address}.cer"));
            }
            catch (CryptographicException)
            {
                results[hash].Type = CertificateQueryResultType.Missing;
                return;
            }
            if (cert.PublicKey.Oid.Value != "1.2.840.10045.2.1")
            {
                results[hash].Type = CertificateQueryResultType.Missing;
                return;
            }
            if (!hash.Equals(Contract.CreateSignatureRedeemScript(ECPoint.DecodePoint(cert.PublicKey.EncodedKeyValue.RawData, ECCurve.Secp256r1)).ToScriptHash()))
            {
                results[hash].Type = CertificateQueryResultType.Missing;
                return;
            }
            using (X509Chain chain = new X509Chain())
            {
                results[hash].Certificate = cert;
                if (chain.Build(cert))
                {
                    results[hash].Type = CertificateQueryResultType.Good;
                }
                else if (chain.ChainStatus.Length == 1 && chain.ChainStatus[0].Status == X509ChainStatusFlags.NotTimeValid)
                {
                    results[hash].Type = CertificateQueryResultType.Expired;
                }
                else
                {
                    results[hash].Type = CertificateQueryResultType.Invalid;
                }
            }
        }
    }
}
