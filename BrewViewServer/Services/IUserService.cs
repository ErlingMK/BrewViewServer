namespace BrewViewServer.Services
{
    public interface IUserService
    {
        string GetUserName();
        string CurrentUser { get; }
    }
}