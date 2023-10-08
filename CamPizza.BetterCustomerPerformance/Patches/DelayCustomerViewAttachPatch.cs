using CamPizza.BetterCustomerPerformance.Components;
using HarmonyLib;
using Kitchen;
using Unity.Entities;

namespace CamPizza.BetterCustomerPerformance.Patches
{
    [HarmonyPatch]
    public class DelayCustomerViewAttachPatch
    {
        [HarmonyPatch(typeof(CreateCustomerGroup), "NewCustomer")]
        [HarmonyPostfix]
        public static void NewCustomer(CreateCustomerGroup __instance, ref Entity __result, bool is_cat) {
            ModLogger.Log($"instance={__instance} result={__result} is_cat={is_cat}");
            __instance.EntityManager.RemoveComponent(__result, typeof(CRequiresView));
            __instance.EntityManager.AddComponentData(__result, new CFutureCustomerRequiresView {
                IsCat = is_cat,
            });
        }
    }
}
