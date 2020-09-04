using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BrewViewServer.Util
{
    public class StringGenerator
    {

        public static string GenerateStateValue()
        {
            var byteArray = new byte[32];
            var randomString = "";
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(byteArray);

                randomString = Convert.ToBase64String(byteArray);

            }
            return randomString;
        }
    }
}
