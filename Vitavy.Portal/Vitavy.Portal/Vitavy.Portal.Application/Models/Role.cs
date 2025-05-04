namespace Vitavy.Portal.Application.Models;

public class Role
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}