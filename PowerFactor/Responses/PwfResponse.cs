namespace PowerFactor.Responses;

public class Result
{
    
}

public class PwfResponse
{
    public string VerificationSignature { get; set; }
    public string SecretKey { get; set; }
    public string CipherText { get; set; }
    public string Content { get; set; }
    public List<Result> Results { get; set; }
}