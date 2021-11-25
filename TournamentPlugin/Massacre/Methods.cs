namespace TournamentPlugin.Massacre
{
    public class Methods
    {
        private readonly Plugin _plugin;
        public Methods(Plugin plugin) => this._plugin = plugin;

        private EventHandlers EventHandlers { get; set; }

        public void StartMatch()
        {
            if (EventHandlers != null)
                UnregisterEvents();

            RegisterEvents();
        }

        public void RegisterEvents()
        {
            EventHandlers = new EventHandlers(_plugin);
            Exiled.Events.Handlers.Player.Died += EventHandlers.OnPlayerDied;
        }

        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.Died -= EventHandlers.OnPlayerDied;
            EventHandlers = null;
        }
    }
}