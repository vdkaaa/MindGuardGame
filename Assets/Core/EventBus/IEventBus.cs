namespace MindGuard.Core.EventBus
{
    using System;

    /// <summary>
    /// Interface para un sistema de publicación/suscripción de eventos.
    /// Los eventos deben ser structs para garantizar seguridad de tipos.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Suscribe un handler a eventos de tipo T.
        /// </summary>
        /// <typeparam name="T">El tipo de evento (debe ser struct)</typeparam>
        /// <param name="handler">El handler a ejecutar cuando se publique el evento</param>
        void Subscribe<T>(Action<T> handler) where T : struct;

        /// <summary>
        /// Desuscribe un handler de eventos de tipo T.
        /// </summary>
        /// <typeparam name="T">El tipo de evento (debe ser struct)</typeparam>
        /// <param name="handler">El handler a remover</param>
        void Unsubscribe<T>(Action<T> handler) where T : struct;

        /// <summary>
        /// Publica un evento a todos los handlers suscritos.
        /// </summary>
        /// <typeparam name="T">El tipo de evento (debe ser struct)</typeparam>
        /// <param name="eventData">Los datos del evento a publicar</param>
        void Publish<T>(T eventData) where T : struct;
    }
}
