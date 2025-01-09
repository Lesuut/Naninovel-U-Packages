namespace Naninovel.U.SideTip
{
    /// <summary>
    /// A custom engine service used to tooltip control
    /// </summary>
    [InitializeAtRuntime] // makes the service auto-initialize with other built-in engine services
    public class TipManager : IEngineService<TipConfiguration>
    {
        public TipConfiguration Configuration { get; }

        public TipManager(TipConfiguration config,
            IResourceProviderManager providersManager, ILocalizationManager localizationManager)
        {
            Configuration = config;
        }

        public UniTask InitializeServiceAsync()
        {
            // Invoked when the engine is initializing, after services required in the constructor are initialized;
            // it's safe to use the required services here (IResourceProviderManager in this case).

            return UniTask.CompletedTask;
        }

        public void ResetService()
        {
            // Invoked when resetting engine state (eg, loading a script or starting a new game).
        }

        public void DestroyService()
        {
            // Invoked when destroying the engine.
        }
    }
}
