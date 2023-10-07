using System.Linq;
using HarmonyLib;
using KitchenData;
using static KitchenData.Appliance;

namespace CamPizza.ShowUpgrade.Patches
{
    [HarmonyPatch]
    public class NextUpgradeSectionPatch
    {
        [HarmonyPatch(typeof(Appliance), "Localise")]
        [HarmonyPostfix]
        public static void AddUpgradeInfo(Appliance __instance) {
            if (!__instance.HasUpgrades) {
                return;
            }
            __instance.Sections.Add(new Section {
                Title = "Next Upgrade",
                Description =
                    string.Join(", ", __instance.Upgrades.Select(app => app.Name).ToList()),
                RangeDescription = ""
            });
        }
    }
}
