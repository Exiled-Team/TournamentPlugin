namespace TournamentPlugin.Configs
{
    using UnityEngine;

    public class TeleporterLink
    {
        public Vector3 Position { get; set; }
        public Vector3 Destination { get; set; }

        public void Deconstruct(out Vector3 pos, out Vector3 dest)
        {
            pos = Position;
            dest = Destination;
        }
    }
}