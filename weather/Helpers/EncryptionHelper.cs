using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class EncryptionHelper
{
    public static string EncryptString(string plainText, string keyString)
    {
        var key = Encoding.UTF8.GetBytes(keyString.PadRight(32, '0').Substring(0, 32));
#pragma warning disable CA1416 
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.GenerateIV();
            var iv = aesAlg.IV;

            using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
            using (var msEncrypt = new MemoryStream())
            {
                msEncrypt.Write(iv, 0, iv.Length);
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }
                var encrypted = msEncrypt.ToArray();
                return Convert.ToBase64String(encrypted);
            }
        }
#pragma warning restore CA1416 
    }
}
