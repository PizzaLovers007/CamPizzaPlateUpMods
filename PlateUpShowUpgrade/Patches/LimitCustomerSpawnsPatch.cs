using System;
using System.Reflection;
using HarmonyLib;
using Kitchen;
using Unity.Entities;

namespace PlateUpShowUpgrade.Patches
{
    [HarmonyPatch]
    public class LimitCustomerSpawnsPatch
    {
        private static readonly MethodInfo GetEntityQueryMethod =
            AccessTools.Method(typeof(CreateCustomerGroup), "GetEntityQuery", new Type[] { typeof(ComponentType).MakeArrayType() });

        private static EntityQuery Queuers;

        [HarmonyPatch(typeof(CreateCustomerGroup), "Initialise")]
        [HarmonyPostfix]
        public static void Initialise(CreateCustomerGroup __instance) {
            Queuers =
                (EntityQuery)GetEntityQueryMethod.Invoke(
                    __instance,
                    new object[] { new ComponentType[] { typeof(CQueuePosition) } });
        }

        [HarmonyPatch(typeof(CreateCustomerGroup), "OnUpdate")]
        [HarmonyPrefix]
        public static bool OnUpdate() {
            int peopleInQueue = Queuers.CalculateEntityCount();
            return peopleInQueue < 30;
        }
    }
}
