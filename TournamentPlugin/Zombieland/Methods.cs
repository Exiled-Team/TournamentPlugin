namespace TournamentPlugin.Zombieland
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomItems.API.Features;
    using Exiled.Loader;
    using MEC;
    using NorthwoodLib.Pools;
    using TournamentPlugin.Components;
    using TournamentPlugin.Configs;
    using UnityEngine;

    public class Methods
    {
        private readonly Plugin _plugin;
        public Methods(Plugin plugin) => this._plugin = plugin;
        public EventHandlers EventHandlers { get; private set; }
        public CoroutineHandle Coroutine { get; private set; }

        public void StartMatch(int matchIndex, int teamCount = 5)
        {
            if (EventHandlers != null)
                UnRegisterEvents();
            RegisterEvents();
            Coroutine = Timing.RunCoroutine(SpawnItems());
            _plugin.ActivePlayers.Clear();
            SpawnTeleporters();
            List<Player[]> teams = ListPool<Player[]>.Shared.Rent();
            for (int i = 0; i < teamCount; i++)
            {
                teams.Add(_plugin.Config.Zombieland.GetTeamMembers(matchIndex, i));
            }

            List<RoleType> unUsedRoles = ListPool<RoleType>.Shared.Rent();
            unUsedRoles.Add(RoleType.FacilityGuard);
            unUsedRoles.Add(RoleType.ClassD);
            unUsedRoles.Add(RoleType.NtfCaptain);
            unUsedRoles.Add(RoleType.ChaosRifleman);
            unUsedRoles.Add(RoleType.Scientist);
            
            foreach (Player[] team in teams)
            {
                RoleType type = unUsedRoles[Loader.Random.Next(unUsedRoles.Count)];
                
                if (!_plugin.ActivePlayers.ContainsKey(type))
                    _plugin.ActivePlayers[type] = new List<Player>();
                
                foreach (Player player in team)
                {
                    player.SetRole(type);
                    _plugin.ActivePlayers[type].Add(player);
                }
            }

            ListPool<RoleType>.Shared.Return(unUsedRoles);
        }

        public void RegisterEvents()
        {
            EventHandlers = new EventHandlers(_plugin, this);
            
            Exiled.Events.Handlers.Player.Died += EventHandlers.OnPlayerDied;
            Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.OnPlayerChangingRole;
        }

        public void UnRegisterEvents()
        {
            Exiled.Events.Handlers.Player.Died -= EventHandlers.OnPlayerDied;
            Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.OnPlayerChangingRole;

            EventHandlers = null;

            Timing.KillCoroutines(Coroutine);
        }

        public void SpawnTeleporters()
        {
            foreach ((Vector3 pos, Vector3 dest) in _plugin.Config.Zombieland.TeleporterLinks)
            {
                SpawnTeleporter(pos, dest);
                SpawnTeleporter(dest, pos);
            }
        }

        public void SpawnTeleporter(Vector3 pos, Vector3 dest)
        {
            Pickup teleporter = new Item(ItemType.Coin).Spawn(pos);
            TeleporterComponent comp = teleporter.Base.gameObject.AddComponent<TeleporterComponent>();
            comp.position = pos;
            comp.destination = dest;
            teleporter.Scale *= 5f;
            teleporter.Locked = true;
        }

        public IEnumerator<float> SpawnItems()
        {
            for (;;)
            {
                foreach (string itemName in _plugin.Config.Zombieland.SpawnableItems)
                {
                    foreach (SpawnPoint point in _plugin.Config.Zombieland.ItemSpawnPoints)
                        if (point.Chance <= Loader.Random.Next(100))
                        {
                            if (Enum.TryParse(itemName, true, out ItemType type))
                                new Item(type).Spawn(point.Position);
                            else if (CustomItem.TryGet(itemName, out CustomItem customItem))
                                customItem.Spawn(point.Position);
                            
                            break;
                        }
                }

                yield return Timing.WaitForSeconds(_plugin.Config.Zombieland.ItemSpawnInterval);
            }
        }

        public void RespawnZombie(Player player)
        {
            Timing.CallDelayed(player.RemoteAdminAccess ? 25f : 40f, () =>
            {
                player.Role = RoleType.Scp0492;
            });
        }
        
        public void SetupZombie(Player player)
        {
            Timing.CallDelayed(0.25f, () =>
            {
                player.Health = _plugin.Config.Zombieland.ZombieHealth;
            });
        }
        
        public void SetupHuman(Player player)
        {
            Timing.CallDelayed(0.25f, () =>
            {
                player.ClearInventory();
                player.Health = _plugin.Config.Zombieland.HumanHealth;

                foreach (string itemName in _plugin.Config.Zombieland.HumanInventory)
                {
                    if (Enum.TryParse(itemName, true, out ItemType itemType))
                        player.AddItem(itemType);
                    else if (CustomItem.TryGet(itemName, out CustomItem customItem))
                        customItem.Give(player);
                }

                foreach (KeyValuePair<AmmoType, ushort> ammo in _plugin.Config.Zombieland.HumanAmmo)
                    player.SetAmmo(ammo.Key, ammo.Value);
            });
        }
    }
}