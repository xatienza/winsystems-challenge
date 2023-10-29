namespace WinSystems.Challenge.Core.Services
{
	using Dto.WinSysAPI.Responses;
	
	public interface IWinSysApiService
	{
		bool ServiceAvailability();
		Task<string> GetToken(string email);
		Task<BlocksReponseDto> GetBlocks();
		Task<bool> CheckOrder(string[] blocks2Order);
		Task<bool> FinallyCheck(string encoded);
	}
}
