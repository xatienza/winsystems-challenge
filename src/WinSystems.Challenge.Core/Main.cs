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

    /// <summary>
    /// Check if the encode string is correct
    /// </summary>
    /// <param name="challenge">the challenge object</param>
    /// <exception cref="Exception">Encode string is wrong</exception>
    private async Task CheckFinalResult(ChallengeEntity challenge)
    {
        await Task.Delay(ChallengeEntity.ApiRequestLimits);
        
        var result = await _service.FinallyCheck(challenge.Encode);
        
        await Task.Delay(ChallengeEntity.ApiRequestLimits);

        if (!result)
            throw new Exception("Encode string is wrong");
    }

    /// <summary>
    /// Check if two blocks are sequential.
    ///
    /// WinSystem API do the hard work.
    /// </summary>
    /// <param name="challenge">The challenge object</param>
    /// <param name="firstBlock">First block of sequence</param>
    /// <param name="secondBlock">Second block of sequence</param>
    /// <returns>If the couple is sequential</returns>
    private async Task<bool> CheckBlocks(ChallengeEntity challenge, string firstBlock, string secondBlock)
    {
        // Check the order through API
        var result = await _service.CheckOrder(new[] { firstBlock, secondBlock });
        
        // Increment Block Check stats counter
        challenge.Stats.IncBlocksCall();
        
        await Task.Delay(ChallengeEntity.ApiRequestLimits);

        return result;
    }

    /// <summary>
    /// Sets manually the last element of blocks
    /// </summary>
    /// <param name="challenge">All the challenge object</param>
    private void SetLastElement(ChallengeEntity challenge)
    {
        if (challenge.OrderedBlocks == challenge.Blocks2Order - 1)
            challenge.SetOrderedBlock(challenge.Blocks[challenge.Blocks2Order - 1]);
    }
    
    private async Task<string[]>DoWorkAsync(ChallengeEntity challenge)
    {
        try
        {
            // Start the challenge
            challenge.Start();

            // set the position to check the sequence
            var blockPosition = challenge.OrderedBlocks;
            
            // order the blocks until all of them are ordered
            while (challenge.OrderedBlocks < challenge.Blocks2Order - 1)
            {
                // life jacket for blocks array
                if (blockPosition > challenge.Blocks2Order)
                    break;

                // get 2 blocks to check
                var refererBlock = challenge.Blocks[challenge.OrderedBlocks - 1];
                var block2Check = challenge.Blocks[blockPosition];

                // check the blocks
                var result = await CheckBlocks(challenge,refererBlock, block2Check);

                // if are sequential then...
                if (result)
                {
                    // set the new block to the encode array
                    challenge.SetOrderedBlock(block2Check);

                    // if the two blocks not are sequential then reorder the blocks array
                    if (blockPosition != challenge.OrderedBlocks)
                    {
                        var wildBlock = challenge.Blocks[challenge.OrderedBlocks];

                        challenge.Blocks[challenge.OrderedBlocks] = block2Check;
                        challenge.Blocks[blockPosition] = wildBlock;
                    }

                    // reindex the unordered position and set the new position to check
                    blockPosition = challenge.OrderedBlocks++;
                }
                else
                    blockPosition++;
            }

            // Set the last element manually
            SetLastElement(challenge);

            // Check the encode order
            await CheckFinalResult(challenge);
        }
        finally
        {
            // Stop the challenge
            challenge.Stop();
        }

        return challenge.Blocks;
    }

    /// <summary>
    /// Method in charge of obtaining the token from the API
    /// </summary>
    /// <param name="email">Authentication user</param>
    /// <returns>JWT auth</returns>
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
