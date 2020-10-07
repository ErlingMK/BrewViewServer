namespace BrewView.Server.Services.Abstractions
{
    public interface IUserService
    {
        string GetUserName();
        string CurrentUser { get; }
    }
}