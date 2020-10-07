using System;
using System.Security.Cryptography;

namespace BrewView.Server.Util.StringUtils
{
    public class StringGenerator
    {

        public static string GenerateStateValue()
        {
            var byteArray = new byte[32];
            var randomString = "";
            using var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(byteArray);

            randomString = Convert.ToBase64String(byteArray);
            return randomString;
        }
    }
}
