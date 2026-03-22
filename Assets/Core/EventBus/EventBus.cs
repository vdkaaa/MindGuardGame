namespace MindGuard.Core.EventBus
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implementación de IEventBus usando un diccionario interno para almacenar handlers.
    /// </summary>
    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, Delegate> _handlers = new();

        /// <summary>
        /// Suscribe un handler a eventos de tipo T.
        /// Si el handler ya estaba suscrito, se agregará nuevamente.
        /// </summary>
        public void Subscribe<T>(Action<T> handler)
        {
            if (handler == null)
                return;

            var type = typeof(T);

            if (_handlers.TryGetValue(type, out var existingDelegate))
            {
                _handlers[type] = Delegate.Combine(existingDelegate, handler);
            }
            else
            {
                _handlers[type] = handler;
            }
        }

        /// <summary>
        /// Desuscribe un handler de eventos de tipo T.
        /// Si el handler no estaba suscrito, no hace nada.
        /// </summary>
        public void Unsubscribe<T>(Action<T> handler)
        {
            if (handler == null)
                return;

            var type = typeof(T);

            if (_handlers.TryGetValue(type, out var existingDelegate))
            {
                var newDelegate = Delegate.Remove(existingDelegate, handler);

                if (newDelegate == null)
                {
                    _handlers.Remove(type);
                }
                else
                {
                    _handlers[type] = newDelegate;
                }
            }
        }

        /// <summary>
        /// Publica un evento a todos los handlers suscritos de tipo T.
        /// Si no hay handlers suscritos, no hace nada y no lanza excepciones.
        /// </summary>
        public void Publish<T>(T eventData)
        {
            var type = typeof(T);

            if (_handlers.TryGetValue(type, out var handler))
            {
                var action = handler as Action<T>;
                action?.Invoke(eventData);
            }
        }
    }
}
