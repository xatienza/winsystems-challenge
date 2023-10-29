namespace WinSystems.Challenge.Core;

using Dto.WinSysAPI.Responses;
using Entities;

public static class Extensions
{
    internal static ChallengeEntity ToChallengeEntity(BlocksReponseDto dto)
        => new ChallengeEntity(dto.Data, dto.Length / dto.ChunkSize);
}