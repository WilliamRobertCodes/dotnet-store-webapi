using System;
namespace EStoreWebApi.Responses.AccountResponses;

public class LogoutResponse
{
	public bool LogoutSuccess { get; set; }

	public LogoutResponse(bool success)
	{
		LogoutSuccess = success;
	}
}

