namespace BrewView.Contracts.User
{
    public class UserValidationResponse
    {
        public UserValidationResponse(bool succeeded, UserValidationResponseMessage message = UserValidationResponseMessage.Error, string idToken = "", string refreshToken = "")
        {
            Succeeded = succeeded;
            IdToken = idToken;
            Message = message;
            RefreshToken = refreshToken;
        }

        public bool Succeeded { get; }
        public UserValidationResponseMessage Message { get; }
        public string IdToken { get; }
        public string RefreshToken { get;}
        public AuthenticationProvider AuthenticationProvider { get; }

    }

    public enum UserValidationResponseMessage
    {
        UserExists = 0,
        UserCreated = 1,
        InvalidPassword = 2,
        UserDoesNotExist = 3,
        SignedIn = 4,
        Error = 5
    }
}