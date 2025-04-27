using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Vitavy.Portal.Application.Requests.Commands;
using Vitavy.Portal.Blazor.Models;

namespace Vitavy.Portal.Blazor.Components.Pages;

public partial class Pilot(ISnackbar snackbar, IMediator mediator, IMapper mapper) : ComponentBase
{
    PilotModel _model = new PilotModel();
    MudForm form;
    private CancellationTokenSource cts = new CancellationTokenSource();
    
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
    
    private async Task<IEnumerable<string>> Search(string value, CancellationToken token)
    {
        // on simule une recherche de villes depuis une API publique.
        await Task.Delay(5, token);
    
        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return cities;
    
        return cities.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
    
    private async Task Submit(CancellationToken token)
    {
        await form.Validate();
    
        if (form.IsValid)
        {
            var command = mapper.Map<LaunchPilotActionCommand>(_model);
            var pilotActionTask = mediator.Send(command, token);
            snackbar.Add("Submitted!");
            var result = await pilotActionTask;
        }
    }

    public void Dispose()
    {
        cts.Cancel();
        form.Dispose();
        snackbar.Dispose();
        cts.Dispose();
    }
}