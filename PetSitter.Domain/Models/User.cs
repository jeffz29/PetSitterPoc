namespace PetSitter.Domain;

public class User
{
    public Guid Id { get; set; }
    public long InternalId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreateTimeUtc { get; set; }
}