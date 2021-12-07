namespace TournamentPlugin.Components
{
    using System;
    using Exiled.API.Features;
    using MEC;
    using UnityEngine;

    public class TeleporterComponent : MonoBehaviour
    {
        public Vector3 position;
        public Vector3 destination;

        private void OnCollisionEnter(Collision other)
        {
            Player player = Player.Get(other.gameObject);
            if (player == null)
                return;

            Timing.CallDelayed(1.5f, () =>
            {
                if ((player.Position - position).sqrMagnitude < 9f)
                    player.Position = destination;
            });
        }
    }
}