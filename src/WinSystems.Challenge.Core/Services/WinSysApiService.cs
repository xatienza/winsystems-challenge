using RestEase;

namespace WinSystems.Challenge.Core.Services;

public class WinSysApiService: IWinSysApiService
{
    private const int Blocks2CheckLength = 2;
    
    public bool ServiceAvailability()
    {
        throw new NotImplementedException();
    }
    
    public string GetToken(string email)
    {
        if (string.IsNullOrEmpty(email))
            throw new ArgumentNullException($"email cannot be null or empty");
        
        throw new NotImplementedException();
    }

    public string GetBlocks(string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new ArgumentNullException($"token cannot be null or empty");
        
        throw new NotImplementedException();
    }

    public bool CheckOrder(string[] blocks2Order)
    {
        if (blocks2Order is null || blocks2Order.Length != Blocks2CheckLength)
            throw new ArgumentNullException($"blocks to order are null or length is incorrect");
        
        
        
        throw new NotImplementedException();
    }

    public bool FinallyCheck(string encoded)
    {
        if (string.IsNullOrEmpty(encoded))
            throw new ArgumentNullException($"encoded cannot be null or empty");

        throw new NotImplementedException();
    }
}