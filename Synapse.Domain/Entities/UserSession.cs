namespace Synapse.Domain.Entities;

public class UserSession
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string RefreshToken { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsRevoked { get; private set; }

    public User User { get; private set; } = null;

    private UserSession()
    {
    }

    public UserSession(Guid userId, string refreshToken = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        RefreshToken = refreshToken;
        CreatedAt = DateTime.UtcNow;
        IsRevoked = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Revoke() => IsRevoked = true;

    public void UpdateRefreshToken(string refreshToken) => RefreshToken = refreshToken;
    public void SetUser(User user) => User = user;
}