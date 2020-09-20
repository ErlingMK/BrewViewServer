using System.Collections.Generic;

namespace BrewViewServer.Authentication.Google
{
    public class GoogleCertificates
    {
        public IList<Key> Keys { get; set; } = new List<Key>();
    }
}