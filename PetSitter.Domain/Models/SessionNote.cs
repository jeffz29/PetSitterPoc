namespace PetSitter.Domain;

public class SessionNote
{
    public Guid Id { get; set; }
    public long InternalId { get; set; }
    public string Note { get; set; } = string.Empty;
}