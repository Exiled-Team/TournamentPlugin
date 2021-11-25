namespace TournamentPlugin.Zombieland
{
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.Events.EventArgs;

    public class EventHandlers
    {
        private readonly Plugin _plugin;
        private readonly Methods _methods;

        public EventHandlers(Plugin plugin, Methods methods)
        {
            _plugin = plugin;
            _methods = methods;
        }

        public void OnPlayerDied(DiedEventArgs ev)
        {
            if (_plugin.StaffUserIds.Contains(ev.Target.UserId) || ev.Killer.IsHuman)
                _methods.RespawnZombie(ev.Target);
        }

        public void OnPlayerChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.NewRole.GetSide() == Side.Scp)
            {
                _methods.SetupZombie(ev.Player);
            }
            else if (ev.NewRole != RoleType.Spectator)
            {
                _methods.SetupHuman(ev.Player);
            }
        }
    }
}