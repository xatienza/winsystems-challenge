namespace WinSystems.Challenge.Core;

using Entities;
using Services;

public class Main
{
    #region Attributes
    private readonly IWinSysApiService _service;
    #endregion
    
    #region Ctor
    public Main()
    {
        _service = new WinSysApiService();
    }
    #endregion

    private async Task CheckFinalResult(ChallengeEntity challenge)
    {
        await Task.Delay(ChallengeEntity.ApiRequestLimits);
        
        var result = await _service.FinallyCheck(challenge.Encode);
        
        await Task.Delay(ChallengeEntity.ApiRequestLimits);

        if (!result)
            throw new HttpRequestException("Order method was wrong");
    }

    private async Task<bool> CheckBlocks(ChallengeEntity challenge, string firstBlock, string secondBlock)
    {
        var result = await _service.CheckOrder(new[] { firstBlock, secondBlock });
        
        challenge.Stats.IncBlocksCall();
        
        await Task.Delay(ChallengeEntity.ApiRequestLimits);

        return result;
    }

    private void SetLastElement(ChallengeEntity challenge)
    {
        if (challenge.OrderedBlocks == challenge.Blocks2Order - 1)
            challenge.SetOrderedBlock(challenge.Blocks[challenge.Blocks2Order - 1]);
    }
    
    private async Task<string[]>DoWorkAsync(ChallengeEntity challenge)
    {
        try
        {
            challenge.Start();

            int blockPosition = challenge.OrderedBlocks;
            while (challenge.OrderedBlocks < challenge.Blocks2Order - 1)
            {
                if (blockPosition > challenge.Blocks2Order)
                    break;

                var refererBlock = challenge.Blocks[challenge.OrderedBlocks - 1];
                var block2Check = challenge.Blocks[blockPosition];

                var result = await CheckBlocks(challenge,refererBlock, block2Check);

                if (result)
                {
                    challenge.SetOrderedBlock(block2Check);

                    if (blockPosition != challenge.OrderedBlocks)
                    {
                        var wildBlock = challenge.Blocks[challenge.OrderedBlocks];

                        challenge.Blocks[challenge.OrderedBlocks] = block2Check;
                        challenge.Blocks[blockPosition] = wildBlock;
                    }

                    blockPosition = challenge.OrderedBlocks++;
                }
                else
                    blockPosition++;
            }

            SetLastElement(challenge);

            await CheckFinalResult(challenge);
        }
        finally
        {
            challenge.Stop();
        }

        return challenge.Blocks;
    }

    private async Task<string> SignIn(string email)
    { 
        var token = await _service.GetToken(email);
        
        await Task.Delay(ChallengeEntity.ApiRequestLimits);
        
        return token;
    }

    private async Task<ChallengeEntity> GetChallenge()
    {
        var blocks = await _service.GetBlocks();
        if (blocks == null)
            throw new NullReferenceException("Something was wrong retrieving blocks");
        
        await Task.Delay(ChallengeEntity.ApiRequestLimits);

        return Extensions.ToChallengeEntity(blocks);
    }
    
    /// <summary>
    /// This method performs the challenge
    /// </summary>
    /// <param name="email">user email</param>
    /// <param name="token">external token</param>
    /// <returns>Ordered array of blocks</returns>
    /// <exception cref="ArgumentException">if email is null</exception>
    public async Task<string[]> Check(string email = "", string token = "")
    {
        // Email cannot be null
        if (string.IsNullOrEmpty(email))
            throw new ArgumentException($"email cannot be null");
        
        // Get JWT Auth
        if (string.IsNullOrEmpty(token))
            await SignIn(email);
        
        // Doing de challenge
        return await DoWorkAsync(await GetChallenge());
    }
}
