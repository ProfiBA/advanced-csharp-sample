using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WebService.Utils
{
    public static class Utils
    {
        // Calculate MD5 Hash for file
        public static string GetMD5(string filepath)
        {
            try
            {
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    var buffer = md5.ComputeHash(File.ReadAllBytes(filepath));
                    var sb = new StringBuilder();
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        sb.Append(buffer[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            catch (Exception)
            {
                return "Error while calculating hash";
            }
        }  
    }
}