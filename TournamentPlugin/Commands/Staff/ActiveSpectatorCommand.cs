namespace TournamentPlugin.Commands.Staff
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ActivateCommand : ICommand
    {
        public string Command { get; } = "activespectate";
        public string[] Aliases { get; } = { "as" };
        public string Description { get; } = "Activates staff spectator mode.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (!player.RemoteAdminAccess)
            {
                response = "Permission denied.";
                return false;
            }

            player.IsOverwatchEnabled = false;
            player.IsGodModeEnabled = true;
            player.Role = RoleType.Tutorial;
            player.NoClipEnabled = true;
            player.IsInvisible = true;
            response = "Staff mode activated.";
            return true;
        }
    }
}