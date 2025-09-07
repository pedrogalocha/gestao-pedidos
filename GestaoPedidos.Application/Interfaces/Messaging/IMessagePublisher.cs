namespace GestaoPedidos.Application.Interfaces.Messaging;

public interface IMessagePublisher
{
    /// <summary>
    /// Publica uma mensagem em uma fila específica.
    /// </summary>
    /// <param name="message">O objeto da mensagem a ser enviado (será serializado para JSON).</param>
    /// <param name="queueName">O nome da fila de destino.</param>
    Task Publish(object message, string queueName);
}
