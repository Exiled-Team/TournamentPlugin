using System;
using CommandSystem;

namespace TournamentPlugin.Commands.Match
{
    using Exiled.API.Features;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class StartMatchCommand : ICommand
    {
        public string Command { get; } = "startmatch";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Starts a match.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (!player.RemoteAdminAccess)
            {
                response = "Permission denied.";
                
                return false;
            }

            if (arguments.Array == null || arguments.Array.Length < 4)
            {
                response = "Invalid number of arguments. Must define matchId, teamCount and gamemode.";
                
                return false;
            }

            if (!int.TryParse(arguments.Array[1], out int matchId))
            {
                response = $"Invalid matchID: {arguments.Array[1]}";

                return false;
            }

            if (!int.TryParse(arguments.Array[2], out int teamCount))
            {
                response = $"Invalid teamCount: {arguments.Array[2]}";

                return false;
            }

            switch (arguments.Array[3])
            {
                case "zombieland":
                {
                    Plugin.Instance.ZMethods.StartMatch(matchId, teamCount);
                    response = "Zombieland should be starting.";

                    return true;
                }
                case "massacre":
                {
                    Plugin.Instance.MMethods.StartMatch();
                    response = "Massacre should be starting.";

                    return true;
                }
                default:
                {
                    response = $"Unrecognized match type: {arguments.Array[3]}";

                    return false;
                }
            }
        }
    }
}