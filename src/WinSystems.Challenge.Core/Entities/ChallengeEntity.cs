namespace WinSystems.Challenge.Core.Entities;

public class ChallengeEntity
{
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public string[] Blocks { get; set; }
    public int Blocks2Order { get; set; }
    public int OrderedBlocks { get; set; }
    public string Encode { get; private set; }
    
    public StatsEntity Stats { get; internal set; } 

    public const int ApiRequestLimits = 1000;

    public ChallengeEntity(string[] blocks, int blocks2Order)
    {
        Blocks = blocks;
        Blocks2Order = blocks2Order;
        Encode = String.Empty;

        Stats = new StatsEntity();
        
        SetFirstBlock();
    }

    private void SetFirstBlock()
    {
        SetOrderedBlock(Blocks[0]);

        OrderedBlocks++;
    }
    
    public void SetOrderedBlock(string block)
    {
        Encode += block;
    }

    public void Start()
    {
        StartTime = DateTime.Now;
    }

    public void Stop()
    {
        EndTime = DateTime.Now;
    }
}