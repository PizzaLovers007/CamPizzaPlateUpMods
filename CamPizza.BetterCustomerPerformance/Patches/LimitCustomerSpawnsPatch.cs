using System;
using System.Reflection;
using HarmonyLib;
using Kitchen;
using KitchenData;
using Unity.Collections;
using Unity.Entities;

namespace CamPizza.BetterCustomerPerformance.Patches
{
    [HarmonyPatch]
    public class LimitCustomerSpawnsPatch
    {
        private static readonly MethodInfo GetEntityQueryMethod =
            AccessTools.Method(
                typeof(CreateCustomerGroup),
                "GetEntityQuery",
                new Type[] { typeof(ComponentType).MakeArrayType() });

        private static readonly MethodInfo RequireMethod =
            AccessTools.FirstMethod(
                    typeof(GenericSystemBase),
                    info => info.Name == "Require"
                        && info.GetParameters().Length == 2
                        && info.GetGenericArguments().Length == 1
                        && info.GetParameters()[1].ParameterType.GetElementType() == info.GetGenericArguments()[0]
                    )
                .MakeGenericMethod(typeof(CCustomerType));

        private static readonly MethodInfo NewGroupMethod =
            AccessTools.Method(typeof(CreateCustomerGroup), "NewGroup");

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
        public static bool OnUpdate(
                CreateCustomerGroup __instance,
                EntityQuery ___ScheduledCustomers,
                EntityQuery ____SingletonEntityQuery_STime_0) {
            STime singleton = ____SingletonEntityQuery_STime_0.GetSingleton<STime>();
            using NativeArray<Entity> nativeArray =
                ___ScheduledCustomers.ToEntityArray(Allocator.Temp);
            using NativeArray<CScheduledCustomer> nativeArray2 =
                ___ScheduledCustomers.ToComponentDataArray<CScheduledCustomer>(Allocator.Temp);

            int queueSize = Queuers.CalculateEntityCount();
            int groupsToSpawn = 30 - queueSize;
            for (int i = 0; i < nativeArray.Length && groupsToSpawn > 0; i++) {
                Entity entity = nativeArray[i];
                CScheduledCustomer cScheduledCustomer = nativeArray2[i];
                if (singleton.TimeOfDayUnbounded > cScheduledCustomer.TimeOfDay) {
                    object[] requireParams = new object[] { entity, null };
                    bool requireResult = (bool)RequireMethod.Invoke(__instance, requireParams);
                    CCustomerType comp = (CCustomerType)requireParams[1];
                    int id = requireResult ? comp.Type : 0;
                    if (GameData.Main.TryGet<CustomerType>(id, out var output)) {
                        NewGroupMethod.Invoke(
                            __instance,
                            new object[] {
                                output,
                                cScheduledCustomer.GroupSize,
                                cScheduledCustomer.IsCat });
                    }
                    __instance.EntityManager.DestroyEntity(entity);
                    groupsToSpawn--;
                }
            }

            return false;
        }
    }
}
