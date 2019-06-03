using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace POE_MVC.Helpers
{
    class Encryption
    {

        public static string GetMD5Hash(string input)
        {
            //Create MD5 Object
            MD5 md5Hash = MD5.Create();
            //Create byte[] of hashed input
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            string result = "";
            foreach (var _byte in data)
            {
                //Convert each byte to Hex String
                result += _byte.ToString("x2");
            }
            return result;
        }

    }
}

