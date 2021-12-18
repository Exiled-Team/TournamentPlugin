using System;
using CommandSystem;

namespace TournamentPlugin.Commands
{
    using Exiled.API.Features;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class TestSpawnZombie : ICommand
    {
        public string Command { get; } = "tz";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Yes";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (player != null)
                player.Role = RoleType.Scp0492;
            response = "Done";
            return true;
        }
    }
}