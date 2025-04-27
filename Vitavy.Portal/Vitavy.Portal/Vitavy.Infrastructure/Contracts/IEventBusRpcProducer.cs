namespace Vitavy.Infrastructure.Contracts;

public interface IEventBusRpcProducer<in TMessage, TResponse>
    where TMessage : class
    where TResponse : class
{
    public Task StartRpc(string producerId, CancellationToken stoppingToken);
    
    /// <summary>
    /// Encapsule un appel AR via EventHub (RabbitMQ) sous forme d'une simple méthode asynchrone
    /// </summary>
    /// <param name="message">Message à envoyer</param>
    /// <param name="routingKey">routing key pour atteindre la bonne cible via le topic</param>
    /// <param name="producerId">Id du producer à utiliser depuis la configuration</param>
    /// <param name="stoppingToken">Stopping token</param>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns>Reponse via la queue de retour, désérialisée en type TResponse</returns>
    Task<TResponse> PublishRpc(TMessage message, string routingKey, string producerId, CancellationToken stoppingToken);
}