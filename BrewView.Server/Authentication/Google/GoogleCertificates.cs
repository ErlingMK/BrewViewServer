using System.Collections.Generic;

namespace BrewView.Server.Authentication.Google
{
    public class GoogleCertificates
    {
        public IList<Key> Keys { get; set; } = new List<Key>();
    }
}