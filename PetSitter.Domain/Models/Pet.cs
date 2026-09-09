namespace PetSitter.Domain;

public class Pet
{
    public Guid Id { get; set; }
    public long InternalId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreateTimeUtc { get; set; }
    public DateTime LastSessionUtc { get; set; }
}