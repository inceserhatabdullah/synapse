namespace Synapse.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public string Password { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<UserSession> _sessions = new();
    public IReadOnlyCollection<UserSession> Sessions => _sessions;

    private User()
    {
    }

    public User(string username, string password)
    {
        Id = Guid.NewGuid();
        Username = username;
        Password = password;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddSession(UserSession session)
    {
        _sessions.Add(session);
        session.SetUser(this);
    }
}