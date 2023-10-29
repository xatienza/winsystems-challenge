using RestEase;

namespace WinSystems.Challenge.Core.Integrations;

using Dto.WinSysAPI.Requests;
using Dto.WinSysAPI.Responses;

[Header("User-Agent", "xatienza-challenge")]
[Header("Cache-Control", "no-cache")]
public interface IWinSysApi
{
    [Header("Authorization")]
    string AuthToken
    {
        get;
        set;
    }

    [Post("api/token")]
    Task<TokenResponseDto> GetToken([Body] TokenRequestDto credentials);
    
    [Get("api/v1/blocks")]
    Task<BlocksReponseDto> GetBlocks();
    
    [Post("api/v1/check")]
    Task<CheckBlocksReponseDto> CheckBlocks([Body] CheckBlocksRequestsDto blocks2Check);
    
    [Post("api/v1/check")]
    Task<CheckBlocksReponseDto> FinalCheck([Body] FinalCheckRequestsDto finalCheck);
}