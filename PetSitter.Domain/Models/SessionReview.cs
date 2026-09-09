namespace PetSitter.Domain;

public class SessionReview
{
    public Guid Id { get; set; }
    public long InternalId { get; set; }
    public DateTime DateTimeUtc { get; set; }
    public long ProviderId { get; set; }
    public long CustomerId { get; set; }
    public decimal Score { get; set; }
    public string Notes { get; set; } = string.Empty;
}