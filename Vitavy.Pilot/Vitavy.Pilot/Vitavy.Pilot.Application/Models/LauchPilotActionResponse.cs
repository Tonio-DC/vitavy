namespace Vitavy.Pilot.Application.Models;

public record LauchPilotActionResponse(string Name, string City, DateTime Date, TimeSpan Time, decimal Temperature);