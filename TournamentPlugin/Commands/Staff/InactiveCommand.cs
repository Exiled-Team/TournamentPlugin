namespace TournamentPlugin.Commands.Staff
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class InactiveCommand : ICommand
    {
        public string Command { get; } = "inactive";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Deactivates staff mode.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (!player.RemoteAdminAccess)
            {
                response = "Permission denied.";
                return false;
            }

            player.Role = RoleType.Spectator;
            player.IsOverwatchEnabled = true;
            response = "Staff mode disabled, you are now in overwatch.";
            return true;
        }
    }
}