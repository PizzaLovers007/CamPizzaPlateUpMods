using HarmonyLib;
using Kitchen;
using Unity.Entities;
using UnityEngine;

namespace PlateUpShowUpgrade.Patches
{
    [HarmonyPatch]
    public class ShortPingPatch
    {
        [HarmonyPatch(typeof(MakePing), "Perform")]
        [HarmonyPrefix]
        public static bool Perform(CPlayerColour ___Colour, ref InteractionData data) {
            Entity entity = data.Context.CreateEntity();
            data.Context.Set(entity, new CRequiresView {
                Type = ViewType.Ping
            });
            data.Context.Set(entity, new CPosition {
                Position = data.Attempt.Location
            });
            data.Context.Set(entity, new CLifetime {
                //RemainingLife = 1f
                RemainingLife = 0.1f
            });
            data.Context.Set(entity, new CPlayerPing {
                //Colour = ___Colour.Color
                Colour = Color.white
            });
            return false;
        }
    }
}
