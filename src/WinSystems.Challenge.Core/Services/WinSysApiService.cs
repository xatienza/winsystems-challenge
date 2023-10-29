using RestEase;

namespace WinSystems.Challenge.Core.Services;

using Dto.WinSysAPI.Requests;
using Dto.WinSysAPI.Responses;
using Integrations;

public class WinSysApiService: IWinSysApiService
{
    private const int Blocks2CheckLength = 2;

    private IWinSysApi _api;
    public WinSysApiService()
    {
        _api = RestClient.For<IWinSysApi>("https://devchallenge.winsysgroup.com");
    }
    
    public bool ServiceAvailability()
    {
        throw new NotImplementedException();
    }
    
    public async Task<string> GetToken(string email)
    {
        if (string.IsNullOrEmpty(email))
            throw new ArgumentNullException($"email cannot be null or empty");

        var response = await _api.GetToken(new TokenRequestDto{Email = email});
        if (response is null)
            throw new NullReferenceException("Get Token Response cannot be null");

        _api.AuthToken = string.Format("Bearer " + response.Token);

        return _api.AuthToken;
    }

    public async Task<BlocksReponseDto> GetBlocks()
    {
        if (string.IsNullOrEmpty(_api.AuthToken))
            throw new ArgumentNullException($"token cannot be null or empty");

        var blocks = await _api.GetBlocks();
        if (blocks.Data is null || blocks.Data.Length == 0)
            throw new NullReferenceException("Blocks array is null");

        return blocks;
    }

    public async Task<bool> CheckOrder(string[] blocks2Order)
    {
        if (blocks2Order is null || blocks2Order.Length != Blocks2CheckLength)
            throw new ArgumentNullException($"blocks to order are null or length is incorrect");

        var result =  await _api.CheckBlocks(new CheckBlocksRequestsDto { Blocks = blocks2Order });

        return result.Message;
    }

    public async Task<bool> FinallyCheck(string encode)
    {
        if (string.IsNullOrEmpty(encode))
            throw new ArgumentNullException($"encoded cannot be null or empty");

        var result = await _api.FinalCheck(new FinalCheckRequestsDto { Encode = encode});

        return result.Message;
    }
}