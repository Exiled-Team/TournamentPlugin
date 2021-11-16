using System;
using Exiled.API.Features;
using MapEvents = Exiled.Events.Handlers.Map;
using PlayerEvents = Exiled.Events.Handlers.Player;
using Scp049Events = Exiled.Events.Handlers.Scp049;
using Scp079Events = Exiled.Events.Handlers.Scp079;
using Scp096Events = Exiled.Events.Handlers.Scp096;
using Scp106Events = Exiled.Events.Handlers.Scp106;
using Scp914Events = Exiled.Events.Handlers.Scp914;
using ServerEvents = Exiled.Events.Handlers.Server;
using WarheadEvents = Exiled.Events.Handlers.Warhead;

namespace TournamentPlugin
{
    using System.Collections.Generic;
    using TournamentPlugin.Configs;

    public class Plugin : Plugin<Config>
    {
        public override string Author { get; } = "Joker119";
        public override string Name { get; } = "TournamentPlugin";
        public override string Prefix { get; } = "TournamentPlugin";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 11, 1);

        public Methods Methods { get; private set; }
        public EventHandlers EventHandlers { get; private set; }

        public List<string> StaffUserIds { get; } = new List<string>();
        public override void OnEnabled()
        {
            EventHandlers = new EventHandlers(this);
            Methods = new Methods(this);
            Exiled.Events.Handlers.Server.RespawningTeam += EventHandlers.OnRespawningTeam;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            EventHandlers = null;
            Methods = null;

            base.OnDisabled();
        }
    }
}