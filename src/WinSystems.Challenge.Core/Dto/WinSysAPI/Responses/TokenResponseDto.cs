namespace WinSystems.Challenge.Core.Dto.WinSysAPI.Responses;

public class TokenResponseDto
{
    public string Token { get; set; }
    public string ExpirationInUnixTime { get; set; }
}