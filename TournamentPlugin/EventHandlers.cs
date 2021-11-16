namespace TournamentPlugin
{
    using Exiled.Events.EventArgs;

    public class EventHandlers
    {
        private readonly Plugin _plugin;
        public EventHandlers(Plugin plugin) => this._plugin = plugin;

        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            ev.IsAllowed = false;
            ev.Players.Clear();
        }
    }
}