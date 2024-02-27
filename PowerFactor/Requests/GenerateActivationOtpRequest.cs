namespace PowerFactor.Requests;

public class GenerateActivationOtpRequest
{
    public int CustomerId { get; set; }
    public string ApplicationName { get; set; }
}