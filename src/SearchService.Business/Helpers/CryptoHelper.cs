namespace SearchService.Business.Helpers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class CryptoHelper
    {
        public string Encrypt(string toEncrypt, string key)
        {
            var toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            var keyArray = Convert.FromBase64String(key);

            var tdes = new TripleDESCryptoServiceProvider { Key = keyArray, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 };

            var cTransform = tdes.CreateEncryptor();
            var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public string Decrypt(string cipherString, string key)
        {
            if (cipherString != null)
            {
                var toEncryptArray = Convert.FromBase64String(cipherString.Replace(' ', '+'));
                var keyArray = Convert.FromBase64String(key);
                var tdes = new TripleDESCryptoServiceProvider { Key = keyArray, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 };

                var cTransform = tdes.CreateDecryptor();
                var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();

                return Encoding.UTF8.GetString(resultArray);
            }

            return string.Empty;
        }
    }
}