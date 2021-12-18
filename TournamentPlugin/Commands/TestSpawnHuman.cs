using System;
using CommandSystem;

namespace TournamentPlugin.Commands
{
    using Exiled.API.Features;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class TestSpawnHuman : ICommand
    {
        public string Command { get; } = "th";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Yes.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (player != null)
                player.Role = RoleType.Scientist;
            response = "Done";
            return true;
        }
    }
}