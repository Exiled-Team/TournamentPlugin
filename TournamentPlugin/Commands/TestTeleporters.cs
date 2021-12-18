using System;
using CommandSystem;

namespace TournamentPlugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class TestTeleporters : ICommand
    {
        public string Command { get; } = "ttp";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "No";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Plugin.Instance.ZMethods.SpawnTeleporters();
            response = "done";
            return true;
        }
    }
}