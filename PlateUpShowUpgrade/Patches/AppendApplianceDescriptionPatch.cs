using HarmonyLib;
using KitchenData;

namespace PlateUpShowUpgrade.Patches
{
    [HarmonyPatch]
    public class AppendApplianceDescriptionPatch
    {
        [HarmonyPatch(typeof(ApplianceInfo), "Import")]
        [HarmonyPostfix]
        public static void Import(ApplianceInfo __instance) {
            __instance.Description += "ASDF";
        }
    }
}
