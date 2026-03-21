namespace MindGuard.Core.ServiceLocator
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Localizador de servicios que permite registrar y obtener instancias de servicios.
    /// </summary>
    public class ServiceLocator
    {
        private readonly Dictionary<Type, object> _services = new();

        /// <summary>
        /// Registra una instancia de servicio del tipo T.
        /// Si el tipo ya existe, lo sobrescribe.
        /// </summary>
        /// <typeparam name="T">El tipo del servicio a registrar</typeparam>
        /// <param name="instance">La instancia del servicio</param>
        public void Register<T>(T instance)
        {
            if (instance == null)
                return;

            var type = typeof(T);
            _services[type] = instance;
        }

        /// <summary>
        /// Obtiene una instancia de servicio del tipo T.
        /// </summary>
        /// <typeparam name="T">El tipo del servicio a obtener</typeparam>
        /// <returns>La instancia del servicio registrado</returns>
        /// <exception cref="InvalidOperationException">Se lanza si el servicio no está registrado</exception>
        public T Get<T>()
        {
            var type = typeof(T);

            if (_services.TryGetValue(type, out var service))
            {
                return (T)service;
            }

            throw new InvalidOperationException($"Servicio {type.Name} no registrado en ServiceLocator");
        }

        /// <summary>
        /// Verifica si un servicio del tipo T está registrado.
        /// </summary>
        /// <typeparam name="T">El tipo del servicio a verificar</typeparam>
        /// <returns>true si el servicio está registrado, false en caso contrario</returns>
        public bool HasService<T>()
        {
            var type = typeof(T);
            return _services.ContainsKey(type);
        }
    }
}
