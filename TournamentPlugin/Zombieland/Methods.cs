namespace TournamentPlugin.Zombieland
{
    using System;
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomItems.API.Features;
    using Exiled.Loader;
    using MEC;
    using TournamentPlugin.Configs;
    using TournamentPlugin.Zombieland.Components;
    using UnityEngine;

    public class Methods
    {
        private readonly Plugin _plugin;
        public Methods(Plugin plugin) => this._plugin = plugin;
        public EventHandlers EventHandlers { get; private set; }
        public CoroutineHandle Coroutine { get; private set; }

        public void StartMatch()
        {
            if (EventHandlers != null)
                UnRegisterEvents();
            RegisterEvents();
            Coroutine = Timing.RunCoroutine(SpawnItems());
            SpawnTeleporters();
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
                player.Health = _plugin.Config.Zombieland.HumanHealth;
                player.ClearInventory();

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