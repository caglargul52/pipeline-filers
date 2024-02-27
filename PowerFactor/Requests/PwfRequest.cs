namespace PowerFactor.Requests;

public class PWFRequest
{
    public string SecretKey { get; set; }
    public string CipherText { get; set; }
    public string VerificationSignature { get; set; }
    public bool IsMutualAuthenticationRequired { get; set; }
}