namespace MindGuard.Bootstrap
{
    using UnityEngine;
    using MindGuard.Core.EventBus;
    using MindGuard.Core.ServiceLocator;

    /// <summary>
    /// Bootstrapper del juego que inicializa todos los servicios base.
    /// Se ejecuta antes que cualquier otro script del juego.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class GameBootstrapper : MonoBehaviour
    {
        /// <summary>
        /// Acceso global al localizador de servicios.
        /// </summary>
        public static ServiceLocator Services { get; private set; }

        private void Awake()
        {
            // Crear instancias de los servicios base
            var eventBus = new EventBus();
            var serviceLocator = new ServiceLocator();

            // Registrar EventBus en el ServiceLocator
            serviceLocator.Register<IEventBus>(eventBus);

            // Asignar el ServiceLocator como acceso global
            Services = serviceLocator;

            // Marcar este GameObject para que persista entre escenas
            DontDestroyOnLoad(gameObject);

            // Log de confirmación
            Debug.Log("MindGuard Bootstrap OK");
        }
    }
}
