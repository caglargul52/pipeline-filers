using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using PowerFactor.Requests;
using PowerFactor.Responses;

namespace PowerFactor;

public abstract class PWFClientBase
{
    string pwfApiLink = "https://demo.coreapi.dev.pwfactor.com/";
    string pwfBackOfficeApiLink = "https://demo.coreapi.dev.pwfactor.com/";

    string pwfServicePath = "api/member/";
    string pwfBackOfficePath = "api/BackOffice/";
    
    private static HttpClient httpClient;
    private static HttpClient backOfficehttpClient;
    
    public async Task<(TResponse? Data, bool IsError)> PostAsync<TResponse, TRequest>(TRequest request, string methodName)
    {
        string json = JsonConvert.SerializeObject(request);
        var pwfRequest = SecurityHelpers.EncryptRequest(json);
        pwfRequest.IsMutualAuthenticationRequired = true;

        httpClient = GetHttpClient(pwfApiLink);

        HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(pwfRequest), Encoding.UTF8, "application/json");
        HttpResponseMessage httpResponse = await httpClient.PostAsync($"{pwfServicePath}{methodName}", contentPost);

        if (!httpResponse.IsSuccessStatusCode)
        {
            return (default, true);
        }

        string asyncResponse = await httpResponse.Content.ReadAsStringAsync();
        var pwfResponse = JsonConvert.DeserializeObject<PwfResponse>(asyncResponse);

        if (pwfRequest.IsMutualAuthenticationRequired && pwfResponse?.CipherText != null)
        {
            var verifySign = SecurityHelpers.VerifySign(pwfResponse.CipherText, pwfResponse.VerificationSignature);
            if (!verifySign)
            {
                return (default, true);
            }

            var aesToken = SecurityHelpers.RSADecryption(pwfResponse.SecretKey);
            var clearText = SecurityHelpers.AESDecryption(pwfResponse.CipherText, aesToken);
            var model = JsonConvert.DeserializeObject<TResponse>(clearText);

            return (model, false);
        }

        return (default, true);
    }
    
    public async Task<(TResponse? Data, bool IsError)> PostAsyncOfBackOffice<TResponse, TRequest>(TRequest request, string methodName)
    {
        string json = JsonConvert.SerializeObject(request); 
        var encodeString = SecurityHelpers.Base64Encode(json);
        
        var pwfRequest = SecurityHelpers.EncryptRequest(encodeString);
        pwfRequest.IsMutualAuthenticationRequired = true;

        backOfficehttpClient = GetBackOfficeHttpClient(pwfBackOfficeApiLink);
        
        HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(pwfRequest), Encoding.UTF8, "application/json");
        HttpResponseMessage httpResponse = await backOfficehttpClient.PostAsync($"{pwfBackOfficePath}{"Call"}", contentPost);
        
        if (!httpResponse.IsSuccessStatusCode)
        {
            return (default, true);
        }

        string asyncResponse = await httpResponse.Content.ReadAsStringAsync();
        var pwfResponse = JsonConvert.DeserializeObject<PwfResponse>(asyncResponse);

        if (pwfRequest.IsMutualAuthenticationRequired && pwfResponse?.CipherText != null)
        {
            var verifySign = SecurityHelpers.VerifySign(pwfResponse.CipherText, pwfResponse.VerificationSignature);
            if (!verifySign)
            {
                return (default, true);
            }

            var aesToken = SecurityHelpers.RSADecryption(pwfResponse.SecretKey);
            var clearText = SecurityHelpers.AESDecryption(pwfResponse.CipherText, aesToken);
            byte[] data = Convert.FromBase64String(clearText);
            string jsonContent = Encoding.UTF8.GetString(data);

            var model = JsonConvert.DeserializeObject<TResponse>(jsonContent);

            return (model, false);
        }

        return (default, true);
    }

    
    private static HttpClient GetHttpClient(string serverIp)
    {
        if (httpClient == null)
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(serverIp),
                Timeout = TimeSpan.FromSeconds(5)
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        return httpClient;
    }
    
    private static HttpClient GetBackOfficeHttpClient(string serverIp)
    {
        if (backOfficehttpClient == null)
        {
            backOfficehttpClient = new HttpClient
            {
                BaseAddress = new Uri(serverIp),
                Timeout = TimeSpan.FromSeconds(5)
            };
            backOfficehttpClient.DefaultRequestHeaders.Accept.Clear();
            backOfficehttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        return backOfficehttpClient;
    }
}