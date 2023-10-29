namespace WinSystems.Challenge.Core;

using Entities;
using Services;

public class Main
{
    private readonly IWinSysApiService _service;

    public Main()
    {
        _service = new WinSysApiService();
    }

    private async Task CheckFinalResult(string encode)
    {
        await Task.Delay(ChallengeEntity.ApiRequestLimits);
        
        var result = await _service.FinallyCheck(encode);

        if (!result)
            throw new HttpRequestException("Order method was wrong");
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

                var result = await _service.CheckOrder(new[] { refererBlock, block2Check });

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

                await Task.Delay(ChallengeEntity.ApiRequestLimits);
            }

            SetLastElement(challenge);

            await CheckFinalResult(challenge.Encode);
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

    
    public async Task<string[]> Check(string email = "", string token = "")
    {
        if (string.IsNullOrEmpty(email))
            throw new ArgumentException($"email cannot be null");
        
        if (string.IsNullOrEmpty(token))
            await SignIn(email);
        
        return await DoWorkAsync(await GetChallenge());
    }
}
