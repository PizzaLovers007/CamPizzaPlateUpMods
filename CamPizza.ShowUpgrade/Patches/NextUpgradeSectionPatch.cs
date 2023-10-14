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

            if (__instance.Upgrades.Count == 1) {
                __instance.Sections.Add(new Section {
                    Title = "Next Upgrade",
                    Description = __instance.Upgrades.Single().Name,
                    RangeDescription = ""
                });
                return;
            }
            __instance.Sections.Add(new Section {
                Title = "Possible Upgrades",
                Description =
                    string.Join(", ", __instance.Upgrades.Select(app => app.Name).ToList()),
                RangeDescription = ""
            });
        }
    }
}
