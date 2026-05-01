namespace Synapse.Domain.Users;

public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public string Password { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<UserSession> _userSession = new();
    public IReadOnlyCollection<UserSession> UserSessions => _userSession.AsReadOnly();

    private User()
    {
    }

    public User(string username, string password)
    {
        Id = Guid.NewGuid();
        Username = username;
        Password = password;
        CreatedAt = DateTime.UtcNow;
    }
}