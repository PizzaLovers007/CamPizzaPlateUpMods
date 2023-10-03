using System.Reflection;
using HarmonyLib;
using Kitchen;
using PlateUpShowUpgrade.Core;

namespace PlateUpShowUpgrade.Patches
{
    public class FixCustomerSpawnsPatch
    {
        private static int newGroupCount;
        private static int groupHandleArriveCount;

        //[HarmonyPatch(typeof(CreateCustomerSchedule), "AddGroup")]
        //[HarmonyPostfix]
        //public static void AddGroup(CustomerType type, int group_size, float time, bool is_special) {
        //    ModLogger.Log(
        //        $"CreateCustomerSchedule.AddGroup Postfix\n" +
        //        $"type={type} group_size={group_size} time={time} is_special={is_special}");
        //}

        [HarmonyPatch]
        public class LogCreateCustomerGroupPatch
        {
            [HarmonyPatch(typeof(CreateCustomerGroup), "NewGroup")]
            [HarmonyPostfix]
            public static void NewGroup() {
                newGroupCount++;
                ModLogger.Log(
                    $"CreateCustomerGroup.NewGroup Postfix count={newGroupCount}");
            }
        }

        //private static readonly MethodInfo BoundsGetter =
        //    AccessTools.PropertyGetter(typeof(CreateCustomerGroup), "Bounds");

        //[HarmonyPatch(typeof(CreateCustomerGroup), "NewCustomer")]
        //[HarmonyPostfix]
        //public static void NewCustomer(CreateCustomerGroup __instance, ref Entity __result) {
        //    ModLogger.Log($"CreateCustomerGroup.NewCustomer Postfix");

        //    Bounds bounds = (Bounds)BoundsGetter.Invoke(__instance, new object[] { });
        //    __instance.EntityManager.SetComponentData<CPosition>(
        //        __result, new Vector3(bounds.min.x - 10, 0, -6));
        //}

        [HarmonyPatch]
        public class LogGroupHandleArrivePatch
        {
            public static MethodBase TargetMethod() {
                var lambdaJobType = AccessTools.Inner(typeof(GroupHandleArrive), "<>c__DisplayClass_OnUpdate_LambdaJob0");
                return AccessTools.Method(lambdaJobType, "OriginalLambdaBody");
            }

            public static void Postfix() {
                groupHandleArriveCount++;
                ModLogger.Log(
                    $"GroupHandleArrive.LambdaJob.OriginalLambdaBody Postfix count={groupHandleArriveCount}");
            }
        }
    }
}
