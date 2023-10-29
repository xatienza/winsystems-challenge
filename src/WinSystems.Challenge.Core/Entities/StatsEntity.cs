namespace WinSystems.Challenge.Core.Entities;

public class StatsEntity
{
    public int CheckBlocksCalls { get; private set; }
    public int CheckEncodeCalls { get; private set; }

    public void IncBlocksCall()
        => CheckBlocksCalls++;

    public void IncCheckEncodeCall()
        => CheckEncodeCalls++;
}