using System;
using CommandSystem;

namespace TournamentPlugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class TestEvents : ICommand
    {
        public string Command { get; } = "te";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "No.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Plugin.Instance.ZMethods.RegisterEvents();
            response = "k";
            return true;
        }
    }
}