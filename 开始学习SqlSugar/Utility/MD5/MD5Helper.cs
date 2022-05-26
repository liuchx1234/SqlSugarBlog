using System.Security.Cryptography;
using System.Text;
namespace 开始学习SqlSugar.MD5
{
    public static class MD5Helper
    {
        public static string MD5Encrypt32(string password)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(password));
            var sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
