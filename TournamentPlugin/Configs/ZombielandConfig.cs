namespace TournamentPlugin.Configs
{
    using System.Collections.Generic;
    using Exiled.API.Enums;
    using Exiled.API.Features.Spawn;
    using UnityEngine;

    public class ZombielandConfig
    {
        public float ZombieHealth { get; set; } = 900f;
        public float HumanHealth { get; set; } = 100f;

        public List<string> HumanInventory { get; set; } = new List<string>
        {
            ItemType.GunCOM15.ToString(),
            ItemType.Flashlight.ToString(),
            ItemType.Medkit.ToString(),
            ItemType.Adrenaline.ToString(),
            ItemType.ArmorLight.ToString(),
        };

        public Dictionary<AmmoType, ushort> HumanAmmo { get; set; } = new Dictionary<AmmoType, ushort>
        {
            {
                AmmoType.Nato9, 50
            },
            {
                AmmoType.Nato556, 50
            },
            {
                AmmoType.Nato762, 50
            },
            {
                AmmoType.Ammo12Gauge, 25
            },
            {
                AmmoType.Ammo44Cal, 50
            }
        };

        public List<SpawnPoint> ItemSpawnPoints { get; set; } = new List<SpawnPoint>
        {
            new RoleSpawnPoint
            {
                Role = RoleType.ChaosConscript,
                Chance = 100,
            }
        };

        public List<string> SpawnableItems { get; set; } = new List<string>
        {
            ItemType.Medkit.ToString(),
            ItemType.Adrenaline.ToString(),
        };

        public float ItemSpawnInterval { get; set; } = 60f;

        public List<TeleporterLink> TeleporterLinks { get; set; } = new List<TeleporterLink>();
    }
}