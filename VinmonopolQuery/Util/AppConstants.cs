namespace VinmonopolQuery.Util
{
    public class AppConstants
    {
        public class Vinmonopol
        {
            public const string ApiUrl = "https://apis.vinmonopolet.no/products/v0/";
            public const string ProductDetailsEndPoint = "details-normal";
            public const string ApiKeyName = "Ocp-Apim-Subscription-Key";
            public const string ApiKey = "fb2d7604481041d381138b8bbe78e89f";
        }


        public const string DbConnection = @"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Brew;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";
        public const string DbConnectionSqlite = @"Data Source=../../../../Brew.db";
    }
}