using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using PowerFactor.Requests;
using PowerFactor.Responses;

namespace PowerFactor;

public class PWFClient : PWFClientBase
{
    public Task<(GenerateActivationOtpResponse? Data, bool IsError)> GenerateActivationOtp(GenerateActivationOtpRequest request)
    {
        string methodName = "GetActivationOtp";
        return PostAsync<GenerateActivationOtpResponse, GenerateActivationOtpRequest>(request, methodName);
    }
    
    public Task<(VerifyLoginOtpResponse? Data, bool IsError)> VerifyLoginOtp(VerifyLoginOtpRequest request)
    {
        string methodName = "VerifyLoginOtp";
        return PostAsync<VerifyLoginOtpResponse, VerifyLoginOtpRequest>(request, methodName);
    }
    
    public Task<(GetUserListResponse? Data, bool IsError)> GetUserList(GetUserListRequest request)
    {
        string methodName = "GetUserList";
        return PostAsyncOfBackOffice<GetUserListResponse, GetUserListRequest>(request, methodName);
    }
    
    public Task<(RemoveUserResponse? Data, bool IsError)> RemoveUser(RemoveUserRequest request)
    {
        string methodName = "RemoveUser";
        return PostAsyncOfBackOffice<RemoveUserResponse, RemoveUserRequest>(request, methodName);
    }
}