namespace EStoreWebApi.Features.Accounts.Responses;

public class LogoutResponse
{
	public bool LogoutSuccess { get; set; }

	public LogoutResponse(bool success)
	{
		LogoutSuccess = success;
	}
}

