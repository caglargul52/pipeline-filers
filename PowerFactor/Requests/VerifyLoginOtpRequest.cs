namespace PowerFactor.Requests;

public class VerifyLoginOtpRequest
{
    public int CustomerId { get; set; }
    public string LoginOtp { get; set; }
    public string[] ApplicationNames { get; set; }
}