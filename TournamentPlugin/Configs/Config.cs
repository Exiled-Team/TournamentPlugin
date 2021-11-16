namespace TournamentPlugin.Configs
{
    using System.ComponentModel;
    using Exiled.API.Interfaces;

    public class Config : IConfig
    {
        [Description("Whether or not this plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Whether debug messages should be shown.")]
        public bool Debug { get; set; } = false;

        [Description("Zombieland settings")] 
        public ZombielandConfig Zombieland { get; set; } = new ZombielandConfig();
    }
}