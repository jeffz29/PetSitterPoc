namespace PetSitter.Domain;

public class Provider
{
    public Guid Id { get; set; }
    public long InternalId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreateTimeUtc { get; set; }
    public ProviderSkills Skills { get; set; }
}