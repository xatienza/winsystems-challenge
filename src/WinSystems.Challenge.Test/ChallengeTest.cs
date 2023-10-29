using WinSystems.Challenge.Core;
using WinSystems.Challenge.Core.Services;

namespace WinSystems.Challenge.Test;

public class ChallengeTest
{
    [Fact]
    public void TestGetTokenSuccess()
    {
        // Arrange
        var service = new WinSysApiService();

        // Act
        var token = Task.Run(async () => await service.GetToken($"xatienza@hotmail.com")).Result;

        // Assert
        Assert.NotEmpty(token);
    }
    
    [Fact]
    public void TestGetTokenFailed()
    {
        // Arrange
        var service = new WinSysApiService();

        // Act
        var exception = Record.ExceptionAsync(async () => await service.GetToken($"Xavier"));
        
        // Assert
        Assert.NotNull(exception);
    }
    
    [Fact]
    public void TestGetBlocksSuccess()
    {
        // Arrange
        var service = new WinSysApiService();

        // Act
        var token = Task.Run(async () => await service.GetToken($"xatienza@hotmail.com")).Result;
        var blocks = Task.Run(async () => await service.GetBlocks()).Result;
        
        // Assert
        Assert.NotNull(token);
        Assert.NotNull(blocks.Data);
    }
    
    [Fact]
    public void TestGetBlocksFailedNoToken()
    {
        // Arrange
        var service = new WinSysApiService();

        // Act
        var exception = Record.ExceptionAsync(async () => await service.GetBlocks());
        
        // Assert
        Assert.NotNull(exception);
    }
    
    [Fact]
    public void TestCheckOrderSuccess()
    {
        // Arrange
        var main = new Main();
        
        // Act
        var blocks = Task.Run(async () => await main.Check($"xatienza@hotmail.com")).Result;
        
        // Assert
        Assert.NotNull(blocks);
        Assert.NotEmpty(blocks);
    }
    
    [Fact]
    public void TestFailsNoEmail()
    {
        // Arrange
        var main = new Main();
        
        // Act
        var exception = Record.ExceptionAsync(() => main.Check());
        
        // Assert
        Assert.NotNull(exception);
    }
}