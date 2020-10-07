namespace BrewView.DatabaseModels.User
{
    public class UserValidationResponse
    {
        public UserValidationResponse(bool succeeded, string idToken, string message, string refreshToken)
        {
            Succeeded = succeeded;
            IdToken = idToken;
            Message = message;
            RefreshToken = refreshToken;
        }

        public bool Succeeded { get; }
        public string Message { get; }
        public string IdToken { get; }
        public string RefreshToken { get;}

    }
}