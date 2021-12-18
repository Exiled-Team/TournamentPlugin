using System;
using CommandSystem;

namespace TournamentPlugin.Commands
{
    using MEC;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class TestItems : ICommand
    {
        public string Command { get; } = "ti";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "items";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Timing.RunCoroutine(Plugin.Instance.ZMethods.SpawnItems());
            response = "Done";
            return true;
        }
    }
}