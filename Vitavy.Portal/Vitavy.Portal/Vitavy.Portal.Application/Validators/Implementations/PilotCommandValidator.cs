using FluentResults;
using Vitavy.Portal.Application.Features.Pilot;

namespace Vitavy.Portal.Application.Validators.Implementations;

public class PilotCommandValidator : IPilotCommandValidator
{
    public async Task<Result<LaunchPilotActionCommand>> Validate(LaunchPilotActionCommand? command)
    {
        if (command is null)
        {
            return Result.Fail("LaunchPilotActionCommand cannot be null"); 
        }

        if (string.IsNullOrWhiteSpace(command.Name))
        {
            return Result.Fail("LaunchPilotActionCommand.Name cannot be empty");
        }

        if (!cities.Contains(command.City))
        {
            return Result.Fail("LaunchPilotActionCommand.City is not valid");
        }

        if (command.Date > DateTime.Today.AddMonths(1))
        {
            return Result.Fail("LaunchPilotActionCommand.Date can't excess 1 month in the future");
        }
        return await Task.FromResult<Result<LaunchPilotActionCommand>>(Result.Ok());
    }
    
    private string[] cities =
    [
        "Tokyo", "Londres", "New York", "Sydney", "Cape Town",
        "Rio de Janeiro", "Moscou", "Pékin", "Mumbai", "Toronto",
        "Le Caire", "Nairobi", "Buenos Aires", "Berlin", "Singapour",
        "Mexico", "Séoul", "Dubaï", "Rome", "Melbourne",
        "Johannesburg", "Istanbul", "Bangkok", "Paris", "Kuala Lumpur",
        "Stockholm", "Madrid", "Lisbonne", "Dakar", "Montréal",
        "Shanghaï", "Hong Kong", "Jakarta", "Auckland", "Manille",
        "Lima", "Caracas", "Bogotá", "Kinshasa", "Hanoï",
        "Varsovie", "Oslo", "Helsinki", "Bruxelles", "Vienne",
        "Prague", "Budapest", "Athènes", "Tallinn", "Vilnius"
    ];
}