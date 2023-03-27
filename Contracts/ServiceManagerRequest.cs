namespace Contracts;


public sealed record ServiceManagerRequest() 
{
    public string? Action { get; init; }
}