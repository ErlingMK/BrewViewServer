namespace BrewView.Server.Services
{
    public interface IUserService
    {
        string GetUserName();
        string CurrentUser { get; }
    }
}