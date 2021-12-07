namespace TournamentPlugin
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Features;
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

        public void OnEndingRound(EndingRoundEventArgs ev)
        {
            foreach (KeyValuePair<RoleType, List<Player>> activeTeam in _plugin.ActivePlayers.ToArray())
            {
                if (activeTeam.Value.All(p => p.Role == RoleType.Spectator))
                    _plugin.ActivePlayers.Remove(activeTeam.Key);
            }

            if (_plugin.ActivePlayers.Count == 1)
            {
                KeyValuePair<RoleType, List<Player>> winningTeam = _plugin.ActivePlayers.ElementAt(0);
                string winnerNames = string.Empty;
                foreach (Player player in winningTeam.Value)
                    winnerNames += $"{player.Nickname}\n";
                Map.Broadcast(new Broadcast($"The {winningTeam.Key} team has won this round! Winners: {winnerNames}", 30), true);
                _plugin.ActivePlayers.Clear();
            }
        }
    }
}