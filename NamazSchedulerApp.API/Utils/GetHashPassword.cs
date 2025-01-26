using System.Text;
using System.Security.Cryptography;

namespace AuthenticationServiNamazSchedulerApp.API.Utils
{
    public class GetHashPassword
    {

        public string GetHashValue(string str)
        {
            UnicodeEncoding ue = new UnicodeEncoding();
            byte[] strInBytes = ue.GetBytes(str);
            SHA1Managed sha1 = new SHA1Managed();
            byte[] strHashValue = sha1.ComputeHash(strInBytes);

            StringBuilder strSB = new StringBuilder();
            foreach (byte u in strHashValue) strSB.Append(u.ToString("X2"));

            return strSB.ToString();
        }
    }
}
