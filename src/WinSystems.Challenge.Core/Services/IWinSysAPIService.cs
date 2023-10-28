namespace WinSystems.Challenge.Core.Services
{
	public interface IWinSysApiService
	{
		bool ServiceAvailability();
		string GetToken(string email);
		string GetBlocks(string token);
		bool CheckOrder(string[] blocks2Order);
		bool FinallyCheck(string encoded);
	}
}
