using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;

namespace TournamentPlugin.Configs
{
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

        [Description("The amount of players in each team.")]
        public int Players { get; set; } = 20;
        
        [Description("The file path of a CSV file containing the bracket. The first (column names) row in this file is skipped. Each row in this bracket is a team, with the first column defining the match that the team will be playing in, and the second defining the team's number during that match. All subsequent columns define team user IDs, and any user IDs after the specififed number of team members become substitutes.")]
        public string Teams { get; set; } = Path.Combine(Paths.Configs, "zombieland.csv");

        private Dictionary<int, Dictionary<int, Tuple<string[], string[]>>> _parsedTeams = new Dictionary<int, Dictionary<int, Tuple<string[], string[]>>>();

        /// <summary>
        /// Parses the teams file. Does not get player objects.
        /// </summary>
        public void ParseTeams()
        {
            _parsedTeams.Clear();
            if (!File.Exists(Teams))
            {
                Log.Error("Failed loading CSV (1).");
                return;
            }

            string[] rows = File.ReadAllLines(Teams);
            if (rows.Length < 2)
            {
                Log.Error("Failed loading CSV (2).");
                return;
            }

            for (int i = 1; i < rows.Length; i++)
            {
                string[] columns = rows[i].Split(',');
                if (columns.Length < Players)
                {
                    Log.Error("There must be a full amount of players defined.");
                    return;
                }

                int idx = -1;
                int team = -1;
                List<string> playerIDs = new List<string>();
                List<string> subIDs = new List<string>();
                
                for (var y = 0; y < columns.Length; y++)
                {
                    switch (y)
                    {
                        case 0:
                            if (!int.TryParse(columns[0], out idx) || idx < 0)
                            {
                                Log.Error("Received a non-numerical index on line " + i + " column 0.");
                                return;
                            }

                            break;
                        case 1:
                            if (!int.TryParse(columns[1], out team) || team < 0)
                            {
                                Log.Error("Received a non-numerical team number on line " + i + " column 1.");
                                return;
                            }

                            break;
                        default:
                            if (y - 2 < Players)
                            {
                                //Remove authentication for better matching.
                                playerIDs.Add(columns[y].Split('@')[0].Trim().ToLower());
                            }
                            else
                            {
                                subIDs.Add(columns[y].Split('@')[0].Trim().ToLower());
                            }

                            break;
                    }
                }

                if (!_parsedTeams.ContainsKey(idx)) _parsedTeams[idx] = new Dictionary<int, Tuple<string[], string[]>>();
                _parsedTeams[idx][team] = new Tuple<string[], string[]>(playerIDs.ToArray(), subIDs.ToArray());
            }

            Log.Info("Successfully loaded teams.");
        }

        public Tuple<string[], string[]> GetTeam(int idx, int team)
        {
            if (!_parsedTeams.ContainsKey(idx))
            {
                throw new IndexOutOfRangeException("The specified index (match number) was not found.");
            }

            if (!_parsedTeams[idx].ContainsKey(team))
            {
                throw new IndexOutOfRangeException("The specified team number was not found.");
            }

            return _parsedTeams[idx][team];
        }
    }
}