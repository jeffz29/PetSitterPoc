namespace PetSitter.Domain;

public class PetNote
{
    public Guid Id { get; set; }
    public long InternalId { get; set; }
    public long PetId { get; set; }
    public string Note { get; set; } = string.Empty;
}