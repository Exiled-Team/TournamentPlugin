namespace TournamentPlugin.Commands.Staff
{
    using System;
    using CommandSystem;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class StaffCommand : ParentCommand
    {
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Please use the active/inactive subcommands.";
            return false;
        }

        public override string Command { get; } = "tournystaff";
        public override string[] Aliases { get; } = { "tstaff" };
        public override string Description { get; } = "Does tournament staff stuff.";

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new ActivateCommand());
            RegisterCommand(new InactiveCommand());
        }
    }
}