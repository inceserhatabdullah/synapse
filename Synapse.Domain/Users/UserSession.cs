namespace Synapse.Domain.Users;

public class UserSession
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string RefreshToken { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public bool IsRevoked { get; private set; }
    
    public User user { get; private set; }
    
    private  UserSession() {}
    
    public UserSession(Guid userId, string refreshToken)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        RefreshToken = refreshToken;
        CreatedAt = DateTime.UtcNow;
        IsRevoked = false;
    }
    
    public void Revoke() => IsRevoked = true;
}