using PowerFactor;
using PowerFactor.Requests;

var request = new GenerateActivationOtpRequest
{
    ApplicationName = "",
    CustomerId = 21321312
};

var client = new PWFClient();

var result = await client.GenerateActivationOtp(request);

if (result.Data is not null)
{
    Console.WriteLine(result.Data.Otp);
}