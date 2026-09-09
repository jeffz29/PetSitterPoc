namespace PetSitter.Domain;

public class PetCareSession
{
    public Guid Id { get; set; }
    public long InternalId { get; set; }
    public int StatusId { get; set; }
    public DateTime StartTimeUtc { get; set; }
    public int DurationMinutes { get; set; }
    public DateTime EndTimeUtc { get; set; }
    public long ProviderId { get; set; }
    public long CustomerId { get; set; }
    public SessionType SessionType { get; set; }
    public decimal CustomerPrice { get; set; }
    public decimal TransactionFee { get; set; }
}