namespace TournamentPlugin.Massacre
{
    using Exiled.Events.EventArgs;

    public class EventHandlers
    {
        private readonly Plugin _plugin;
        public EventHandlers(Plugin plugin) => this._plugin = plugin;

        public void OnPlayerDied(DiedEventArgs ev)
        {
            
        }
    }
}