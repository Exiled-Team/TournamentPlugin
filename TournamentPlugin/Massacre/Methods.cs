namespace TournamentPlugin.Massacre
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Features;
    using Exiled.Loader;
    using MEC;
    using NorthwoodLib.Pools;
    using UnityEngine;

    public class Methods
    {
        private readonly Plugin _plugin;
        public Methods(Plugin plugin) => this._plugin = plugin;

        public void StartMatch()
        {
            List<Player> players = ListPool<Player>.Shared.Rent(Player.List);
            SpawnHumans(ref players);
            Timing.CallDelayed(5f, () => SpawnScps(ref players));
            ListPool<Player>.Shared.Return(players);
        }

        private void SpawnHumans(ref List<Player> players)
        {
            for (int i = 0; i < 25; i++)
            {
                Player player = players[Loader.Random.Next(players.Count)];
                if (_plugin.StaffUserIds.Contains(player.UserId) || player.Role != RoleType.Spectator)
                {
                    i--;
                    players.Remove(player);
                    continue;
                }

                player.IsOverwatchEnabled = false;
                player.IsGodModeEnabled = false;
                player.SetRole(RoleType.ClassD);
                Timing.CallDelayed(0.15f, () =>
                {
                    player.ClearInventory();
                    player.Position = new Vector3(53f, 1020f, -44f);
                });
            }
        }

        private static void SpawnScps(ref List<Player> players)
        {
            for (int i = 0; i < 5; i++)
            {
                Player player = players[Loader.Random.Next(players.Count)];
                if (player.Role != RoleType.Spectator)
                {
                    i--;
                    players.Remove(player);
                    continue;
                }

                player.IsOverwatchEnabled = false;
                player.IsGodModeEnabled = false;
                player.SetRole(RoleType.Scp173);
                Timing.CallDelayed(0.15f, () =>
                {
                    player.Hurt("MassacreStart", player.MaxHealth - 173f);
                    player.Position = new Vector3(53f, 1020f, 44f);
                });
            }
        }
    }
}