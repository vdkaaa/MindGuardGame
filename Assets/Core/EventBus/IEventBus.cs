namespace MindGuard.Core.EventBus
{
    using System;

    /// <summary>
    /// Interface para un sistema de publicación/suscripción de eventos.
    /// Soporta cualquier tipo de evento: structs o records (recomendado por .cursorrules).
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Suscribe un handler a eventos de tipo T.
        /// </summary>
        /// <typeparam name="T">El tipo de evento</typeparam>
        /// <param name="handler">El handler a ejecutar cuando se publique el evento</param>
        void Subscribe<T>(Action<T> handler);

        /// <summary>
        /// Desuscribe un handler de eventos de tipo T.
        /// </summary>
        /// <typeparam name="T">El tipo de evento</typeparam>
        /// <param name="handler">El handler a remover</param>
        void Unsubscribe<T>(Action<T> handler);

        /// <summary>
        /// Publica un evento a todos los handlers suscritos.
        /// </summary>
        /// <typeparam name="T">El tipo de evento</typeparam>
        /// <param name="eventData">Los datos del evento a publicar</param>
        void Publish<T>(T eventData);
    }
}
