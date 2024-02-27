namespace PowerFactor.Responses;

public enum PwfActivationStatus
{
    Active = 1,
    DeleteByClient = 2,
    DeleteByAdmin = 3,
    Blocked= 4
}

public class GetUserListResponse
{
    public PwfActivationStatus ActivationStatus { get; set; }
    public string ActivationToken { get; set; }
    public int CustomerId { get; set; }
    public DateTime ActivationDate { get; set; }
    public string DeviceModel { get; set; }
    public DateTime LastLoginDate { get; set; }
    public string DeviceOSName { get; set; }
    public string DeviceOSVersion { get; set; }
}